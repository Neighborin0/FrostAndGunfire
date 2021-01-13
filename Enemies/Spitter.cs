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
	public class Spitter : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "spitter";
		public static GameObject shootpoint;
		public static void Init()
		{
			Spitter.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				{
					prefab = EnemyBuilder.BuildPrefab("Spitter", guid, "FrostAndGunfireItems/Resources/Spitter/Idle_Left/spitter_idle_left_001", new IntVector2(0, 0), new IntVector2(0, 0), false);
					var enemy = prefab.AddComponent<EnemyBehavior>();
					enemy.aiActor.knockbackDoer.weight = 1000;
					enemy.aiActor.MovementSpeed = 0.5f;
					enemy.aiActor.CollisionDamage = 1f;
					enemy.aiActor.EffectResistances = new ActorEffectResistance[] { new ActorEffectResistance() { resistAmount = 1, resistType = EffectResistanceType.Poison }, };
					enemy.aiActor.IgnoreForRoomClear = false;
					enemy.aiActor.healthHaver.ForceSetCurrentHealth(50f);
					enemy.aiActor.CollisionKnockbackStrength = 5f;
					enemy.aiActor.healthHaver.SetHealthMaximum(50f, null, false);	
					prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/Spitter/Idle_Right", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/Spitter/Idle_Left", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("move_right", "FrostAndGunfireItems/Resources/Spitter/Move_Right", fps: 3, AnimationType.Move, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("move_left", "FrostAndGunfireItems/Resources/Spitter/Move_Left", fps: 3, AnimationType.Move, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("tell_right", "FrostAndGunfireItems/Resources/Spitter/Tell_Right", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("tell_left", "FrostAndGunfireItems/Resources/Spitter/Tell_Left", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("attack_right", "FrostAndGunfireItems/Resources/Spitter/Attack_Right", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("attack_left", "FrostAndGunfireItems/Resources/Spitter/Attack_Left", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("die_right", "FrostAndGunfireItems/Resources/Spitter/Die_Right", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("die_left", "FrostAndGunfireItems/Resources/Spitter/Die_Left", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					DirectionalAnimation die = new DirectionalAnimation()
					{
						AnimNames = new string[] { "die_right", "die_left" },
						Flipped = new FlipType[] { FlipType.None, FlipType.None },
						Type = DirectionType.TwoWayHorizontal,
						Prefix = string.Empty
					};
					DirectionalAnimation tell = new DirectionalAnimation()
					{
						AnimNames = new string[] { "tell_right", "tell_left" },
						Flipped = new FlipType[] { FlipType.None, FlipType.None },
						Type = DirectionType.TwoWayHorizontal,
						Prefix = string.Empty
					};
					DirectionalAnimation attack = new DirectionalAnimation()
					{
						AnimNames = new string[] { "attack_right", "attack_left" },
						Flipped = new FlipType[] { FlipType.None, FlipType.None },
						Type = DirectionType.TwoWayHorizontal,
						Prefix = string.Empty
					};
					enemy.aiAnimator.AssignDirectionalAnimation("tell", tell, AnimationType.Other);
					enemy.aiAnimator.AssignDirectionalAnimation("attack", attack, AnimationType.Other);
					enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
					shootpoint = new GameObject("center");
					shootpoint.transform.parent = enemy.transform;
					shootpoint.transform.position = enemy.sprite.WorldCenter;
					GameObject position = enemy.transform.Find("center").gameObject;
					var bs = prefab.GetComponent<BehaviorSpeculator>();
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
					bs.AttackBehaviors = new List<AttackBehaviorBase>() {
					new ShootBehavior() {
					ShootPoint = position,
					BulletScript = new CustomBulletScriptSelector(typeof(SpitterScript)),
					LeadAmount = 0f,
					AttackCooldown = 4f,
					ContinueAimingDuringTell = true,
					TellAnimation = "tell",
					FireAnimation = "attack",
					RequiresLineOfSight = true,
					Uninterruptible = false,
					StopDuring = ShootBehavior.StopType.Attack,
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
					Game.Enemies.Add("kp:gunzooka", enemy.aiActor);
				}
			}
			
		}

		public class EnemyBehavior : BraveBehaviour
		{

			private RoomHandler m_StartRoom;

			private void Update()
			{
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }	
			}
			private void CheckPlayerRoom()
			{

				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					GameManager.Instance.StartCoroutine(LateEngage());
				}

			}

			private IEnumerator LateEngage()
			{
				yield return new WaitForSeconds(0.5f);
				base.aiActor.HasBeenEngaged = true;
				yield break;
			}
			private void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_ENM_beholster_death_01", base.aiActor.gameObject);
				};
			}
		}

		public class SpitterScript : Script
		{
			protected override IEnumerator Top() 
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				}
				AkSoundEngine.PostEvent("Play_ENM_blobulord_bubble_01", this.BulletBank.aiActor.gameObject);
				base.Fire(new Direction(0f, Brave.BulletScript.DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), new SpitBurst());
				yield break;
			}
		}
		public class SpitBurst : Bullet
		{
			public SpitBurst() : base("bigBullet", false, false, false)
			{
				
			}
			protected override IEnumerator Top()
			{
				AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
				GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
				var goop = this.Projectile.gameObject.AddComponent<GoopDoer>();
				goop.goopDefinition = goopDef;
				goop.positionSource = GoopDoer.PositionSource.HitBoxCenter;
				goop.updateTiming = GoopDoer.UpdateTiming.Always;
				goop.updateFrequency = 0.05f;
				goop.isTimed = false;
				goop.goopTime = 1;
				goop.updateOnPreDeath = true;
				goop.updateOnDeath = false;
				goop.updateOnAnimFrames = true;
				goop.updateOnCollision = false;
				goop.updateOnGrounded = false;
				goop.updateOnDestroy = false;
				goop.defaultGoopRadius = 1f;
				goop.suppressSplashes = false;
				goop.goopSizeVaries = true;
				goop.varyCycleTime = 0.9f;
				goop.radiusMin = 1f;
				goop.radiusMax = 1f;
				goop.goopSizeRandom = false;
				goop.UsesDispersalParticles = false;
				goop.DispersalDensity = 3;
				goop.DispersalMinCoherency = 0.2f;
				goop.DispersalMaxCoherency = 1;
				yield return this.Wait(120);
				this.ChangeSpeed(new Speed(3f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(2f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(1f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 20);
				yield return this.Wait(10);
				this.Vanish(true);
				yield break;
			}

			public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				if (preventSpawningProjectiles){return;}
				for (int i = 0; i < 8; i++)
				{
					base.Fire(new Direction((float)(i * 45), Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new WallBullet());
				}
				AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
				GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
				DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
				goopManagerForGoopType.TimedAddGoopCircle(this.Projectile.sprite.WorldCenter, 3f, 0.35f, false);
				goopDef.damagesEnemies = false;
			}


		}
		public class WallBullet : Bullet
		{
			public WallBullet() : base("default", false, false, false)
			{
			}

		}
	}
}




	





	

