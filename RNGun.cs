using System;
using Gungeon;
using ItemAPI;
using GungeonAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000009 RID: 9
	public class RNGun : GunBehaviour
	{
		private bool HasReloaded;

		// Token: 0x0600003D RID: 61 RVA: 0x00003DF0 File Offset: 0x00001FF0
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("R.N.GUN", "rngun");
			Game.Items.Rename("outdated_gun_mods:r.n.gun", "kp:r.n.gun");
			gun.gameObject.AddComponent<RNGun>();
			gun.SetShortDescription("Randomly Generated");
			gun.SetLongDescription("A gun as unpredictable as the Gungeon, the random bullet generator in this gun allows this gun to fire randomized rounds.");
			gun.SetupSprite(null, "rngun_idle_001", 7);
			gun.SetAnimationFPS(gun.shootAnimation, 15);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);
			gun.AddProjectileModuleFrom("ak-47", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.reloadTime = 1.1f;
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.DefaultModule.positionOffset = new Vector3((int)0.5, 0, 0);
			gun.SetBaseMaxAmmo(200);
			gun.DefaultModule.cooldownTime = 0.4f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.quality = PickupObject.ItemQuality.C;
			///how to get a gun's muzzle flash
			Gun gun4 = PickupObjectDatabase.GetById(38) as Gun;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
			Guid.NewGuid().ToString();
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController x = gun.CurrentOwner as PlayerController;
			if (x.HasPickupID(PickupObjectDatabase.GetByEncounterName("Crystal Die").PickupObjectId))
			{
				projectile.baseData.damage *= 0.2f * (float)UnityEngine.Random.Range(5, 25);
				projectile.baseData.speed *= 0.55f * (float)UnityEngine.Random.Range(2, 10);
				projectile.baseData.force += (float)UnityEngine.Random.Range(1, 17);
				projectile.AdditionalScaleMultiplier *= 0.5f * (float)UnityEngine.Random.Range(1, 25);
			}
			else
			{
				projectile.baseData.damage *= 0.2f * (float)UnityEngine.Random.Range(5, 20);
				projectile.baseData.speed *= 0.55f * (float)UnityEngine.Random.Range(2, 5);
				projectile.baseData.force += (float)UnityEngine.Random.Range(1, 13);
				projectile.AdditionalScaleMultiplier *= 0.5f * (float)UnityEngine.Random.Range(1, 20);
			}
		}
		protected void Update()
		{
			bool flag = this.gun.CurrentOwner;
			if (flag)
			{
				bool flag2 = !this.gun.PreventNormalFireAudio;
				if (flag2)
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
	
		// Token: 0x0600003F RID: 63 RVA: 0x00003FC0 File Offset: 0x000021C0
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
		}

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Slots_01", base.gameObject);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003FF0 File Offset: 0x000021F0
		private void LateUpdate()
		{
			int numberOfShotsInClip = UnityEngine.Random.Range(1, 15);
			bool flag = this.gun && this.gun.IsReloading && this.gun.CurrentOwner is PlayerController;
			if (flag)
			{
		
				this.gun.DefaultModule.numberOfShotsInClip = numberOfShotsInClip;
			}
		}
	}
}
