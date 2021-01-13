using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	public class Ophaims : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "ophaim";
		private static tk2dSpriteCollectionData OphaimCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			Ophaims.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				
				prefab = EnemyBuilder.BuildPrefab("Ophaim", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
				companion.aiActor.knockbackDoer.weight = 1000;
				companion.aiActor.MovementSpeed = 5f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.healthHaver.SetHealthMaximum(15f, null, false);
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle"
						
						

					}
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "die",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
							Flipped = new DirectionalAnimation.FlipType[2],
							AnimNames = new string[]
							{

					   "die",
							}

						}
					}
				};
				
				bool flag3 = OphaimCollection == null;
				if (flag3)
				{
					OphaimCollection = SpriteBuilder.ConstructCollection(prefab, "Ophaims_Collection");
					UnityEngine.Object.DontDestroyOnLoad(OphaimCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], OphaimCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, OphaimCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					


					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, OphaimCollection, new List<int>
					{

					6,
					7,
					8,
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;

				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				float[] angles = { 45, 135, 225, 135 };
				prefab.GetComponent<ObjectVisibilityManager>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
			    bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("fuck");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("fuck").gameObject;
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
					ShootPoint = m_CachedGunAttachPoint,
					BulletScript = new CustomBulletScriptSelector(typeof(OphaimScript)),
					LeadAmount = 0f,
					AttackCooldown = 1f,
					RequiresLineOfSight = false,
					Uninterruptible = true
				}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new PingPongAroundBehavior
				{
					motionType = PingPongAroundBehavior.MotionType.Diagonals,
					startingAngles = angles
				}
			};
				
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
			    Game.Enemies.Add("kp:ophaim", companion.aiActor);
			}
		}




		private static string[] spritePaths = new string[]
		{
			
			//idles
			"FrostAndGunfireItems/Resources/ophaim/ophaim_idle_001",
			"FrostAndGunfireItems/Resources/ophaim/ophaim_idle_002",
			"FrostAndGunfireItems/Resources/ophaim/ophaim_idle_003",
			"FrostAndGunfireItems/Resources/ophaim/ophaim_idle_004",
			"FrostAndGunfireItems/Resources/ophaim/ophaim_idle_005",
			"FrostAndGunfireItems/Resources/ophaim/ophaim_idle_006",
			//die
			"FrostAndGunfireItems/Resources/ophaim/ophaim_die_001",
			"FrostAndGunfireItems/Resources/ophaim/ophaim_die_002",
			"FrostAndGunfireItems/Resources/ophaim/ophaim_die_003",
				};

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
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_OBJ_stone_crumble_01", base.aiActor.gameObject);
				};

			}


		}

		public class OphaimScript : Script
		{
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				}
				for (int i = 0; i < 4; i++)
				{
					this.Fire(new Direction((float)(i * 80), DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new DefaultBullet());
				}
				yield break;
			}
		}

		public class DefaultBullet : Bullet
		{
			public DefaultBullet() : base("default", false, false, false) { }
			
		}

	}
}

	





	

