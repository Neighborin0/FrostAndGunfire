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
	public class Centipede : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "centipede";
		private static tk2dSpriteCollectionData CentipedeCollection;
		public static GameObject shootpoint;


		public static void Init()
		{

				Centipede.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				
				prefab = EnemyBuilder.BuildPrefab("Centipede", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();;
				companion.aiActor.knockbackDoer.weight = 800;
				companion.aiActor.MovementSpeed = 6f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(30f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(55f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider

				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 15,
					ManualHeight = 17,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 15,
					ManualHeight = 17,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;

				AIAnimator aiAnimator = companion.aiAnimator;
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

					   "die_right",
						   "die_left"

							}

						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_right",
						"idle_left"
					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
						{
						"run_right",
						"run_left"
						}
				};
				
				bool flag3 = CentipedeCollection == null;
				if (flag3)
				{
					CentipedeCollection = SpriteBuilder.ConstructCollection(prefab, "Centipede_Collection");
					UnityEngine.Object.DontDestroyOnLoad(CentipedeCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], CentipedeCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CentipedeCollection, new List<int>
					{

					0,
					1

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CentipedeCollection, new List<int>
					{

                    2,
					3
						

					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CentipedeCollection, new List<int>
					{

					4,
					5,
					6,
					7,
					8,
					9
					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CentipedeCollection, new List<int>
					{

					10,
					11,
					12,
					13,
					14,
					15


					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CentipedeCollection, new List<int>
					{

				 16,
				 17,
				 18,



					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CentipedeCollection, new List<int>
					{

				19,
				20,
				21

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;

				}

				var bs = prefab.GetComponent<BehaviorSpeculator>();
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
					PauseTime = 0.25f
				}
			};
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootBehavior() {
					ShootPoint = m_CachedGunAttachPoint,
					BulletScript = new CustomBulletScriptSelector(typeof(CentipedeScript)),
					LeadAmount = 0f,
					AttackCooldown = 0.001f,
					RequiresLineOfSight = false,
					Uninterruptible = true,
					MaxUsages = 6
				}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = false,
					CustomRange = 1000f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("d5a7b95774cd41f080e517bea07bf495").behaviorSpeculator;
				//Tools.DebugInformation(load);
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("kp:centipede", companion.aiActor);


			}
		}

		public class CentipedeScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{
				float backwardsbullets = this.BulletBank.aiAnimator.FacingDirection * -1;
				Vector2 vector2 = BulletBank.aiActor.sprite.WorldCenter;
				AkSoundEngine.PostEvent("Play_WPN_stickycrossbow_shot_01", this.BulletBank.aiActor.gameObject);
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				}
				this.Fire(new Direction(backwardsbullets, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new CentipedeBullet());
				this.Fire(new Direction(backwardsbullets, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new CentipedeBullet1());
				this.Fire(new Direction(backwardsbullets, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new CentipedeBullet2());
				this.Fire(new Direction(backwardsbullets, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new CentipedeBullet3());
				this.Fire(new Direction(backwardsbullets, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new CentipedeBullet4());
				yield break;
			}
		}


		public class CentipedeBullet : Bullet
		{
			public CentipedeBullet() : base("default", false, false, false) { }
			protected override IEnumerator Top()
			{
				for (int i = 0; i < 1000; i++)
				{
					this.Position = this.BulletBank.aiActor.sprite.WorldCenter;
					UpdatePosition();
					yield return this.Wait(1);
				} 
				yield break;
			}
		}
		public class CentipedeBullet1 : Bullet
		{
			public CentipedeBullet1() : base("default", false, false, false) { }
			protected override IEnumerator Top()
			{
				for (int i = 0; i < 1000; i++)
				{
					
					this.Position = new Vector2(this.BulletBank.aiActor.sprite.WorldCenter.x - (this.BulletBank.aiActor.FacingDirection / this.BulletBank.aiActor.FacingDirection), this.BulletBank.aiActor.sprite.WorldCenter.y - (this.BulletBank.aiActor.FacingDirection / this.BulletBank.aiActor.FacingDirection));
					UpdatePosition();
			
					yield return this.Wait(1);
				}
				yield break;
			}
		}
		public class CentipedeBullet2 : Bullet
		{
			public CentipedeBullet2() : base("default", false, false, false) { }
			protected override IEnumerator Top()
			{
				for (int i = 0; i < 1000; i++)
				{
					this.Position = new Vector2(this.BulletBank.aiAnimator.FacingDirection * -2, this.BulletBank.aiAnimator.FacingDirection * -2);
					UpdatePosition();
					yield return this.Wait(1);
				}
				yield break;
			}
		}
		public class CentipedeBullet3 : Bullet
		{
			public CentipedeBullet3() : base("default", false, false, false) { }
			protected override IEnumerator Top()
			{
				for (int i = 0; i < 1000; i++)
				{
					this.Position = new Vector2(this.BulletBank.aiAnimator.FacingDirection * -3, this.BulletBank.aiAnimator.FacingDirection * -3);
					UpdatePosition();
					yield return this.Wait(1);
				}
				yield break;
			}
		}
		public class CentipedeBullet4 : Bullet
		{
			public CentipedeBullet4() : base("default", false, false, false) { }
			protected override IEnumerator Top()
			{
				for (int i = 0; i < 1000; i++)
				{
					this.Position = new Vector2(this.BulletBank.aiAnimator.FacingDirection * -4, this.BulletBank.aiAnimator.FacingDirection * -4);
					UpdatePosition();
					yield return this.Wait(1);
				}
				yield break;
			}
		}

		private static string[] spritePaths = new string[]
		{
			
			//idles
			"FrostAndGunfireItems/Resources/shellet/shellet_idle_left_001",
			"FrostAndGunfireItems/Resources/shellet/shellet_idle_left_002",
			"FrostAndGunfireItems/Resources/shellet/shellet_idle_right_001",
			"FrostAndGunfireItems/Resources/shellet/shellet_idle_right_002",
			//run
			"FrostAndGunfireItems/Resources/shellet/shellet_run_left_001",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_left_002",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_left_003",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_left_004",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_left_005",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_left_006",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_right_001",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_right_002",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_right_003",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_right_004",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_right_005",
			"FrostAndGunfireItems/Resources/shellet/shellet_run_right_006",
			//death
			"FrostAndGunfireItems/Resources/shellet/shellet_die_right_001",
			"FrostAndGunfireItems/Resources/shellet/shellet_die_right_002",
			"FrostAndGunfireItems/Resources/shellet/shellet_die_right_003",
			"FrostAndGunfireItems/Resources/shellet/shellet_die_left_001",
			"FrostAndGunfireItems/Resources/shellet/shellet_die_left_002",
			"FrostAndGunfireItems/Resources/shellet/shellet_die_left_003",
				};

		public class EnemyBehavior : BraveBehaviour
		{
			
			private RoomHandler m_StartRoom;
			private void Update() {
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
					AkSoundEngine.PostEvent("Play_OBJ_skeleton_collapse_01", base.aiActor.gameObject);
				};
			}


		}

	
	}
}

	





	

