using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	public class BabyGoodCannonKin : CompanionItem
	{
		public static GameObject prefab;
		public static readonly string guid = "baby cannon";
		private static tk2dSpriteCollectionData BabyCannonCollection;


		public static void Init()
		{
			string name = "Baby Good Cannon Kin";
			string resourcePath = "FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_left_001";
			GameObject gameObject = new GameObject();
			BabyGoodCannonKin item = gameObject.AddComponent<BabyGoodCannonKin>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Barrels of Love";
			string longDesc = "One of the spawn of the long thought dead Cannon Kin race, this little lad will point his barrel at anyone who may endanger you or himself.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.D;
			item.CompanionGuid = BabyGoodCannonKin.guid;
			item.SetupUnlockOnCustomFlag(CustomDungeonFlags.BOSS_RUSH_AND_WANDERER, true);
			BabyGoodCannonKin.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//AIActor source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = CompanionBuilder.BuildPrefab("Baby Cannon", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9));
				var companion = prefab.AddComponent<BabyCannonBehavior>();
				companion.aiActor.MovementSpeed = 8f;
				companion.aiActor.healthHaver.PreventAllDamage = true;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(18f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = false;
				companion.aiActor.CanTargetEnemies = true;
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
				bool flag3 = BabyCannonCollection == null;
				if (flag3)
				{
					BabyCannonCollection = SpriteBuilder.ConstructCollection(prefab, "BabyCannon_Collection");
					UnityEngine.Object.DontDestroyOnLoad(BabyCannonCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], BabyCannonCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BabyCannonCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						4,
						5
						
						
					}, "attack_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BabyCannonCollection, new List<int>
					{
						6,
						7,
						8,
						9,
						10,
						11

					}, "attack_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, BabyCannonCollection, new List<int>
					{
					12,
					13,
					14,
					15

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BabyCannonCollection, new List<int>
					{
						16,
						17,
						18,
						19
						

					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BabyCannonCollection, new List<int>
					{
						20,
						21,
						22,
						23,
						24,
						25


					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BabyCannonCollection, new List<int>
					{
						26,
						27,
						28,
						29,
						30,
						31
						


					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;

				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				bs.AttackBehaviors.Add(new CannonAttackBehavior());
				bs.MovementBehaviors.Add(new BabyGoodCannonKin.ApproachEnemiesBehavior());
				bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });
				
			}
		}



		private static string[] spritePaths = new string[]
		{
			
			//attacks
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_left_001",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_left_002",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_left_003",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_left_004",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_left_005",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_left_006",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_right_001",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_right_002",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_right_003",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_right_004",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_right_005",
			"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_attack_right_006",
			//idles
		"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_left_001",
		"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_left_002",
		"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_left_003",
		"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_left_004",
		"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_right_001",
		"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_right_002",
		"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_right_003",
		"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_idle_right_004",
						//run
             	"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_left_001",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_left_002",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_left_003",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_left_004",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_left_005",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_left_006",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_right_001",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_right_002",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_right_003",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_right_004",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_right_005",
				"FrostAndGunfireItems/Resources/baby_cannon/baby_cannon_run_right_006",

				};

		public class BabyCannonBehavior : CompanionController
		{
			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325
			private void Start()
			{

				base.spriteAnimator.Play("idle");
				this.Owner = this.m_owner;
			}

			// Token: 0x0400006A RID: 106
			public PlayerController Owner;
		}


		public class CannonAttackBehavior : AttackBehaviorBase
		{

			private PlayerController Owner;
			public override void Destroy()
			{
				base.Destroy();
			}

			// Token: 0x0600010A RID: 266 RVA: 0x0000A145 File Offset: 0x00008345
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<BabyGoodCannonKin.BabyCannonBehavior>().Owner;

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
						this.m_aiActor.MovementSpeed = 0f;
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
				yield return new WaitForSeconds(0.28f);
				bool flag = this.Owner == null;
				if (flag)
				{
					this.Owner = this.m_aiActor.GetComponent<BabyCannonBehavior>().Owner;
				}
				float num = -1f;

				List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag2 = activeEnemies == null | activeEnemies.Count <= 0;
				if (!flag2)
				{
					AIActor nearestEnemy = this.Owner.CurrentRoom.GetRandomActiveEnemy(false);
					bool flag3 = nearestEnemy && num < 10f;
					if (flag3)
					{
						bool flag4 = this.IsInRange(nearestEnemy);
						if (flag4)
						{
							bool flag5 = !nearestEnemy.IsHarmlessEnemy && nearestEnemy.IsNormalEnemy && !nearestEnemy.healthHaver.IsDead && nearestEnemy != this.m_aiActor;
							if (flag5)
							{
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
					}
				}
				this.m_aiActor.MovementSpeed = 8f;
			}
				
			

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

			// Token: 0x0600010F RID: 271 RVA: 0x0000A5FC File Offset: 0x000087FC
			public override float GetMaxRange()
			{
				return 40f;
			}

			// Token: 0x06000110 RID: 272 RVA: 0x0000A614 File Offset: 0x00008814
			public override float GetMinReadyRange()
			{
				return 40f;
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
						this.Owner = this.m_aiActor.GetComponent<BabyCannonBehavior>().Owner;
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



	

