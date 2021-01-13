using System;
using System.Collections.Generic;
using System.Reflection;
using GungeonAPI;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace FrostAndGunfireItems
{
	public class Fertilizer : PlayerItem
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00007860 File Offset: 0x00005A60
		public static void Init()
		{
			string name = "Fertilizer";
			string resourcePath = "FrostAndGunfireItems/Resources/fertilizer";
			GameObject gameObject = new GameObject();
			Fertilizer item = gameObject.AddComponent<Fertilizer>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Promotes Growth";
			string longDesc = "It doesn't much but provide sustenance for plants...";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 200f);
			item.consumable = false;
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.IgnoredByRat = true;
		}
		protected override void DoEffect(PlayerController user)
		{
		   user.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Healing_Sparkles_001") as GameObject, Vector3.zero, true, false, false);
		}



	}
}
