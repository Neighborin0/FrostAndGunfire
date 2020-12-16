using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using Dungeonator;
using System.Collections;

namespace FrostAndGunfireItems
{
    public class Squire : CompanionItem
	{
        public static GameObject prefab;
        public static readonly string guid = "squire";
		

		public static void Init()
        {
			string name = "Squire";
			string resourcePath = "FrostAndGunfireItems/Resources/squire/squire";
			GameObject gameObject = new GameObject();
			Squire squire = gameObject.AddComponent<Squire>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Chivalry";
			string longDesc = "After failing to protect his master from a stray bullet, this Gun Squire went into self inflicted exile for years. With you there is a new chance for this lonely squire. He'll protect you to the bitter end.";
			ItemBuilder.SetupItem(squire, shortDesc, longDesc, "kp");
			squire.quality = PickupObject.ItemQuality.B;
			squire.CompanionGuid = Squire.guid;
			Squire.BuildPrefab();
        }

	
		public static void BuildPrefab()
        {
            if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
                return;

            //Create the prefab with a starting sprite and hitbox offset/size
            prefab = CompanionBuilder.BuildPrefab("Sir Shields A Lot", guid, "FrostAndGunfireItems/Resources/squire/Idle/squire_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

            //Add a companion component to the prefab (could be a custom class)
            var companion = prefab.AddComponent<SquireBehavior>();
			companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
			companion.aiActor.MovementSpeed = 6f;
			companion.aiActor.HasShadow = false;
			//Add all of the needed animations (most of the animations need to have specific, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			//prefab.AddAnimation("attack", "FrostAndGunfireItems/Resources/squire/Attack", fps: 10, AnimationType.Other, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_right", "FrostAndGunfireItems/Resources/squire/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_left", "FrostAndGunfireItems/Resources/squire/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			var bs = prefab.GetComponent<BehaviorSpeculator>();
            bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });
        }

		public class SquireBehavior : CompanionController
		{
			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325
			private void Start()
			{
			
				base.spriteAnimator.Play("idle");
				this.Owner = this.m_owner;
				
			}

			public override void Update()
			{
			
				if (Owner != null)
				{
					RoomHandler currentRoom = Owner.CurrentRoom;
					if (currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
					{
						List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
						bool flag2 = activeEnemies == null | activeEnemies.Count <= 0;
						if (!flag2 && !Stop)
						{
							
								GameManager.Instance.StartCoroutine(Shield());
								Stop = true;
							
						}
					}
					else
					{
						GameManager.Instance.StopCoroutine(Shield());
					}
				}
			}
		

			public IEnumerator Shield()
			{

				yield return new WaitForSeconds(6f);
				RoomHandler currentRoom = Owner.CurrentRoom;
				Gun gun = PickupObjectDatabase.GetById(380) as Gun;
				Gun currentGun = Owner.CurrentGun;
				GameObject gameObject = gun.ObjectToInstantiateOnReload.gameObject;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, Owner.sprite.WorldTopCenter, Quaternion.identity);
				SingleSpawnableGunPlacedObject @interface = GameObjectExtensions.GetInterface<SingleSpawnableGunPlacedObject>(gameObject2);
				BreakableShieldController component = gameObject2.GetComponent<BreakableShieldController>();
				bool flag3 = gameObject2;
				if (flag3)
				{
				   
					@interface.Initialize(currentGun);
					component.Initialize(currentGun);

				}
				if(Owner.HasPickupID(380))
				{
					Stop = false;
					yield return new WaitForSeconds(8f);
					component.Deactivate();
				}
				else
				{
					Stop = false;
					yield return new WaitForSeconds(5f);
					component.Deactivate();
				}
				yield break;

			}


			private bool Stop;
			public PlayerController Owner;
		}



	}
}
