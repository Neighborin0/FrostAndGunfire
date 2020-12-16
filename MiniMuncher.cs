using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000F RID: 15
	public class MiniMuncher : PlayerItem
	{
		//Good to Go
		private float duration = 5f;
		private PickupObject.ItemQuality itemToGiveQuality = ItemQuality.D;

		// Token: 0x0600006B RID: 107 RVA: 0x00005D14 File Offset: 0x00003F14
		public static void Init()
		{
			string name = "Mini Muncher";
			string resourcePath = "FrostAndGunfireItems/Resources/munch/muncher_idle_001";
			GameObject gameObject = new GameObject();
			MiniMuncher minimuncher = gameObject.AddComponent<MiniMuncher>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Eats Lead";
			string longDesc = "This baby muncher doesn't have as big of an appetite as it's older counterparts, but will still happily eat any guns you'll give it. ";
			ItemBuilder.SetupItem(minimuncher, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(minimuncher, ItemBuilder.CooldownType.Timed, 0.3f);
			minimuncher.consumable = true;
			minimuncher.numberOfUses = 3;
			minimuncher.quality = PickupObject.ItemQuality.C;

		}


		protected override void DoEffect(PlayerController user)
		{


			Gun currentGun = user.CurrentGun;
			PickupObject.ItemQuality quality = currentGun.quality;
			if (numberOfUses == 1)
			{
				bool flag6 = currentGun.quality == ItemQuality.D;
				if (flag6)
				{
					this.itemToGiveQuality = ItemQuality.D;

				}
				else
				{
					bool flag3 = currentGun.quality == ItemQuality.C;
					if (flag3)
					{
						this.itemToGiveQuality = ItemQuality.C;

					}
					else
					{
						bool flag4 = currentGun.quality == ItemQuality.B;
						if (flag4)
						{
							this.itemToGiveQuality = ItemQuality.B;

						}
						else
						{
							bool flag5 = currentGun.quality == ItemQuality.A;
							if (flag5)
							{
								this.itemToGiveQuality = ItemQuality.A;

							}
							else
							{
								bool flag7 = currentGun.quality == ItemQuality.S;
								if (flag7)
								{
									this.itemToGiveQuality = ItemQuality.S;

								}
							}
						}
					}
				}
				GetGun();
			}
			else
			{
				base.StartCoroutine(this.HandleDuration(user));
				bool flag2 = currentGun.quality == ItemQuality.D;
				if (flag2)
				{
					this.itemToGiveQuality = ItemQuality.D;

				}
				else
				{
					bool flag3 = currentGun.quality == ItemQuality.C;
					if (flag3)
					{
						this.itemToGiveQuality = ItemQuality.C;

					}
					else
					{
						bool flag4 = currentGun.quality == ItemQuality.B;
						if (flag4)
						{
							this.itemToGiveQuality = ItemQuality.B;

						}
						else
						{
							bool flag5 = currentGun.quality == ItemQuality.A;
							if (flag5)
							{
								this.itemToGiveQuality = ItemQuality.A;

							}
							else
							{
								bool flag6 = currentGun.quality == ItemQuality.S;
								if (flag6)
								{
									this.itemToGiveQuality = ItemQuality.S;

								}
								else
								{
									this.itemToGiveQuality = ItemQuality.D;
								}
							}
						
						}
					}
				}
				
			}
			AkSoundEngine.PostEvent("Play_CHR_muncher_eat_01", base.gameObject);
			user.inventory.DestroyCurrentGun();
		}


		

	

		private IEnumerator HandleDuration(PlayerController user)
		{
			if (this.IsCurrentlyActive)
			{
				yield break;
			}
			AkSoundEngine.PostEvent("Play_CHR_muncher_chew_01", base.gameObject);
			this.IsCurrentlyActive = true;
			this.m_activeElapsed = 0f;
			this.m_activeDuration = this.duration;
			while (this.m_activeElapsed < this.m_activeDuration && this.IsCurrentlyActive)
			{

				yield return null;
			}
			this.GetGun();
			this.IsCurrentlyActive = false;
			yield break;
		}




		private void GetGun()
		{
			PlayerController lastOwner = this.LastOwner;
				PickupObject.ItemQuality itemQuality = itemToGiveQuality;
				PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false);
			LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, lastOwner.CenterPosition, Vector2.up, 1f, true, true, false);
			
		
		}

	

		public override bool CanBeUsed(PlayerController user)
		{
			return user.CurrentGun != null && user.CurrentGun.CanActuallyBeDropped(user) && !user.CurrentGun.InfiniteAmmo;
	
		
		}

	}
}
