using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using static DirectionalAnimation;

namespace FrostAndGunfireItems
{
	public class Milton : AIActor
	{
		public static GameObject prefab;
		//Always make sure to give your enemy a unique guid. This is essentially the id of your enemy and is integral for many parts of EnemyAPI
		public static readonly string guid = "milton";
		//This shootpoint gameObject determines well, where the enemy shoots from, I'll explain more when we get to the AttackBehaviors.
		public static GameObject shootpoint;
		public static void Init()
		{
			//As always don't forget to initalize your enemy. 
			Milton.BuildPrefab();
		}
		public static void BuildPrefab()
		{

			if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				//Sets up the prefab of the enemy. The spritepath, "FrostAndGunfireItems/Resources/Milton/Idle/milton_idle_001", determines the setup sprite for your enemy. vvvv This bool right here determines whether or not an enemy has an AiShooter or not. AIShooters are necessary if you want your enemy to hold a gun for example. An example of this can be seen in Humphrey.
				prefab = EnemyBuilder.BuildPrefab("Milton", guid, "FrostAndGunfireItems/Resources/Milton/Idle/milton_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false);
				//This line extends a BraveBehavior called EnemyBehavior, this is a generic behavior I use for setting up things that can't be setup in BuildPrefab.
				var enemy = prefab.AddComponent<EnemyBehavior>();
				//Here you can setup various things like movement speed, weight, and health. There's a lot you can do with the AiActor parameter so feel free to experiment.
				enemy.aiActor.MovementSpeed = 2f;
				enemy.aiActor.knockbackDoer.weight = 100;
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.CollisionDamage = 1f;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(10f);
				enemy.aiActor.healthHaver.SetHealthMaximum(22f, null, false);
				//This is where you setup your animations. Most animations need specific frame names to be recognized like idle or die. 
				//The AddAnimation lines gets sprites from the folder specified in second phrase of the this line. At the very least you need an animation that contains the word idle for the idle animations for example.
				//AnimationType determines what kind of animation your making. In Gungeon there are 7 different Animation Types: Move, Idle, Fidget, Flight, Hit, Talk, Other. For a majority of these animations, these play automatically, however specific animations need to be told when to play such as Attack.
				//DirectionType determines the amount of ways an animation can face. You'll have to change your animation names to correspond with the DirectionType. For example if you want an animation to face eight ways you'll have to name your animations something like ""attack_south_west", "attack_north_east",  "attack_east", "attack_south_east",  "attack_north",  "attack_south", "attack_west", "attack_north_west" and change DirectionType to  DirectionType.EightWayOrdinal.
				//I suggest looking at the sprites of base game enemies to determine the names for the different directions.
				prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/Milton/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
				prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/Milton/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
				prefab.AddAnimation("run_left", "FrostAndGunfireItems/Resources/Milton/Move", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal);
				prefab.AddAnimation("run_right", "FrostAndGunfireItems/Resources/Milton/Move", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal);
				//Note that the "die" and "attack" animations are only set to Move because they will be overwritten later.
				//tk2dSpriteAnimationClip.WrapMode.Once determines how an animation plays out. If you don't want it to loop, leave it to Once, otherwise you can change it to Loop or something.
				//Assign animation well assigns an animation to an animation type. By default this is on, but since we're overwritting this set this to false.	
				prefab.AddAnimation("attack_left", "FrostAndGunfireItems/Resources/Milton/Attack", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal , tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("attack_right", "FrostAndGunfireItems/Resources/Milton/Attack", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_left", "FrostAndGunfireItems/Resources/Milton/Die", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_right", "FrostAndGunfireItems/Resources/Milton/Die", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				//Here we create a new DirectionalAnimation for our enemy to pull from. 
				//Make sure the AnimNames correspong to the AddAnimation names.
				DirectionalAnimation attack = new DirectionalAnimation()
				{
					AnimNames = new string[] { "attack_right", "attack_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty
				};
				DirectionalAnimation die = new DirectionalAnimation()
				{
					AnimNames = new string[] { "die_right", "die_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty
				};
				//Because Dodge Roll is Dodge Roll and there is no animation types for attack and death, we have to assign them to the Other category.
				enemy.aiAnimator.AssignDirectionalAnimation("attack", attack, AnimationType.Other);
				enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
				//This is where we get into the meat and potatoes of our enemy. This is where all the behaviors of our enemy are made.
				//This shootpoint block of code determines where our bullets will orginate from. In this case, the center of the enemy.
				shootpoint = new GameObject("milton center");
				shootpoint.transform.parent = enemy.transform;
				shootpoint.transform.position = enemy.sprite.WorldCenter;
				GameObject position = enemy.transform.Find("milton center").gameObject;
				//this line adds a BehaviorSpeculator to our enemy which is the base for adding behaviors on to.
				var bs = prefab.GetComponent<BehaviorSpeculator>();
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
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootBehavior() {
					ShootPoint = position,
					//This line selects our Bullet Script
					BulletScript = new CustomBulletScriptSelector(typeof(MiltonScript)),
					LeadAmount = 0f,
					AttackCooldown = 4f,
					FireAnimation = "attack",
					RequiresLineOfSight = true,
					Uninterruptible = false,
				}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = false,
					CustomRange = 15f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				//Adds the enemy to MTG spawn pool and spawn command
				Game.Enemies.Add("kp:milton", enemy.aiActor);
			}
		}
		public class EnemyBehavior : BraveBehaviour
		{
			//This determines that the enemy is active when a player is in the room
			private RoomHandler m_StartRoom;
			private void Update()
			{
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
			}
			private void CheckPlayerRoom()
			{
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom){base.aiActor.HasBeenEngaged = true;}
			}
			private void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//This line determines what happens when an enemy dies. For now it's something simple like playing a death sound.
				//A full list of all the sounds can be found in the SFX.txt document that comes with this github.
				base.aiActor.healthHaver.OnPreDeath += (obj) =>{AkSoundEngine.PostEvent("Play_VO_kali_death_01", base.aiActor.gameObject);};
			}
		}
		public class MiltonScript : Script 
		{
			//Here we are going to create a basic bullet script.
			protected override IEnumerator Top() 
			{
				//In this line we're going to take the fiery bullet from Muzzle Flare and it to our enemy.
				//In this case, the giery bullet is the default bullet of the Muzzle Flare.
				//Try looking up the bullet scripts of other enemies in DnSpy to try to find thier bullet names.
				//The Guid of all enemies can be found in the "enemies" file of the github.
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody){base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("d8a445ea4d944cc1b55a40f22821ae69").bulletBank.GetBullet("default"));}
				//Here the code is going to count each "i" and fire a bullet from each "i" found.
				for (int i = 0; i < 2; i++)
				{
					//Here we can determine the direction of our bullet, speed, and assign a Bullet class. 
					//Bullets classes allow you to do some wacky shit, but for now, lets keep it simple.
					//Here the direction the bullet will be fired in will be a 40 degree angle * for each "i" away from the aim of our enemy, which a speed of 4.
					base.Fire(new Direction(i * 40, Brave.BulletScript.DirectionType.Aim, -1f), new Speed(4f, SpeedType.Absolute), new MiltonBullet());
					yield return this.Wait(6);//Here the script will wait 6 frames before firing another shot.
				}
				yield break;
			}
		}
		public class MiltonBullet : Bullet
		{
			//la bullet
			public MiltonBullet() : base("default", false, false, false)
			{
			}
			
				protected override IEnumerator Top() //In the IEnumerator we can do a lot of stuff with out bullet. You could reverse the speed, make it home in on players, like always don't be afraid to look at the Bullet Scripts of enemies and experiment.
			{
				//Here we're going to have our bullet slow down by 1 every 20 frames.
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(3f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(2f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(1f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				yield break;
			    }
		    }
		}
	}


	





	

