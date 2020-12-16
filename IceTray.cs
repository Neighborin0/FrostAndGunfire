
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GungeonAPI;
namespace FrostAndGunfireItems
{
	
	public class IceTray : PassiveItem
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00004058 File Offset: 0x00002258
		public static void Init()
		{
			string name = "Ice Tray";
			string resourcePath = "FrostAndGunfireItems/Resources/ice_tray";
			GameObject gameObject = new GameObject(name);
			IceTray iceTray = gameObject.AddComponent<IceTray>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Ice Ice Baby";
			string longDesc = "Upon clearing a room there is a chance that a piece of ice will spawn.\n\nAn item that can be found in Fridge Maidens everywhere, this ice tray is able to produce it's own ice cubes. Perfect for a refreshing day at the Breach.";
			ItemBuilder.SetupItem(iceTray, shortDesc, longDesc, "kp");
			ItemBuilder.AddPassiveStatModifier(iceTray, PlayerStats.StatType.Coolness, 3f, StatModifier.ModifyMethod.ADDITIVE);
			iceTray.quality = PickupObject.ItemQuality.S;
			iceTray.SetupUnlockOnCustomFlag(CustomDungeonFlags.LICH_KILLED_AND_WANDERER, true);
			iceTray.AddToSubShop(ItemBuilder.ShopType.OldRed, 2000);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000040AF File Offset: 0x000022AF
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnRoomClearEvent += this.RoomCleared;
			foreach (Gun gun in base.Owner.inventory.AllGuns)
			{
				bool flag = base.Owner.inventory.ContainsGun(130);
				if (flag)
				{
					bool flag2 = gun.PickupObjectId.Equals(130);
					if (flag2)
					{
						
						gun.SetBaseMaxAmmo(150);
					}
				}
			}
		}

		protected override void Update()
		{
			bool flag = base.Owner;
			if (flag)
			{
				foreach (Gun gun in base.Owner.inventory.AllGuns)
				{
					bool flag2 = base.Owner.inventory.ContainsGun(130);
					if (flag2)
					{
						bool flag3 = gun.PickupObjectId.Equals(130);
						if (flag3)
						{
							bool flag5 = gun.AdjustedMaxAmmo != 150;
							if (flag5)
							{
								gun.SetBaseMaxAmmo(150);
							}
						}
					}
				}
			}
		}


	

		private void RoomCleared(PlayerController obj)
		{
			float value = UnityEngine.Random.value;
			if ((double)value < 0.5)
			{
				PickupObject byId = PickupObjectDatabase.GetById(ETGMod.Databases.Items["Ice"].PickupObjectId);
				LootEngine.SpawnItem(byId.gameObject, obj.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004504 File Offset: 0x00002704
		public override DebrisObject Drop(PlayerController player)
		{
			foreach (Gun gun in base.Owner.inventory.AllGuns)
			{
				bool flag = base.Owner.inventory.ContainsGun(130);
				if (flag)
				{
					bool flag2 = gun.PickupObjectId.Equals(130);
					if (flag2)
					{
						Gun gun2 = PickupObjectDatabase.GetById(130) as Gun;
						int adjustedMaxAmmo = gun2.AdjustedMaxAmmo;
						gun.DefaultModule.numberOfShotsInClip = gun2.DefaultModule.numberOfShotsInClip;
						gun.SetBaseMaxAmmo(adjustedMaxAmmo);
					}
				}
			}
			player.OnRoomClearEvent -= this.RoomCleared;
			DebrisObject debrisObject = base.Drop(player);
			IceTray component = debrisObject.GetComponent<IceTray>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

	
	}
}
