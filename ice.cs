
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000A RID: 10
	public class Ice : PassiveItem
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00004058 File Offset: 0x00002258
		public static void Init()
		{
			string name = "Ice";
			string resourcePath = "FrostAndGunfireItems/Resources/ice";
			GameObject gameObject = new GameObject(name);
			Ice ice = gameObject.AddComponent<Ice>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Cool Down";
			string longDesc = "";
			ItemBuilder.SetupItem(ice, shortDesc, longDesc, "kp");
			ice.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000040AF File Offset: 0x000022AF
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			bool flag = player.GetComponent<Ice.LowerCooldown>() != null;
			if (flag)
			{
				UnityEngine.Object.Destroy(player.GetComponent<Ice.LowerCooldown>());
			}
			player.gameObject.AddComponent<Ice.LowerCooldown>().parent = this;
		}
		private class LowerCooldown : BraveBehaviour
		{
			FieldInfo remainingDamageCooldown = typeof(PlayerItem).GetField("remainingDamageCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo remainingTimeCooldown = typeof(PlayerItem).GetField("remainingTimeCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
			public Ice parent;

			void FixedUpdate()
			{
				PlayerController player = this.GetComponent<PlayerController>();
				if (player == null || !player.passiveItems.Contains(parent))
				{
					Destroy(this);
					return;
				}

				if (player.activeItems == null) return;

				foreach (PlayerItem item in player.activeItems)
				{
					if (item == null) continue;
					float maxTime = item.timeCooldown;
					float maxDamage = item.damageCooldown;
					try
					{

						var curRemTime = (float)remainingTimeCooldown.GetValue(item);
						var curRemDmg = (float)remainingDamageCooldown.GetValue(item);

						if (curRemDmg <= 0 || curRemDmg <= 0) continue;
						if(!player.HasPickupID(170))
						{
							remainingTimeCooldown.SetValue(item, curRemTime - (maxTime * 0.45f));
							remainingDamageCooldown.SetValue(item, curRemDmg - (maxDamage * 0.45f));

						}
						else
						{
							remainingTimeCooldown.SetValue(item, curRemTime - (maxTime * 0.55f));
							remainingDamageCooldown.SetValue(item, curRemDmg - (maxDamage * 0.55f));
						}
					
					
					}
					catch (Exception e)
					{
						ETGModConsole.Log(e.Message + ": " + e.StackTrace);
					}
					player.DropPassiveItem(parent);
				}
			}

		}

		public override DebrisObject Drop(PlayerController player)
		{
			UnityEngine.Object.Destroy(base.gameObject, 1f);
			DebrisObject debrisObject = base.Drop(player);
			Ice component = debrisObject.GetComponent<Ice>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

	}
}

