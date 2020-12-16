using System;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class Core : GunBehaviour
	{
		private bool HasReloaded;
		



		public static void Add()
		{



			Gun gun2 = PickupObjectDatabase.GetById(748) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(384) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("The Core", "core");
			Game.Items.Rename("outdated_gun_mods:the_core", "kp:the_core");
			gun.gameObject.AddComponent<Core>();
			gun.SetShortDescription("Condensed Star");
			gun.SetLongDescription("A gun powered by a red dwarf, this gun fires molten rock at unfortunate gundead.");
			gun.SetupSprite(null, "core_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			Gun other = PickupObjectDatabase.GetById(94) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(420);
			gun.DefaultModule.customAmmoType = gun2.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoType = gun2.DefaultModule.ammoType;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
			gun.DefaultModule.burstShotCount = 3;
			gun.DefaultModule.burstCooldownTime = 0.1f;
			gun.damageModifier = 1;
			gun.reloadTime = 1.3f;
			gun.barrelOffset.transform.localPosition += new Vector3(0.22f, 0.02f, 0f);
			gun.DefaultModule.cooldownTime = 0.24f;
			gun.DefaultModule.numberOfShotsInClip = 18;
			gun.quality = PickupObject.ItemQuality.C;
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.PISTOL;
			gun.CanBeDropped = true;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.hitEffects = gun4.DefaultModule.projectiles[0].hitEffects;
			projectile.baseData.damage *= 0.86f;
			projectile.baseData.speed *= 1.08f;
			projectile.baseData.force *= 0.6f;
			projectile.AppliesFire = true;
			projectile.FireApplyChance = 1f;
			projectile.SetProjectileSpriteRight("core_projectile_001", 15, 10);
			ETGMod.Databases.Items.Add(gun, null, "ANY");
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
				AkSoundEngine.PostEvent("Play_OBJ_blackhole_close_01", base.gameObject);
			}
		}
		public override void PostProcessProjectile(Projectile projectile)
		{
			Gun gun4 = PickupObjectDatabase.GetById(384) as Gun;
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			if (x.HasPickupID(372) || x.HasPickupID(398) || x.HasPickupID(113))
			{
				projectile.baseData.speed *= 2f;
				projectile.baseData.damage *= 1.36f;
			}
	
		
		}
		

			public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_TRP_flame_torch_01", base.gameObject);
		}

	
	}
}
