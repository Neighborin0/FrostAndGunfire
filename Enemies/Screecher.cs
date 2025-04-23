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
	public class Screecher : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "screecher";
		public static GameObject shootpoint;
		public static void BuildPrefab()
		{
			if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				{
					prefab = EnemyBuilder.BuildPrefab("Screecher", guid, "FrostAndGunfireItems/Resources/bubbulon/Idle/bubbulon_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false, false);
					var enemy = prefab.AddComponent<EnemyBehavior>();
					enemy.aiActor.knockbackDoer.weight = float.MaxValue;
					enemy.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
					enemy.aiActor.MovementSpeed = 2f;
					enemy.aiActor.CollisionDamage = 1f;
					enemy.aiActor.IgnoreForRoomClear = false;
					enemy.aiActor.healthHaver.ForceSetCurrentHealth(20f);
					enemy.aiActor.CollisionKnockbackStrength = 1f;		
					enemy.aiActor.healthHaver.SetHealthMaximum(20f, null, false);
					prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/bubbulon/Idle", fps: 9, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/bubbulon/Idle", fps: 9, AnimationType.Idle, DirectionType.TwoWayHorizontal);
					prefab.AddAnimation("die_right", "FrostAndGunfireItems/Resources/bubbulon/Death", fps: 12, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("die_left", "FrostAndGunfireItems/Resources/bubbulon/Death", fps: 12, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("tell_right", "FrostAndGunfireItems/Resources/FireFly/Tell_Right", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
					prefab.AddAnimation("tell_left", "FrostAndGunfireItems/Resources/FireFly/Tell_Left", fps: 6, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
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
					enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
					enemy.aiAnimator.AssignDirectionalAnimation("tell", tell, AnimationType.Other);
					shootpoint = new GameObject("fuck");
					shootpoint.transform.parent = enemy.transform;
					shootpoint.transform.position = enemy.sprite.WorldCenter;
					GameObject m_CachedGunAttachPoint = enemy.transform.Find("fuck").gameObject;
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
					bs.AttackBehaviors = new List<AttackBehaviorBase>
					{
					new MeduziStoneEyesBehavior
					{
						AttackCooldown = 5,
						shootPoint = m_CachedGunAttachPoint,
						RequiresLineOfSight = false,
						anim = "tell",
						
					}
					};
					bs.MovementBehaviors = new List<MovementBehaviorBase>
					{
					new SeekTargetBehavior
					{
					StopWhenInRange = false,
					CustomRange = 10f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 100f,
					MaxActiveRange = 100f
					}
					};
					Game.Enemies.Add("kp:screecher", enemy.aiActor);
				}
			}

		}

		public class EnemyBehavior : BraveBehaviour
		{
			private void Start()
			{
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_BOSS_blobulord_burst_01", base.aiActor.gameObject);
				};
			}
		}

		public class Screech : BasicAttackBehavior
		{
				public Screech()
				{
					this.distortionMaxRadius = 20f;
					this.distortionDuration = 1.5f;
					this.distortionIntensity = 0.5f;
					this.distortionThickness = 0.04f;
				}
				public override void Start()
				{
					base.Start();
					tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
				}

				public override void Upkeep()
				{
					base.Upkeep();
				}

				public override BehaviorResult Update()
				{
					BehaviorResult behaviorResult = base.Update();
					if (behaviorResult != BehaviorResult.Continue)
					{
						return behaviorResult;
					}
					if (!this.IsReady())
					{
						return BehaviorResult.Continue;
					}
					this.m_aiAnimator.PlayUntilFinished(this.anim, true, null, -1f, false);
					this.m_state = State.Firing;
					this.m_aiActor.ClearPath();
					this.m_updateEveryFrame = true;
					return BehaviorResult.RunContinuous;
				}
				public override ContinuousBehaviorResult ContinuousUpdate()
				{
					base.ContinuousUpdate();
					if (this.m_state == State.Firing)
					{
					this.m_distortionCenter = this.shootPoint.transform.position.XY();
					Exploder.DoDistortionWave(this.m_distortionCenter, this.distortionIntensity, this.distortionThickness, this.distortionMaxRadius, this.distortionDuration);
					this.m_timer = this.distortionDuration - BraveTime.DeltaTime;
					this.m_prevWaveDist = 0f;
					this.m_timer -= BraveTime.DeltaTime;
					float num = BraveMathCollege.LinearToSmoothStepInterpolate(0f, this.distortionMaxRadius, 1f - this.m_timer / this.distortionDuration);
						for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
						{
							PlayerController playerController = GameManager.Instance.AllPlayers[i];
							if (!playerController.healthHaver.IsDead)
							{
								if (!playerController.spriteAnimator.QueryInvulnerabilityFrame() && playerController.healthHaver.IsVulnerable)
								{
								Tools.PrintNoID("this is being read");
									Vector2 unitCenter = playerController.specRigidbody.GetUnitCenter(ColliderType.HitBox);
									float num2 = Vector2.Distance(unitCenter, this.m_distortionCenter);
									if (num2 >= this.m_prevWaveDist - 0.25f && num2 <= num + 0.25f)
									{
										GameManager.Instance.StartCoroutine(Attack(playerController));
									}
								}
							}
						}
						this.m_prevWaveDist = num;
					}
					if (this.m_aiAnimator.IsPlaying(this.anim) || this.m_timer > 0f)
					{
						return ContinuousBehaviorResult.Continue;
					}
					return ContinuousBehaviorResult.Finished;
				}

			public IEnumerator Attack(PlayerController playerController)
			{
				Vector2 offset = new Vector2(0, .2f);
			    float value = playerController.stats.GetBaseStatValue(PlayerStats.StatType.Accuracy) * -16f;
				playerController.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, value, playerController);
				var obj = GameObject.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Stun") as GameObject);
				var sprite = obj.GetComponent<tk2dSprite>();
				sprite.PlaceAtPositionByAnchor(playerController.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
				sprite.transform.SetParent(playerController.transform);
				yield return new WaitForSeconds(5f);
				GameObject.Destroy(sprite);
				float value1 = playerController.stats.GetBaseStatValue(PlayerStats.StatType.Accuracy) / -16f;
				playerController.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, value1, playerController);
			}
			public override void EndContinuousUpdate()
				{
					base.EndContinuousUpdate();
					this.m_state = State.Idle;
					this.m_aiAnimator.EndAnimationIf(this.anim);
					this.m_updateEveryFrame = false;
					this.UpdateCooldowns();
				}

				// Token: 0x06004AD0 RID: 19152 RVA: 0x00013A8B File Offset: 0x00011C8B
				public override bool IsOverridable()
				{
					return false;
				}

				public GameObject shootPoint;
				public float distortionMaxRadius = 20f;
				private Vector2 m_distortionCenter;
				public float distortionDuration = 1.5f;
				public string anim;
				public float distortionIntensity = 0.5f;
				public float distortionThickness = 0.04f;
				private Screech.State m_state;
				private float m_timer;
				private float m_prevWaveDist;
				private enum State
				{
					Idle,
					WaitingToFire,
					Firing
				}
			}
		}
}





	





	

