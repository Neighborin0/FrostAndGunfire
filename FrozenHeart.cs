using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace FrostAndGunfireItems
{
	//Fix the fuck here
	public class FrozenHeart : PlayerItem
	{
		
		public static void Init()
		{
			string name = "Frozen Heart";
			string resourcePath = "FrostAndGunfireItems/Resources/refrigerate";
			GameObject gameObject = new GameObject();
			FrozenHeart frozenHeart = gameObject.AddComponent<FrozenHeart>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Calm Yourself";
			string longDesc = "Exchanges health for armor when unarmored. Exchanges armor for health when armored. \n\nAllows the user to change thier temperature at will, bringing them out their rage or enrages them more.";
			ItemBuilder.SetupItem(frozenHeart, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(frozenHeart, ItemBuilder.CooldownType.Timed, 0f);
			frozenHeart.consumable = false;
			frozenHeart.CanBeDropped = false;
			frozenHeart.quality = PickupObject.ItemQuality.EXCLUDED;
			FrozenHeart.spriteIDs = new int[FrozenHeart.spritePaths.Length];
			FrozenHeart.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(FrozenHeart.spritePaths[0], frozenHeart.sprite.Collection);
			FrozenHeart.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(FrozenHeart.spritePaths[1], frozenHeart.sprite.Collection);
			try
			{
				Hook hook = new Hook(typeof(PlayerItem).GetMethod("Pickup"), typeof(FrozenHeart).GetMethod("CursedPickup"));
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005298 File Offset: 0x00003498
     public static void CursedPickup(Action<PlayerItem, PlayerController> orig, PlayerItem self, PlayerController player)
		{
			bool flag = player.CurrentItem != null && player.CurrentItem.name.Contains("Frozen Heart");
			bool flag2 = player.maxActiveItemsHeld > 1 && flag;
			if (flag2)
			{
				MethodInfo method = typeof(PlayerController).GetMethod("ChangeItem", BindingFlags.Instance | BindingFlags.NonPublic);
				method.Invoke(player, new object[]
				{
					1
				});
			}
			bool flag3 = FrozenHeart.cursedPlayers.Contains(player) && !self.name.Contains("Frozen Heart") && flag;
			if (flag3)
			{
				bool flag4 = player.maxActiveItemsHeld <= player.activeItems.Count;
				if (flag4)
				{
					return;
				}
			}
			orig(self, player);
		}

		Hook healthPickupHook = new Hook(
				   typeof(HealthPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
			   typeof(FrozenHeart).GetMethod("HealthHook")
			   );
		public static void HealthHook(Action<HealthPickup, PlayerController> orig, HealthPickup self, PlayerController user)
		{
			orig(self, user);
			if (user.HasPickupID(Gungeon.Game.Items["kp:frozen_heart"].PickupObjectId))
			{
				if (self.armorAmount > 0)
				{
					float currentHealth = user.healthHaver.GetCurrentHealth();
					user.healthHaver.Armor += currentHealth * 2;
					user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f);
					user.ForceZeroHealthState = true;
					setactive = false;
					
				}
			}
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.ArmorUp(player);
		}
		private void LateUpdate()
		{
			if (setactive)
			{
				this.SetActiveStatus(true);
			}
			else
			{
				this.SetActiveStatus(false);
			}
			bool flag10 = base.LastOwner;
			bool flag11 = flag10;
			if (flag11)
			{
				PlayerController lastOwner = this.LastOwner;
				float armor = lastOwner.healthHaver.Armor;
				float currentHealth = lastOwner.healthHaver.GetCurrentHealth();
				bool flag = armor > 0f && currentHealth > 0.5f;
				if (flag)
				{
					this.ArmorUp(lastOwner);
					lastOwner.ForceZeroHealthState = true;
				}
				bool flag5 = !HeartBurn.cursedPlayers.Contains(lastOwner);
				if (flag5)
				{
					HeartBurn.cursedPlayers.Add(lastOwner);
				}
				int num = -1;
				for (int i = 0; i < lastOwner.activeItems.Count; i++)
				{
					bool flag6 = lastOwner.activeItems[i] == this;
					if (flag6)
					{
						num = i;
						break;
					}
				}
				bool flag7 = num != 0;
				if (flag7)
				{
					PlayerItem value = lastOwner.activeItems[0];
					lastOwner.activeItems[0] = this;
					lastOwner.activeItems[num] = value;
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000054DC File Offset: 0x000036DC
		protected override void DoEffect(PlayerController user)
		{
			bool flag = this.activeStatus;
			if (flag)
			{
				this.ArmorUp(user);
			}
			else
			{
				bool flag2 = !this.activeStatus;
				if (flag2)
				{
					this.DoActiveEffect1(user);
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005518 File Offset: 0x00003718
		private void ArmorUp(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
			float currentHealth = user.healthHaver.GetCurrentHealth();
			user.healthHaver.Armor += currentHealth * 2;
			user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f);
			user.ForceZeroHealthState = true;
			setactive = false;
			this.SetActiveStatus(false);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000055C9 File Offset: 0x000037C9
		private void DoActiveEffect1(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_ENM_iceslime_shot_01", base.gameObject);
			GameManager.Instance.StartCoroutine(this.ArmorLost());
			user.ForceZeroHealthState = false;
			setactive = true;
			this.SetActiveStatus(true);
		}
		// Token: 0x06000059 RID: 89 RVA: 0x0000560D File Offset: 0x0000380D
		private IEnumerator ArmorLost()
		{
			float armor = this.LastOwner.healthHaver.Armor;
			float currentHealth = this.LastOwner.healthHaver.GetCurrentHealth();
			PlayerController user = this.LastOwner;
			yield return new WaitForSeconds(0.01f);
			user.healthHaver.Armor *= 0f;
			yield return new WaitForSeconds(0.01f);
			user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f + (armor / 2));
			yield return new WaitForSeconds(0.01f);
			yield break;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000561C File Offset: 0x0000381C
		private void SetActiveStatus(bool active)
		{
			this.activeStatus = active;
			base.sprite.SetSprite(active ? FrozenHeart.spriteIDs[1] : FrozenHeart.spriteIDs[0]);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005645 File Offset: 0x00003845
		private void HealState(bool noheals)
		{
			this.cantBeHeal = noheals;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005650 File Offset: 0x00003850
		public override bool CanBeUsed(PlayerController user)
		{
			bool hasCrest = user.healthHaver.HasCrest;
			bool result;
			if (hasCrest)
			{
				result = (user.healthHaver.GetCurrentHealth() > 1E+10f);
			}
			else
			{
				result = user;
			}
			return result;
		}

		// Token: 0x0400001F RID: 31
		public static List<PlayerController> cursedPlayers = new List<PlayerController>();

		// Token: 0x04000020 RID: 32
		private static int[] spriteIDs;

		// Token: 0x04000021 RID: 33
		private static readonly string[] spritePaths = new string[]
		{
			"FrostAndGunfireItems/Resources/refrigerate",
			"FrostAndGunfireItems/Resources/refrigerate2"
		};

		// Token: 0x04000022 RID: 34
		public bool activeStatus = false;

		static bool setactive;
		// Token: 0x04000023 RID: 35
		public bool cantBeHeal;
	}
}
