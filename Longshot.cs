using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200001C RID: 28
	public class Longshot : PassiveItem
	{
		private bool Stop;

		// Token: 0x060000B7 RID: 183 RVA: 0x0000816C File Offset: 0x0000636C
		public static void Init()
		{
			string name = "Longshot";
			string resourcePath = "FrostAndGunfireItems/Resources/longshot";
			GameObject gameObject = new GameObject(name);
			Longshot longshot = gameObject.AddComponent<Longshot>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Loooooooooooooooooong";
			string longDesc = "Doubles range.\n\nThese rounds increases the lifespan of bullets by 50%. This lead to them being extremely thin and malnourished however";
			ItemBuilder.SetupItem(longshot, shortDesc, longDesc, "kp");
			longshot.quality = PickupObject.ItemQuality.D;
			ItemBuilder.AddPassiveStatModifier(longshot, PlayerStats.StatType.RangeMultiplier, 2f, StatModifier.ModifyMethod.ADDITIVE);
			longshot.AddToSubShop(ItemBuilder.ShopType.Trorc);
		}

		protected override void Update()
		{
			bool flag = Owner;
			if (flag)
			{
				if (Owner.HasPickupID(49) || Owner.HasPickupID(5) || Owner.HasPickupID(385) || Owner.HasPickupID(25))
				{
					this.AddStat(PlayerStats.StatType.ReloadSpeed, -0.3f, StatModifier.ModifyMethod.ADDITIVE);
					Owner.stats.RecalculateStats(Owner, true, false);
				}
				
				
			}
		}

		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
		{
			StatModifier statModifier = new StatModifier();
			statModifier.amount = amount;
			statModifier.statToBoost = statType;
			statModifier.modifyType = method;
			foreach (StatModifier statModifier2 in this.passiveStatModifiers)
			{
				bool flag = statModifier2.statToBoost == statType;
				bool flag2 = flag;
				if (flag2)
				{
					return;
				}
			}
			bool flag3 = this.passiveStatModifiers == null;
			bool flag4 = flag3;
			if (flag4)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier
				};
				return;
			}
			this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
			{
				statModifier
			}).ToArray<StatModifier>();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004400 File Offset: 0x00002600
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
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			Longshot component = debrisObject.GetComponent<Longshot>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
