using System;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200001C RID: 28
	public class Blamulet : PassiveItem
	{
		
		// Token: 0x060000B7 RID: 183 RVA: 0x0000816C File Offset: 0x0000636C
		public static void Init()
		{
			string name = "Blamulet";
			string resourcePath = "FrostAndGunfireItems/Resources/blamulet";
			GameObject gameObject = new GameObject(name);
			Blamulet blamulet = gameObject.AddComponent<Blamulet>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Blanks Go Boom?";
			string longDesc = "Previously known as the Gunpowder Ammolet, this ammolet is worn after thousands of explosion. Lucky there is still some Gunpowder inside.";
			ItemBuilder.SetupItem(blamulet, shortDesc, longDesc, "kp");
			blamulet.quality = PickupObject.ItemQuality.D;
			blamulet.AddToSubShop(ItemBuilder.ShopType.Trorc);
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnUsedBlank += BoomBoom;
			HealthHaver healthHaver = player.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
		}

		private void BoomBoom(PlayerController x, int arg2)
		{
			if (Owner.HasPickupID(108) || Owner.HasPickupID(332))
			{
				SpawnObjectPlayerItem component = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
				GameObject gameObject = component.objectToSpawn.gameObject;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, Owner.sprite.WorldBottomCenter, Quaternion.identity);
				tk2dBaseSprite component2 = gameObject2.GetComponent<tk2dBaseSprite>();
				bool flag6 = component2;
				if (flag6)
				{
					component2.PlaceAtPositionByAnchor(Owner.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				}
			}
			if (Owner.HasPickupID(109))
			{
				SpawnObjectPlayerItem component = PickupObjectDatabase.GetById(109).GetComponent<SpawnObjectPlayerItem>();
				GameObject gameObject = component.objectToSpawn.gameObject;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, Owner.sprite.WorldBottomCenter, Quaternion.identity);
				tk2dBaseSprite component2 = gameObject2.GetComponent<tk2dBaseSprite>();
				bool flag6 = component2;
				if (flag6)
				{
					component2.PlaceAtPositionByAnchor(Owner.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				}
			}
			ExplosionData defaultSmallExplosionData2 = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
			this.boom.effect = defaultSmallExplosionData2.effect;
			this.boom.ignoreList = defaultSmallExplosionData2.ignoreList;
			this.boom.ss = defaultSmallExplosionData2.ss;
			Exploder.Explode(Owner.sprite.WorldCenter, boom, Vector2.zero, null, false, CoreDamageTypes.None, false);
			
		}

		private void HandleEffect(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
		{
			if (Owner.HasPickupID(108))
			{
				SpawnObjectPlayerItem component = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
				GameObject gameObject = component.objectToSpawn.gameObject;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, Owner.sprite.WorldBottomCenter, Quaternion.identity);
				tk2dBaseSprite component2 = gameObject2.GetComponent<tk2dBaseSprite>();
				bool flag6 = component2;
				if (flag6)
				{
					component2.PlaceAtPositionByAnchor(Owner.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				}
			}
			if (Owner.HasPickupID(109))
			{
				SpawnObjectPlayerItem component = PickupObjectDatabase.GetById(109).GetComponent<SpawnObjectPlayerItem>();
				GameObject gameObject = component.objectToSpawn.gameObject;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, Owner.sprite.WorldBottomCenter, Quaternion.identity);
				tk2dBaseSprite component2 = gameObject2.GetComponent<tk2dBaseSprite>();
				bool flag6 = component2;
				if (flag6)
				{
					component2.PlaceAtPositionByAnchor(Owner.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				}
			}
			ExplosionData defaultSmallExplosionData2 = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
			this.boom.effect = defaultSmallExplosionData2.effect;
			this.boom.ignoreList = defaultSmallExplosionData2.ignoreList;
			this.boom.ss = defaultSmallExplosionData2.ss;
			Exploder.Explode(Owner.sprite.WorldCenter, this.boom, Vector2.zero, null, false, CoreDamageTypes.None, false);

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
			player.OnUsedBlank -= BoomBoom;
			DebrisObject debrisObject = base.Drop(player);
			Blamulet component = debrisObject.GetComponent<Blamulet>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
