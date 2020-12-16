
using GungeonAPI;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace FrostAndGunfireItems
{


	public class FrostandGunFireItems : ETGModule
	{

		public static Texture2D ModLogo;
		public static Hook MainMenuFoyerUpdateHook;
		public static bool LogoEnabled = false;
		public static GameObject shrine;
		public static AdvancedStringDB Strings;
		public override void Init()
		{

		}



		private void MainMenuUpdateHook(Action<MainMenuFoyerController> orig, MainMenuFoyerController self)
		{
			orig(self);
			if (((dfTextureSprite)self.TitleCard).Texture.name != ModLogo.name)
			{
				((dfTextureSprite)self.TitleCard).Texture = ModLogo;
				LogoEnabled = true;
			}
		}
	
			// Token: 0x060000E8 RID: 232 RVA: 0x00009624 File Offset: 0x00007824
			public override void Start()
		{
			
			AdvancedGameStatsManager.AdvancedGameSave = new SaveManager.SaveType
			{
				filePattern = "Slot{0}.advancedSave",
				encrypted = true,
				backupCount = 3,
				backupPattern = "Slot{0}.advancedBackup.{1}",
				backupMinTimeMin = 45,
				legacyFilePattern = "advancedGameStatsSlot{0}.txt"
			};
			for (int i = 0; i < 3; i++)
			{
				SaveManager.SaveSlot saveSlot = (SaveManager.SaveSlot)i;
				Tools.SafeMove(Path.Combine(SaveManager.OldSavePath, string.Format(AdvancedGameStatsManager.AdvancedGameSave.legacyFilePattern, saveSlot)), Path.Combine(SaveManager.OldSavePath,
					string.Format(AdvancedGameStatsManager.AdvancedGameSave.filePattern, saveSlot)), false);
				Tools.SafeMove(Path.Combine(SaveManager.OldSavePath, string.Format(AdvancedGameStatsManager.AdvancedGameSave.filePattern, saveSlot)), Path.Combine(SaveManager.OldSavePath,
					string.Format(AdvancedGameStatsManager.AdvancedGameSave.filePattern, saveSlot)), false);
				Tools.SafeMove(Tools.PathCombine(SaveManager.SavePath, "01", string.Format(AdvancedGameStatsManager.AdvancedGameSave.filePattern, saveSlot)), Path.Combine(SaveManager.SavePath,
					string.Format(AdvancedGameStatsManager.AdvancedGameSave.filePattern, saveSlot)), true);
			}
			AdvancedGameStatsManager.Init();
			FrostandGunFireItems.Strings = new AdvancedStringDB();
			try
			{
				
				GungeonAP.Init();
				FAGFShrine.Add();
				ChanceShrine.Add();
				ChampionShrine.Add();
				TableNPC.Add();
				//SuperDuperSecretThing.Add();
				//LogShrine.Add();

				ETGModConsole.Commands.AddUnit("iceflow", (args) =>
				{
					DungeonHandler.debugFlow = !DungeonHandler.debugFlow;
					string status = DungeonHandler.debugFlow ? "enabled" : "disabled";
					string color = DungeonHandler.debugFlow ? "00FF00" : "FF0000";
					Tools.Print($"Debug flow {status}", color);
				});
				ETGModConsole.Commands.AddUnit("showpos", (args) =>
				{
					ETGModConsole.Log("Player position: " + GameManager.Instance.PrimaryPlayer.transform.position);
					ETGModConsole.Log("Player center: " + GameManager.Instance.PrimaryPlayer.sprite.WorldCenter);
				});
				ETGModConsole.Commands.AddUnit("hitboxes", (args) =>
				{
					foreach (var obj in GameObject.FindObjectsOfType<SpeculativeRigidbody>())
					{
						if (obj && obj.sprite && Vector2.Distance(obj.sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter) < 8)
						{
							Tools.Log(obj?.name);
							HitboxMonitor.DisplayHitbox(obj);
						}
					}
				});
			}


			catch (Exception e)
			{
				Tools.Print("Failed to load shrineAPI", "FF0000", true);
				Tools.PrintException(e);
			}

			//item and shit
			ItemBuilder.Init();
			BigSniperRifle.Add();
			AfflictedAmmolet.Init();
			Flamethrower.Add();
			Drawn.Add();
			Core.Add();
			HolyRounds.Init();
			Meltdown.Init();
			FrozenHeart.Init();
			Fury.Init();
			//HeartBurn.Init();
			EmergencySupplies.Init();
			Longshot.Init();
			Frostfire.Add();
			BuildAGun.Add();
			MirrorGuon.Init();
			Handgun.Add();
			BrokenDuplicator.Init();
			ElementalForce.Add();
			SpookyBullets.Init();
			TrashBag.Init();
			CrystalDie.Init();
			GoldChainz.Init();
			VoidBullets.Init();
			Willo.Init();
			Penguin.Init();
			TableTechLife.Init();
			PureFlower.Init();
			CC.Init();
			PotionOfGunSpeed.Init();
			MiniMuncher.Init();
			Smelter.Init();
			TableTechLife2.Init();
			Penguin2.Init();
			SlimeAmmolet.Init();
			CripplingAmmolet.Init();
			PocketPistol.Init();
			SpentSack.Init();
			SuperShroom.Init();
			Balloon.Init();
			BalloonCompanion.Init();
			BeastlyPistol.Add();
			PerfectRound.Init();
			CorruptedRifle.Add();
			Graviton.Add();
			FrozenArmor.Init();
			VoidChest.Init();
			Blamulet.Init();
			IBuy.Init();
			Squire.Init();
			ToyChest.Init();
			Orbiter.Add();
			BloodiedKey.Init();
			GeminiGuonStone.Init();
			LockedArmor.Init();
			Headband.Init();
			TableTechShame.Init();
			RNGun.Add();
			//
			//enemies
			CannonKin.Init();
			Silencer.Init();
			MiniMushboom.Init();
			Salamander.Init();
			Shellet.Init();
			//ReLoad.Init();
			IronBlow.Init();
			Summoner.Init();
			RoomMimic.Init();
			//unlockables
			IceTray.Init();
			Ice.Init();
			BFO.Init();
			TableTechPetrify.Init();
			Icepick.Add();
			Hotshot.Add();
			ParadoxUnlockItem.Init();
			RatDieVirus.Init();
			BulletSponge2.Init();
			BabyGoodCannonKin.Init();
			DemonDeals.Init();
			RemoteDetonator.Add();
		    BiggunronSword.Add();
			TestItem.Init();
			HeartBurn2.Init();
			//unlocks
			string a = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_ROBOT) ? null : "-Defeat The Lich As Robot\n";
			string b = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_WANDERER) ? null : "-Defeat The Dragun As Either Wanderer\n";
			string c = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.RAT_KILLED_AND_WANDERER) ? null : "-Defeat The Rat As Either Wanderer\n";
			string d = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.CHALLENGE_MODE_AND_WANDERER) ? null : "-Beat Challenge Mode As Either Wanderer\n";
			string x = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BOSS_RUSH_AND_WANDERER) ? null : "-Beat Boss Rush As Either Wanderer\n";
			string f = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BLESSED_AND_WANDERER) ? null : "-Beat Blessed Mode As Either Wanderer\n";
			string g = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BLESSED_AND_WANDERER) ? null : "-Defeat The Advanced Dragun As Either Wanderer\n";
			string h = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_PILOT) ? null : "-Defeat The Lich As Pilot\n";
			string z = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_MARINE) ? null : "-Defeat The Lich As Marine\n";
			string j = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_HUNTER) ? null : "-Defeat The Lich As Hunter\n";
			string k = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_PARADOX) ? null : "-Defeat The Lich As Paradox\n";
			string l = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_BULLET) ? null : "-Defeat The Lich As Bullet\n";
			string m = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_CONVICT) ? null : "-Defeat The Lich As Convict\n";
			int num = UnityEngine.Random.Range(0, 2);
			string color1 = num == 1 ? "33F0FF" : "FFAF33";
			Tools.Print("Frost And Gunfire enabled. Enjoy the 1.0 Update!", color1);
			Tools.PrintNoID("TODO List:\n" + b + c + d + x + f + g + h + z + j + k + l + m + a, color1);
			//
			ETGModConsole.Commands.AddUnit("roomname", (args) =>
			{
				var room = GameManager.Instance.PrimaryPlayer.CurrentRoom;
				Tools.Print(room.GetRoomName());
			});
			Hook hook = new Hook(typeof(StringTableManager).GetMethod("GetSynergyString", BindingFlags.Static | BindingFlags.Public), typeof(FrostandGunFireItems).GetMethod("SynergyStringHook"));
			foreach (AdvancedSynergyEntry advancedSynergyEntry in GameManager.Instance.SynergyManager.synergies)
			{
				bool flag = advancedSynergyEntry.NameKey == "#EMERGENCYHELP";
				if (flag)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Emergency Supplies"].PickupObjectId); //Good
					bool flag2 = advancedSynergyEntry.MandatoryItemIDs.Contains(63);
					if (flag2)
					{
						advancedSynergyEntry.MandatoryItemIDs.Remove(63);
					}
					bool flag3 = !advancedSynergyEntry.OptionalItemIDs.Contains(63);
					if (flag3)
					{
						advancedSynergyEntry.OptionalItemIDs.Add(63);
					}
				}

				bool flag17 = advancedSynergyEntry.NameKey == "#MIRRORSHIELD"; //Good
				if (flag17)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Mirror Guon Stone"].PickupObjectId);
					bool flag10 = advancedSynergyEntry.MandatoryItemIDs.Contains(190);
					if (flag10)
					{
						advancedSynergyEntry.MandatoryItemIDs.Remove(190);
					}
					bool flag11 = !advancedSynergyEntry.OptionalItemIDs.Contains(190);
					if (flag11)
					{
						advancedSynergyEntry.OptionalItemIDs.Add(190);
					}
				}


				bool flag21 = advancedSynergyEntry.NameKey == "#RELODESTAR";//Good
				if (flag21)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Afflicted Ammolet"].PickupObjectId);
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Slime Ammolet"].PickupObjectId);
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Rusty Ammolet"].PickupObjectId);
				}

				

				bool flag13 = advancedSynergyEntry.NameKey == "#ICETOMEETYOU";
				if (flag13)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Ice Tray"].PickupObjectId); //Good

				}

			

				bool flag15 = advancedSynergyEntry.NameKey == "#MASSIVEEFFECT";
				if (flag15)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Super Shroom"].PickupObjectId);//Good

				}

				bool flag18 = advancedSynergyEntry.NameKey == "#PAPERWORK"; //Good
				if (flag18)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Table Tech Petrify"].PickupObjectId);
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Table Tech Life"].PickupObjectId);
				}

				bool flag19 = advancedSynergyEntry.NameKey == "#PEACANNON";///Good
				if (flag19)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Super Shroom"].PickupObjectId);
				}
				bool flag20 = advancedSynergyEntry.NameKey == "#PANDEMICPISTOL";///Good
				if (flag20)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["RATDIE Virus"].PickupObjectId);
				}

			}


			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
			{
					new SynergyStuff.SmelterSynergy()
			}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
			{
					new SynergyStuff.DevilDealSynergy()
			}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
			{
					new SynergyStuff.RatDieVirusSynergy()
			}).ToArray<AdvancedSynergyEntry>();


			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
				{
						new SynergyStuff.HolyRoundsSynergy()
				}).ToArray<AdvancedSynergyEntry>();

			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
				{
					new SynergyStuff.TableTechLifeSynergy()
				}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
					{
					new SynergyStuff.PocketPistolSynergy()
					}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
				{
					new SynergyStuff.PocketPistolSynergy2()
				}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
			{
					new SynergyStuff.SpentSackSynergy()
			}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
		{
					new SynergyStuff.CoreSynergy()
		}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
		{
					new SynergyStuff.DrawnSynergy()
		}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
	{
					new SynergyStuff.VoidBulletsSynergy()
	}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.TrashBagSynergy()
}).ToArray<AdvancedSynergyEntry>();
	
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
				
					new SynergyStuff.SlimeAmmoletSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.RNGUNSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.PureFlowerSynergy()
}).ToArray<AdvancedSynergyEntry>();
		
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
		{
					new SynergyStuff.GoldChainzSynergy()
		}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
	{
					new SynergyStuff.FlamethrowerSynergy()
	}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BeastlyPistolSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.LongshotSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.IceTraySynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.IceTraySynergy2()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.SuperShroomSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.RageRifleSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BalloonSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BalloonSynergy2()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BFOSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BlamuletSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyAlpha()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyBees()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyBig()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyBlade()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyBouncy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyChaos()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyCharm()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyCursed()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyDevolved()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyFrost()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyGilded()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyHeat()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyHeavy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyMagic()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyOmega()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyOrbit()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyPlus()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyPoison()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyRocket()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyShadow()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyShock()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergySnow()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergySpooky()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.BuildAGunSynergyZombie()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.PenguinSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.OrbiterSynergy()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.OrbiterSynergy2()
}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
					new SynergyStuff.LockedArmorSynergy()
}).ToArray<AdvancedSynergyEntry>();









			ModLogo = ResourceExtractor.GetTextureFromResource("FrostAndGunfireItems/Resources/logo2.png");
			try
			{
				MainMenuFoyerUpdateHook = new Hook(
					typeof(MainMenuFoyerController).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance),
					typeof(FrostandGunFireItems).GetMethod("MainMenuUpdateHook", BindingFlags.NonPublic | BindingFlags.Instance),
					typeof(MainMenuFoyerController)
					
				);
			}
			catch (Exception ex)
			{

				Debug.LogException(ex);
				return;
			}



















		}

		
	
		public static string SynergyStringHook(Func<string, int, string> action, string key, int index = -1)
		{
			string text = action(key, index);
			bool flag = string.IsNullOrEmpty(text);
			if (flag)
			{
				text = key;
			}
			return text;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000961F File Offset: 0x0000781F
		public override void Exit()
		{
			//private void MainMenuUpdateHook(Action<MainMenuFoyerController> orig, MainMenuFoyerController self)
			//{
			//	orig(self);
			//	if (((dfTextureSprite)self.TitleCard).Texture.name != ModLogo.name)
			//	{
			//		((dfTextureSprite)self.TitleCard).Texture = ModLogo;
			//		LogoEnabled = true;
			//	}
			//}


		}


	}
}
