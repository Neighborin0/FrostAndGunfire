using System;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000F RID: 15
	public class PocketPistol : PlayerItem
	{
		private static int[] spriteIDs;
		private static readonly string[] spritePaths = new string[]
			{
			"FrostAndGunfireItems/Resources/pocket_pistol",
		"FrostAndGunfireItems/Resources/pocket_shotgun",
	};
		public static void Init()
		{
			string name = "Pocket Pistol";
			string resourcePath = "FrostAndGunfireItems/Resources/pocket_pistol";
			GameObject gameObject = new GameObject();
			PocketPistol pocketPistol = gameObject.AddComponent<PocketPistol>();
			PocketPistol.spriteIDs = new int[PocketPistol.spritePaths.Length];
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Mini Gun";
			string longDesc = "A tiny pistol that is far too small to wield. Due to it's small size, the weapons has to go on cooldown often, making it an unreliable weapon in combat.";
			ItemBuilder.SetupItem(pocketPistol, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(pocketPistol, ItemBuilder.CooldownType.Timed, 0.3f);
			pocketPistol.consumable = true;
			pocketPistol.numberOfUses = 150;
			pocketPistol.quality = PickupObject.ItemQuality.D;
			pocketPistol.AddToSubShop(ItemBuilder.ShopType.Trorc);
			PocketPistol.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(PocketPistol.spritePaths[0], pocketPistol.sprite.Collection);
			PocketPistol.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(PocketPistol.spritePaths[1], pocketPistol.sprite.Collection);
		}

		private void LateUpdate()
		{
			PlayerController player = LastOwner;
			bool flag = player;
			bool flag2 = flag;
			if (flag2)
			{
				if (player.HasPickupID(601))
				{
					base.sprite.SetSprite(PocketPistol.spriteIDs[1]);
				}
				else
				{
					base.sprite.SetSprite(PocketPistol.spriteIDs[0]);
				}
			}
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005D80 File Offset: 0x00003F80
		protected override void DoEffect(PlayerController user)
		{
			if (user.HasPickupID(PickupObjectDatabase.GetByEncounterName("Elemental Force").PickupObjectId))
			{
				float value = UnityEngine.Random.value;
				if ((double)value < 0.50)
				{
					base.numberOfUses += 2;
				}
				
			}
				AkSoundEngine.PostEvent("Play_WPN_Gun_Shot_01", base.gameObject);
			Projectile projectile2 = ((Gun)ETGMod.Databases.Items["ak-47"]).DefaultModule.projectiles[0];
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
			if (user.HasPickupID(601))
			{
				GameObject gameObject1 = SpawnManager.SpawnProjectile(projectile2.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle + 3),true);
				GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle + 2), true);
				GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile2.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle - 2), true);
				Projectile component1 = gameObject1.GetComponent<Projectile>();
				Projectile component2 = gameObject2.GetComponent<Projectile>();
				Projectile component3 = gameObject3.GetComponent<Projectile>();
				if (component1 != null)
				{
					component1.Owner = user;
					component1.AdditionalScaleMultiplier *= 0.5f;
					component1.baseData.damage /= 2f;
					component1.collidesWithPlayer = false;
				}
				if (component2 != null)
				{
					component2.Owner = user;
					component2.AdditionalScaleMultiplier *= 0.5f;
					component2.baseData.damage /= 2f;
					component2.collidesWithPlayer = false;
				}
				if (component3 != null)
				{
					component3.Owner = user;
					component3.AdditionalScaleMultiplier *= 0.5f;
					component3.baseData.damage /= 2f;
					component3.collidesWithPlayer = false;
				}
			}
			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag3 = component != null;
			bool flag4 = flag3;
			if (flag4)
			{
				component.Owner = user;
					component.AdditionalScaleMultiplier *= 0.5f;
				component.baseData.damage = 1f;
				component.collidesWithPlayer = false;
			}
			
		}

	
	}
}
