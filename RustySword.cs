using System;
using Gungeon;
using UnityEngine;
using ItemAPI;

namespace FrostAndGunfireItems
{
	
	public class RustySword : GunBehaviour
	{
		private bool HasReloaded;
		//Add synergies in a dedicated update
	
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Rusty Sword", "build");
			Game.Items.Rename("outdated_gun_mods:rusty_sword", "kp:rusty_sword");
			gun.gameObject.AddComponent<RustySword>();
			gun.SetShortDescription("Trusty, Not Safe");
			gun.SetLongDescription("A well worn sword from far away.\n\nAs melee weapons are rejected in Gungeon, this thing is far from being safe.");
			gun.SetupSprite(null, "build_idle_001", 1);
			gun.SetAnimationFPS(gun.shootAnimation, 8);
			gun.SetAnimationFPS(gun.idleAnimation, 1);
			gun.SetAnimationFPS(gun.reloadAnimation, 1);
			gun.AddProjectileModuleFrom("wonderboy", true, false);
			gun.SetBaseMaxAmmo(100);
			gun.reloadTime = 0f;
			gun.DefaultModule.cooldownTime = 0.1f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 0;
			gun.quality = PickupObject.ItemQuality.SPECIAL;
			gun.encounterTrackable.EncounterGuid = "the_nail";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.CanBeDropped = false;
			gun.InfiniteAmmo = true;
			Gun gun2 = (Gun)ETGMod.Databases.Items["wonderboy"];
			Gun gun3 = gun;
			gun3.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun3.PostProcessProjectile, gun2.PostProcessProjectile);
			Gun gun4 = gun;
			gun4.OnPostFired = (Action<PlayerController, Gun>)Delegate.Combine(gun4.OnPostFired, gun2.OnPostFired);
			Gun gun5 = gun;
			gun5.OnFinishAttack = (Action<PlayerController, Gun>)Delegate.Combine(gun5.OnFinishAttack, gun2.OnFinishAttack);
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.IsHeroSword = gun2.IsHeroSword;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.range = 4;
			gun.HeroSwordDoesntBlank = gun2.HeroSwordDoesntBlank;
			gun.DefaultModule.GetCurrentProjectile().baseData.damage = 20f;
			gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
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
				AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
			}
		}
	
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
		}

		
	}
}
