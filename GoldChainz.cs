using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class GoldChainz : PassiveItem
	{
		private bool Stop;
		
		public static void Init()
		{
			string name = "Gold Chainz";
			string resourcePath = "FrostAndGunfireItems/Resources/chainz";
			GameObject gameObject = new GameObject(name);
			GoldChainz goldChainz = gameObject.AddComponent<GoldChainz>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Pop Pop";
			string longDesc = "Mo' money, mo' bulletz. \n\nTheze golden chainz were said to fallen from a distant planet, where the true gun god lives.";
			ItemBuilder.SetupItem(goldChainz, shortDesc, longDesc, "kp");
			goldChainz.quality = PickupObject.ItemQuality.A;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000069BB File Offset: 0x00004BBB
		public override void Pickup(PlayerController player)
		{
			
			base.Pickup(player);
			player.OnRoomClearEvent += this.RoomCleared;
			player.OnEnteredCombat += this.EnterCombat;
		}

		private void RoomCleared(PlayerController obj)
		{
			this.EvaluateStats();
		}

		private void EnterCombat()
		{
			this.EvaluateStats();
		}


		protected override void Update()
		{
			bool flag = base.Owner;
			if (flag)
			{
				if (!Owner.HasPickupID(641))
				{
					this.RemoveStat(PlayerStats.StatType.ReloadSpeed);
					Stop = false;
				}
			}

		}


		// Token: 0x06000084 RID: 132 RVA: 0x000069CC File Offset: 0x00004BCC
		private void EvaluateStats()
		{
			if (Owner.HasPickupID(204) && !Stop)
			{
				this.RemoveStat(PlayerStats.StatType.ReloadSpeed);
				this.AddStat(PlayerStats.StatType.ReloadSpeed, -0.3f, StatModifier.ModifyMethod.ADDITIVE);
				Stop = true;
				
			}
			float YVChance = (float)base.Owner.carriedConsumables.Currency;
			if (YVChance >= 186)
			{
				this.RemoveStat(PlayerStats.StatType.ExtremeShadowBulletChance);
				this.AddStat(PlayerStats.StatType.ExtremeShadowBulletChance, 185 * 0.27f, StatModifier.ModifyMethod.ADDITIVE);
			}
			if (YVChance <= 185)
			{
				this.RemoveStat(PlayerStats.StatType.ExtremeShadowBulletChance);
				this.AddStat(PlayerStats.StatType.ExtremeShadowBulletChance, (float)base.Owner.carriedConsumables.Currency * 0.27f, StatModifier.ModifyMethod.ADDITIVE);
			}
			base.Owner.stats.RecalculateStats(base.Owner, true, false);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006A20 File Offset: 0x00004C20
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

		// Token: 0x06000086 RID: 134 RVA: 0x00006AC4 File Offset: 0x00004CC4
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

		// Token: 0x06000087 RID: 135 RVA: 0x00006B2C File Offset: 0x00004D2C
		public override DebrisObject Drop(PlayerController player)
		{
			this.RemoveStat(PlayerStats.StatType.ReloadSpeed);
			player.OnRoomClearEvent -= this.RoomCleared;
			player.OnEnteredCombat -= this.EnterCombat;
			Stop = false;
			DebrisObject debrisObject = base.Drop(player);
			GoldChainz component = debrisObject.GetComponent<GoldChainz>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x04000027 RID: 39
	
	}
}
