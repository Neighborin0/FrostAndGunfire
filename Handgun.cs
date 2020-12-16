using System;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000023 RID: 35
	public class Handgun : GunBehaviour
	{
		//Add Giant Fist Synergy
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Handgun", "handgun");
			Game.Items.Rename("outdated_gun_mods:handgun", "kp:handgun");
			gun.gameObject.AddComponent<Handgun>();
			gun.SetShortDescription("Huge Hands Hans!?");
			gun.SetLongDescription("The severed hand of the Giant that terrorized the local citizens of an underwater city, this gun has his explosive personality in every finger.");
			gun.SetupSprite(null, "handgun_idle_001", 10);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 8);
			gun.AddProjectileModuleFrom("rpg", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
			gun.reloadTime = 0.9f;
			gun.DefaultModule.cooldownTime = 0.7f;
			gun.SetBaseMaxAmmo(100);
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.quality = PickupObject.ItemQuality.S;
			gun.gunHandedness = GunHandedness.TwoHanded;
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			gun.muzzleFlashEffects = null;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.speed *= 1.6f;
			projectile.SetProjectileSpriteRight("handgun_projectile_001", 10, 5);
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
		}


		// Token: 0x060000DA RID: 218 RVA: 0x00009099 File Offset: 0x00007299
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_RPG_shot_01", base.gameObject);
		}


	}
}
