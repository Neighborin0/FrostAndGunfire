using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// There is some kind of fuck here
	public class FrozenArmor : PassiveItem
	{
		FieldInfo remainingDamageCooldown = typeof(PlayerItem).GetField("remainingDamageCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
		FieldInfo remainingTimeCooldown = typeof(PlayerItem).GetField("remainingTimeCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
		public FrozenArmor parent;
		private bool Stop;
	

		public static void Init()
		{

			string name = "Frost Armor";
			string resourcePath = "FrostAndGunfireItems/Resources/frost";
			GameObject gameObject = new GameObject(name);
			FrozenArmor frozenArmor = gameObject.AddComponent<FrozenArmor>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Cold Shoulders";
			string longDesc = "Reduces an item's cooldown when hit.\n\nA relic of the Frost Riders, a band of legendary knights that roamed the Gungeon in search of it's fabled treasures. Unfortunately, their armor proved useless against the flames of the Forge.\n\nPerhaps this armor has other purposes?";
			ItemBuilder.SetupItem(frozenArmor, shortDesc, longDesc, "kp");
			frozenArmor.quality = PickupObject.ItemQuality.D;
			
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			HealthHaver healthHaver = player.healthHaver;
			if(!Stop)
			{
				player.healthHaver.Armor += 1;
				Stop = true;
			}
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
		
		}

	

		private void HandleEffect(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
		{
			if (Owner.activeItems == null) return;
			foreach (PlayerItem item in Owner.activeItems)
			{
				if (item == null) continue;
				float maxTime = item.timeCooldown;
				float maxDamage = item.damageCooldown;
				try
				{

					var curRemTime = (float)remainingTimeCooldown.GetValue(item);
					var curRemDmg = (float)remainingDamageCooldown.GetValue(item);

					if (curRemDmg <= 0 || curRemDmg <= 0) continue;

					remainingTimeCooldown.SetValue(item, curRemTime - (maxTime * 0.25f));
					remainingDamageCooldown.SetValue(item, curRemDmg - (maxDamage * 0.25f));


				}
				catch (Exception e)
				{
					ETGModConsole.Log(e.Message + ": " + e.StackTrace);
				}

			}


		}

		protected override void OnDestroy()
		{
			bool flag = base.Owner;
			if (flag)
			{
				HealthHaver healthHaver = base.Owner.healthHaver;
				healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
				
			}
			base.OnDestroy();
		}
		public override DebrisObject Drop(PlayerController player)
		{
			HealthHaver healthHaver = player.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
			DebrisObject debrisObject = base.Drop(player);
			
			FrozenArmor component = debrisObject.GetComponent<FrozenArmor>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
