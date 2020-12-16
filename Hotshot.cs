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
			gun.SetAnimationFPS(gun.reloadAnimation, 27);
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
			projectile.baseData.damage *= 0.9f;
			projectile.baseData.speed *= 1.01f;
			projectile.SetProjectileSpriteRight("hotshot_projectile_001", 7, 7);
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
			AkSoundEngine.PostEvent("Play_WPN_burninghand_shot_01", base.gameObject);
			this.DoEffect(player, gun);
		}

		private void DoEffect(PlayerController user, Gun gun)
		{
			bool flag = this.gun.CurrentOwner;
			if (flag)
			{
				Projectile projectile2 = ((Gun)ETGMod.Databases.Items[336]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag3 = component != null;
				bool flag4 = flag3;
				if (flag4)
				{
					component.Owner = user;
					component.Shooter = user.specRigidbody;
					component.baseData.speed = 20f;
					component.baseData.damage *= 0.3f;
					
				}
			}
		}
	


	}
}
