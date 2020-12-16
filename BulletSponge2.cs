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
	class BulletSponge2 : CompanionItem
	{
		public static GameObject prefab;
		public static readonly string guid = "bullet sponge";
		private static tk2dSpriteCollectionData BulletSpongeCollection;


	

		static int NumOfBulletsHitWith;
		public static void Init()
		{
			string itemName = "Bullet Sponge";
			string resourceName = "FrostAndGunfireItems/Resources/bullet_sponge_idle_001";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<BulletSponge2>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "Extra Absorbent";
			string longDesc = "add lore later";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.C;
			item.CompanionGuid = guid;

			BuildPrefab();
		}
		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			prefab = CompanionBuilder.BuildPrefab("Bullet Sponge", guid, "FrostAndGunfireItems/Resources/bullet_sponge_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

			//Add a companion component to the prefab (could be a custom class)
			var companion = prefab.AddComponent<BulletSpongeBehavior>();
			companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
			companion.aiActor.MovementSpeed = 9f;
			companion.CanInterceptBullets = true;
			companion.aiActor.healthHaver.PreventAllDamage = true;
			companion.aiActor.CollisionDamage = 0f;
			companion.aiActor.HasShadow = false;
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
			
			bool flag3 = BulletSpongeCollection == null;
			if (flag3)
			{
				BulletSpongeCollection = SpriteBuilder.ConstructCollection(prefab, "Bullet_Sponge_Collection");
				UnityEngine.Object.DontDestroyOnLoad(BulletSpongeCollection);
				for (int i = 0; i < spritePaths.Length; i++)
				{
					SpriteBuilder.AddSpriteToCollection(spritePaths[i], BulletSpongeCollection);
				}
				SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletSpongeCollection, new List<int>
					{
					0,
					1,
					2,
					3,
					4,
					5,
					6,
					
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;

			}
			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that
			var bs = prefab.GetComponent<BehaviorSpeculator>();
			bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });
		}

		private static readonly string[] spritePaths = new string[]
			{
		   "FrostAndGunfireItems/Resources/bullet_sponge_idle_001",
			"FrostAndGunfireItems/Resources/bullet_sponge_idle_002",
			 "FrostAndGunfireItems/Resources/bullet_sponge_idle_003",
			  "FrostAndGunfireItems/Resources/bullet_sponge_idle_004",
			   "FrostAndGunfireItems/Resources/bullet_sponge_idle_005",
				"FrostAndGunfireItems/Resources/bullet_sponge_idle_006",
				 "FrostAndGunfireItems/Resources/bullet_sponge_idle_007",
				 //bullet_sponge2
				 "FrostAndGunfireItems/Resources/bullet_sponge_002",
				 "FrostAndGunfireItems/Resources/bullet_sponge_003"
				//bullet_sponge3

	};


		public class BulletSpongeBehavior : CompanionController
		{
			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325
			private void Start()
			{
           this.Owner = this.m_owner;
			}

			
			// Token: 0x0400006A RID: 106
			public PlayerController Owner;
		}
	}
}


