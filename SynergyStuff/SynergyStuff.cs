using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrostAndGunfireItems
{
	internal class SynergyStuff
	{
		// Token: 0x02000006 RID: 6
		public class SmelterSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public SmelterSynergy()
			{
				this.NameKey = "Blast Powered";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Smelter"].PickupObjectId,
					564
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class IceTraySynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public IceTraySynergy()
			{
				this.NameKey = "Ice Is Nice";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Ice Tray"].PickupObjectId,
					170
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class RatDieVirusSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public RatDieVirusSynergy()
			{
				this.NameKey = "Moldy Cheese";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["RATDIE Virus"].PickupObjectId,
				};
				this.OptionalItemIDs = new List<int>
				{
					663,
					662,
					667,
				};
				this.OptionalGunIDs = new List<int>
				{
					626
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();

			}
		}


		public class IceTraySynergy2 : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public IceTraySynergy2()
			{
				this.NameKey = "Refilled Trays";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Ice Tray"].PickupObjectId,
				};
				this.MandatoryGunIDs = new List<int>
				{
					130
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BalloonSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public BalloonSynergy()
			{
				this.NameKey = "What's Up?";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Blue Balloon"].PickupObjectId,
				};
				this.MandatoryGunIDs = new List<int>
				{
					520
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BalloonSynergy2 : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public BalloonSynergy2()
			{
				this.NameKey = "Happy Blankday";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Blue Balloon"].PickupObjectId,
					ETGMod.Databases.Items["Toy Chest"].PickupObjectId,
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BFOSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public BFOSynergy()
			{
				this.NameKey = "My New Favorite Toy";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["B.F.O"].PickupObjectId,
					ETGMod.Databases.Items["Toy Chest"].PickupObjectId,
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		
		public class RageRifleSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public RageRifleSynergy()
			{
				this.NameKey = "I HATE THIS GAME!!!!";
				this.MandatoryItemIDs = new List<int>
				{
					323
				};
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Rage Rifle").PickupObjectId
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		

		public class SlimeAmmoletSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public SlimeAmmoletSynergy()
			{
				this.NameKey = "Adhesive";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Slime Ammolet"].PickupObjectId,
					205
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class GoldChainzSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public GoldChainzSynergy()
			{
				this.NameKey = "Throne Butt";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Gold Chainz"].PickupObjectId,
					204
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

	

		public class RNGUNSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public RNGUNSynergy()
			{
				this.NameKey = "R.N.GOD";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Crystal Die"].PickupObjectId,


				};
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("R.N.GUN").PickupObjectId
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}



		public class FlamethrowerSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public FlamethrowerSynergy()
			{
				this.NameKey = "MMMMMM MMMMMM!!!";
				this.MandatoryItemIDs = new List<int>
				{
					453
				};
				this.MandatoryGunIDs = new List<int>
				{
					
						PickupObjectDatabase.GetByEncounterName("Flamethrower").PickupObjectId
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}


		public class BeastlyPistolSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public BeastlyPistolSynergy()
			{
				this.NameKey = "Bloodlust";
				this.OptionalItemIDs = new List<int>
				{
					285,
					313,
					524,
					167
				};
				this.MandatoryGunIDs = new List<int>
				{
					
						PickupObjectDatabase.GetByEncounterName("Beastly Pistol").PickupObjectId
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
	

		public class PureFlowerSynergy : AdvancedSynergyEntry
		{
			// Token: 0x06000009 RID: 9 RVA: 0x0000268C File Offset: 0x0000088C
			public PureFlowerSynergy()
			{
				this.NameKey = "Afterblam";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Pure Flower"].PickupObjectId,


				};
				this.MandatoryGunIDs = new List<int>
				{
				   33
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

	
		public class TableTechLifeSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public TableTechLifeSynergy()
			{
				this.NameKey = "Devolved";
				this.MandatoryItemIDs = new List<int>
				{
					 ETGMod.Databases.Items["Table Tech Life"].PickupObjectId,


				};
				this.OptionalItemIDs = new List<int>
				{
					 638,
					  293,

				};
				this.OptionalGunIDs = new List<int>
				{
					 484,
					

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BlamuletSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BlamuletSynergy()
			{
				this.NameKey = "Big Blam";
				this.MandatoryItemIDs = new List<int>
				{
					 ETGMod.Databases.Items["Blamulet"].PickupObjectId,


				};
				this.OptionalItemIDs = new List<int>
				{
					108,
					109

				};
				this.OptionalGunIDs = new List<int>
				{
					 332


				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}


		public class LongshotSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public LongshotSynergy()
			{
				this.NameKey = "Quick Scope";
				this.MandatoryItemIDs = new List<int>
				{
					 ETGMod.Databases.Items["Longshot"].PickupObjectId,


				};
				this.OptionalGunIDs = new List<int>
				{
					 49,
					 5,
					 385,
					 25,


				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class DevilDealSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public DevilDealSynergy()
			{
				this.NameKey = "Hot Sales";
				this.MandatoryItemIDs = new List<int>
				{
					 ETGMod.Databases.Items["Devil's Discount"].PickupObjectId,


				};
				this.MandatoryGunIDs = new List<int>
				{
					336

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>();
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}




		public class PocketPistolSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public PocketPistolSynergy()
			{
				this.NameKey = "Pocket Shotgun";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Pocket Pistol"].PickupObjectId,
			
				};
				this.MandatoryGunIDs = new List<int>
				{
					601

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class PocketPistolSynergy2 : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public PocketPistolSynergy2()
			{
				this.NameKey = "Pretty Boy";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Pocket Pistol"].PickupObjectId,

				};
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Elemental Force").PickupObjectId

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		//public class CursedCynlinderSynergy : AdvancedSynergyEntry
		///{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			///public CursedCynlinderSynergy()
			///{
			///	this.NameKey = "3spooky5me";
				///this.MandatoryItemIDs = new List<int>
				//{
				//	ETGMod.Databases.Items["Afflicted Ammolet"].PickupObjectId,
				//	ETGMod.Databases.Items["Cursed Cynlinder"].PickupObjectId

				//};
			
				//this.IgnoreLichEyeBullets = true;
				//this.statModifiers = new List<StatModifier>(0);
				//this.bonusSynergies = new List<CustomSynergyType>();
			//}
		//}
		public class SpentSackSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public SpentSackSynergy()
			{
				this.NameKey = "Sack Pack";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Spent Sack"].PickupObjectId,
					133
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}




		public class OrbiterSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public OrbiterSynergy()
			{
				this.NameKey = "Orbital Velocity";
				this.MandatoryItemIDs = new List<int>
				{
					661

				};
				this.MandatoryGunIDs = new List<int>
				{
						PickupObjectDatabase.GetByEncounterName("Orbiter").PickupObjectId
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class OrbiterSynergy2 : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public OrbiterSynergy2()
			{
				this.NameKey = "Around The World";
				this.MandatoryGunIDs = new List<int>
				{
				PickupObjectDatabase.GetByEncounterName("Orbiter").PickupObjectId,
				597
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class CoreSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public CoreSynergy()
			{
				this.NameKey = "Fly Me To The Sun";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("The Core").PickupObjectId

				};
				this.OptionalItemIDs = new List<int>
				{
					398,
					113

				};
				this.OptionalGunIDs = new List<int>
				{
					372

				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class LockedArmorSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public LockedArmorSynergy()
			{
				this.NameKey = "Unsealed";
				this.MandatoryGunIDs = new List<int>
				{
					ETGMod.Databases.Items["Locked Armor"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					166


				};
				this.OptionalGunIDs = new List<int>
				{
					95

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class PenguinSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public PenguinSynergy()
			{
				this.NameKey = "Happy Sleet";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Penguin"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					636,
			    

				};
				this.OptionalGunIDs = new List<int>
				{
					402

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class SuperShroomSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600036F RID: 879 RVA: 0x0001CEC8 File Offset: 0x0001B0C8
			public SuperShroomSynergy()
			{
				this.NameKey = "Let's A Go!";
				this.MandatoryItemIDs = new List<int>
				{
					global::ETGMod.Databases.Items["Super Shroom"].PickupObjectId
				};
				this.MandatoryGunIDs = new List<int>
				{
					376
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class DrawnSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public DrawnSynergy()
			{
				this.NameKey = "Pen To Paper";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Drawn").PickupObjectId,
					477

				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}


	
		public class HolyRoundsSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public HolyRoundsSynergy()
			{
				this.NameKey = "Blursed Bullets";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Holy Rounds"].PickupObjectId,
					571
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class TrashBagSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public TrashBagSynergy()
			{
				this.NameKey = "One Man's Treasure";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Trash Bag"].PickupObjectId,
					641
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class VoidBulletsSynergy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public VoidBulletsSynergy()
			{
				this.NameKey = "Inspiral";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Void Bullets"].PickupObjectId,
				};
				this.OptionalItemIDs = new List<int>
				{
					169,
					155

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyPlus : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyPlus()
			{
				this.NameKey = "Build Modifier: +1";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					286
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class BuildAGunSynergyRocket : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyRocket()
			{
				this.NameKey = "Build Modifier: Rocket";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.OptionalItemIDs = new List<int>
				{
					113,
					284

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyHeavy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyHeavy()
			{
				this.NameKey = "Build Modifier: Heavy";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					111

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class BuildAGunSynergyShock: AdvancedSynergyEntry //broke
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyShock()
			{
				this.NameKey = "Build Modifier: Shock";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					298

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyBouncy : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyBouncy()
			{
				this.NameKey = "Build Modifier: Bouncy";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					288

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergySpooky : AdvancedSynergyEntry //synergy doesn't show up
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergySpooky()
			{
				this.NameKey = "Build Modifier: Spooky";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					172

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class BuildAGunSynergyAlpha : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyAlpha()
			{
				this.NameKey = "Build Modifier: Alpha";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					373

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class BuildAGunSynergyOmega : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyOmega()
			{
				this.NameKey = "Build Modifier: Omega";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					374

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class BuildAGunSynergyPoison : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyPoison()
			{
				this.NameKey = "Build Modifier: Poison";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					204

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyHeat : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyHeat()
			{
				this.NameKey = "Build Modifier: Heat";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					295

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyFrost : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyFrost()
			{
				this.NameKey = "Build Modifier: Frost";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					278

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyBig : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyBig()
			{
				this.NameKey = "Build Modifier: Big";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.OptionalItemIDs = new List<int>
				{
					277,
					523

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyShadow : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyShadow()
			{
				this.NameKey = "Build Modifier: Shadow";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					352

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyChaos : AdvancedSynergyEntry //broke
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyChaos()
			{
				this.NameKey = "Build Modifier: Chaos";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.OptionalItemIDs = new List<int>
				{
					521,
					569

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyCharm : AdvancedSynergyEntry //broke
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyCharm()
			{
				this.NameKey = "Build Modifier: Charm";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					527

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyCursed : AdvancedSynergyEntry 
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyCursed()
			{
				this.NameKey = "Build Modifier: Cursed";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					571

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyGilded : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyGilded()
			{
				this.NameKey = "Build Modifier: Gilded";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					532

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyMagic : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyMagic()
			{
				this.NameKey = "Build Modifier: Magic";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					533

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyZombie : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyZombie()
			{
				this.NameKey = "Build Modifier: Zombie";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					528

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class BuildAGunSynergyBees : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyBees()
			{
				this.NameKey = "Build Modifier: Bees";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					630

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class BuildAGunSynergyDevolved : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyDevolved()
			{
				this.NameKey = "Build Modifier: Devolved";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					638

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergySnow : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergySnow()
			{
				this.NameKey = "Build Modifier: Snow";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					636

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyBlade : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyBlade()
			{
				this.NameKey = "Build Modifier: Blade";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.OptionalItemIDs = new List<int>
				{
					640,
					822

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class BuildAGunSynergyOrbit : AdvancedSynergyEntry
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
			public BuildAGunSynergyOrbit()
			{
				this.NameKey = "Build Modifier: Orbit";
				this.MandatoryGunIDs = new List<int>
				{
					PickupObjectDatabase.GetByEncounterName("Build-A-Gun").PickupObjectId

				};
				this.MandatoryItemIDs = new List<int>
				{
					661

				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

	}
}
