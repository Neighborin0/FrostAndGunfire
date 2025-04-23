using System;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000026 RID: 38
	 class Blobby : AdvancedGunBehaviour
	{

		public List<Gun> GunsAbsorbed = new List<Gun>();
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Blobby", "blobby");
			Game.Items.Rename("outdated_gun_mods:blobby", "kp:blobby");
			gun.gameObject.AddComponent<Blobby>();
			gun.SetShortDescription("Bone Chilling");
			gun.SetLongDescription("A gun formed through the power of frost, this ancient weapon pierces foes with a frozen might. Most can't even touch it without freezing.");
			gun.SetupSprite(null, "blobby_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.AddProjectileModuleFrom("blasphemy", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.3f;
			gun.DefaultModule.cooldownTime = 0.31f;
			gun.DefaultModule.numberOfShotsInClip = gun.GetBaseMaxAmmo();
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.SetBaseMaxAmmo(250);
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage *= 0.30f;
			gun.gunClass = GunClass.SILLY;
			projectile.baseData.speed *= 0.7f;
			gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_WANDERER, true);
			projectile.SetProjectileSpriteRight("icepick_projectile_001", 18, 10);
		}

		public override void OnReloadPressed(PlayerController user, Gun gun, bool manualReload)
		{
			IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
			if (nearestInteractable is PassiveItem)
			{
				PassiveItem item = nearestInteractable as PassiveItem;
				Tools.GetRandomStatUp(user);
				user.CurrentRoom.DeregisterInteractable(item);
				UnityEngine.Object.Destroy(item.gameObject);
			}
			if (nearestInteractable is PlayerItem)
			{
				PlayerItem item = nearestInteractable as PlayerItem;
				Tools.GetRandomStatUp(user);
				user.CurrentRoom.DeregisterInteractable(item);
				UnityEngine.Object.Destroy(item.gameObject);
			}
			if (nearestInteractable is Gun)
			{
				Gun mineminemine = nearestInteractable as Gun;
				MergeGuns(mineminemine, gun);
				user.stats.RecalculateStats(user, false, false);
				user.CurrentRoom.DeregisterInteractable(mineminemine);
				UnityEngine.Object.Destroy(mineminemine.gameObject);
			}

		}

		public  void MergeGuns(Gun merged, Gun target)
		{
			ProjectileVolleyData projectileVolleyData = CombineVolleys(target, merged);
			ReconfigureVolley(projectileVolleyData);
			target.RawSourceVolley = projectileVolleyData;
			target.GainAmmo(target.GetBaseMaxAmmo());
			target.SetBaseMaxAmmo(target.GetBaseMaxAmmo() + merged.GetBaseMaxAmmo());
		
		}

		protected static ProjectileVolleyData CombineVolleys(Gun sourceGun, Gun mergeGun)
		{
			ProjectileVolleyData projectileVolleyData = ScriptableObject.CreateInstance<ProjectileVolleyData>();
			if (sourceGun.RawSourceVolley != null)
			{
				projectileVolleyData.InitializeFrom(sourceGun.RawSourceVolley);
			}
			else
			{
				projectileVolleyData.projectiles = new List<ProjectileModule>();
				projectileVolleyData.projectiles.Add(ProjectileModule.CreateClone(sourceGun.singleModule, true, -1));
				projectileVolleyData.BeamRotationDegreesPerSecond = float.MaxValue;
			}
			if (mergeGun.RawSourceVolley != null)
			{
				for (int i = 0; i < mergeGun.RawSourceVolley.projectiles.Count; i++)
				{
					ProjectileModule projectileModule = ProjectileModule.CreateClone(mergeGun.RawSourceVolley.projectiles[i], true, -1);
					projectileModule.ignoredForReloadPurposes = (projectileModule.ammoCost <= 0 || projectileModule.numberOfShotsInClip <= 0);
					projectileVolleyData.projectiles.Add(projectileModule);
					if (!string.IsNullOrEmpty(mergeGun.gunSwitchGroup) && i == 0)
					{
						projectileModule.runtimeGuid = ((projectileModule.runtimeGuid == null) ? Guid.NewGuid().ToString() : projectileModule.runtimeGuid);
						sourceGun.AdditionalShootSoundsByModule.Add(projectileModule.runtimeGuid, mergeGun.gunSwitchGroup);
					}
					if (mergeGun.RawSourceVolley.projectiles[i].runtimeGuid != null && mergeGun.AdditionalShootSoundsByModule.ContainsKey(mergeGun.RawSourceVolley.projectiles[i].runtimeGuid))
					{
						sourceGun.AdditionalShootSoundsByModule.Add(mergeGun.RawSourceVolley.projectiles[i].runtimeGuid, mergeGun.AdditionalShootSoundsByModule[mergeGun.RawSourceVolley.projectiles[i].runtimeGuid]);
					}
				}
			}
			else
			{
				ProjectileModule projectileModule2 = ProjectileModule.CreateClone(mergeGun.singleModule, true, -1);
				projectileModule2.ignoredForReloadPurposes = (projectileModule2.ammoCost <= 0 || projectileModule2.numberOfShotsInClip <= 0);
				projectileVolleyData.projectiles.Add(projectileModule2);
				if (!string.IsNullOrEmpty(mergeGun.gunSwitchGroup))
				{
					projectileModule2.runtimeGuid = ((projectileModule2.runtimeGuid == null) ? Guid.NewGuid().ToString() : projectileModule2.runtimeGuid);
					sourceGun.AdditionalShootSoundsByModule.Add(projectileModule2.runtimeGuid, mergeGun.gunSwitchGroup);
				}
			}
			return projectileVolleyData;
		}

		protected static void ReconfigureVolley(ProjectileVolleyData newVolley)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num = 0;
			for (int i = 0; i < newVolley.projectiles.Count; i++)
			{
				if (newVolley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Automatic)
				{
					flag = true;
				}
				if (newVolley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Beam)
				{
					flag = true;
				}
				if (newVolley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Burst)
				{
					flag4 = true;
				}
				if (newVolley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Charged)
				{
					flag3 = true;
					num++;
				}
			}
			if (!flag && !flag2 && !flag3 && !flag4)
			{
				return;
			}
			if (!flag && !flag2 && !flag3 && flag4)
			{
				return;
			}
			int num2 = 0;
			for (int j = 0; j < newVolley.projectiles.Count; j++)
			{
				if (newVolley.projectiles[j].shootStyle == ProjectileModule.ShootStyle.SemiAutomatic)
				{
					newVolley.projectiles[j].shootStyle = ProjectileModule.ShootStyle.Automatic;
				}
				if (newVolley.projectiles[j].shootStyle == ProjectileModule.ShootStyle.Charged && num > 1)
				{
					num2++;
					if (num > 1)
					{
					}
				}
			}
		}
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_ENM_iceslime_shot_01", base.gameObject);
		}

	}
}
