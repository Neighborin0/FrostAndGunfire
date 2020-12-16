using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class Graviton : GunBehaviour
	{
		private bool HasReloaded;
		
		
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Graviton", "graviton");
			Game.Items.Rename("outdated_gun_mods:graviton", "kp:graviton");
			gun.gameObject.AddComponent<Graviton>();
			gun.SetShortDescription("Never Let Me Down");
			gun.SetLongDescription("The bullets fired from this pistol are connected to a core hidden with this weapon. Reloading seems to excite the bullets, making them far faster and deadlier.");
			gun.SetupSprite(null, "graviton_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 8);
			Gun gun2 = PickupObjectDatabase.GetById(15) as Gun;
			Gun other = PickupObjectDatabase.GetById(15) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(321);
			gun.DefaultModule.customAmmoType = gun2.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoType = gun2.DefaultModule.ammoType;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.damageModifier = 1;
			gun.reloadTime = 0.85f;
			gun.barrelOffset.transform.localPosition += new Vector3(0.6f, -0f, 0f);
			gun.DefaultModule.cooldownTime = 0.3f;
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.quality = PickupObject.ItemQuality.B;
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.PISTOL;
			gun.CanBeDropped = true;
			Gun gun3 = PickupObjectDatabase.GetById(15) as Gun;
			gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun2.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.AdditionalScaleMultiplier *= 1f;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
			projectile.baseData.damage *= 1.8f;
			projectile.baseData.speed = 0.1f;
			projectile.AdditionalScaleMultiplier *= 1.2f;
			projectile.DefaultTintColor = Color.cyan;
			projectile.HasDefaultTint = true;
		}


		protected void Update(Projectile projectile)
		{
			PlayerController playerController = this.gun.CurrentOwner as PlayerController;
			bool flag = playerController && playerController != null;
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
			}
		}

	

		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			bool flag = x == null;
			if (flag)
			{
				this.gun.ammo = this.gun.GetBaseMaxAmmo();
			}
			this.gun.DefaultModule.ammoCost = 1;
			base.StartCoroutine(this.TimedDestroy(projectile));
		}

	

		// Token: 0x0600000B RID: 11 RVA: 0x000023A1 File Offset: 0x000005A1
		public IEnumerator TimedDestroy(Projectile projectile)
		{
			bool flag = this.gun.CurrentOwner;
			if (flag)
			{
				yield return new WaitForSeconds(0.3f);

				bool flag2 = gun.IsReloading;
				if (!flag2)
				{
					base.StartCoroutine(this.TimedDestroy(projectile));
				}
				else
				{
					AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", base.gameObject);
					projectile.baseData.speed = 35f;
					projectile.UpdateSpeed();
					
				}
				
			}	
			yield break;
			}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_spacerifle_shot_01", base.gameObject);
		}

	
	}

}


