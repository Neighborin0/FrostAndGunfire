using System;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	 class Gooper : AdvancedGunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Glooper", "gooper");
			Game.Items.Rename("outdated_gun_mods:glooper", "kp:glooper");
			gun.gameObject.AddComponent<Gooper>();
			gun.SetShortDescription("Woop Woop Woop Woop Woop");
			gun.SetLongDescription("A squid found in the nearby waters of the Gungeon, the Glooper's ink can be used to slow Gundead.");
			gun.SetupSprite(null, "gooper_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 8);
			gun.SetAnimationFPS(gun.chargeAnimation, 5);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);
			gun.gunHandedness = GunHandedness.TwoHanded;
			Gun gun2 = PickupObjectDatabase.GetById(196) as Gun;
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(37) as Gun).gunSwitchGroup;
			for (int i = 0; i < 3; i++)
			{
				gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
				gun.DefaultModule.customAmmoType = gun2.DefaultModule.customAmmoType;
				gun.DefaultModule.ammoType = gun2.DefaultModule.ammoType;
			}
			foreach (ProjectileModule mod in gun.Volley.projectiles)
			{
				mod.ammoCost = 1;
				mod.shootStyle = ProjectileModule.ShootStyle.Charged;
				mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				mod.cooldownTime = 0.6f;
				mod.angleVariance = 20f;
				mod.numberOfShotsInClip = 8;
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(mod.projectiles[0]);
				mod.projectiles[0] = projectile;
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);
				projectile.baseData.damage *= 2.5f;
				projectile.baseData.speed *= 0.6f;
				projectile.AppliesSpeedModifier = true;
				projectile.SpeedApplyChance = 1f; 
				BulletStatusEffectItem slowdownmon = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>();
				GameActorSpeedEffect slowdown = slowdownmon.SpeedModifierEffect;
				slowdown.SpeedMultiplier *= 0.5f;
				slowdown.AppliesTint = true;
				slowdown.AppliesDeathTint = true;
				slowdown.duration = 3f;
				slowdown.TintColor = Color.black;
				projectile.speedEffect = slowdown;
				if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
				gun.DefaultModule.positionOffset = new Vector3(1f, 0, 0);
				projectile.transform.parent = gun.barrelOffset;
				projectile.SetProjectileSpriteRight("ink_projectile_001", 12, 12);
				if (projectile.gameObject.GetComponent<GoopModifier>())
				{
					Destroy(projectile.gameObject.GetComponent<GoopModifier>());
				}

				GoopModifier goop = projectile.gameObject.AddComponent<GoopModifier>();
				goop.SpawnGoopOnCollision = true;
				goop.InFlightSpawnFrequency = 0.05f;
				goop.InFlightSpawnRadius = 0.7f;
				gun.gunClass = GunClass.SILLY;
				goop.SpawnGoopInFlight = true;
				goop.CollisionSpawnRadius = 0.7f;
				AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
				GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/oil goop.asset");
				goop.goopDefinition = goopDef;
				goopDef.AppliesSpeedModifier = true;
				goopDef.damagesPlayers = false;
				goopDef.CanBeIgnited = false;
				goopDef.playerStepsLifetime = 0f;
				goopDef.playerStepsChangeLifetime = true;
				ProjectileModule.ChargeProjectile chargeProj = new ProjectileModule.ChargeProjectile
				{
					Projectile = projectile,
					ChargeTime = 0.6f,
				};
				goopDef.lifespan = 3f;
				mod.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj };
			}
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 1;
			gun.quality = PickupObject.ItemQuality.C;
			gun.reloadTime = 1.5f;
			gun.muzzleFlashEffects = null;
			gun.SetBaseMaxAmmo(80);
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
		}

		

		public override void PostProcessProjectile(Projectile projectile)
		{
			if (gun.CurrentOwner != null)
			{
			 AkSoundEngine.PostEvent("Play_WPN_fish_impact_01", base.gameObject);
			}
		}
	
	}
}
