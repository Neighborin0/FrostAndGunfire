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

namespace FrostAndGunfireItems
{
	public class CannonKin : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "cannon";
		private static tk2dSpriteCollectionData CannonCollection;


		public static void Init()
		{

			CannonKin.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//AIActor source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("Cannon", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<CannonBehavior>();
				companion.aiActor.knockbackDoer.weight = 150;
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(18f);
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
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "attack",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
							Flipped = new DirectionalAnimation.FlipType[2],
							AnimNames = new string[]
							{

					   "attack_right",
						   "attack_left"

							}

						}
					}
				};
				bool flag3 = CannonCollection == null;
				if (flag3)
				{
					CannonCollection = SpriteBuilder.ConstructCollection(prefab, "Cannon_Collection");
					UnityEngine.Object.DontDestroyOnLoad(CannonCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], CannonCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CannonCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						4,
						5
						
						
					}, "attack_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CannonCollection, new List<int>
					{
						6,
						7,
						8,
						9,
						10,
						11

					}, "attack_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, CannonCollection, new List<int>
					{
					12,
					13,
					14,
					15

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CannonCollection, new List<int>
					{
						16,
						17,
						18,
						19
						

					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CannonCollection, new List<int>
					{
						20,
						21,
						22,
						23,
						24,
						25


					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CannonCollection, new List<int>
					{
						26,
						27,
						28,
						29,
						30,
						31
						


					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CannonCollection, new List<int>
					{
						32,
						33,
						34


					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CannonCollection, new List<int>
					{
						35,
						36,
						37


					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;

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
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = true,
					CustomRange = 30f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				bs.AttackBehaviors.Add(new CannonAttackBehavior());
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("kp:cannon_kin", companion.aiActor);

			}
		}



		private static string[] spritePaths = new string[]
		{
			
			//attacks
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_left_001",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_left_002",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_left_003",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_left_004",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_left_005",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_left_006",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_right_001",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_right_002",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_right_003",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_right_004",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_right_005",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_attack_left_006",
			//idles
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_idle_left_001",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_idle_left_002",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_idle_left_003",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_idle_left_004",
			"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_idle_right_001",
				"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_idle_right_002",
					"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_idle_right_003",
						"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_idle_right_004",
						//run
             "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_left_001",
			   "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_left_002",
				 "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_left_003",
				   "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_left_004",
					 "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_left_005",
					   "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_left_006",
						 "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_right_001",
						  "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_right_002",
						   "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_right_003",
							"FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_right_004",
							 "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_right_005",
							  "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_run_right_006",
							  //death
							   "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_die_right_001",
								 "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_die_right_002",
								   "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_die_right_003",
									 "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_die_left_001",
									  "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_die_left_002",
									   "FrostAndGunfireItems/Resources/cannon_kin/cannonkin_die_left_003",


				};

		public class CannonBehavior : BraveBehaviour
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
	

		public class CannonAttackBehavior : AttackBehaviorBase
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
				if (base.m_aiActor.CanTargetPlayers)
				{
					yield return new WaitForSeconds(0.28f);
					AkSoundEngine.PostEvent("Play_WPN_seriouscannon_shot_01", this.m_aiActor.gameObject);
					var cm = UnityEngine.Object.Instantiate<GameObject>((GameObject)BraveResources.Load("Global Prefabs/_ChallengeManager", ".prefab"));
					this.Rocket = (cm.GetComponent<ChallengeManager>().PossibleChallenges.Where(c => c.challenge is SkyRocketChallengeModifier).First().challenge as SkyRocketChallengeModifier).Rocket;
					UnityEngine.Object.Destroy(cm);
					RoomHandler absoluteRoom = this.m_aiActor.Position.GetAbsoluteRoom();
					PlayerController player = GameManager.Instance.PrimaryPlayer;
					SkyRocket component = SpawnManager.SpawnProjectile(this.Rocket, Vector3.zero, Quaternion.identity, true).GetComponent<SkyRocket>();
					component.Target = player.specRigidbody;
					component.AscentTime = 0.5f;
					component.DescentTime = 1f;
					component.Variance = 0f;
					//component.DownSprite = "cannonball_001";
					component.transform.position = base.m_aiActor.sprite.WorldCenter;
					tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
					component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
					this.m_spawnedRockets++;
				}
				else
				{
					AIActor nearestEnemy = m_aiActor.ParentRoom.GetRandomActiveEnemy(false);
					AkSoundEngine.PostEvent("Play_WPN_seriouscannon_shot_01", this.m_aiActor.gameObject);
					var cm = UnityEngine.Object.Instantiate<GameObject>((GameObject)BraveResources.Load("Global Prefabs/_ChallengeManager", ".prefab"));
					this.Rocket = (cm.GetComponent<ChallengeManager>().PossibleChallenges.Where(c => c.challenge is SkyRocketChallengeModifier).First().challenge as SkyRocketChallengeModifier).Rocket;
					UnityEngine.Object.Destroy(cm);
					SkyRocket component = SpawnManager.SpawnProjectile(this.Rocket, Vector3.zero, Quaternion.identity, true).GetComponent<SkyRocket>();
					component.Target = nearestEnemy.specRigidbody;
					component.AscentTime = 0.5f;
					component.DescentTime = 1f;
					component.Variance = 0f;
					//component.DownSprite = "cannonball_001";
					component.transform.position = base.m_aiActor.sprite.WorldCenter;
					tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
					component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
					this.m_spawnedRockets++;
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
			private float attackCooldown = 3f;

			// Token: 0x0400006E RID: 110
			private float attackDuration = 0.01f;

			// Token: 0x0400006F RID: 111
			private float attackTimer;

			// Token: 0x04000070 RID: 112
			private float attackCooldownTimer;

			// Token: 0x04000071 RID: 113

			public PlayerController o;
			private int m_spawnedRockets;

			public GameObject Rocket;
		}



	}
}



	

