
using GungeonAPI;
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
	public class HeartBurn2 : PlayerItem
	{
		static bool setactive;
		bool disabled = false;
		public static void Init()
		{
			string name = "Heartburn";
			string resourcePath = "FrostAndGunfireItems/Resources/heartburn";
			GameObject gameObject = new GameObject();
			HeartBurn2 heartBurn = gameObject.AddComponent<HeartBurn2>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Calm Yourself";
			string longDesc = "Exchanges health for armor when unarmored. Exchanges armor for health when armored.\n\nAllows the user to burn themselves, bringing them out their rage or enraging them more.";
			ItemBuilder.SetupItem(heartBurn, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(heartBurn, ItemBuilder.CooldownType.Timed, 0f);
			heartBurn.consumable = false;
			heartBurn.CanBeDropped = false;
			heartBurn.quality = PickupObject.ItemQuality.EXCLUDED;
			HeartBurn2.spriteIDs = new int[HeartBurn2.spritePaths.Length];
			HeartBurn2.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(HeartBurn2.spritePaths[0], heartBurn.sprite.Collection);
			HeartBurn2.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(HeartBurn2.spritePaths[1], heartBurn.sprite.Collection);
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
			bool flag = player.CurrentItem != null && player.CurrentItem.name.Contains("Heartburn2");
			bool flag2 = player.maxActiveItemsHeld > 1 && flag;
			if (flag2)
			{
				MethodInfo method = typeof(PlayerController).GetMethod("ChangeItem", BindingFlags.Instance | BindingFlags.NonPublic);
				method.Invoke(player, new object[]
				{
					1
				});
			}
			bool flag3 = HeartBurn2.cursedPlayers.Contains(player) && !self.name.Contains("Heartburn2") && flag;
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
			   typeof(HeartBurn2).GetMethod("HealthHook")
			   );

		public static void HealthHook(Action<HealthPickup, PlayerController> orig, HealthPickup self, PlayerController user)
		{
			orig(self, user);
			if (user.HasPickupID(Gungeon.Game.Items["kp:heartburn"].PickupObjectId))
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


		private void LateUpdate()
		{
			bool flag10 = base.LastOwner;
			bool flag11 = flag10;
			if (flag11)
			{
				if(setactive)
				{
					this.SetActiveStatus(true);
				}
				else
				{
					this.SetActiveStatus(false);
				}
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
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.ArmorUp(player);
		}
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
			float currentHealth = user.healthHaver.GetCurrentHealth();
			user.healthHaver.Armor += currentHealth * 2;
			user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f);
			user.ForceZeroHealthState = true;
			setactive = false;
			this.SetActiveStatus(false);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005F99 File Offset: 0x00004199
		private void DoActiveEffect1(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_TRP_flame_torch_01", base.gameObject);
			GameManager.Instance.StartCoroutine(this.ArmorLost());
			user.ForceZeroHealthState = false;
			setactive = true;
			this.SetActiveStatus(true);
		}

		private IEnumerator ArmorLost()
		{			float armor = this.LastOwner.healthHaver.Armor;
			float currentHealth = this.LastOwner.healthHaver.GetCurrentHealth();
			PlayerController user = this.LastOwner;
			yield return new WaitForSeconds(0.01f);
			user.healthHaver.Armor *= 0f;
			yield return new WaitForSeconds(0.01f);
			user.healthHaver.ForceSetCurrentHealth(currentHealth * 0f + (armor / 2));
			yield return new WaitForSeconds(0.01f);
			yield break;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005FEC File Offset: 0x000041EC
		private void SetActiveStatus(bool active)
		{
			this.activeStatus = active;
			base.sprite.SetSprite(active ? HeartBurn2.spriteIDs[1] : HeartBurn2.spriteIDs[0]);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006015 File Offset: 0x00004215
		

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
				result = user;
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
	}
}
