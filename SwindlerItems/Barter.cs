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
            Barter barter = gameObject.AddComponent<Barter>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
            string shortDesc = "The Art of The Deal";
            string longDesc = "Allows shops to be rerolled at the cost of 15 casings.\n\nThe Swindler was always able to talk his way into a good deal, unfortunately he couldn't talk himself out of fights...";
            barter.SetupItem(shortDesc, longDesc, "kp");
            barter.SetCooldownType(ItemBuilder.CooldownType.Damage, 0f);
            barter.consumable = false;
            barter.IgnoredByRat = true;
            barter.quality = PickupObject.ItemQuality.A;
            barter.SetupUnlockOnCustomFlag(CustomDungeonFlags.DRAGUN_KILLED_AND_SWINDLER, true);
        }

        protected override void DoEffect(PlayerController user)
        {
            user.carriedConsumables.Currency -= 15;
            AkSoundEngine.PostEvent("Play_OBJ_coin_medium_01", base.gameObject);
            if (StaticReferenceManager.AllShops != null)
            {
                foreach (BaseShopController baseShopController in StaticReferenceManager.AllShops)
                {
                    List<ShopItemController> list = Tools.ReflectGetField<List<ShopItemController>>(typeof(BaseShopController), "m_itemControllers", baseShopController);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list != null)
                        {
                            ShopItemController shopItemController = list[i];
                            if (shopItemController != null && shopItemController.item.PickupObjectId != 224 && !shopItemController.IsResourcefulRatKey && shopItemController.item != null && list != null)
                            {
                                PickupObject item = shopItemController.item;
                                GameObject gameObject = null;
                                if (item is PassiveItem)
                                {
                                    gameObject = GameManager.Instance.RewardManager.ItemsLootTable.SelectByWeightWithoutDuplicates(this.ItemsAlreadySelected, true);
                                    if (gameObject != null)
                                    {
                                        this.ItemsAlreadySelected.Add(gameObject);
                                    }
                                }
                                else if (item is PlayerItem)
                                {
                                    gameObject = GameManager.Instance.RewardManager.ItemsLootTable.SelectByWeightWithoutDuplicates(this.ItemsAlreadySelected, true);
                                    if (gameObject != null)
                                    {
                                        this.ItemsAlreadySelected.Add(gameObject);
                                    }
                                }
                                else if (item is Gun)
                                {
                                    gameObject = GameManager.Instance.RewardManager.GunsLootTable.SelectByWeightWithoutDuplicates(this.ItemsAlreadySelected, true);
                                    if (gameObject != null)
                                    {
                                        this.ItemsAlreadySelected.Add(gameObject);
                                    }
                                }
                                if (gameObject != null)
                                {
                                    ShopItemController shopItemController2 = this.ReplaceShopItem(baseShopController, shopItemController, gameObject);
                                    if (shopItemController2 != null)
                                    {
                                        LootEngine.DoDefaultItemPoof(shopItemController2.sprite.WorldCenter, false, false);
                                    }
                                }
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
                if (gameObject != null)
                {
                    PickupObject component2 = gameObject.GetComponent<PickupObject>();
                    if (component2 != null && component2 != oldShopItemController.item)
                    {
                        Transform parent = oldShopItemController.gameObject.transform.parent;
                        if (parent != null && component.quality != PickupObject.ItemQuality.EXCLUDED && component.quality != PickupObject.ItemQuality.SPECIAL && component.PickupObjectId != 670)
                        {
                            list2[i] = itemToAdd;
                            Barter.InitializeInternal(oldShopItemController, component);
                            return oldShopItemController;
                        }
                    }
                }
            }
            return null;
        }

        public static void InitializeInternal(ShopItemController sic, PickupObject i)
        {
            BaseShopController baseShopController = Tools.ReflectGetField<BaseShopController>(typeof(ShopItemController), "m_baseParentShop", sic);
            ShopController shopController = Tools.ReflectGetField<ShopController>(typeof(ShopItemController), "m_parentShop", sic);
            sic.item = i;
            if (i is SpecialKeyItem && (i as SpecialKeyItem).keyType == SpecialKeyItem.SpecialKeyType.RESOURCEFUL_RAT_LAIR)
            {
                sic.IsResourcefulRatKey = true;
            }
            if (sic.item && sic.item.encounterTrackable)
            {
                GameStatsManager.Instance.SingleIncrementDifferentiator(sic.item.encounterTrackable);
            }
            sic.CurrentPrice = (int)Math.Round((double)((float)sic.item.PurchasePrice * 1.05f));
            if (baseShopController != null && baseShopController.baseShopType == BaseShopController.AdditionalShopType.KEY)
            {
                sic.CurrentPrice = 1;
                if (sic.item.quality == PickupObject.ItemQuality.A)
                {
                    sic.CurrentPrice = 2;
                }
                if (sic.item.quality == PickupObject.ItemQuality.S)
                {
                    sic.CurrentPrice = 3;
                }
            }
            if (baseShopController != null && (sic.item is BankMaskItem || sic.item is BankBagItem || sic.item is PaydayDrillItem))
            {
                EncounterTrackable encounterTrackable = sic.item.encounterTrackable;
                bool flag = encounterTrackable && !encounterTrackable.PrerequisitesMet();
                if (flag)
                {
                    if (sic.item is BankMaskItem)
                    {
                        sic.SetsFlagOnSteal = true;
                        sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_BANKMASK;
                    }
                    else if (sic.item is BankBagItem)
                    {
                        sic.SetsFlagOnSteal = true;
                        sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_BANKBAG;
                    }
                    else if (sic.item is PaydayDrillItem)
                    {
                        sic.SetsFlagOnSteal = true;
                        sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_DRILL;
                    }
                    sic.OverridePrice = new int?(9999);
                }
            }
            sic.gameObject.GetOrAddComponent<tk2dSprite>();
            tk2dSprite tk2dSprite = i.GetComponent<tk2dSprite>();
            if (tk2dSprite == null)
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
            if (shopController != null)
            {
                if (shopController is MetaShopController)
                {
                    sic.UseOmnidirectionalItemFacing = true;
                    sic.sprite.IsPerpendicular = false;
                }
                sic.sprite.HeightOffGround += shopController.ItemHeightOffGroundModifier;
            }
            else if (baseShopController.baseShopType == BaseShopController.AdditionalShopType.BLACKSMITH)
            {
                sic.UseOmnidirectionalItemFacing = true;
            }
            else if (baseShopController.baseShopType == BaseShopController.AdditionalShopType.TRUCK || baseShopController.baseShopType == BaseShopController.AdditionalShopType.GOOP || baseShopController.baseShopType == BaseShopController.AdditionalShopType.CURSE || baseShopController.baseShopType == BaseShopController.AdditionalShopType.BLANK || baseShopController.baseShopType == BaseShopController.AdditionalShopType.KEY || baseShopController.baseShopType == BaseShopController.AdditionalShopType.RESRAT_SHORTCUT)
            {
                sic.UseOmnidirectionalItemFacing = true;
            }
            sic.sprite.PlaceAtPositionByAnchor(sic.transform.parent.position, tk2dBaseSprite.Anchor.MiddleCenter);
            sic.sprite.transform.position = sic.sprite.transform.position.Quantize(0.0625f);
            DepthLookupManager.ProcessRenderer(sic.sprite.renderer);
            tk2dSprite componentInParent = sic.transform.parent.gameObject.GetComponentInParent<tk2dSprite>();
            if (componentInParent != null)
            {
                componentInParent.AttachRenderer(sic.sprite);
            }
            SpriteOutlineManager.AddOutlineToSprite(sic.sprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
            GameObject gameObject = null;
            if (shopController != null && shopController.shopItemShadowPrefab != null)
            {
                gameObject = shopController.shopItemShadowPrefab;
            }
            if (baseShopController != null && baseShopController.shopItemShadowPrefab != null)
            {
                gameObject = baseShopController.shopItemShadowPrefab;
            }
            if (gameObject != null)
            {
                GameObject gameObject2 = Tools.ReflectGetField<GameObject>(typeof(ShopItemController), "m_shadowObject", sic);
                if (!gameObject2)
                {
                    gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    Tools.ReflectSetField<GameObject>(typeof(ShopItemController), "m_shadowObject", gameObject2, sic);
                }
                tk2dBaseSprite component = gameObject2.GetComponent<tk2dBaseSprite>();
                component.PlaceAtPositionByAnchor(sic.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                component.transform.position = component.transform.position.Quantize(0.0625f);
                sic.sprite.AttachRenderer(component);
                component.transform.parent = sic.sprite.transform;
                component.HeightOffGround = -0.5f;
                if (shopController is MetaShopController)
                {
                    component.HeightOffGround = -0.0625f;
                }
            }
            sic.sprite.UpdateZDepth();
            SpeculativeRigidbody orAddComponent = sic.gameObject.GetOrAddComponent<SpeculativeRigidbody>();
            orAddComponent.PixelColliders = new List<PixelCollider>();
            PixelCollider pixelCollider = new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Circle,
                CollisionLayer = CollisionLayer.HighObstacle,
                ManualDiameter = 14
            };
            Vector2 vector = sic.sprite.WorldCenter - sic.transform.position.XY();
            pixelCollider.ManualOffsetX = PhysicsEngine.UnitToPixel(vector.x) - 7;
            pixelCollider.ManualOffsetY = PhysicsEngine.UnitToPixel(vector.y) - 7;
            orAddComponent.PixelColliders.Add(pixelCollider);
            orAddComponent.Initialize();
            orAddComponent.OnPreRigidbodyCollision = null;
            sic.RegenerateCache();
            if (!GameManager.Instance.IsFoyer && sic.item is Gun && GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns)
            {
                sic.ForceOutOfStock();
            }
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.carriedConsumables.Currency >= 15 && user.CurrentRoom.IsShop && user.CurrentRoom != null && user.carriedConsumables != null;
        }
    }
}
