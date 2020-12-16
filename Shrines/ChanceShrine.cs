
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static GungeonAPI.OldShrineFactory;

namespace FrostAndGunfireItems
{
	// Token: 0x02000009 RID: 9
	public static class ChanceShrine
	{
		
		
		public static void Add()
		{
			OldShrineFactory sf = new OldShrineFactory()
			{

				name = "Chance Shrine",
				modID = "kp",
				text = "A shrine of chance.",
				spritePath = "FrostAndGunfireItems/Resources/shrine/chance_shrine.png",
				room = RoomFactory.BuildFromResource("FrostAndGunfireItems/Resources/rooms/ChanceShrineRoom.room").room,
				RoomWeight = 1,
				acceptText = $"Chance It?" ,
				declineText = "Leave",
				OnAccept = Accept,
				OnDecline = null,
				CanUse = CanUse,
				//offset = new Vector3(43.8f, 42.4f, 42.9f),
				offset = new Vector3((int)-1.1, 6, 0),
				talkPointOffset = new Vector3(0, 3, 0),
				isToggle = false,
				isBreachShrine = false
			};
			//register shrine
			sf.Build();
		}

		public static bool CanUse(PlayerController player, GameObject shrine)
		{
			float num = 1.4f * shrine.GetComponent<CustomShrineController>().numUses;
			if(shrine.GetComponent<CustomShrineController>().numUses == 0)
			{
				return player.carriedConsumables.Currency >= 20;
			}
			else
			{
				return player.carriedConsumables.Currency >= 20 * num;
			}
			
		}


		public static void Accept(PlayerController player, GameObject shrine)
		{
			shrine.GetComponent<CustomShrineController>().numUses += 1;
			double numUses = 1.4 * shrine.GetComponent<CustomShrineController>().numUses;
			if (shrine.GetComponent<CustomShrineController>().numUses == 0)
			{
				player.carriedConsumables.Currency -= 20;
			}
			else
			{
				player.carriedConsumables.Currency -= 20 * (int)numUses;
			}
			float num = UnityEngine.Random.Range(0f, 1f);
			///nothing
			bool flag = (double)num < 0.40;
			bool flag2 = flag;
			if (flag2)
			{
				string header = "You Get Nothing!";
				string text = "You Lose!";
				Notify(header, text);
				AkSoundEngine.PostEvent("Play_wpn_jk47_droop_01", shrine);
	
			}

			else
			{
				///D tier items
				bool flag9 = (double)num < 0.80;
				bool flag10 = flag9;
				if (flag10)
				{
					Vector2 offest = new Vector2(0f, 2f);
					PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.D;
					PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
					if (itemOfTypeAndQuality)
					{
						LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, player.sprite.WorldBottomCenter, Vector2.zero - offest, 1f, true, true, false);
					}
					AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);
				}
				else
				{

					///C tier items
					bool flag3 = (double)num < 0.85;
					bool flag4 = flag3;
					if (flag4)
					{
						PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.C;
						PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
						if (itemOfTypeAndQuality)
						{
							LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, player.CenterPosition, Vector2.up, 1f, true, true, false);
						}
						AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);

					}

					else
					{
						///B tier items
						bool flag5 = (double)num < 0.90;
						bool flag6 = flag5;
						if (flag6)
						{
							PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.B;
							PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
							if (itemOfTypeAndQuality)
							{
								LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, player.CenterPosition, Vector2.up, 1f, true, true, false);
							}
							AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);

						}

						else
						{
							///A tier items
							bool flag7 = (double)num < 0.97;
							bool flag8 = flag7;
							if (flag8)
							{
								PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.A;
								PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
								if (itemOfTypeAndQuality)
								{
									LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, player.CenterPosition, Vector2.up, 1f, true, true, false);
								}
								AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);
							}

							else
							{
								///S tier items

								PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.S;
								PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
								if (itemOfTypeAndQuality)
								{
									LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, player.CenterPosition, Vector2.up, 1f, true, true, false);
								}
								AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);
							}
						}
					}
				}
			
			}

			
		}

		private static void Notify(string header, string text)
		{
			tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, false, false);
		}
		// Token: 0x0600004F RID: 79 RVA: 0x00004810 File Offset: 0x00002A10
	}
}
	


