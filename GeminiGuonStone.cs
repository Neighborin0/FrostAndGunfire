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
    class GeminiGuonStone : IounStoneOrbitalItem
    {

        public static Hook guonHook;
        public static PlayerOrbital orbitalPrefab;
      

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            string itemName = "Gemini Guon Stone"; 
            string resourceName = "FrostAndGunfireItems/Resources/gemini"; 

            GameObject obj = new GameObject();

            var item = obj.AddComponent<GeminiGuonStone>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Insert Text";
            string longDesc = "Text";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
            item.quality = PickupObject.ItemQuality.EXCLUDED;

            BuildPrefab();
            BuildPrefab2();
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
                GameObject gameObject = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/gemini_white", null, true);
                gameObject.name = "Pale Guon";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(15, 15));
                orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                orbitalPrefab.shouldRotate = false;
                orbitalPrefab.orbitRadius = 4f;
                orbitalPrefab.orbitDegreesPerSecond = 90f;
                orbitalPrefab.orbitDegreesPerSecond = 200f;
                orbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }

        public static void BuildPrefab2()
        {
            bool flag = orbitalPrefab != null;
            bool flag2 = !flag;
            bool flag3 = flag2;
            bool flag4 = flag3;
            if (flag4)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/gemini_black", null, true);
                gameObject.name = "Black Guon";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(15, 15));
                orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                orbitalPrefab.shouldRotate = false;
                orbitalPrefab.orbitRadius = 0.8f;
                orbitalPrefab.orbitDegreesPerSecond = 90f;
                orbitalPrefab.orbitDegreesPerSecond = 30f;
                orbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            GeminiGuonStone.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(GeminiGuonStone).GetMethod("GuonInit"));
            bool flag = player.gameObject.GetComponent<GeminiGuonBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                player.gameObject.GetComponent<GeminiGuonBehavior>().Destroy();
            }
            player.gameObject.AddComponent<GeminiGuonBehavior>();
            GameManager.Instance.OnNewLevelFullyLoaded += this.FixGuon;
        }

    

        // Token: 0x06000079 RID: 121 RVA: 0x000073D8 File Offset: 0x000055D8
        public override DebrisObject Drop(PlayerController player)
        {
            player.GetComponent<GeminiGuonBehavior>().Destroy();
            GeminiGuonStone.guonHook.Dispose();
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            return base.Drop(player);
        }
      

        private void FixGuon()
        {
            bool flag = base.Owner && base.Owner.GetComponent<GeminiGuonStone>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<GeminiGuonBehavior>().Destroy();
            }
            PlayerController owner = base.Owner;
            owner.gameObject.AddComponent<GeminiGuonBehavior>();
        }


        protected override void OnDestroy()
        {
            GeminiGuonStone.guonHook.Dispose();
            bool flag = base.Owner && base.Owner.GetComponent<GeminiGuonBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<GeminiGuonBehavior>().Destroy();
            }
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            base.OnDestroy();
        }

        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            
            orig(self, player);
        }

        private class GeminiGuonBehavior : BraveBehaviour
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
