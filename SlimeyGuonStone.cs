using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using MonoMod.RuntimeDetour;
using ItemAPI;
using GungeonAPI;



namespace FrostAndGunfireItems
{
    class SlimeyGuonStone : IounStoneOrbitalItem
    {

        public static Hook guonHook;
        public static PlayerOrbital orbitalPrefab;
        int NumOfBulletsHitWith;
     
        public static void Init()
        {
            string itemName = "Slimey Guon Stone";
            string resourceName = "FrostAndGunfireItems/Resources/bullet_sponge_idle_001";

            GameObject obj = new GameObject();

            var item = obj.AddComponent<SlimeyGuonStone>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Extra Absorbent";
            string longDesc = "A creature hailing from the shoals surrounding the Gungeon, The Bullet Sponge is adept at taking in nearby materials and expelling them at high speed.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
            item.quality = PickupObject.ItemQuality.C;
            BuildPrefab();
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            item.OrbitalPrefab = orbitalPrefab;
            item.CanBeDropped = false;
            item.Identifier = IounStoneIdentifier.GENERIC;
        }


        public static void BuildPrefab()
        {
            bool flag = orbitalPrefab != null;
            bool flag2 = !flag;
            bool flag3 = flag2;
            bool flag4 = flag3;
            if (flag4)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/bullet_sponge_idle_001", null, true);
                gameObject.name = "Slimey Guon";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(15, 15));
                orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                orbitalPrefab.shouldRotate = false;
                orbitalPrefab.orbitRadius = 2.5f;
                orbitalPrefab.orbitDegreesPerSecond = 90f;
                orbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }
        // Token: 0x06000077 RID: 119 RVA: 0x0000731C File Offset: 0x0000551C
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(SlimeyGuonStone).GetMethod("GuonInit"));
            bool flag = player.gameObject.GetComponent<SlimeBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                player.gameObject.GetComponent<SlimeBehavior>().Destroy();
            }
            player.gameObject.AddComponent<SlimeBehavior>();
            GameManager.Instance.OnNewLevelFullyLoaded += this.FixGuon;
            bool flag3 = this.m_extantOrbital != null;
            if (flag3)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollison));
            }
        }
      
        // Token: 0x06000078 RID: 120 RVA: 0x00007378 File Offset: 0x00005578
        private void OnPreCollison(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            bool flag = Owner != null;
            if (flag)
            {
                Projectile component = other.GetComponent<Projectile>();
                if (NumOfBulletsHitWith <= 10 & component != null)
                {
                    NumOfBulletsHitWith++;
                    if (NumOfBulletsHitWith == 10 & component != null)
                    {
                        AkSoundEngine.PostEvent("Play_BOSS_blobulord_burst_01", base.gameObject);
                        for (int i = 0; i <= 5; i++)
                        {
                            PlayerController player = GameManager.Instance.PrimaryPlayer;
                            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("42be66373a3d4d89b91a35c9ff8adfec");
                            AIActor aiactor = AIActor.Spawn(orLoadByGuid, new Vector3(myRigidbody.transform.position.x + i, myRigidbody.transform.position.y, myRigidbody.transform.position.z), player.gameObject.transform.position.GetAbsoluteRoom(), true, AIActor.AwakenAnimationType.Awaken, true);
                            CompanionController orAddComponent = aiactor.gameObject.GetOrAddComponent<CompanionController>();
                            orAddComponent.companionID = CompanionController.CompanionIdentifier.NONE;
                            orAddComponent.Initialize(Owner);
                            //aiactor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                            aiactor.specRigidbody.CollideWithOthers = true;
                            aiactor.specRigidbody.CollideWithTileMap = false;
                            aiactor.specRigidbody.enabled = true;
                            aiactor.specRigidbody.Reinitialize();
                            aiactor.IgnoreForRoomClear = true;
                            aiactor.CollisionDamage = 10f;
                            //aiactor.IsHarmlessEnemy = true;
                            aiactor.CanTargetEnemies = true;
                            aiactor.CanTargetPlayers = false;
                            aiactor.RegisterOverrideColor(new Color32(255, 192, 203, 220), "Fren :)");
                        }
                        Destroy(myRigidbody.gameObject);
                        NumOfBulletsHitWith = 0;
                    }
                }
               
            }
        }

        // Token: 0x06000079 RID: 121 RVA: 0x000073D8 File Offset: 0x000055D8
        public override DebrisObject Drop(PlayerController player)
        {
            player.GetComponent<SlimeBehavior>().Destroy();
            guonHook.Dispose();
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            bool flag = this.m_extantOrbital != null;
            if (flag)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollison));
            }
            return base.Drop(player);
        }


        private void FixGuon()
        {
            bool flag = base.Owner && base.Owner.GetComponent<SlimeBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<SlimeBehavior>().Destroy();
            }
            bool flag3 = this.m_extantOrbital != null;
            if (flag3)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(OnPreCollison));
            }
            PlayerController owner = base.Owner;
            owner.gameObject.AddComponent<SlimeBehavior>();
        }


        protected override void OnDestroy()
        {
            guonHook.Dispose();
            bool flag = base.Owner && base.Owner.GetComponent<SlimeBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<SlimeBehavior>().Destroy();
            }
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            base.OnDestroy();
        }

        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {

            orig(self, player);
        }

        private class SlimeBehavior : BraveBehaviour
        {
            PlayerController owner;

            void Start()
            {
                this.owner = base.GetComponent<PlayerController>();

            }






            public void Destroy()
            {
                UnityEngine.Object.Destroy(this);
            }
        }
    }
}
