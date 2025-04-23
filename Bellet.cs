using System;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	 class Bellet : AdvancedGunBehaviour
	{

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Bellet", "bellet");
			Game.Items.Rename("outdated_gun_mods:bellet", "kp:bellet");
			gun.gameObject.AddComponent<Bellet>();
			gun.SetShortDescription("Bing Bang Bong");
			gun.SetLongDescription("A strange bell dropped from the Mindflayer, the Bellet sends shockwaves of bullets around itself when rung.");
			gun.SetupSprite(null, "bellet_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 8);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);
			gun.carryPixelOffset = new IntVector2((int)8f , -3);
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(37) as Gun).gunSwitchGroup;
			for (int i = 0; i < 20; i++)
			{
				gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(22) as Gun, true, false);
			}
			int g = 0;
			foreach (ProjectileModule mod in gun.Volley.projectiles)
			{
				mod.ammoCost = 1;
				mod.shootStyle = ProjectileModule.ShootStyle.Charged;
				mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				mod.cooldownTime = 0.5f;
				mod.angleVariance = 0f;
				mod.angleFromAim = g * 18;
				mod.numberOfShotsInClip = 1;
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(mod.projectiles[0]);
				mod.projectiles[0] = projectile;
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);
				if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
				projectile.transform.parent = gun.barrelOffset;
				ProjectileModule.ChargeProjectile chargeProj = new ProjectileModule.ChargeProjectile
				{
					Projectile = projectile,
					ChargeTime = 0.6f,
				};
				mod.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj };
				g++;
			}
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 1;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_BOSS_mineflayer_bellshot_01";
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;
			gun.quality = PickupObject.ItemQuality.B;
			gun.reloadTime = 1.5f;
			gun.muzzleFlashEffects = null;
			gun.SetBaseMaxAmmo(50);
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.SILLY;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController player = gun.CurrentOwner as PlayerController;
			if (player.PlayerHasActiveSynergy("Ding Dong"))
			{
				projectile.DefaultTintColor = Color.gray;
				projectile.HasDefaultTint = true;
				BulletStatusEffectItem slowdownmon = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>();
				GameActorSpeedEffect slowdown = slowdownmon.SpeedModifierEffect;
				slowdown.SpeedMultiplier *= 0.5f;
				slowdown.duration = 3f;
				projectile.AppliesSpeedModifier = true;
				projectile.SpeedApplyChance = 1f;
				projectile.speedEffect = slowdown;

			}
		}
		public override void OnAutoReload(PlayerController player, Gun gun)
        {
               if (player.PlayerHasActiveSynergy("Mineflayed"))
                {
                    SpawnObjectPlayerItem minePrefab = PickupObjectDatabase.GetById(66).GetComponent<SpawnObjectPlayerItem>();
                    GameObject mine = minePrefab.objectToSpawn.gameObject;
                    GameObject mine2 = UnityEngine.Object.Instantiate<GameObject>(mine, player.sprite.WorldBottomCenter, Quaternion.identity);
                    tk2dBaseSprite minesprite = mine2.GetComponent<tk2dBaseSprite>();
                    if (minesprite)
                    {
					minesprite.PlaceAtPositionByAnchor(player.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                    }
                }
            }
        }
	
	}

