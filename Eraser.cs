using System;
using System.Collections;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000026 RID: 38
	 class Eraser : AdvancedGunBehaviour
	{

		private static CombineEvaporateEffect effect;
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Eraser", "eraser");
			Game.Items.Rename("outdated_gun_mods:eraser", "kp:eraser");
			gun.gameObject.AddComponent<Eraser>();
			gun.gunClass = GunClass.SILLY;
			gun.SetShortDescription(" ");
			gun.SetLongDescription("   ");
			gun.SetupSprite(null, "eraser_idle_001", 8);
			gun.AddProjectileModuleFrom("blasphemy", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;
			gun.DefaultModule.cooldownTime = 0f;
			gun.DefaultModule.numberOfShotsInClip = 0;
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.quality = PickupObject.ItemQuality.A;
			gun.SetBaseMaxAmmo(0);
			Guid.NewGuid().ToString();
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage *= 0f;
			projectile.baseData.speed *= 0f;
			effect = GetCombineEffect();
			ItemBuilder.AddPassiveStatModifier(gun, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
		}
		private static CombineEvaporateEffect GetCombineEffect()
		{
			var gun = Gungeon.Game.Items["combined_rifle"].GetComponent<Gun>();
			return gun.alternateVolley.projectiles[0].projectiles[0].GetComponent<CombineEvaporateEffect>();
		}
		protected override void OnPickedUpByPlayer(PlayerController player)
		{
			gun.encounterTrackable.m_doNotificationOnEncounter = false;
			player.PostProcessThrownGun += Erase;
			if (player.PlayerHasActiveSynergy("Erased"))
			{
				player.PostProcessThrownGun -= Erase;
				player.PostProcessThrownGun += EraseButBouncy;
			}
			else
			{
				player.PostProcessThrownGun += Erase;
				player.PostProcessThrownGun -= EraseButBouncy;
			}
		}

		public override void OnDropped()
		{
			PlayerController player = gun.CurrentOwner as PlayerController;
			player.PostProcessThrownGun -= Erase;
			player.PostProcessThrownGun -= EraseButBouncy;
		}
		private void Erase(Projectile obj)
		{	
			PlayerController player = gun.CurrentOwner as PlayerController;
			HomingModifier homingModifier = obj.gameObject.GetOrAddComponent<HomingModifier>();
			homingModifier.HomingRadius = 10f;
			homingModifier.AngularVelocity = 10f;
			PierceProjModifier pierce = obj.gameObject.GetOrAddComponent<PierceProjModifier>();
			pierce.penetratesBreakables = true;
			pierce.penetration = 9999;
			pierce.preventPenetrationOfActors = false;
			obj.pierceMinorBreakables = true;
			obj.OnWillKillEnemy += Disintegrate;
			obj.projectile.baseData.damage = 400;
			obj.projectile.baseData.speed = 9;
			obj.IgnoreTileCollisionsFor(0.01f);
			player.PostProcessThrownGun -= Erase;
			player.PostProcessThrownGun -= EraseButBouncy;
		}

		private void EraseButBouncy(Projectile obj)
		{
			PlayerController player = gun.CurrentOwner as PlayerController;
			BounceProjModifier boing = obj.gameObject.GetOrAddComponent<BounceProjModifier>();
			boing.numberOfBounces = 3;
			HomingModifier homingModifier = obj.gameObject.GetOrAddComponent<HomingModifier>();
			homingModifier.HomingRadius = 10f;
			homingModifier.AngularVelocity = 10f;
			PierceProjModifier pierce = obj.gameObject.GetOrAddComponent<PierceProjModifier>();
			pierce.penetratesBreakables = true;
			pierce.penetration = 9999;
			pierce.preventPenetrationOfActors = false;
			obj.pierceMinorBreakables = true;
			obj.OnWillKillEnemy += Disintegrate;
			obj.projectile.baseData.damage = 400;
			obj.projectile.baseData.speed = 9;
			obj.IgnoreTileCollisionsFor(0.01f);
			player.PostProcessThrownGun -= Erase;
			player.PostProcessThrownGun -= EraseButBouncy;
		}


		private void Disintegrate(Projectile arg1, SpeculativeRigidbody arg2)
		{
			if (!arg2.aiActor.healthHaver.IsBoss)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemyDeath(arg2.aiActor));
			}
		}

		private IEnumerator HandleEnemyDeath(AIActor target)
		{
			target.EraseFromExistenceWithRewards(false);
			Transform copyTransform = this.CreateEmptySprite(target);
			tk2dSprite copySprite = copyTransform.GetComponentInChildren<tk2dSprite>();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Eraser.effect.ParticleSystemToSpawn, copySprite.WorldCenter.ToVector3ZisY(0f), Quaternion.identity);
			gameObject.transform.parent = copyTransform;
			float elapsed = 0f;
			float duration = 2.5f;
			copySprite.renderer.material.DisableKeyword("TINTING_OFF");
			copySprite.renderer.material.EnableKeyword("TINTING_ON");
			copySprite.renderer.material.DisableKeyword("EMISSIVE_OFF");
			copySprite.renderer.material.EnableKeyword("EMISSIVE_ON");
			copySprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
			copySprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
			copySprite.renderer.material.SetFloat("_EmissiveThresholdSensitivity", 5f);
			copySprite.renderer.material.SetFloat("_EmissiveColorPower", 1f);
			int emId = Shader.PropertyToID("_EmissivePower");
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				float t = elapsed / duration;
				copySprite.renderer.material.SetFloat(emId, Mathf.Lerp(1f, 10f, t));
				copySprite.renderer.material.SetFloat("_BurnAmount", t);
				copyTransform.position += Vector3.up * BraveTime.DeltaTime * 1f;
				yield return null;
			}
			UnityEngine.Object.Destroy(copyTransform.gameObject);
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
			tk2dSprite.usesOverrideMaterial = true;
			bool flag = target.optionalPalette != null;
			if (flag)
			{
				tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
			}
			bool flag2 = tk2dSprite.renderer.material.shader.name.Contains("ColorEmissive");
			if (flag2)
			{
			}
			return gameObject2.transform;
		}
	}
}
