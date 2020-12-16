using System;
using System.Collections;
using System.Reflection;
using Dungeonator;
using GungeonAPI;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace FrostAndGunfireItems
{
	//Reduce chance
	public class SpentSack : PlayerItem
	{
		private static int[] spriteIDs;
		private static readonly string[] spritePaths = new string[]
			{
			"FrostAndGunfireItems/Resources/sack_empty",
		"FrostAndGunfireItems/Resources/sack_halfway",
		"FrostAndGunfireItems/Resources/sack_full",

	};
		int id;
		private  int usesMax;
		public static void Init()
		{
			string name = "Spent Sack";
			string resourcePath = "FrostAndGunfireItems/Resources/sack_empty";
			GameObject gameObject = new GameObject();
			SpentSack spent = gameObject.AddComponent<SpentSack>();
			SpentSack.spriteIDs = new int[SpentSack.spritePaths.Length];
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Reduce, Reuse, Reload";
			string longDesc = "Firing seems fill up this sack(?)\n\nWhat appears to be an old busted up sack, is actually a small Tarnisher. It hungers for ammo...";
			ItemBuilder.SetupItem(spent, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(spent, ItemBuilder.CooldownType.Timed, 3);
			spent.consumable = false;
		    spent.UsesNumberOfUsesBeforeCooldown = true;
			spent.quality = PickupObject.ItemQuality.B;
			spent.AddToSubShop(ItemBuilder.ShopType.Trorc);
			SpentSack.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(SpentSack.spritePaths[0], spent.sprite.Collection);
			SpentSack.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(SpentSack.spritePaths[1], spent.sprite.Collection);
			SpentSack.spriteIDs[2] = SpriteBuilder.AddSpriteToCollection(SpentSack.spritePaths[2], spent.sprite.Collection);

		}


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += GainShells;
		
		}

		private void GainShells(Projectile arg1, float effectchance)
		{
			if(LastOwner.HasPickupID(133))
				{
				usesMax = 100;
			    }
			else
			{
				usesMax = 70;
			}
			if (!LastOwner.CurrentGun.InfiniteAmmo)
			{
				float value = UnityEngine.Random.value;
				if ((double)value < 0.25 * effectchance)
				{
					if (numberOfUses < usesMax)
					{
						base.numberOfUses += 1;
					}

				}
			}

		}

		public override void Update()
		{
			PlayerController player = LastOwner;
			bool flag = player;
			bool flag2 = flag;
			if (flag2)
			{
				if (numberOfUses >= 50)
					id = spriteIDs[2];
				else if (numberOfUses >= 25 && numberOfUses <= 49)
					id = spriteIDs[1];
				else
					id = spriteIDs[0];
				sprite.SetSprite(id);
				
				
			}
		}
	

		// Token: 0x0600006D RID: 109 RVA: 0x00005D80 File Offset: 0x00003F80
		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
			user.CurrentGun.GainAmmo(numberOfUses);
			base.StartCoroutine(this.RemoveShells(user));
		}
		public override bool CanBeUsed(PlayerController user)
		{
				return numberOfUses >= 15 && user.CurrentGun != null && user.CurrentGun.CanActuallyBeDropped(user) && !user.CurrentGun.InfiniteAmmo;
		}


		private IEnumerator RemoveShells(PlayerController user)
		{
			yield return new WaitForSeconds(0.1f);
			numberOfUses -= numberOfUses;
			yield break;
		}

	}
}
