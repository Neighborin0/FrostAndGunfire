
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
		Hook chesthook = new Hook(
				typeof(Chest).GetMethod("Interact", BindingFlags.Public | BindingFlags.Instance),
				typeof(Greed).GetMethod("SpendMoney")
			);
		Hook lockhook = new Hook(
				typeof(InteractableLock).GetMethod("Interact", BindingFlags.Public | BindingFlags.Instance),
				typeof(Greed).GetMethod("SpendMoneyLock")
			);
		Hook keyHook = new Hook(
			   typeof(KeyBulletPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
			   typeof(Greed).GetMethod("KeyHookMethod")
		   );
		private static GameObject costVFX;
		private static string vfxName = "CostVFX";

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
			BuildPrefab();
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage += SpawnMoney;
			player.gameObject.AddComponent<DisplayCost>();
		}

		public static void BuildPrefab()
		{
			costVFX = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/vfx_stuff/cost_vfx", null, false);
			costVFX.name = vfxName;
			GameObject.DontDestroyOnLoad(costVFX);
			FakePrefab.MarkAsFakePrefab(costVFX);
			costVFX.SetActive(false);
		}

		private void SpawnMoney(float damage, bool fatal, HealthHaver enemyHealth)
		{
			float value = UnityEngine.Random.value;
			if ((double)value < 0.05)
			{
				if (enemyHealth.aiActor && enemyHealth && !enemyHealth.IsBoss && fatal && enemyHealth.aiActor.IsNormalEnemy)
				{
					for (int i = 0; i < 15; i++)
					{
						PickupObject byId = PickupObjectDatabase.GetById(68);
						LootEngine.SpawnItem(byId.gameObject, enemyHealth.aiActor.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
					}
				}
			}
		}


		public static void SpendMoney(Action<Chest, PlayerController> orig, Chest self, PlayerController player)
		{
			if (player.HasPickupID(Gungeon.Game.Items["kp:greed"].PickupObjectId))
			{
				if (player.carriedConsumables.Currency >= 40)
				{
					self.ForceUnlock();
					player.carriedConsumables.Currency -= 20;
				}
			}
			orig(self, player);
		}
		public static void SpendMoneyLock(Action<InteractableLock, PlayerController> orig, InteractableLock self, PlayerController player)
		{
			if (player.HasPickupID(Gungeon.Game.Items["kp:greed"].PickupObjectId))
			{
				if (player.carriedConsumables.Currency >= 40)
				{
					self.ForceUnlock();
					player.carriedConsumables.Currency -= 20;
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
					player.carriedConsumables.Currency += 15;
				}
			}
		
		}

		private class DisplayCost : BraveBehaviour
		{
			List<Chest> encounteredChests = new List<Chest>();
			List<InteractableLock> encounterLocks = new List<InteractableLock>();
			PlayerController player;
			InteractableLock nearbylock;
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
				HandleChests();
			}

			void HandleChests()
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
				encounteredChests.Clear();
			}
		}

	}



}

