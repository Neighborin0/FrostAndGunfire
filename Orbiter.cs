using System;
using Gungeon;
using UnityEngine;
using ItemAPI;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	
	public class Orbiter : GunBehaviour
	{
		
	
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Orbiter", "orbiter");
			Game.Items.Rename("outdated_gun_mods:orbiter", "kp:orbiter");
			gun.gameObject.AddComponent<Orbiter>();
			gun.SetShortDescription("The Gungeon Revolves Around Me");
			gun.SetLongDescription("An early prototype of the Mr. Accretion Jr., the Orbiter's main purpose was to test the orbital capacities of guns.");
			gun.SetupSprite(null, "orbiter_idle_001", 10);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.AddProjectileModuleFrom("ak-47", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;
			gun.DefaultModule.cooldownTime = 0.25f;
			gun.DefaultModule.numberOfShotsInClip = 400;
			gun.quality = PickupObject.ItemQuality.S;
			gun.gunHandedness = GunHandedness.NoHanded;
			gun.preventRotation = true;
			gun.muzzleFlashEffects = null;
			gun.SetBaseMaxAmmo(300);
			Guid.NewGuid().ToString();
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage *= 1.3f;
			projectile.baseData.speed *= 1f;
			projectile.baseData.range = 0.3f;
			projectile.transform.parent = gun.barrelOffset;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.LICH_KILLED_AND_PILOT, true);
		}

		
		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			bool flag = x == null;
			if (flag)
			{
				this.gun.ammo = this.gun.GetBaseMaxAmmo();
			}
			if(x.HasPickupID(661))
			{
				projectile.baseData.speed *= 2f;
			}
			float value = UnityEngine.Random.value;
			if (x.HasPickupID(597) && (double)value < 0.10)
			{
				projectile = null;
				Projectile projectile2 = ((Gun)ETGMod.Databases.Items[597]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
				Projectile component1 = gameObject.GetComponent<Projectile>();
				if (component1 != null)
				{
					component1.collidesWithPlayer = false;
					component1.Owner = gun.CurrentOwner;

				}
			}

			BounceProjModifier bounceProjModifier = projectile.gameObject.AddComponent<BounceProjModifier>();
			PierceProjModifier pierceProjModifier = projectile.gameObject.AddComponent<PierceProjModifier>();
			pierceProjModifier.penetration = 10;
			bounceProjModifier.projectile.specRigidbody.CollideWithTileMap = false;
			bounceProjModifier.projectile.ResetDistance();
			bounceProjModifier.projectile.baseData.range = Mathf.Max(bounceProjModifier.projectile.baseData.range, 500f);
			bool flag3 = bounceProjModifier.projectile.baseData.speed > 50f;
			if (flag3)
			{
				bounceProjModifier.projectile.baseData.speed = 20f;
				bounceProjModifier.projectile.UpdateSpeed();
			}
			OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
			orbitProjectileMotionModule.lifespan = 15f;
			orbitProjectileMotionModule.MaxRadius = 9f;
			orbitProjectileMotionModule.MinRadius = 3f;
			bool flag4 = bounceProjModifier.projectile.OverrideMotionModule != null && bounceProjModifier.projectile.OverrideMotionModule is HelixProjectileMotionModule;
			if (flag4)
			{ 
				orbitProjectileMotionModule.StackHelix = true;
				orbitProjectileMotionModule.ForceInvert = (bounceProjModifier.projectile.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
			}
			bounceProjModifier.projectile.OverrideMotionModule = orbitProjectileMotionModule;
			base.PostProcessProjectile(projectile);
		}

		
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_spacerifle_shot_01", base.gameObject);
		}

		
		
	}
}
