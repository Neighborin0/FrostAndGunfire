using System;
using System.Collections;
using Dungeonator;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class CrystalDie : PlayerItem
	{
		
		public static void Init()
		{
			string name = "Crystal Die";
			string resourcePath = "FrostAndGunfireItems/Resources/dice";
			GameObject gameObject = new GameObject();
			CrystalDie crystalDie = gameObject.AddComponent<CrystalDie>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Re-roll It All";
			string longDesc = "This special die contains chronotons within it's center, changing the user's timeline. Who knows what these effects can do?";
			ItemBuilder.SetupItem(crystalDie, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(crystalDie, ItemBuilder.CooldownType.Timed, 10f);
			crystalDie.consumable = true;
			crystalDie.numberOfUses = 3;
			crystalDie.quality = PickupObject.ItemQuality.C;
			crystalDie.SetupUnlockOnCustomFlag(CustomDungeonFlags.CHALLENGE_MODE_AND_WANDERER, true);
		}

		
		protected override void DoEffect(PlayerController user)
		{
			this.DiceEffect(user);
		}

		private void DiceEffect(PlayerController user)
		{
			string header = "";
			string text = "";
			HealthHaver healthHaver = user.healthHaver;
			AkSoundEngine.PostEvent("Play_OBJ_dice_bless_01", base.gameObject);
			int num = UnityEngine.Random.Range(0, 18);
			bool flag = num == 0;
			if (flag)
			{
				StatModifier statModifier = new StatModifier();
				statModifier.statToBoost = PlayerStats.StatType.GlobalPriceMultiplier;
				statModifier.amount = -0.15f;
				statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(statModifier);
				user.stats.RecalculateStats(user, false, false);
				header = "Bargined";
			}
			bool flag2 = num == 1;
			if (flag2)
			{
				StatModifier statModifier2 = new StatModifier();
				statModifier2.statToBoost = PlayerStats.StatType.Curse;
				statModifier2.amount = (float)UnityEngine.Random.Range(1, 5);
				statModifier2.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(statModifier2);
				user.stats.RecalculateStats(user, false, false);
				header = "Cursed";
			}
			bool flag3 = num == 2;
			if (flag3)
			{
				AkSoundEngine.PostEvent("Play_WPN_LowerCaseR_Angry_Loser_01", base.gameObject);
				UnityEngine.Object.Destroy(this);
				user.DropActiveItem(this, 4f, false);
				header = "Loser";
				text = "You get nothing! You lose!";
			}
			bool flag4 = num == 3;
			if (flag4)
			{
				PickupObject byId = PickupObjectDatabase.GetById(127);
				LootEngine.SpawnItem(byId.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
				PickupObject byId2 = PickupObjectDatabase.GetById(127);
				LootEngine.SpawnItem(byId2.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
				PickupObject byId3 = PickupObjectDatabase.GetById(127);
				LootEngine.SpawnItem(byId3.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
				header = "Junked";
			}
			bool flag5 = num == 4;
			if (flag5)
			{
				IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
				Chest chest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(bestRewardLocation);
				chest.IsLocked = false;
				header = "Gifted";
			}
			bool flag6 = num == 5;
			if (flag6)
			{
				user.carriedConsumables.Currency -= UnityEngine.Random.Range(20, 80);
				header = "Robbed";
			}
			bool flag7 = num == 6;
			if (flag7)
			{
				PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.C;
				PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value <= 0.3f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
				bool flag8 = itemOfTypeAndQuality;
				bool flag9 = flag8;
				if (flag9)
				{
					LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, base.transform.position, Vector2.up, 0.1f, true, false, false);
				}
				UnityEngine.Object.Destroy(this);
				user.DropActiveItem(this, 4f, false);
				header = "Traded";
			}
			bool flag10 = num == 7;
			if (flag10)
			{
				StatModifier statModifier3 = new StatModifier();
				StatModifier statModifier4 = new StatModifier();
				StatModifier statModifier5 = new StatModifier();
				statModifier3.statToBoost = PlayerStats.StatType.Damage;
				statModifier3.amount = 0.2f;
				statModifier3.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(statModifier3);
				statModifier4.statToBoost = PlayerStats.StatType.RateOfFire;
				statModifier4.amount = 0.2f;
				statModifier4.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(statModifier4);
				statModifier5.statToBoost = PlayerStats.StatType.ReloadSpeed;
				statModifier5.amount = -0.2f;
				statModifier5.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(statModifier5);
				user.stats.RecalculateStats(user, false, false);
				header = "Buffed";
			}
			bool flag11 = num == 8;
			if (flag11)
			{
				healthHaver.ApplyDamage(0.5f, Vector2.zero, "Gambling Addiction", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				header = "Damaged";
			}
			bool flag12 = num == 9;
			if (flag12)
			{
				StatModifier statModifier6 = new StatModifier();
				StatModifier statModifier7 = new StatModifier();
				statModifier6.statToBoost = PlayerStats.StatType.Curse;
				statModifier6.amount = (float)UnityEngine.Random.Range(-3, -1);
				statModifier6.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(statModifier6);
				user.stats.RecalculateStats(user, false, false);
				header = "Blessed";
			}
			bool flag13 = num == 10;
			if (flag13)
			{
				float currentHealth = user.healthHaver.GetCurrentHealth();
				user.healthHaver.ForceSetCurrentHealth(currentHealth + UnityEngine.Random.Range(1f, 4f));
				header = "Healed";
			}
			bool flag14 = num == 11;
			if (flag14)
			{
				user.healthHaver.Armor += (float)UnityEngine.Random.Range(1, 3);
				header = "Armored";
			}
			bool flag15 = num == 12;
			if (flag15)
			{
				StatModifier statModifier8 = new StatModifier();
				statModifier8.statToBoost = PlayerStats.StatType.AmmoCapacityMultiplier;
				statModifier8.amount = -0.2f;
				statModifier8.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(statModifier8);
				user.stats.RecalculateStats(user, false, false);
				header = "Limited";
			}
			bool flag16 = num == 13;
			if (flag16)
			{
				PickupObject byId4 = PickupObjectDatabase.GetById(78);
				LootEngine.SpawnItem(byId4.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
				header = "Reloaded";
			}
			bool flag17 = num == 14;
			if (flag17)
			{
				user.carriedConsumables.Currency += UnityEngine.Random.Range(20, 60);
				header = "Mo' Money";
			}
			bool flag18 = num == 15;
			if (flag18)
			{
				header = "Nothing";
			}
			bool flag19 = num == 16;
			if (flag19)
			{
				user.CurrentGun.ammo = 0;
				header = "Emptied";
			}
			bool flag20 = num == 17;
			if (flag20)
			{
				Exploder.DoDefaultExplosion(user.transform.position, Vector2.one, null, false, CoreDamageTypes.Fire, false);
				header = "Exploded";
			}
			this.Notify(header, text);
		}


		private void Notify(string header, string text)
		{
			tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, false, true);
		}

	

	}
}
