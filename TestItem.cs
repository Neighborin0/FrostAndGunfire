using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	
	public class TestItem : PassiveItem
	{
		private bool Stop;
		
		public static void Init()
		{
			string name = "Test Item";
			string resourcePath = "FrostAndGunfireItems/Resources/chainz";
			GameObject gameObject = new GameObject(name);
			TestItem item = gameObject.AddComponent<TestItem>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Pop Pop";
			string longDesc = "Mo' money, mo' bulletz. \n\nTheze golden chainz were said to fallen from a distant planet, where the true gun god lives.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000069BB File Offset: 0x00004BBB
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.ForceZeroHealthState = true;

		}

	
		// Token: 0x06000087 RID: 135 RVA: 0x00006B2C File Offset: 0x00004D2C
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			TestItem component = debrisObject.GetComponent<TestItem>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x04000027 RID: 39
	
	}
}
