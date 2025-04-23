using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using Dungeonator;
using GungeonAPI;
using System.Collections;
using Gungeon;
using static DirectionalAnimation;

namespace FrostAndGunfireItems
{
    public class Spike : AIActor
	{
        public static GameObject prefab;
        public static readonly string guid = "Spike";
		public float InternalCooldown;
		private float m_lastUsedTime;

		public static void Init()
        {
			Spike.BuildPrefab();
		}

		public static void BuildPrefab()
        {
			if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				//Sets up the prefab of the enemy. The spritepath, "FrostAndGunfireItems/Resources/Milton/Idle/milton_idle_001", determines the setup sprite for your enemy. vvvv This bool right here determines whether or not an enemy has an AiShooter or not. AIShooters are necessary if you want your enemy to hold a gun for example. An example of this can be seen in here.
				prefab = EnemyBuilder.BuildPrefab("Spike", guid, "FrostAndGunfireItems/Resources/Milton/Idle/milton_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), true, false);
				//This line extends a BraveBehavior called EnemyBehavior, this is a generic behavior I use for setting up things that can't be setup in BuildPrefab.
				var enemy = prefab.AddComponent<EnemyBehavior>();
				//Here you can setup various things like movement speed, weight, and health. There's a lot you can do with the AiActor parameter so feel free to experiment.
				//For Humphrey let's make him a flying enemy
				enemy.aiActor.MovementSpeed = 2f;
				enemy.aiActor.knockbackDoer.weight = 100;
				enemy.aiActor.IgnoreForRoomClear = true;
				enemy.aiActor.CollisionDamage = 1f;
				enemy.aiActor.CanTargetEnemies = true;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(30f);
				enemy.aiActor.healthHaver.SetHealthMaximum(45f, null, false);
				//This is where you setup your animations. Most animations need specific frame names to be recognized like idle or die. 
				//The AddAnimation lines gets sprites from the folder specified in second phrase of the this line. At the very least you need an animation that contains the word idle for the idle animations for example.
				//AnimationType determines what kind of animation your making. In Gungeon there are 7 different Animation Types: Move, Idle, Fidget, Flight, Hit, Talk, Other. For a majority of these animations, these play automatically, however specific animations need to be told when to play such as Attack.
				//DirectionType determines the amount of ways an animation can face. You'll have to change your animation names to correspond with the DirectionType. For example if you want an animation to face eight ways you'll have to name your animations something like ""attack_south_west", "attack_north_east",  "attack_east", "attack_south_east",  "attack_north",  "attack_south", "attack_west", "attack_north_west" and change DirectionType to  DirectionType.EightWayOrdinal.
				//I suggest looking at the sprites of base game enemies to determine the names for the different directions.
				prefab.AddAnimation("idle_back_right", "FrostAndGunfireItems/Resources/Humphrey/Idle_Back_Right", fps: 5, AnimationType.Idle, DirectionType.FourWay);
				prefab.AddAnimation("idle_front_left", "FrostAndGunfireItems/Resources/Humphrey/Idle_Front_Left", fps: 5, AnimationType.Idle, DirectionType.FourWay);
				prefab.AddAnimation("idle_front_right", "FrostAndGunfireItems/Resources/Humphrey/Idle_Front_Right", fps: 5, AnimationType.Idle, DirectionType.FourWay);
				prefab.AddAnimation("idle_back_left", "FrostAndGunfireItems/Resources/Humphrey/Idle_Back_Left", fps: 5, AnimationType.Idle, DirectionType.FourWay);
				//Note that the "die"animations are only set to Move because they will be overwritten later.
				//tk2dSpriteAnimationClip.WrapMode.Once determines how an animation plays out. If you don't want it to loop, leave it to Once, otherwise you can change it to Loop or something.
				//Assign animation well, assigns an animation to an animation type. By default this is on, but since we're overwritting this set this to false.	
				prefab.AddAnimation("die_left", "FrostAndGunfireItems/Resources/Humphrey/Die", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_right", "FrostAndGunfireItems/Resources/Humphrey/Die", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				//Here we create a new DirectionalAnimation for our enemy to pull from. 
				//Make sure the AnimNames correspong to the AddAnimation names.		
				DirectionalAnimation die = new DirectionalAnimation()
				{
					AnimNames = new string[] { "die_right", "die_left" },
					Flipped = new FlipType[] { FlipType.None, FlipType.None },
					Type = DirectionType.TwoWayHorizontal,
					Prefix = string.Empty
				};
				//Because Dodge Roll is Dodge Roll and there is no animation types for death, we have to assign them to the Other category.
				enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
				//This is where we get into the meat and potatoes of our enemy. This is where all the behaviors of our enemy are made.

				//this line adds a BehaviorSpeculator to our enemy which is the base for adding behaviors on to. 
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				//We're also going to duplicate the bulletbank and aishooter from the Bullet Kin.
				//The "762" represents the the gun id we want our enemy to hold. For a list of all gun ids, look at the items file on this github.
				//For now let's give him the finished gun.
				GameObject position = enemy.transform.Find("GunAttachPoint").gameObject;
				AIActor SourceEnemy = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
				EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, SourceEnemy.aiShooter, SourceEnemy.GetComponent<AIBulletBank>(), 762, position.transform);

				//Here we will add some basic behaviors such as TargetPlayerBehavior and SeekTargetBehavior.
				//You can change many things in these behaviors so feel free to go nuts.
				bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = true,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f,
				}
			};
				//Now this is one of the most important behaviors because it allows our enemy to shoot.
				//If we want our enemy to shoot out of a gun, use ShootGunBehavior	
				bs.AttackBehaviors = new List<AttackBehaviorBase>
				{
					new ShootGunBehavior{
					GroupCooldownVariance = 0.2f,
					LineOfSight = true,
					//With weapon type you can determine if you want your enemy to fire the gun directly or to use a bullet script. Let's use a Bullet Script for now. For the rest of code, let's modify it to be close to the Finished Gun.
					WeaponType = WeaponType.AIShooterProjectile,
					OverrideBulletName = null,
					BulletScript = null,
					FixTargetDuringAttack = true,
					StopDuringAttack = true,
					LeadAmount = 0,
					LeadChance = 1,
					RespectReload = true,
					MagazineCapacity = 6,
					ReloadSpeed = 2f,
					EmptiesClip = true,
					SuppressReloadAnim = false,
					TimeBetweenShots = -1,
					PreventTargetSwitching = true,
					OverrideAnimation = null,
					OverrideDirectionalAnimation = null,
					HideGun = false,
					UseLaserSight = false,
					UseGreenLaser = false,
					PreFireLaserTime = -1,
					AimAtFacingDirectionWhenSafe = false,
					Cooldown = 0.7f,
					CooldownVariance = 0,
					AttackCooldown = 0,
					GlobalCooldown = 0,
					InitialCooldown = 0,
					InitialCooldownVariance = 0,
					GroupName = null,
					GroupCooldown = 0,
					MinRange = 0,
					Range = 16,
					MinWallDistance = 0,
					MaxEnemiesInRoom = 0,
					MinHealthThreshold = 0,
					MaxHealthThreshold = 1,
					HealthThresholds = new float[0],
					AccumulateHealthThresholds = true,
					targetAreaStyle = null,
					IsBlackPhantom = false,
					resetCooldownOnDamage = null,
					RequiresLineOfSight = true,
					MaxUsages = 0,
					}
			};

				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = true,
					CustomRange = 15f,
					LineOfSight = true,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new CompanionFollowPlayerBehavior
				{
				 CanRollOverPits = true,
				 DisableInCombat = true,
				 CatchUpSpeed = 5f,
				}
			};
				//Adds the enemy to MTG spawn pool and spawn command
				Game.Enemies.Add("kp:spike", enemy.aiActor);
			}
		}

		public class SpikeBehavior : CompanionController
		{
			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325
			private void Start()
			{
				
				base.spriteAnimator.Play("idle");
				
				this.Owner = this.m_owner;
			}

			// Token: 0x0400006A RID: 106
			public PlayerController Owner;
		}



	}
}
