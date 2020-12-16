using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	// Token: 0x02000018 RID: 24
	public class DemonDeals : PassiveItem
	{


		bool Stop;

		// Token: 0x0600007E RID: 126 RVA: 0x00006878 File Offset: 0x00004A78
		public static void Init()
		{
			string name = "Devil's Discount";
			string resourcePath = "FrostAndGunfireItems/Resources/demon_deal";
			GameObject gameObject = new GameObject(name);
			DemonDeals item = gameObject.AddComponent<DemonDeals>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "You Pay In Other Ways...";
			string longDesc = "A contract with an otherworldly, evil, force, this contract allows you to purchase anything you want, at a price...";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = ItemQuality.B;
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.GlobalPriceMultiplier, -0.4f, StatModifier.ModifyMethod.ADDITIVE);
			item.AddToSubShop(ItemBuilder.ShopType.Cursula);
			item.SetupUnlockOnCustomFlag(CustomDungeonFlags.LICH_KILLED_AND_CONVICT, true);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000068EC File Offset: 0x00004AEC
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnItemPurchased += this.OnItemPurchased;
		}

		protected override void Update()
		{
			bool flag = base.Owner;
			if (flag)
			{
				if (Owner.HasPickupID(336) && !Stop)
				{
					StatModifier statModifier = new StatModifier();
					statModifier.statToBoost = PlayerStats.StatType.GlobalPriceMultiplier;
					statModifier.amount = -0.1f;
					statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					Owner.ownerlessStatModifiers.Add(statModifier);
					Owner.stats.RecalculateStats(Owner, true, false);
					Stop = true;
				}
			}
		}


		// Token: 0x0600004A RID: 74 RVA: 0x00004400 File Offset: 0x0000260
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
		private void OnItemPurchased(PlayerController player, ShopItemController obj)
		{
			if(player.HasPickupID(336))
			{
				StatModifier statModifier1 = new StatModifier();
				statModifier1.statToBoost = PlayerStats.StatType.Curse;
				statModifier1.amount = 1f;
				statModifier1.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				player.ownerlessStatModifiers.Add(statModifier1);
				player.stats.RecalculateStats(player, false, false);
			}
			StatModifier statModifier = new StatModifier();
			statModifier.statToBoost = PlayerStats.StatType.Curse;
			statModifier.amount = 1f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			player.ownerlessStatModifiers.Add(statModifier);
			player.stats.RecalculateStats(player, false, false);
		}




		// Token: 0x06000082 RID: 130 RVA: 0x0000697C File Offset: 0x00004B7C
		public override DebrisObject Drop(PlayerController player)
		{
			player.OnItemPurchased -= this.OnItemPurchased;
			DebrisObject debrisObject = base.Drop(player);
			DemonDeals component = debrisObject.GetComponent<DemonDeals>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
