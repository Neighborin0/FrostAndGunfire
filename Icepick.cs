using System;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000026 RID: 38
	public class Icepick : GunBehaviour
	{
		private bool HasReloaded;
		public static int IcePickID;

		// Token: 0x060000E3 RID: 227 RVA: 0x00009484 File Offset: 0x00007684
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Icepick", "icepick");
			Game.Items.Rename("outdated_gun_mods:icepick", "kp:icepick");
			gun.gameObject.AddComponent<Icepick>();
			gun.SetShortDescription("Bone Chilling");
			gun.SetLongDescription("A gun formed through the power of frost, this ancient weapon pierces foes with a frozen might. Most can't even touch it without freezing.");
			gun.SetupSprite(null, "icepick_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.AddProjectileModuleFrom("blasphemy", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.3f;
			gun.DefaultModule.cooldownTime = 0.31f;
			gun.DefaultModule.numberOfShotsInClip = 7;
			gun.gunHandedness = GunHandedness.OneHanded;
			Gun gun4 = PickupObjectDatabase.GetById(223) as Gun;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
			gun.quality = PickupObject.ItemQuality.D;
			gun.SetBaseMaxAmmo(250);
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage *= 0.30f;
			projectile.baseData.speed *= 0.7f;
			gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_WANDERER, true);
			projectile.SetProjectileSpriteRight("icepick_projectile_001", 18, 10);
			IcePickID = gun.PickupObjectId;
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

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_OBJ_cryo_freeze_01", base.gameObject);
			}
		}
		// Token: 0x060000E4 RID: 228 RVA: 0x0000958C File Offset: 0x0000778C
	
		// Token: 0x060000E5 RID: 229 RVA: 0x00007ADA File Offset: 0x00005CDA
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_ENM_iceslime_shot_01", base.gameObject);
		}

	}
}
