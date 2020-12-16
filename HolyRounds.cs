using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	// Token: 0x02000018 RID: 24
	public class HolyRounds : PassiveItem
	{
		
		private bool Stop;

		// Token: 0x0600007E RID: 126 RVA: 0x00006878 File Offset: 0x00004A78
		public static void Init()
		{
			string name = "Holy Rounds";
			string resourcePath = "FrostAndGunfireItems/Resources/holy_rounds";
			GameObject gameObject = new GameObject(name);
			HolyRounds holyRounds = gameObject.AddComponent<HolyRounds>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Bless You";
			string longDesc = "This special ammo allows the user to resist all forms of curse. Imbudes your weapons with holy strength. \n\nThis ammo was said to have given to those chosen by Kaliber herself.";
			ItemBuilder.SetupItem(holyRounds, shortDesc, longDesc, "kp");
			holyRounds.quality = ItemQuality.A;
			ItemBuilder.AddPassiveStatModifier(holyRounds, PlayerStats.StatType.Damage, 0.1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(holyRounds, PlayerStats.StatType.ReloadSpeed, -0.1f, StatModifier.ModifyMethod.ADDITIVE);
			holyRounds.SetupUnlockOnCustomFlag(CustomDungeonFlags.BLESSED_AND_WANDERER, true);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000068EC File Offset: 0x00004AEC
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.EvaluateStats();
		}


		


		// Token: 0x06000080 RID: 128 RVA: 0x000068FE File Offset: 0x00004AFE
		protected override void Update()
		{
			float curse = PlayerStats.GetTotalCurse();
			bool flag = base.Owner;
			bool flag2 = flag;
			if (flag2)
			{
				
				if (curse > 0)
				{
					this.EvaluateStats();
				}
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006910 File Offset: 0x00004B10
		private void EvaluateStats()
		{
			if (Owner.HasPickupID(571) && !Stop)
			{
				StatModifier statModifier = new StatModifier();
				statModifier.statToBoost = PlayerStats.StatType.Damage;
				statModifier.amount = 0.4f;
				statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				Owner.ownerlessStatModifiers.Add(statModifier);
				Owner.stats.RecalculateStats(Owner, false, false);
				Stop = true;
			}
				StatModifier statModifier2 = new StatModifier();
			statModifier2.statToBoost = PlayerStats.StatType.Curse;
			statModifier2.amount = PlayerStats.GetTotalCurse() * -1f;
			statModifier2.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			Owner.ownerlessStatModifiers.Add(statModifier2);
			Owner.stats.RecalculateStats(Owner, false, false);
		}


		// Token: 0x06000082 RID: 130 RVA: 0x0000697C File Offset: 0x00004B7C
		public override DebrisObject Drop(PlayerController player)
		{
			if (player.HasPickupID(571) && Stop)
			{
				StatModifier statModifier = new StatModifier();
				statModifier.statToBoost = PlayerStats.StatType.Damage;
				statModifier.amount = -0.4f;
				statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				Owner.ownerlessStatModifiers.Add(statModifier);
				Owner.stats.RecalculateStats(Owner, false, false);
				Stop = false;
			}
			DebrisObject debrisObject = base.Drop(player);
			HolyRounds component = debrisObject.GetComponent<HolyRounds>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
