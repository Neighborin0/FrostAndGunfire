using System;
using System.Collections;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200001E RID: 30
	public class Hotshot : GunBehaviour
	{


		// Token: 0x060000BE RID: 190 RVA: 0x00008300 File Offset: 0x00006500
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Hotshot", "hotshot");
			Game.Items.Rename("outdated_gun_mods:hotshot", "kp:hotshot");
			gun.gameObject.AddComponent<Hotshot>();
			gun.SetShortDescription("Burn Baby, Burn");
			gun.SetLongDescription("A gun formed through the power of fire, this ancient weapon sets foes ablaze. Most can't even touch it without burning.");
			gun.SetupSprite(null, "hotshot_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 14);
			gun.SetAnimationFPS(gun.finalShootAnimation, 14);
			gun.SetAnimationFPS(gun.reloadAnimation, 15);
			gun.AddProjectileModuleFrom("mac10", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.3f;
			gun.DefaultModule.cooldownTime = 0.1f;
			gun.DefaultModule.numberOfShotsInClip = 5;
			gun.DefaultModule.angleVariance = 8f;
			gun.gunHandedness = GunHandedness.OneHanded;
			Gun gun4 = PickupObjectDatabase.GetById(38) as Gun;
			Gun gun1 = PickupObjectDatabase.GetById(336) as Gun;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
			gun.quality = PickupObject.ItemQuality.D;
			gun.SetBaseMaxAmmo(300);
			Guid.NewGuid().ToString();
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			gun.barrelOffset.transform.localPosition += new Vector3(0.2f, 0f, 0f);
			projectile.baseData.damage *= 0.9f;
			projectile.baseData.speed *= 1.01f;
			gun.gunClass = GunClass.FIRE;
			projectile.SetProjectileSpriteRight("hotshot_projectile_001", 7, 7);

			Projectile fireball = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(336) as Gun).DefaultModule.projectiles[0]);
			fireball.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(fireball.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(fireball);
			//fireball.Owner = gun.CurrentOwner;
			//fireball.Shooter = gun.CurrentOwner.specRigidbody;
			fireball.baseData.speed = 20f;
			fireball.baseData.damage *= 0.3f;

			gun.DefaultModule.finalProjectile = fireball;
			gun.DefaultModule.usesOptionalFinalProjectile = true;
			gun.DefaultModule.numberOfFinalProjectiles = 1;
			gun.DefaultModule.finalCustomAmmoType = gun1.DefaultModule.customAmmoType;
			gun.DefaultModule.finalAmmoType = gun1.DefaultModule.ammoType;

			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_WANDERER, true);
			gun.gunHandedness = GunHandedness.OneHanded;


		
		}

	

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
		}
		// Token: 0x060000C0 RID: 192 RVA: 0x00007843 File Offset: 0x00005A43
		public override void OnAutoReload(PlayerController player, Gun gun)
		{
			AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
			//this.DoEffect(player, gun);
		}

		
	


	}
}
