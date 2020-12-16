using System;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class IBuy : PlayerItem
	{
	
		public static void Init()
		{
			string name = "iBuy";
			string resourcePath = "FrostAndGunfireItems/Resources/ibuy";
			GameObject gameObject = new GameObject();
			IBuy buy = gameObject.AddComponent<IBuy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Same Second Delivery";
			string longDesc = "This device is able to connect to great Hegemony Shopping Network. Due powerful transporter technology within this device it's is able to transfer things between a dealer and customer instantly.";
			ItemBuilder.SetupItem(buy, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(buy, ItemBuilder.CooldownType.Timed, 1f);
			buy.consumable = false;
			buy.quality = PickupObject.ItemQuality.C;
		}

	
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}


		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
			IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
			if (nearestInteractable is PassiveItem)
			{
				
				PassiveItem passiveItem = nearestInteractable as PassiveItem;
				if(passiveItem.PickupObjectId == 127)
				{
					user.carriedConsumables.Currency += 1;
				}
				PickupObject.ItemQuality quality = (passiveItem).quality;
				bool flag6 = quality == ItemQuality.D;
				if (flag6)
				{
					user.carriedConsumables.Currency += UnityEngine.Random.Range(10, 20);

				}
				else
				{
					bool flag3 = quality == ItemQuality.C;
					if (flag3)
					{
						user.carriedConsumables.Currency += UnityEngine.Random.Range(20, 30);

					}
					else
					{
						bool flag4 = quality == ItemQuality.B;
						if (flag4)
						{
							user.carriedConsumables.Currency += UnityEngine.Random.Range(30, 40);

						}
						else
						{
							bool flag5 = quality == ItemQuality.A;
							if (flag5)
							{
								user.carriedConsumables.Currency += UnityEngine.Random.Range(40, 50);

							}
							else
							{
								bool flag7 = quality == ItemQuality.S;
								if (flag7)
								{
									user.carriedConsumables.Currency += UnityEngine.Random.Range(50, 60);

								}
							}
						}
					}
				}
				user.CurrentRoom.DeregisterInteractable(passiveItem);
				UnityEngine.Object.Destroy(passiveItem.gameObject);
			}
			else if (nearestInteractable is PlayerItem)
			{
				PlayerItem playerItem = nearestInteractable as PlayerItem;
				PickupObject.ItemQuality quality = (playerItem).quality;
				bool flag6 = quality == ItemQuality.D;
				if (flag6)
				{
					user.carriedConsumables.Currency += UnityEngine.Random.Range(10, 20);

				}
				else
				{
					bool flag3 = quality == ItemQuality.C;
					if (flag3)
					{
						user.carriedConsumables.Currency += UnityEngine.Random.Range(20, 30);

					}
					else
					{
						bool flag4 = quality == ItemQuality.B;
						if (flag4)
						{
							user.carriedConsumables.Currency += UnityEngine.Random.Range(30, 40);

						}
						else
						{
							bool flag5 = quality == ItemQuality.A;
							if (flag5)
							{
								user.carriedConsumables.Currency += UnityEngine.Random.Range(40, 50);

							}
							else
							{
								bool flag7 = quality == ItemQuality.S;
								if (flag7)
								{
									user.carriedConsumables.Currency += UnityEngine.Random.Range(50, 60);

								}
							}
						}
					}
				}
				user.CurrentRoom.DeregisterInteractable(playerItem);
				UnityEngine.Object.Destroy(playerItem.gameObject);
			}
			else if (nearestInteractable is Gun)
			{
				Gun gun = nearestInteractable as Gun;
				PickupObject.ItemQuality quality = (gun).quality;
				bool flag6 = quality == ItemQuality.D;
				if (flag6)
				{
					user.carriedConsumables.Currency += UnityEngine.Random.Range(10, 20);

				}
				else
				{
					bool flag3 = quality == ItemQuality.C;
					if (flag3)
					{
						user.carriedConsumables.Currency += UnityEngine.Random.Range(20, 30);

					}
					else
					{
						bool flag4 = quality == ItemQuality.B;
						if (flag4)
						{
							user.carriedConsumables.Currency += UnityEngine.Random.Range(30, 40);

						}
						else
						{
							bool flag5 = quality == ItemQuality.A;
							if (flag5)
							{
								user.carriedConsumables.Currency += UnityEngine.Random.Range(40, 50);

							}
							else
							{
								bool flag7 = quality == ItemQuality.S;
								if (flag7)
								{
									user.carriedConsumables.Currency += UnityEngine.Random.Range(50, 60);

								}
							}
						}
					}
				}
				user.CurrentRoom.DeregisterInteractable(gun);
				UnityEngine.Object.Destroy(gun.gameObject);
			}

		}

		public override bool CanBeUsed(PlayerController user)
		{
			IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
			if (nearestInteractable is PassiveItem)
			{
				PassiveItem passiveItem = (nearestInteractable as PassiveItem);
			}
			return (nearestInteractable is PassiveItem | nearestInteractable is PlayerItem | nearestInteractable is Gun);
		}
	}
}
