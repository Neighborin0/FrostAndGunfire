using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dungeonator;
using GungeonAPI;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000008 RID: 8
	public class Frog : CompanionItem
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000037BC File Offset: 0x000019BC
		public static void Init()
		{
			string name = "hcbnhrdgcn nxgfgnxghn";
			string resourcePath = "FrostAndGunfireItems/Resources/capsule";
			GameObject gameObject = new GameObject();
			Frog creatureCapsule = gameObject.AddComponent<Frog>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Go!";
			string longDesc = "An ancient device to used capture creatures. Previously owned by the famed monster tamer, Brash Ketchup, this capsule contains some rare creatures that will happily fight for you.";
			ItemBuilder.SetupItem(creatureCapsule, shortDesc, longDesc, "kp");
			creatureCapsule.CompanionGuid = Frog.guid;
			Frog.BuildPrefab();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003874 File Offset: 0x00001A74
	

		// Token: 0x06000039 RID: 57 RVA: 0x000039CB File Offset: 0x00001BCB
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000039D8 File Offset: 0x00001BD8
		public static void BuildPrefab()
		{
			bool flag = Frog.FrogPrefab != null || CompanionBuilder.companionDictionary.ContainsKey(Frog.guid);
			bool flag2 = flag;
			if (!flag2)
			{
				Frog.FrogPrefab = CompanionBuilder.BuildPrefab("Frog", Frog.guid, Frog.spritePaths[0], new IntVector2(3, 2), new IntVector2(8, 9));
				Frog.FrogBehavior frogBehavior = Frog.FrogPrefab.AddComponent<Frog.FrogBehavior>();
				AIAnimator aiAnimator = frogBehavior.aiAnimator;
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
				bool flag3 = Frog.frogCollection == null;
				if (flag3)
				{
					Frog.frogCollection = SpriteBuilder.ConstructCollection(Frog.FrogPrefab, "Frog_Collection");
					UnityEngine.Object.DontDestroyOnLoad(Frog.frogCollection);
					for (int i = 0; i < Frog.spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(Frog.spritePaths[i], Frog.frogCollection);
					}
					SpriteBuilder.AddAnimation(frogBehavior.spriteAnimator, Frog.frogCollection, new List<int>
					{
						0,
						1,
						2,
						3
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(frogBehavior.spriteAnimator, Frog.frogCollection, new List<int>
					{
						4,
						5,
						6,
						7
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(frogBehavior.spriteAnimator, Frog.frogCollection, new List<int>
					{
						8,
						9,
						10,
						11,
						12,
						13
					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 10f;
					SpriteBuilder.AddAnimation(frogBehavior.spriteAnimator, Frog.frogCollection, new List<int>
					{
						14,
						15,
						16,
						17,
						18,
						19
					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 10f;
				}
				frogBehavior.aiActor.MovementSpeed = 7f;
				frogBehavior.specRigidbody.Reinitialize();
				frogBehavior.aiActor.CanTargetEnemies = true;
				BehaviorSpeculator behaviorSpeculator = frogBehavior.behaviorSpeculator;
				behaviorSpeculator.AttackBehaviors.Add(new Frog.FrogAttackBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new Frog.ApproachEnemiesBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
				{
					IdleAnimations = new string[]
					{
						"idle"
					}
				});
				UnityEngine.Object.DontDestroyOnLoad(Frog.FrogPrefab);
				FakePrefab.MarkAsFakePrefab(Frog.FrogPrefab);
				Frog.FrogPrefab.SetActive(false);
			}
		}

		// Token: 0x04000009 RID: 9
		public static GameObject FrogPrefab;

		// Token: 0x0400000A RID: 10
		public static readonly string guid = "frog";

		// Token: 0x0400000B RID: 11
		private static string[] spritePaths = new string[]
		{
			"FrostAndGunfireItems/Resources/frog/frog_idle_left_001",
			"FrostAndGunfireItems/Resources/frog/frog_idle_left_002",
			"FrostAndGunfireItems/Resources/frog/frog_idle_left_003",
			"FrostAndGunfireItems/Resources/frog/frog_idle_left_004",
			"FrostAndGunfireItems/Resources/frog/frog_idle_right_001",
			"FrostAndGunfireItems/Resources/frog/frog_idle_right_002",
			"FrostAndGunfireItems/Resources/frog/frog_idle_right_003",
			"FrostAndGunfireItems/Resources/frog/frog_idle_right_004",
			"FrostAndGunfireItems/Resources/frog/frog_run_left_001",
			"FrostAndGunfireItems/Resources/frog/frog_run_left_002",
			"FrostAndGunfireItems/Resources/frog/frog_run_left_003",
			"FrostAndGunfireItems/Resources/frog/frog_run_left_004",
			"FrostAndGunfireItems/Resources/frog/frog_run_left_005",
			"FrostAndGunfireItems/Resources/frog/frog_run_left_006",
			"FrostAndGunfireItems/Resources/frog/frog_run_right_001",
			"FrostAndGunfireItems/Resources/frog/frog_run_right_002",
			"FrostAndGunfireItems/Resources/frog/frog_run_right_003",
			"FrostAndGunfireItems/Resources/frog/frog_run_right_004",
			"FrostAndGunfireItems/Resources/frog/frog_run_right_005",
			"FrostAndGunfireItems/Resources/frog/frog_run_right_006"
		};

		// Token: 0x0400000C RID: 12
		private static tk2dSpriteCollectionData frogCollection;

		// Token: 0x0400000D RID: 13
	

		// Token: 0x0200002B RID: 43
		public class FrogBehavior : CompanionController
		{
			// Token: 0x060000F5 RID: 245 RVA: 0x0000988C File Offset: 0x00007A8C
			private void Start()
			{
				base.spriteAnimator.Play("idle");
				this.Owner = this.m_owner;
			}

			// Token: 0x0400005C RID: 92
			public PlayerController Owner;
		}

		// Token: 0x0200002C RID: 44
		public class FrogAttackBehavior : AttackBehaviorBase
		{
			// Token: 0x060000F7 RID: 247 RVA: 0x000098B5 File Offset: 0x00007AB5
			public override void Destroy()
			{
				base.Destroy();
			}

			// Token: 0x060000F8 RID: 248 RVA: 0x000098BF File Offset: 0x00007ABF
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<Frog.FrogBehavior>().Owner;
			}

			// Token: 0x060000F9 RID: 249 RVA: 0x000098E4 File Offset: 0x00007AE4
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

			// Token: 0x060000FA RID: 250 RVA: 0x000099F8 File Offset: 0x00007BF8
			private void StopAttacking()
			{
				this.isAttacking = false;
				this.attackTimer = 0f;
				this.attackCooldownTimer = this.attackCooldown;
			}

			// Token: 0x060000FB RID: 251 RVA: 0x00009A1C File Offset: 0x00007C1C
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
						bool flag4 = !aiactor2.IsMimicEnemy && aiactor2.healthHaver && !aiactor2.healthHaver.IsBoss && aiactor2.healthHaver.IsVulnerable;
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

			// Token: 0x060000FC RID: 252 RVA: 0x00009B3C File Offset: 0x00007D3C
			private void Attack()
			{
				bool flag = this.Owner == null;
				if (flag)
				{
					this.Owner = this.m_aiActor.GetComponent<Frog.FrogBehavior>().Owner;
				}
				float num = -1f;
				List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag2 = activeEnemies == null | activeEnemies.Count <= 0;
				if (!flag2)
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, this.m_aiActor.sprite.WorldCenter, out num, null);
					bool flag3 = nearestEnemy && num < 30f;
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
								this.m_aiActor.transform.position = unitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
								Projectile projectile = ((Gun)ETGMod.Databases.Items[599]).DefaultModule.projectiles[0];
								GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_aiActor.sprite.WorldTopCenter, Quaternion.Euler(0f, 0f, z), true);
								Projectile component = gameObject.GetComponent<Projectile>();
								bool flag6 = component != null;
								bool flag7 = flag6;
								if (flag7)
								{
									component.Owner = this.m_aiActor;
									component.Shooter = this.m_aiActor.specRigidbody;
									component.AppliesFire = false;
									component.baseData.damage = 2f;
									component.baseData.force = 1f;
									component.AdditionalScaleMultiplier *= 0.5f * (float)UnityEngine.Random.Range(1, 5);
									component.collidesWithPlayer = false;
								}
							}
						}
					}
				}
			}

			// Token: 0x060000FD RID: 253 RVA: 0x00009D48 File Offset: 0x00007F48
			public override float GetMaxRange()
			{
				return 10000f;
			}

			// Token: 0x060000FE RID: 254 RVA: 0x00009D60 File Offset: 0x00007F60
			public override float GetMinReadyRange()
			{
				return 100f;
			}

			// Token: 0x060000FF RID: 255 RVA: 0x00009D78 File Offset: 0x00007F78
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

			// Token: 0x06000100 RID: 256 RVA: 0x00009DF8 File Offset: 0x00007FF8
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

			// Token: 0x0400005D RID: 93
			private bool isAttacking;

			// Token: 0x0400005E RID: 94
			private float attackCooldown = 1f;

			// Token: 0x0400005F RID: 95
			private float attackDuration = 0.5f;

			// Token: 0x04000060 RID: 96
			private float attackTimer;

			// Token: 0x04000061 RID: 97
			private float attackCooldownTimer;

			// Token: 0x04000062 RID: 98
			private PlayerController Owner;

			// Token: 0x04000063 RID: 99
			private List<AIActor> roomEnemies = new List<AIActor>();
		}

		// Token: 0x0200002D RID: 45
		public class ApproachEnemiesBehavior : MovementBehaviorBase
		{
			// Token: 0x06000102 RID: 258 RVA: 0x00009E97 File Offset: 0x00008097
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
			}

			// Token: 0x06000103 RID: 259 RVA: 0x00009EA4 File Offset: 0x000080A4
			public override void Upkeep()
			{
				base.Upkeep();
				base.DecrementTimer(ref this.repathTimer, false);
			}

			// Token: 0x06000104 RID: 260 RVA: 0x00009EBC File Offset: 0x000080BC
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

			// Token: 0x06000105 RID: 261 RVA: 0x00009FD8 File Offset: 0x000081D8
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
						bool flag3 = aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor | aiactor.EnemyGuid == this.EnemiesToAvoid[i];
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

			// Token: 0x04000037 RID: 55
			public string[] EnemiesToAvoid = new string[]
			{
			 "ba928393c8ed47819c2c5f593100a5bc"

			};


			// Token: 0x04000064 RID: 100
			public float PathInterval = 0.25f;

			// Token: 0x04000065 RID: 101
			public float DesiredDistance = 5f;

			// Token: 0x04000066 RID: 102
			private float repathTimer;

			// Token: 0x04000067 RID: 103
			private List<AIActor> roomEnemies = new List<AIActor>();

			// Token: 0x04000068 RID: 104
			private bool isInRange;

			// Token: 0x04000069 RID: 105
			private PlayerController Owner;
		}
	}
}
