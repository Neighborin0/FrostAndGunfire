using System;
using System.Collections.Generic;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200001C RID: 28
	public class PerfectRound : PassiveItem
	{
		//improve sprite

		public static void Init()
		{

			string name = "Medal of Perfection";
			string resourcePath = "FrostAndGunfireItems/Resources/perfect";
			GameObject gameObject = new GameObject(name);
			PerfectRound perfectRound = gameObject.AddComponent<PerfectRound>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "You're Not Perfect";
			string longDesc = "Stats up, when you perform perfectly...\n\nThis extraordinary artifact indicates mastery of the Gungeon. Are you worthy enough to possess it?";
			ItemBuilder.SetupItem(perfectRound, shortDesc, longDesc, "kp");
			perfectRound.quality = PickupObject.ItemQuality.D;
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.RangeMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.Accuracy, -0.3f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.AdditionalClipCapacityMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.AdditionalShotBounces, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.AdditionalShotPiercing, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.AmmoCapacityMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.ChargeAmountMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.Coolness, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.Damage, 0.3f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.DamageToBosses, 0.3f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.DodgeRollDamage, 0.25f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.GlobalPriceMultiplier, -0.30f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.MovementSpeed, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.RateOfFire, 0.3f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(perfectRound, PlayerStats.StatType.ReloadSpeed, -0.3f, StatModifier.ModifyMethod.ADDITIVE);
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			HealthHaver healthHaver = player.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
		}

		private void HandleEffect(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
		{
			AkSoundEngine.PostEvent("Play_WPN_LowerCaseR_Angry_Loser_01", base.gameObject);
			StatModifier statModifier = new StatModifier();
			Owner.ownerlessStatModifiers.Add(statModifier);
			Owner.stats.RecalculateStats(Owner, false, false);
			statModifier.statToBoost = PlayerStats.StatType.Curse;
			statModifier.amount = 2f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			Owner.DropPassiveItem(this);
			UnityEngine.Object.Destroy(base.gameObject, 1f);
		}
		protected override void OnDestroy()
		{
			bool flag = base.Owner;
			if (flag)
			{
				HealthHaver healthHaver = base.Owner.healthHaver;
				healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
				
			}
			base.OnDestroy();
		}
		public override DebrisObject Drop(PlayerController player)
		{
			HealthHaver healthHaver = player.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
			DebrisObject debrisObject = base.Drop(player);
			PerfectRound component = debrisObject.GetComponent<PerfectRound>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
