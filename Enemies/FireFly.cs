using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using static DirectionalAnimation;

namespace FrostAndGunfireItems
{
	public class Firefly : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "firefly";
		public static GameObject shootpoint;
		public static void Init()
		{
			Firefly.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				{
					prefab = EnemyBuilder.BuildPrefab("Firefly", guid, "FrostAndGunfireItems/Resources/FireFly/Idle_Right/firefly_idle_right_001", new IntVector2(0, 0), new IntVector2(0, 0), false);
					var enemy = prefab.AddComponent<EnemyBehavior>();
					enemy.aiActor.knockbackDoer.weight = 50;
					enemy.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
					enemy.aiActor.MovementSpeed = 3f;
					enemy.aiActor.CollisionDamage = 1f;
					enemy.aiActor.IgnoreForRoomClear = false;
					enemy.aiActor.healthHaver.ForceSetCurrentHealth(12f);
					enemy.aiActor.CollisionKnockbackStrength = 1f;
					enemy.aiActor.healthHaver.SetHealthMaximum(12f, null, false);
					prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/FireFly/Idle_Right", fps: 32, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/FireFly/Idle_Left", fps: 32, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("tell_right", "FrostAndGunfireItems/Resources/FireFly/Tell_Right", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("tell_left", "FrostAndGunfireItems/Resources/FireFly/Tell_Left", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("die_right", "FrostAndGunfireItems/Resources/FireFly/Die_R", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("die_left", "FrostAndGunfireItems/Resources/FireFly/Die_L", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("attack_right", "FrostAndGunfireItems/Resources/FireFly/Attack_Right", fps: 3, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("attack_left", "FrostAndGunfireItems/Resources/FireFly/Attack_Left", fps: 3, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					DirectionalAnimation die = new DirectionalAnimation()
					{
						AnimNames = new string[] { "die_right", "die_left" },
						Flipped = new FlipType[] { FlipType.None, FlipType.None },
						Type = DirectionType.TwoWayHorizontal,
						Prefix = string.Empty
					};
					DirectionalAnimation tell = new DirectionalAnimation()
					{
						AnimNames = new string[] { "tell_right", "tell_left" },
						Flipped = new FlipType[] { FlipType.None, FlipType.None },
						Type = DirectionType.TwoWayHorizontal,
						Prefix = string.Empty
					};
					DirectionalAnimation attack = new DirectionalAnimation()
					{
						AnimNames = new string[] { "attack_right", "attack_left" },
						Flipped = new FlipType[] { FlipType.None, FlipType.None },
						Type = DirectionType.TwoWayHorizontal,
						Prefix = string.Empty
					};
					enemy.aiAnimator.AssignDirectionalAnimation("tell", tell, AnimationType.Other);
					enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
					enemy.aiAnimator.AssignDirectionalAnimation("attack", attack, AnimationType.Other);
					shootpoint = new GameObject("center");
					shootpoint.transform.parent = enemy.transform;
					shootpoint.transform.position = enemy.sprite.WorldCenter;
					GameObject position = enemy.transform.Find("center").gameObject;
					var bs = prefab.GetComponent<BehaviorSpeculator>();
					bs.TargetBehaviors = new List<TargetBehaviorBase>
					{
					new TargetPlayerBehavior
					{
					Radius = 35f,
					LineOfSight = true,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f,
					}
					};
					bs.AttackBehaviors = new List<AttackBehaviorBase>() {
					new ShootBehavior() {
					ShootPoint = position,
					BulletScript = new CustomBulletScriptSelector(typeof(SpawnFlys)),
					LeadAmount = 0f,
					AttackCooldown = 8f,
					ContinueAimingDuringTell = true,
					TellAnimation = "tell",
					FireAnimation = "attack",
					RequiresLineOfSight = true,
					Uninterruptible = false,
					}
					};
					bs.MovementBehaviors = new List<MovementBehaviorBase>
					{
					new SeekTargetBehavior
					{
					StopWhenInRange = false,
					CustomRange = 15f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
					}
					};
					Game.Enemies.Add("kp:firefly", enemy.aiActor);
				}
			}

		}

		public class EnemyBehavior : BraveBehaviour
		{

			private RoomHandler m_StartRoom;
			
			private void Update()
			{
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
			}
			private void CheckPlayerRoom()
			{
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					GameManager.Instance.StartCoroutine(LateEngage());
				}
			}

			private IEnumerator LateEngage()
			{
				yield return new WaitForSeconds(0.5f);
				base.aiActor.HasBeenEngaged = true;
				yield break;
			}
			private void Start()
			{
				Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
				mat.mainTexture = base.aiActor.sprite.renderer.material.mainTexture;
				mat.SetColor("_EmissiveColor", new Color32(255, 0, 0, 255));
				mat.SetFloat("_EmissiveColorPower", 1.55f);
				mat.SetFloat("_EmissivePower", 100);
				aiActor.sprite.renderer.material = mat;
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
			}
		}

		public class SpawnFlys : Script 
		{
			protected override IEnumerator Top() 
			{
				AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", this.BulletBank.aiActor.gameObject);
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
				}
				AkSoundEngine.PostEvent("Play_WPN_bees_impact_01", this.BulletBank.aiActor.gameObject);
				for (int i = 0; i < 10; i++)
				{
					base.Fire(new Direction(UnityEngine.Random.Range(-20, 20f), Brave.BulletScript.DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(4f, 7f), SpeedType.Absolute), new FlyBullet());
					yield return this.Wait(2);
				}
				yield break;
			}
		}


		public class FlyBullet : Bullet
		{
			public FlyBullet() : base("reversible", false, false, false)
			{
			}
			protected override IEnumerator Top()
			{
				yield return this.Wait(10);
				this.Projectile.spriteAnimator.Play();
				this.Projectile.collidesOnlyWithPlayerProjectiles = true;
				this.Projectile.collidesWithProjectiles = true;
				this.Projectile.UpdateCollisionMask();
				for (int i = 0; i < 600; i++)
				{
					float aim = this.GetAimDirection(5f, UnityEngine.Random.Range(4f, 7f));
					float delta = BraveMathCollege.ClampAngle180(aim - this.Direction);
					if (Mathf.Abs(delta) > 100f)
					{
						yield break;
					}
					this.Direction += Mathf.MoveTowards(0f, delta, 3f);
					yield return this.Wait(2);
				}
				this.Vanish(false);
				yield break;
			}
		}
	}
	}


	





	

