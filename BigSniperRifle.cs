using System;
using System.Collections;
using System.Linq;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class BigSniperRifle : GunBehaviour
	{
		
		public BigSniperRifle()
		{
			this.SuckRadius = 7f;
		}

		
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Big Sniper Rifle", "beeg");
			Game.Items.Rename("outdated_gun_mods:big_sniper_rifle", "kp:big_sniper_rifle");
			gun.gameObject.AddComponent<BigSniperRifle>();
			gun.SetShortDescription("It's A Big ****ing Gun");
			gun.SetLongDescription("it's a sniper rifle BUT BIGGER AND BETTER THAN EVER.");
			gun.SetupSprite(null, "beeg_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 10);
			gun.SetAnimationFPS(gun.reloadAnimation, 14);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(601) as Gun, true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.4f;
			gun.DefaultModule.angleVariance = 0f;
			gun.SetBaseMaxAmmo(80);
			gun.DefaultModule.cooldownTime = 0.8f;
			gun.DefaultModule.numberOfShotsInClip = 8;
			gun.quality = PickupObject.ItemQuality.A;
			Gun gun2 = PickupObjectDatabase.GetById(5) as Gun;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.barrelOffset.transform.localPosition += new Vector3(2.5f, 0.27f, 0);
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage *= 1.7f;
			projectile.AdditionalScaleMultiplier *= 0.6f;
			projectile.baseData.speed *= 2f;
			projectile.AdditionalScaleMultiplier *= 0.7f;
			gun.gunHandedness = GunHandedness.TwoHanded;
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");

		}


		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_sniperrifle_shot_01", base.gameObject);
		}

		protected void Update()
		{
		  
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


		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_rpg_reload_01", base.gameObject);
				player.CurrentRoom.ApplyActionToNearbyEnemies(player.CenterPosition, this.SuckRadius, new Action<AIActor, float>(this.ProcessEnemy));
			}
		}

		private void ProcessEnemy(AIActor target, float distance)
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			for (int i = 0; i < this.TargetEnemies.Length; i++)
			{
				bool flag = target.EnemyGuid == this.TargetEnemies[i];
				if (flag)
				{
					gun.ammo += 1;
					GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(target));
					AkSoundEngine.PostEvent("Play_NPC_BabyDragun_Munch_01", base.gameObject);
					target.EraseFromExistence(true);
					break;
				}
			}
		}

		
	
		private IEnumerator HandleEnemySuck(AIActor target)
		{
			Transform copySprite = this.CreateEmptySprite(target);
			Vector3 startPosition = copySprite.transform.position;
			float elapsed = 0f;
			float duration = 1f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				bool flag = this.gun && copySprite;
				if (flag)
				{
					Vector3 position = this.gun.PrimaryHandAttachPoint.position;
					float t = elapsed / duration * (elapsed / duration);
					copySprite.position = Vector3.Lerp(startPosition, position, t);
					copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
					copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), t);
					position = default(Vector3);
				}
				yield return null;
			}
			bool flag2 = copySprite;
			if (flag2)
			{
				UnityEngine.Object.Destroy(copySprite.gameObject);
			}
			bool flag3 = this.gun;
			if (flag3)
			{
				
			}
			yield break;
		}


		private Transform CreateEmptySprite(AIActor target)
		{
			GameObject gameObject = new GameObject("suck image");
			gameObject.layer = target.gameObject.layer;
			tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
			gameObject.transform.parent = SpawnManager.Instance.VFX;
			tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
			tk2dSprite.transform.position = target.sprite.transform.position;
			GameObject gameObject2 = new GameObject("image parent");
			gameObject2.transform.position = tk2dSprite.WorldCenter;
			tk2dSprite.transform.parent = gameObject2.transform;
			bool flag = target.optionalPalette != null;
			if (flag)
			{
				tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
			}
			return gameObject2.transform;
		}

		
		public float SuckRadius;

	
		public string[] TargetEnemies = new string[]
		{
			"31a3ea0c54a745e182e22ea54844a82d",
			"c5b11bfc065d417b9c4d03a5e385fe2c"
		};
		private bool HasReloaded;

	

	}
}
