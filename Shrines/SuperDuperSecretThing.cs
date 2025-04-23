
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
				room = RoomFactory.BuildFromResource("FrostAndGunfireItems/Resources/rooms/MinibossShrineRoom.room").room,
				RoomWeight = 1.3f,
				acceptText = "Poke It?",
				declineText = "Leave It",
				OnAccept = null,
				OnDecline = null,
				OnInteracted = null,
				offset = new Vector3(100f, 100f, 0f),
				talkPointOffset = new Vector3(0, 3, 0),
				isToggle = false,
				isBreachShrine = false,
				
			};
			//register shrine
			sf.Build();
		}



	}
			// Token: 0x0600004F RID: 79 RVA: 0x00004810 File Offset: 0x00002A10
		}
	


