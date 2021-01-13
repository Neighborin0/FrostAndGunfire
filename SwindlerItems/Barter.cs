using System;
using System.Collections.Generic;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class Barter : PlayerItem
	{

		public List<GameObject> ItemsAlreadySelected { get; } = new List<GameObject>();
		public static void Init()
		{
			string name = "Barter";
			string resourcePath = "FrostAndGunfireItems/Resources/barter";
			GameObject gameObject = new GameObject();
			Barter item = gameObject.AddComponent<Barter>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "The Art of The Deal";
			string longDesc = "Allows shops to be rerolled at the cost of 15 casings.\n\nThe Swindler was always able to talk his way into a good deal, unfortunately he couldn't talk himself out of fights...";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 0f);
			item.consumable = false;
			item.quality = PickupObject.ItemQuality.EXCLUDED;
		}

	

	

		// Token: 0x060000A5 RID: 165 RVA: 0x000078D4 File Offset: 0x00005AD4
		protected override void DoEffect(PlayerController user)
		{
			user.carriedConsumables.Currency -= 15;
			AkSoundEngine.PostEvent("Play_OBJ_coin_medium_01", base.gameObject);
			foreach (BaseShopController baseShopController in StaticReferenceManager.AllShops)
			{
				List<ShopItemController> list = Tools.ReflectGetField<List<ShopItemController>>(typeof(BaseShopController), "m_itemControllers", baseShopController);
				for (int i = 0; i < list.Count; i++)
				{
					ShopItemController shopItemController = list[i];
					if (shopItemController != null && shopItemController.item.PickupObjectId != 224)
					{
						PickupObject item = shopItemController.item;			
						GameObject gameObject = null;
						if (item is PassiveItem)
							{
								gameObject = GameManager.Instance.RewardManager.ItemsLootTable.SelectByWeightWithoutDuplicates(ItemsAlreadySelected, true);
								if (gameObject != null)
								{
									ItemsAlreadySelected.Add(gameObject);
								}
							}
							else
							{
								if (item is PlayerItem)
								{
									gameObject = GameManager.Instance.RewardManager.ItemsLootTable.SelectByWeightWithoutDuplicates(ItemsAlreadySelected, true);
									if (gameObject != null)
									{
										ItemsAlreadySelected.Add(gameObject);
									}
								}
								else
								{
									if (item is Gun)
									{
										gameObject = GameManager.Instance.RewardManager.GunsLootTable.SelectByWeightWithoutDuplicates(ItemsAlreadySelected, true);
										if (gameObject != null)
										{
											ItemsAlreadySelected.Add(gameObject);
										}
									}
								}
							}
							if (gameObject != null)
							{	
								ShopItemController x = this.ReplaceShopItem(baseShopController, shopItemController, gameObject);
								if (x != null)
								{
									LootEngine.DoDefaultItemPoof(x.sprite.WorldCenter, false, false);
								}
							}
						}
					}
				}
			}
		

		private ShopItemController ReplaceShopItem(BaseShopController shopController, ShopItemController oldShopItemController, GameObject itemToAdd)
		{
			PickupObject component = itemToAdd.GetComponent<PickupObject>();
			List<ShopItemController> list = Tools.ReflectGetField<List<ShopItemController>>(typeof(BaseShopController), "m_itemControllers", shopController);
			List<GameObject> list2 = Tools.ReflectGetField<List<GameObject>>(typeof(BaseShopController), "m_shopItems", shopController);
			for (int i = 0; i < list2.Count; i++)
			{
				GameObject gameObject = list2[i];
				bool flag = gameObject == null;
				if (!flag)
				{
					PickupObject component2 = gameObject.GetComponent<PickupObject>();
					bool flag2 = component2 == null;
					if (!flag2)
					{
						bool flag3 = component2 != oldShopItemController.item;
						if (!flag3)
						{
							Transform parent = oldShopItemController.gameObject.transform.parent;
							if (parent != null && component.quality != ItemQuality.EXCLUDED && component.quality != ItemQuality.SPECIAL)
							{
								list2[i] = itemToAdd;
								Barter.InitializeInternal(oldShopItemController, component);
								return oldShopItemController;
							}
						}
					}
				}
			}
			return null;
		}

		private static void InitializeInternal(ShopItemController sic, PickupObject i)
		{
			BaseShopController baseShopController = Tools.ReflectGetField<BaseShopController>(typeof(ShopItemController), "m_baseParentShop", sic);
			ShopController shopController = Tools.ReflectGetField<ShopController>(typeof(ShopItemController), "m_parentShop", sic);
			sic.item = i;
			bool flag = i is SpecialKeyItem && (i as SpecialKeyItem).keyType == 0;
			if (flag)
			{
				sic.IsResourcefulRatKey = true;
			}
			bool flag2 = sic.item && sic.item.encounterTrackable;
			if (flag2)
			{
				GameStatsManager.Instance.SingleIncrementDifferentiator(sic.item.encounterTrackable);
			}
			sic.CurrentPrice = sic.item.PurchasePrice;
			bool flag3 = baseShopController != null && baseShopController.baseShopType == BaseShopController.AdditionalShopType.KEY;
			if (flag3)
			{
				sic.CurrentPrice = 1;
				bool flag4 = sic.item.quality == ItemQuality.A;
				if (flag4)
				{
					sic.CurrentPrice = 2;
				}
				bool flag5 = sic.item.quality == ItemQuality.S;
				if (flag5)
				{
					sic.CurrentPrice = 3;
				}
			}
			bool flag6 = baseShopController != null && baseShopController.baseShopType == null && (sic.item is BankMaskItem || sic.item is BankBagItem || sic.item is PaydayDrillItem);
			if (flag6)
			{
				EncounterTrackable encounterTrackable = sic.item.encounterTrackable;
				bool flag7 = encounterTrackable && !encounterTrackable.PrerequisitesMet();
				if (flag7)
				{
					bool flag8 = sic.item is BankMaskItem;
					if (flag8)
					{
						sic.SetsFlagOnSteal = true;
						sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_BANKMASK;
					}
					else
					{
						bool flag9 = sic.item is BankBagItem;
						if (flag9)
						{
							sic.SetsFlagOnSteal = true;
							sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_BANKBAG;
						}
						else
						{
							bool flag10 = sic.item is PaydayDrillItem;
							if (flag10)
							{
								sic.SetsFlagOnSteal = true;
								sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_DRILL;
							}
						}
					}
					sic.OverridePrice = new int?(9999);
				}
			}
			GameObjectExtensions.GetOrAddComponent<tk2dSprite>(sic.gameObject);
			tk2dSprite tk2dSprite = i.GetComponent<tk2dSprite>();
			bool flag11 = tk2dSprite == null;
			if (flag11)
			{
				tk2dSprite = i.GetComponentInChildren<tk2dSprite>();
			}
			sic.sprite.SetSprite(tk2dSprite.Collection, tk2dSprite.spriteId);
			sic.sprite.IsPerpendicular = true;
			bool useOmnidirectionalItemFacing = sic.UseOmnidirectionalItemFacing;
			if (useOmnidirectionalItemFacing)
			{
				sic.sprite.IsPerpendicular = false;
			}
			sic.sprite.HeightOffGround = 1f;
			bool flag12 = shopController != null;
			if (flag12)
			{
				bool flag13 = shopController is MetaShopController;
				if (flag13)
				{
					sic.UseOmnidirectionalItemFacing = true;
					sic.sprite.IsPerpendicular = false;
				}
				sic.sprite.HeightOffGround += shopController.ItemHeightOffGroundModifier;
			}
			else
			{
				bool flag14 = baseShopController.baseShopType == BaseShopController.AdditionalShopType.BLACKSMITH;
				if (flag14)
				{
					sic.UseOmnidirectionalItemFacing = true;
				}
				else
				{
					bool flag15 = baseShopController.baseShopType == BaseShopController.AdditionalShopType.TRUCK || baseShopController.baseShopType == BaseShopController.AdditionalShopType.GOOP || baseShopController.baseShopType == BaseShopController.AdditionalShopType.CURSE || baseShopController.baseShopType == BaseShopController.AdditionalShopType.BLANK || baseShopController.baseShopType == BaseShopController.AdditionalShopType.KEY || baseShopController.baseShopType == BaseShopController.AdditionalShopType.RESRAT_SHORTCUT;
					if (flag15)
					{
						sic.UseOmnidirectionalItemFacing = true;
					}
				}
			}
			sic.sprite.PlaceAtPositionByAnchor(sic.transform.parent.position, tk2dBaseSprite.Anchor.MiddleCenter);
			sic.sprite.transform.position = dfVectorExtensions.Quantize(sic.sprite.transform.position, 0.0625f);
			DepthLookupManager.ProcessRenderer(sic.sprite.renderer);
			tk2dSprite componentInParent = sic.transform.parent.gameObject.GetComponentInParent<tk2dSprite>();
			bool flag16 = componentInParent != null;
			if (flag16)
			{
				componentInParent.AttachRenderer(sic.sprite);
			}
			SpriteOutlineManager.AddOutlineToSprite(sic.sprite, Color.black, 0.1f, 0.05f, 0);
			GameObject gameObject = null;
			bool flag17 = shopController != null && shopController.shopItemShadowPrefab != null;
			if (flag17)
			{
				gameObject = shopController.shopItemShadowPrefab;
			}
			bool flag18 = baseShopController != null && baseShopController.shopItemShadowPrefab != null;
			if (flag18)
			{
				gameObject = baseShopController.shopItemShadowPrefab;
			}
			bool flag19 = gameObject != null;
			if (flag19)
			{
				GameObject gameObject2 = Tools.ReflectGetField<GameObject>(typeof(ShopItemController), "m_shadowObject", sic);
				bool flag20 = !gameObject2;
				if (flag20)
				{
					gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
					Tools.ReflectSetField<GameObject>(typeof(ShopItemController), "m_shadowObject", gameObject2, sic);
				}
				tk2dBaseSprite component = gameObject2.GetComponent<tk2dBaseSprite>();
				component.PlaceAtPositionByAnchor(sic.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				component.transform.position = dfVectorExtensions.Quantize(component.transform.position, 0.0625f);
				sic.sprite.AttachRenderer(component);
				component.transform.parent = sic.sprite.transform;
				component.HeightOffGround = -0.5f;
				bool flag21 = shopController is MetaShopController;
				if (flag21)
				{
					component.HeightOffGround = -0.0625f;
				}
			}
			sic.sprite.UpdateZDepth();
			SpeculativeRigidbody orAddComponent = GameObjectExtensions.GetOrAddComponent<SpeculativeRigidbody>(sic.gameObject);
			orAddComponent.PixelColliders = new List<PixelCollider>();
			PixelCollider pixelCollider = new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Circle,
				CollisionLayer = CollisionLayer.HighObstacle,
				ManualDiameter = 14
			};
			Vector2 vector = sic.sprite.WorldCenter - Vector3Extensions.XY(sic.transform.position);
			pixelCollider.ManualOffsetX = PhysicsEngine.UnitToPixel(vector.x) - 7;
			pixelCollider.ManualOffsetY = PhysicsEngine.UnitToPixel(vector.y) - 7;
			orAddComponent.PixelColliders.Add(pixelCollider);
			orAddComponent.Initialize();
			orAddComponent.OnPreRigidbodyCollision = null;
			sic.RegenerateCache();
			bool flag22 = !GameManager.Instance.IsFoyer && sic.item is Gun && GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns;
			if (flag22)
			{
				sic.ForceOutOfStock();
			}
		}
		// Token: 0x060000A6 RID: 166 RVA: 0x0000793C File Offset: 0x00005B3C
		public override bool CanBeUsed(PlayerController user)
		{
			return user.carriedConsumables.Currency >= 15;
		}

	}
}
