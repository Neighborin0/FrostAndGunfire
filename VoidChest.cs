using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class VoidChest : PlayerItem
	{
		private static int[] spriteIDs;
		private static readonly string[] spritePaths = new string[]
			{
			"FrostAndGunfireItems/Resources/void_chest_001",
			"FrostAndGunfireItems/Resources/void_chest_002",
			"FrostAndGunfireItems/Resources/void_chest_003",


	};
		int id;
		private int stacks;
		
		public static void Init()
		{
			string name = "Void Chest";
			string resourcePath = "FrostAndGunfireItems/Resources/void_chest_001";
			GameObject gameObject = new GameObject();
			VoidChest voidChest = gameObject.AddComponent<VoidChest>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "End Game";
			string longDesc = "A bottomless chest, throwing things into it seems like a waste...";
			ItemBuilder.SetupItem(voidChest, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(voidChest, ItemBuilder.CooldownType.Timed, 0f);
			voidChest.consumable = false;
			voidChest.quality = PickupObject.ItemQuality.B;
			voidChest.AddToSubShop(ItemBuilder.ShopType.Flynt);
			VoidChest.spriteIDs = new int[VoidChest.spritePaths.Length];
			VoidChest.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(VoidChest.spritePaths[0], voidChest.sprite.Collection);
			VoidChest.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(VoidChest.spritePaths[1], voidChest.sprite.Collection);
			VoidChest.spriteIDs[2] = SpriteBuilder.AddSpriteToCollection(VoidChest.spritePaths[2], voidChest.sprite.Collection);
		}

		public override void Update()
		{
			PlayerController player = LastOwner;
			bool flag = player;
			bool flag2 = flag;
			if (flag2)
			{
				if (stacks >= 5)
					id = spriteIDs[2];
				else if (stacks >= 2 && stacks <= 4)
					id = spriteIDs[1];
				else
					id = spriteIDs[0];
				sprite.SetSprite(id);


			}
		}
		protected override void DoEffect(PlayerController user)
		{

			tk2dSpriteCollectionData tk2dSpriteCollectionData = null;
				int spriteId = -1;
				Vector3 position = Vector3.zero;
			if (stacks >= 5)
			{
				IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
				Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(bestRewardLocation);
				chest2.IsLocked = false;
				base.StartCoroutine(this.RemoveStacks());
			}
			else
			{
				AkSoundEngine.PostEvent("Play_WPN_zapper_reload_01", base.gameObject);
				stacks += 1;
				IPlayerInteractable lastInteractable = user.GetLastInteractable();
				if (lastInteractable is HeartDispenser)
				{
					HeartDispenser exists = lastInteractable as HeartDispenser;
					if (exists && HeartDispenser.CurrentHalfHeartsStored > 0)
					{
						if (HeartDispenser.CurrentHalfHeartsStored > 1)
						{
							HeartDispenser.CurrentHalfHeartsStored -= 2;

						}
						else
						{
							HeartDispenser.CurrentHalfHeartsStored--;

						}
						return;
					}
				}

				if (StaticReferenceManager.AllDebris != null)
				{
					DebrisObject debrisObject = null;
					float num = float.MaxValue;
					for (int j = 0; j < StaticReferenceManager.AllDebris.Count; j++)
					{
						DebrisObject debrisObject2 = StaticReferenceManager.AllDebris[j];
						if (debrisObject2.IsPickupObject)
						{
							float sqrMagnitude = (user.CenterPosition - debrisObject2.transform.position.XY()).sqrMagnitude;
							if (sqrMagnitude <= 25f)
							{
								HealthPickup component = debrisObject2.GetComponent<HealthPickup>();
								AmmoPickup component2 = debrisObject2.GetComponent<AmmoPickup>();
								KeyBulletPickup component3 = debrisObject2.GetComponent<KeyBulletPickup>();
								SilencerItem component4 = debrisObject2.GetComponent<SilencerItem>();
								if ((component && component.armorAmount == 0 && (component.healAmount == 0.5f || component.healAmount == 1f)) || component2 || component3 || component4)
								{
									float num2 = Mathf.Sqrt(sqrMagnitude);
									if (num2 < num && num2 < 5f)
									{
										num = num2;
										debrisObject = debrisObject2;
									}
								}
							}
						}
					}
					if (debrisObject)
					{
						HealthPickup component5 = debrisObject.GetComponent<HealthPickup>();
						AmmoPickup component6 = debrisObject.GetComponent<AmmoPickup>();
						KeyBulletPickup component7 = debrisObject.GetComponent<KeyBulletPickup>();
						SilencerItem component8 = debrisObject.GetComponent<SilencerItem>();
					
						if (component5)
						{
							if (component5.sprite)
							{
								tk2dSpriteCollectionData = component5.sprite.Collection;
								spriteId = component5.sprite.spriteId;
								position = component5.transform.position;
								
							}
							if (component5.armorAmount == 0 && component5.healAmount == 0.5f)
							{
								
								UnityEngine.Object.Destroy(component5.gameObject);
							}
							else if (component5.armorAmount == 0 && component5.healAmount == 1f)
							{
								
								UnityEngine.Object.Destroy(component5.gameObject);
							}
						}
						else if (component6)
						{
							if (component6.sprite)
							{
								tk2dSpriteCollectionData = component6.sprite.Collection;
								spriteId = component6.sprite.spriteId;
								position = component6.transform.position;
							}
						
							UnityEngine.Object.Destroy(component6.gameObject);
						}
						else if (component7)
						{
							
							if (component7.sprite)
							{
								tk2dSpriteCollectionData = component7.sprite.Collection;
								spriteId = component7.sprite.spriteId;
								position = component7.transform.position;
							}

							UnityEngine.Object.Destroy(component7.gameObject);
						}
						else if (component8)
						{
							if (component8.sprite)
							{
								tk2dSpriteCollectionData = component8.sprite.Collection;
								spriteId = component8.sprite.spriteId;
								position = component8.transform.position;
							}
							
							UnityEngine.Object.Destroy(component8.gameObject);
						}
					}
				}

				if (tk2dSpriteCollectionData != null)
				{
					tk2dSprite targetSprite = tk2dSprite.AddComponent(new GameObject("sucked sprite")
					{
						transform =
					{
						position = position
					}
					}, tk2dSpriteCollectionData, spriteId);
					GameManager.Instance.Dungeon.StartCoroutine(this.HandleSuck(targetSprite));
				}
			}
			
		
		}

		private IEnumerator RemoveStacks()
		{
			yield return new WaitForSeconds(0.1f);
			stacks -= stacks;
			yield break;
		}

		private IEnumerator HandleSuck(tk2dSprite targetSprite)
		{
			float elapsed = 0f;
			float duration = 0.25f;
			PlayerController owner = this.LastOwner;
			if (targetSprite)
			{
				Vector3 startPosition = targetSprite.transform.position;
				while (elapsed < duration && owner)
				{
					elapsed += BraveTime.DeltaTime;
					if (targetSprite)
					{
						targetSprite.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), elapsed / duration);
						targetSprite.transform.position = Vector3.Lerp(startPosition, owner.CenterPosition.ToVector3ZisY(0f), elapsed / duration);
					}
					yield return null;
				}
			}
			UnityEngine.Object.Destroy(targetSprite.gameObject);
			yield break;
		}
		public override bool CanBeUsed(PlayerController user)
		{
			if(stacks < 5)
			{
				if (!user)
				{
					return false;
				}
				List<DebrisObject> allDebris = StaticReferenceManager.AllDebris;
				if (allDebris != null)
				{
					for (int j = 0; j < allDebris.Count; j++)
					{
						DebrisObject debrisObject = allDebris[j];
						if (debrisObject && debrisObject.IsPickupObject)
						{
							float sqrMagnitude = (user.CenterPosition - debrisObject.transform.position.XY()).sqrMagnitude;
							if (sqrMagnitude <= 25f)
							{
								HealthPickup component = debrisObject.GetComponent<HealthPickup>();
								AmmoPickup component2 = debrisObject.GetComponent<AmmoPickup>();
								KeyBulletPickup component3 = debrisObject.GetComponent<KeyBulletPickup>();
								SilencerItem component4 = debrisObject.GetComponent<SilencerItem>();
								if ((component && component.armorAmount == 0 && (component.healAmount == 0.5f || component.healAmount == 1f)) || component2 || component3 || component4)
								{
									float num = Mathf.Sqrt(sqrMagnitude);
									if (num < 5f)
									{
										return true;
									}
								}
							}
						}
					}
				}
				if (user)
				{
					IPlayerInteractable lastInteractable = user.GetLastInteractable();
					if (lastInteractable is HeartDispenser)
					{
						HeartDispenser exists = lastInteractable as HeartDispenser;
						if (exists && HeartDispenser.CurrentHalfHeartsStored > 0)
						{
							return true;
						}
					}
				}
				return false;
			}
		
			else
			{
				
			}
			return base.CanBeUsed(user);
		}




	}
}
