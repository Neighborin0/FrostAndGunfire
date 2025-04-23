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
    class BulletSponge2 : IounStoneOrbitalItem
    {

        public static Hook guonHook;
        public static PlayerOrbital orbitalPrefab;
        int NumOfBulletsHitWith;
        public static int[] spriteIDs;
        private static readonly string[] spritePaths = new string[]
    {
            "FrostAndGunfireItems/Resources/bullet_sponge_idle_001",
              "FrostAndGunfireItems/Resources/bullet_sponge_002",
                "FrostAndGunfireItems/Resources/bullet_sponge_003",
    };
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            string itemName = "Bullet Sponge";
            string resourceName = "FrostAndGunfireItems/Resources/bullet_sponge_idle_001";

            GameObject obj = new GameObject();

            var item = obj.AddComponent<BulletSponge2>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Extra Absorbent";
            string longDesc = "A creature hailing from the shoals surrounding the Gungeon, The Bullet Sponge is adept at taking in nearby materials and expelling them at high speed.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
            item.quality = PickupObject.ItemQuality.C;

            BuildPrefab();
            BulletSponge2.spriteIDs = new int[BulletSponge2.spritePaths.Length];
            BulletSponge2.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(BulletSponge2.spritePaths[0], item.sprite.Collection);
            BulletSponge2.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(BulletSponge2.spritePaths[1], item.sprite.Collection);
            BulletSponge2.spriteIDs[2] = SpriteBuilder.AddSpriteToCollection(BulletSponge2.spritePaths[2], item.sprite.Collection);
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            item.OrbitalPrefab = orbitalPrefab;
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
                gameObject.name = "Bullet Sponge";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(13, 18));
                orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                orbitalPrefab.shouldRotate = false;
                orbitalPrefab.orbitRadius = 2.5f;
                orbitalPrefab.orbitDegreesPerSecond = 90f;
                orbitalPrefab.orbitDegreesPerSecond = 180f;
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
            guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(BulletSponge2).GetMethod("GuonInit"));
            bool flag = player.gameObject.GetComponent<SpongeBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                player.gameObject.GetComponent<SpongeBehavior>().Destroy();
            }
            player.gameObject.AddComponent<SpongeBehavior>();
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
                    if (NumOfBulletsHitWith >= 7)
                    {
                        myRigidbody.sprite.SetSprite(BulletSponge2.spritePaths[2]);
                    }
                    else if (NumOfBulletsHitWith >= 3 && NumOfBulletsHitWith <= 7)
                    {
                        myRigidbody.sprite.SetSprite(BulletSponge2.spritePaths[1]);
                    }
                    else
                    {
                        myRigidbody.sprite.SetSprite(BulletSponge2.spritePaths[0]);
                    }

                    if (NumOfBulletsHitWith == 10)
                    {
                        AkSoundEngine.PostEvent("Play_BOSS_blobulord_burst_01", base.gameObject);
                            for (int i = 0; i <= 16; i++)
                        {
                            if (Owner.HasPickupID(452))
                            {
                                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[207]).DefaultModule.projectiles[0];
                                GameObject gameObject1 = SpawnManager.SpawnProjectile(projectile2.gameObject, myRigidbody.transform.position, Quaternion.Euler(0f, 0f, 22.5f * i), true);
                                Projectile component1 = gameObject1.GetComponent<Projectile>();
                                if (component1 != null)
                                {
                                    component1.Owner = Owner;
                                    component1.collidesWithPlayer = false;
                                }
                              
                            }
                            else
                            {
                                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[33]).DefaultModule.projectiles[0];
                                GameObject gameObject1 = SpawnManager.SpawnProjectile(projectile2.gameObject, myRigidbody.transform.position, Quaternion.Euler(0f, 0f, 22.5f * i), true);
                                Projectile component1 = gameObject1.GetComponent<Projectile>();
                                if (component1 != null)
                                {
                                    component1.Owner = Owner;
                                    component1.collidesWithPlayer = false;
                                }
                            }
                            NumOfBulletsHitWith = 0;
                            myRigidbody.sprite.SetSprite(BulletSponge2.spritePaths[0]);
                        }
                    }

                }
            }
        }

        // Token: 0x06000079 RID: 121 RVA: 0x000073D8 File Offset: 0x000055D8
        public override DebrisObject Drop(PlayerController player)
        {
            player.GetComponent<SpongeBehavior>().Destroy();
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
            bool flag = base.Owner && base.Owner.GetComponent<SpongeBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<SpongeBehavior>().Destroy();
            }
            bool flag3 = this.m_extantOrbital != null;
            if (flag3)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(OnPreCollison));
            }
            PlayerController owner = base.Owner;
            owner.gameObject.AddComponent<SpongeBehavior>();
        }


        protected override void OnDestroy()
        {
            guonHook.Dispose();
            bool flag = base.Owner && base.Owner.GetComponent<SpongeBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<SpongeBehavior>().Destroy();
            }
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            base.OnDestroy();
        }

        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {

            orig(self, player);
        }

        private class SpongeBehavior : BraveBehaviour
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
