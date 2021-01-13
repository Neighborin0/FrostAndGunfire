using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;

namespace FrostAndGunfireItems
{
	public class Summoner : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "summoner";
		private static tk2dSpriteCollectionData SummonerCollection;


		public static void Init()
		{

			Summoner.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			AIActor source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("Summoner", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<SummonBehavior>();
				companion.aiActor.MovementSpeed = 4f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(18f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(45f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 15,
					ManualHeight = 17,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
				
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 15,
					ManualHeight = 17,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,
					

				
				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "die",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
							Flipped = new DirectionalAnimation.FlipType[2],
							AnimNames = new string[]
							{
					
					   "die_right",
						   "die_left"
					
							}
							
						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_right",
						"idle_left"
					}
				};
				DirectionalAnimation anim = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"summon",
						
					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "summon",
						anim = anim
					}
				};

				bool flag3 = SummonerCollection == null;
				if (flag3)
				{
					SummonerCollection = SpriteBuilder.ConstructCollection(prefab, "Summoner_Collection");
					UnityEngine.Object.DontDestroyOnLoad(SummonerCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], SummonerCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SummonerCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						
						
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SummonerCollection, new List<int>
					{
						4,
						5,
						6,
						7,

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, SummonerCollection, new List<int>
					{
						8,
						9,
						10,
						11
					

					}, "summon", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 6f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SummonerCollection, new List<int>
					{
						12,
						13,
						14,
						

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, SummonerCollection, new List<int>
					{
						15,
					16,
					17,
					

					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				///bs.MovementBehaviors.Add(new SeekTargetBehavior());
				bs.TargetBehaviors.Add(new TargetPlayerBehavior());
				Game.Enemies.Add("kp:summoner", companion.aiActor);


			}
		}



		private static string[] spritePaths = new string[]
		{
			

			"FrostAndGunfireItems/Resources/summoner/summoner_idle_right_001",
			"FrostAndGunfireItems/Resources/summoner/summoner_idle_right_002",
			"FrostAndGunfireItems/Resources/summoner/summoner_idle_right_003",
			"FrostAndGunfireItems/Resources/summoner/summoner_idle_right_004",
			"FrostAndGunfireItems/Resources/summoner/summoner_idle_left_001",
		   "FrostAndGunfireItems/Resources/summoner/summoner_idle_left_002",
		   "FrostAndGunfireItems/Resources/summoner/summoner_idle_left_003",
			"FrostAndGunfireItems/Resources/summoner/summoner_idle_left_004",
		   "FrostAndGunfireItems/Resources/summoner/summoner_summon_001",
		   "FrostAndGunfireItems/Resources/summoner/summoner_summon_002",
			"FrostAndGunfireItems/Resources/summoner/summoner_summon_003",
			"FrostAndGunfireItems/Resources/summoner/summoner_summon_004",
		   //"FrostAndGunfireItems/Resources/summoner/summoner_hit_right_001",
			//"FrostAndGunfireItems/Resources/summoner/summoner_hit_left_001",
		  "FrostAndGunfireItems/Resources/summoner/summoner_die_left_001",
			 "FrostAndGunfireItems/Resources/summoner/summoner_die_left_002",
			 "FrostAndGunfireItems/Resources/summoner/summoner_die_left_003",
			   "FrostAndGunfireItems/Resources/summoner/summoner_die_right_001",
			  "FrostAndGunfireItems/Resources/summoner/summoner_die_right_002",
				"FrostAndGunfireItems/Resources/summoner/summoner_die_right_003",
				 
				};

	
		public class SummonBehavior : BraveBehaviour
		{
			public string summonAnimation = "summon";
			public static BlinkPassiveItem m_BlinkPassive;
	
			
			private void Start()
			{
			
				GameManager.Instance.StartCoroutine(Summon());
				ReDetermineEnemyPool();
			 base.aiActor.healthHaver.OnPreDeath += (obj) =>
					{
						AkSoundEngine.PostEvent("Play_WPN_Life_Orb_Fade_01", base.aiActor.gameObject);
					};
			  }
			public IEnumerator Summon()
			{
				if (this.aiActor.ParentRoom.IsSealed)
				{

					m_BlinkPassive = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>();
					Vector2 offset = new Vector2(0, 2.3f);
					Vector3 offset1 = new Vector2(0, 2.3f);
					yield return new WaitForSeconds(2f);
					this.aiAnimator.PlayForDuration(summonAnimation, 5.2f, true, null, -1f, false);
					this.aiActor.knockbackDoer.SetImmobile(true, "laugh");
					AkSoundEngine.PostEvent("Play_ENM_jammer_curse_01", base.gameObject);
					this.aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_DBZ_Charge") as GameObject, Vector3.zero - offset1, false, false, false);
					yield return new WaitForSeconds(1f);
					AkSoundEngine.PostEvent("Play_ENM_jammer_curse_01", base.gameObject);
					this.aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_DBZ_Charge") as GameObject, Vector3.zero - offset1, false, false, false);
					yield return new WaitForSeconds(1f);
					AkSoundEngine.PostEvent("Play_ENM_jammer_curse_01", base.gameObject);
					this.aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_DBZ_Charge") as GameObject, Vector3.zero - offset1, false, false, false);
					yield return new WaitForSeconds(1f);
					AkSoundEngine.PostEvent("Play_ENM_jammer_curse_01", base.gameObject);
					this.aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_DBZ_Charge") as GameObject, Vector3.zero - offset1, false, false, false);
					yield return new WaitForSeconds(1f);
					AkSoundEngine.PostEvent("Play_ENM_jammer_curse_01", base.gameObject);
					this.aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_DBZ_Charge") as GameObject, Vector3.zero - offset1, false, false, false);
					yield return new WaitForSeconds(1.3f);
					if (this.aiActor != null)
					{

						string text;
						text = BraveUtility.RandomElement<string>(enemylisttouse);
						AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(text);
						//IntVector2 bestRewardLocation = this.aiActor.transform.position.GetAbsoluteRoom().GetBestRewardLocation(orLoadByGuid.Clearance, this.aiActor.sprite.WorldCenter - offset, true);
						AIActor aiactor = AIActor.Spawn(orLoadByGuid, base.aiActor.sprite.WorldBottomCenter - offset, this.aiActor.gameObject.transform.position.GetAbsoluteRoom(), true, AIActor.AwakenAnimationType.Default, true);
						var position = aiactor.GetComponent<tk2dSprite>().WorldBottomCenter;
						if (this.aiActor.IsBlackPhantom)
						{
							aiactor.BecomeBlackPhantom();
						}
						if (position != null)
						{
							aiactor.PlayEffectOnActor(m_BlinkPassive.BlinkpoofVfx, Vector3.zero, true, false, false);
							AkSoundEngine.PostEvent("Play_OBJ_item_spawn_01", base.gameObject);

						}
						aiactor.aiAnimator.PlayDefaultSpawnState();
						aiactor.CanDropCurrency = false;
						this.aiActor.knockbackDoer.SetImmobile(false, "laugh");
					}
					yield return new WaitForSeconds(2f);
					GameManager.Instance.StartCoroutine(Summon());


				}
				else
				{
					yield return new WaitForSeconds(1f);
					GameManager.Instance.StartCoroutine(Summon());
				}
			}

		

		public void ReDetermineEnemyPool()
		{
			bool flag = false;
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			GlobalDungeonData.ValidTilesets validTilesets = tilesetId;
			if (validTilesets <= GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				if (validTilesets <= GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
				{
					switch (validTilesets)
					{
						case GlobalDungeonData.ValidTilesets.GUNGEON:
							enemylisttouse = gpEnemyPalette;
							flag = true;
							break;
						case GlobalDungeonData.ValidTilesets.CASTLEGEON:
							enemylisttouse = keepEnemyPalette;
							flag = true;
							break;
						case GlobalDungeonData.ValidTilesets.WESTGEON:
							break;
						case GlobalDungeonData.ValidTilesets.SEWERGEON:
							enemylisttouse = oubEnemyPalette;
							flag = true;
							break;
						default:
							if (validTilesets == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
							{
								enemylisttouse = abbeyEnemyPalette;
								flag = true;
							}
							break;
					}
				}
				else if (validTilesets != GlobalDungeonData.ValidTilesets.MINEGEON)
				{
					if (validTilesets != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
					{
						if (validTilesets == GlobalDungeonData.ValidTilesets.FORGEGEON)
						{
							enemylisttouse = forgeEnemyPalette;
							flag = true;
						}
					}
					else
					{
						enemylisttouse = hollowEnemyPalette;
						flag = true;
					}
				}
				else
				{
					enemylisttouse = minesEnemyPalette;
					flag = true;
				}
			}
			else if (validTilesets <= GlobalDungeonData.ValidTilesets.OFFICEGEON)
			{
				if (validTilesets != GlobalDungeonData.ValidTilesets.HELLGEON)
				{
					if (validTilesets != GlobalDungeonData.ValidTilesets.PHOBOSGEON)
					{
						if (validTilesets == GlobalDungeonData.ValidTilesets.OFFICEGEON)
						{
							enemylisttouse = rngEnemyPalette;
							flag = true;
						}
					}
					else
					{
						enemylisttouse = keepEnemyPalette;
						flag = true;
					}
				}
				else
				{
					enemylisttouse = hellEnemyPalette;
					flag = true;
				}
			}
			else if (validTilesets != GlobalDungeonData.ValidTilesets.BELLYGEON)
			{
				if (validTilesets != GlobalDungeonData.ValidTilesets.JUNGLEGEON)
				{
					if (validTilesets == GlobalDungeonData.ValidTilesets.RATGEON)
					{
						enemylisttouse = ratEnemyPalette;
						flag = true;
					}
				}
				else
				{
					enemylisttouse = jungleEnemyPalette;
					flag = true;
				}
			}
			else
			{
				enemylisttouse = bellyEnemyPalette;
				flag = true;
			}
			bool flag2 = !flag;
			if (flag2)
			{
				enemylisttouse = keepEnemyPalette;
			}
		}

		public static List<string> enemylisttouse;

		// Token: 0x040000F4 RID: 244
		public static List<string> keepEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["ak47_bullet_kin"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["rubber_kin"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["bookllet"],
			EnemyGuidDatabase.Entries["blue_bookllet"],
			EnemyGuidDatabase.Entries["green_bookllet"],
			EnemyGuidDatabase.Entries["gigi"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
		};

		// Token: 0x040000F5 RID: 245
		public static List<string> oubEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["ak47_bullet_kin"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["mutant_bullet_kin"],
			EnemyGuidDatabase.Entries["shroomer"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["mutant_shotgun_kin"],
			EnemyGuidDatabase.Entries["sniper_shell"],
			EnemyGuidDatabase.Entries["professional"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["king_bullat"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["poisbulon"],
			EnemyGuidDatabase.Entries["poopulon"],
			EnemyGuidDatabase.Entries["gigi"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
			EnemyGuidDatabase.Entries["gat"],
			EnemyGuidDatabase.Entries["gunzookie"],
			EnemyGuidDatabase.Entries["gunzockie"],
			EnemyGuidDatabase.Entries["fungun"],

		};

		// Token: 0x040000F6 RID: 246
		public static List<string> gpEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["ak47_bullet_kin"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["sniper_shell"],
			EnemyGuidDatabase.Entries["professional"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["rubber_kin"],
			EnemyGuidDatabase.Entries["tazie"],
			EnemyGuidDatabase.Entries["king_bullat"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["dynamite_kin"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["blobuloid"],
			EnemyGuidDatabase.Entries["blobulin"],
			EnemyGuidDatabase.Entries["bookllet"],
			EnemyGuidDatabase.Entries["blue_bookllet"],
			EnemyGuidDatabase.Entries["green_bookllet"],
			EnemyGuidDatabase.Entries["gigi"],
			EnemyGuidDatabase.Entries["muzzle_wisp"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["chancebulon"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
			EnemyGuidDatabase.Entries["bullet_shark"],
			EnemyGuidDatabase.Entries["beadie"],
			EnemyGuidDatabase.Entries["executioner"]
		};

		// Token: 0x040000F7 RID: 247
		public static List<string> abbeyEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["cardinal"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["skullet"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["king_bullat"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["poopulon"],
			EnemyGuidDatabase.Entries["bloodbulon"],
			EnemyGuidDatabase.Entries["skusket"],
			EnemyGuidDatabase.Entries["bookllet"],
			EnemyGuidDatabase.Entries["blue_bookllet"],
			EnemyGuidDatabase.Entries["green_bookllet"],
			EnemyGuidDatabase.Entries["muzzle_wisp"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["wizbang"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
			EnemyGuidDatabase.Entries["bullet_shark"],
			EnemyGuidDatabase.Entries["gun_cultist"]
		};

		// Token: 0x040000F8 RID: 248
		public static List<string> minesEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["treadnaughts_bullet_kin"],
			EnemyGuidDatabase.Entries["minelet"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["sniper_shell"],
			EnemyGuidDatabase.Entries["professional"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["bombshee"],
			EnemyGuidDatabase.Entries["rubber_kin"],
			EnemyGuidDatabase.Entries["tazie"],
			EnemyGuidDatabase.Entries["king_bullat"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["dynamite_kin"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["poisbulon"],
			EnemyGuidDatabase.Entries["skusket"],
			EnemyGuidDatabase.Entries["gigi"],
			EnemyGuidDatabase.Entries["muzzle_wisp"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
			EnemyGuidDatabase.Entries["coaler"],
			EnemyGuidDatabase.Entries["gat"],
			EnemyGuidDatabase.Entries["gunzookie"],
			EnemyGuidDatabase.Entries["gunzockie"],
			EnemyGuidDatabase.Entries["bullet_shark"],
			EnemyGuidDatabase.Entries["chancebulon"],
			EnemyGuidDatabase.Entries["fungun"],
		};

		// Token: 0x040000F9 RID: 249
		public static List<string> ratEnemyPalette = new List<string>
		{

			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["mutant_shotgun_kin"],
			EnemyGuidDatabase.Entries["ashen_shotgun_kin"],
			EnemyGuidDatabase.Entries["skullet"],
			EnemyGuidDatabase.Entries["creech"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["chancebulon"],
			EnemyGuidDatabase.Entries["ammomancer"],
			EnemyGuidDatabase.Entries["jammomancer"],
			EnemyGuidDatabase.Entries["jamerlengo"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["lead_maiden"],
			EnemyGuidDatabase.Entries["misfire_beast"],
			EnemyGuidDatabase.Entries["phaser_spider"],
			EnemyGuidDatabase.Entries["killithid"],
			EnemyGuidDatabase.Entries["shelleton"],
			EnemyGuidDatabase.Entries["gunreaper"],
			EnemyGuidDatabase.Entries["mouser"],
			EnemyGuidDatabase.Entries["mouser"],
			EnemyGuidDatabase.Entries["mouser"],
			EnemyGuidDatabase.Entries["mouser"],
			EnemyGuidDatabase.Entries["rat"],
			EnemyGuidDatabase.Entries["rat_candle"]
		};

		// Token: 0x040000FA RID: 250
		public static List<string> hollowEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["ak47_bullet_kin"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["cardinal"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["sniper_shell"],
			EnemyGuidDatabase.Entries["professional"],
			EnemyGuidDatabase.Entries["gummy"],
			EnemyGuidDatabase.Entries["skullet"],
			EnemyGuidDatabase.Entries["skullmet"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["king_bullat"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["dynamite_kin"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["poisbulon"],
			EnemyGuidDatabase.Entries["blizzbulon"],
			EnemyGuidDatabase.Entries["bloodbulon"],
			EnemyGuidDatabase.Entries["skusket"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["chancebulon"],
			EnemyGuidDatabase.Entries["wizbang"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
		};

		// Token: 0x040000FB RID: 251
		public static List<string> rngEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["office_bullet_kin"],
			EnemyGuidDatabase.Entries["office_bullette_kin"],
			EnemyGuidDatabase.Entries["brollet"],
			EnemyGuidDatabase.Entries["western_bullet_kin"],
			EnemyGuidDatabase.Entries["pirate_bullet_kin"],
			EnemyGuidDatabase.Entries["armored_bullet_kin"],
			EnemyGuidDatabase.Entries["western_shotgun_kin"],
			EnemyGuidDatabase.Entries["pirate_shotgun_kin"],
			EnemyGuidDatabase.Entries["gargoyle"],
			EnemyGuidDatabase.Entries["necronomicon"],
			EnemyGuidDatabase.Entries["tablet_bookllett"],
			EnemyGuidDatabase.Entries["grey_cylinder"],
			EnemyGuidDatabase.Entries["red_cylinder"],
			EnemyGuidDatabase.Entries["bullet_mech"],
			EnemyGuidDatabase.Entries["m80_kin"],
			EnemyGuidDatabase.Entries["candle_kin"],
			EnemyGuidDatabase.Entries["western_cactus"],
			EnemyGuidDatabase.Entries["musketball"],
			EnemyGuidDatabase.Entries["bird_parrot"],
			EnemyGuidDatabase.Entries["western_snake"],
			EnemyGuidDatabase.Entries["kalibullet"],
			EnemyGuidDatabase.Entries["kbullet"],
			EnemyGuidDatabase.Entries["blue_fish_bullet_kin"],
			EnemyGuidDatabase.Entries["green_fish_bullet_kin"],
			EnemyGuidDatabase.Entries["fridge_maiden"],
			EnemyGuidDatabase.Entries["titan_bullet_kin"],
			EnemyGuidDatabase.Entries["titan_bullet_kin_boss"],
			EnemyGuidDatabase.Entries["titaness_bullet_kin_boss"]
		};

		// Token: 0x040000FC RID: 252
		public static List<string> forgeEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["ak47_bullet_kin"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["shroomer"],
			EnemyGuidDatabase.Entries["ashen_bullet_kin"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["sniper_shell"],
			EnemyGuidDatabase.Entries["professional"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["king_bullat"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["dynamite_kin"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["leadbulon"],
			EnemyGuidDatabase.Entries["muzzle_wisp"],
			EnemyGuidDatabase.Entries["muzzle_flare"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["chancebulon"],
			EnemyGuidDatabase.Entries["cubulead"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
			EnemyGuidDatabase.Entries["wizbang"],
			EnemyGuidDatabase.Entries["coaler"],
			EnemyGuidDatabase.Entries["gun_cultist"],
			EnemyGuidDatabase.Entries["lead_cube"],
		};

		// Token: 0x040000FD RID: 253
		public static List<string> hellEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["ak47_bullet_kin"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["cardinal"],
			EnemyGuidDatabase.Entries["shroomer"],
			EnemyGuidDatabase.Entries["ashen_bullet_kin"],
			EnemyGuidDatabase.Entries["mutant_bullet_kin"],
			EnemyGuidDatabase.Entries["fallen_bullet_kin"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["ashen_shotgun_kin"],
			EnemyGuidDatabase.Entries["shotgrub"],
			EnemyGuidDatabase.Entries["sniper_shell"],
			EnemyGuidDatabase.Entries["professional"],
			EnemyGuidDatabase.Entries["gummy"],
			EnemyGuidDatabase.Entries["skullet"],
			EnemyGuidDatabase.Entries["skullmet"],
			EnemyGuidDatabase.Entries["creech"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["king_bullat"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["poisbulon"],
			EnemyGuidDatabase.Entries["leadbulon"],
			EnemyGuidDatabase.Entries["poopulon"],
			EnemyGuidDatabase.Entries["bloodbulon"],
			EnemyGuidDatabase.Entries["muzzle_wisp"],
			EnemyGuidDatabase.Entries["muzzle_flare"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["cubulead"],
			EnemyGuidDatabase.Entries["wizbang"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
			EnemyGuidDatabase.Entries["gunzookie"],
			EnemyGuidDatabase.Entries["gunzockie"],
			EnemyGuidDatabase.Entries["tombstoner"],
			EnemyGuidDatabase.Entries["gun_cultist"],

		};

		// Token: 0x040000FE RID: 254
		public static List<string> jungleEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["western_snake"],
			EnemyGuidDatabase.Entries["chameleon"],
			EnemyGuidDatabase.Entries["gigi"],
			EnemyGuidDatabase.Entries["bird_parrot"],
			EnemyGuidDatabase.Entries["phaser_spider"],
			EnemyGuidDatabase.Entries["misfire_beast"],
			EnemyGuidDatabase.Entries["gunzookie"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
			EnemyGuidDatabase.Entries["blue_fish_bullet_kin"],
			EnemyGuidDatabase.Entries["green_fish_bullet_kin"],
			EnemyGuidDatabase.Entries["skullet"],
			EnemyGuidDatabase.Entries["arrow_head"],
			EnemyGuidDatabase.Entries["shambling_round"],
			EnemyGuidDatabase.Entries["hooded_bullet"],
			EnemyGuidDatabase.Entries["gummy"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["gunsinger"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["treadnaughts_bullet_kin"],
			EnemyGuidDatabase.Entries["sniper_shell"],
			EnemyGuidDatabase.Entries["professional"],
			EnemyGuidDatabase.Entries["fungun"],
			EnemyGuidDatabase.Entries["spogre"],
			EnemyGuidDatabase.Entries["pot_fairy"]
		};

		// Token: 0x040000FF RID: 255
		public static List<string> bellyEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["blue_fish_bullet_kin"],
			EnemyGuidDatabase.Entries["green_fish_bullet_kin"],
			EnemyGuidDatabase.Entries["pirate_bullet_kin"],
			EnemyGuidDatabase.Entries["pirate_shotgun_kin"],
			EnemyGuidDatabase.Entries["musketball"],
			EnemyGuidDatabase.Entries["dynamite_kin"],
			EnemyGuidDatabase.Entries["tarnisher"],
			EnemyGuidDatabase.Entries["bullet_shark"],
			EnemyGuidDatabase.Entries["great_bullet_shark"],
			EnemyGuidDatabase.Entries["flesh_cube"],
			EnemyGuidDatabase.Entries["shotgrub"],
			EnemyGuidDatabase.Entries["kbullet"],
			EnemyGuidDatabase.Entries["kalibullet"],
			EnemyGuidDatabase.Entries["creech"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["blobuloid"],
			EnemyGuidDatabase.Entries["blobulin"],
			EnemyGuidDatabase.Entries["poisbulon"],
			EnemyGuidDatabase.Entries["poisbuloid"],
			EnemyGuidDatabase.Entries["poisbulin"],
			EnemyGuidDatabase.Entries["poopulon"],
			EnemyGuidDatabase.Entries["bloodbulon"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["beadie"],
			EnemyGuidDatabase.Entries["mutant_bullet_kin"],
			EnemyGuidDatabase.Entries["gummy_spent"],
			EnemyGuidDatabase.Entries["shelleton"],
			EnemyGuidDatabase.Entries["skusket"],
			EnemyGuidDatabase.Entries["skusket_head"],
			EnemyGuidDatabase.Entries["skullmet"],
			EnemyGuidDatabase.Entries["spectral_gun_nut"]
		};

		// Token: 0x04000100 RID: 256
		public static List<string> westEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["blobulon"]
		};

		// Token: 0x04000101 RID: 257
		public static List<string> chaosEnemyPalette = new List<string>
		{
			EnemyGuidDatabase.Entries["bullet_kin"],
			EnemyGuidDatabase.Entries["ak47_bullet_kin"],
			EnemyGuidDatabase.Entries["bandana_bullet_kin"],
			EnemyGuidDatabase.Entries["veteran_bullet_kin"],
			EnemyGuidDatabase.Entries["treadnaughts_bullet_kin"],
			EnemyGuidDatabase.Entries["minelet"],
			EnemyGuidDatabase.Entries["cardinal"],
			EnemyGuidDatabase.Entries["shroomer"],
			EnemyGuidDatabase.Entries["ashen_bullet_kin"],
			EnemyGuidDatabase.Entries["mutant_bullet_kin"],
			EnemyGuidDatabase.Entries["fallen_bullet_kin"],
			EnemyGuidDatabase.Entries["chance_bullet_kin"],
			EnemyGuidDatabase.Entries["key_bullet_kin"],
			EnemyGuidDatabase.Entries["hooded_bullet"],
			EnemyGuidDatabase.Entries["red_shotgun_kin"],
			EnemyGuidDatabase.Entries["blue_shotgun_kin"],
			EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
			EnemyGuidDatabase.Entries["mutant_shotgun_kin"],
			EnemyGuidDatabase.Entries["executioner"],
			EnemyGuidDatabase.Entries["ashen_shotgun_kin"],
			EnemyGuidDatabase.Entries["shotgrub"],
			EnemyGuidDatabase.Entries["sniper_shell"],
			EnemyGuidDatabase.Entries["professional"],
			EnemyGuidDatabase.Entries["gummy"],
			EnemyGuidDatabase.Entries["skullet"],
			EnemyGuidDatabase.Entries["skullmet"],
			EnemyGuidDatabase.Entries["creech"],
			EnemyGuidDatabase.Entries["hollowpoint"],
			EnemyGuidDatabase.Entries["bombshee"],
			EnemyGuidDatabase.Entries["rubber_kin"],
			EnemyGuidDatabase.Entries["tazie"],
			EnemyGuidDatabase.Entries["king_bullat"],
			EnemyGuidDatabase.Entries["grenade_kin"],
			EnemyGuidDatabase.Entries["dynamite_kin"],
			EnemyGuidDatabase.Entries["arrow_head"],
			EnemyGuidDatabase.Entries["blobulon"],
			EnemyGuidDatabase.Entries["blobuloid"],
			EnemyGuidDatabase.Entries["blobulin"],
			EnemyGuidDatabase.Entries["poisbulon"],
			EnemyGuidDatabase.Entries["poisbuloid"],
			EnemyGuidDatabase.Entries["poisbulin"],
			EnemyGuidDatabase.Entries["blizzbulon"],
			EnemyGuidDatabase.Entries["leadbulon"],
			EnemyGuidDatabase.Entries["poopulon"],
			EnemyGuidDatabase.Entries["bloodbulon"],
			EnemyGuidDatabase.Entries["skusket"],
			EnemyGuidDatabase.Entries["bookllet"],
			EnemyGuidDatabase.Entries["blue_bookllet"],
			EnemyGuidDatabase.Entries["green_bookllet"],
			EnemyGuidDatabase.Entries["gigi"],
			EnemyGuidDatabase.Entries["muzzle_wisp"],
			EnemyGuidDatabase.Entries["muzzle_flare"],
			EnemyGuidDatabase.Entries["cubulon"],
			EnemyGuidDatabase.Entries["chancebulon"],
			EnemyGuidDatabase.Entries["cubulead"],
			EnemyGuidDatabase.Entries["apprentice_gunjurer"],
			EnemyGuidDatabase.Entries["gunjurer"],
			EnemyGuidDatabase.Entries["high_gunjurer"],
			EnemyGuidDatabase.Entries["lore_gunjurer"],
			EnemyGuidDatabase.Entries["gunsinger"],
			EnemyGuidDatabase.Entries["aged_gunsinger"],
			EnemyGuidDatabase.Entries["ammomancer"],
			EnemyGuidDatabase.Entries["jammomancer"],
			EnemyGuidDatabase.Entries["jamerlengo"],
			EnemyGuidDatabase.Entries["wizbang"],
			EnemyGuidDatabase.Entries["pot_fairy"],
			EnemyGuidDatabase.Entries["bullat"],
			EnemyGuidDatabase.Entries["shotgat"],
			EnemyGuidDatabase.Entries["grenat"],
			EnemyGuidDatabase.Entries["spirat"],
			EnemyGuidDatabase.Entries["coaler"],
			EnemyGuidDatabase.Entries["gat"],
			EnemyGuidDatabase.Entries["diagonal_det"],
			EnemyGuidDatabase.Entries["diagonal_x_det"],
			EnemyGuidDatabase.Entries["gunzookie"],
			EnemyGuidDatabase.Entries["gunzockie"],
			EnemyGuidDatabase.Entries["bullet_shark"],
			EnemyGuidDatabase.Entries["great_bullet_shark"],
			EnemyGuidDatabase.Entries["tombstoner"],
			EnemyGuidDatabase.Entries["gun_cultist"],
			EnemyGuidDatabase.Entries["beadie"],
			EnemyGuidDatabase.Entries["gun_nut"],
			EnemyGuidDatabase.Entries["spectral_gun_nut"],
			EnemyGuidDatabase.Entries["chain_gunner"],
			EnemyGuidDatabase.Entries["fungun"],
			EnemyGuidDatabase.Entries["spogre"],
			EnemyGuidDatabase.Entries["mountain_cube"],
			EnemyGuidDatabase.Entries["flesh_cube"],
			EnemyGuidDatabase.Entries["lead_cube"],
			EnemyGuidDatabase.Entries["lead_maiden"],
			EnemyGuidDatabase.Entries["misfire_beast"],
			EnemyGuidDatabase.Entries["phaser_spider"],
			EnemyGuidDatabase.Entries["killithid"],
			EnemyGuidDatabase.Entries["grip_master"],
			EnemyGuidDatabase.Entries["tarnisher"],
			EnemyGuidDatabase.Entries["shambling_round"],
			EnemyGuidDatabase.Entries["shelleton"],
			EnemyGuidDatabase.Entries["agonizer"],
			EnemyGuidDatabase.Entries["revolvenant"],
			EnemyGuidDatabase.Entries["gunreaper"],
			EnemyGuidDatabase.Entries["mine_flayers_bell"],
			EnemyGuidDatabase.Entries["mine_flayers_claymore"],
			EnemyGuidDatabase.Entries["fusebot"],
			EnemyGuidDatabase.Entries["ammoconda_ball"],
			EnemyGuidDatabase.Entries["bullet_kings_toadie"],
			EnemyGuidDatabase.Entries["bullet_kings_toadie_revenge"],
			EnemyGuidDatabase.Entries["old_kings_toadie"],
			EnemyGuidDatabase.Entries["draguns_knife"],
			EnemyGuidDatabase.Entries["dragun_knife_advanced"],
			EnemyGuidDatabase.Entries["gummy_spent"],
			EnemyGuidDatabase.Entries["candle_guy"],
			EnemyGuidDatabase.Entries["convicts_past_soldier"],
			EnemyGuidDatabase.Entries["robots_past_terminator"],
			EnemyGuidDatabase.Entries["marines_past_imp"],
			EnemyGuidDatabase.Entries["chicken"],
			EnemyGuidDatabase.Entries["rat"],
			EnemyGuidDatabase.Entries["dragun_egg_slimeguy"],
			EnemyGuidDatabase.Entries["poopulons_corn"],
			EnemyGuidDatabase.Entries["rat_candle"],
			EnemyGuidDatabase.Entries["tiny_blobulord"],
			EnemyGuidDatabase.Entries["office_bullet_kin"],
			EnemyGuidDatabase.Entries["office_bullette_kin"],
			EnemyGuidDatabase.Entries["brollet"],
			EnemyGuidDatabase.Entries["western_bullet_kin"],
			EnemyGuidDatabase.Entries["pirate_bullet_kin"],
			EnemyGuidDatabase.Entries["armored_bullet_kin"],
			EnemyGuidDatabase.Entries["western_shotgun_kin"],
			EnemyGuidDatabase.Entries["pirate_shotgun_kin"],
			EnemyGuidDatabase.Entries["gargoyle"],
			EnemyGuidDatabase.Entries["necronomicon"],
			EnemyGuidDatabase.Entries["tablet_bookllett"],
			EnemyGuidDatabase.Entries["grey_cylinder"],
			EnemyGuidDatabase.Entries["red_cylinder"],
			EnemyGuidDatabase.Entries["bullet_mech"],
			EnemyGuidDatabase.Entries["m80_kin"],
			EnemyGuidDatabase.Entries["candle_kin"],
			EnemyGuidDatabase.Entries["western_cactus"],
			EnemyGuidDatabase.Entries["musketball"],
			EnemyGuidDatabase.Entries["bird_parrot"],
			EnemyGuidDatabase.Entries["western_snake"],
			EnemyGuidDatabase.Entries["kalibullet"],
			EnemyGuidDatabase.Entries["kbullet"],
			EnemyGuidDatabase.Entries["blue_fish_bullet_kin"],
			EnemyGuidDatabase.Entries["green_fish_bullet_kin"],
			EnemyGuidDatabase.Entries["fridge_maiden"],
			EnemyGuidDatabase.Entries["titan_bullet_kin"],
			EnemyGuidDatabase.Entries["titan_bullet_kin_boss"],
			EnemyGuidDatabase.Entries["titaness_bullet_kin_boss"],
			EnemyGuidDatabase.Entries["robots_past_critter_3"],
			EnemyGuidDatabase.Entries["robots_past_critter_2"],
			EnemyGuidDatabase.Entries["robots_past_critter_1"],
			EnemyGuidDatabase.Entries["snake"]
		};
	}

}
}



	

