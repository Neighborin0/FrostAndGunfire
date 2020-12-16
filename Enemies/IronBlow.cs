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
	public class IronBlow : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "iron blow";
		private static tk2dSpriteCollectionData IronBlowCollection;


		public static void Init()
		{

			IronBlow.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//AIActor source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("IronBlow", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 1000;
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0.05f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(100f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(45f, null, false);
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
				aiAnimator.HitAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
				{
						"hit_right",
						"hit_left"
				}
				};
				bool flag3 = IronBlowCollection == null;
				if (flag3)
				{
					IronBlowCollection = SpriteBuilder.ConstructCollection(prefab, "IronBlow_Collection");
					UnityEngine.Object.DontDestroyOnLoad(IronBlowCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], IronBlowCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, IronBlowCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, IronBlowCollection, new List<int>
					{
					
						5,
						6,
						7,
						8,
						9

					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					

				}

				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
			
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = false,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f
				}
			};
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootBehavior() {
					ShootPoint = prefab,
					BulletScript = new CustomBulletScriptSelector(typeof(TestBulletScript)),
					LeadAmount = 0f,
					AttackCooldown = 5f,
					HideGun = true,
					RequiresLineOfSight = true,
							
				}
			};
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("206405acad4d4c33aac6717d184dc8d4").behaviorSpeculator;
				//Tools.DebugInformation(load);
				AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("kp:iron_blow", companion.aiActor);


			}
		}



		private static string[] spritePaths = new string[]
		{
			
			//idles
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_left_001",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_left_002",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_left_003",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_left_004",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_left_005",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_right_001",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_right_002",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_right_003",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_right_004",
			"FrostAndGunfireItems/Resources/silencer/silencer_idle_right_005",
			//dance
			"FrostAndGunfireItems/Resources/silencer/silencer_dance_001",
			"FrostAndGunfireItems/Resources/silencer/silencer_dance_002",
			"FrostAndGunfireItems/Resources/silencer/silencer_dance_003",
			"FrostAndGunfireItems/Resources/silencer/silencer_dance_004",
			//death
			"FrostAndGunfireItems/Resources/silencer/silencer_die_left_001",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_left_002",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_left_003",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_left_004",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_left_005",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_left_006",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_right_001",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_right_002",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_right_003",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_right_004",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_right_005",
			"FrostAndGunfireItems/Resources/silencer/silencer_die_right_006",
			//hit
				"FrostAndGunfireItems/Resources/silencer/silencer_hit_right_001",
				"FrostAndGunfireItems/Resources/silencer/silencer_hit_left_001",


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
					base.aiActor.HasBeenEngaged = true;
				}

			}
			private void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_VO_kali_death_01", base.aiActor.gameObject);
				};
			}


		}
	

		public class TestBulletScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{
				
				base.Fire(new Direction(-70f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(70f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-60f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(60f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-50f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(50f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-40f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(40f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-30f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(30f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-20f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(20f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-10f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(10f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				yield return Wait(70);

				base.Fire(new Direction(-70f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(70f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-60f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(60f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-50f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(50f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-40f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(40f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-30f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(30f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-20f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(20f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-10f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(10f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				yield return Wait(70);
				base.Fire(new Direction(-70f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(70f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-60f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(60f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-50f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(50f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-40f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(40f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-30f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(30f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-20f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(20f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(-10f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(10f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.
				yield break;
			}
		}
	}

	}





	

