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
	public class Salamander : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "salamander";
		private static tk2dSpriteCollectionData SalamanderCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			Salamander.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				float AttackAnimationThingAMaWhatIts = 0.5f;
				prefab = EnemyBuilder.BuildPrefab("Salamander", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 800;
				companion.aiActor.MovementSpeed = 1f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(45f);
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
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[4],
					AnimNames = new string[]
					{
						"idle_back_right", //Good
						"idle_front_right", //Good
						"idle_front_left",//Good
						"idle_back_left",//Good
						

					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[4],
					AnimNames = new string[]
					{
						"run_back_right", //Good
						"run_front_right", //Good
						"run_front_left",//Good
						"run_back_left",//Good
						

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
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "tell",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.FourWay,
							Flipped = new DirectionalAnimation.FlipType[4],
							AnimNames = new string[]
							{
	                    "tell_back_right", //Good
						"tell_front_right", //Good
						"tell_front_left",//Good
						"tell_back_left",//Good
							}

						}
					}
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "attack",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.EightWayOrdinal,
							Flipped = new DirectionalAnimation.FlipType[8],
							AnimNames = new string[]
							{
						"attack_north",
					   "attack_north_east",
					    "attack_east",
					   "attack_south_east",
					   "attack_south",
						"attack_south_west",
					   "attack_west",
						"attack_north_west",

							}

						}
					}
				};
				bool flag3 = SalamanderCollection == null;
				if (flag3)
				{
					SalamanderCollection = SpriteBuilder.ConstructCollection(prefab, "Salamander_Collection");
					UnityEngine.Object.DontDestroyOnLoad(SalamanderCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], SalamanderCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{

					0,
					1,
					2,
					3


					}, "idle_front_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{

					4,
					5,
					6,
					7
				


					}, "idle_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{

				8,
				9,
				10,
				11




					}, "idle_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
				12,
				13,
				14,
				15

					}, "idle_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
				 16,
				 17,
				 18,
				 19,
				 20,
				 21
					}, "run_front_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
			22,
			23,
			24,
			25,
			26,
			27
					}, "run_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
			28,
			29,
			30,
			31,
			32,
			33
					}, "run_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		34,
		35,
		36,
		37,
		38,
		39
					}, "run_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		34,
		35,
		36,
		37,
		38,
		39
					}, "run_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
	    40
					}, "tell_front_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 3f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		41
					}, "tell_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 3f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		42
					}, "tell_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 3f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		43
					}, "tell_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 3f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		44
					}, "attack_east", tk2dSpriteAnimationClip.WrapMode.Once).fps = AttackAnimationThingAMaWhatIts;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		45
					}, "attack_north", tk2dSpriteAnimationClip.WrapMode.Once).fps = AttackAnimationThingAMaWhatIts;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		46
					}, "attack_north_east", tk2dSpriteAnimationClip.WrapMode.Once).fps = AttackAnimationThingAMaWhatIts;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		47
					}, "attack_north_west", tk2dSpriteAnimationClip.WrapMode.Once).fps = AttackAnimationThingAMaWhatIts;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		48
					}, "attack_south", tk2dSpriteAnimationClip.WrapMode.Once).fps = AttackAnimationThingAMaWhatIts;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		49
					}, "attack_south_east", tk2dSpriteAnimationClip.WrapMode.Once).fps = AttackAnimationThingAMaWhatIts;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		50
					}, "attack_south_west", tk2dSpriteAnimationClip.WrapMode.Once).fps = AttackAnimationThingAMaWhatIts;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		51
					}, "attack_west", tk2dSpriteAnimationClip.WrapMode.Once).fps = AttackAnimationThingAMaWhatIts;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		52,
		53,
		54,
		55,
		56,
		57,

					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SalamanderCollection, new List<int>
					{
		58,
		59,
		60,
		61,
		62,
		63

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7;



				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
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
					BulletScript = new CustomBulletScriptSelector(typeof(SalamanderScript)),
					LeadAmount = 0f,
					AttackCooldown = 5f,
					TellAnimation = "tell",
					FireAnimation = "attack",
					RequiresLineOfSight = true,
					StopDuring = ShootBehavior.StopType.Attack,
					Uninterruptible = true
				}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = true,
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
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("6e972cd3b11e4b429b888b488e308551").behaviorSpeculator;
			    //Tools.DebugInformation(load);
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
			Game.Enemies.Add("kp:salamander", companion.aiActor);


			}
		}



		private static string[] spritePaths = new string[]
		{
			
			//idles
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_front_left_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_front_left_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_front_left_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_front_left_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_back_left_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_back_left_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_back_left_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_back_left_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_front_right_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_front_right_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_front_right_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_front_right_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_back_right_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_back_right_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_back_right_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_idle_back_right_004",
			//
			//run
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_left_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_left_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_left_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_left_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_left_005",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_left_006",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_left_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_left_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_left_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_left_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_left_005",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_left_006",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_right_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_right_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_right_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_right_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_right_005",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_front_right_006",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_right_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_right_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_right_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_right_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_right_005",
			"FrostAndGunfireItems/Resources/salamander/salamander_run_back_right_006",
			//tell
			"FrostAndGunfireItems/Resources/salamander/salamander_tell_front_left_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_tell_back_left_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_tell_front_right_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_tell_back_right_001",
			//why are there so many animations whyyyyyyyyyyy
			"FrostAndGunfireItems/Resources/salamander/salamander_attack_east_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_attack_north_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_attack_north_east_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_attack_north_west_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_attack_south_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_attack_south_east_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_attack_south_west_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_attack_west_001",
			//aaaaaaaaaaaaaaaaaaaaaa
			"FrostAndGunfireItems/Resources/salamander/salamander_die_right_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_right_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_right_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_right_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_right_005",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_right_006",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_left_001",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_left_002",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_left_003",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_left_004",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_left_005",
			"FrostAndGunfireItems/Resources/salamander/salamander_die_left_006",


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
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_ENM_beholster_death_01", base.aiActor.gameObject);
				};

			}


		}

		public class SalamanderScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("465da2bb086a4a88a803f79fe3a27677").bulletBank.GetBullet("Breath"));
				}
				for (int i = 0; i < 60; i++)
				{
					this.Fire(new Direction(UnityEngine.Random.Range(-15f, 15f), DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), new SalamanderBullet());
					yield return this.Wait(3);
				}

				yield break;
			}
		}


		public class SalamanderBullet : Bullet
		{
			public SalamanderBullet() : base("Breath", false, false, false)
			{

			}
		}
	}
}

	





	

