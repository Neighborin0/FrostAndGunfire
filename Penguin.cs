using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000D RID: 13
	public class Penguin : CompanionItem
	{
		//Add Snowballs and Sunglasses synergy
		public static void Init()
		{
			string name = "Penguin";
			string resourcePath = "FrostAndGunfireItems/Resources/crown";
			GameObject gameObject = new GameObject();
			Penguin penguin = gameObject.AddComponent<Penguin>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Wenk Wenk";
			string longDesc = "This strange crown has attracted a strange penguin towards you. He'll fight for you till the bitter end, but something seems off about him...";
			ItemBuilder.SetupItem(penguin, shortDesc, longDesc, "kp");
			penguin.quality = PickupObject.ItemQuality.C;
			penguin.CompanionGuid = Penguin.guid;
			penguin.Synergies = new CompanionTransformSynergy[0];
			Penguin.BuildPrefab();
			ItemBuilder.AddPassiveStatModifier(penguin, PlayerStats.StatType.Coolness, 1f, StatModifier.ModifyMethod.ADDITIVE);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000534D File Offset: 0x0000354D
		public override void Pickup(PlayerController player)
		{

			base.Pickup(player);
		}


	

		// Token: 0x06000061 RID: 97 RVA: 0x00005358 File Offset: 0x00003558
		public static void BuildPrefab()
		{
		
			bool flag = Penguin.PenguinPrefab != null || CompanionBuilder.companionDictionary.ContainsKey(Penguin.guid);
			bool flag2 = flag;
			if (!flag2)
			{
				Penguin.PenguinPrefab = CompanionBuilder.BuildPrefab("Penguin", Penguin.guid, Penguin.spritePaths[0], new IntVector2(3, 2), new IntVector2(8, 9));
				Penguin.PenguinBehavior penguinBehavior = Penguin.PenguinPrefab.AddComponent<Penguin.PenguinBehavior>();
				AIAnimator aiAnimator = penguinBehavior.aiAnimator;
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
				bool flag3 = Penguin.penguinCollection == null;
				if (flag3)
				{
					Penguin.penguinCollection = SpriteBuilder.ConstructCollection(Penguin.PenguinPrefab, "Penguin_Collection");
					UnityEngine.Object.DontDestroyOnLoad(Penguin.penguinCollection);
					for (int i = 0; i < Penguin.spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(Penguin.spritePaths[i], Penguin.penguinCollection);
					}
					SpriteBuilder.AddAnimation(penguinBehavior.spriteAnimator, Penguin.penguinCollection, new List<int>
					{
						0,
						1,
						2,
						3
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(penguinBehavior.spriteAnimator, Penguin.penguinCollection, new List<int>
					{
						4,
						5,
						6,
						7
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(penguinBehavior.spriteAnimator, Penguin.penguinCollection, new List<int>
					{
						8,
						9,
						10,
						11,
						12,
						13,
						14
					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 14f;
					SpriteBuilder.AddAnimation(penguinBehavior.spriteAnimator, Penguin.penguinCollection, new List<int>
					{
						15,
						16,
						17,
						18,
						19,
						20,
						21
					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 14f;
				}
				penguinBehavior.aiActor.MovementSpeed = 7f;
				penguinBehavior.specRigidbody.Reinitialize();
				penguinBehavior.specRigidbody.CollideWithTileMap = false;
				penguinBehavior.aiActor.CanTargetEnemies = true;
				BehaviorSpeculator behaviorSpeculator = penguinBehavior.behaviorSpeculator;
					behaviorSpeculator.AttackBehaviors.Add(new Penguin.PenguinAttackBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new Penguin.ApproachEnemiesBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
				{
					IdleAnimations = new string[]
					{
						"idle"
					}
				});
				UnityEngine.Object.DontDestroyOnLoad(Penguin.PenguinPrefab);
				FakePrefab.MarkAsFakePrefab(Penguin.PenguinPrefab);
				Penguin.PenguinPrefab.SetActive(false);
			}
		}

		// Token: 0x04000016 RID: 22
		public static GameObject PenguinPrefab;

		// Token: 0x04000017 RID: 23
		public static readonly string guid = "gunter";

		private List<CompanionController> companionsSpawned = new List<CompanionController>();


		// Token: 0x04000018 RID: 24
		private static string[] spritePaths = new string[]
		{
			"FrostAndGunfireItems/Resources/penguin/penguin_idle_left_001",
			"FrostAndGunfireItems/Resources/penguin/penguin_idle_left_002",
			"FrostAndGunfireItems/Resources/penguin/penguin_idle_left_003",
			"FrostAndGunfireItems/Resources/penguin/penguin_idle_left_004",
			"FrostAndGunfireItems/Resources/penguin/penguin_idle_right_001",
			"FrostAndGunfireItems/Resources/penguin/penguin_idle_right_002",
			"FrostAndGunfireItems/Resources/penguin/penguin_idle_right_003",
			"FrostAndGunfireItems/Resources/penguin/penguin_idle_right_004",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_left_001",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_left_002",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_left_003",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_left_004",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_left_005",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_left_006",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_left_007",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_right_001",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_right_002",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_right_003",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_right_004",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_right_005",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_right_006",
			"FrostAndGunfireItems/Resources/penguin/penguin_run_right_007"
		};

		// Token: 0x04000019 RID: 25
		private static tk2dSpriteCollectionData penguinCollection;

		// Token: 0x02000031 RID: 49
		public class PenguinBehavior : CompanionController
		{
			// Token: 0x06000119 RID: 281 RVA: 0x0000A9D9 File Offset: 0x00008BD9
			private void Start()
			{
				base.spriteAnimator.Play("idle");
				this.Owner = this.m_owner;
			}

			// Token: 0x04000079 RID: 121
			public PlayerController Owner;
		}

		// Token: 0x02000032 RID: 50
		public class PenguinAttackBehavior : AttackBehaviorBase
		{
		    

			public override void Destroy()
			{
				
				base.Destroy();
			}

			// Token: 0x0600011C RID: 284 RVA: 0x0000A9F9 File Offset: 0x00008BF9
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<Penguin.PenguinBehavior>().Owner;
			}

			// Token: 0x0600011D RID: 285 RVA: 0x0000AA1C File Offset: 0x00008C1C
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
						
						this.Attack();
						result = BehaviorResult.SkipAllRemainingBehaviors;
					}
					else
					{
						
						result = BehaviorResult.Continue;
					}
				}
				return result;
			}

			// Token: 0x0600011E RID: 286 RVA: 0x0000AB30 File Offset: 0x00008D30
			private void StopAttacking()
			{
				this.isAttacking = false;
				this.attackTimer = 0f;
				this.attackCooldownTimer = this.attackCooldown;
			}

			// Token: 0x0600011F RID: 287 RVA: 0x0000AB54 File Offset: 0x00008D54
			public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
			{
				AIActor aiactor = null;
				nearestDistance = float.MaxValue;
				bool flag = activeEnemies == null;
				bool flag2 = flag;
				bool flag3 = flag2;
				AIActor result;
				if (flag3)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor2 = activeEnemies[i];
						bool flag4 = aiactor2.healthHaver && aiactor2.healthHaver.IsVulnerable;
						bool flag5 = flag4;
						bool flag6 = flag5;
						if (flag6)
						{
							bool flag7 = !aiactor2.healthHaver.IsDead;
							bool flag8 = flag7;
							bool flag9 = flag8;
							if (flag9)
							{
								bool flag10 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
								bool flag11 = flag10;
								bool flag12 = flag11;
								if (flag12)
								{
									float num = Vector2.Distance(position, aiactor2.CenterPosition);
									bool flag13 = num < nearestDistance;
									bool flag14 = flag13;
									bool flag15 = flag14;
									if (flag15)
									{
										nearestDistance = num;
										aiactor = aiactor2;
									}
								}
							}
						}
					}
					result = aiactor;
				}
				return result;
			}

			// Token: 0x06000120 RID: 288 RVA: 0x0000AC74 File Offset: 0x00008E74
			private void Attack()
			{
				
				bool flag = this.Owner == null;
				if (flag)
				{
					this.Owner = this.m_aiActor.GetComponent<Penguin.PenguinBehavior>().Owner;
				}
				float num = -1f;

				List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag2 = activeEnemies == null | activeEnemies.Count <= 0;
				if (!flag2)
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, this.m_aiActor.sprite.WorldCenter, out num, null);
					bool flag3 = nearestEnemy && num < 10f;
					if (flag3)
					{
						bool flag4 = this.IsInRange(nearestEnemy);
						if (flag4)
						{
							bool flag5 = !nearestEnemy.IsHarmlessEnemy && nearestEnemy.IsNormalEnemy && !nearestEnemy.healthHaver.IsDead && nearestEnemy != this.m_aiActor;
							if (flag5)
							{
								Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
								Vector2 unitCenter2 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
								Projectile projectile = ((Gun)ETGMod.Databases.Items[402]).DefaultModule.projectiles[0];
								GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_aiActor.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z), true);
								Projectile component = gameObject.GetComponent<Projectile>();
								bool flag6 = component != null;
								bool flag7 = flag6;
								if (flag7)
								{
									
									component.Owner = Owner;
									component.Shooter = this.m_aiActor.specRigidbody;
								if (Owner.HasPickupID(402) || Owner.HasPickupID(636))
									{
										component.baseData.damage = 4f;
										component.AdditionalScaleMultiplier = 2f;
									}
							
								else
									{
										component.baseData.damage = 2.5f;
									}
									component.Owner = Owner;
									component.baseData.force = 1f;
									component.collidesWithPlayer = false;
								}
							}
						}
					}
				}
			}

			// Token: 0x06000121 RID: 289 RVA: 0x0000AE80 File Offset: 0x00009080
			public override float GetMaxRange()
			{
				return 5f;
			}

			// Token: 0x06000122 RID: 290 RVA: 0x0000AE98 File Offset: 0x00009098
			public override float GetMinReadyRange()
			{
				return 5f;
			}

			// Token: 0x06000123 RID: 291 RVA: 0x0000AEB0 File Offset: 0x000090B0
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

			// Token: 0x06000124 RID: 292 RVA: 0x0000AF30 File Offset: 0x00009130
			public bool IsInRange(AIActor enemy)
			{
				
				bool flag;
				if (enemy == null)
				{
					flag = true;
				}
				else
				{
					SpeculativeRigidbody specRigidbody = enemy.specRigidbody;
					Vector2? vector = (specRigidbody != null) ? new Vector2?(specRigidbody.UnitCenter) : null;
					flag = (vector == null);
				}
				bool flag2 = flag;
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, enemy.specRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			// Token: 0x0400007A RID: 122
			private bool isAttacking;

			// Token: 0x0400007B RID: 123
			private float attackCooldown = 0.9f;

			// Token: 0x0400007C RID: 124
			private float attackDuration = 0.01f;

			// Token: 0x0400007D RID: 125
			private float attackTimer;

			// Token: 0x0400007E RID: 126
			private float attackCooldownTimer;

			// Token: 0x0400007F RID: 127
			private PlayerController Owner;

			// Token: 0x04000080 RID: 128
			private List<AIActor> roomEnemies = new List<AIActor>();
		}

		
		public class ApproachEnemiesBehavior : MovementBehaviorBase
		{
			// Token: 0x06000126 RID: 294 RVA: 0x00009E97 File Offset: 0x00008097
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
			}

			// Token: 0x06000127 RID: 295 RVA: 0x0000AFCF File Offset: 0x000091CF
			public override void Upkeep()
			{
				base.Upkeep();
				base.DecrementTimer(ref this.repathTimer, false);
			}

			// Token: 0x06000128 RID: 296 RVA: 0x0000AFE8 File Offset: 0x000091E8
			public override BehaviorResult Update()
			{
				SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
				bool flag = this.repathTimer > 0f;
				BehaviorResult result;
				if (flag)
				{
					result = ((overrideTarget == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors);
				}
				else
				{
					bool flag2 = overrideTarget == null;
					if (flag2)
					{
						this.PickNewTarget();
						result = BehaviorResult.Continue;
					}
					else
					{
						this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
						bool flag3 = overrideTarget != null && !this.isInRange;
						if (flag3)
						{
							this.m_aiActor.PathfindToPosition(overrideTarget.UnitCenter, null, true, null, null, null, false);
							this.repathTimer = this.PathInterval;
							result = BehaviorResult.SkipRemainingClassBehaviors;
						}
						else
						{
							bool flag4 = overrideTarget != null && this.repathTimer >= 0f;
							if (flag4)
							{
								this.m_aiActor.ClearPath();
								this.repathTimer = -1f;
							}
							result = BehaviorResult.Continue;
						}
					}
				}
				return result;
			}

			// Token: 0x06000129 RID: 297 RVA: 0x0000B104 File Offset: 0x00009304
			private void PickNewTarget()
			{

				bool flag = this.m_aiActor == null;
				if (!flag)
				{
					bool flag2 = this.Owner == null;
					if (flag2)
					{
						this.Owner = this.m_aiActor.GetComponent<Penguin.PenguinBehavior>().Owner;
					}
					this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.roomEnemies);
					for (int i = 0; i < this.roomEnemies.Count; i++)
					{
						AIActor aiactor = this.roomEnemies[i];
						bool flag3 = aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor || aiactor.EnemyGuid == "ba928393c8ed47819c2c5f593100a5bc";
						if (flag3)
						{

								this.roomEnemies.Remove(aiactor);
							
						}
					}
					bool flag4 = this.roomEnemies.Count == 0;
					if (flag4)
					{
						this.m_aiActor.OverrideTarget = null;
					}
					else
					{
						AIActor aiActor = this.m_aiActor;
						AIActor aiactor2 = this.roomEnemies[UnityEngine.Random.Range(0, this.roomEnemies.Count)];
						aiActor.OverrideTarget = ((aiactor2 != null) ? aiactor2.specRigidbody : null);
					}
				}
			}

		
			public float PathInterval = 0.25f;

			// Token: 0x04000082 RID: 130
			public float DesiredDistance = 5f;

			// Token: 0x04000083 RID: 131
			private float repathTimer;

			// Token: 0x04000084 RID: 132
			private List<AIActor> roomEnemies = new List<AIActor>();

			// Token: 0x04000085 RID: 133
			private bool isInRange;

			// Token: 0x04000086 RID: 134
			private PlayerController Owner;
		}
	}
}
