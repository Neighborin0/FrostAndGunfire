using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class CorruptedRifle : GunBehaviour
	{
		private bool HasReloaded;

	
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Rage Rifle", "corrupted");
			Game.Items.Rename("outdated_gun_mods:rage_rifle", "kp:rage_rifle");
			gun.gameObject.AddComponent<CorruptedRifle>();
			gun.SetShortDescription("Fallen");
			gun.SetLongDescription("More curse, more damage.\n\nA gun made by an enraged blacksmith in Bullet Hell, this gun powers itself through rage and hatred.");
			gun.SetupSprite(null, "corrupted_idle_001", 5);
			gun.SetAnimationFPS(gun.shootAnimation, 16);
			gun.SetAnimationFPS(gun.reloadAnimation, 6);
			Gun gun2 = PickupObjectDatabase.GetById(577) as Gun;
			Gun other = PickupObjectDatabase.GetById(577) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(460);
			gun.DefaultModule.customAmmoType = gun2.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoType = gun2.DefaultModule.ammoType;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.damageModifier = 1;
			gun.reloadTime = 1.4f;
			gun.barrelOffset.transform.localPosition += new Vector3(0f, -0f, 0f);
			gun.DefaultModule.cooldownTime = 0.1f;
			gun.DefaultModule.numberOfShotsInClip = 20;
			gun.DefaultModule.angleVariance = 7f;
			gun.quality = PickupObject.ItemQuality.B;
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.RIFLE;
			gun.CanBeDropped = true;
			Gun gun3 = PickupObjectDatabase.GetById(61) as Gun;
			gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun2.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.AdditionalScaleMultiplier *= 1.1f;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
			ItemBuilder.AddPassiveStatModifier(gun, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
		}

		
	

		protected void Update()
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
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

		public override void PostProcessProjectile(Projectile projectile)
		{
			float value = UnityEngine.Random.value;
			PlayerController x = gun.CurrentOwner as PlayerController;
			if (x != null)
			{
				if (x.HasPickupID(323) && (double)value < 1)
				{
					projectile.baseData.damage *= 0.8f + (PlayerStats.GetTotalCurse() * 0.10f);
					projectile.AdditionalScaleMultiplier *= 1.3f;
					projectile.DefaultTintColor = Color.magenta;
					projectile.HasDefaultTint = true;

				}
				else
				{
					projectile.baseData.damage *= 0.4f + (PlayerStats.GetTotalCurse() * 0.10f);
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
				AkSoundEngine.PostEvent("Play_WPN_Life_Orb_Fade_01", base.gameObject);
			}
		}
		

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_spacerifle_shot_01", base.gameObject);
		
			
		}

		
	
	}
}
