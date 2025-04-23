
using GungeonAPI;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FrostAndGunfireItems
{

    class Greed : PassiveItem
    {
		
		
		private static GameObject costVFX;
		private static GameObject costVFX2;
		private static string vfxName2 = "CostVFX2";
		private static string vfxName = "CostVFX";
		public List<GameObject> ItemsAlreadySelected { get; } = new List<GameObject>();

		public static void Init()
		{
			string name = "Greed";
			string resourcePath = "FrostAndGunfireItems/Resources/greed";
			GameObject gameObject = new GameObject(name);
			Greed item = gameObject.AddComponent<Greed>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "MONEY MONEY MONEY MONEY";
			string longDesc = "More money, more money, more money...at some costs.\n\nThe Swindler will do anything for a quick buck, no matter how much it hurts him.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.CanBeDropped = false;
			Hook shopHook = new Hook(
				typeof(ShopItemController).GetMethod("Interact", BindingFlags.Public | BindingFlags.Instance),
				typeof(Greed).GetMethod("CantUseGreedOn")
			);
			Hook chesthook = new Hook(
					typeof(Chest).GetMethod("Interact", BindingFlags.Public | BindingFlags.Instance),
					typeof(Greed).GetMethod("SpendMoney")
				);
			Hook npcLockHook = new Hook(
					typeof(DungeonDoorController).GetMethod("Interact", BindingFlags.Public | BindingFlags.Instance),
					typeof(Greed).GetMethod("SpendMoneyDoor")
				);
			Hook lockhook = new Hook(
					typeof(InteractableLock).GetMethod("Interact", BindingFlags.Public | BindingFlags.Instance),
					typeof(Greed).GetMethod("SpendMoneyLock")
				);
			Hook keyHook = new Hook(
				   typeof(KeyBulletPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
				   typeof(Greed).GetMethod("KeyHookMethod")
			   );
			BuildPrefab();
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage += SpawnMoney;
			player.OnNewFloorLoaded += FloorLoad;
			player.gameObject.AddComponent<DisplayCost>();
		}

		private void FloorLoad(PlayerController user)
		{
			user.CurrentRoom.Entered += RerollKeys;
			user.OnNewFloorLoaded -= FloorLoad;
		}

		private void RerollKeys(PlayerController user)
        {
			if(user.CurrentRoom.IsShop)
            {
				Tools.Print("helllo");
            }
        }

		private ShopItemController ReplaceShopItem(BaseShopController shopController, ShopItemController oldShopItemController, GameObject itemToAdd)
		{
			PickupObject component = itemToAdd.GetComponent<PickupObject>();
			List<ShopItemController> list = Tools.ReflectGetField<List<ShopItemController>>(typeof(BaseShopController), "m_itemControllers", shopController);
			List<GameObject> list2 = Tools.ReflectGetField<List<GameObject>>(typeof(BaseShopController), "m_shopItems", shopController);
			for (int i = 0; i < list2.Count; i++)
			{
				GameObject gameObject = list2[i];
				if (gameObject != null)
				{
					PickupObject component2 = gameObject.GetComponent<PickupObject>();
					if (component2 != null && component2 != oldShopItemController.item)
					{
						Transform parent = oldShopItemController.gameObject.transform.parent;
						if (parent != null && component.quality != PickupObject.ItemQuality.EXCLUDED && component.quality != PickupObject.ItemQuality.SPECIAL && component.PickupObjectId != 670)
						{
							list2[i] = itemToAdd;
							oldShopItemController.Initialize(component, shopController);
							return oldShopItemController;
						}
					}
				}
			}
			return null;
		}

		public static void BuildPrefab()
		{
			costVFX = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/vfx_stuff/cost_vfx", null, false);
			costVFX.name = vfxName;
			GameObject.DontDestroyOnLoad(costVFX);
			FakePrefab.MarkAsFakePrefab(costVFX);
			costVFX.SetActive(false);
			costVFX2 = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/vfx_stuff/cost_vfx2", null, false);
			costVFX2.name = vfxName2;
			GameObject.DontDestroyOnLoad(costVFX2);
			FakePrefab.MarkAsFakePrefab(costVFX2);
			costVFX2.SetActive(false);
		}

		private void SpawnMoney(float damage, bool fatal, HealthHaver enemyHealth)
		{
			float value = UnityEngine.Random.value;
			if ((double)value < 0.05)
			{
				if (enemyHealth.aiActor && enemyHealth && !enemyHealth.IsBoss && fatal && enemyHealth.aiActor.IsNormalEnemy && !enemyHealth.aiActor.IsFlying && enemyHealth.aiActor.CanDropCurrency)
				{

					PickupObject byId = PickupObjectDatabase.GetById(Gungeon.Game.Items["kp:pouch"].PickupObjectId);
					LootEngine.DoDefaultItemPoof(enemyHealth.aiActor.sprite.WorldCenter, false, false);
					GameObject gameObject = SpawnManager.SpawnDebris(byId.gameObject, enemyHealth.aiActor.sprite.WorldCenter, Quaternion.identity);
					PickupObject component = gameObject.GetOrAddComponent<PickupObject>();
					List<PickupObject> list = new List<PickupObject>();
					list.Add(component);
					PickupMover component2 = gameObject.GetComponent<PickupMover>();
					if (component2)
					{
						component2.enabled = false;
					}
					DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
					DebrisObject debrisObject = orAddComponent;
					debrisObject.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnGrounded, new Action<DebrisObject>(delegate (DebrisObject sourceDebris)
					{
						sourceDebris.GetComponent<CurrencyPickup>().PreventPickup = false;
						sourceDebris.OnGrounded = null;
					}));
					orAddComponent.shouldUseSRBMotion = true;
					orAddComponent.angularVelocity = 1f;
					orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
					orAddComponent.canRotate = false;
					orAddComponent.bounceCount = 1;
					GameManager.Instance.StartCoroutine(TimedDestroy(list));
				}
			}
		}

		private IEnumerator TimedDestroy(List<PickupObject> coins)
		{
			float elapsed = 0f;
			while (elapsed < BankBagItem.cachedCoinLifespan * 0.75f)
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
			float flickerTimer = 0f;
			while (elapsed < BankBagItem.cachedCoinLifespan * 0.75f)
			{
				elapsed += BraveTime.DeltaTime;
				flickerTimer += BraveTime.DeltaTime;
				for (int i = 0; i < coins.Count; i++)
				{
					if (coins[i] && coins[i].renderer)
					{
						bool enabled = flickerTimer % 0.2f > 0.15f;
						coins[i].renderer.enabled = enabled;
					}
				}
				yield return null;
			}
			for (int j = 0; j < coins.Count; j++)
			{
				if (coins[j])
				{
					LootEngine.DoDefaultItemPoof(coins[j].sprite.WorldCenter, false, false);
					UnityEngine.Object.Destroy(coins[j].gameObject);
				}
			}
			yield break;
		}

		public static void CantUseGreedOn(Action<ShopItemController, PlayerController> orig, ShopItemController self, PlayerController player)
		{
			BaseShopController shopController = Tools.ReflectGetField<BaseShopController>(typeof(ShopItemController), "m_baseParentShop", self);
			if (self.item.PickupObjectId == 67 && player.HasPickupID(Gungeon.Game.Items["kp:greed"].PickupObjectId))
			{
				Tools.PrintNoID("Hook is being read");
				AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", player.gameObject);
				shopController.NotifyFailedPurchase(self);
				return;
			}
			orig(self, player);
		}
		
		public static void SpendMoney(Action<Chest, PlayerController> orig, Chest self, PlayerController player)
		{
			if (self.IsLocked)
			{
				if (player.HasPickupID(Gungeon.Game.Items["kp:greed"].PickupObjectId) && !player.HasPickupID(166))
				{
					if (player.carriedConsumables.Currency >= 40 && !self.IsLockBroken)
					{
						player.carriedConsumables.Currency -= 40;
						self.ForceUnlock();
					}
				}
			}
			orig(self, player);
		}
		public static void SpendMoneyLock(Action<InteractableLock, PlayerController> orig, InteractableLock self, PlayerController player)
		{
			if (player.HasPickupID(Gungeon.Game.Items["kp:greed"].PickupObjectId) && !self.IsBusted && !self.HasBeenPicked && !player.HasPickupID(166))
			{
				if (player.carriedConsumables.Currency >= 20)
				{		
					player.carriedConsumables.Currency -= 20;
					self.ForceUnlock();
				}
			}
			orig(self, player);
		}
		public static void SpendMoneyDoor(Action<DungeonDoorController, PlayerController> orig, DungeonDoorController self, PlayerController player)
		{
			if (player.HasPickupID(Gungeon.Game.Items["kp:greed"].PickupObjectId) && !player.HasPickupID(166))
			{
				if (player.carriedConsumables.Currency >= 20)
				{
					player.carriedConsumables.Currency -= 20;
					self.Unlock();
				}
			}
			orig(self, player);
		}
		public static void KeyHookMethod(Action<KeyBulletPickup, PlayerController> orig, KeyBulletPickup self, PlayerController player)
		{
			orig(self, player);
			if (player.HasPickupID(Gungeon.Game.Items["kp:greed"].PickupObjectId))
			{
				if (!self.IsRatKey)
				{
					player.carriedConsumables.KeyBullets *= 0;
					player.carriedConsumables.Currency += 30;
				}
			}
		
		}

		protected override void OnDestroy()
		{
			Owner.OnAnyEnemyReceivedDamage -= SpawnMoney;
			base.OnDestroy();
		}

		private class DisplayCost : BraveBehaviour
		{
			List<Chest> encounteredChests = new List<Chest>();
			List<InteractableLock> encounterLocks = new List<InteractableLock>();
			List<DungeonDoorController> encounterNPCLocks = new List<DungeonDoorController>();
			PlayerController player;
			InteractableLock nearbylock;
			DungeonDoorController npcLock;
			Chest nearbyChest;
			Vector2 offset = new Vector2(0, .25f);
			void Start()
			{
				player = GetComponent<PlayerController>();
			}

			void FixedUpdate()
			{
				if (!player || player.CurrentRoom == null)
					return;
				IPlayerInteractable nearestInteractable = player.CurrentRoom.GetNearestInteractable(player.sprite.WorldCenter, 1f, player);
				if (nearestInteractable != null && nearestInteractable is Chest)
				{
					var chest = nearestInteractable as Chest;
					if (chest.IsLocked)
					{
						if (!encounteredChests.Contains(chest) && !chest.transform.Find(vfxName))
							InitializeChest(chest);
						else
							nearbyChest = chest;
					}
				}
				else
				{
					nearbyChest = null;
				}
				if (nearestInteractable != null && nearestInteractable is DungeonDoorController)
				{
					var dungeonDoorController = nearestInteractable as DungeonDoorController;
					if (dungeonDoorController != null && dungeonDoorController.Mode == DungeonDoorController.DungeonDoorMode.COMPLEX && dungeonDoorController.isLocked)
					{
						if (!encounterNPCLocks.Contains(dungeonDoorController) && !dungeonDoorController.transform.Find(vfxName2))
							InitializeNPCLock(dungeonDoorController);
						else
							npcLock = dungeonDoorController;
					}
				}
				else
				{
					npcLock = null;
				}
				if (nearestInteractable != null && nearestInteractable is InteractableLock)
				{
					var interactableLock = nearestInteractable as InteractableLock;
					if (interactableLock != null && interactableLock.IsLocked)
					{
						if (!encounterLocks.Contains(interactableLock) && !interactableLock.transform.Find(vfxName2))
							InitLock(interactableLock);
						else
							nearbylock = interactableLock;
					}
				}
				else
				{
					npcLock = null;
				}
				HandleLocks();
			}

			void HandleLocks()
			{
				foreach (var chest in encounteredChests)
				{
					if (!chest)
						continue;

					var fx = chest?.transform?.Find(vfxName)?.GetComponent<tk2dSprite>();

					if (!fx)
						continue;

					if (chest != nearbyChest)
						fx.scale = Vector3.Lerp(fx.scale, Vector3.zero, .25f);
					else
						fx.scale = Vector3.Lerp(fx.scale, Vector3.one, .25f);

					if (Vector3.Distance(fx.scale, Vector3.zero) < .01f)
						fx.scale = Vector3.zero;

					fx.PlaceAtPositionByAnchor(chest.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				}
				foreach (var m_lock in encounterLocks)
				{
					if (!m_lock)
						continue;

					var fx = m_lock?.transform?.Find(vfxName2)?.GetComponent<tk2dSprite>();

					if (!fx)
						continue;

					if (m_lock != nearbylock)
						fx.scale = Vector3.Lerp(fx.scale, Vector3.zero, .25f);
					else
						fx.scale = Vector3.Lerp(fx.scale, Vector3.one, .25f);

					if (Vector3.Distance(fx.scale, Vector3.zero) < .01f)
						fx.scale = Vector3.zero;

					fx.PlaceAtPositionByAnchor(m_lock.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				}
				foreach (var m_NPClock in encounterNPCLocks)
				{
					if (!m_NPClock)
						continue;

					var fx = m_NPClock?.transform?.Find(vfxName2)?.GetComponent<tk2dSprite>();

					if (!fx)
						continue;

					if (m_NPClock != npcLock)
						fx.scale = Vector3.Lerp(fx.scale, Vector3.zero, .25f);
					else
						fx.scale = Vector3.Lerp(fx.scale, Vector3.one, .25f);

					if (Vector3.Distance(fx.scale, Vector3.zero) < .01f)
						fx.scale = Vector3.zero;

					fx.PlaceAtPositionByAnchor(m_NPClock.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				}

			}

			void InitializeNPCLock(DungeonDoorController NPCLOCK)
			{
				GameObject prefab = costVFX2;
				var sprite = GameObject.Instantiate(prefab, NPCLOCK.transform).GetComponent<tk2dSprite>();
				sprite.name = vfxName2;
				sprite.PlaceAtPositionByAnchor(NPCLOCK.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				sprite.scale = Vector3.zero;
				npcLock = NPCLOCK;
				encounterNPCLocks.Add(NPCLOCK);
			}
			void InitLock(InteractableLock fnsdnfnsfns)
			{
				GameObject prefab = costVFX2;
				var sprite = GameObject.Instantiate(prefab, fnsdnfnsfns.transform).GetComponent<tk2dSprite>();
				sprite.name = vfxName2;
				sprite.PlaceAtPositionByAnchor(fnsdnfnsfns.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				sprite.scale = Vector3.zero;
				nearbylock = fnsdnfnsfns;
				encounterLocks.Add(fnsdnfnsfns);
			}
			void InitializeChest(Chest chest)
			{
				GameObject prefab = costVFX;
				var sprite = GameObject.Instantiate(prefab, chest.transform).GetComponent<tk2dSprite>();
				sprite.name = vfxName;
				sprite.PlaceAtPositionByAnchor(chest.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				sprite.scale = Vector3.zero;
				nearbyChest = chest;
				encounteredChests.Add(chest);
			}
			public void DestroyAllFX()
			{
				foreach (var chest in encounteredChests)
				{
					var fx = chest.transform.Find(vfxName);
					if (fx)
						Destroy(fx);
				}
				foreach (var m_lock in encounterLocks)
				{
					var fx = m_lock.transform.Find(vfxName2);
					if (fx)
						Destroy(fx);
				}
				foreach (var m_NPClock in encounterNPCLocks)
				{
					var fx = m_NPClock.transform.Find(vfxName2);
					if (fx)
						Destroy(fx);
				}
				encounteredChests.Clear();
				encounterNPCLocks.Clear();
				encounterLocks.Clear();

			}
		}

	}



}

