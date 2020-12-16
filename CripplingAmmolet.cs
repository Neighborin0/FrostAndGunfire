using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Dungeonator;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;


namespace FrostAndGunfireItems
{
	
	public class CripplingAmmolet: BlankModificationItem
	{
		public static GameObject customVFXPrefab;

		public static PlayerController player;
		public static void Init()
		{
			string name = "Rusty Ammolet";
			string resourcePath = "FrostAndGunfireItems/Resources/cripple_charm";
			GameObject gameObject = new GameObject(name);
			CripplingAmmolet cripplingAmmolet = gameObject.AddComponent<CripplingAmmolet>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Blanks Cripple";
			string longDesc = "This enchanted amulet was created by a great gunsmith eons ago. The rusted metal from this amulet can temporarily cripple the aggresive abilities of Gundead.";
			ItemBuilder.SetupItem(cripplingAmmolet, shortDesc, longDesc, "kp");
			cripplingAmmolet.quality = PickupObject.ItemQuality.A;
			ItemBuilder.AddPassiveStatModifier(cripplingAmmolet, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(cripplingAmmolet, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
			cripplingAmmolet.AddToSubShop(ItemBuilder.ShopType.OldRed);
			cripplingAmmolet.AddToSubShop(ItemBuilder.ShopType.Trorc);
			CripplingAmmolet.ID3 = cripplingAmmolet.PickupObjectId;
			customVFXPrefab = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/cripple_vfx", null, true);
			GameObject.DontDestroyOnLoad(customVFXPrefab);
			FakePrefab.MarkAsFakePrefab(customVFXPrefab);
			customVFXPrefab.SetActive(false);
		}
		private static int ID3;
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			
		}
		private static Hook BlankHook = new Hook(
  typeof(SilencerInstance).GetMethod("ProcessBlankModificationItemAdditionalEffects", BindingFlags.Instance | BindingFlags.NonPublic),
  typeof(CripplingAmmolet).GetMethod("BlankModHook")
);

		public static void BlankModHook(Action<SilencerInstance, BlankModificationItem, Vector2, PlayerController> orig, SilencerInstance silencer, BlankModificationItem bmi, Vector2 centerPoint, PlayerController user)
		{
			orig(silencer, bmi, centerPoint, user);
			bool flag = user.HasPickupID(ID3);
			if (flag)
			{
				RoomHandler currentRoom = user.CurrentRoom;
				if (currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
				{

					foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
					{
						bool flag4 = aiactor.behaviorSpeculator != null;
						bool flag5 = flag4;
						bool flag6 = flag5;
						if (flag6)
						{


							GameManager.Instance.StartCoroutine(CripplingAmmolet.RemoveCripple(aiactor));


						}
					}
				}

			}


		}


		private static IEnumerator RemoveCripple(AIActor aiactor)
		{
			Vector2 offset = new Vector2(0, .30f);
			aiactor.CanTargetPlayers = false;
				var obj = GameObject.Instantiate(customVFXPrefab);
			obj.SetActive(true);
			var sprite = obj.GetComponent<tk2dSprite>();
				sprite.PlaceAtPositionByAnchor(aiactor.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				sprite.transform.SetParent(aiactor.transform);
				yield return new WaitForSeconds(3.5f);
				aiactor.CanTargetPlayers = true;
				GameObject.Destroy(sprite);
				yield break;
			
		}

		public override DebrisObject Drop(PlayerController player)
		{
		
			DebrisObject debrisObject = base.Drop(player);
			CripplingAmmolet component = debrisObject.GetComponent<CripplingAmmolet>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		


	
	}
}
