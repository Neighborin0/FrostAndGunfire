using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GungeonAPI;
using MonoMod.RuntimeDetour;

namespace FrostAndGunfireItems
{
	public class Pods : IounStoneOrbitalItem
    {
        public static Hook guonHook;
        public static PlayerOrbital orbitalPrefab;
        private int everyothershot;
        private float fakehp;
        public static void Init()
		{
            string itemName = "Pods";
            string resourceName = "FrostAndGunfireItems/Resources/pod_thing";

            GameObject obj = new GameObject();

            var item = obj.AddComponent<Pods>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Back At Ya!";
            string longDesc = "The strong reinforced glass in this guon stone allows it to reflect bullets.\n\nOne of the many treasures in Lonk's possesion, lost when wandering the Gungeon.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
            item.CanBeDropped = false;
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;
            item.Identifier = IounStoneIdentifier.GENERIC;

        }
        public static void BuildPrefab()
        {
            bool flag = orbitalPrefab != null;
            bool flag2 = !flag;
            if (flag2)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("FrostAndGunfireItems/Resources/pod_thing", null, true);
                gameObject.name = "Plant Pod";     
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(11, 13));
                orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
               // speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyCollider;
                orbitalPrefab.shouldRotate = false;
                orbitalPrefab.orbitRadius = 2f;
                orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
                orbitalPrefab.orbitDegreesPerSecond = 90f;
                orbitalPrefab.orbitDegreesPerSecond = 180f;
                orbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }

        

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            if (player.CurrentItem != null)
            {
                if (player.CurrentItem.timeCooldown != 0)
                {
                    fakehp = player.CurrentItem.timeCooldown / 20;
                }
                if (player.CurrentItem.roomCooldown != 0)
                {
                    fakehp = player.CurrentItem.roomCooldown * 2;
                }
                if (player.CurrentItem.damageCooldown != 0)
                {
                    fakehp = player.CurrentItem.damageCooldown / 40;
                }
            }
            else
            {
                fakehp = 4;
            }
            Tools.PrintNoID(fakehp);
            Pods.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(Pods).GetMethod("GuonInit"));
            if (player.gameObject.GetComponent<Pods.PodGuonBehavior>() != null)
            {
                player.gameObject.GetComponent<Pods.PodGuonBehavior>().Destroy();
            }
            bool flag3 = this.m_extantOrbital != null;
            if (flag3)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollison));
            }
            player.gameObject.AddComponent<Pods.PodGuonBehavior>();
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.OnUsedPlayerItem += SpawnPod;
            GameManager.Instance.OnNewLevelFullyLoaded += this.FixGuon;       
        }

        private void SpawnPod(PlayerController arg1, PlayerItem arg2)
        {  
            var pod = Gungeon.Game.Items["kp:pods"];
            arg1.AcquirePassiveItemPrefabDirectly(pod as PassiveItem);
            arg1.OnUsedPlayerItem -= SpawnPod;
        }

        protected override void OnDestroy()
        {
            Owner.PostProcessProjectile -= this.PostProcessProjectile;
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            base.OnDestroy();
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.GetComponent<PodGuonBehavior>().Destroy();
            Pods.guonHook.Dispose();
            bool flag = this.m_extantOrbital != null;
            if (flag)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollison));
            }
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.OnUsedPlayerItem -= SpawnPod;
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            return base.Drop(player);
        }

        private void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {
            everyothershot += 1;
            if(everyothershot == 2)
            {
                everyothershot -= everyothershot;
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[Owner.CurrentGun.PickupObjectId]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, m_extantOrbital.transform.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    PostProcessProjectile(component, 1f);
                    component.Owner = base.Owner;
                    component.Shooter = base.Owner.specRigidbody;
                    HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
                    homingModifier.HomingRadius = 7f;
                    homingModifier.AngularVelocity = 2f;
                    component.baseData.damage *= 0.5f;
                    component.baseData.speed *= 0.7f;
                    component.specRigidbody.CollideWithTileMap = false;
                }
            }
        }


        private void FixGuon()
        {
            bool flag = base.Owner && base.Owner.GetComponent<PodGuonBehavior>() != null;
            bool flag2 = flag;
            if (flag2)
            {
                base.Owner.GetComponent<PodGuonBehavior>().Destroy();
            }
            if (this.m_extantOrbital != null)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(OnPreCollison));
            }
            PlayerController owner = base.Owner;
            owner.gameObject.AddComponent<PodGuonBehavior>();
        }

        private void OnPreCollison(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
         
            bool flag = Owner != null;
            if (flag)
            {
                fakehp -= 1;
                if(fakehp <= 0)
                {
                    var pod = Gungeon.Game.Items["kp:pods"].PickupObjectId;
                    Owner.RemovePassiveItem(pod);
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
        }

        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            orig(self, player);
        }

        private class PodGuonBehavior : BraveBehaviour
        {
            PlayerController owner;

            void Start()
            {         this.owner = base.GetComponent<PlayerController>();

            }






            public void Destroy()
            {
                UnityEngine.Object.Destroy(this);
            }
        }
    }
}

