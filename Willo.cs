using System;
using System.Collections.Generic;
using Dungeonator;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000E RID: 14
	public class Willo : CompanionItem
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00005790 File Offset: 0x00003990
		public static void Init()
		{
			string name = "Wisper";
			string resourcePath = "FrostAndGunfireItems/Resources/wisper/wisper";
			GameObject gameObject = new GameObject();
			Willo willo = gameObject.AddComponent<Willo>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Friendly Fire";
			string longDesc = "This baby muzzle flash has seen all the secrets of the universe and is happy to share them with you. All of them...";
			ItemBuilder.SetupItem(willo, shortDesc, longDesc, "kp");
			willo.quality = PickupObject.ItemQuality.B;
			willo.CompanionGuid = Willo.guid;
			willo.SetupUnlockOnCustomFlag(CustomDungeonFlags.LICH_KILLED_AND_HUNTER, true);
			willo.Synergies = new CompanionTransformSynergy[0];
			Willo.BuildPrefab();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005803 File Offset: 0x00003A03
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.RevealSecretRooms();
			GameManager.Instance.OnNewLevelFullyLoaded += this.RevealSecretRooms;
			player.gameObject.AddComponent<Willo.WilloChestBehaviour>();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005834 File Offset: 0x00003A34
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			Willo component = debrisObject.GetComponent<Willo>();
			GameManager.Instance.OnNewLevelFullyLoaded -= this.RevealSecretRooms;
			bool flag = player.GetComponent<WilloChestBehaviour>() != null;
			if (flag)
			{
				UnityEngine.Object.Destroy(player.GetComponent<WilloChestBehaviour>());
			}
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005874 File Offset: 0x00003A74
		private void RevealSecretRooms()
		{
			for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
			{
				RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
				bool flag = roomHandler.connectedRooms.Count != 0;
				bool flag2 = flag;
				if (flag2)
				{
					bool flag3 = roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET;
					bool flag4 = flag3;
					if (flag4)
					{
						roomHandler.RevealedOnMap = true;
						Minimap.Instance.RevealMinimapRoom(roomHandler, true, true, roomHandler == GameManager.Instance.PrimaryPlayer.CurrentRoom);
					}
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00005924 File Offset: 0x00003B24
		public static void BuildPrefab()
		{

			Willo.gunVFXPrefab = SpriteBuilder.SpriteFromResource(Willo.gunVFX, null, false);
			Willo.itemVFXPrefab = SpriteBuilder.SpriteFromResource(Willo.itemVFX, null, false);
			Willo.gunVFXPrefab.name = Willo.vfxName;
			Willo.itemVFXPrefab.name = Willo.vfxName;
			UnityEngine.Object.DontDestroyOnLoad(Willo.gunVFXPrefab);
			FakePrefab.MarkAsFakePrefab(Willo.gunVFXPrefab);
			Willo.gunVFXPrefab.SetActive(false);
			UnityEngine.Object.DontDestroyOnLoad(Willo.itemVFXPrefab);
			FakePrefab.MarkAsFakePrefab(Willo.itemVFXPrefab);
			Willo.itemVFXPrefab.SetActive(false);
			bool flag = Willo.WisperPrefab != null || CompanionBuilder.companionDictionary.ContainsKey(Willo.guid);
			bool flag2 = flag;
			if (!flag2)
			{
				Willo.WisperPrefab = CompanionBuilder.BuildPrefab("Wisper", Willo.guid, Willo.spritePaths[0], new IntVector2(3, 2), new IntVector2(8, 9));
				Willo.WilloBehavior willoBehavior = Willo.WisperPrefab.AddComponent<Willo.WilloBehavior>();
				AIAnimator aiAnimator = willoBehavior.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle"
					}
				};
				DirectionalAnimation anim = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"attack"
					}
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "attack",
						anim = anim
					}
				};
				bool flag3 = Willo.wispCollection == null;
				bool flag4 = flag3;
				if (flag4)
				{
					Willo.wispCollection = SpriteBuilder.ConstructCollection(Willo.WisperPrefab, "Wisper_Collection");
					UnityEngine.Object.DontDestroyOnLoad(Willo.wispCollection);
					for (int i = 0; i < Willo.spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(Willo.spritePaths[i], Willo.wispCollection);
					}
					SpriteBuilder.AddAnimation(willoBehavior.spriteAnimator, Willo.wispCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						4,
						5,
						6
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
					SpriteBuilder.AddAnimation(willoBehavior.spriteAnimator, Willo.wispCollection, new List<int>
					{
						7,
						8,
						9,
						10,
						11,
						12
					}, "attack", tk2dSpriteAnimationClip.WrapMode.RandomLoop).fps = 9f;
				}
				willoBehavior.aiActor.MovementSpeed = 7f;
				willoBehavior.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
				willoBehavior.specRigidbody.Reinitialize();
				BehaviorSpeculator behaviorSpeculator = willoBehavior.behaviorSpeculator;
				behaviorSpeculator.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
				{
					IdleAnimations = new string[]
					{
						"idle"
					}
				});
				UnityEngine.Object.DontDestroyOnLoad(Willo.WisperPrefab);
				FakePrefab.MarkAsFakePrefab(Willo.WisperPrefab);
				Willo.WisperPrefab.SetActive(false);
			}
		}

		// Token: 0x0400001A RID: 26
		public static GameObject WisperPrefab;

		// Token: 0x0400001B RID: 27
		private static readonly string guid = "wisper";

		// Token: 0x0400001C RID: 28
		private static string[] spritePaths = new string[]
		{
			"FrostAndGunfireItems/Resources/wisper/wisper_idle_001",
			"FrostAndGunfireItems/Resources/wisper/wisper_idle_002",
			"FrostAndGunfireItems/Resources/wisper/wisper_idle_003",
			"FrostAndGunfireItems/Resources/wisper/wisper_idle_004",
			"FrostAndGunfireItems/Resources/wisper/wisper_idle_005",
			"FrostAndGunfireItems/Resources/wisper/wisper_idle_006",
			"FrostAndGunfireItems/Resources/wisper/wisper_idle_007",
			"FrostAndGunfireItems/Resources/wisper/wisper_attack_001",
			"FrostAndGunfireItems/Resources/wisper/wisper_attack_002",
			"FrostAndGunfireItems/Resources/wisper/wisper_attack_003",
			"FrostAndGunfireItems/Resources/wisper/wisper_attack_004",
			"FrostAndGunfireItems/Resources/wisper/wisper_attack_005",
			"FrostAndGunfireItems/Resources/wisper/wisper_attack_006"
		};

		// Token: 0x0400001D RID: 29
		private static tk2dSpriteCollectionData wispCollection;

		// Token: 0x0400001E RID: 30
		private static string gunVFX = "FrostAndGunfireItems/Resources/vfx_stuff/gun_vfx_001";

		// Token: 0x0400001F RID: 31
		private static string itemVFX = "FrostAndGunfireItems/Resources/vfx_stuff/item_vfx_001";

		// Token: 0x04000020 RID: 32
		private static string vfxName = "WisperVFX";

		// Token: 0x04000021 RID: 33
		private static List<Tuple<Chest, int>> foundChests = new List<Tuple<Chest, int>>();

		// Token: 0x04000022 RID: 34
		private static GameObject gunVFXPrefab;

		// Token: 0x04000023 RID: 35
		private static GameObject itemVFXPrefab;

		// Token: 0x02000034 RID: 52
		private class WilloChestBehaviour : BraveBehaviour
		{
			List<Chest> encounteredChests = new List<Chest>();
			PlayerController player;
			Chest nearbyChest;
			Vector2 offset = new Vector2(0, .25f);


			void Start()
			{
				player = GetComponent<PlayerController>();
			}

			void FixedUpdate()
			{
				if (!player || player.CurrentRoom == null)
					return;
				IPlayerInteractable nearestInteractable = player.CurrentRoom.GetNearestInteractable(player.sprite.WorldCenter, 1f, player);
				if (nearestInteractable != null && nearestInteractable is Chest)
				{
					var chest = nearestInteractable as Chest;

					if (!encounteredChests.Contains(chest) && !chest.transform.Find(vfxName))
						InitializeChest(chest);
					else
						nearbyChest = chest;
				}
				else
				{
					nearbyChest = null;
				}

				HandleChests();
			}

			void HandleChests()
			{
				foreach (var chest in encounteredChests)
				{
					if (!chest)
						continue;

					var fx = chest?.transform?.Find(vfxName)?.GetComponent<tk2dSprite>();

					if (!fx)
						continue;

					if (chest != nearbyChest)
						fx.scale = Vector3.Lerp(fx.scale, Vector3.zero, .25f);
					else
						fx.scale = Vector3.Lerp(fx.scale, Vector3.one, .25f);

					if (Vector3.Distance(fx.scale, Vector3.zero) < .01f)
						fx.scale = Vector3.zero;

					fx.PlaceAtPositionByAnchor(chest.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				}
			}

			void InitializeChest(Chest chest)
			{
				int guess = GetGuess(chest);
				GameObject prefab;
				if (guess == 0)
					prefab = gunVFXPrefab;
				else
					prefab = itemVFXPrefab;

				var sprite = GameObject.Instantiate(prefab, chest.transform).GetComponent<tk2dSprite>();
				sprite.name = vfxName;
				sprite.PlaceAtPositionByAnchor(chest.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				sprite.scale = Vector3.zero;

				nearbyChest = chest;
				encounteredChests.Add(chest);
			}

			int GetGuess(Chest chest)
			{
				var type = chest.ChestType;
				if (type == Chest.GeneralChestType.WEAPON)
					return 0;
				else if (type == Chest.GeneralChestType.ITEM)
					return 1;
				else
				{
					var contents = chest.PredictContents(player);
					foreach (var item in contents)
					{
						if (item is Gun) return 0;
						if (item is PlayerItem || item is PassiveItem) return 1;
					}
				}
				return UnityEngine.Random.Range(0, 2);
			}

			public void DestroyAllFX()
			{
				foreach (var chest in encounteredChests)
				{
					var fx = chest.transform.Find(vfxName);
					if (fx)
						Destroy(fx);
				}
				encounteredChests.Clear();
			}
		}
	


		// Token: 0x02000035 RID: 53
		public class WilloBehavior : CompanionController
		{
			// Token: 0x06000132 RID: 306 RVA: 0x0000B721 File Offset: 0x00009921
			private void Start()
			{
				base.spriteAnimator.Play("idle");
				this.player = this.m_owner;
			}


			public PlayerController player;
		}

		// Token: 0x02000036 RID: 54
	}
}
