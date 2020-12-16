
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace FrostAndGunfireItems
{
	
	public static class ChampionShrine
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00004700 File Offset: 0x00002900
		public static void Add()
		{
			OldShrineFactory sf = new OldShrineFactory()
			{

				name = "Champion Shrine",
				modID = "kp",
				text = "A shrine dedicated to the champions of the Gungeon.",
				spritePath = "FrostAndGunfireItems/Resources/shrine/champion_shrine.png",
				acceptText = "Toggle All Champions Mode?",
				declineText = "Leave",
				OnAccept = Accept,
				OnDecline = null,
				//offset = new Vector3(43.8f, 42.4f, 42.9f),
				offset = new Vector3((int)32, 57.4f, 0),
				talkPointOffset = new Vector3(0, 0, 0),
				isToggle = false,
				isBreachShrine = true
			};
			//register shrine
			sf.Build();
		}

		public static void Accept(PlayerController player, GameObject shrine)
		{
			string header = "";
			string text = "";
			if (Champions.AllChampions == true)
			{
				Champions.AllChampions = false;
				Champions.ChampionsOn = false;
				global::ETGModConsole.Log("All Champions have been disabled", false);
				header = "All Champions Mode Disabled";

			}
			else
			{
				Champions.AllChampions = true;
				Champions.ChampionsOn = true;
				global::ETGModConsole.Log("All Champions have been enabled", false);
				header = "All Champions Mode Enabled";
			}
			Notify(header, text);

		}

		private static void Notify(string header, string text)
		{
			tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, false, true);
		}

	}
}
	


