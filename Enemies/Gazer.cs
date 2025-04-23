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
	public class Gazer : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "gazer";
		public static GameObject shootpoint;
		public static void BuildPrefab()
		{
			if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				{
					prefab = EnemyBuilder.BuildPrefab("Gazer", guid, "FrostAndGunfireItems/Resources/gazer/Idle/gazer_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false, false);
					var enemy = prefab.AddComponent<EnemyBehavior>();
					enemy.aiActor.knockbackDoer.weight = float.MaxValue;
					enemy.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
					enemy.aiActor.MovementSpeed = 2f;
					enemy.aiActor.CollisionDamage = 1f;
					enemy.aiActor.IgnoreForRoomClear = false;
					enemy.aiActor.healthHaver.ForceSetCurrentHealth(25f);
					enemy.aiActor.CollisionKnockbackStrength = 1f;		
					enemy.aiActor.healthHaver.SetHealthMaximum(25f, null, false);
					prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/gazer/Idle", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/gazer/Idle", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("tell_right", "FrostAndGunfireItems/Resources/gazer/Tell", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("tell_left", "FrostAndGunfireItems/Resources/gazer/Tell", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("attack_right", "FrostAndGunfireItems/Resources/gazer/Attack", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("attack_left", "FrostAndGunfireItems/Resources/gazer/Attack", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("die_right", "FrostAndGunfireItems/Resources/gazer/Death", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("die_left", "FrostAndGunfireItems/Resources/gazer/Death", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				
					DirectionalAnimation tell = new DirectionalAnimation()
					{
						AnimNames = new string[] { "tell_right", "tell_left" },
						Flipped = new FlipType[] { FlipType.None, FlipType.None },
						Type = DirectionType.TwoWayHorizontal,
						Prefix = string.Empty
					};
					DirectionalAnimation die = new DirectionalAnimation()
					{
						AnimNames = new string[] { "die_right", "die_left" },
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
					enemy.aiAnimator.AssignDirectionalAnimation("attack", attack, AnimationType.Other);
					enemy.aiAnimator.AssignDirectionalAnimation("tell", tell, AnimationType.Other);
					enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
					Vector2 offset = new Vector2(0, .13f);
					shootpoint = new GameObject("fuck");
					shootpoint.transform.parent = enemy.transform;
					shootpoint.transform.position = enemy.sprite.WorldCenter + offset;
					AIBeamShooter aIBeamShooter = prefab.GetOrAddComponent<AIBeamShooter>();
					AIActor actor = EnemyDatabase.GetOrLoadByGuid("21dd14e5ca2a4a388adab5b11b69a1e1");
					AIBeamShooter aIBeamShooter2 = actor.GetComponent<AIBeamShooter>();
					aIBeamShooter.beamTransform = shootpoint.transform;
					aIBeamShooter.beamModule = aIBeamShooter2.beamModule;
					aIBeamShooter.beamProjectile = aIBeamShooter2.beamProjectile;
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
					bs.AttackBehaviors = new List<AttackBehaviorBase>
					{
					new ShootBeamBehavior() {
					AttackCooldown = 4,	
					unitCatchUpSpeed = 1f,
					FireAnimation = "attack",
					TellAnimation = "tell",
					degTurnRateAcceleration = 30f,
					beamSelection = ShootBeamBehavior.BeamSelection.All,
					firingTime = 3f,
					stopWhileFiring = true,
					restrictBeamLengthToAim = false,
					RequiresLineOfSight = true,
					trackingType = ShootBeamBehavior.TrackingType.Follow,
					initialAimType = ShootBeamBehavior.InitialAimType.Aim,
					specificBeamShooter = aIBeamShooter
					 }
					};
					bs.MovementBehaviors = new List<MovementBehaviorBase>
					{
					new SeekTargetBehavior
					{
					StopWhenInRange = false,
					CustomRange = 10f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 100f,
					MaxActiveRange = 100f
					}
					};
					Game.Enemies.Add("kp:gazer", enemy.aiActor);
				}
			}

		}
		public class EnemyBehavior : BraveBehaviour
		{
			private void Start()
			{
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					Exploder.DoDefaultExplosion(base.aiActor.sprite.WorldCenter, Vector2.zero);
				};
			}
		}
	}
}





	





	

