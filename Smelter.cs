using System;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000019 RID: 25
	public class Smelter : PlayerItem
	{
	

		// Token: 0x060000A4 RID: 164 RVA: 0x00007860 File Offset: 0x00005A60
		public static void Init()
		{
			string name = "Smelter";
			string resourcePath = "FrostAndGunfireItems/Resources/smelter";
			GameObject gameObject = new GameObject();
			Smelter smelter = gameObject.AddComponent<Smelter>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Stone and Iron";
			string longDesc = "Exchanges blanks for armor.";
			ItemBuilder.SetupItem(smelter, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(smelter, ItemBuilder.CooldownType.Timed, 1f);
			smelter.consumable = false;
			smelter.quality = PickupObject.ItemQuality.D;
			smelter.AddToSubShop(ItemBuilder.ShopType.OldRed);
		}

	

	

		// Token: 0x060000A5 RID: 165 RVA: 0x000078D4 File Offset: 0x00005AD4
		protected override void DoEffect(PlayerController user)
		{
			if (user.HasPickupID(564))
			{
				user.Blanks -= 1;
			}
			else
			{
				user.Blanks -= 2;
			}
			
			AkSoundEngine.PostEvent("Play_TRP_flame_torch_01", base.gameObject);
				user.healthHaver.Armor += 1f;
			
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000793C File Offset: 0x00005B3C
		public override bool CanBeUsed(PlayerController user)
		{
			if (user.HasPickupID(564))
			{
				return user.Blanks > 0;
			}

			else
			{
				return user.Blanks > 1;
			}
		}

	}
}
