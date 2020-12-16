
using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000015 RID: 21
	public class HeartBurn : PlayerItem
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00005B50 File Offset: 0x00003D50
		public static void Init()
		{
			string name = "Heartburn";
			string resourcePath = "FrostAndGunfireItems/Resources/heartburn";
			GameObject gameObject = new GameObject();
			HeartBurn heartBurn = gameObject.AddComponent<HeartBurn>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Calm Yourself";
			string longDesc = "Exchanges health for armor when unarmored. Exchanges armor for health when armored.\n\nAllows the user to burn themselves, bringing them out their rage or enraging them more.";
			ItemBuilder.SetupItem(heartBurn, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(heartBurn, ItemBuilder.CooldownType.Timed, 0f);
			heartBurn.consumable = false;
			heartBurn.CanBeDropped = false;
			heartBurn.quality = PickupObject.ItemQuality.EXCLUDED;
			HeartBurn.spriteIDs = new int[HeartBurn.spritePaths.Length];
			HeartBurn.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(HeartBurn.spritePaths[0], heartBurn.sprite.Collection);
			HeartBurn.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(HeartBurn.spritePaths[1], heartBurn.sprite.Collection);
			try
			{
				Hook hook = new Hook(typeof(PlayerItem).GetMethod("Pickup"), typeof(HeartBurn).GetMethod("CursedPickup"));
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005C68 File Offset: 0x00003E68
		public static void CursedPickup(Action<PlayerItem, PlayerController> orig, PlayerItem self, PlayerController player)
		{
			bool flag = player.CurrentItem != null && player.CurrentItem.name.Contains("Heartburn");
			bool flag2 = player.maxActiveItemsHeld > 1 && flag;
			if (flag2)
			{
				MethodInfo method = typeof(PlayerController).GetMethod("ChangeItem", BindingFlags.Instance | BindingFlags.NonPublic);
				method.Invoke(player, new object[]
				{
					1
				});
			}
			bool flag3 = HeartBurn.cursedPlayers.Contains(player) && !self.name.Contains("Heartburn") && flag;
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

		public override void Update()
		{
			if(base.LastOwner.healthHaver.Armor > 0 && base.LastOwner.healthHaver.GetCurrentHealth() > 0.5f)
			{
				cantBeHeal = true;
			}
			else
			{
				cantBeHeal = false;
			}
		}
		private void LateUpdate()
		{
			bool flag10 = base.LastOwner;
			bool flag11 = flag10;
			if (flag11)
			{
				PlayerController lastOwner = this.LastOwner;
				float armor = lastOwner.healthHaver.Armor;
				float currentHealth = lastOwner.healthHaver.GetCurrentHealth();
				bool flag = armor > 0f && currentHealth > 0.5f && !this.cantBeHeal;
				if (flag)
				{
					this.ArmorUp(lastOwner);
				}
				bool flag2 = armor > 0f && currentHealth == 0.5f && !this.cantBeHeal;
				if (flag2)
				{
					lastOwner.healthHaver.SetHealthMaximum(0.5f, null, false);
				}
				bool flag3 = this.cantBeHeal;
				if (flag3)
				{
					lastOwner.healthHaver.SetHealthMaximum(0.5f, null, false);
					bool flag4 = (double)armor < 0.5;
					if (flag4)
					{
						this.DoActiveEffect2(lastOwner);
					}
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

	
	

		// Token: 0x0600006F RID: 111 RVA: 0x00005EAC File Offset: 0x000040AC
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

		// Token: 0x06000070 RID: 112 RVA: 0x00005EE8 File Offset: 0x000040E8
		private void ArmorUp(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
			float armor = user.healthHaver.Armor;
			float currentHealth = user.healthHaver.GetCurrentHealth();
			user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f + 0.5f);

				user.healthHaver.Armor += currentHealth + 0.5f;
             this.HealState(true);
			this.SetActiveStatus(false);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005F5C File Offset: 0x0000415C
		private void DoActiveEffect2(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_TRP_flame_torch_01", base.gameObject);
			this.ArmorUp(user);
			GameManager.Instance.StartCoroutine(this.ArmorLost2());
			this.HealState(false);
			this.SetActiveStatus(true);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005F99 File Offset: 0x00004199
		private void DoActiveEffect1(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_TRP_flame_torch_01", base.gameObject);
			GameManager.Instance.StartCoroutine(this.ArmorLost());
			this.HealState(false);
			this.SetActiveStatus(true);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005FCE File Offset: 0x000041CE
		private IEnumerator ArmorLost2()
		{
			float armor = this.LastOwner.healthHaver.Armor;
			float currentHealth = this.LastOwner.healthHaver.GetCurrentHealth();
			PlayerController user = this.LastOwner;
			yield return new WaitForSeconds(0.01f);
			user.healthHaver.Armor *= 0f;
			yield return new WaitForSeconds(0.01f);
			user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f + 0.5f);
			yield break;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005FDD File Offset: 0x000041DD
		private IEnumerator ArmorLost()
		{			float armor = this.LastOwner.healthHaver.Armor;
			float currentHealth = this.LastOwner.healthHaver.GetCurrentHealth();
			PlayerController user = this.LastOwner;
			yield return new WaitForSeconds(0.01f);
			user.healthHaver.Armor *= 0f;
			yield return new WaitForSeconds(0.01f);
			if (armor < 2)
			{
				user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f + armor);
			}
			else
			{
				user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f + (armor - 0.5f));
			}
			yield return new WaitForSeconds(0.01f);
			yield break;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005FEC File Offset: 0x000041EC
		private void SetActiveStatus(bool active)
		{
			this.activeStatus = active;
			base.sprite.SetSprite(active ? HeartBurn.spriteIDs[1] : HeartBurn.spriteIDs[0]);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006015 File Offset: 0x00004215
		private void HealState(bool noheals)
		{
			this.cantBeHeal = noheals;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00006020 File Offset: 0x00004220
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
				bool flag = this.cantBeHeal;
				if (flag)
				{
					result = (user.healthHaver.GetCurrentHealth() > 0f);
				}
				else
				{
					result = (user.healthHaver.GetCurrentHealth() > 0.5f);
				}
			}
			return result;
		}

		// Token: 0x04000024 RID: 36
		public static List<PlayerController> cursedPlayers = new List<PlayerController>();

		// Token: 0x04000025 RID: 37
		private static int[] spriteIDs;

		// Token: 0x04000026 RID: 38
		private static readonly string[] spritePaths = new string[]
		{
			"FrostAndGunfireItems/Resources/heartburn",
			"FrostAndGunfireItems/Resources/heartburn2"
		};

		// Token: 0x04000027 RID: 39
		private bool activeStatus = false;

		// Token: 0x04000028 RID: 40
		private bool cantBeHeal;
	}
}
