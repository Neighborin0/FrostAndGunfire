using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000F RID: 15
	public class TableTechShame : PassiveItem
	{
		

		
	
		public static void Init()
		{
			string name = "Table Tech Shame";
			string resourcePath = "FrostAndGunfireItems/Resources/tabletech_shame";
			GameObject gameObject = new GameObject();
			TableTechShame tableTechShame = gameObject.AddComponent<TableTechShame>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Flip Off";
			string longDesc = "A angry note from the Knights of the Octagonal Table. This letter allows even Tables to shame you.";
			ItemBuilder.SetupItem(tableTechShame, shortDesc, longDesc, "kp");
			tableTechShame.CanBeDropped = false;
			tableTechShame.quality = PickupObject.ItemQuality.EXCLUDED;
		
		}

		public static string[] SoundToGet = new string[]
	{
			"Play_WPN_LowerCaseR_Angry_Loser_01",
			"Play_WPN_LowerCaseR_Angry_Chump_01",
			"Play_WPN_LowerCaseR_Angry_Dork_01",
			"Play_WPN_LowerCaseR_Angry_Noodle_01",
			"Play_WPN_LowerCaseR_Angry_Taffer_01",


	};


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnTableFlipCompleted += HandleFlip;
		
		}

		
		private void HandleFlip(FlippableCover table)
		{
			string Play;
			Play = BraveUtility.RandomElement<string>(SoundToGet);
			AkSoundEngine.PostEvent(Play, base.gameObject);
		}


		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			player.OnTableFlipCompleted -= HandleFlip;
			TableTechShame component = debrisObject.GetComponent<TableTechShame>();
			component.m_pickedUpThisRun = true;
		
	
			
			return debrisObject;

		}
	
	}
}
