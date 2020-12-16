using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000014 RID: 20
	public class TrashBag : PassiveItem
	{
		

		// Token: 0x06000088 RID: 136 RVA: 0x00006B58 File Offset: 0x00004D58
		public static void Init()
		{
			string name = "Trash Bag";
			string resourcePath = "FrostAndGunfireItems/Resources/trash_bag";
			GameObject gameObject = new GameObject(name);
			TrashBag trashBag = gameObject.AddComponent<TrashBag>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "One Man's Trash...";
			string longDesc = "This trash allows the wearer to carry more items and increases their junk senses. Perhaps you can salavage more things for junk?";
			ItemBuilder.SetupItem(trashBag, shortDesc, longDesc, "kp");
			trashBag.quality = PickupObject.ItemQuality.B;
			ItemBuilder.AddPassiveStatModifier(trashBag, PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000A5D0 File Offset: 0x000087D0
		protected override void Update()
		{
			bool flag = base.Owner;
			if (flag)
			{
				this.CalculateStats(base.Owner);
			}
		}
		public override void Pickup(PlayerController player)
		{
			
			base.Pickup(player);
			player.OnRoomClearEvent += this.RoomCleared;
		

		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006C4C File Offset: 0x00004E4C
		private void RoomCleared(PlayerController obj)
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			bool flag = (double)num < 0.08;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				PickupObject byId = PickupObjectDatabase.GetById(127);
				LootEngine.SpawnItem(byId.gameObject, obj.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
		}

		private void CalculateStats(PlayerController player)
		{
			this.currentItems = player.passiveItems.Count;
			bool flag = this.currentItems != this.lastItems;
			if (flag)
			{
				this.RemoveStat(PlayerStats.StatType.AmmoCapacityMultiplier);
				foreach (PassiveItem passiveItem in player.passiveItems)
				{
					bool flag2 = passiveItem.PickupObjectId == 127;
					if (flag2)
					{
						this.AddStat(PlayerStats.StatType.AmmoCapacityMultiplier, 0.1f, StatModifier.ModifyMethod.ADDITIVE);
					}
					bool flag3 = passiveItem.PickupObjectId == 641;
					if (flag3)
					{
						this.AddStat(PlayerStats.StatType.AmmoCapacityMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);
					}
				}
				this.lastItems = this.currentItems;
				player.stats.RecalculateStats(player, true, false);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000A6C4 File Offset: 0x000088C4
		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = 0)
		{
			StatModifier statModifier = new StatModifier
			{
				amount = amount,
				statToBoost = statType,
				modifyType = method
			};
			bool flag = this.passiveStatModifiers == null;
			if (flag)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier
				};
			}
			else
			{
				this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
				{
					statModifier
				}).ToArray<StatModifier>();
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000A72C File Offset: 0x0000892C
		private void RemoveStat(PlayerStats.StatType statType)
		{
			List<StatModifier> list = new List<StatModifier>();
			for (int i = 0; i < this.passiveStatModifiers.Length; i++)
			{
				bool flag = this.passiveStatModifiers[i].statToBoost != statType;
				if (flag)
				{
					list.Add(this.passiveStatModifiers[i]);
				}
			}
			this.passiveStatModifiers = list.ToArray();
		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			TrashBag component = debrisObject.GetComponent<TrashBag>();
			player.OnRoomClearEvent -= this.RoomCleared;
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		private int currentItems;


		private int lastItems;
	}
}
