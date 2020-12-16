using System;
using System.Collections.Generic;
using GungeonAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x020000E4 RID: 228
	public static class TableNPC
	{
		public static bool YouTalkedToMeAlready;
		private static int TableNum = 0;
		public static bool QuestComplete = false;
		public static bool RewardGotten = false;
		private static int TableReq;
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "The Knight That's A Round Table",
				modID = "kp",
				spritePath = "FrostAndGunfireItems/Resources/TableNPC/tableNPC_idle_001.png",
				acceptText = "Teach me the ways of the Table, oh Wooden One.",
				declineText = "I'll remain pitiful.",
				OnAccept = Accept,
				OnDecline = null,
				CanUse = CanUse,
				offset = new Vector3(-1.5f, 5.52f, 0f),
				//offset = new Vector3(51.2f, 50.8f, 51.3f),
				talkPointOffset = new Vector3(0.75f, 1.5f, 0f),
				isToggle = false,
				isBreachShrine = false,
				room = RoomFactory.BuildFromResource("FrostAndGunfireItems/Resources/rooms/FilpsalotRoom.room").room,
				RoomWeight = 1.5f,
				interactableComponent = typeof(TableNPCInteractable)
			};

			GameObject gameObject = shrineFactory.Build();
			TableNPCInteractable component = gameObject.GetComponent<TableNPCInteractable>();
			gameObject.AddAnimation("idle", "FrostAndGunfireItems/Resources/TableNPC/tableNPC_idle", 4, NPCBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			gameObject.AddAnimation("talk", "FrostAndGunfireItems/Resources/TableNPC/tableNPC_talk", 9, NPCBuilder.AnimationType.Talk, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			//gameObject.AddAnimation("talk_start", "FrostAndGunfireItems/Resources/TableNPC/tableNPC_talk", 12, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			//gameObject.AddAnimation("do_effect", "FrostAndGunfireItems/Resources/TableNPC/tableNPC_talk", 12, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			component.conversation = new List<string>
			{
				"You there!",
				"I've seen thee dawble about the Gungeon.",
				"It's pitiful.",
				"Worry not pitiful one, for I, Ser Flipsalot, will teach thy the ways of the Table."
			};
			component.conversation2 = new List<string>
			{
				"You did it, you actually did it!",
				"I thought this quest would surely end you.",
				"Congratulations.",
				"I will teach thee a secret technique from the Knights of the Octagonal Table.",
					"",


			};
			component.conversation3 = new List<string>
			{
				"Well what are you standing around here for?",
				"You have a quest to complete.",
					"",


			};
			component.conversation4 = new List<string>
			{
				"Welcome back mighty hero.",
				"I have nothing more to teach you.",
				"",
			
			};
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000306B0 File Offset: 0x0002E8B0
		private static bool CanUse(PlayerController player, GameObject npc)
		{
			return true;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000306C4 File Offset: 0x0002E8C4
		public static void Accept(PlayerController player, GameObject npc)
		{
			if(QuestComplete && !RewardGotten)
			{
				RewardGotten = true;
				string itemToGive;
				itemToGive = BraveUtility.RandomElement<string>(ItemToGet);
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName(itemToGive).gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
				player.OnNewFloorLoaded -= ShameNotification;

			}
			else if(!YouTalkedToMeAlready)
			{
				YouTalkedToMeAlready = true;
				TableReq = UnityEngine.Random.Range(2, 5);
				//string header = "Flip " + TableReq + " tables";
				string header = "Flip " + TableReq + " tables";
				string text = "";
				player.OnTableFlipped += TableQuest;
				player.OnNewFloorLoaded += ShameNotification;
		     	Notify(header, text);
			}
		
		}

		private static void TableQuest(FlippableCover cover)
		{
			PlayerController player = GameManager.Instance.PrimaryPlayer;
			TableNum++;
				if (TableNum == TableReq)
			  {
					QuestComplete = true;
				string header = "Quest Completed";
				string text = "";
				AkSoundEngine.PostEvent("Play_OBJ_chest_surprise_01", player.gameObject);
				Notify(header, text);
			}
	}

		public static string[] ItemToGet = new string[]
		{
			"Table Tech Life",
			"Table Tech Petrify",
			"Table Tech Stun",
			"Table Tech Rage",
			"Table Tech Sight",
			"Table Tech Blank",
			"Table Tech Life",
			"Table Tech Money",
			"Table Tech Heat",
			"Portable Table Device",

		};

		private static void ShameNotification(PlayerController player)
		{
			string header = "Shame";
			string text = "Quest Incomplete";
			PickupObject byId = PickupObjectDatabase.GetByEncounterName("Table Tech Shame");
			player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
			TableReq = -1;
			YouTalkedToMeAlready = false;
		   player.OnNewFloorLoaded -= ShameNotification;
			Notify(header, text);
		}

		private static void Notify(string header, string text)
		{
			tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, false, true);
		}
	}
}
