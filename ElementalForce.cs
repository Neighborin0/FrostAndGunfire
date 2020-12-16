using System;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000012 RID: 18
	
		//Adjust hand animations/reload sound
		//Make it an actual shotgun (look at the bootleg shoutgun)
		public class ElementalForce : GunBehaviour
		{
			// Token: 0x0600018E RID: 398 RVA: 0x0000F6FC File Offset: 0x0000D8FC
			public static void Add()
			{
				Gun gun = ETGMod.Databases.Items.NewGun("Elemental Force", "force");
				Game.Items.Rename("outdated_gun_mods:elemental_force", "kp:elemental_force");
				gun.gameObject.AddComponent<ElementalForce>();
				gun.SetShortDescription("Force Jump!");
				gun.SetLongDescription("An old gun, wielded by a speedy mercenary. Knocks enemies back and you with powerful pellets.");
				gun.SetupSprite(null, "force_idle_001", 8);
				gun.SetAnimationFPS(gun.shootAnimation, 14);
				gun.SetAnimationFPS(gun.reloadAnimation, 14);
			for (int i = 0; i < 4; i++)
			{
				gun.AddProjectileModuleFrom("shotgun", true, false);
			}
				gun.DefaultModule.ammoCost = 1;
				gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				gun.DefaultModule.sequenceStyle = 0;
				gun.reloadTime = 0.6f;
				gun.DefaultModule.cooldownTime = 0.316f;
				gun.DefaultModule.numberOfShotsInClip = 2;
				gun.quality = PickupObject.ItemQuality.D;
				gun.gunHandedness = GunHandedness.TwoHanded;
				Gun gun2 = PickupObjectDatabase.GetById(51) as Gun;
				gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
				gun.SetBaseMaxAmmo(440);
				gun.DefaultModule.angleVariance = 12f;
				gun.RawSourceVolley.UsesShotgunStyleVelocityRandomizer = true;
				gun.Volley.UsesShotgunStyleVelocityRandomizer = true;
			foreach (ProjectileModule mod in gun.Volley.projectiles)
			{
				mod.ammoCost = 1;
				mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				mod.burstShotCount = 4;
				mod.burstCooldownTime = 0.2f;
				mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				mod.cooldownTime = 0.316f;
				mod.angleVariance = 15f;
				mod.numberOfShotsInClip = 2;
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(mod.projectiles[0]);
				mod.projectiles[0] = projectile;
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);
				//gun.DefaultModule.projectiles[0] = projectile;
				projectile.transform.parent = gun.barrelOffset;
				projectile.baseData.damage *= 1.3f;
				projectile.baseData.speed *= UnityEngine.Random.Range(0.7f, 1.2f);
				projectile.baseData.force *= 1.8f;
				projectile.AppliesKnockbackToPlayer = true;
				projectile.PlayerKnockbackForce = 10f;
				projectile.baseData.range = 30f;
			}

			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			}
			// Token: 0x06000190 RID: 400 RVA: 0x0000F94C File Offset: 0x0000DB4C
			public override void OnPostFired(PlayerController player, Gun gun)
			{
				gun.PreventNormalFireAudio = true;
				AkSoundEngine.PostEvent("Play_WPN_shotgun_shot_01", base.gameObject);
			}


		}

	
		
	}

