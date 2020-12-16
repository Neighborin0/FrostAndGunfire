using System;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000B RID: 11
	public class VoidBullets : PassiveItem
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00004570 File Offset: 0x00002770
		public static void Init()
		{
			string name = "Void Bullets";
			string resourcePath = "FrostAndGunfireItems/Resources/void";
			GameObject gameObject = new GameObject(name);
			VoidBullets voidBullets = gameObject.AddComponent<VoidBullets>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Downward Spiral";
			string longDesc = "Shots have a chance to shoot black holes.\n\nEach one of these bullets contain a mini-star, which is orbited by mini-planets, which have mini-people on it.The stars inside these bullets often collide in on themselves and cause black holes.";
			ItemBuilder.SetupItem(voidBullets, shortDesc, longDesc, "kp");
			voidBullets.quality = PickupObject.ItemQuality.A;
			
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000045C7 File Offset: 0x000027C7
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
		}

	
		private void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
		{
			if (Owner.HasPickupID(169) || Owner.HasPickupID(155))
			{

				effectChanceScalar = 1f;
				bool flag3 = UnityEngine.Random.value < this.extrablackholechance * effectChanceScalar;
				bool flag4 = flag3;
				if (flag4)
				{
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[169]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					bool flag5 = component != null;
					bool flag6 = flag5;
					if (flag6)
					{
						component.Owner = base.Owner;
						component.Shooter = base.Owner.specRigidbody;
						component.baseData.speed = 18f;
						component.baseData.damage = 5f;
					}
				}
			}
			else
			{
				effectChanceScalar = 1f;
				bool flag = UnityEngine.Random.value < this.blackholechance * effectChanceScalar;
				bool flag2 = flag;
				if (flag2)
				{
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[169]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					bool flag3 = component != null;
					bool flag4 = flag3;
					if (flag4)
					{
						component.Owner = base.Owner;
						component.Shooter = Owner.specRigidbody;
						component.baseData.speed = 18f;
						component.baseData.damage = 5f;
					}
				}
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000046F8 File Offset: 0x000028F8
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			VoidBullets component = debrisObject.GetComponent<VoidBullets>();
			player.PostProcessProjectile -= this.PostProcessProjectile;
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x0400000F RID: 15
		private float blackholechance = 0.01f;
		private float extrablackholechance = 0.02f;
	}
}
