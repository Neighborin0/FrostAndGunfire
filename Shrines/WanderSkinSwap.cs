
using GungeonAPI;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace FrostAndGunfireItems
{
	
	public  class WanderSkinSwap : SimpleShrine
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00004700 File Offset: 0x00002900
		public static void Add()
		{
			OldShrineFactory sf = new OldShrineFactory()
			{

				name = "Skin Swap Shrine",
				modID = "kp",
				spritePath = "FrostAndGunfireItems/Resources/characters/Wanderer/alt_skin_obj_sprite_002.png",
				OnDecline = null,
				//offset = new Vector3(43.8f, 42.4f, 42.9f),
				offset = new Vector3(15.5f, 23.0f, 24f),
				talkPointOffset = new Vector3(0, 0, 0),
				CanUse = CanUse,
				OnInteracted = OnInteract,
				CantTalk = false,
				isToggle = true,
				isBreachShrine = true,
				
			};
			//register shrine
			GameObject gameObject = sf.Build();
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<SpeculativeRigidbody>();
			speculativeRigidbody.PixelColliders.Clear();
			speculativeRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.PlayerHitBox,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				ManualWidth = 15,
				ManualHeight = 13,
				ManualDiameter = 30,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});


		}

		private static bool CanUse(PlayerController player, GameObject npc)
		{
			return player.name.Contains("Wanderer");
		}
		public static void OnInteract(PlayerController player, GameObject shrine)
		{
			var sprite = shrine.GetComponent<tk2dBaseSprite>();
			WanderSkinSwap.spriteIDs = new int[WanderSkinSwap.spritePaths.Length];
			WanderSkinSwap.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(WanderSkinSwap.spritePaths[0], sprite.sprite.Collection);
			WanderSkinSwap.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(WanderSkinSwap.spritePaths[1], sprite.sprite.Collection);
			if (GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Fury"].PickupObjectId))
				{

				ETGModConsole.RunCommand(new String[] { "character" }, new String[] { "wanderer" });
				sprite.sprite.SetSprite(WanderSkinSwap.spritePaths[0]);

			}
			else if(GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Meltdown"].PickupObjectId))
            {
				ETGModConsole.RunCommand(new String[] { "character" }, new String[] { "wanderer_fire" });
				sprite.sprite.SetSprite(WanderSkinSwap.spritePaths[1]);
			}

		}

		public static int[] spriteIDs;
		private static readonly string[] spritePaths = new string[]
	{
			"FrostAndGunfireItems/Resources/characters/Wanderer/alt_skin_obj_sprite_002",
			"FrostAndGunfireItems/Resources/characters/Wanderer/alt_skin_obj_sprite_001"
	};
		// Token: 0x04000021 RID: 33

	}
}
	


