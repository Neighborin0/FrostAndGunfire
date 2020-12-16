using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{

    public class SpookyBullets : PassiveItem
    {
        



        public static void Init()
        {
            string name = "Cursed Cylinder";
            string resourcePath = "FrostAndGunfireItems/Resources/spooky_bullets";
            GameObject gameObject = new GameObject(name);
            SpookyBullets spookyBullets = gameObject.AddComponent<SpookyBullets>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
            string shortDesc = "2sp00k3y";
            string longDesc = "Nearby enemies are feared when reloading.\n\nWhen reloading, this cylinder emits a high frequency screech that only the Gundead can hear. It reminds them of the fabled Bullet Hell.";
            ItemBuilder.SetupItem(spookyBullets, shortDesc, longDesc, "kp");
            spookyBullets.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(spookyBullets, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }


        public override void Pickup(PlayerController player)
        {
            bool flag = this.fleeData == null || this.fleeData.Player != player;
            if (flag)
            {
                this.fleeData = new FleePlayerData();
                this.fleeData.Player = player;
                this.fleeData.StartDistance = 9f;
            }
            player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Combine(player.OnReloadedGun, new Action<PlayerController, Gun>(this.DoEffect));
            base.Pickup(player);
        }


        private void DoEffect(PlayerController user, Gun gun)
        {
            this.InternalCooldown = 10f;
            bool flag = Time.realtimeSinceStartup - this.m_lastUsedTime < this.InternalCooldown;
            if (!flag)
            {
                this.m_lastUsedTime = Time.realtimeSinceStartup;
                this.HandleFear(user, true);
            }
            
        }






        private void HandleFear(PlayerController user, bool active)
        {
            
            RoomHandler currentRoom = user.CurrentRoom;
            bool flag = !currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag2 = !flag;
            if (flag2)
            {
                if (active)
                {
                    foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                    {
                        bool flag3 = aiactor.behaviorSpeculator != null;
                        bool flag4 = flag3;
                        if (flag4)
                        {
                            aiactor.behaviorSpeculator.FleePlayerData = this.fleeData;
                            FleePlayerData fleePlayerData = new FleePlayerData();
                            base.StartCoroutine(this.RemoveFear(aiactor));
                        }
                    }
                }
                else
                {
                    foreach(AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                    {
                        bool flag3 = aiactor.behaviorSpeculator != null;
                        bool flag4 = flag3;
                        if (flag4)
                        {
                            aiactor.behaviorSpeculator.FleePlayerData = this.fleeData;
                            FleePlayerData fleePlayerData = new FleePlayerData();
                            base.StartCoroutine(this.RemoveFear(aiactor));
                        }
                    }
                }
            }
        }

        private IEnumerator RemoveFear(AIActor aiactor)
        {
            yield return new WaitForSeconds(3.5f);
            aiactor.behaviorSpeculator.FleePlayerData = null;
            yield break;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            SpookyBullets component = debrisObject.GetComponent<SpookyBullets>();
            component.m_pickedUpThisRun = true;
            return debrisObject;
        }

        private FleePlayerData fleeData;

       

        public float InternalCooldown;

     

        private float m_lastUsedTime;
    
    }
}
