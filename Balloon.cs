using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace FrostAndGunfireItems
{
    public class Balloon : PassiveItem
	{
		

		public int MaxNumberOfCompanions = 2;
		private List<CompanionController> companionsSpawned = new List<CompanionController>();
		private int RoomsCleared;
		public BalloonCompanion companion;

		public static void Init()
        {
			string name = "Blue Balloon";
			string resourcePath = "FrostAndGunfireItems/Resources/balloon/balloon_sprite";
			GameObject gameObject = new GameObject();
			Balloon balloon = gameObject.AddComponent<Balloon>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Yay! To The Mesosphere!";
			string longDesc = "A favorite of Gundead children, this immortal balloon creates a massive shockwave when it's injured. He takes time to regenrate however.";
			ItemBuilder.SetupItem(balloon, shortDesc, longDesc, "kp");
			balloon.quality = PickupObject.ItemQuality.B;
			//balloon.CanBeDropped = false;
			balloon.AddToSubShop(ItemBuilder.ShopType.OldRed);
        }

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.CreateNewCompanion(player);
			player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
			player.OnRoomClearEvent += this.RoomCleared;
		}

		protected override void Update()
		{
			if (Owner != null)
			{


				if (companionsSpawned.Count > 0 && Owner.HasPickupID(520))
				{
					
						Owner.SetIsFlying(true, "Player", true, false);
						Owner.AdditionalCanDodgeRollWhileFlying.BaseValue = true;
				}
				else
				{
					Owner.SetIsFlying(false, "Player", true, false);
				}
			}
		}


		private void RoomCleared(PlayerController obj)
		{
			RoomsCleared += 1;
			if(RoomsCleared == 3)
			{
				if(companionsSpawned.Count < 1)
				{
					this.CreateNewCompanion(obj);
				}
				RoomsCleared -= RoomsCleared;
			}
		}

		private void CreateNewCompanion(PlayerController player)
		{
			bool flag = this.companionsSpawned.Count + 1 == this.MaxNumberOfCompanions;
			bool flag2 = flag;
			if (!flag2)
			{
				bool flag3 = this.companionsSpawned.Count >= this.MaxNumberOfCompanions;
				bool flag4 = !flag3;
				if (flag4)
				{
					AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("balloon");
					Vector3 vector = player.transform.position;
					bool flag5 = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER;
					bool flag6 = flag5;
					if (flag6)
					{
						vector += new Vector3(1.125f, -0.3125f, 0f);
					}
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
					CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
					this.companionsSpawned.Add(orAddComponent);
					orAddComponent.Initialize(player);
					bool flag7 = orAddComponent.specRigidbody;
					bool flag8 = flag7;
					if (flag8)
					{
						PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
					}
					orAddComponent.healthHaver.OnDeath += this.OnDeath;
				}
			}

		}

		private void OnDeath(Vector2 obj)
		{
			this.removeDeadCompanions();
		}
		private void removeDeadCompanions()
		{
			for (int i = this.companionsSpawned.Count - 1; i >= 0; i--)
			{
				bool flag = !this.companionsSpawned[i];
				bool flag2 = flag;
				if (flag2)
				{
					this.companionsSpawned.RemoveAt(i);
				}
				else
				{
					bool flag3 = this.companionsSpawned[i].healthHaver && this.companionsSpawned[i].healthHaver.IsDead;
					bool flag4 = flag3;
					if (flag4)
					{
						UnityEngine.Object.Destroy(this.companionsSpawned[i].gameObject);
						this.companionsSpawned.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004A40 File Offset: 0x00002C40
		private void DestroyCompanions()
		{
			for (int i = this.companionsSpawned.Count - 1; i >= 0; i--)
			{
				bool flag = this.companionsSpawned[i];
				bool flag2 = flag;
				if (flag2)
				{
					UnityEngine.Object.Destroy(this.companionsSpawned[i].gameObject);
				}
				this.companionsSpawned.RemoveAt(i);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004AAC File Offset: 0x00002CAC
		private void HandleNewFloor(PlayerController player)
		{
			int count = this.companionsSpawned.Count;
			this.DestroyCompanions();
			for (int i = 0; i < count; i++)
			{
				this.CreateNewCompanion(player);
			}
		}

	






	}
}
