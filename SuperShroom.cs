using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	//Reduce cooldown
	public class SuperShroom : AffectEnemiesInRoomItem
	{
	
	
		public static void Init()
		{
			string name = "Super Shroom";
			string resourcePath = "FrostAndGunfireItems/Resources/shroom";
			GameObject gameObject = new GameObject();
			SuperShroom superShroom = gameObject.AddComponent<SuperShroom>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Super Sized!";
			string longDesc = "The spores released from this shroom make nearby Gundead incredibly lethargic.\n\nThis mushroom was imported from a far away kingdom, where it's powers are used to make people powerful. Due to rot however, it's powers can only make those near it sluggish.";
			ItemBuilder.SetupItem(superShroom, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(superShroom, ItemBuilder.CooldownType.Damage, 270f);
			superShroom.quality = PickupObject.ItemQuality.B;
		}


	


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000A320 File Offset: 0x00008520
		private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
			PlayerController lastOwner = this.LastOwner;
			bool flag = lastOwner.CurrentGun.PickupObjectId.Equals(376);
			bool flag2 = flag && !this.Stop;
			if (flag2)
			{
				sourceProjectile.AdditionalScaleMultiplier *= 2f;
				this.Stop = true;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000A37C File Offset: 0x0000857C
		public override void Update()
		{
			bool flag = this.LastOwner;
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = !this.LastOwner.HasPickupID(641);
				if (flag3)
				{
					this.Stop = false;
				}
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000A3BD File Offset: 0x000085BD
		protected override void OnPreDrop(PlayerController user)
		{
			user.PostProcessProjectile -= this.PostProcessProjectile;
			base.OnPreDrop(user);
		}



		protected override void DoEffect(PlayerController user)
		{
			base.DoEffect(user);
			GameManager.Instance.StartCoroutine(this.HandleTime());
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004FA0 File Offset: 0x000031A0
		protected override void AffectEnemy(AIActor target)
		{
			bool flag = target != null && target.healthHaver.IsVulnerable && !target.healthHaver.IsBoss && !SuperShroom.BannedEnemies.Contains(target.EnemyGuid);
			if (flag)
			{
				this.affectedEnemies.Add(target);
				GameManager.Instance.StartCoroutine(this.Grow(target));
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004FFF File Offset: 0x000031FF
		private IEnumerator HandleTime()
		{
			this.m_isCurrentlyActive = true;
			while (this.affectedEnemies.Any<AIActor>())
			{
				yield return null;
			}
			this.m_isCurrentlyActive = false;
			yield break;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000500E File Offset: 0x0000320E
		private IEnumerator Grow(AIActor target)
		{
			AkSoundEngine.PostEvent("Play_WPN_zapper_reload_01", base.gameObject);
			float elapsed = 0f;
			bool flag = target == null;
			if (flag)
			{
				this.affectedEnemies.Remove(target);
				yield break;
			}
			target.MovementSpeed *= 0.75f;
			if (target.bulletBank != null && target.healthHaver != null)
			{
				for (int i = 0; i < target.bulletBank.Bullets.Count; i++)
				{
					target.bulletBank.Bullets[i].ProjectileData.speed *= 0.75f;
				}
			}
			Vector2 startScale = target.EnemyScale;
			int cachedLayer = target.gameObject.layer;
			int cachedOutlineLayer = cachedLayer;
			bool depixelatesTargets = this.DepixelatesTargets;
			if (depixelatesTargets)
			{
				target.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
				cachedOutlineLayer = SpriteOutlineManager.ChangeOutlineLayer(target.sprite, LayerMask.NameToLayer("Unpixelated"));
			}
			target.ClearPath();
			bool flag2 = target.knockbackDoer;
			if (flag2)
			{
				target.knockbackDoer.weight *= 1.5f;
				target.MovementSpeed *= 0.75f;
			}
			bool flag3 = target.healthHaver;
			if (flag3)
			{
				target.healthHaver.AllDamageMultiplier *= this.DamageMultiplier;
			}

			while (elapsed < this.BigTime)
			{
				bool flag4 = target == null;
				if (flag4)
				{
					this.affectedEnemies.Remove(target);
					yield break;
				}
				elapsed += target.LocalDeltaTime;
				target.EnemyScale = Vector2.Lerp(startScale, this.TargetScale, elapsed / this.BigTime);
				yield return null;
			}
			elapsed = 0f;
			while (elapsed < this.HoldTime)
			{
				this.m_activeElapsed = elapsed;
				this.m_activeDuration = this.HoldTime;
				bool flag5 = target == null;
				if (flag5)
				{
					this.affectedEnemies.Remove(target);
					yield break;
				}
				elapsed += target.LocalDeltaTime;
				yield return null;
			}
			elapsed = 0f;
			while (elapsed < this.RegrowTime)
			{
				bool flag6 = target == null;
				if (flag6)
				{
					this.affectedEnemies.Remove(target);
					yield break;
				}
				elapsed += target.LocalDeltaTime;
				target.EnemyScale = Vector2.Lerp(this.TargetScale, startScale, elapsed / this.RegrowTime);
				yield return null;
			}
			bool flag7 = target == null;
			if (flag7)
			{
				this.affectedEnemies.Remove(target);
				yield break;
			}
			bool flag8 = target.knockbackDoer;
			if (flag8)
			{
				target.knockbackDoer.weight /= 1.5f;
				target.MovementSpeed /= 0.75f;
				if (target.bulletBank != null && target.healthHaver != null)
				{
					for (int i = 0; i < target.bulletBank.Bullets.Count; i++)
					{
						target.bulletBank.Bullets[i].ProjectileData.speed /= 0.75f;
					}
				}
			}
			bool flag9 = target.healthHaver;
			if (flag9)
			{
				target.healthHaver.AllDamageMultiplier /= this.DamageMultiplier;
			}
			
			bool depixelatesTargets2 = this.DepixelatesTargets;
			if (depixelatesTargets2)
			{
				target.gameObject.layer = cachedLayer;
				SpriteOutlineManager.ChangeOutlineLayer(target.sprite, cachedOutlineLayer);
			}
			this.affectedEnemies.Remove(target);
			target.MovementSpeed /= 0.75f;

			yield break;
		}


		// Token: 0x04000019 RID: 25
		public Vector2 TargetScale = new Vector2(1.5f, 1.5f);

		// Token: 0x0400001A RID: 26
		public float BigTime = 0.1f;

		// Token: 0x0400001B RID: 27
		public float HoldTime = 6f;

		// Token: 0x0400001C RID: 28
		public float RegrowTime = 3f;

		// Token: 0x0400001D RID: 29
		public float DamageMultiplier = 1f;

		// Token: 0x0400001E RID: 30
		public bool DepixelatesTargets = true;

		// Token: 0x0400001F RID: 31
		private List<AIActor> affectedEnemies = new List<AIActor>();

		public static string[] BannedEnemies = new string[]
			{
			   "ba928393c8ed47819c2c5f593100a5bc", // metal_cube_guy
            "2b6854c0849b4b8fb98eb15519d7db1c", // bullet_kin_mech
            "9215d1a221904c7386b481a171e52859", // lead_maiden_fridge
            "226fd90be3a64958a5b13cb0a4f43e97", // musket_kin
            "df4e9fedb8764b5a876517431ca67b86", // bullet_kin_gal_titan_boss
            "1f290ea06a4c416cabc52d6b3cf47266", // bullet_kin_titan_boss
            "c4cf0620f71c4678bb8d77929fd4feff", // bullet_kin_titan
            "3cadf10c489b461f9fb8814abc1a09c1", // minelet
            "21dd14e5ca2a4a388adab5b11b69a1e1", // shelleton
            "1bc2a07ef87741be90c37096910843ab", // chancebulon
            "57255ed50ee24794b7aac1ac3cfb8a95", // gun_cultist
            "4db03291a12144d69fe940d5a01de376", // hollowpoint
            "206405acad4d4c33aac6717d184dc8d4", // apprentice_gunjurer
            "c4fba8def15e47b297865b18e36cbef8", // gunjurer
            "56fb939a434140308b8f257f0f447829", // lore_gunjurer
            "9b2cf2949a894599917d4d391a0b7394", // high_gunjurer
            "43426a2e39584871b287ac31df04b544", // wizbang
            "699cd24270af4cd183d671090d8323a1", // key_bullet_kin // Flee behaviour generates an exception in the logs.
            "a446c626b56d4166915a4e29869737fd", // chance_bullet_kin // His drops sometimes don't appear correctly when resized.
            "22fc2c2c45fb47cf9fb5f7b043a70122", // grip_master // Being tossed from a room from tiny Grip Master can soft lock the game.
            "42be66373a3d4d89b91a35c9ff8adfec", // blobulin
            "b8103805af174924b578c98e95313074", // poisbulin
            "3e98ccecf7334ff2800188c417e67c15", // killithid
            "ffdc8680bdaa487f8f31995539f74265", // muzzle_wisp
            "d8a445ea4d944cc1b55a40f22821ae69", // muzzle_flare
            "c2f902b7cbe745efb3db4399927eab34", // skusket_head
            "98ca70157c364750a60f5e0084f9d3e2", // phaser_spider
            "14ea47ff46b54bb4a98f91ffcffb656d", // rat_candle
            "6ad1cafc268f4214a101dca7af61bc91", // rat
            "479556d05c7c44f3b6abb3b2067fc778", // wall_mimic
            "796a7ed4ad804984859088fc91672c7f", // pedestal_mimic
            "475c20c1fd474dfbad54954e7cba29c1", // tarnisher
            "45192ff6d6cb43ed8f1a874ab6bef316", // misfire_beast
            "eeb33c3a5a8e4eaaaaf39a743e8767bc", // candle_guy
            "56f5a0f2c1fc4bc78875aea617ee31ac", // spectre
            "2feb50a6a40f4f50982e89fd276f6f15", // bullat
            "2d4f8b5404614e7d8b235006acde427a", // shotgat
            "b4666cb6ef4f4b038ba8924fd8adf38f", // grenat
            "7ec3e8146f634c559a7d58b19191cd43", // spirat
            "af84951206324e349e1f13f9b7b60c1a", // skusket
            "e667fdd01f1e43349c03a18e5b79e579", // tutorial_turret
            "41ba74c517534f02a62f2e2028395c58", // faster_tutorial_turret
            "ac986dabc5a24adab11d48a4bccf4cb1", // det
            "3f6d6b0c4a7c4690807435c7b37c35a5", // agonizer
            "cd4a4b7f612a4ba9a720b9f97c52f38c", // lead_maiden
            "98ea2fe181ab4323ab6e9981955a9bca", // shambling_round
            "d5a7b95774cd41f080e517bea07bf495", // revolvenant
            "88f037c3f93b4362a040a87b30770407", // gunreaper
            "1386da0f42fb4bcabc5be8feb16a7c38", // snake
            "566ecca5f3b04945ac6ce1f26dedbf4f", // mine_flayers_claymore
            // Thwomp type enemies
            "f155fd2759764f4a9217db29dd21b7eb", // mountain_cube
            "33b212b856b74ff09252bf4f2e8b8c57", // lead_cube
            "3f2026dc3712490289c4658a2ba4a24b", // flesh_cube
            // Chest Mimics
            "2ebf8ef6728648089babb507dec4edb7", // brown_chest_mimic
            "d8d651e3484f471ba8a2daa4bf535ce6", // blue_chest_mimic
            "abfb454340294a0992f4173d6e5898a8", // green_chest_mimic
            "d8fd592b184b4ac9a3be217bc70912a2", // red_chest_mimic
            "6450d20137994881aff0ddd13e3d40c8", // black_chest_mimic
            "ac9d345575444c9a8d11b799e8719be0", // rat_chest_mimic
			};
		private bool Stop;
	}






}

