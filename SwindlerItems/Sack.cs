using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// There is some kind of fuck here
	public class Sack : PassiveItem
	{
		public float CoinLifespan = 6f;
		public float MinPercentToDrop = 0.20f;
		public float MaxPercentToDrop = 1f;
		public int MaxCoinsToDrop = -1;
		public static void Init()
		{
			string name = "Sack";
			string resourcePath = "FrostAndGunfireItems/Resources/swindler_sack";
			GameObject gameObject = new GameObject(name);
			Sack sack = gameObject.AddComponent<Sack>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Life Savings";
			string longDesc = "A small sack containing what little you got.\n\nIt is prone to damage.";
			ItemBuilder.SetupItem(sack, shortDesc, longDesc, "kp");
			sack.quality = PickupObject.ItemQuality.EXCLUDED;	
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnNewFloorLoaded += GiveMoney;
			player.OnReceivedDamage += HandleEffect;
		}

		private void GiveMoney(PlayerController player)
		{
			player.carriedConsumables.Currency += 25;
			player.carriedConsumables.KeyBullets -= 1;
			player.OnNewFloorLoaded -= GiveMoney;
		}

		private void HandleEffect(PlayerController player)
		{
			if (Owner.carriedConsumables.Currency > 0)
			{
				float num = Owner.carriedConsumables.Currency * 0.30f;
				if (this.MaxCoinsToDrop > 0)
				{
					num = Mathf.Clamp(num, 0, this.MaxCoinsToDrop);
				}
				num = Mathf.Min(num, Owner.carriedConsumables.Currency);
				AkSoundEngine.PostEvent("Play_OBJ_coin_spill_01", base.gameObject);
				Owner.carriedConsumables.Currency = (int)(Owner.carriedConsumables.Currency - num);
				LootEngine.SpawnCurrencyManual(Owner.CenterPosition, (int)num);
			}
		}

		protected override void OnDestroy()
		{
			Owner.OnReceivedDamage -= HandleEffect;
			base.OnDestroy();
		}
		public override DebrisObject Drop(PlayerController player)
		{
			Owner.OnReceivedDamage -= HandleEffect;
			DebrisObject debrisObject = base.Drop(player);
			Sack component = debrisObject.GetComponent<Sack>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
