using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ItemAPI;
using UnityEngine;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	public class Pouch : PickupObject
	{
		private bool m_hasBeenPickedUp;

		public static void Init()
		{
			string name = "Pouch";
			string resourcePath = "FrostAndGunfireItems/Resources/pouch";
			GameObject gameObject = new GameObject(name);
			Pouch item = gameObject.AddComponent<Pouch>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "";
			string longDesc = "";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
		}
		 void Start()
		{
			SpeculativeRigidbody srb = base.gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(17, 13));
			srb.CollideWithTileMap = false;
			srb.CollideWithOthers = true;
			srb.ForceAlwaysUpdate = true;
			srb.CanBeCarried = false;
			srb.CanBePushed = false;
			srb.PrimaryPixelCollider.CollisionLayer = CollisionLayer.Pickup;
			srb.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(srb.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollison));
		}

		private void OnPreCollison(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			if (this.m_hasBeenPickedUp)
			{
				return;
			}
			PlayerController component = otherRigidbody.GetComponent<PlayerController>();
			if (component != null)
			{
				m_hasBeenPickedUp = true;
				Pickup(component);
			}
		}

		public override void Pickup(PlayerController player)
		{
			player.carriedConsumables.Currency += 15;
			player.BloopItemAboveHead(base.sprite, "FrostAndGunfireItems/Resources/pouch");
			AkSoundEngine.PostEvent("Play_OBJ_coin_medium_01", base.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	
	}
}
