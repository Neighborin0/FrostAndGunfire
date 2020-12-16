
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace FrostAndGunfireItems
{
	
	public static class LogShrine
	{
		 static tk2dUIScrollbar tk2DUIScrollbar;
		// Token: 0x0600004D RID: 77 RVA: 0x00004700 File Offset: 0x00002900
		public static void Add()
		{
			OldShrineFactory sf = new OldShrineFactory()
			{

				name = "Log Shrine",
				modID = "kp",
				text = "To Do List\n\nBeat the Dragun as either Wanderer\n\nBeat the Lich as either Wanderer\n\nBeat the Rat as either Wanderer\n\nBeat Challenge Mode as either Wanderer\n\nBeat Boss Rush as either Wanderer\n\nBeat the Lich as Pilot\n\nBeat the Lich as Marine\n\nBeat the Lich as Convict\n\nBeat the Lich as Hunter\n\nBeat the Lich as Bullet\n\nBeat the Lich as Robot\n\nBeat the Lich as Paradox\n\nBeat the Lich as Gunslinger",
				spritePath = "FrostAndGunfireItems/Resources/shrine/champion_shrine.png",
				acceptText = "Leave",
				declineText = null,
				OnAccept = null,
				OnDecline = null,
				//offset = new Vector3(43.8f, 42.4f, 42.9f),
				offset = new Vector3((int)44, 57.4f, 0),
				talkPointOffset = new Vector3(0, 0, 0),
				isToggle = false,
				isBreachShrine = true
				
			};
			//register shrine
			sf.Build();
		}
	}
}
	


