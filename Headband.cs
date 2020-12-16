using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200001C RID: 28
	public class Headband : PassiveItem
	{
		private bool StopLaser;



		// Token: 0x060000B7 RID: 183 RVA: 0x0000816C File Offset: 0x0000636C
		public static void Init()
		{
			string name = "Focus Headband";
			string resourcePath = "FrostAndGunfireItems/Resources/headband";
			GameObject gameObject = new GameObject(name);
			Headband headband = gameObject.AddComponent<Headband>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Trigger Discipline";
			string longDesc = "Fires a laser at nearby enemies, as long as it's bearer remains still.\n\nThis headband was worn by ancient Gun Monks who wanted to unlock their third arm, allowing them to wield three guns at once. Prehaps if you practice some discipline, you too will be able to power of the Monks.";
			ItemBuilder.SetupItem(headband, shortDesc, longDesc, "kp");
			headband.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		protected override void Update()
		{
			bool flag = Owner;
			if (flag)
			{
				if (StopLaser != true)
				{
					base.StartCoroutine(DoLaserStuff());
				}

			}
		}

		private IEnumerator DoLaserStuff()
		{
			
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			float num = -1f;
			AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, base.Owner.sprite.WorldCenter, out num, null);
			if (base.Owner.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
			{
				StopLaser = true;
				if (base.Owner.specRigidbody.Velocity.magnitude < 0.05f)
				{
					yield return new WaitForSeconds(1.8f);
					while (true)
					{
						if ((base.Owner.specRigidbody.Velocity.magnitude < 0.05f && !nearestEnemy.IsHarmlessEnemy && nearestEnemy.IsNormalEnemy && !nearestEnemy.healthHaver.IsDead))
						{
							AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", base.gameObject);
							outlineMaterial.SetColor("_OverrideColor", new Color(0f, 110f, 199f));
							yield return new WaitForSeconds(0.1f);

							Vector2 unitCenter = base.Owner.sprite.WorldCenter;
							Vector2 unitCenter2 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
							float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
							Projectile projectile = ((Gun)ETGMod.Databases.Items[385]).DefaultModule.projectiles[0];
							GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z), true);
							Projectile component = gameObject.GetComponent<Projectile>();
							bool flag3 = component != null;
							bool flag4 = flag3;
							if (flag4)
							{
								component.Owner = base.Owner;
								component.Shooter = base.Owner.specRigidbody;
								component.CanTransmogrify = false;
								component.DefaultTintColor = Color.cyan;
							}
							yield return null;
						}
					}
			
				}
			}
			outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			StopLaser = false;
		}

		public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
		{
			AIActor aiactor = null;
			nearestDistance = float.MaxValue;
			bool flag = activeEnemies == null;
			bool flag2 = flag;
			bool flag3 = flag2;
			AIActor result;
			if (flag3)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					AIActor aiactor2 = activeEnemies[i];
					bool flag4 = aiactor2.healthHaver && aiactor2.healthHaver.IsVulnerable;
					bool flag5 = flag4;
					bool flag6 = flag5;
					if (flag6)
					{
						bool flag7 = !aiactor2.healthHaver.IsDead;
						bool flag8 = flag7;
						bool flag9 = flag8;
						if (flag9)
						{
							bool flag10 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
							bool flag11 = flag10;
							bool flag12 = flag11;
							if (flag12)
							{
								float num = Vector2.Distance(position, aiactor2.CenterPosition);
								bool flag13 = num < nearestDistance;
								bool flag14 = flag13;
								bool flag15 = flag14;
								if (flag15)
								{
									nearestDistance = num;
									aiactor = aiactor2;
								}
							}
						}
					}
				}
				result = aiactor;
			}
			return result;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			base.StopCoroutine(DoLaserStuff());
			StopLaser = false;
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			DebrisObject debrisObject = base.Drop(player);
			Headband component = debrisObject.GetComponent<Headband>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}
