using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.BossBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using static DirectionalAnimation;

namespace FrostAndGunfireItems
{
	public class RoomMimic : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "Room Mimic";
		public static GameObject shootpoint;
		public static string TargetVFX;
		public static void Init()
		{

			RoomMimic.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			if (prefab == null || !BossBuilder.Dictionary.ContainsKey(guid))
			{
				prefab = BossBuilder.BuildPrefab("RoomMimic", guid, "FrostAndGunfireItems/Resources/RooMimic/Idle/room_mimic_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				BossBuilder.SetupBasicIntro(companion.aiActor, "Roomimic", "Face Off!", "FrostAndGunfireItems/Resources/roomimic_bosscard", Color.cyan, "intro_right");
				companion.gameObject.AddComponent<RoomMimicIntro>();
				companion.aiActor.knockbackDoer.weight = float.MaxValue;
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0.05f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.SetHealthMaximum(500f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.procedurallyOutlined = false;
				companion.aiActor.CanTargetPlayers = true;
				companion.gameObject.AddComponent<RoomMimicIntro>();
				companion.aiActor.healthHaver.SetHealthMaximum(1000f, null, false);
				prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/RooMimic/Idle", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal);
				prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/RooMimic/Idle", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal);
				prefab.AddAnimation("tell_right", "FrostAndGunfireItems/Resources/RooMimic/Tell", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("tell_left", "FrostAndGunfireItems/Resources/RooMimic/Tell", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("swirl_right", "FrostAndGunfireItems/Resources/RooMimic/Swirl", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Loop, assignAnimation: false);
				prefab.AddAnimation("swirl_left", "FrostAndGunfireItems/Resources/RooMimic/Swirl", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Loop, assignAnimation: false);
				prefab.AddAnimation("scream_right", "FrostAndGunfireItems/Resources/RooMimic/Scream", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Loop, assignAnimation: false);
				prefab.AddAnimation("scream_left", "FrostAndGunfireItems/Resources/RooMimic/Scream", fps: 5, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Loop, assignAnimation: false);
				prefab.AddAnimation("prepuke_right", "FrostAndGunfireItems/Resources/RooMimic/Prepuke", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("prepuke_left", "FrostAndGunfireItems/Resources/RooMimic/Prepuke", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("puke_right", "FrostAndGunfireItems/Resources/RooMimic/Puke", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("puke_left", "FrostAndGunfireItems/Resources/RooMimic/Puke", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_right", "FrostAndGunfireItems/Resources/RooMimic/Die", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_left", "FrostAndGunfireItems/Resources/RooMimic/Die", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("suck_right", "FrostAndGunfireItems/Resources/RooMimic/Suck", fps: 4, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("suck_left", "FrostAndGunfireItems/Resources/RooMimic/Suck", fps: 4, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("belch_right", "FrostAndGunfireItems/Resources/RooMimic/Belch", fps: 4, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Loop, assignAnimation: false);
				prefab.AddAnimation("belch_left", "FrostAndGunfireItems/Resources/RooMimic/Belch", fps: 4, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Loop, assignAnimation: false);
				prefab.AddAnimation("chew_right", "FrostAndGunfireItems/Resources/RooMimic/Chew", fps: 9, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("chew_left", "FrostAndGunfireItems/Resources/RooMimic/Chew", fps: 9, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("intro_right", "FrostAndGunfireItems/Resources/RooMimic/Intro", fps: 11, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("intro_left", "FrostAndGunfireItems/Resources/RooMimic/Intro", fps: 11, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				DirectionalAnimation swirl = new DirectionalAnimation()
				{ AnimNames = new string[] { "swirl_right", "swirl_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("swirl", swirl, AnimationType.Other);
				DirectionalAnimation scream = new DirectionalAnimation()
				{ AnimNames = new string[] { "scream_right", "scream_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("scream", scream, AnimationType.Other);
				DirectionalAnimation tell = new DirectionalAnimation()
				{ AnimNames = new string[] { "tell_right", "tell_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("tell", tell, AnimationType.Other);
				DirectionalAnimation prepuke = new DirectionalAnimation()
				{ AnimNames = new string[] { "prepuke_right", "prepuke_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("prepuke", prepuke, AnimationType.Other);
				DirectionalAnimation puke = new DirectionalAnimation()
				{ AnimNames = new string[] { "puke_right", "puke_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("puke", puke, AnimationType.Other);
				DirectionalAnimation intro = new DirectionalAnimation()
				{ AnimNames = new string[] { "intro_right", "intro_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("intro", intro, AnimationType.Other);
				DirectionalAnimation die = new DirectionalAnimation()
				{ AnimNames = new string[] { "die_right", "die_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
				DirectionalAnimation suck = new DirectionalAnimation()
				{ AnimNames = new string[] { "suck_right", "suck_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("suck", suck, AnimationType.Other);
				DirectionalAnimation belch = new DirectionalAnimation()
				{ AnimNames = new string[] { "belch_right", "belch_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("belch", belch, AnimationType.Other);
				DirectionalAnimation chew = new DirectionalAnimation()
				{ AnimNames = new string[] { "chew_right", "chew_left" }, Flipped = new FlipType[] { FlipType.None, FlipType.None }, Type = DirectionType.TwoWayHorizontal, Prefix = string.Empty };
				companion.aiAnimator.AssignDirectionalAnimation("chew", chew, AnimationType.Other);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider

				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 10,
					ManualWidth = 101,
					ManualHeight = 27,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 10,
					ManualWidth = 101,
					ManualHeight = 27,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});

				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("attach");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("attach").gameObject;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = false,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f
				}
			};
				bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>
				{

					new AttackBehaviorGroup.AttackGroupItem()
					{

						Probability = 1,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
					   BulletScript = new CustomBulletScriptSelector(typeof(SwirlScript)),
					   LeadAmount = 0f,
					   AttackCooldown = 3f,
					   GroupCooldown = 10f,
					   FireAnimation = "swirl",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Swirl Whirly"

					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(AAAAAAAAAAAAAAScript)),
						LeadAmount = 0f,
						AttackCooldown = 3f,
						GroupCooldown = 10f,
						FireAnimation = "scream",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "SCREAMMMMMMMM AAAAAAAAAHHHHHHHHH"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1f,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(SkeletonBulletScript)),
						LeadAmount = 0f,
						MaxUsages = 2,
						AttackCooldown = 6f,
						TellAnimation = "prepuke",
						FireAnimation = "puke",
						RequiresLineOfSight = false,
						GroupCooldown = 10f,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Skeleton Spookerino Wowie Zowie AHHHHH AHH, The Skeletons Are Eating Me, AHHHHHHH, man this name is stupid as hell. Stop reading my shit. Stop posting this on the Discord AHHH. At least hope you enjoy the mod tho."
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1f,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(BubbleScript)),
						LeadAmount = 0f,
						AttackCooldown = 5f,
						TellAnimation = "chew",
						FireAnimation = "belch",
						GroupCooldown = 10f,
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Bubble Blowing Diaper Baby"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(SpitUpScript)),
						LeadAmount = 0f,
						AttackCooldown = 3.5f,
						GroupCooldown = 10f,
						TellAnimation = "tell",
						FireAnimation = "suck",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Cuck and Suck"
					},		
				};
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("kp:room_mimic", companion.aiActor);

			}
		}
	}

	public class SwirlScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				    base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
			}
				for (int i = 0; i < 195; i++)
				{
					
					base.Fire(new Direction(360 - (i * 20), Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new WallBullet());
				yield return this.Wait(4);
				if (i % 10 == 9)
				{
					this.Fire(new Direction(0f, Brave.BulletScript.DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new BurstBullet());
				}
			}
			
			yield break;
			}

		}

	public class BubbleScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			AkSoundEngine.PostEvent("Play_BOSS_doormimic_vomit_01", this.BulletBank.aiActor.gameObject);
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6e972cd3b11e4b429b888b488e308551").bulletBank.GetBullet("bubble"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
			}
			for (int i = 0; i < 30; i++)
			{
				this.Fire(new Direction(UnityEngine.Random.Range(0f, 360f), Brave.BulletScript.DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(2f, 4f), SpeedType.Absolute), new BubbleBullet(i));
				yield return this.Wait(10);
			}
			yield break;
		}

	}
	public class BubbleBullet : Bullet
	{
		public BubbleBullet(int spawnDelay) : base("bubble", false, false, false)
		{

		}
		protected override IEnumerator Top()
		{
			this.Projectile.sprite.HeightOffGround = 4f;
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
			base.Fire(new Direction(base.GetAimDirection(1f, 10f), Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), null);
		}
	}

		public class SkeletonBulletScript : Script
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("5288e86d20184fa69c91ceb642d31474").bulletBank.GetBullet("skull"));
			}
			AkSoundEngine.PostEvent("Play_BOSS_doormimic_vomit_01", this.BulletBank.aiActor.gameObject);
			yield return this.Wait(22);
			base.Fire(new Direction(-40f, Brave.BulletScript.DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SpawnSkeletonBullet()); 
			base.Fire(new Direction(40f, Brave.BulletScript.DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SpawnSkeletonBullet()); 
			yield break;
		}
	}

	public class SpawnSkeletonBullet : Bullet
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x000085A7 File Offset: 0x000067A7
		public SpawnSkeletonBullet() : base("skull", false, false, false)
		{
		}

		protected override IEnumerator Top()
		{
			yield return this.Wait(30);
			this.ChangeSpeed(new Speed(3f, SpeedType.Absolute), 20);
			yield return this.Wait(20);
			this.ChangeSpeed(new Speed(2f, SpeedType.Absolute), 20);
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
			var list = new List<string> {
				//"shellet",
				"336190e29e8a4f75ab7486595b700d4a"
			};
			string guid = BraveUtility.RandomElement<string>(list);
			var Enemy = EnemyDatabase.GetOrLoadByGuid(guid);
			AIActor.Spawn(Enemy.aiActor, this.Projectile.sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
		}
		
	}


	public class AAAAAAAAAAAAAAScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
			}
			AkSoundEngine.PostEvent("Play_ENM_beholster_intro_01", this.BulletBank.aiActor.gameObject);
			for (int k = 0; k < 70; k++)
			{
				this.Fire(new Direction((float)k * 360f / 64f, Brave.BulletScript.DirectionType.Absolute, -1f), new Speed (10f, SpeedType.Absolute), new WallBullet());
			}
			yield return Wait(75);
			AkSoundEngine.PostEvent("Play_ENM_beholster_intro_01", this.BulletBank.aiActor.gameObject);
			for (int k = 0; k < 70; k++)
			{
				this.Fire(new Direction((float)k * 360f / 64f, Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new WallBullet());
			}
			yield return Wait(72);
			AkSoundEngine.PostEvent("Play_ENM_beholster_intro_01", this.BulletBank.aiActor.gameObject);
			for (int k = 0; k < 70; k++)
			{
				this.Fire(new Direction((float)k * 360f / 64f, Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new WallBullet());
			}
			yield return Wait(25);
			yield break;
		}

	}

	public class SpitUpScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
			}
			AkSoundEngine.PostEvent("Play_ENM_beholster_intro_01", this.BulletBank.aiActor.gameObject);
			for (int k = 0; k < 70; k++)
			{
				this.Fire(new Direction(UnityEngine.Random.Range(0f, 360f), Brave.BulletScript.DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(8f, 13f), SpeedType.Absolute), new ReverseBullet());
			}
			yield break;
		}
	}


	public class BurstBullet : Bullet
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x000085A7 File Offset: 0x000067A7
		public BurstBullet() : base("reversible", false, false, false)
		{
		}

		protected override IEnumerator Top()
		{
			this.Projectile.spriteAnimator.Play();
			yield break;
		}
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				base.Fire(new Direction((float)(i * 45), Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new WallBullet());
			}
		}
	}

	public class ReverseBullet : Bullet
			{
				// Token: 0x06000A91 RID: 2705 RVA: 0x00030B38 File Offset: 0x0002ED38
				public ReverseBullet() : base("reversible", false, false, false)
				{
				}

				// Token: 0x06000A92 RID: 2706 RVA: 0x00030B48 File Offset: 0x0002ED48
				protected override IEnumerator Top()
				{
			float speed = this.Speed;
			yield return this.Wait(100);
			this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 20);
			yield return this.Wait(40);
			this.Direction += 180f;
			this.Projectile.spriteAnimator.Play();
			yield return this.Wait(60);
			this.ChangeSpeed(new Speed(speed, SpeedType.Absolute), 40);
			yield return this.Wait(130);
			this.Vanish(true);
			yield break;
		}
			}

			public class WallBullet : Bullet
			{
				// Token: 0x06000A91 RID: 2705 RVA: 0x00030B38 File Offset: 0x0002ED38
				public WallBullet() : base("default", false, false, false)
				{
				}

			}

			public class EnemyBehavior : BraveBehaviour
			{
				private void Start()
				{
					//base.aiActor.HasBeenEngaged = true;
			base.aiActor.healthHaver.OnPreDeath += (obj) =>
			{
				AkSoundEngine.PostEvent("Play_ENM_beholster_death_01", base.aiActor.gameObject);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.ROOMIMIC_KILLED, true);
				//Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(spawnspot);
				//chest2.IsLocked = false;

			};
			base.healthHaver.healthHaver.OnDeath += (obj) =>
			{
				for (int i = 0; i < 2; i++)
				{
					Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
					chest2.IsLocked = false;
				}


			}; ;
			this.aiActor.knockbackDoer.SetImmobile(true, "laugh");
				}


			}
		}
	





	

