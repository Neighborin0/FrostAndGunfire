using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000015 RID: 21
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
			spookyBullets.AddToSubShop(ItemBuilder.ShopType.Cursula);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006E70 File Offset: 0x00005070
		public override void Pickup(PlayerController player)
		{
			bool flag = this.fleeData == null || this.fleeData.Player != player;
			bool flag2 = flag;
			if (flag2)
			{
				this.fleeData = new FleePlayerData();
				this.fleeData.Player = player;
				this.fleeData.StartDistance = 9f;
			}
			player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Combine(player.OnReloadedGun, new Action<PlayerController, Gun>(this.DoEffect));
			base.Pickup(player);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00006EF4 File Offset: 0x000050F4
		private void DoEffect(PlayerController user, Gun gun)
		{
			this.InternalCooldown = 11f;
			bool flag = Time.realtimeSinceStartup - this.m_lastUsedTime < this.InternalCooldown;
			bool flag2 = !flag;
			if (flag2)
			{
				user.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Curse") as GameObject, Vector3.zero, true, false, false);
				this.m_lastUsedTime = Time.realtimeSinceStartup;
				this.HandleFear(user, true);
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00006F40 File Offset: 0x00005140
		private void HandleFear(PlayerController user, bool active)
		{
			RoomHandler currentRoom = user.CurrentRoom;
			bool flag = !currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
			bool flag2 = !flag;
			bool flag3 = flag2;
			if (flag3)
			{
				if (active)
				{
					foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
					{
						bool flag4 = aiactor.behaviorSpeculator != null;
						bool flag5 = flag4;
						bool flag6 = flag5;
						if (flag6)
						{
							aiactor.behaviorSpeculator.FleePlayerData = this.fleeData;
							FleePlayerData fleePlayerData = new FleePlayerData();
							base.StartCoroutine(this.RemoveFear(aiactor));
						}
					}
				}
				else
				{
					foreach (AIActor aiactor2 in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
					{
						bool flag7 = aiactor2.behaviorSpeculator != null;
						bool flag8 = flag7;
						bool flag9 = flag8;
						if (flag9)
						{
							aiactor2.behaviorSpeculator.FleePlayerData = this.fleeData;
							FleePlayerData fleePlayerData2 = new FleePlayerData();
							base.StartCoroutine(this.RemoveFear(aiactor2));
						}
					}
				}
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007094 File Offset: 0x00005294
		private IEnumerator RemoveFear(AIActor aiactor)
		{
				yield return new WaitForSeconds(3f);
				aiactor.behaviorSpeculator.FleePlayerData = null;
				yield break;
			
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000070AC File Offset: 0x000052AC
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Remove(player.OnReloadedGun, new Action<PlayerController, Gun>(this.DoEffect));
			SpookyBullets component = debrisObject.GetComponent<SpookyBullets>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x04000033 RID: 51
		private FleePlayerData fleeData;

		// Token: 0x04000034 RID: 52
		public float InternalCooldown;

		// Token: 0x04000035 RID: 53
		private float m_lastUsedTime;
	}
}
