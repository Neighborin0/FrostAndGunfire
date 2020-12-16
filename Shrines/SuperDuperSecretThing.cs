
using GungeonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static GungeonAPI.OldShrineFactory;

namespace FrostAndGunfireItems
{
	// Token: 0x02000009 RID: 9
	public static class SuperDuperSecretThing
	{
		
		public static void Add()
		{
			OldShrineFactory sf = new OldShrineFactory
			{

				name = "Miniboss Shrine",
				modID = "kp",
				text = "It's A Huge Hole.",
				spritePath = "FrostAndGunfireItems/Resources/shrine/shrine_base.png",
				//room = RoomFactory.BuildFromResource("FrostAndGunfireItems/Resources/rooms/MinibossShrineRoom.room").room,
				RoomWeight = 9999f,
				acceptText = "Poke It?",
				declineText = "Leave It",
				OnAccept = Accept,
				OnDecline = null,
				//offset = new Vector3(43.8f, 42.4f, 42.9f),
				offset = new Vector3(-1, -1, 0),
				talkPointOffset = new Vector3(0, 3, 0),
				isToggle = false,
				isBreachShrine = false,

				
			};
			//register shrine
			sf.Build();
		}


		public static void Accept(PlayerController player, GameObject shrine)
		{
			GameManager.Instance.StartCoroutine(DoBossSpawn(player, shrine));
		}

		private static IEnumerator DoBossSpawn(PlayerController player, GameObject shrine)
		{
			IntVector2 spawnspot = player.CurrentRoom.GetCenterCell();
			IntVector2 offset = new IntVector2(4, 0);
			yield return new WaitForSeconds(2f);
			UnityEngine.Object.Destroy(shrine);
			LootEngine.DoDefaultItemPoof(shrine.transform.position, false, true);
			yield return new WaitForSeconds(2f);
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("Room Mimic");
			AIActor aiactor = AIActor.Spawn(orLoadByGuid, spawnspot - offset, player.gameObject.transform.position.GetAbsoluteRoom(), true, AIActor.AwakenAnimationType.Default, true);
		    aiactor.ParentRoom.SealRoom();
			aiactor.healthHaver.OnDeath += (obj) =>
			{
				Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(spawnspot);
				chest2.IsLocked = false;

			};
			yield break;
		}



	}
			// Token: 0x0600004F RID: 79 RVA: 0x00004810 File Offset: 0x00002A10
		}
	


