using System;
using Gungeon;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000021 RID: 33
	public class Polarity : GunBehaviour
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00008B5C File Offset: 0x00006D5C
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Polarity", "polarity");
			Game.Items.Rename("outdated_gun_mods:polarity", "kp:polarity");
			gun.gameObject.AddComponent<Polarity>();
			gun.SetShortDescription("Aurora Borealis!?");
			gun.SetLongDescription("A gun that's been modified to shoot magnetic balls. \n\n If it's red, knock em dead, if it's blue, bring them to you.");
			gun.SetupSprite(null, "polarity_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(720) as Gun, true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;
			gun.SetBaseMaxAmmo(500);
			gun.AppliesHoming = true;
			gun.DefaultModule.cooldownTime = 0.1f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 500;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.encounterTrackable.EncounterGuid = "polarity";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.gunHandedness = GunHandedness.TwoHanded;
			
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00008C80 File Offset: 0x00006E80
		public override void PostProcessProjectile(Projectile projectile)
		{
			
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			x.PostProcessBeamTick += PostProcessBeam;
			bool flag = x == null;
			if (flag)
			{
				this.gun.ammo = this.gun.GetBaseMaxAmmo();
			}
			this.gun.DefaultModule.ammoCost = 1;
			
		}

		private void PostProcessBeam(BeamController arg1, SpeculativeRigidbody arg2, float arg3)
		{
			arg1.knockbackStrength -= 100f;
			arg1.projectile.baseData.force = -100f;
		}
	}
}
