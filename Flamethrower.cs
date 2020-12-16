using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000024 RID: 36
	public class Flamethrower : GunBehaviour
	{
		
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Flamethrower", "flamethrower");
			Game.Items.Rename("outdated_gun_mods:flamethrower", "kp:flamethrower");
			gun.gameObject.AddComponent<Flamethrower>();
			gun.SetShortDescription("Mmph mphna mprh.");
			gun.SetLongDescription("Murr hurr mphuphurrur, hurr mph phrr. Ow dow how dow. Mmmmmmmrrrrrrrpppghhh!");
			gun.SetupSprite(null, "flamethrower_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			//gun.AddProjectileModuleFrom("ak-47", true, false);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(125) as Gun, true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.reloadTime = 0.4f;
			gun.DefaultModule.angleVariance = 13f;
			gun.DefaultModule.cooldownTime = 0.04f;
			gun.SetBaseMaxAmmo(1500);
			gun.CurrentAmmo = 1500;
			gun.muzzleFlashEffects = null;
			gun.barrelOffset.transform.localPosition += new Vector3(0.9f, -0.4f, 0f);
			gun.DefaultModule.numberOfShotsInClip = 1500;
			gun.quality = PickupObject.ItemQuality.A;
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.isAudioLoop = false;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage *= 0.9f;
			projectile.shouldRotate = true;
			projectile.baseData.speed *= 0.45f;
			projectile.baseData.range = 7f;
			projectile.hitEffects.HasProjectileDeathVFX = false;
			projectile.hitEffects.suppressMidairDeathVfx = true;
			projectile.baseData.force *= 0f;
			//projectile.SetProjectileSpriteRight("f_projectile_001", 18, 10);
			projectile.AnimateProjectile(new List<string> {
				"f_projectile_001",
				"f_projectile_002",
				"f_projectile_003",
				"f_projectile_004",
				"f_projectile_005",
				"f_projectile_006",
				"f_projectile_007",
				"f_projectile_008",
				"f_projectile_009",
				"f_projectile_010",
				"f_projectile_011",
				"f_projectile_012",
				"f_projectile_013",


			}, 15, false, new List<IntVector2> {
				new IntVector2(12, 12),
				new IntVector2(18, 18),
				new IntVector2(22, 22),
				new IntVector2(26, 26),
				new IntVector2(30, 30),
				new IntVector2(32, 32),
				new IntVector2(32, 32),
				new IntVector2(34, 34),
				new IntVector2(36, 36),
				new IntVector2(38, 38),
				new IntVector2(40, 40),
				new IntVector2(39, 42),
				new IntVector2(40, 44),


			}, Tool.ConstructListOfSameValues(false, 13), Tool.ConstructListOfSameValues(tk2dSprite.Anchor.MiddleCenter, 13), Tool.ConstructListOfSameValues(true, 13), Tool.ConstructListOfSameValues(false, 13),
		   Tool.ConstructListOfSameValues<Vector3?>(null, 13), new List<IntVector2?> {
			 new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
		   }, new List<IntVector2?> {
			new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
				new IntVector2(18, 10),
		   }, Tool.ConstructListOfSameValues<Projectile>(null, 13));
		}


		protected void Update()
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			bool flag = x;
			if (flag)
			{
				if (x.HasPickupID(453))
				{
					bool flag5 = !this.HasImmunity;
					if (flag5)
					{
						this.m_fireImmunity = new DamageTypeModifier();
					this.m_fireImmunity.damageMultiplier = 0f;
					this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
					x.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
						HasImmunity = true;
				}
					else
					{
						
						HasImmunity = false;
					}
				
			}
		}
			}


		// Token: 0x060000DE RID: 222 RVA: 0x00009279 File Offset: 0x00007479
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_BOSS_doormimic_flame_01", base.gameObject);
		}

		// Token: 0x02000044 RID: 68
	
		private DamageTypeModifier m_fireImmunity;
		private bool HasImmunity;
	}
}
