using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using FullInspector;
using Brave.BulletScript;
using System.Collections;
using System.Reflection;
using GungeonAPI;
using MonoMod.RuntimeDetour;
using Dungeonator;


namespace FrostAndGunfireItems
{
	
	public class ChampionBlobulord : OverrideBehavior
		{
		
		public override string OverrideAIActorGUID => "1b5810fafbec445d89921a4efb4e42b7";
		public override void DoOverride()
			{
			UnityEngine.Object.Destroy(actor.CurrentGoop);
			FrostandGunFireItems.Strings.Enemies.Set("#POISBULORD", "Poisbulord");
			healthHaver.overrideBossName = "#POISBULORD";
			actor.EffectResistances = new ActorEffectResistance[] { new ActorEffectResistance() { resistAmount = 1, resistType = EffectResistanceType.Poison }, };
			actor.RegisterOverrideColor(new Color(0, 1, 0), "tint");
			healthHaver.SetHealthMaximum(healthHaver.GetMaxHealth() * 1.2f);
			healthHaver.OnPreDeath += (obj) =>
			{
				ChampionReward((actor.sprite.WorldCenter));
			};
			EnemyTools.DisableSuperTinting(actor);
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
			UnityEngine.Object.Destroy(actor.gameObject.GetComponent("GoopDoer"));
			UnityEngine.Object.Destroy(actor.gameObject.GetComponent("GoopDoer"));
			GoopDoer goop = actor.gameObject.AddComponent<GoopDoer>();
			goop.goopDefinition = goopDef;
			goop.positionSource = GoopDoer.PositionSource.HitBoxCenter;
			goop.updateTiming = GoopDoer.UpdateTiming.Always;
			goop.updateFrequency = 0.05f;
			goop.isTimed = false;
			goop.goopTime = 1;
			goop.updateOnPreDeath = true;
			goop.updateOnDeath = false;
			goop.updateOnAnimFrames = true;
			goop.updateOnCollision = false;
			goop.updateOnGrounded = false;
			goop.updateOnDestroy = false;
			goop.defaultGoopRadius = 3f;
			goop.suppressSplashes = false;
			goop.goopSizeVaries = true;
			goop.varyCycleTime = 0.9f;
			goop.radiusMin = 3f;
			goop.radiusMax = 3f;
			goop.goopSizeRandom = true;
			goop.UsesDispersalParticles = false;
			goop.DispersalDensity = 3;
			goop.DispersalMinCoherency = 0.2f;
			goop.DispersalMaxCoherency = 1;
			ShootBehavior shootBehavior = behaviorSpec.AttackBehaviorGroup.AttackBehaviors[1].Behavior as ShootBehavior;
			shootBehavior.BulletScript = new CustomBulletScriptSelector(typeof(BallBurstScript));
			ShootBehavior shootBehavior1 = behaviorSpec.AttackBehaviorGroup.AttackBehaviors[5].Behavior as ShootBehavior;
			shootBehavior1.BulletScript = new CustomBulletScriptSelector(typeof(BubbleScript));
			shootBehavior1.ClearGoop = true;
			shootBehavior1.ClearGoopRadius = 5f;
		}
			public class BallBurstScript : Script 
			{
				protected override IEnumerator Top() 
				{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
						base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
				}
				for (int i = 0; i < 8; i++)
				{
					float aim = this.GetAimDirection((float)((UnityEngine.Random.value >= 0.4f) ? 0 : 1), 8f) + UnityEngine.Random.Range(-10f, 10f);
					this.Fire(new Direction(aim, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new BallBust());

					yield return this.Wait(40);
				}

				yield break;
				}
			}
		public void ChampionReward(Vector2 worldCenter)
		{
			GenericLootTable singleItemRewardTable = GameManager.Instance.RewardManager.CurrentRewardData.SingleItemRewardTable;
			LootEngine.SpawnItem(singleItemRewardTable.SelectByWeight(false), worldCenter, Vector2.up, 1f, true, false, false);
		}
		public class BubbleScript : Script
		{
			// Token: 0x060000F8 RID: 248 RVA: 0x00005A88 File Offset: 0x00003C88
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6e972cd3b11e4b429b888b488e308551").bulletBank.GetBullet("bubble"));
				}
				for (int i = 0; i < 4; i++)
				{
					float num = this.RandomAngle();
					for (int j = 0; j < 32; j++)
					{
						float num2 = num + (float)j * 11.25f;
						this.Fire(new Offset(2f, 0f, num2, string.Empty, DirectionType.Absolute), new Direction(num2, DirectionType.Absolute, -1f), new Speed(4f, SpeedType.Absolute), new BubBullet(i));
					}
				}
				yield return this.Wait(80);
				yield break;
			}

			// Token: 0x040000FE RID: 254
			private const int NumBullets = 32;

			// Token: 0x040000FF RID: 255
			private const int NumWaves = 4;

			// Token: 0x02000043 RID: 67
			public class BubBullet : Bullet
			{
				// Token: 0x060000F9 RID: 249 RVA: 0x00005AA3 File Offset: 0x00003CA3
				public BubBullet(int spawnDelay) : base("bubble", false, false, false)
				{
					this.m_spawnDelay = spawnDelay;
				}

				// Token: 0x060000FA RID: 250 RVA: 0x00005ABC File Offset: 0x00003CBC
				protected override IEnumerator Top()
				{
					this.Projectile.spriteAnimator.Play("bubble_projectile_spawn");
					yield return this.Wait(20);
					this.ChangeSpeed(new Speed(3f, SpeedType.Absolute), 20);
					yield return this.Wait(20);
					this.ChangeSpeed(new Speed(1f, SpeedType.Absolute), 20);
					yield return this.Wait(20);
					this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 20);
					yield return this.Wait(10);
					this.Projectile.spriteAnimator.Play("bubble_projectile_burst");
					this.Vanish(true);
					yield break;
				}
				public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
				{
					if (preventSpawningProjectiles)
					{
						return;
					}
					base.Fire(new Direction(base.GetAimDirection(1f, 14f), DirectionType.Absolute, -1f), new Speed(14f, SpeedType.Absolute), null);
				}
				// Token: 0x04000100 RID: 256
				private int m_spawnDelay;
			}
		}
		public class BallBust : Bullet
			{
			public BallBust() : base("bigBullet", false, false, false)
			{

			}
			protected override IEnumerator Top()
			{
				
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(7f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(4f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(1f, SpeedType.Absolute), 20);
				yield return this.Wait(20);
				this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 20);
				yield return this.Wait(10);
				this.Vanish(true);
				yield break;
			}
			public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				if (preventSpawningProjectiles)
				{
					return;
				}
				for (int i = 0; i < 8; i++)
				{
					base.Fire(new Direction((float)(i * 45), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new WallBullet());
				}
			}
		}
		}
    }

