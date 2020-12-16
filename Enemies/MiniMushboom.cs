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
	public class MiniMushboom : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "mini mushboom";
		private static tk2dSpriteCollectionData MiniShroomCollection;


		public static void Init()
		{

			MiniMushboom.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			AIActor source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("Mini Mushboom", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = true;
				companion.aiActor.aiAnimator.HitReactChance = 0.05f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(1f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(1f, null, false);
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
				companion.aiActor.PreventBlackPhantom = true;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle",
					}
				};
				DirectionalAnimation anim = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
						{
						"dance",

						},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "dance",
						anim = anim
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
				bool flag3 = MiniShroomCollection == null;
				if (flag3)
				{
					MiniShroomCollection = SpriteBuilder.ConstructCollection(prefab, "Mini_Shroom_Collection");
					UnityEngine.Object.DontDestroyOnLoad(MiniShroomCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], MiniShroomCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, MiniShroomCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5

					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 3f;

				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				bs.AttackBehaviors.Add(new Boom());
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
				Game.Enemies.Add("kp:mini_mushboom", companion.aiActor);

			}
		}



		private static string[] spritePaths = new string[]
		{
			
			//idles
			"FrostAndGunfireItems/Resources/mini_mushboom/mini_mushboom_idle_001",
			"FrostAndGunfireItems/Resources/mini_mushboom/mini_mushboom_idle_002",
			"FrostAndGunfireItems/Resources/mini_mushboom/mini_mushboom_idle_003",
			"FrostAndGunfireItems/Resources/mini_mushboom/mini_mushboom_idle_004",
			"FrostAndGunfireItems/Resources/mini_mushboom/mini_mushboom_idle_005",
			"FrostAndGunfireItems/Resources/mini_mushboom/mini_mushboom_idle_006",
				};

		public class EnemyBehavior : BraveBehaviour
		{
			private void Start()
			{
				base.aiActor.knockbackDoer.SetImmobile(true, "laugh");
				base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					
					Exploder.DoDefaultExplosion(base.aiActor.sprite.WorldCenter, Vector2.zero);
				}; 
				base.aiActor.healthHaver.OnDeath += (obj) =>
				{
					Destroy(base.aiActor);
				};

			}
		}

		public class Boom : AttackBehaviorBase
		{

			private HeatIndicatorController m_playerRadialIndicator;
			bool HasExploded;
			//public string danceAnimation = "dance";
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
						this.isAttacking = true;

					}
					bool flag6 = this.attackTimer > 0f && flag3;
					if (flag6)
					{

						Attack();
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

			public void Attack()
			{
				int i = 0;
				this.m_playerRadialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), this.m_aiActor.sprite.WorldCenter.ToVector3ZisY(0f),
				Quaternion.identity, this.m_aiActor.transform)).GetComponent<HeatIndicatorController>();
				this.m_playerRadialIndicator.IsFire = false;
				m_playerRadialIndicator.CurrentRadius = 3f;
				float num = this.m_playerRadialIndicator.CurrentRadius;
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (i == 0 || i == 1)
				{
					if (playerController && Vector2.Distance(playerController.CenterPosition, this.m_aiActor.Position.XY()) < num && !HasExploded)
					{
						Exploder.DoDefaultExplosion(this.m_aiActor.sprite.WorldCenter, Vector2.zero);
						HasExploded = true;
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





			// Token: 0x0400006C RID: 108
			private bool isAttacking;

			// Token: 0x0400006D RID: 109
			private float attackCooldown = 0f;

			// Token: 0x0400006E RID: 110
			private float attackDuration = 100f;

			// Token: 0x0400006F RID: 111
			private float attackTimer;

			// Token: 0x04000070 RID: 112
			private float attackCooldownTimer;


		}



	}



}




	

