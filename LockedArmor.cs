using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// There is some kind of fuck here
	public class LockedArmor : PassiveItem
	{
		private bool Stop;

		public static void Init()
		{

			string name = "Locked Armor";
			string resourcePath = "FrostAndGunfireItems/Resources/locked_armor";
			GameObject gameObject = new GameObject(name);
			LockedArmor item = gameObject.AddComponent<LockedArmor>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Under Lock And Key";
			string longDesc = "Negates damage as long as its bearer has a key.\n\nSir Lockhart Jr came to the Gungeon centuries ago seeking to kill his past but the pure rage and insanity of his inability to do so drove him to gain a strange obsession with locks. Through studying the locked doors and chests of the Gungeon, Lockhart created the ultimate lock, impenetrable and impossible to unlock";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = PickupObject.ItemQuality.C;
			item.AddToSubShop(ItemBuilder.ShopType.Flynt);

		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			HealthHaver healthHaver = player.healthHaver;
			if (!Stop)
			{
				player.carriedConsumables.KeyBullets += 2;
				Stop = true;
			}
				healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
		
		
		}

	

		private void HandleEffect(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
		{
			if (Owner.carriedConsumables.KeyBullets > 0)
			{
				PlayerController owner = base.Owner;
				GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
				AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
				GameObject gameObject2 = new GameObject("silencer");
				SilencerInstance silencerInstance = gameObject2.AddComponent<SilencerInstance>();
				float num = 0.25f;
				silencerInstance.TriggerSilencer(owner.sprite.WorldCenter, 25f, 5f, gameObject, 0f, 3f, 3f, 3f, 250f, 5f, num, owner, false, false);
				bool flag = args == EventArgs.Empty;
				if (!flag)
				{

					source.GetComponent<PlayerController>();
					args.ModifiedDamage = 0f;
					Owner.carriedConsumables.KeyBullets -= 1;
				}

				if (Owner.HasPickupID(166) || Owner.HasPickupID(95))
				{
					bool flag3 = args == EventArgs.Empty;
					if (!flag3)
					{
						bool flag2 = UnityEngine.Random.value < 0.3;
						if (flag2)
						{
							source.GetComponent<PlayerController>();
							args.ModifiedDamage = 0f;
						}
					}
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
			
			LockedArmor component = debrisObject.GetComponent<LockedArmor>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
