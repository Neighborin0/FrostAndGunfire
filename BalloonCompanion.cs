using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
namespace FrostAndGunfireItems
{
    public class BalloonCompanion : CompanionItem
	{
        public static GameObject prefab;
        public static readonly string guid = "balloon";
		public int MaxNumberOfCompanions = 100;
		private List<CompanionController> companionsSpawned = new List<CompanionController>();


		public static void Init()
        {
			BuildPrefab();
        }

	
		public static void BuildPrefab()
        {
            if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
                return;

            //Create the prefab with a starting sprite and hitbox offset/size
            prefab = CompanionBuilder.BuildPrefab("Balloony", guid, "FrostAndGunfireItems/Resources/balloon/Idle/balloon_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

            //Add a companion component to the prefab (could be a custom class)
            var companion = prefab.AddComponent<BalloonBehavior>();
			companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
			companion.aiActor.MovementSpeed = 9f;
			companion.CanInterceptBullets = true;
			companion.aiActor.healthHaver.PreventAllDamage = false;
			companion.aiActor.CollisionDamage = 0f;
			companion.aiActor.HasShadow = false;
			companion.aiActor.specRigidbody.CollideWithOthers = true;
			companion.aiActor.specRigidbody.CollideWithTileMap = false;
			companion.aiActor.healthHaver.ForceSetCurrentHealth(0.1f);
			companion.aiActor.healthHaver.SetHealthMaximum(0.1f, null, false);
			companion.aiActor.specRigidbody.PixelColliders.Clear();
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
			
			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/balloon/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/balloon/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_right", "FrostAndGunfireItems/Resources/balloon/MoveRight", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_left", "FrostAndGunfireItems/Resources/balloon/MoveLeft", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal);

			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that
			var bs = prefab.GetComponent<BehaviorSpeculator>();
            bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });
        }

		public class BalloonBehavior : CompanionController
		{
			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325
			private void Start()
			{
				base.aiActor.healthHaver.OnPreDeath += DoBlank;
				this.Owner = this.m_owner;
			}

			private void DoBlank(Vector2 obj)
			{
				Owner.ForceBlank(25f, 0.5f, false, true, base.aiActor.sprite.WorldCenter, true, -1f);
				float value = UnityEngine.Random.value;
				if (Owner.HasPickupID(ETGMod.Databases.Items["Toy Chest"].PickupObjectId) && (double)value < 0.01)
				{
					PickupObject byId3 = PickupObjectDatabase.GetById(224);
					LootEngine.SpawnItem(byId3.gameObject, base.aiActor.sprite.WorldCenter, Vector2.up, 1f, false, true, false);
				}
			}

			// Token: 0x0400006A RID: 106
			public PlayerController Owner;
		}



	}
}
