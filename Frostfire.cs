using System;
using Gungeon;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000013 RID: 19
	public class Frostfire : GunBehaviour
	{
		// Complete rework
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Frostfire", "frostfire");
			Game.Items.Rename("outdated_gun_mods:frostfire", "kp:frostfire");
			gun.gameObject.AddComponent<Frostfire>();
			gun.SetShortDescription("Paradox");
			gun.SetLongDescription("A gun formed through the power of frost and fire, this gun both freeze and burns enemies.");
			gun.SetupSprite(null, "frostfire_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.AddProjectileModuleFrom("cold_45", true, false);
			gun.gunHandedness = GunHandedness.TwoHanded;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.positionOffset = new Vector3(1, 0, 0);
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
			gun.reloadTime = 1f;
			Gun gun2 = PickupObjectDatabase.GetById(377) as Gun;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.DefaultModule.cooldownTime = 0.1f;
			gun.DefaultModule.numberOfShotsInClip = 14;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.SetBaseMaxAmmo(300);
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005828 File Offset: 0x00003A28
		public override void PostProcessProjectile(Projectile projectile)
		{
			projectile.DefaultTintColor = Color.cyan;
			projectile.HasDefaultTint = true; 
			projectile.baseData.damage *= 1.2f;
			projectile.baseData.speed *= 0.93f;
			projectile.AdditionalScaleMultiplier = 1.50f;
			projectile.AppliesFire = true;
			projectile.FireApplyChance = 0.99f;
			projectile.fireEffect.AffectsEnemies = true;
			base.PostProcessProjectile(projectile);
			projectile.gameObject.AddComponent<Frostfire.FrostfireProjectile>();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00005896 File Offset: 0x00003A96
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_ENM_iceslime_shot_01", base.gameObject);
		}

		// Token: 0x02000052 RID: 82
		internal class FrostfireProjectile : MonoBehaviour
		{
			// Token: 0x0600021E RID: 542 RVA: 0x00013370 File Offset: 0x00011570
			public void Start()
			{
				Gun gun2 = PickupObjectDatabase.GetById(748) as Gun;
				this.projectile = base.gameObject.GetComponent<Projectile>();
				this.player = (this.projectile.Owner as PlayerController);
				this.projectile.sprite = gun2.projectile.sprite;
			
			}

			// Token: 0x040000EA RID: 234
			private Projectile projectile;

			// Token: 0x040000EB RID: 235
			private PlayerController player;
		}
	}
}
