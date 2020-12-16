using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GungeonAPI;
using Dungeonator;
using System.Collections;

namespace FrostAndGunfireItems
{
	// Token: 0x02000018 RID: 24
	public class RatDieVirus : PassiveItem
	{

		private DamageTypeModifier m_poisonImmunity;
		private bool HasImmunity;
		public static void Init()
		{
			string name = "RATDIE Virus";
			string resourcePath = "FrostAndGunfireItems/Resources/RatDie_Virus";
			GameObject gameObject = new GameObject(name);
			RatDieVirus item = gameObject.AddComponent<RatDieVirus>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Solid";
			string longDesc = "Upon killing an enemy, poison goop is created.\n\nAs more and more Gungeoneers began to pour into the Gunegon, the Resourceful Rat created the RatDie virus as a way to kill off Gungeoneers before they got to him. The virus was intended to kill Gungeoneers with a fatal heart attack, but due to the Resourceful Rat only having a middle schooler's education, the virus ended up making the Gungeoneers walking pathogens, unexpectedly aiding them in their travels.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			item.quality = ItemQuality.A;
			item.SetupUnlockOnCustomFlag(CustomDungeonFlags.RAT_KILLED_AND_WANDERER, true);
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
			this.m_poisonImmunity = new DamageTypeModifier();
			this.m_poisonImmunity.damageMultiplier = 0f;
			this.m_poisonImmunity.damageType = CoreDamageTypes.Poison;
			Owner.healthHaver.damageTypeModifiers.Add(this.m_poisonImmunity);

		}

		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
		{
			if (enemyHealth && fatal && !enemyHealth.aiActor.healthHaver.IsBoss)
			{
				if (Owner.HasPickupID(626) || Owner.HasPickupID(663) || Owner.HasPickupID(662) || Owner.HasPickupID(667))
				{
					AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
					GoopDefinition goopDef = (PickupObjectDatabase.GetById(626) as Gun).DefaultModule.projectiles[0].cheeseEffect.CheeseGoop;
					DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
					goopManagerForGoopType.TimedAddGoopCircle(enemyHealth.aiActor.sprite.WorldCenter, 2.3f, 0.25f, false);
					goopDef.damagesEnemies = false;
				}
				else
				{
					AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
					GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
					DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
					goopManagerForGoopType.TimedAddGoopCircle(enemyHealth.aiActor.sprite.WorldCenter, 2.3f, 0.25f, false);
					goopDef.damagesEnemies = false;
					
				}
			}
		}

		public override DebrisObject Drop(PlayerController player)
		{
			player.OnAnyEnemyReceivedDamage -= OnEnemyDamaged;
			Owner.healthHaver.damageTypeModifiers.Remove(this.m_poisonImmunity);
			DebrisObject debrisObject = base.Drop(player);
			RatDieVirus component = debrisObject.GetComponent<RatDieVirus>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
