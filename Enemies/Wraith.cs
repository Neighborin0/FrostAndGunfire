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
	public class ReLoad : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "reLoad";
		private static tk2dSpriteCollectionData SalamanderCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			ReLoad.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				float AttackAnimationThingAMaWhatIts = 0.5f;
				prefab = BossBuilder.BuildPrefab("ReLoad", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false, true);
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
	                  "attack_south_west",
					   "attack_north_east",
						"attack_east",
					   "attack_south_east",
					   "attack_north",
					   "attack_south",
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
				bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>
				{
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ReLoadConfuseAttack{
						attackCooldown = 7
						},
						NickName = "Test Attack1"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
					   BulletScript = new CustomBulletScriptSelector(typeof(SkullScript)),
					   LeadAmount = 0f,
					   AttackCooldown = 5f,
					   TellAnimation = "tell",
					    FireAnimation = "attack",
					    RequiresLineOfSight = true,
					    StopDuring = ShootBehavior.StopType.Attack,
					    Uninterruptible = true
						},
						NickName = "Test Attack2"
					}

				};
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
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").behaviorSpeculator;
				//Tools.DebugInformation(load);
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
			Game.Enemies.Add("kp:wraith", companion.aiActor);


			}
		}

		public class SkullScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				for (int i = 0; i < 3; i++)
				{
					this.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute));
					yield return this.Wait(3);
				}
				yield break;
			}

		}

		public class SkullBullet : Bullet
		{
			public SkullBullet() : base("skull", false, false, false)
			{

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
					AkSoundEngine.PostEvent("Play_VO_kali_death_01", base.aiActor.gameObject);
				};

			}


		}

		public class ReLoadConfuseAttack : AttackBehaviorBase
		{


			public override void Destroy()
			{
				base.Destroy();
			}

			// Token: 0x0600010A RID: 266 RVA: 0x0000A145 File Offset: 0x00008345
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);


			}

			// Token: 0x0600010B RID: 267 RVA: 0x0000A168 File Offset: 0x00008368
			public override BehaviorResult Update()
			{
				bool flag = this.attackTimer > 0f && this.isAttacking;
				if (flag)
				{
					base.DecrementTimer(ref this.attackTimer, false);
				}
				else
				{
					bool flag2 = this.attackCooldownTimer > 0f && !this.isAttacking;
					if (flag2)
					{
						base.DecrementTimer(ref this.attackCooldownTimer, false);
					}
				}
				bool flag3 = this.IsReady();
				bool flag4 = (!flag3 || this.attackCooldownTimer > 0f || this.attackTimer == 0f || this.m_aiActor.TargetRigidbody == null) && this.isAttacking;
				BehaviorResult result;
				if (flag4)
				{
					this.StopAttacking();
					result = BehaviorResult.Continue;
				}
				else
				{
					bool flag5 = flag3 && this.attackCooldownTimer == 0f && !this.isAttacking;
					if (flag5)
					{
						this.attackTimer = this.attackDuration;
						this.m_aiAnimator.PlayUntilFinished(this.attackAnimation, false, null, -1f, false);
						this.isAttacking = true;
					}
					bool flag6 = this.attackTimer > 0f && flag3;
					if (flag6)
					{
						GameManager.Instance.StartCoroutine(Attack());
						result = BehaviorResult.SkipAllRemainingBehaviors;
					}
					else
					{
						result = BehaviorResult.Continue;
					}
				}
				return result;
			}

			// Token: 0x0600010C RID: 268 RVA: 0x0000A2AF File Offset: 0x000084AF
			private void StopAttacking()
			{
				this.isAttacking = false;
				this.attackTimer = 0f;
				this.attackCooldownTimer = this.attackCooldown;
			}


			// Token: 0x0600010E RID: 270 RVA: 0x0000A3F0 File Offset: 0x000085F0

			public IEnumerator Attack()
			{
				yield return new WaitForSeconds(2f);
				AkSoundEngine.PostEvent("Play_WPN_seriouscannon_shot_01", this.m_aiActor.gameObject);
				Exploder.DoDistortionWave(this.m_aiActor.sprite.WorldCenter, 1f, 0.04f, 20f, 0.6f);
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					if (!playerController.healthHaver.IsDead)
					{
						if (!playerController.spriteAnimator.QueryInvulnerabilityFrame() && playerController.healthHaver.IsVulnerable)
						{
							Vector2 offset = new Vector2(0, .2f);
							float value = playerController.stats.GetBaseStatValue(PlayerStats.StatType.Accuracy) * -16f;
							playerController.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, value, playerController);
							var obj = GameObject.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Stun") as GameObject);
							var sprite = obj.GetComponent<tk2dSprite>();
							sprite.PlaceAtPositionByAnchor(playerController.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
							sprite.transform.SetParent(playerController.transform);
							yield return new WaitForSeconds(5f);
							GameObject.Destroy(sprite);
							float value1 = playerController.stats.GetBaseStatValue(PlayerStats.StatType.Accuracy) / -16f;
							playerController.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, value1, playerController);
						}
					}
				}
			}

			// Token: 0x0600010F RID: 271 RVA: 0x0000A5FC File Offset: 0x000087FC
			public override float GetMaxRange()
			{
				return 16f;
			}

			// Token: 0x06000110 RID: 272 RVA: 0x0000A614 File Offset: 0x00008814
			public override float GetMinReadyRange()
			{
				return 16f;
			}

			// Token: 0x06000111 RID: 273 RVA: 0x0000A62C File Offset: 0x0000882C
			public override bool IsReady()
			{
				AIActor aiActor = this.m_aiActor;
				bool flag;
				if (aiActor == null)
				{
					flag = true;
				}
				else
				{
					SpeculativeRigidbody targetRigidbody = aiActor.TargetRigidbody;
					Vector2? vector = (targetRigidbody != null) ? new Vector2?(targetRigidbody.UnitCenter) : null;
					flag = (vector == null);
				}
				bool flag2 = flag;
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			// Token: 0x06000112 RID: 274 RVA: 0x0000A6AC File Offset: 0x000088AC


			// Token: 0x0400006B RID: 107
			public string attackAnimation = "attack";

			// Token: 0x0400006C RID: 108
			private bool isAttacking;

			// Token: 0x0400006D RID: 109
			public float attackCooldown = 10f;

			// Token: 0x0400006E RID: 110
			public float attackDuration = 0.01f;

			// Token: 0x0400006F RID: 111
			private float attackTimer;

			// Token: 0x04000070 RID: 112
			private float attackCooldownTimer;

			// Token: 0x04000071 RID: 113

			public PlayerController o;
		
		}

	}
}

	





	

