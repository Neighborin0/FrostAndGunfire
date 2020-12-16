using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace FrostAndGunfireItems
{
	//Good to Go
	public class AfflictedAmmolet : BlankModificationItem
	{
	
		public static void Init()
		{
			string name = "Afflicted Ammolet";
			string resourcePath = "FrostAndGunfireItems/Resources/Charm";
			GameObject gameObject = new GameObject(name);
			AfflictedAmmolet afflictedAmmolet = gameObject.AddComponent<AfflictedAmmolet>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Blanks Fear";
			string longDesc = "Blanks apply fear for three seconds.\n\nThe cursed metal used to forge this unholy amulet makes blanks scream with agony. It reminds Gundead of the fabled Bullet Hell.";
			ItemBuilder.SetupItem(afflictedAmmolet, shortDesc, longDesc, "kp");
			afflictedAmmolet.quality = ItemQuality.D;
			ItemBuilder.AddPassiveStatModifier(afflictedAmmolet, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(afflictedAmmolet, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
			afflictedAmmolet.AddToSubShop(ItemBuilder.ShopType.OldRed);
			afflictedAmmolet.AddToSubShop(ItemBuilder.ShopType.Cursula);
			ID = afflictedAmmolet.PickupObjectId;
		}
		private static int ID;


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			if (fleeData == null || fleeData.Player != player)
			{
				fleeData = new FleePlayerData
				{
					Player = player,
					StartDistance = 100f
				};
			}
		}

		private static Hook BlankHook = new Hook(
	  typeof(SilencerInstance).GetMethod("ProcessBlankModificationItemAdditionalEffects", BindingFlags.Instance | BindingFlags.NonPublic),
	  typeof(AfflictedAmmolet).GetMethod("BlankModHook")
  );


		public static void BlankModHook(Action<SilencerInstance, BlankModificationItem, Vector2, PlayerController> orig, SilencerInstance silencer, BlankModificationItem bmi, Vector2 centerPoint, PlayerController user)
		{
			orig(silencer, bmi, centerPoint, user);
			if (user.HasPickupID(AfflictedAmmolet.ID))
			{
				RoomHandler currentRoom = user.CurrentRoom;
				if (currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
				{

					foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
					{
					
						if (aiactor.behaviorSpeculator != null)
						{

							aiactor.behaviorSpeculator.FleePlayerData = fleeData;
							GameManager.Instance.StartCoroutine(RemoveFear(aiactor));

						}
					}
				}

			}


		}



	
		private static IEnumerator RemoveFear(AIActor aiactor)
		{
				yield return new WaitForSeconds(3.5f);
				aiactor.behaviorSpeculator.FleePlayerData = null;
				yield break;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			AfflictedAmmolet component = debrisObject.GetComponent<AfflictedAmmolet>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x04000033 RID: 51
		private static FleePlayerData fleeData;
		private static PlayerController player;

	

	}
}
