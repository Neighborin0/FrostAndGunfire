
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
	public static class FAGFShrine
	{
		
		public static void Add()
		{
			OldShrineFactory sf = new OldShrineFactory
			{

				name = "FAGF Shrine",
				modID = "kp",
				text = "A shrine to two elementals. It's cold to the touch but also radiates warmth.",
				spritePath = "FrostAndGunfireItems/Resources/shrine/shrine_base.png",
				room = RoomFactory.BuildFromResource("FrostAndGunfireItems/Resources/rooms/FAGFShrineRoom.room").room,
				RoomWeight = 1.5f,
				acceptText = "Make an offer to the elementals?",
				declineText = "Leave",
				OnAccept = Accept,
				OnDecline = null,
				CanUse = CanUse,
				//offset = new Vector3(43.8f, 42.4f, 42.9f),
				offset = new Vector3(-1, -1, 0),
				talkPointOffset = new Vector3(0, 3, 0),
				isToggle = false,
				isBreachShrine = false,

				
			};
			//register shrine
			sf.Build();
		}

		public static bool CanUse(PlayerController player, GameObject shrine)
		{
			return shrine.GetComponent<CustomShrineController>().numUses == 0 && player.carriedConsumables.Currency > 34;
		}

		public static void Accept(PlayerController player, GameObject shrine)
		{
			
			
				
					int RandomItem = UnityEngine.Random.Range(0, 50);
					if (RandomItem == 0)
					{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Elemental Force").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			

					}
					if (RandomItem == 1)
					{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Build-A-Gun").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			
				
					}
					if (RandomItem == 2)
					{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Longshot").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
					
						if (RandomItem == 3)
						{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("The Core").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
						if (RandomItem == 4)
						{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Drawn").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
						if (RandomItem == 5)
						{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("R.N.Gun").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
						if (RandomItem == 6)
						{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Cursed Cylinder").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
						
						if (RandomItem == 7)
						{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Table Tech Life").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
						if (RandomItem == 8)
						{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Penguin").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
			if (RandomItem == 9)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Rusty Ammolet").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}

			if (RandomItem == 10)
							{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Mirror Guon Stone").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
							
							if (RandomItem == 11)
							{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Trash Bag").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
							if (RandomItem == 12)
							{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Wisper").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
							if (RandomItem == 13)
							{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Void Bullets").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}

								
								if (RandomItem == 14)
								{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Flamethrower").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
								if (RandomItem == 15)
								{

				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Holy Rounds").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
								if (RandomItem == 16)
								{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Gold Chainz").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
								if (RandomItem == 17)
								{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Pure Flower").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
			}
							
								if (RandomItem == 18)
								{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Handgun").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
								if (RandomItem == 19)
								{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Afflicted Ammolet").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 20)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Slime Ammolet").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 21)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Table Tech Petrify").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 22)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Blue Balloon").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 23)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Ice Tray").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 24)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Perfection Round").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 25)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Frost Armor").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 26)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Blamulet").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 27)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("B.F.O").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 28)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Toy Chest").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 29)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Squire").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 30)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Locked Armor").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 31)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Potion of Gun Swiftness").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 32)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Crystal Die").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 33)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Broken Duplicator").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 34)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Mini Muncher").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 35)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Smelter").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 36)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Pocket Pistol").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 37)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Emergency Supplies").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 38)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Spent Sack").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 39)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Super Shroom").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 40)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Void Chest").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 41)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("iBuy").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 42)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Bloodied Key").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 43)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Beastly Pistol").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 44)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Rage Rifle").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 45)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Graviton").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 46)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Hotshot").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 47)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Icepick").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 48)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Orbiter").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}
			if (RandomItem == 49)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetByEncounterName("Big Sniper Rifle").gameObject, player.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

			}



			player.carriedConsumables.Currency -= 35;
			shrine.GetComponent<CustomShrineController>().numUses++;
			shrine.GetComponent<CustomShrineController>().GetRidOfMinimapIcon();
			AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);
		}



			}
			// Token: 0x0600004F RID: 79 RVA: 0x00004810 File Offset: 0x00002A10
		}
	


