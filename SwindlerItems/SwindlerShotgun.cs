using System;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{

		public class SwindlersShotgun : GunBehaviour
		{
			// Token: 0x0600018E RID: 398 RVA: 0x0000F6FC File Offset: 0x0000D8FC
			public static void Add()
			{
				Gun gun = ETGMod.Databases.Items.NewGun("Swindler's Shotgun", "swindle");
				Game.Items.Rename("outdated_gun_mods:swindler's_shotgun", "kp:swindler's_shotgun");
				gun.gameObject.AddComponent<SwindlersShotgun>();
				gun.SetShortDescription("Cheapest Money Can Buy");
				gun.SetLongDescription("A shotgun quickly bought by the Swindler on his way to the Gungeon. It's very poor quality, but it will do.");
				gun.SetupSprite(null, "swindle_idle_001", 8);
			    gun.InfiniteAmmo = true;
				gun.SetAnimationFPS(gun.shootAnimation, 14);
				gun.SetAnimationFPS(gun.reloadAnimation, 14);
			for (int i = 0; i < 4; i++)
			{
				gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(202) as Gun, true, false);
			}
				gun.DefaultModule.ammoCost = 1;
				gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				gun.DefaultModule.sequenceStyle = 0;
				gun.reloadTime = 0.9f;
				gun.DefaultModule.cooldownTime = 0.9f;
				gun.DefaultModule.numberOfShotsInClip = 2;
				gun.quality = PickupObject.ItemQuality.EXCLUDED;
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
				projectile.transform.parent = gun.barrelOffset;
				projectile.baseData.damage *= 1f;
				projectile.baseData.speed *= UnityEngine.Random.Range(0.7f, 0.9f);
			}
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			}
			// Token: 0x06000190 RID: 400 RVA: 0x0000F94C File Offset: 0x0000DB4C
			public override void OnPostFired(PlayerController player, Gun gun)
			{
				gun.PreventNormalFireAudio = true;
				AkSoundEngine.PostEvent("Play_WPN_shotgun_shot_01", base.gameObject);
			}


		}

	
		
	}

