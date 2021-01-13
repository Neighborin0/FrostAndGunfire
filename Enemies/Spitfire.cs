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
	public class Spitfire : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "spitfire";
		private static tk2dSpriteCollectionData SpitfireCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			Spitfire.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("Spitfire", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
				companion.aiActor.knockbackDoer.weight = 100;
				companion.aiActor.MovementSpeed = 3f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.CollisionSetsPlayerOnFire = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(15f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.healthHaver.SetHealthMaximum(15f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.EffectResistances = new ActorEffectResistance[]{new ActorEffectResistance(){ resistAmount = 1, resistType = EffectResistanceType.Fire },};
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
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				DirectionalAnimation die = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "die",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "die",
						anim = die
					}
				};
				DirectionalAnimation attack = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "attack",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "attack",
						anim = attack
					}
				};
				DirectionalAnimation Itworked = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "tell",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "tell",
						anim = Itworked
					}
				};
				bool flag3 = SpitfireCollection == null;
				if (flag3)
				{
					SpitfireCollection = SpriteBuilder.ConstructCollection(prefab, "Spitfire_Collection");
					UnityEngine.Object.DontDestroyOnLoad(SpitfireCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], SpitfireCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SpitfireCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					6
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SpitfireCollection, new List<int>
					{

					7,
					8,
					9,
					10,
					11,
					12,			
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SpitfireCollection, new List<int>
					{
					13,
					14,
					15,
					16
					}, "tell", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SpitfireCollection, new List<int>
					{
					17,
					18,
					19,
					20,
					21,
					22,
					23
					}, "attack", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
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
					BulletScript = new CustomBulletScriptSelector(typeof(SpitScript)),
					LeadAmount = 0f,
					AttackCooldown = 4f,
					TellAnimation = "tell",
					FireAnimation = "attack",
					RequiresLineOfSight = true,
					Uninterruptible = true,			
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
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("6e972cd3b11e4b429b888b488e308551").behaviorSpeculator;
			    //Tools.DebugInformation(load);
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
			Game.Enemies.Add("kp:spitfire", companion.aiActor);


			}
		}



		private static string[] spritePaths = new string[]
		{
			
			//idles
			"FrostAndGunfireItems/Resources/spitfire/spitfire_idle_001",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_idle_002",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_idle_003",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_idle_004",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_idle_005",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_idle_006",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_idle_007",
			//die
			"FrostAndGunfireItems/Resources/spitfire/spitfire_die_001",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_die_002",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_die_003",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_die_004",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_die_005",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_die_006",
			//tell
			"FrostAndGunfireItems/Resources/spitfire/spitfire_tell_001",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_tell_002",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_tell_003",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_tell_004",
			//attack
			"FrostAndGunfireItems/Resources/spitfire/spitfire_attack_001",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_attack_002",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_attack_003",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_attack_004",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_attack_005",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_attack_006",
			"FrostAndGunfireItems/Resources/spitfire/spitfire_attack_007",
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
					DoFireGoop((base.aiActor.sprite.WorldCenter));
					AkSoundEngine.PostEvent("Play_VO_kali_death_01", base.aiActor.gameObject);
				};

			}

			private void DoFireGoop(Vector2 v)
			{
				AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
				GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
				DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
				goopManagerForGoopType.TimedAddGoopCircle(v, 2.3f, 0.35f, false);
				goopDef.damagesEnemies = false;
			}


		}

		public class SpitScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("d8a445ea4d944cc1b55a40f22821ae69").bulletBank.GetBullet("default"));
				}
				for (int i = 0; i < 9; i++)
				{
					
					base.Fire(new Direction(360 - (i * 36), DirectionType.Absolute, -1f), new Speed(4f, SpeedType.Absolute), new FireBullet());
					yield return this.Wait(6);
				}

				yield break;
			}
		}


		public class FireBullet : Bullet
		{
			public FireBullet() : base("default", false, false, false)
			{
			}
				protected override IEnumerator Top()
			   {
				AkSoundEngine.PostEvent("Play_BOSS_doormimic_flame_01", base.Projectile.gameObject);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(3f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(2f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(1f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 20);
				yield return this.Wait(360);
				this.Vanish(false);
				yield break;
			    }
		    }
		}
	}


	





	

