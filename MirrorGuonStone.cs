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
    class MirrorGuon : IounStoneOrbitalItem
    {

        public static Hook guonHook;
        public static PlayerOrbital orbitalPrefab;

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            string itemName = "Mirror Guon Stone"; 
            string resourceName = "FrostAndGunfireItems/Resources/mirror"; 

            GameObject obj = new GameObject();

            var item = obj.AddComponent<MirrorGuon>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Back At Ya!";
            string longDesc = "The strong reinforced glass in this guon stone allows it to reflect bullets.\n\nOne of the many treasures in Lonk's possesion, lost when wandering the Gungeon.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
            item.quality = PickupObject.ItemQuality.A;

            BuildPrefab();
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
                GameObject gameObject = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/mirror", null, true);
                gameObject.name = "Mirror Guon";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(15, 15));
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
            MirrorGuon.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(MirrorGuon).GetMethod("GuonInit"));
            bool flag = player.gameObject.GetComponent<MirrorGuon.MirrorGuonBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                player.gameObject.GetComponent<MirrorGuon.MirrorGuonBehavior>().Destroy();
            }
            player.gameObject.AddComponent<MirrorGuon.MirrorGuonBehavior>();
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
                bool flag2 = component != null && !(component.Owner is PlayerController);
                if (flag2)
                {
                    PassiveReflectItem.ReflectBullet(component, true, Owner, 10f, 1f, 1f, 0f);
                    PhysicsEngine.SkipCollision = true;
                }
            }
        }

        // Token: 0x06000079 RID: 121 RVA: 0x000073D8 File Offset: 0x000055D8
        public override DebrisObject Drop(PlayerController player)
        {
            player.GetComponent<MirrorGuon.MirrorGuonBehavior>().Destroy();
            MirrorGuon.guonHook.Dispose();
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
            bool flag = base.Owner && base.Owner.GetComponent<MirrorGuon.MirrorGuonBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<MirrorGuon.MirrorGuonBehavior>().Destroy();
            }
            bool flag3 = this.m_extantOrbital != null;
            if (flag3)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(OnPreCollison));
            }
            PlayerController owner = base.Owner;
            owner.gameObject.AddComponent<MirrorGuon.MirrorGuonBehavior>();
        }


        protected override void OnDestroy()
        {
            MirrorGuon.guonHook.Dispose();
            bool flag = base.Owner && base.Owner.GetComponent<MirrorGuon.MirrorGuonBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<MirrorGuon.MirrorGuonBehavior>().Destroy();
            }
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            base.OnDestroy();
        }

        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            
            orig(self, player);
        }

        private class MirrorGuonBehavior : BraveBehaviour
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
