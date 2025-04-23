using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	public class GnashLV3 : CompanionItem
	{
		public static GameObject prefab;
		public static readonly string guid = "gnash man";


		public static void Init()
		{
			string name = "Gnash LV3";
			string resourcePath = "FrostAndGunfireItems/Resources/balloon/balloon_sprite";
			GameObject gameObject = new GameObject();
			GnashLV3 item = gameObject.AddComponent<GnashLV3>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Fully Grown";
			string longDesc = "Give him meat, watch him grow.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.CompanionGuid = guid;
			BuildPrefab();
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnRoomClearEvent += this.RoomCleared;
		}


		private void RoomCleared(PlayerController player)
		{
			float value = UnityEngine.Random.value;
			if ((double)value < 0.05)
			{
				GenericLootTable singleItemRewardTable = GameManager.Instance.RewardManager.CurrentRewardData.SingleItemRewardTable;
				LootEngine.SpawnItem(singleItemRewardTable.SelectByWeight(false), player.CenterPosition, Vector2.up, 1f, true, false, false);
			}
		}

		public override DebrisObject Drop(PlayerController player)
		{

			player.OnRoomClearEvent -= this.RoomCleared;
			DebrisObject debrisObject = base.Drop(player);
			GnashLV3 component = debrisObject.GetComponent<GnashLV3>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;
			prefab = CompanionBuilder.BuildPrefab("Gnash LV3", guid, "FrostAndGunfireItems/Resources/squire/Idle/squire_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

				var companion = prefab.AddComponent<GnashLV3Behavior>();
				companion.aiActor.MovementSpeed = 10f;
				companion.aiActor.healthHaver.PreventAllDamage = true;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = false;
				companion.aiActor.CanTargetEnemies = true;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = false;
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
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
				companion.aiAnimator.specRigidbody.PixelColliders.Add(new PixelCollider
				{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.PlayerHitBox,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 0,
				ManualOffsetY = 0,
				ManualWidth = 10,
				ManualHeight = 26,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
				});
				prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/squire/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
				prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/squire/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				bs.AttackBehaviors.Add(new Punch());
				bs.OverrideBehaviors.Add(new EatMeatLV3());
				bs.MovementBehaviors.Add(new KillEm());
				bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });

			}
		}

		
		public class GnashLV3Behavior : CompanionController
		{
			private void Start()
			{
				Owner = this.m_owner;
				base.spriteAnimator.Play("idle");
			}

			public PlayerController Owner;
		}

		

		public class Punch : AttackBehaviorBase
		{
			public override BehaviorResult Update()
			{
				base.DecrementTimer(ref this.attackTimer, false);
				if (Owner == null)
				{
					if (this.m_aiActor && this.m_aiActor.CompanionOwner)
					{
						Owner = this.m_aiActor.CompanionOwner;
					}
					else
					{
						Owner = GameManager.Instance.BestActivePlayer;
					}
				}
				BehaviorResult result;
				if (this.m_aiActor && this.m_aiActor.OverrideTarget)
				{
					SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
					this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
					if (isInRange)
					{

						if (overrideTarget != null && this.attackTimer == 0)
						{
						overrideTarget.healthHaver.ApplyDamage(this.DamagePerTick, this.m_aiActor.specRigidbody.Velocity, "Companion Melee", CoreDamageTypes.Fire, DamageCategory.DamageOverTime, false, null, false);
						KnockbackDoer knockbackDoer = overrideTarget.knockbackDoer;
						knockbackDoer.ApplyKnockback(m_aiActor.sprite.WorldCenter * -1, 800f, true);
						this.attackTimer = this.TickDelay;
							result = BehaviorResult.SkipAllRemainingBehaviors;
						}
						else result = BehaviorResult.Continue;
					}
					else result = BehaviorResult.Continue;
				}
				else result = BehaviorResult.Continue;
				return result;
			}

			public override float GetMaxRange()
			{
				return 5f;
			}
			public override float GetMinReadyRange()
			{
				return 5f;
			}
			public override bool IsReady()
			{
				AIActor aiActor = this.m_aiActor;
				bool flag;
				if (aiActor == null) flag = true;
				else
				{
					SpeculativeRigidbody targetRigidbody = aiActor.TargetRigidbody;
					Vector2? vector = (targetRigidbody != null) ? new Vector2?(targetRigidbody.UnitCenter) : null;
					flag = (vector == null);
				}
				bool flag2 = flag;
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}
		private PlayerController Owner;
		public float DamagePerTick = 30;
		public float TickDelay = 8;
		public float DesiredDistance = 5;
		private float attackTimer;
		private bool isInRange;

	}
		public class KillEm : MovementBehaviorBase
		{
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
			}

			public override void Upkeep()
			{
				base.Upkeep();
				base.DecrementTimer(ref this.repathTimer, false);
			}

			public override BehaviorResult Update()
			{
				SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
				BehaviorResult result;
				if (this.repathTimer > 0f)
				{
					result = ((overrideTarget == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors);
				}
				else
				{
					if (overrideTarget == null)
					{
						this.PickNewTarget();
						result = BehaviorResult.Continue;
					}
					else
					{
						this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
						if (overrideTarget != null && !this.isInRange)
						{
							this.m_aiActor.PathfindToPosition(overrideTarget.UnitCenter, null, true, null, null, null, false);
							this.repathTimer = this.PathInterval;
							result = BehaviorResult.SkipRemainingClassBehaviors;
						}
						else
						{
							if (overrideTarget != null && this.repathTimer >= 0f)
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

			private void PickNewTarget()
			{

				if (m_aiActor != null)
				{
					if (Owner == null)
					{
						Owner = m_aiActor.GetComponent<GnashLV3Behavior>().Owner;
					}
					Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.roomEnemies);
					for (int i = 0; i < this.roomEnemies.Count; i++)
					{
						AIActor aiactor = this.roomEnemies[i];
						if (aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor || aiactor.EnemyGuid == "ba928393c8ed47819c2c5f593100a5bc")
						{

							this.roomEnemies.Remove(aiactor);

						}
					}
					if (this.roomEnemies.Count == 0)
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

			public float DesiredDistance = 5f;

			private float repathTimer;

			private List<AIActor> roomEnemies = new List<AIActor>();

			private bool isInRange;

			private PlayerController Owner;
		}
	public class EatMeatLV3 : OverrideBehaviorBase
	{
		public override void Start()
		{
			base.Start();
			this.m_cachedSpeed = this.m_aiActor.MovementSpeed;
			this.m_heartsMunched = 0;
		}

		private bool IsHeartInRoom()
		{

			PlayerController companionOwner = this.m_aiActor.CompanionOwner;
			if (!companionOwner || companionOwner.CurrentRoom == null)
			{
				return false;
			}
			List<HealthPickup> componentsAbsoluteInRoom = companionOwner.CurrentRoom.GetComponentsAbsoluteInRoom<HealthPickup>();
			HealthPickup closestToPosition = BraveUtility.GetClosestToPosition<HealthPickup>(componentsAbsoluteInRoom, companionOwner.CenterPosition, new HealthPickup[0]);
			if (closestToPosition != null)
			{
				this.m_targetHeart = closestToPosition;
				return true;
			}
			return false;
		}

		private void MunchHeart(PickupObject targetHeart)
		{
			UnityEngine.Object.Destroy(targetHeart.gameObject);
			this.m_heartsMunched++;
			m_aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Healing_Sparkles_001") as GameObject, Vector3.zero, true, false, false);
			//this.m_aiAnimator.PlayUntilFinished("munch", false, null, -1f, false);
			if (this.m_heartsMunched >= this.RequiredHearts)
			{
				GiveReward();
			}
		}

		private void GiveReward()
		{
			if (this.m_aiActor.CompanionOwner != null)
			{
				GenericLootTable singleItemRewardTable = GameManager.Instance.RewardManager.CurrentRewardData.SingleItemRewardTable;
				LootEngine.SpawnItem(singleItemRewardTable.SelectByWeight(false), m_aiActor.CompanionOwner.CenterPosition, Vector2.up, 1f, true, false, false);
				this.m_heartsMunched = 0;
			}
		}

		public override void Upkeep()
		{
			base.DecrementTimer(ref this.m_repathTimer, false);
			base.Upkeep();
		}
		public override BehaviorResult Update()
		{
			if (!this.m_targetHeart)
			{
				this.m_aiActor.MovementSpeed = this.m_cachedSpeed;
				this.m_targetHeart = null;
				if (this.m_repathTimer <= 0f)
				{
					this.m_repathTimer = 1f;
					if (this.IsHeartInRoom())
					{
						return BehaviorResult.SkipAllRemainingBehaviors;
					}
				}
				return base.Update();
			}
			this.m_aiActor.PathfindToPosition(this.m_targetHeart.sprite.WorldCenter, new Vector2?(this.m_targetHeart.sprite.WorldCenter), true, null, null, null, false);
			if (this.m_aiActor.Path != null && this.m_aiActor.Path.WillReachFinalGoal)
			{
				this.m_aiActor.MovementSpeed = 8f;
				if (Vector2.Distance(this.m_targetHeart.sprite.WorldCenter, this.m_aiActor.CenterPosition) < 1.25f)
				{
					this.MunchHeart(this.m_targetHeart);
					this.m_targetHeart = null;
				}
				return BehaviorResult.SkipAllRemainingBehaviors;
			}
			if (Vector2.Distance(this.m_targetHeart.sprite.WorldCenter, this.m_aiActor.CenterPosition) < 1.25f)
			{
				this.MunchHeart(this.m_targetHeart);
				this.m_targetHeart = null;
				return BehaviorResult.SkipAllRemainingBehaviors;
			}
			return base.Update();
		}

		public int RequiredHearts = 2;


		private int m_heartsMunched;

		private PickupObject m_targetHeart;

		private float m_repathTimer;

		private float m_cachedSpeed;
	}

}


