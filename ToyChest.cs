using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace FrostAndGunfireItems
{
    public class ToyChest : PassiveItem
	{
		private int RoomsCleared;

		public static void Init()
        {
			string name = "Toy Chest";
			string resourcePath = "FrostAndGunfireItems/Resources/toy_chest";
			GameObject gameObject = new GameObject();
			ToyChest toyChest = gameObject.AddComponent<ToyChest>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Happy Something!";
			string longDesc = "Drops pickup every couple of rooms.\n\nThis chest was forged by a jolly, old, blacksmith that wanted to give gifts to Gundead children.";
			ItemBuilder.SetupItem(toyChest, shortDesc, longDesc, "kp");
			toyChest.quality = PickupObject.ItemQuality.C;
			toyChest.AddToSubShop(ItemBuilder.ShopType.Flynt);
        }

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnRoomClearEvent += this.RoomCleared;
		}

		
		private void RoomCleared(PlayerController player)
		{
			RoomsCleared += 1;
			if(RoomsCleared == 4)
			{
				
				AkSoundEngine.PostEvent("Play_OBJ_chest_surprise_01", base.gameObject);
				GenericLootTable singleItemRewardTable = GameManager.Instance.RewardManager.CurrentRewardData.SingleItemRewardTable;
				LootEngine.SpawnItem(singleItemRewardTable.SelectByWeight(false), player.CenterPosition, Vector2.up, 1f, true, false, false);
				RoomsCleared -= RoomsCleared;
			}
		}

		public override DebrisObject Drop(PlayerController player)
		{

			player.OnRoomClearEvent -= this.RoomCleared;
			DebrisObject debrisObject = base.Drop(player);
			ToyChest component = debrisObject.GetComponent<ToyChest>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}


	}
}
