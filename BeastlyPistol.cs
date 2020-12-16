using System;
using System.Collections;
using Dungeonator;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{

	public class BeastlyPistol : GunBehaviour
	{
		private bool HasReloaded;
		//Fix gun stat ups spreading to other guns
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Beastly Pistol", "beast");
			Game.Items.Rename("outdated_gun_mods:beastly_pistol", "kp:beastly_pistol");
			gun.gameObject.AddComponent<BeastlyPistol>();
			gun.SetShortDescription("Half Beast, Half Gun");
			gun.SetLongDescription("An unholy abomination between beast and gun, this gun has an unsatiated bloodlust. Just seeing Gundead makes this gun go ravenous.");
			gun.SetupSprite(null, "beast_idle_001", 3);
			gun.SetAnimationFPS(gun.shootAnimation, 10);
			gun.SetAnimationFPS(gun.reloadAnimation, 6);
			gun.AddProjectileModuleFrom("ak-47", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1f;
			gun.DefaultModule.cooldownTime = 0.4f;
			gun.DefaultModule.numberOfShotsInClip = 8;
			gun.quality = PickupObject.ItemQuality.B;
			gun.gunHandedness = GunHandedness.OneHanded;
			Gun gun4 = PickupObjectDatabase.GetById(50) as Gun;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
			gun.SetBaseMaxAmmo(400);
			Guid.NewGuid().ToString();
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage *= 1.5f;
			projectile.baseData.speed *= 1f;
			projectile.DefaultTintColor = Color.red;
			projectile.HasDefaultTint = true;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
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
			PlayerController x = gun.CurrentOwner as PlayerController;
			bool flag7 = gun.CurrentOwner;
			if (flag7)



			{
				RoomHandler currentRoom = x.CurrentRoom;
				bool f = !currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag2 = !f;
				bool flag3 = flag2;
				if (flag3)
				{

					foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
					{
						bool flag4 = aiactor.behaviorSpeculator != null;
						bool flag5 = flag4;
						bool flag6 = flag5;
						if (flag6 && !x.HasPickupID(285) || !x.HasPickupID(313) || !x.HasPickupID(524) || !x.HasPickupID(167))
						{
							projectile.AdditionalScaleMultiplier += 0.3f;
							projectile.baseData.speed += 0.5f;
							projectile.baseData.damage += 0.5f;

						}
						else
						{
							projectile.AdditionalScaleMultiplier += 0.4f;
							projectile.baseData.speed += 0.8f;
							projectile.baseData.damage += 0.73f;
						}
					}
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
				AkSoundEngine.PostEvent("Play_ENM_bigshroom_roar_01", base.gameObject);
			}
		}
		


		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
		}


	}
}
