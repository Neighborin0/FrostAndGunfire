using System;
using Gungeon;
using UnityEngine;
using ItemAPI;
using GungeonAPI;
using System.Collections;

namespace FrostAndGunfireItems
{
	
	public class BuildAGun : GunBehaviour
	{
		private bool HasReloaded;
		//Add synergies in a dedicated update
	
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Build-A-Gun", "build");
			Game.Items.Rename("outdated_gun_mods:build-a-gun", "kp:build-a-gun");
			gun.gameObject.AddComponent<BuildAGun>();
			gun.SetShortDescription("Impressionable");
			gun.SetLongDescription("A gun left unfinished and abandoned by its creator. It still has great potential.");
			gun.SetupSprite(null, "build_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.AddProjectileModuleFrom("ak-47", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.1f;
			gun.DefaultModule.cooldownTime = 0.1f;
			gun.DefaultModule.numberOfShotsInClip = 6;
			gun.quality = PickupObject.ItemQuality.D;
			gun.gunHandedness = GunHandedness.OneHanded;
			Gun gun4 = PickupObjectDatabase.GetById(50) as Gun;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
			gun.SetBaseMaxAmmo(200);
			Guid.NewGuid().ToString();
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage *= 0.9f;
			projectile.transform.parent = gun.barrelOffset;
			projectile.SetProjectileSpriteRight("build_projectile", 5, 5);
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}

		protected void Update()
		{
			
			if (gun.CurrentOwner)
			{
				
				if (!gun.PreventNormalFireAudio)
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
			PlayerController x = gun.CurrentOwner as PlayerController;
			if (x != null)
			{
				float value = UnityEngine.Random.value;
				if (x.HasPickupID(286) && (double)value < 0.30)
				{
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
					}
				}
				if (x.HasPickupID(352) && (double)value < 0.10)
				{
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.HasDefaultTint = true;
						component1.DefaultTintColor = Color.black;
						component1.collidesWithPlayer = false;
					}
				}
				if (x.HasPickupID(111) && (double)value < 0.05)
				{
					projectile.AdditionalScaleMultiplier *= 1.5f;
					projectile.DefaultTintColor = Color.gray;
					projectile.AppliedStunDuration = 1.5f;
					projectile.AppliesStun = true;
				}
				if (x.HasPickupID(113) && (double)value < 0.10 || x.HasPickupID(284) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[129]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;
					}
				}
				if (x.HasPickupID(298) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[159]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;
					}
				}
				if (x.HasPickupID(288) && (double)value < 0.20)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[50]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.AdditionalScaleMultiplier *= 1.5f;
						component1.Owner = gun.CurrentOwner;
					}
				}
				if (x.HasPickupID(172) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[45]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;
						component1.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(component1.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.AddFearEffect));
					}
				}
				if (x.HasPickupID(241) && (double)value < 0.20)
				{
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;
						
					}
				}
				if (x.HasPickupID(204) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[207]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;
					
					}
				}
				if (x.HasPickupID(295) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[125]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;
					
					}
				}
				if (x.HasPickupID(278) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items["Icepick"]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;
						
					}
				}
				if (x.HasPickupID(571) && (double)value < 0.10)
				{
					projectile.baseData.damage *= 1.4f + (PlayerStats.GetTotalCurse() * 0.10f);
					projectile.AdditionalScaleMultiplier *= 2f + (PlayerStats.GetTotalCurse() * 0.10f);
				}
				if (x.HasPickupID(532) && (double)value < 0.10)
				{
					projectile.baseData.damage *= 1f + (x.carriedConsumables.Currency * 0.03f);
					projectile.AdditionalScaleMultiplier += x.carriedConsumables.Currency;
					projectile.HasDefaultTint = true;
					projectile.DefaultTintColor = Color.yellow;
				}
				if (x.HasPickupID(527) && (double)value < 0.9)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[379]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}

				}
				if (x.HasPickupID(533) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[385]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}

				if (x.HasPickupID(521) && (double)value < 0.15 || x.HasPickupID(569) && (double)value < 0.15)
				{
					projectile = null;
					Gun randomGun;
					int pickupObjectId;
					do
					{
						randomGun = PickupObjectDatabase.GetRandomGun();
						pickupObjectId = randomGun.PickupObjectId;
					}
					while (randomGun.HasShootStyle(ProjectileModule.ShootStyle.SemiAutomatic));
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[pickupObjectId]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, x.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					bool flag4 = component != null;
					if (flag4)
					{
						component.Owner = x;
						component.Shooter = x.specRigidbody;
					}

				
				}
				if (x.HasPickupID(277) && (double)value < 0.15 || x.HasPickupID(523) && (double)value < 0.15)
				{
					projectile.AdditionalScaleMultiplier *= 3f;
				}
				if (x.HasPickupID(323) && (double)value < 0.15)
				{
					projectile.baseData.damage *= 1.5f;
					projectile.baseData.speed *= 1.5f;

				}
				if (x.HasPickupID(528) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[704]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}
				if (x.HasPickupID(528) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[704]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}
				if (x.HasPickupID(630) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[14]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}
				if (x.HasPickupID(638) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[8]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}
				if (x.HasPickupID(636) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[402]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}
				if (x.HasPickupID(579) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[180]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}
				if (x.HasPickupID(640) && (double)value < 0.10 || x.HasPickupID(822) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[417]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}
				if (x.HasPickupID(661) && (double)value < 0.10)
				{
					projectile = null;
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[597]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (x.CurrentGun == null) ? 0f : x.CurrentGun.CurrentAngle), true);
					Projectile component1 = gameObject.GetComponent<Projectile>();
					if (component1 != null)
					{
						component1.collidesWithPlayer = false;
						component1.Owner = gun.CurrentOwner;

					}
				}






			}
		}


		private void AddFearEffect(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			PlayerController player = gun.CurrentOwner as PlayerController;
			bool flag = arg2 != null && arg2.aiActor != null && player != null;
			if (flag)
			{
				bool flag2 = arg2 != null && arg2.healthHaver.IsAlive;
				if (flag2)
				{
					bool flag3 = arg2.aiActor.EnemyGuid != "465da2bb086a4a88a803f79fe3a27677" && arg2.aiActor.EnemyGuid != "05b8afe0b6cc4fffa9dc6036fa24c8ec";
					if (flag3)
					{
						base.StartCoroutine(this.HandleFear(arg2));
					}
				}
			}
		}

		private IEnumerator HandleFear(SpeculativeRigidbody enemy)
		{
			PlayerController player = gun.CurrentOwner as PlayerController;
			bool flag = this.fleeData == null || this.fleeData.Player != player;
			if (flag)
			{
				this.fleeData = new FleePlayerData();
				this.fleeData.Player = player;
				this.fleeData.StartDistance *= 2f;
			}
			bool flag2 = enemy.aiActor.behaviorSpeculator != null;
			if (flag2)
			{
				enemy.aiActor.behaviorSpeculator.FleePlayerData = this.fleeData;
				FleePlayerData fleePlayerData = new FleePlayerData();
				yield return new WaitForSeconds(7f);
				enemy.aiActor.behaviorSpeculator.FleePlayerData.Player = null;
			}
			yield break;
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
			PlayerController x = gun.CurrentOwner as PlayerController;
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
			bool flag = gun.ClipShotsRemaining >= 6 && x.HasPickupID(373);
			if (flag)
			{
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
				projectile.baseData.damage *= 1.5f;
				projectile.AdditionalScaleMultiplier *= 2f;
			}
			bool flag2 = gun.ClipShotsRemaining == 1 && x.HasPickupID(374);
			if (flag2)
			{
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
				projectile.baseData.damage *= 1.5f;
				projectile.AdditionalScaleMultiplier *= 2f;
			}
		}

		private FleePlayerData fleeData;


	}
}
