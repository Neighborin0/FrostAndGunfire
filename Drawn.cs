using System;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class Drawn : GunBehaviour
	{

		private bool HasReloaded;
		//Adjust reload sound

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Drawn", "drawn");
			Game.Items.Rename("outdated_gun_mods:drawn", "kp:drawn");
			gun.gameObject.AddComponent<Drawn>();
			gun.SetShortDescription("madee bye me ._.");
			gun.SetLongDescription("A poorly drawn gun made by a young child, this gun's ink slows anyone who comes in contact with it.");
			gun.SetupSprite(null, "drawn_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 16);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(816) as Gun, true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.5f;
			gun.SetBaseMaxAmmo(200);
			gun.DefaultModule.cooldownTime = 0.4f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 7;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.DefaultModule.positionOffset = new Vector3(0, 0, 0);
			Gun gun4 = PickupObjectDatabase.GetById(477) as Gun;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage *= 0.9f;
			projectile.baseData.speed *= 1.3f;
			projectile.baseData.force *= 1.15f;
			projectile.AppliesStun = false;
			projectile.speedEffect.duration = 1f;
			projectile.SetProjectileSpriteRight("drawn_projectile_001", 16, 8);
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BULLET;
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController x = gun.CurrentOwner as PlayerController;
			if (x != null)
			{
				float value = UnityEngine.Random.value;
				if (x.HasPickupID(477) && (double)value < 0.30)
				{
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[477]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
					}
				}

			}
		}

		protected void Update()
		{

			if (gun.CurrentOwner)
			{

				if (!gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag3 = !this.gun.IsReloading && !this.HasReloaded;
				if (flag3)
				{
					this.HasReloaded = true;
				}
			}
		}

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_pillow_reload_01", base.gameObject);
			}
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_pollster_shot_01", base.gameObject);
		}


	
	}
}
