using System;
using System.Collections;
using ItemAPI;
using UnityEngine;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	public class ClusterShot : PassiveItem
	{
		public float InternalCooldown = 4f;
		public float OtherCooldown;
		public bool IsShotJammed;
		public static void Init()
		{
			string name = "Cluster Shot";
			string resourcePath = "FrostAndGunfireItems/Resources/cluster";
			GameObject gameObject = new GameObject(name);
			ClusterShot item = gameObject.AddComponent<ClusterShot>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Three Stooges Syndrome";
			string longDesc = "Gives the wielder a chance to fire a tight, cluser of bullets\n\nThese bullets are made so compactly that they can easily cluster in a clip. They come out eventually...";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.B;
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, -0.30f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, 0.30f, StatModifier.ModifyMethod.ADDITIVE);
			item.AddToSubShop(ItemBuilder.ShopType.Trorc);
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
		}

		private void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
		{
			if (this.OtherCooldown <= 0f)
			{
				if (!IsShotJammed)
				{
					if (Owner.CurrentGun != null)
					{
						if (UnityEngine.Random.value < 0.2 * effectChanceScalar)
						{
							IsShotJammed = true;
							UnityEngine.GameObject.Destroy(projectile.gameObject);
							if (Owner.PlayerHasActiveSynergy("Jammed"))
							{
								ExplosionData defaultSmallExplosionData2 = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
								this.boom.effect = defaultSmallExplosionData2.effect;
								this.boom.ignoreList = defaultSmallExplosionData2.ignoreList;
								this.boom.ss = defaultSmallExplosionData2.ss;
								Exploder.Explode(Owner.sprite.WorldCenter, boom, Vector2.zero, null, false, CoreDamageTypes.None, false);
							}
						}
					
					}
				}
				else
				{
					if (Owner.CurrentGun != null)
					{
						IsShotJammed = false;
						for (int i = 0; i < 10; i++)
						{
							
							Projectile projectile2 = Owner.CurrentGun.DefaultModule.GetCurrentProjectile();
							GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-8, 8)), true);
							Projectile component = gameObject.GetComponent<Projectile>();
							if (component != null)
							{
								component.Owner = base.Owner;
								component.Shooter = base.Owner.specRigidbody;
								component.baseData.speed *= UnityEngine.Random.Range(0.7f, 1.3f);
								component.baseData.damage /= 2.5f;
								component.baseData.range = 12f;
							Owner.DoPostProcessProjectile(component);
							}	
							this.OtherCooldown = this.InternalCooldown;
						}
					}
				}
			}
		}

		private ExplosionData boom = new ExplosionData
		{
			breakSecretWalls = true,
			doExplosionRing = true,
			damageRadius = 5.5f,
			damageToPlayer = 0f,
			doDamage = true,
			damage = 40f,
			doDestroyProjectiles = true,
			doForce = true,
			debrisForce = 60f,
			preventPlayerForce = true,
			explosionDelay = 0.1f,
			usesComprehensiveDelay = false,
			doScreenShake = true,
		};
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			ClusterShot component = debrisObject.GetComponent<ClusterShot>();
			player.PostProcessProjectile -= this.PostProcessProjectile;
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
