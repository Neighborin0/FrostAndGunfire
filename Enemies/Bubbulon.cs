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
	public class Bubbulon : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bubbulon";
		public static void BuildPrefab()
		{
			if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				{
					prefab = EnemyBuilder.BuildPrefab("Bubbulon", guid, "FrostAndGunfireItems/Resources/bubbulon/Idle/bubbulon_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false, false);
					var enemy = prefab.AddComponent<EnemyBehavior>();
					enemy.aiActor.knockbackDoer.weight = float.MaxValue;
					enemy.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
					enemy.aiActor.MovementSpeed = 6f;
					enemy.aiActor.CollisionDamage = 1f;
					
					enemy.aiActor.IgnoreForRoomClear = false;
					enemy.aiActor.healthHaver.ForceSetCurrentHealth(15f);
					enemy.aiActor.CollisionKnockbackStrength = 1f;
					enemy.aiActor.healthHaver.SetHealthMaximum(15f, null, false);
					prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/bubbulon/Idle", fps: 9, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					
					prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/bubbulon/Idle", fps: 9, AnimationType.Idle, DirectionType.TwoWayHorizontal);	
					prefab.AddAnimation("die_right", "FrostAndGunfireItems/Resources/bubbulon/Death", fps: 14, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("die_left", "FrostAndGunfireItems/Resources/bubbulon/Death", fps: 14, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				
					DirectionalAnimation die = new DirectionalAnimation()
					{
						AnimNames = new string[] { "die_right", "die_left" },
						Flipped = new FlipType[] { FlipType.None, FlipType.None },
						Type = DirectionType.TwoWayHorizontal,
						Prefix = string.Empty
					};
					enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
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
					float[] angles = { 45, 135, 225, 135 };
					bs.MovementBehaviors = new List<MovementBehaviorBase>
					{
					new PingPongAroundBehavior
					{
					motionType = PingPongAroundBehavior.MotionType.Diagonals,
					startingAngles = angles
					}
					};
					enemy.aiActor.specRigidbody.PixelColliders.Clear();
					enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
					{
						ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
						CollisionLayer = CollisionLayer.EnemyCollider,
						IsTrigger = false,
						BagleUseFirstFrameOnly = false,
						SpecifyBagelFrame = string.Empty,
						ManualOffsetX = 30,
						ManualOffsetY = 9,
						ManualWidth = 20,
						ManualHeight = 20,
						ManualDiameter = 30,
						ManualLeftX = 30,
						ManualLeftY = 30,
						ManualRightX = 30,
						ManualRightY = 30
					});
					enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider

					{
						ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
						CollisionLayer = CollisionLayer.EnemyHitBox,
						IsTrigger = false,
						BagleUseFirstFrameOnly = false,
						SpecifyBagelFrame = string.Empty,
						BagelColliderNumber = 0,
						ManualOffsetX = 25,
						ManualOffsetY = 9,
						ManualWidth = 20,
						ManualHeight = 20,
						ManualDiameter = 0,
						ManualLeftX = 0,
						ManualLeftY = 0,
						ManualRightX = 0,
						ManualRightY = 0
					});
					Game.Enemies.Add("kp:bubbulon", enemy.aiActor);
				}
			}

		}

		public class EnemyBehavior : BraveBehaviour
		{
			private void Start()
			{
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_BOSS_blobulord_burst_01", base.aiActor.gameObject);
				};
			}
		}

	}
}


	





	

