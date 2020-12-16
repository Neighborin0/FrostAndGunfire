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
	public class Mushroom : CompanionItem
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000037BC File Offset: 0x000019BC
		public static void Init()
		{
			string name = "fhhvvbbnj";
			string resourcePath = "FrostAndGunfireItems/Resources/capsule";
			GameObject gameObject = new GameObject();
			Mushroom mushroom = gameObject.AddComponent<Mushroom>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Go!";
			string longDesc = "An ancient device to used capture creatures. Previously owned by the famed monster tamer, Brash Ketchup, this capsule contains some rare creatures that will happily fight for you.";
			ItemBuilder.SetupItem(mushroom, shortDesc, longDesc, "kp");
			mushroom.CompanionGuid = Mushroom.guid;
			Mushroom.BuildPrefab();
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
			bool flag = Mushroom.MushroomPrefab != null || CompanionBuilder.companionDictionary.ContainsKey(Mushroom.guid);
			bool flag2 = flag;
			if (!flag2)
			{
				Mushroom.MushroomPrefab = CompanionBuilder.BuildPrefab("Mushroom", Mushroom.guid, Mushroom.spritePaths[0], new IntVector2(3, 2), new IntVector2(8, 9));
				Mushroom.MushroomBehavior mushroomBehavior = Mushroom.MushroomPrefab.AddComponent<Mushroom.MushroomBehavior>();
				AIAnimator aiAnimator = mushroomBehavior.aiAnimator;
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
				DirectionalAnimation anim = new DirectionalAnimation
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
						anim = anim
					}
				};


				bool flag3 = Mushroom.mushroomcollection == null;
				if (flag3)
				{
					Mushroom.mushroomcollection = SpriteBuilder.ConstructCollection(Mushroom.MushroomPrefab, "Mushroom_Collection");
					UnityEngine.Object.DontDestroyOnLoad(Mushroom.mushroomcollection);
					for (int i = 0; i < Mushroom.spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(Mushroom.spritePaths[i], Mushroom.mushroomcollection);
					}
					SpriteBuilder.AddAnimation(mushroomBehavior.spriteAnimator, Mushroom.mushroomcollection, new List<int>
					{
						0,
						1,
						2,
						3
						
						
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 15f;
					SpriteBuilder.AddAnimation(mushroomBehavior.spriteAnimator, Mushroom.mushroomcollection, new List<int>
					{
						4,
						5,
						6,
						7
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 15f;
					SpriteBuilder.AddAnimation(mushroomBehavior.spriteAnimator, Mushroom.mushroomcollection, new List<int>
					{
						8,
						9,
						10,
						11,
						12,
						13
					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 10f;
					SpriteBuilder.AddAnimation(mushroomBehavior.spriteAnimator, Mushroom.mushroomcollection, new List<int>
					{

					14,
					15,
					16,
					17,
					18,
					19


					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 10f;
					SpriteBuilder.AddAnimation(mushroomBehavior.spriteAnimator, Mushroom.mushroomcollection, new List<int>
					{
						20,
						21,
						22,
						23,
						24,
						25,
						26,
						27,
						28,
						29,
						30,
						31,
						32,
						33
					}, "attack", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 15f;
				}
				mushroomBehavior.aiActor.MovementSpeed = 7f;
				mushroomBehavior.specRigidbody.Reinitialize();
				mushroomBehavior.aiActor.CanTargetEnemies = true;
				BehaviorSpeculator behaviorSpeculator = mushroomBehavior.behaviorSpeculator;
				behaviorSpeculator.AttackBehaviors.Add(new Mushroom.MushroomAttackBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new Mushroom.ApproachEnemiesBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
				{
					IdleAnimations = new string[]
					{
						"idle"
					}
				});
				UnityEngine.Object.DontDestroyOnLoad(Mushroom.MushroomPrefab);
				FakePrefab.MarkAsFakePrefab(Mushroom.MushroomPrefab);
				Mushroom.MushroomPrefab.SetActive(false);
			}
		}

		// Token: 0x04000009 RID: 9
		public static GameObject MushroomPrefab;

		// Token: 0x0400000A RID: 10
		public static readonly string guid = "shrooom";

		// Token: 0x0400000B RID: 11
		private static string[] spritePaths = new string[]
		{
			"FrostAndGunfireItems/Resources/mushroom/mushroom_idle_left_001",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_idle_left_002",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_idle_left_003",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_idle_left_004",

			"FrostAndGunfireItems/Resources/mushroom/mushroom_idle_right_001",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_idle_right_002",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_idle_right_003",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_idle_right_004",


			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_right_001",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_right_002",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_right_003",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_right_004",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_right_005",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_right_006",

			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_left_001",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_left_002",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_left_003",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_run_left_004",
	"FrostAndGunfireItems/Resources/mushroom/mushroom_run_left_005",
	"FrostAndGunfireItems/Resources/mushroom/mushroom_run_left_006",


			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_001",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_002",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_003",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_004",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_005",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_006",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_007",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_008",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_009",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_010",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_011",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_012",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_013",
			"FrostAndGunfireItems/Resources/mushroom/mushroom_attack_014"



		};

		// Token: 0x0400000C RID: 12
		private static tk2dSpriteCollectionData mushroomcollection;

		public class MushroomBehavior : CompanionController
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
		public class MushroomAttackBehavior : AttackBehaviorBase
		{
			// Token: 0x06000109 RID: 265 RVA: 0x000098B5 File Offset: 0x00007AB5
			public override void Destroy()
			{
				base.Destroy();
			}

			// Token: 0x0600010A RID: 266 RVA: 0x0000A145 File Offset: 0x00008345
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<Mushroom.MushroomBehavior>().Owner;
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
						this.m_aiAnimator.PlayUntilFinished(this.attackAnimation);
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

			// Token: 0x0600010C RID: 268 RVA: 0x0000A2AF File Offset: 0x000084AF
			private void StopAttacking()
			{
				this.isAttacking = false;
				this.attackTimer = 0f;
				this.attackCooldownTimer = this.attackCooldown;
			}

			// Token: 0x0600010D RID: 269 RVA: 0x0000A2D0 File Offset: 0x000084D0
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

			// Token: 0x0600010E RID: 270 RVA: 0x0000A3F0 File Offset: 0x000085F0
			private void Attack()
			{
				bool flag = this.Owner == null;
				if (flag)
				{
					this.Owner = this.m_aiActor.GetComponent<Mushroom.MushroomBehavior>().Owner;
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
								this.m_aiActor.transform.position = unitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
								Projectile projectile = ((Gun)ETGMod.Databases.Items[207]).DefaultModule.projectiles[0];
								GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_aiActor.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z), true);
								Projectile component = gameObject.GetComponent<Projectile>();
								bool flag6 = component != null;
								bool flag7 = flag6;
								if (flag7)
								{
									component.Owner = this.m_aiActor;
									component.Shooter = this.m_aiActor.specRigidbody;
									component.baseData.damage = 3f;
									component.baseData.force = 1f;
									component.collidesWithPlayer = false;
								}
							}
						}
					}
				}
			}

			// Token: 0x0600010F RID: 271 RVA: 0x0000A5FC File Offset: 0x000087FC
			public override float GetMaxRange()
			{
				return 7f;
			}

			// Token: 0x06000110 RID: 272 RVA: 0x0000A614 File Offset: 0x00008814
			public override float GetMinReadyRange()
			{
				return 7f;
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

			// Token: 0x0400006B RID: 107
			public string attackAnimation = "attack";

			// Token: 0x0400006C RID: 108
			private bool isAttacking;

			// Token: 0x0400006D RID: 109
			private float attackCooldown = 1.2f;

			// Token: 0x0400006E RID: 110
			private float attackDuration = 0.01f;

			// Token: 0x0400006F RID: 111
			private float attackTimer;

			// Token: 0x04000070 RID: 112
			private float attackCooldownTimer;

			// Token: 0x04000071 RID: 113
			private PlayerController Owner;

			// Token: 0x04000072 RID: 114
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
						this.Owner = this.m_aiActor.GetComponent<Mushroom.MushroomBehavior>().Owner;
					}
					this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.roomEnemies);
					for (int i = 0; i < this.roomEnemies.Count; i++)
					{
						AIActor aiactor = this.roomEnemies[i];
						bool flag3 = aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor;
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
