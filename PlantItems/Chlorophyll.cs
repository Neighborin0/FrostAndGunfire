
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

    class Chlorophyll : PassiveItem
    {
		
		public static void Init()
		{
			string name = "Chlorophyll";
			string resourcePath = "FrostAndGunfireItems/Resources/plant_heart";
			GameObject gameObject = new GameObject(name);
			Chlorophyll item = gameObject.AddComponent<Chlorophyll>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "100% Organic";
			string longDesc = "Armor is exchanged for cooldown reduction when picked up.\n\nArmor is useless to a being such as the Plant and it much rather use such things to help bolster its spawn.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.CanBeDropped = false;
		}

		//private void LateUpdate()
		//{
		//	if (this != null)
		//	{
		//		dfAtlas.ItemInfo pain = new dfAtlas.ItemInfo();
		//		pain.name = "Plant Heart";
		//		pain.texture = ResourceExtractor.GetTextureFromResource("FrostAndGunfireItems/Resources/plant_heart.png");
		//		pain.sizeInPixels = new Vector2(15f, 13f);
		//		Texture2D atlas = GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.Texture;
		//		Rect region = GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.Items[0].region;

		//		for (int x = 0; x < pain.texture.width; x++)
		//		{
		//			for (int y = 0; y < pain.texture.height; y++)
		//			{
		//				atlas.SetPixel(x + (int)(region.xMin * 2048), y + (int)(region.yMin * 2048), pain.texture.GetPixel(x, y));
		//			}
		//		}
		//		atlas.Apply(false, false);
		//		pain.region = new Rect(region.xMin, region.yMin, (float)pain.texture.width / 2048f, (float)pain.texture.height / 2048f);
		//		GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.AddItem(pain);
		//		GameUIRoot.Instance.heartControllers[0].fullHeartSpriteName = "Plant Heart";
		//		Owner.healthHaver.Armor = 0f;
		//	}
		//}
		Hook healthPickupHook = new Hook(
		typeof(HealthPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
		typeof(Chlorophyll).GetMethod("HealthHook"));

		public static void HealthHook(Action<HealthPickup, PlayerController> orig, HealthPickup self, PlayerController user)
		{
			FieldInfo remainingDamageCooldown = typeof(PlayerItem).GetField("remainingDamageCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo remainingTimeCooldown = typeof(PlayerItem).GetField("remainingTimeCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
			orig(self, user);
			if (user.HasPickupID(Gungeon.Game.Items["kp:chlorophyll"].PickupObjectId))
			{
				if (self.armorAmount > 0)
				{
					if (user.activeItems == null) return;
					foreach (PlayerItem item in user.activeItems)
					{
						if (item == null) continue;
						float maxTime = item.timeCooldown;
						float maxDamage = item.damageCooldown;
						try
						{

							var curRemTime = (float)remainingTimeCooldown.GetValue(item);
							var curRemDmg = (float)remainingDamageCooldown.GetValue(item);

							if (curRemDmg <= 0 || curRemDmg <= 0) continue;

							remainingTimeCooldown.SetValue(item, curRemTime - (maxTime * 0.125f));
							remainingDamageCooldown.SetValue(item, curRemDmg - (maxDamage * 0.125f));

						}
						catch (Exception e)
						{
							ETGModConsole.Log(e.Message + ": " + e.StackTrace);
						}
					}
				}
			}
		}
	
	}

}

