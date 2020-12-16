using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000F RID: 15
	public class PotionOfGunSpeed : PlayerItem
	{
		private float duration = 10f;
		private static int[] spriteIDs;
		private static readonly string[] spritePaths = new string[]
			{
			"FrostAndGunfireItems/Resources/chug",
		"FrostAndGunfireItems/Resources/chug_empty",
	};
		// Token: 0x0600006B RID: 107 RVA: 0x00005D14 File Offset: 0x00003F14
		public static void Init()
		{
			string name = "Potion Of Gun Swiftness";
			string resourcePath = "FrostAndGunfireItems/Resources/chug";
			GameObject gameObject = new GameObject();
			PotionOfGunSpeed potionofgunspeed = gameObject.AddComponent<PotionOfGunSpeed>();
			PotionOfGunSpeed.spriteIDs = new int[PotionOfGunSpeed.spritePaths.Length];
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Super Speedo!";
			string longDesc = "Immensely increases reload speed and doubles fire rate.\n\nOne of Goopton's many potions, the Potion of Gun Swiftness increases the speed of one's gun to insane levels. Don't drink it...";
			ItemBuilder.SetupItem(potionofgunspeed, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(potionofgunspeed, ItemBuilder.CooldownType.Damage, 650f);
			potionofgunspeed.consumable = false;
			potionofgunspeed.quality = PickupObject.ItemQuality.S;
			PotionOfGunSpeed.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(PotionOfGunSpeed.spritePaths[0], potionofgunspeed.sprite.Collection);
			PotionOfGunSpeed.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(PotionOfGunSpeed.spritePaths[1], potionofgunspeed.sprite.Collection);
			potionofgunspeed.AddToSubShop(ItemBuilder.ShopType.Goopton);
		}

	

		
		// Token: 0x0600006D RID: 109 RVA: 0x00005D80 File Offset: 0x00003F80
		protected override void DoEffect(PlayerController user)
		{

			AkSoundEngine.PostEvent("Play_OBJ_dice_bless_01", base.gameObject);
			base.sprite.SetSprite(PotionOfGunSpeed.spriteIDs[1]);
			ApplySuperSpeedo(user, true);
			base.StartCoroutine(this.HandleDuration(user));
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003981 File Offset: 0x00001B81
		private IEnumerator HandleDuration(PlayerController user)
		{
			if (this.IsCurrentlyActive)
			{
				yield break;
			}
			this.IsCurrentlyActive = true;
			this.m_activeElapsed = 0f;
			this.m_activeDuration = this.duration;
			while (this.m_activeElapsed < this.m_activeDuration && this.IsCurrentlyActive)
			{
				
				yield return null;
			}
			ApplySuperSpeedo(user, false);
			this.IsCurrentlyActive = false;
			yield break;
		}

		private void ApplySuperSpeedo(PlayerController user, bool apply)
		{
			bool flag = (apply && base.IsCurrentlyActive) || (!apply && !base.IsCurrentlyActive);
			if (!flag)
			{
				if (apply)
				{
					float value = user.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed) - 999f;
					user.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, value, user);
					float value1 = user.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire) * 1.5f;
					user.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, value1, user);
					float value2 = user.stats.GetBaseStatValue(PlayerStats.StatType.ProjectileSpeed) * 2f;
					user.stats.SetBaseStatValue(PlayerStats.StatType.ProjectileSpeed, value2, user);
					float value3 = user.stats.GetBaseStatValue(PlayerStats.StatType.RangeMultiplier) * 2f;
					user.stats.SetBaseStatValue(PlayerStats.StatType.RangeMultiplier, value3, user);
				}
				else
				{
					float value = user.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed) + 999f;
					user.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, value, user);
					float value1 = user.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire) / 1.5f;
					user.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, value1, user);
					float value2 = user.stats.GetBaseStatValue(PlayerStats.StatType.ProjectileSpeed) / 2f;
					user.stats.SetBaseStatValue(PlayerStats.StatType.ProjectileSpeed, value2, user);
					float value3 = user.stats.GetBaseStatValue(PlayerStats.StatType.RangeMultiplier) / 2f;
					user.stats.SetBaseStatValue(PlayerStats.StatType.RangeMultiplier, value3, user);
				}
			}
		}

		protected override void OnPreDrop(PlayerController user)
		{
			this.ApplySuperSpeedo(user, false);
			base.IsCurrentlyActive = false;
			base.OnPreDrop(user);
		}

		public override bool CanBeUsed(PlayerController user)
		{
			base.sprite.SetSprite(PotionOfGunSpeed.spriteIDs[0]);
			return base.CanBeUsed(user);
		
		}

	}
}
