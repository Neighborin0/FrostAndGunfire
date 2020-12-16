
using ItemAPI;
using System;

using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000017 RID: 23
	public class EmergencySupplies : PlayerItem
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00007537 File Offset: 0x00005737
		public EmergencySupplies()
		{
			this.spawnItemId = 73;
			this.spawnItemId = 78;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00007554 File Offset: 0x00005754
		public static void Init()
		{
			string name = "Emergency Supplies";
			string resourcePath = "FrostAndGunfireItems/Resources/supplies";
			GameObject gameObject = new GameObject();
			EmergencySupplies emergencySupplies = gameObject.AddComponent<EmergencySupplies>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Just In Time";
			string longDesc = "An old bag full of supplies!\n\nLeft by whom though?";
			ItemBuilder.SetupItem(emergencySupplies, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(emergencySupplies, ItemBuilder.CooldownType.Damage, 1000f);
			emergencySupplies.consumable = true;
			emergencySupplies.quality = PickupObject.ItemQuality.D;
			emergencySupplies.AddToSubShop(ItemBuilder.ShopType.Trorc);
		}


		
		protected override void DoEffect(PlayerController user)
		{
			float currentHealth = user.healthHaver.GetCurrentHealth();
				PickupObject byId = PickupObjectDatabase.GetById(this.spawnItemId);
			LootEngine.SpawnItem(byId.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			user.healthHaver.Armor += 1f;
			user.healthHaver.ForceSetCurrentHealth(currentHealth + 0.5f);
			PickupObject byId2 = PickupObjectDatabase.GetById(565);
			LootEngine.SpawnItem(byId2.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			PickupObject byId4 = PickupObjectDatabase.GetById(67);
			LootEngine.SpawnItem(byId4.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			PickupObject byId3 = PickupObjectDatabase.GetById(224);
			LootEngine.SpawnItem(byId3.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
		}
	

		// Token: 0x04000038 RID: 56
		public int spawnItemId;

	}
}
