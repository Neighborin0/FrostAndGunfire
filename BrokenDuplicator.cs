using System;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class BrokenDuplicator : PlayerItem
	{
	
		public static void Init()
		{
			string name = "Broken Duplicator";
			string resourcePath = "FrostAndGunfireItems/Resources/duplicator";
			GameObject gameObject = new GameObject();
			BrokenDuplicator brokenDuplicator = gameObject.AddComponent<BrokenDuplicator>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Chest Overflow Error";
			string longDesc = "Has a 65% chance to spawn a chest when near one. 35% chance to backfire. Horribly. \n\nA tool made by Chester Locke, this duplicator is broken from years of Gungeoneers overusing its power.";
			ItemBuilder.SetupItem(brokenDuplicator, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(brokenDuplicator, ItemBuilder.CooldownType.Damage, 850f);
			brokenDuplicator.consumable = false;
			brokenDuplicator.quality = PickupObject.ItemQuality.B;
			brokenDuplicator.AddToSubShop(ItemBuilder.ShopType.Goopton);
			brokenDuplicator.AddToSubShop(ItemBuilder.ShopType.Flynt);
		}

	
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}


		protected override void DoEffect(PlayerController user)
		{
			StatModifier statModifier = new StatModifier();
			user.ownerlessStatModifiers.Add(statModifier);
			user.stats.RecalculateStats(user, false, false);
			IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
			bool flag = !(nearestInteractable is Chest);
			bool flag2 = !flag;
			bool flag3 = flag2;
			if (flag3)
			{
				Chest chest = nearestInteractable as Chest;
				bool isMimic = chest.IsMimic;
				bool flag4 = isMimic;
				bool flag5 = flag4;
				if (flag5)
				{
				}
				bool flag6 = user && user.CurrentRoom != null;
				float value = UnityEngine.Random.value;
				bool flag7 = (double)value < 0.65;
				bool flag8 = flag6 && flag7;
				bool flag9 = flag8;
				if (flag9)
				{
					
					IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
					Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(bestRewardLocation);
					chest2.IsLocked = false;
				}
				else
				{
					AkSoundEngine.PostEvent("Play_WPN_glass_impact_01", base.gameObject);
					Exploder.DoDefaultExplosion(chest.transform.position, Vector2.one, null, false, CoreDamageTypes.Fire, false);
					statModifier.statToBoost = PlayerStats.StatType.Curse;
					statModifier.amount = 1f;
					statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(statModifier);
					user.stats.RecalculateStats(user, false, false);
					PickupObject byId = PickupObjectDatabase.GetById(127);
					LootEngine.SpawnItem(byId.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
					UnityEngine.Object.Destroy(this);
					user.DropActiveItem(this, 4f, false);
				}
			}
		}

		public override bool CanBeUsed(PlayerController user)
		{
			IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
			return nearestInteractable is Chest;
		}
	}
}
