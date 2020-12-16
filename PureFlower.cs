
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000A RID: 10
	public class PureFlower : PassiveItem
	{
		public static GameObject vfx;
		public static void Init()
		{
			
			string name = "Pure Flower";
			string resourcePath = "FrostAndGunfireItems/Resources/aura/aura_white";
			GameObject gameObject = new GameObject(name);
			PureFlower guna = gameObject.AddComponent<PureFlower>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Radiant Aura";
			string longDesc = "Changes stats based on the aura.\n\nThis flower radiates an aura that affects those nearby in strange ways. The light from this flower changes often. The flower is fragile however.";
			ItemBuilder.SetupItem(guna, shortDesc, longDesc, "kp");
			guna.quality = PickupObject.ItemQuality.A;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000040AF File Offset: 0x000022AF
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.DoAura(player);
			player.OnRoomClearEvent += this.RoomCleared;
	
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000040D8 File Offset: 0x000022D8
		private void DoAura(PlayerController player)
		{
			int num = UnityEngine.Random.Range(0, 4);
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(player.sprite);
		
			bool flag = num == 0;
			if (flag)
			{
				this.AddStat(PlayerStats.StatType.Damage, 0.2f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.RateOfFire, 0.2f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.PlayerBulletScale, 0.2f, StatModifier.ModifyMethod.ADDITIVE);
				player.stats.RecalculateStats(player, false, false);
				outlineMaterial.SetColor("_OverrideColor", new Color(99f, 0f, 0f));
			}
			bool flag2 = num == 1;
			if (flag2)
			{
				HealthHaver healthHaver = player.healthHaver;
				healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.ModifyIncomingDamage));
				player.ReceivesTouchDamage = false;
				player.ImmuneToPits.SetOverride("Guna", true, null);
				this.AddStat(PlayerStats.StatType.DodgeRollSpeedMultiplier, 0.2f, StatModifier.ModifyMethod.ADDITIVE);
				player.stats.RecalculateStats(player, false, false);
				outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 99f));
			}
			bool flag3 = num == 2;
			if (flag3)
			{
				this.AddStat(PlayerStats.StatType.MovementSpeed, 1f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.ProjectileSpeed, 0.2f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.ReloadSpeed, -0.2f, StatModifier.ModifyMethod.ADDITIVE);
				player.stats.RecalculateStats(player, false, false);
				outlineMaterial.SetColor("_OverrideColor", new Color(99f, 99f, 0f));
			}
			bool flag4 = num == 3;
			if (flag4)
			{
				this.AddStat(PlayerStats.StatType.Accuracy, -0.4f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.Coolness, 2f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.MoneyMultiplierFromEnemies, 0.35f, StatModifier.ModifyMethod.ADDITIVE);
				player.stats.RecalculateStats(player, false, false);
				outlineMaterial.SetColor("_OverrideColor", new Color(99f, 99f, 99f));
			}
		}

	
		// Token: 0x06000046 RID: 70 RVA: 0x000042C8 File Offset: 0x000024C8
		private void DisableVFX(PlayerController user)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004302 File Offset: 0x00002502
		private void RoomCleared(PlayerController obj)
		{
			this.RemoveAura(obj);
		   if (obj.HasPickupID(33))
			{
				this.DoAura2(obj);
			}
		   else
			{
				this.DoAura(obj);
			}
				
		}

		private void DoAura2(PlayerController player)
		{
			int num = UnityEngine.Random.Range(0, 4);
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(player.sprite);

			bool flag = num == 0;
			if (flag)
			{
				this.AddStat(PlayerStats.StatType.Damage, 0.35f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.RateOfFire, 0.3f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.PlayerBulletScale, 0.3f, StatModifier.ModifyMethod.ADDITIVE);
				player.stats.RecalculateStats(player, false, false);
				outlineMaterial.SetColor("_OverrideColor", new Color(99f, 0f, 0f));
			}
			bool flag2 = num == 1;
			if (flag2)
			{
				HealthHaver healthHaver = player.healthHaver;
				healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.ModifyIncomingDamage));
				player.ReceivesTouchDamage = false;
				player.ImmuneToPits.SetOverride("Guna", true, null);
				this.AddStat(PlayerStats.StatType.DodgeRollSpeedMultiplier, 0.3f, StatModifier.ModifyMethod.ADDITIVE);
				player.stats.RecalculateStats(player, false, false);
				outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 99f));
			}
			bool flag3 = num == 2;
			if (flag3)
			{
				this.AddStat(PlayerStats.StatType.MovementSpeed, 1.8f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.ProjectileSpeed, 0.3f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.ReloadSpeed, -0.4f, StatModifier.ModifyMethod.ADDITIVE);
				player.stats.RecalculateStats(player, false, false);
				outlineMaterial.SetColor("_OverrideColor", new Color(99f, 99f, 0f));
			}
			bool flag4 = num == 3;
			if (flag4)
			{
				this.AddStat(PlayerStats.StatType.Accuracy, -0.6f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.Coolness, 3f, StatModifier.ModifyMethod.ADDITIVE);
				this.AddStat(PlayerStats.StatType.MoneyMultiplierFromEnemies, 0.45f, StatModifier.ModifyMethod.ADDITIVE);
				player.stats.RecalculateStats(player, false, false);
				outlineMaterial.SetColor("_OverrideColor", new Color(99f, 99f, 99f));
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004318 File Offset: 0x00002518
		private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
		{
			bool flag = args == EventArgs.Empty;
			if (!flag)
			{
				bool flag2 = UnityEngine.Random.value < this.ignoreChance;
				if (flag2)
				{
					source.GetComponent<PlayerController>();
					args.ModifiedDamage = 0f;
				}
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000435C File Offset: 0x0000255C
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

		// Token: 0x0600004B RID: 75 RVA: 0x00004468 File Offset: 0x00002668
		private void RemoveAura(PlayerController user)
		{
			this.RemoveStat(PlayerStats.StatType.Damage);
			this.RemoveStat(PlayerStats.StatType.RateOfFire);
			this.RemoveStat(PlayerStats.StatType.MovementSpeed);
			this.RemoveStat(PlayerStats.StatType.DodgeRollSpeedMultiplier);
			this.RemoveStat(PlayerStats.StatType.ReloadSpeed);
			this.RemoveStat(PlayerStats.StatType.PlayerBulletScale);
			this.RemoveStat(PlayerStats.StatType.MoneyMultiplierFromEnemies);
			this.RemoveStat(PlayerStats.StatType.Accuracy);
			this.RemoveStat(PlayerStats.StatType.RangeMultiplier);
			this.RemoveStat(PlayerStats.StatType.ProjectileSpeed);
			this.RemoveStat(PlayerStats.StatType.Coolness);
			this.ignoreChance = 0f;
			user.ReceivesTouchDamage = true;
			user.ImmuneToPits.SetOverride("Guna", false, null);
			this.DisableVFX(user);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004504 File Offset: 0x00002704
		public override DebrisObject Drop(PlayerController player)
		{
			this.RemoveAura(player);
			player.OnRoomClearEvent -= this.RoomCleared;
			DebrisObject debrisObject = base.Drop(player);
			PureFlower component = debrisObject.GetComponent<PureFlower>();
			component.m_pickedUpThisRun = true;
			UnityEngine.Object.Destroy(base.gameObject, 1f);
			return debrisObject;
		}

		// Token: 0x0400000E RID: 14
		private float ignoreChance = 0.2f;
	}
}
