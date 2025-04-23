using System;
using System.Collections;
using ItemAPI;
using UnityEngine;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	public class BurstBullets : PassiveItem
	{
		public float InternalCooldown = 2f;
		public float OtherCooldown;
		public RadialBurstInterface GoBurst;
		public static void Init()
		{
			string name = "Burst Bullets";
			string resourcePath = "FrostAndGunfireItems/Resources/burst";
			GameObject gameObject = new GameObject(name);
			BurstBullets item = gameObject.AddComponent<BurstBullets>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Bullets Go Boom?";
			string longDesc = "Gives the wielder a chance to fire a burst of bullets around themselves.\n\nThese bullets where overstuffed with gunpowder and have a chance to burst any moment.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.B;
			item.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

		}

		protected override void Update()
		{
			base.Update();
			if (this.m_owner != null && this.m_pickedUp)
			{
				OtherCooldown -= BraveTime.DeltaTime;
			}
		}
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
			player.PostProcessBeamChanceTick += this.PostProcessBrrrrrrr;
		}

		private void PostProcessBrrrrrrr(BeamController obj)
		{
			if (this.OtherCooldown <= 0f)
			{
				if (Owner.CurrentGun != null)
				{
					if (UnityEngine.Random.value < 0.1f)
					{
						this.OtherCooldown = this.InternalCooldown;
						GoBurst.DoBurst(Owner, null, new Vector2?(Vector2.up * 0.625f));
						if (Owner.PlayerHasActiveSynergy("Reverb"))
						{
							GameManager.Instance.StartCoroutine(Reverb());
						}

					}
				}
			}
		}
			
		private void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
		{
			if (this.OtherCooldown <= 0f)
			{
				if (Owner.CurrentGun != null)
				{
					if (UnityEngine.Random.value < 0.1 * effectChanceScalar)
					{
						UnityEngine.GameObject.Destroy(projectile.gameObject);
						this.OtherCooldown = this.InternalCooldown;
						GoBurst.DoBurst(Owner, null, new Vector2?(Vector2.up * 0.625f));
						if(Owner.PlayerHasActiveSynergy("Reverb"))
						{
							GameManager.Instance.StartCoroutine(Reverb());
						}
					}
				}
			}
		}

		private IEnumerator Reverb()
		{
			yield return new WaitForSeconds(1f);
			GoBurst.DoBurst(Owner, null, new Vector2?(Vector2.up * 0.625f));
			yield break;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			BurstBullets component = debrisObject.GetComponent<BurstBullets>();
			player.PostProcessProjectile -= this.PostProcessProjectile;
			player.PostProcessBeamChanceTick -= this.PostProcessBrrrrrrr;
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
