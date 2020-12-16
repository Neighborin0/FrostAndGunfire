using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using Dungeonator;
using GungeonAPI;
using System.Collections;

namespace FrostAndGunfireItems
{
    public class BFO : CompanionItem
	{
        public static GameObject prefab;
        public static readonly string guid = "BFO";
		public float InternalCooldown;
		private float m_lastUsedTime;

		public static void Init()
        {
			string name = "B.F.O";
			string resourcePath = "FrostAndGunfireItems/Resources/BFO/Idle/bfo_idle_001";
			GameObject gameObject = new GameObject();
			BFO bFO = gameObject.AddComponent<BFO>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Best Friend Object";
			string longDesc = "A toy made by the Hegemony, the B.F.O is equipped with a state of the art \"fun\" targetting system.\n\nThis is not responsible for any possible health hazards. Results may vary. Battery may be included?";
			ItemBuilder.SetupItem(bFO, shortDesc, longDesc, "kp");
			bFO.quality = PickupObject.ItemQuality.S;
			bFO.CompanionGuid = BFO.guid;
			bFO.SetupUnlockOnCustomFlag(CustomDungeonFlags.LICH_KILLED_AND_ROBOT, true);
			BFO.BuildPrefab();

		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnEnteredCombat +=  EnterCombat;
			player.OnNewFloorLoaded += (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		}

		private void EnterCombat()
		{
			HandleHighlight();
		}

		private void HandleHighlight()
		{
			RoomHandler absoluteRoom = Vector3Extensions.GetAbsoluteRoom(base.transform.position);
			AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
			bool flag4 = randomActiveEnemy != null && randomActiveEnemy.healthHaver  && randomActiveEnemy.healthHaver.IsAlive && randomActiveEnemy.healthHaver.IsVulnerable;
			if (flag4)
			{
				if (randomActiveEnemy.healthHaver.IsBoss)
				{
					randomActiveEnemy.MovementSpeed *= 0.90f;
					randomActiveEnemy.healthHaver.SetHealthMaximum(randomActiveEnemy.healthHaver.GetMaxHealth() * 0.9f);
					Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(randomActiveEnemy.sprite);
					outlineMaterial.SetColor("_OverrideColor", new Color(99f, 0f, 0f));
					randomActiveEnemy.healthHaver.OnDeath += OnPreDeath;
				}
				else
				{
					randomActiveEnemy.healthHaver.SetHealthMaximum(randomActiveEnemy.healthHaver.GetMaxHealth() / 1.5f);
					randomActiveEnemy.MovementSpeed *= 0.90f;
					Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(randomActiveEnemy.sprite);
					outlineMaterial.SetColor("_OverrideColor", new Color(99f, 0f, 0f));
					randomActiveEnemy.healthHaver.OnDeath += OnPreDeath;
				}

			}

		}

		private void OnPreDeath(Vector2 obj)
		{
			float value = UnityEngine.Random.value;
			if (Owner.HasPickupID((ETGMod.Databases.Items["Toy Chest"].PickupObjectId)) && (double)value < 0.01)
			{
				GenericLootTable singleItemRewardTable = GameManager.Instance.RewardManager.CurrentRewardData.SingleItemRewardTable;
				LootEngine.SpawnItem(singleItemRewardTable.SelectByWeight(false), Owner.CenterPosition, Vector2.up, 1f, true, false, false);
			
			}
			base.StartCoroutine(HolUp());
			this.InternalCooldown = 25f;
			bool flag = Time.realtimeSinceStartup - this.m_lastUsedTime < this.InternalCooldown;
			if (!flag)
			{
				this.m_lastUsedTime = Time.realtimeSinceStartup;
				int num = UnityEngine.Random.Range(0, 3);
				if (num == 0)
				{
					AkSoundEngine.PostEvent("Play_WPN_radgun_cool_01", base.gameObject);
				}
				if (num == 1)
				{
					AkSoundEngine.PostEvent("Play_WPN_radgun_noice_01", base.gameObject);
				}
				if (num == 2)
				{
					AkSoundEngine.PostEvent("Play_WPN_radgun_rad_01", base.gameObject);
				}
			}	

		}

		private IEnumerator HolUp()
		{
			yield return new WaitForSeconds(1.5f);
			RoomHandler currentRoom = Owner.CurrentRoom;
			if (currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
			{
				HandleHighlight();
			}
			yield break;
		}

		public override DebrisObject Drop(PlayerController player)
		{

			player.OnEnteredCombat -= EnterCombat;
			player.OnNewFloorLoaded -=  HandleNewFloor;
		DebrisObject debrisObject = base.Drop(player);
			BFO component = debrisObject.GetComponent<BFO>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		private void HandleNewFloor(PlayerController obj)
		{
			if (Owner != null)
			{
				obj.OnEnteredCombat = EnterCombat;
			}
		}

		public static void BuildPrefab()
        {
            if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
                return;

            //Create the prefab with a starting sprite and hitbox offset/size
            prefab = CompanionBuilder.BuildPrefab("Best Friend", guid, "FrostAndGunfireItems/Resources/BFO/Idle/bfo_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

            //Add a companion component to the prefab (could be a custom class)
            var companion = prefab.AddComponent<BFOBehavior>();
			companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
			companion.aiActor.MovementSpeed = 11f;
			companion.aiActor.HasShadow = false;
			companion.CanInterceptBullets = true;
			companion.aiActor.healthHaver.PreventAllDamage = true;
			companion.aiActor.CollisionDamage = 0f;
			companion.aiActor.specRigidbody.CollideWithOthers = true;
			companion.aiActor.specRigidbody.CollideWithTileMap = false;
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
			prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/BFO/Idle", fps: 9, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/BFO/Idle", fps: 9, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_right", "FrostAndGunfireItems/Resources/BFO/MoveRight", fps: 10, AnimationType.Move, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_left", "FrostAndGunfireItems/Resources/BFO/MoveLeft", fps: 10, AnimationType.Move, DirectionType.TwoWayHorizontal);

			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that
			var bs = prefab.GetComponent<BehaviorSpeculator>();
            bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });
        }

		public class BFOBehavior : CompanionController
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



	}
}
