using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000015 RID: 21
	public class BloodiedKey : PlayerItem
	{
		//Description and sound effect on use.
		public static void Init()
		{
			string name = "Bloodied Key";
			string resourcePath = "FrostAndGunfireItems/Resources/blood_key";
			GameObject gameObject = new GameObject(name);
			BloodiedKey item = gameObject.AddComponent<BloodiedKey>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Open Your Heart";
			string longDesc = "Allows the user to open chests using hearts.\n\nTales from the Gungeon speak of a gungeoneer known as the Master Of Unlocking, who had met an untimely demise attempting to unlock a mimic. The Gungeoneer's legacy lives on through those who wield his key.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.C;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 0f);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
			item.AddToSubShop(ItemBuilder.ShopType.Flynt);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006E70 File Offset: 0x00005070
		protected override void DoEffect(PlayerController player)
		{
			AkSoundEngine.PostEvent("Play_BOSS_blobulord_burst_01", base.gameObject);
			PlayableCharacters characterIdentity = player.characterIdentity;
			float health = player.healthHaver.GetCurrentHealth();
			IPlayerInteractable nearestInteractable = player.CurrentRoom.GetNearestInteractable(player.CenterPosition, 1f, player);
			if(nearestInteractable is Chest)
			{
				Chest chest = nearestInteractable as Chest;
				if(chest.IsLocked)
				{
					bool flag = characterIdentity != PlayableCharacters.Robot;
					if (flag)
					{
						chest.ForceOpen(player);
						player.healthHaver.ForceSetCurrentHealth(health - 1.5f);
					}
					else
					{
						bool flag2 = characterIdentity == PlayableCharacters.Robot;
						if (flag2)
						{
							chest.ForceOpen(player);
							player.healthHaver.Armor -= 2;
						}
					}
				
					
				}
			}
		
		}

		public override bool CanBeUsed(PlayerController user)
		{
			IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
			Chest chest = nearestInteractable as Chest;
			PlayableCharacters characterIdentity = user.characterIdentity;
			bool flag = characterIdentity == PlayableCharacters.Robot;
			bool result;
			if (flag)
			{
				result = user.healthHaver.Armor > 2f && nearestInteractable is Chest && chest.IsLocked;
			}
			else
			{
				result = user.healthHaver.GetCurrentHealth() > 1.5f && nearestInteractable is Chest && chest.IsLocked;
			}
			return result;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00006EF4 File Offset: 0x000050F4

		// Token: 0x06000093 RID: 147 RVA: 0x000070AC File Offset: 0x000052AC


	}
}
