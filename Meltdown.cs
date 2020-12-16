using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000020 RID: 32
	public class Meltdown : PassiveItem
	{
		// Token: 0x060000CA RID: 202 RVA: 0x00008814 File Offset: 0x00006A14
		public static void Init()
		{
			string name = "Meltdown";
			string resourcePath = "FrostAndGunfireItems/Resources/meltdown";
			GameObject gameObject = new GameObject(name);
			Meltdown meltdown = gameObject.AddComponent<Meltdown>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Keep Calm";
			string longDesc = "Remain cool and remain calm. Keep you gun steady and ready. If you lose your cool, it's fight or flight.";
			ItemBuilder.SetupItem(meltdown, shortDesc, longDesc, "kp");
			meltdown.quality = PickupObject.ItemQuality.EXCLUDED;
	
		}

		public override void Pickup(PlayerController player)
		{

			base.Pickup(player);
			this.EvaluateStats(player, true);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00008560 File Offset: 0x00006760
		protected override void Update()
		{

			bool flag = base.Owner;
			bool flag2 = flag;
			if (flag2)
			{

				this.EvaluateStats(base.Owner, false);
			}
		}


		// Token: 0x060000C5 RID: 197 RVA: 0x00008590 File Offset: 0x00006790
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			Meltdown component = debrisObject.GetComponent<Meltdown>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000085BC File Offset: 0x000067BC
		private void EvaluateStats(PlayerController player, bool force = false)
		{

			this.hasArmor = (player.healthHaver.Armor > 0f || player.healthHaver.HasCrest);
			this.shouldRestat = (this.hadArmor != this.hasArmor);
			bool flag = !this.shouldRestat && !force;
			bool flag2 = !flag;
			if (flag2)
			{
				bool flag3 = this.hasArmor;
				bool flag4 = flag3;
				if (flag4)
				{
					this.RemoveStat(PlayerStats.StatType.Damage);
					this.RemoveStat(PlayerStats.StatType.MovementSpeed);
					this.RemoveStat(PlayerStats.StatType.ReloadSpeed);
					this.AddStat(PlayerStats.StatType.MovementSpeed, +0.35f, StatModifier.ModifyMethod.ADDITIVE);
					this.AddStat(PlayerStats.StatType.ReloadSpeed, -0.1f, StatModifier.ModifyMethod.ADDITIVE);
					this.AddStat(PlayerStats.StatType.RateOfFire, 0.1f, StatModifier.ModifyMethod.ADDITIVE);
					this.hadArmor = true;
					

				}
				else
				{
					this.RemoveStat(PlayerStats.StatType.MovementSpeed);
					this.RemoveStat(PlayerStats.StatType.ReloadSpeed);
					this.RemoveStat(PlayerStats.StatType.RateOfFire);
					this.AddStat(PlayerStats.StatType.ReloadSpeed, +0.10f, StatModifier.ModifyMethod.ADDITIVE);
					this.AddStat(PlayerStats.StatType.MovementSpeed, -0.75f, StatModifier.ModifyMethod.ADDITIVE);
					this.AddStat(PlayerStats.StatType.Damage, 0.25f, StatModifier.ModifyMethod.ADDITIVE);
					this.hadArmor = false;
				}
				player.stats.RecalculateStats(player, true, false);
			}
		}

	
		// Token: 0x060000C7 RID: 199 RVA: 0x00008708 File Offset: 0x00006908
		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
		{
			foreach (StatModifier statModifier in this.passiveStatModifiers)
			{
				bool flag = statModifier.statToBoost == statType;
				bool flag2 = flag;
				if (flag2)
				{
					return;
				}
			}
			StatModifier statModifier2 = new StatModifier
			{
				amount = amount,
				statToBoost = statType,
				modifyType = method
			};
			bool flag3 = this.passiveStatModifiers == null;
			bool flag4 = flag3;
			if (flag4)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier2
				};
				return;
			}
			this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
			{
				statModifier2
			}).ToArray<StatModifier>();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000087AC File Offset: 0x000069AC
		private void RemoveStat(PlayerStats.StatType statType)
		{
			List<StatModifier> list = new List<StatModifier>();
			for (int i = 0; i < this.passiveStatModifiers.Length; i++)
			{
				bool flag = this.passiveStatModifiers[i].statToBoost != statType;
				bool flag2 = flag;
				if (flag2)
				{
					list.Add(this.passiveStatModifiers[i]);
				}
			}
			this.passiveStatModifiers = list.ToArray();
		}

		// Token: 0x04000048 RID: 72
		private bool hasArmor;

		// Token: 0x04000049 RID: 73
		private bool hadArmor;

		// Token: 0x0400004A RID: 74
		private bool shouldRestat;


		// Token: 0x0400004B RID: 75

	}
}
