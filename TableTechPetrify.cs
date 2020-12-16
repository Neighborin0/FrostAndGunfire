using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using GungeonAPI;

namespace FrostAndGunfireItems
{
	// Token: 0x0200000F RID: 15
	public class TableTechPetrify : PassiveItem
	{
		private static PlayerController owner;
		public GameActorEffect PetrifyEffect =  new GameActorPetrifyEffect(owner);

		
	
		public static void Init()
		{
			string name = "Table Tech Petrify";
			string resourcePath = "FrostAndGunfireItems/Resources/tabletech_petrify";
			GameObject gameObject = new GameObject();
			TableTechPetrify tableTechPetrify = gameObject.AddComponent<TableTechPetrify>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Flips Petrify";
			string longDesc = "This ancient technique will petrify nearby enemies when a table is flipped.\n\nA Table Tech created in a time where people would flip rocks instead of tables. This stone tablet is written an ancient language that few understand.";
			ItemBuilder.SetupItem(tableTechPetrify, shortDesc, longDesc, "kp");
			tableTechPetrify.quality = PickupObject.ItemQuality.C;
			tableTechPetrify.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_MEDUZI, true);

		}



		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnTableFlipCompleted = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipCompleted, new Action<FlippableCover>(HandleFlip));
		
		}

		
		private void HandleFlip(FlippableCover table)
		{
			this.InternalCooldown = 12f;
			bool flag = Time.realtimeSinceStartup - this.m_lastUsedTime < this.InternalCooldown;
			bool flag2 = !flag;
			if (flag2)
			{
				this.m_lastUsedTime = Time.realtimeSinceStartup;
				base.StartCoroutine(this.HandlePetrify(table));
			}
			
		}

	

		private IEnumerator HandlePetrify(FlippableCover table)
		{

			AkSoundEngine.PostEvent("Play_ENM_gorgun_gaze_01", base.gameObject);
			yield return new WaitForSeconds(0.9f);
			Exploder.DoDistortionWave(table.transform.position, 1f, 0.04f, 20f, 0.3f);
			yield return new WaitForSeconds(0.2f);
			if (Owner.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) > 0)
			{
				foreach (AIActor enemy in Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
				{
					enemy.ApplyEffect(PetrifyEffect);
					
				}
			}
			yield break;
		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			TableTechPetrify component = debrisObject.GetComponent<TableTechPetrify>();
			component.m_pickedUpThisRun = true;
		
	
			
			return debrisObject;

		}
		public float InternalCooldown;

		// Token: 0x04000035 RID: 53
		private float m_lastUsedTime;
		
	}
}
