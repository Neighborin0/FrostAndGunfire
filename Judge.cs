using System;
using Gungeon;
using UnityEngine;
using ItemAPI;
using GungeonAPI;
using System.Collections;
using System.Collections.Generic;

namespace FrostAndGunfireItems
{
	
	 class Judge : AdvancedGunBehaviour
	{
		private static List<Projectile> Numbas = new List<Projectile>();
		private DamageTypeModifier m_electricityImmunity;
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Judgement", "judge");
			Game.Items.Rename("outdated_gun_mods:judgement", "kp:judgement");
			gun.gameObject.AddComponent<Judge>();
			gun.SetShortDescription("Game and Gun");
			gun.SetLongDescription("A gun left unfinished and abandoned by its creator. It still has great potential.");
			gun.SetupSprite(null, "judge_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 3);
			gun.AddProjectileModuleFrom("ak-47", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.1f;
			gun.DefaultModule.cooldownTime = 0.7f;
			gun.DefaultModule.numberOfShotsInClip = 9;
			gun.quality = PickupObject.ItemQuality.B;
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.SetBaseMaxAmmo(91);
			gun.gunClass = GunClass.PISTOL;
			Guid.NewGuid().ToString();
			//1
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			projectile.baseData.damage /= projectile.baseData.damage;
			projectile.baseData.force = 0;
			projectile.transform.parent = gun.barrelOffset;
			projectile.SetProjectileSpriteRight("one_projectile_001", 8, 17);
			projectile.name = "1";
			Numbas.Add(projectile);
			//
			//2
			Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile2.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile2);
			projectile2.transform.parent = gun.barrelOffset;
			projectile2.SetProjectileSpriteRight("two_projectile", 9, 17);
			projectile2.name = "2";
			Numbas.Add(projectile2);
			//
			//3
			Projectile projectile3 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile3.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile3.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile3);
			projectile3.baseData.damage *= 2;
			projectile3.baseData.force *= -4;
			projectile3.transform.parent = gun.barrelOffset;
			projectile3.SetProjectileSpriteRight("three_projectile", 10, 17);
			projectile3.name = "3";
			Numbas.Add(projectile3);
			//
			//4
			Projectile projectile4 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile4.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile4.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile4);
			PierceProjModifier pierce = projectile4.gameObject.GetOrAddComponent<PierceProjModifier>();
			pierce.penetratesBreakables = true;
			pierce.penetration = 9999;
			pierce.preventPenetrationOfActors = false;
			projectile4.transform.parent = gun.barrelOffset;
			projectile4.SetProjectileSpriteRight("four_projectile", 10, 17);
			projectile4.name = "4";
			Numbas.Add(projectile4);
			//
			//5
			Projectile projectile5 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile5.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile5.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile5);
			projectile5.baseData.speed *= 1.2f;
			projectile5.damageTypes = CoreDamageTypes.Electric;
			GoopModifier goop = projectile5.gameObject.AddComponent<GoopModifier>();
			goop.SpawnGoopOnCollision = true;
			goop.InFlightSpawnFrequency = 0.05f;
			goop.InFlightSpawnRadius = 1f;
			goop.SpawnGoopInFlight = true;
			goop.CollisionSpawnRadius = 0.7f;
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/water goop.asset");
			goop.goopDefinition = goopDef;
			projectile5.transform.parent = gun.barrelOffset;
			projectile5.SetProjectileSpriteRight("five_projectile", 10, 17);
			projectile5.name = "5";
			Numbas.Add(projectile5);
			//
			//6
			Projectile projectile6 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(39) as Gun).DefaultModule.projectiles[0]);
			projectile6.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile6.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile6);
			projectile6.transform.parent = gun.barrelOffset;
			projectile6.baseData.damage *= 0;
			projectile6.SetProjectileSpriteRight("six_projectile", 10, 17);
			projectile6.name = "six";
			Numbas.Add(projectile6);
			//
			//7
			Projectile projectile7 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile7.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile7.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile7);
			projectile7.transform.parent = gun.barrelOffset;
			projectile7.baseData.damage *= 3f;
			projectile7.baseData.speed *= 3f;
			projectile7.SetProjectileSpriteRight("seven_projectile", 9, 17);
			projectile7.name = "7";

			Numbas.Add(projectile7);
			//8
			Projectile projectile8 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile8.gameObject.SetActive(false);
			BulletStatusEffectItem frostbullets = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>();
			GameActorFreezeEffect frostyfreezeyfreeze = frostbullets.FreezeModifierEffect;
			frostyfreezeyfreeze.FreezeAmount = 100f;
			projectile8.FreezeApplyChance = 1f;
			projectile8.AppliesFreeze = true;
			projectile8.baseData.damage *= 0;
			projectile8.freezeEffect = frostyfreezeyfreeze;
			FakePrefab.MarkAsFakePrefab(projectile8.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile8);
			projectile8.transform.parent = gun.barrelOffset;
			projectile8.SetProjectileSpriteRight("eight_projectile", 10, 17);
			projectile8.name = "eight";
			Numbas.Add(projectile8);
			//
			//9
			Projectile projectile9 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile9.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile9.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile9);
			projectile9.transform.parent = gun.barrelOffset;
			projectile9.baseData.damage = 350f;
			projectile9.baseData.speed /= 5;
			projectile9.SetProjectileSpriteRight("nine_projectile", 10, 17);
			projectile9.name = "nine";
			Numbas.Add(projectile9);
			//
			gun.muzzleFlashEffects = null;
			gun.DefaultModule.projectiles = new List<Projectile> { projectile };
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController player = gun.CurrentOwner as PlayerController;
			if (player.PlayerHasActiveSynergy("Bombsweeper") && UnityEngine.Random.value <= 0.09)
			{
				Tools.SpawnProjectile(player, 23, 15, 332);
			}
		}


		protected override void Update()
		{
			
			if (gun.CurrentOwner)
			{
				if (gun.CurrentOwner.CurrentGun == this.gun)
				{
					this.m_electricityImmunity = new DamageTypeModifier();
					this.m_electricityImmunity.damageMultiplier = 0f;
					this.m_electricityImmunity.damageType = CoreDamageTypes.Electric;
					gun.CurrentOwner.healthHaver.damageTypeModifiers.Add(this.m_electricityImmunity);
				}

			   gun.DefaultModule.projectiles[0] = Numbas[UnityEngine.Random.Range(0, Numbas.Count)];
			}
		}
	
	}
}
