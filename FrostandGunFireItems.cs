using System;
using System.Linq;
using System.Reflection;
using GungeonAPI;
using ItemAPI;
using MonoMod.RuntimeDetour;
using BepInEx;
using UnityEngine;
using System.IO;
using Dungeonator;
using FrostAndGunfireItems;
using Alexandria.CharacterAPI;

namespace FrostAndGunfireItems
{
	[BepInDependency("etgmodding.etg.mtgapi")]
	[BepInDependency("alexandria.etgmod.alexandria")]
	[BepInPlugin("kp.etg.frostandgunfire", "Frost and Gunfire", "1.0.0")]
	public class FrostandGunFireItems : BaseUnityPlugin
	{

		public void Awake()
		{
			SaveAPIManager.Setup("kp");
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0003DE4C File Offset: 0x0003C04C
		private void MainMenuUpdateHook(Action<MainMenuFoyerController> orig, MainMenuFoyerController self)
		{
			orig(self);
			if (((dfTextureSprite)self.TitleCard).Texture.name != FrostandGunFireItems.ModLogo.name)
			{
				((dfTextureSprite)self.TitleCard).Texture = FrostandGunFireItems.ModLogo;
				FrostandGunFireItems.LogoEnabled = true;
			}
		}

		public void GMStart(GameManager gameManager)
		{
			//CharacterFilePath = this.FolderPath() + "/characters/";
			ETGMod.Assets.SetupSpritesFromFolder(Path.Combine(this.FolderPath(), "sprites"));
			FilePath = this.FolderPath() + "/rooms";
			Strings = new AdvancedStringDB();
			
			//GameStatsManager.Instance.SetCharacterSpecificFlag(ETGModCompatibility.ExtendEnum<PlayableCharacters>("kp.etg.frostandgunfire", "Wanderer"), CharacterSpecificGungeonFlags.KILLED_PAST, true);
			try
			{
				GungeonAP.Init();
				WanderSkinSwap.Add();
				FAGFShrine.Add();
				ChanceShrine.Add();
				ChampionShrine.Add();		
				TableNPC.Add();
				SuperDuperSecretThing.Add();
				ETGModConsole.Commands.AddUnit("iceflow", delegate (string[] args)
				{
					DungeonHandler.debugFlow = !DungeonHandler.debugFlow;
					string str = DungeonHandler.debugFlow ? "enabled" : "disabled";
					string color2 = DungeonHandler.debugFlow ? "00FF00" : "FF0000";
					Tools.Print<string>("Debug flow " + str, color2, false);
				});
				ETGModConsole.Commands.AddUnit("showpos", delegate (string[] args)
				{
					ETGModConsole.Log("Player position: " + GameManager.Instance.PrimaryPlayer.transform.position.ToString(), false);
					ETGModConsole.Log("Player center: " + GameManager.Instance.PrimaryPlayer.sprite.WorldCenter.ToString(), false);
				});

			}
			catch (Exception e)
			{
				Tools.Print<string>("Failed to load shrineAPI", "FF0000", true);
				Tools.PrintException(e, "FF0000");
			}
			ItemBuilder.Init();
			ItemAPI.Hooks.Init();
			EnemyTools.Init();
			BigSniperRifle.Add();
			AfflictedAmmolet.Init();
			Flamethrower.Add();
			Drawn.Add();
			//Core.Add();
			HolyRounds.Init();
			Meltdown.Init();
			FrozenHeart.Init();
			Fury.Init();
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
			LockedArmor.Init();
			TableTechShame.Init(); 
			 RNGun.Add();
			Sack.Init();
			Greed.Init();
			Barter.Init();
			SwindlersShotgun.Add();
			Pouch.Init();
			BulletSponge2.Init();
			//SlimeyGuonStone.Init();
			//GeminiGuonStone.Init();
			//Bellet.Add();
			//Gooper.Add();
			//Gooper2.Add();
			//Judge.Add();
			//Eraser.Add();




			BurstBullets.Init();
			ClusterShot.Init();
			//DualWields.AddDualWields();
			//SynergyForms.AddSynergies();
			IceTray.Init();
			Ice.Init();
			BFO.Init();
			TableTechPetrify.Init();
			Icepick.Add();
			Hotshot.Add();
			ParadoxUnlockItem.Init();
			RatDieVirus.Init();
			BabyGoodCannonKin.Init();
			DemonDeals.Init();
			RemoteDetonator.Add();
			BiggunronSword.Add();
			HeartBurn2.Init();
			//PlantCompanionItem.Init();
		
			//Loader.Init();
			//CharacterHooks.Init();
			//CharacterSwitcher.Init();
			//CharApi.Init("kp");
			
			RoomMimic.Init();
			//Milton.Init();
			CannonKin.Init();
			Silencer.Init();
			MiniMushboom.Init();
			Salamander.Init();
			Ophaims.Init();
			Spitfire.Init();
			Spitter.Init();
			Mushboom.Init();
			Firefly.Init();
			Bubbulon.BuildPrefab();
			Globbulon.BuildPrefab();
			Gazer.BuildPrefab();
			Observer.BuildPrefab();
			//Screecher.BuildPrefab();
			SuppressorShroom.BuildPrefab();
			Humphrey.Init();
			Shellet.Init();
			//Spike.Init();
			Loader.BuildCharacter("FrostAndGunfireItems/Resources/characters/Wanderer", "Wanderer", new Vector3(13.1f, 23.4f, 23.9f), true, new Vector3(15.1f, 23.4f, 24f), false, true, false, true);
			Loader.BuildCharacter("FrostAndGunfireItems/Resources/characters/w", "Wanderer_Fire", new Vector3(100000f, 1000000f, 100000f), true, new Vector3(100000f, 100000f, 100000f), false, true, false, true);
			Loader.BuildCharacter("FrostAndGunfireItems/Resources/characters/Swindler", "Swindler_(Beta)", new Vector3(23.5f, 28.4f, 26.9f), false, new Vector3(100000f, 100000f, 100000f), true, false, false, true);











			//SuperDuperSecretThing.Add();
			string text = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_ROBOT) ? null : "-Defeat The Lich As Robot\n";
			string text2 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_WANDERER) ? null : "-Defeat The Dragun As Either Wanderer\n";
			string text3 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.RAT_KILLED_AND_WANDERER) ? null : "-Defeat The Rat As Either Wanderer\n";
			string text4 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.CHALLENGE_MODE_AND_WANDERER) ? null : "-Beat Challenge Mode As Either Wanderer\n";
			//string text5 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BOSS_RUSH_AND_WANDERER) ? null : "-Beat Boss Rush As Either Wanderer\n";
			string text6 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BLESSED_AND_WANDERER) ? null : "-Beat Blessed Mode As Either Wanderer\n";
			string text7 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BLESSED_AND_WANDERER) ? null : "-Defeat The Advanced Dragun As Either Wanderer\n";
			string text8 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_PILOT) ? null : "-Defeat The Lich As Pilot\n";
			string text9 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_MARINE) ? null : "-Defeat The Lich As Marine\n";
			string text10 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_HUNTER) ? null : "-Defeat The Lich As Hunter\n";
			string text11 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_PARADOX) ? null : "-Defeat The Lich As Paradox\n";
			//string text12 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_BULLET) ? null : "-Defeat The Lich As Bullet\n";
			string text13 = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LICH_KILLED_AND_CONVICT) ? null : "-Defeat The Lich As Convict\n";
			int num = UnityEngine.Random.Range(0, 2);
			string color = (num == 1) ? "33F0FF" : "FFAF33";
			Tools.Print<string>("Frost And Gunfire enabled. Enjoy the Bepinex Update!", color, false);
			ETGModConsole.Log("Is this shit even working????");
			Tools.PrintNoID<string>(string.Concat(new string[]
			{
				"TODO List:\n",
				text2,
				text3,
				text4,
				//text5,
				text6,
				text7,
				text8,
				text9,
				text10,
				text11,
				//text12,
				text13,
				text
			}), color, false);
			ETGModConsole.Commands.AddUnit("roomname", delegate (string[] args)
			{
				RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
				Tools.Print<string>(currentRoom.GetRoomName(), "FFFFFF", false);
			});	
			ETGModConsole.Commands.AddUnit("kp:hidehitboxes", (args) => HitboxMonitor.DeleteHitboxDisplays());
			ETGModConsole.Commands.AddUnit("kp:showhitboxes", (args) =>
			{
				foreach (var obj in GameObject.FindObjectsOfType<SpeculativeRigidbody>())
				{
					HitboxMonitor.DisplayHitbox(obj);
				}
			});
		
			
			Hook hook = new Hook(typeof(StringTableManager).GetMethod("GetSynergyString", BindingFlags.Static | BindingFlags.Public), typeof(FrostandGunFireItems).GetMethod("SynergyStringHook"));
			foreach (AdvancedSynergyEntry advancedSynergyEntry in GameManager.Instance.SynergyManager.synergies)
			{
				bool flag = advancedSynergyEntry.NameKey == "#EMERGENCYHELP";
				if (flag)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Emergency Supplies"].PickupObjectId);
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
				bool flag4 = advancedSynergyEntry.NameKey == "#MIRRORSHIELD";
				if (flag4)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Mirror Guon Stone"].PickupObjectId);
					bool flag5 = advancedSynergyEntry.MandatoryItemIDs.Contains(190);
					if (flag5)
					{
						advancedSynergyEntry.MandatoryItemIDs.Remove(190);
					}
					bool flag6 = !advancedSynergyEntry.OptionalItemIDs.Contains(190);
					if (flag6)
					{
						advancedSynergyEntry.OptionalItemIDs.Add(190);
					}
				}
				bool flag7 = advancedSynergyEntry.NameKey == "#RELODESTAR";
				if (flag7)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Afflicted Ammolet"].PickupObjectId);
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Slime Ammolet"].PickupObjectId);
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Rusty Ammolet"].PickupObjectId);
				}
				bool flag8 = advancedSynergyEntry.NameKey == "#ICETOMEETYOU";
				if (flag8)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Ice Tray"].PickupObjectId);
				}
				bool flag9 = advancedSynergyEntry.NameKey == "#MASSIVEEFFECT";
				if (flag9)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Super Shroom"].PickupObjectId);
				}
				bool flag10 = advancedSynergyEntry.NameKey == "#PAPERWORK";
				if (flag10)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Table Tech Petrify"].PickupObjectId);
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Table Tech Life"].PickupObjectId);
				}
				bool flag11 = advancedSynergyEntry.NameKey == "#PEACANNON";
				if (flag11)
				{
					advancedSynergyEntry.OptionalItemIDs.Add(ETGMod.Databases.Items["Super Shroom"].PickupObjectId);
				}
				bool flag12 = advancedSynergyEntry.NameKey == "#PANDEMICPISTOL";
				if (flag12)
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
			/*	new SynergyStuff.CoreSynergy()
			}).ToArray<AdvancedSynergyEntry>();
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
			{*/
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
			GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
			{
				new SynergyStuff.BulletSpongeSynergy()
			}).ToArray<AdvancedSynergyEntry>();
			FrostandGunFireItems.ModLogo = ResourceExtractor.GetTextureFromResource("FrostAndGunfireItems/Resources/logo2.png");
			try
			{
				FrostandGunFireItems.MainMenuFoyerUpdateHook = new Hook(typeof(MainMenuFoyerController).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic), typeof(FrostandGunFireItems).GetMethod("MainMenuUpdateHook", BindingFlags.Instance | BindingFlags.NonPublic), typeof(MainMenuFoyerController));
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
		public void Start()
		{
			ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
			ETGMod.Assets.SetupSpritesFromFolder(Path.Combine(this.FolderPath(), "sprites"));
		}


		// Token: 0x060007D4 RID: 2004 RVA: 0x000108FC File Offset: 0x0000EAFC
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

		// Token: 0x0400047C RID: 1148
		public static Texture2D ModLogo;

		// Token: 0x0400047D RID: 1149
		public static Hook MainMenuFoyerUpdateHook;

		// Token: 0x0400047E RID: 1150
		public static bool LogoEnabled;

		// Token: 0x0400047F RID: 1151
		public static GameObject shrine;

		// Token: 0x04000480 RID: 1152
		public static AdvancedStringDB Strings;

		// Token: 0x04000481 RID: 1153
		public static string ZipFilePath;

		public static string CharacterFilePath;

		// Token: 0x04000482 RID: 1154
		public static string FilePath;
	}
}
