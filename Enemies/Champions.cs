using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dungeonator;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{

	public class Champions : ETGModule
	{

		public bool DepixelatesTargets = true;
		public Vector2 TargetScale = new Vector2(1.5f, 1.5f);
		public Vector2 TargetScale2 = new Vector2(0.8f, 0.8f);
		public static bool ChampionsOn;
		public static bool AllChampions;
		bool disabled = false;


		public override void Exit()
		{
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000A39C File Offset: 0x0000859C
		public override void Start()
		{
			try
			{
				this.CreateOrLoadConfiguration();
			}
			catch (Exception e)
			{
				Tools.PrintException(e, "FF0000");
			}
		}

		private void CreateOrLoadConfiguration()
		{
			bool flag = !File.Exists(SaveFilePath);
			if (flag)
			{
				global::ETGModConsole.Log("", false);
				Directory.CreateDirectory(ConfigDirectory);
				File.Create(SaveFilePath).Close();
				UpdateConfiguration();
			}
			else
			{
				string text = File.ReadAllText(SaveFilePath);
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					File.WriteAllText(SaveFilePath, EnableChamps);
				}
				else
				{
					this.UpdateConfiguration();
				}
			}
		}
		public void UpdateConfiguration()
		{
			bool flag = !File.Exists(SaveFilePath);
			if (flag)
			{
				global::ETGModConsole.Log("", false);
				Directory.CreateDirectory(ConfigDirectory);
				File.Create(SaveFilePath).Close();
			}
			File.WriteAllText(SaveFilePath, EnableChamps);

		}
		private static string ConfigDirectory = Path.Combine(global::ETGMod.ResourcesDirectory, "fagfconfig");
		private static string SaveFilePath = Path.Combine(ConfigDirectory, "togglechamps.json");
		private static string EnableChamps = ChampionsOn ? "false" : "true";
		public override void Init()
		{
			if (File.Exists(SaveFilePath))
			{
				string[] lines = File.ReadAllLines(SaveFilePath);
				if (lines.Contains("true"))
				{
					ChampionsOn = true;

				}
				else
				{
					ChampionsOn = false;

				}
			}
			else
			{
				ChampionsOn = true;

			}
			global::ETGModConsole.Commands.AddGroup("kp", delegate (string[] args)
			{
				global::ETGModConsole.Log("Please specify a command.", false);
			});
			global::ETGModConsole.Commands.GetGroup("kp").AddUnit("gilbert", delegate (string[] args)
			{
				PlayerController player = GameManager.Instance.PrimaryPlayer;
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("gilbert");
				Vector3 vector = player.sprite.WorldTopCenter;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);


			});
			global::ETGModConsole.Commands.GetGroup("kp").AddUnit("unlockall", delegate (string[] args)
			{
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_ROBOT, true); //Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BLESSED_AND_WANDERER, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BOSS_RUSH_AND_WANDERER, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.CHALLENGE_MODE_AND_WANDERER, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_WANDERER, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_BULLET, true);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_CONVICT, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_HUNTER, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_MARINE, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_PARADOX, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_PILOT, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_WANDERER, true);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.RAT_KILLED_AND_WANDERER, true);//Done
			});
			global::ETGModConsole.Commands.GetGroup("kp").AddUnit("lockall", delegate (string[] args)
			{
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_ROBOT, false); //Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BLESSED_AND_WANDERER, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BOSS_RUSH_AND_WANDERER, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.CHALLENGE_MODE_AND_WANDERER, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_WANDERER, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_BULLET, false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_CONVICT, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_HUNTER, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_MARINE, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_PARADOX, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_PILOT, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_WANDERER, false);//Done
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.RAT_KILLED_AND_WANDERER, false);//Done
			});
			global::ETGModConsole.Commands.GetGroup("kp").AddUnit("togglechampions", delegate (string[] args)
			{
				if (ChampionsOn == true)
				{
					File.WriteAllText(SaveFilePath, "false");
					ChampionsOn = false;
					global::ETGModConsole.Log("Champions have been disabled", false);
				}
				else
				{
					ChampionsOn = true;
					File.WriteAllText(SaveFilePath, "true");
					global::ETGModConsole.Log("Champions have been enabled", false);
				}
			});
			global::ETGModConsole.Commands.GetGroup("kp").AddUnit("toggleui", delegate (string[] args)
			{
				if (!disabled)
				{
					Tools.PrintNoID("Ui is disabled");
					//GameUIRoot.Instance.SetAmmoCountColor(Color.red, GameManager.Instance.PrimaryPlayer);
					GameUIRoot.Instance.HideCoreUI("disabled");
					GameUIRoot.Instance.ForceHideGunPanel = true;
					GameUIRoot.Instance.ForceHideItemPanel = true;
					disabled = true;
				}
				else if (disabled)
				{
					Tools.PrintNoID("Ui is enabled");
					GameUIRoot.Instance.ShowCoreUI("disabled");
					GameUIRoot.Instance.ForceHideGunPanel = false;
					GameUIRoot.Instance.ForceHideItemPanel = false;
					disabled = false;
				}
			});
			global::ETGModConsole.Commands.GetGroup("kp").AddUnit("allchampions", delegate (string[] args)
			{
				if (AllChampions == true)
				{
					AllChampions = false;
					ChampionsOn = false;
					global::ETGModConsole.Log("All Champions have been disabled", false);
				}
				else
				{
					AllChampions = true;
					ChampionsOn = true;
					global::ETGModConsole.Log("All Champions have been enabled", false);
				}
			});
			ETGMod.AIActor.OnPostStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPostStart, new Action<AIActor>(MakeChampion));
			ETGMod.AIActor.OnPostStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPostStart, new Action<AIActor>(AssignUnlock));
		}
		private void AssignUnlock(AIActor target)
		{
			PlayableCharacters characterIdentity = GameManager.Instance.PrimaryPlayer.characterIdentity;
			//Lich unlocks
			if (target.EnemyGuid == "7c5d5f09911e49b78ae644d2b50ff3bf")
			{
				target.healthHaver.OnDeath += (obj) =>
				{
					//Wanderer Lich Win
					if((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Fury"].PickupObjectId)) || (GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Meltdown"].PickupObjectId)))
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_WANDERER, true);
					}
					switch (characterIdentity)
					{
						case PlayableCharacters.Robot:
							AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_ROBOT, true);
							break;
						case PlayableCharacters.Pilot:
							AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_PILOT, true);
							break;
						case PlayableCharacters.Guide:
							AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_HUNTER, true);
							break;
						case PlayableCharacters.Eevee:
							AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_PARADOX, true);
							break;
						case PlayableCharacters.Soldier:
							AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.LICH_KILLED_AND_MARINE, true);
							break;
					}
				};
			}
			//Dragun unlock
			if (target.EnemyGuid == "465da2bb086a4a88a803f79fe3a27677")
			{
				target.healthHaver.OnDeath += (obj) =>
				{
					//Dragun Kill
					if ((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Fury"].PickupObjectId)) || (GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Meltdown"].PickupObjectId)))
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_WANDERER, true);
					}
					//Blessed Mode Win
					if ((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Fury"].PickupObjectId)) && GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns || GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns &&(GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Meltdown"].PickupObjectId)))
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BLESSED_AND_WANDERER, true);
					}
					//Boss Rush Win
					if ((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Fury"].PickupObjectId)) && GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH || GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH && (GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Meltdown"].PickupObjectId)))
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BOSS_RUSH_AND_WANDERER, true);
					}
					//Challenge Mod
					if ((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Fury"].PickupObjectId)) && ChallengeManager.Instance == true || ChallengeManager.Instance == true && (GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Meltdown"].PickupObjectId)))
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.CHALLENGE_MODE_AND_WANDERER, true);
					}
				};
			}
			//Rat unlocks
			if (target.EnemyGuid == "4d164ba3f62648809a4a82c90fc22cae")
			{
				target.healthHaver.OnDeath += (obj) =>
				{
					
					if ((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Fury"].PickupObjectId)) || (GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Meltdown"].PickupObjectId)))
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.RAT_KILLED_AND_WANDERER, true);
					}
				};
			}
		}
		public float AddedMasterRoundChance;
		public Color magenta = Color.magenta;
		public Color yellow = Color.yellow;
		public Color cyan = Color.cyan;
		public static List<AIActor> IsChampion = new List<AIActor>();
		private void MakeChampion(AIActor target)
		{
			float value = UnityEngine.Random.value;
			if (ChampionsOn && !target.CompanionOwner && !BannedEnemies.Contains(target.EnemyGuid) && !target.healthHaver.IsBoss)
			{
				if (!AllChampions)
				{
					if ((double)value < 0.03)
					{
						DoChampionStuff(target);
						IsChampion.Add(target);
						EnemyTools.DisableSuperTinting(target);
					}
				}
				else
				{
					DoChampionStuff(target);
					IsChampion.Add(target);
					EnemyTools.DisableSuperTinting(target);
				}
			}
		}

		private void DoChampionStuff(AIActor target)
		{
			
			float dropchance = UnityEngine.Random.value;
			bool flag = target.bulletBank != null && target.healthHaver != null;
			if (flag)
			{
				for (int i = 0; i < target.bulletBank.Bullets.Count; i++)
				{
					target.bulletBank.Bullets[i].ProjectileData.speed *= 1.2f;
				}
			}
			if (!AllChampions && (double)dropchance < 0.05)
			{
				target.healthHaver.OnPreDeath += (obj) =>
				{
					ChampionReward((target.sprite.WorldCenter));
				};
			}
			int num = UnityEngine.Random.Range(0, 11);
			if (num == 0 && !SuperShroom.BannedEnemies.Contains(target.EnemyGuid))
			{
				target.procedurallyOutlined = false;
				Vector2 startScale = target.EnemyScale;
				int cachedLayer = target.gameObject.layer;
				int cachedOutlineLayer = cachedLayer;
				bool depixelatesTargets = this.DepixelatesTargets;
				if (depixelatesTargets)
				{
					target.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
					cachedOutlineLayer = SpriteOutlineManager.ChangeOutlineLayer(target.sprite, LayerMask.NameToLayer("Unpixelated"));
				}
				target.ClearPath();
				bool flag2 = target.knockbackDoer;
				if (flag2)
				{
					target.knockbackDoer.weight *= 1.5f;
					target.MovementSpeed *= 0.75f;
					target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 2f);
				}
				bool depixelatesTargets2 = this.DepixelatesTargets;
				if (depixelatesTargets2)
				{
					target.gameObject.layer = cachedLayer;
					SpriteOutlineManager.ChangeOutlineLayer(target.sprite, cachedOutlineLayer);

				}
				target.EnemyScale = this.TargetScale;

			}
			if (num == 1 && !SuperShroom.BannedEnemies.Contains(target.EnemyGuid))
			{
				target.procedurallyOutlined = false;
				Vector2 startScale = target.EnemyScale;
				int cachedLayer = target.gameObject.layer;
				int cachedOutlineLayer = cachedLayer;
				bool depixelatesTargets = this.DepixelatesTargets;
				if (depixelatesTargets)
				{
					target.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
					cachedOutlineLayer = SpriteOutlineManager.ChangeOutlineLayer(target.sprite, LayerMask.NameToLayer("Unpixelated"));
				}
				target.ClearPath();
				bool flag2 = target.knockbackDoer;
				if (flag2)
				{
					target.knockbackDoer.weight *= 0.5f;
					target.MovementSpeed *= 1.6f;
					target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 1.2f);
				}
				target.EnemyScale = TargetScale2;
				bool depixelatesTargets2 = this.DepixelatesTargets;
				if (depixelatesTargets2)
				{
					target.gameObject.layer = cachedLayer;
					SpriteOutlineManager.ChangeOutlineLayer(target.sprite, cachedOutlineLayer);
				}
	


			}
			if (num == 2 && !NoNo.Contains(target.EnemyGuid))
			{
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 1.5f);
				target.EffectResistances = new ActorEffectResistance[]
				{
					new ActorEffectResistance()
					{
						resistAmount = 1000,
						resistType = EffectResistanceType.Fire
					},
				};
				target.RegisterOverrideColor(new Color(2, 0, 0), "tint");
				target.healthHaver.OnPreDeath += (obj) =>
				{
					DoFireGoop((target.sprite.WorldCenter));
				};

			}
			if (num == 3)
			{
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 1.5f);
				target.EffectResistances = new ActorEffectResistance[]
				{
					new ActorEffectResistance()
					{
						resistAmount = 1000,
						resistType = EffectResistanceType.Poison
					},
				};
				target.RegisterOverrideColor(new Color(0, 2, 0), "tint");
				target.healthHaver.OnPreDeath += (obj) =>
				{
					DoPoisonGoop((target.sprite.WorldCenter));
				};



			}
			if (num == 4 && !NoNo.Contains(target.EnemyGuid))
			{
				//blue
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 1.5f);
				target.RegisterOverrideColor(new Color(0, 0, 2), "tint");
				target.MovementSpeed *= 1.3f;
				target.aiShooter.PostProcessProjectile += PostProcessProjectile;
			}
			if (num == 5)
			{
				//white
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 2f);
				target.RegisterOverrideColor(new Color(2, 2, 2), "tint");

			}

			if (num == 6)
			{
				//magenta
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 1.5f);
				target.RegisterOverrideColor(magenta , "tint");
				target.aiShooter.PostProcessProjectile += JammedBullets;
			}

			if (num == 7)
			{
				//yellow
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 1.5f);
				target.RegisterOverrideColor(yellow, "tint");
			}

			if (num == 8)
			{
				//glitch
				int Health = UnityEngine.Random.Range(1, 2);
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * Health);
				target.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/GlitchUnlit");
				target.sprite.usesOverrideMaterial = true;
				target.sprite.renderer.material.SetFloat("_GlitchInterval", 0.05f);
				target.sprite.renderer.material.SetFloat("_DispProbability", 0.4f);
				target.sprite.renderer.material.SetFloat("_DispIntensity", 0.04f);
				target.sprite.renderer.material.SetFloat("_ColorProbability", 0.4f);
				target.sprite.renderer.material.SetFloat("_ColorIntensity", 0.04f);
				if (target.bulletBank != null && target.healthHaver != null)
				{
					for (int i = 0; i < target.bulletBank.Bullets.Count; i++)
					{
						target.bulletBank.Bullets[i].ProjectileData.speed *= UnityEngine.Random.Range(1f, 1.5f);
					}
				}
			}
			if (num == 9)
			{
				//Purple
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 1.5f);
				target.RegisterOverrideColor(new Color(1f, .192f, 2f), "tint");
				target.healthHaver.OnPreDeath += (obj) =>
				{
					SpawnBlob((target.sprite.WorldCenter));
				};

			}
			if (num == 10)
			{
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 1.5f);
				target.RegisterOverrideColor(cyan, "tint");
				target.healthHaver.OnPreDeath += (obj) =>
				{
					DoIceGoop((target.sprite.WorldCenter));
				};
			}

		}

		public void ChampionReward(Vector2 worldCenter)
		{
			GenericLootTable singleItemRewardTable = GameManager.Instance.RewardManager.CurrentRewardData.SingleItemRewardTable;
			LootEngine.SpawnItem(singleItemRewardTable.SelectByWeight(false), worldCenter, Vector2.up, 1f, true, false, false);
		}

		

		private void JammedBullets(Projectile projectile)
{
	projectile.baseData.speed *= 1.15f;
	projectile.BecomeBlackBullet();

}


		private void PostProcessProjectile(Projectile projectile)
		{
			projectile.baseData.speed *= 1.4f;
		}

		private void SpawnBlob(Vector2 v)
		{
			
			PlayerController player = GameManager.Instance.PrimaryPlayer;
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("42be66373a3d4d89b91a35c9ff8adfec");
			AIActor aiactor = AIActor.Spawn(orLoadByGuid, v, player.gameObject.transform.position.GetAbsoluteRoom(), true, AIActor.AwakenAnimationType.Awaken, true);
			aiactor.MovementSpeed *= 0.8f;
		}
		private void DoIceGoop(Vector2 v)
		{
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/water goop.asset");
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
			goopManagerForGoopType.TimedAddGoopCircle(v, 2.3f, 0.35f, false);
			goopManagerForGoopType.FreezeGoopCircle(v, 50f);

		}
		private void DoPoisonGoop(Vector2 v)
		{
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
			goopManagerForGoopType.TimedAddGoopCircle(v, 2.3f, 0.35f, false);
			goopDef.damagesEnemies = false;
		}

		private void DoFireGoop(Vector2 v)
		{
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
			goopManagerForGoopType.TimedAddGoopCircle(v, 2.3f, 0.35f, false);
			goopDef.damagesEnemies = false;
		}
		public static string[] BannedEnemies = new string[]
			{
			"mini mushboom",
			};
		public static string[] NoNo = new string[]
			{
			 "c0260c286c8d4538a697c5bf24976ccf",
			 "4d37ce3d666b4ddda8039929225b7ede",
			 "3cadf10c489b461f9fb8814abc1a09c1",
			};

	}
}
	

