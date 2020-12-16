using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000C RID: 12
	public class TableTechLife : CompanionItem
	{
		public Chest chest;
		//Add poof to spawn 
		//Remove sound on new level loaded
		public static void Init()
		{
			string name = "Table Tech Life";
			string resourcePath = "FrostAndGunfireItems/Resources/tabletech_life";
			GameObject gameObject = new GameObject();
			TableTechLife tableTechLife = gameObject.AddComponent<TableTechLife>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Living On The Flip Side";
			string longDesc = "This ancient technique allows the user to create life from tables.\n\n Chapter ??? of the Table Sutra. A man who puts his heart and soul into every flip will be able to overcome any challenge.";
			ItemBuilder.SetupItem(tableTechLife, shortDesc, longDesc, "kp");
			tableTechLife.quality = PickupObject.ItemQuality.B;
			tableTechLife.CompanionGuid = TableTechLife.guid;
			TableTechLife.BuildPrefab();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000047B0 File Offset: 0x000029B0
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnTableFlipCompleted = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipCompleted, new Action<FlippableCover>(this.HandleFlip));
			player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		}

		
		// Token: 0x06000055 RID: 85 RVA: 0x0000480A File Offset: 0x00002A0A
		private void HandleFlip(FlippableCover table)
		{
			if (Owner.HasPickupID(293) || Owner.HasPickupID(484) || Owner.HasPickupID(638))
			{
				CreateMimic(base.Owner);
			}
			else
			{
				this.CreateNewCompanion(base.Owner);
			}
			
			AkSoundEngine.PostEvent("Play_OBJ_smallchest_spawn_01", base.gameObject);
		}

	

		
		// Token: 0x06000056 RID: 86 RVA: 0x0000481C File Offset: 0x00002A1C
		private void CreateNewCompanion(PlayerController player)
		{
			bool flag = this.companionsSpawned.Count + 1 == this.MaxNumberOfCompanions;
			bool flag2 = flag;
			if (!flag2)
			{
				bool flag3 = this.companionsSpawned.Count >= this.MaxNumberOfCompanions;
				bool flag4 = !flag3;
				if (flag4)
				{
						AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
						Vector3 vector = player.transform.position;
						bool flag5 = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER;
						bool flag6 = flag5;
						if (flag6)
						{
							vector += new Vector3(1.125f, -0.3125f, 0f);
						}
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
					CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
					this.companionsSpawned.Add(orAddComponent);
					orAddComponent.Initialize(player);
					bool flag7 = orAddComponent.specRigidbody;
					bool flag8 = flag7;
					if (flag8)
					{
						PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
					}
					orAddComponent.healthHaver.OnDeath += this.OnDeath;
					//orAddComponent.aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_ChestKnock_001") as GameObject, Vector3.zero, false, false, false);
					LootEngine.DoDefaultItemPoof(orAddComponent.aiActor.sprite.WorldCenter, false, true);
					orAddComponent.aiAnimator.PlayUntilFinished("spawn", false, null, -1f, false);
				
				}
			}
		}

		private void CreateMimic(PlayerController player)
		{
			bool flag = this.companionsSpawned.Count + 1 == this.MaxNumberOfCompanions;
			bool flag2 = flag;
			if (!flag2)
			{
				bool flag3 = this.companionsSpawned.Count >= this.MaxNumberOfCompanions;
				bool flag4 = !flag3;
				if (flag4)
				{
						AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(TableTechLife2.guid);
						Vector3 vector = player.transform.position;
						bool flag5 = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER;
						bool flag6 = flag5;
						if (flag6)
						{
							vector += new Vector3(1.125f, -0.3125f, 0f);
						}
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
					CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
					this.companionsSpawned.Add(orAddComponent);
					orAddComponent.Initialize(player);
					bool flag7 = orAddComponent.specRigidbody;
					bool flag8 = flag7;
					if (flag8)
					{
						PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
					}
					orAddComponent.healthHaver.OnDeath += this.OnDeath;
				
				}
			}
		}
		// Token: 0x06000057 RID: 87 RVA: 0x00004970 File Offset: 0x00002B70
		private void OnDeath(Vector2 v)
		{
			this.removeDeadCompanions();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000497C File Offset: 0x00002B7C
		private void removeDeadCompanions()
		{
			for (int i = this.companionsSpawned.Count - 1; i >= 0; i--)
			{
				bool flag = !this.companionsSpawned[i];
				bool flag2 = flag;
				if (flag2)
				{
					this.companionsSpawned.RemoveAt(i);
				}
				else
				{
					bool flag3 = this.companionsSpawned[i].healthHaver && this.companionsSpawned[i].healthHaver.IsDead;
					bool flag4 = flag3;
					if (flag4)
					{
						UnityEngine.Object.Destroy(this.companionsSpawned[i].gameObject);
						this.companionsSpawned.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004A40 File Offset: 0x00002C40
		private void DestroyCompanions()
		{
			for (int i = this.companionsSpawned.Count - 1; i >= 0; i--)
			{
				bool flag = this.companionsSpawned[i];
				bool flag2 = flag;
				if (flag2)
				{
					UnityEngine.Object.Destroy(this.companionsSpawned[i].gameObject);
				}
				this.companionsSpawned.RemoveAt(i);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004AAC File Offset: 0x00002CAC
		private void HandleNewFloor(PlayerController player)
		{
			int count = this.companionsSpawned.Count;
			this.DestroyCompanions();
			for (int i = 0; i < count; i++)
			{
				this.CreateNewCompanion(player);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004AE8 File Offset: 0x00002CE8
		public override DebrisObject Drop(PlayerController player)
		{
			player.OnTableFlipCompleted = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipCompleted, new Action<FlippableCover>(this.HandleFlip));
			player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
			this.DestroyCompanions();
			return base.Drop(player);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004B4C File Offset: 0x00002D4C
		public static void BuildPrefab()
		{
			bool flag = TableTechLife.TablePrefab != null || CompanionBuilder.companionDictionary.ContainsKey(TableTechLife.guid);
			bool flag2 = flag;
			if (!flag2)
			{
				TableTechLife.TablePrefab = CompanionBuilder.BuildPrefab("Table Boi", TableTechLife.guid, TableTechLife.spritePaths[0], new IntVector2(3, 2), new IntVector2(8, 9));
				TableTechLife.TableBehavior tableBehavior = TableTechLife.TablePrefab.AddComponent<TableTechLife.TableBehavior>();
				tableBehavior.aiAnimator.facingType = AIAnimator.FacingType.Movement;
				tableBehavior.aiAnimator.faceSouthWhenStopped = false;
				tableBehavior.aiAnimator.faceTargetWhenStopped = false;
				AIAnimator aiAnimator = tableBehavior.aiAnimator;
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "run",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "die",
						anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.Single,
							Prefix = "die",
							AnimNames = new string[1],
							Flipped = new DirectionalAnimation.FlipType[1]
						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
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
				
				bool flag3 = TableTechLife.TableBoiCollection == null;
				if (flag3)
				{
					TableTechLife.TableBoiCollection = SpriteBuilder.ConstructCollection(TableTechLife.TablePrefab, "Table_Boi_Collection");
					UnityEngine.Object.DontDestroyOnLoad(TableTechLife.TableBoiCollection);
					for (int i = 0; i < TableTechLife.spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(TableTechLife.spritePaths[i], TableTechLife.TableBoiCollection);
					}
					SpriteBuilder.AddAnimation(tableBehavior.spriteAnimator, TableTechLife.TableBoiCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						4
					}, "attack", tk2dSpriteAnimationClip.WrapMode.Once).fps = 20f;
					SpriteBuilder.AddAnimation(tableBehavior.spriteAnimator, TableTechLife.TableBoiCollection, new List<int>
					{
						5,
						6,
						7,
						8
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
					SpriteBuilder.AddAnimation(tableBehavior.spriteAnimator, TableTechLife.TableBoiCollection, new List<int>
					{
						9,
						10,
						11
					}, "run", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 12f;
					SpriteBuilder.AddAnimation(tableBehavior.spriteAnimator, TableTechLife.TableBoiCollection, new List<int>
					{
						12,
						13,
						14,
						15,
						16,
						17,
						18,
						19
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 24f;
					
				}
				tableBehavior.aiActor.MovementSpeed = 10f;
				tableBehavior.aiActor.HasShadow = false;
				tableBehavior.aiActor.CanTargetPlayers = false;
				BehaviorSpeculator behaviorSpeculator = tableBehavior.behaviorSpeculator;
				behaviorSpeculator.AttackBehaviors.Add(new TableTechLife.TableAttackBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new TableTechLife.ApproachEnemiesBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
				{
					IdleAnimations = new string[]
						{
						"idle"
						}
				});
				UnityEngine.Object.DontDestroyOnLoad(TableTechLife.TablePrefab);
				FakePrefab.MarkAsFakePrefab(TableTechLife.TablePrefab);
				TableTechLife.TablePrefab.SetActive(false);
				tableBehavior.CanInterceptBullets = true;
				tableBehavior.aiActor.healthHaver.PreventAllDamage = false;
				tableBehavior.aiActor.CollisionDamage = 0f;
				tableBehavior.aiActor.specRigidbody.CollideWithOthers = true;
				tableBehavior.aiActor.specRigidbody.CollideWithTileMap = false;
				tableBehavior.aiActor.healthHaver.ForceSetCurrentHealth(15f);
				tableBehavior.aiActor.healthHaver.SetHealthMaximum(11f, null, false);
				tableBehavior.aiActor.specRigidbody.PixelColliders.Clear();
				tableBehavior.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 20,
					ManualOffsetY = 6,
					ManualWidth = 1,
					ManualHeight = 1,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				tableBehavior.aiAnimator.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.PlayerHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 20,
					ManualOffsetY = 6,
					ManualWidth = 37,
					ManualHeight = 24,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				
			}
		}

		// Token: 0x04000010 RID: 16
		public static GameObject TablePrefab;

		// Token: 0x04000011 RID: 17
		public static readonly string guid = "tableboi";

		// Token: 0x04000012 RID: 18
		public int MaxNumberOfCompanions = 100;

		// Token: 0x04000013 RID: 19
		private List<CompanionController> companionsSpawned = new List<CompanionController>();

		// Token: 0x04000014 RID: 20
		private static string[] spritePaths = new string[]
		{
			"FrostAndGunfireItems/Resources/table_bois/tableboi_attack_001",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_attack_002",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_attack_003",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_attack_004",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_attack_005",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_idle_001",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_idle_002",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_idle_003",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_idle_004",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_run_001",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_run_002",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_run_003",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_die_001",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_die_002",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_die_003",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_die_004",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_die_005",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_die_006",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_die_007",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_die_008",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_spawn_001",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_spawn_002",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_spawn_003",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_spawn_004",
			"FrostAndGunfireItems/Resources/table_bois/tableboi_spawn_005"
		};

		// Token: 0x04000015 RID: 21
		private static tk2dSpriteCollectionData TableBoiCollection;

		// Token: 0x0200002E RID: 46
		public class TableBehavior : CompanionController
		{
			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325
			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325die
			private void Start()
			{

				base.spriteAnimator.Play("idle");
				this.Owner = this.m_owner;
			}

			// Token: 0x0400006A RID: 106
			public PlayerController Owner;
		}

		// Token: 0x0200002F RID: 47
		public class TableAttackBehavior : AttackBehaviorBase
		{ 
			
			
			public override void Destroy()
			{
				base.Destroy();
			}

			// Token: 0x0600010A RID: 266 RVA: 0x0000A145 File Offset: 0x00008345
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<TableTechLife.TableBehavior>().Owner;
			
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
						AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", this.m_aiActor.gameObject);
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

			// Token: 0x0600010E RID: 270 RVA: 0x0000A3F0 File Offset: 0x000085F0
			private void Attack()
			{
				bool flag = this.Owner == null;
				if (flag)
				{
					this.Owner = this.m_aiActor.GetComponent<TableTechLife.TableBehavior>().Owner;
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
								Vector2 unitCenter = this.m_aiActor.specRigidbody.sprite.WorldBottomLeft;
								Vector2 unitCenter2 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
								Projectile projectile = ((Gun)ETGMod.Databases.Items["ak-47"]).DefaultModule.projectiles[0];
								GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_aiActor.sprite.WorldBottomLeft, Quaternion.Euler(0.5f, 0f, z), true);
								Projectile component = gameObject.GetComponent<Projectile>();
								bool flag6 = component != null;
								bool flag7 = flag6;
								if (flag7)
								{
								
									if (Owner.HasPickupID(529))
									{
										component.baseData.damage = 7f;
										component.AdditionalScaleMultiplier = 1.4f;
									}
									else
									{
										component.baseData.damage = 5f;
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
			private float attackCooldown = 2.5f;

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

		// Token: 0x02000030 RID: 48
		public class ApproachEnemiesBehavior : MovementBehaviorBase
		{
			// Token: 0x06000114 RID: 276 RVA: 0x00009E97 File Offset: 0x00008097
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
			}

			// Token: 0x06000115 RID: 277 RVA: 0x0000A756 File Offset: 0x00008956
			public override void Upkeep()
			{
				base.Upkeep();
				base.DecrementTimer(ref this.repathTimer, false);
			}

			// Token: 0x06000116 RID: 278 RVA: 0x0000A770 File Offset: 0x00008970
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

			// Token: 0x06000117 RID: 279 RVA: 0x0000A88C File Offset: 0x00008A8C
			private void PickNewTarget()
			{
				bool flag = this.m_aiActor == null;
				if (!flag)
				{
					bool flag2 = this.Owner == null;
					if (flag2)
					{
						this.Owner = this.m_aiActor.GetComponent<TableTechLife.TableBehavior>().Owner;
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

		

			// Token: 0x04000073 RID: 115
			public float PathInterval = 0.25f;

			// Token: 0x04000074 RID: 116
			public float DesiredDistance = 3f;

			// Token: 0x04000075 RID: 117
			private float repathTimer;

			// Token: 0x04000076 RID: 118
			private List<AIActor> roomEnemies = new List<AIActor>();

			// Token: 0x04000077 RID: 119
			private bool isInRange;

			// Token: 0x04000078 RID: 120
			private PlayerController Owner;
		}
	}
}
