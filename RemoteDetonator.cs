using System;
using Gungeon;
using UnityEngine;
using ItemAPI;
using GungeonAPI;
using System.Collections;
using Dungeonator;
using System.Linq;

namespace FrostAndGunfireItems
{
	
	public class RemoteDetonator : GunBehaviour
	{
		private bool HasReloaded;
		public GameObject Rocket;

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Remote Detonator", "remote");
			Game.Items.Rename("outdated_gun_mods:remote_detonator", "kp:remote_detonator");
			gun.gameObject.AddComponent<RemoteDetonator>();
			gun.SetShortDescription("On Demand Air Strike");
			gun.SetLongDescription("A remote created by a mad doctor, its advanced targetting system can target enemies well underground.");
			gun.SetupSprite(null, "remote_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.AddProjectileModuleFrom("ak-47", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.1f;
			gun.DefaultModule.cooldownTime = 0.4f;
			gun.DefaultModule.numberOfShotsInClip = 40;
			gun.quality = PickupObject.ItemQuality.S;
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.muzzleFlashEffects = null;
			gun.SetBaseMaxAmmo(40);
			Guid.NewGuid().ToString();
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.sprite.renderer.enabled = false;
			projectile.baseData.damage = 0;
			projectile.baseData.range = 0;
			projectile.baseData.force = 0;
			projectile.hitEffects.suppressMidairDeathVfx = true;
			projectile.transform.parent = gun.barrelOffset;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.LICH_KILLED_AND_MARINE, true);
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
				var cm = UnityEngine.Object.Instantiate<GameObject>((GameObject)BraveResources.Load("Global Prefabs/_ChallengeManager", ".prefab"));
				this.Rocket = (cm.GetComponent<ChallengeManager>().PossibleChallenges.Where(c => c.challenge is SkyRocketChallengeModifier).First().challenge as SkyRocketChallengeModifier).Rocket;
				UnityEngine.Object.Destroy(cm);
				SkyRocket component = SpawnManager.SpawnProjectile(this.Rocket, Vector3.zero, Quaternion.identity, true).GetComponent<SkyRocket>();
				component.AscentTime = 0.1f;
				component.DescentTime = 0.5f;
				component.Variance = 0f;
				component.IgnoreExplosionQueues = true;
				component.TargetVector2 = x.unadjustedAimPoint;
				tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
				component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
				//Exploder.DoDefaultExplosion(x.unadjustedAimPoint, Vector2.zero);
			
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
			PlayerController x = gun.CurrentOwner as PlayerController;
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", base.gameObject);
		}

		private FleePlayerData fleeData;


	}
}
