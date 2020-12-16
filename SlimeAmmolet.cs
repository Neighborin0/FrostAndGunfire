using System;
using System.Collections;
using System.Reflection;
using Dungeonator;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x0200001C RID: 28
	public class SlimeAmmolet : BlankModificationItem
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x0000816C File Offset: 0x0000636C
		public static void Init()
		{
			string name = "Slime Ammolet";
			string resourcePath = "FrostAndGunfireItems/Resources/slime_charm";
			GameObject gameObject = new GameObject(name);
			SlimeAmmolet slimeAmmolet = gameObject.AddComponent<SlimeAmmolet>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Blanks Slow";
			string longDesc = "Blanks apply slowness for three seconds.\n\nAt the core of this amulet lies a member of the Blobulonian army who has absorbed the powers of a blank.";
			ItemBuilder.SetupItem(slimeAmmolet, shortDesc, longDesc, "kp");
			ItemBuilder.AddPassiveStatModifier(slimeAmmolet, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
			slimeAmmolet.quality = PickupObject.ItemQuality.B;
			slimeAmmolet.AddToSubShop(ItemBuilder.ShopType.OldRed);
			slimeAmmolet.AddToSubShop(ItemBuilder.ShopType.Goopton);
			SlimeAmmolet.ID2 = slimeAmmolet.PickupObjectId;
		}
		private static int ID2;
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
	
			
		}

		private static Hook BlankHook = new Hook(
  typeof(SilencerInstance).GetMethod("ProcessBlankModificationItemAdditionalEffects", BindingFlags.Instance | BindingFlags.NonPublic),
  typeof(SlimeAmmolet).GetMethod("BlankModHook")
);

		protected override void Update()
		{
			bool flag = Owner;
			if (flag)
			{
				if (Owner.HasPickupID(205))
				{
					this.m_poisImmunity = new DamageTypeModifier();
					this.m_poisImmunity.damageMultiplier = 0f;
					this.m_poisImmunity.damageType = CoreDamageTypes.Poison;
					Owner.healthHaver.damageTypeModifiers.Add(this.m_poisImmunity);
				}
			}
		}


		public static void BlankModHook(Action<SilencerInstance, BlankModificationItem, Vector2, PlayerController> orig, SilencerInstance silencer, BlankModificationItem bmi, Vector2 centerPoint, PlayerController user)
		{
			orig(silencer, bmi, centerPoint, user);
			bool flag = user.HasPickupID(SlimeAmmolet.ID2);
			if (flag)
			{
				if(user.HasPickupID(205))
				{
					AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
					GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
					DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
					goopManagerForGoopType.TimedAddGoopCircle(user.sprite.WorldCenter, 3f, 0.35f, false);
				}
				RoomHandler currentRoom = user.CurrentRoom;
				if (currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
				{

					foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
					{
						bool flag4 = aiactor.behaviorSpeculator != null;
						bool flag5 = flag4;
						bool flag6 = flag5;
						if (flag6)
						{

							
								GameManager.Instance.StartCoroutine(SlimeAmmolet.RemoveSlow(aiactor));
							

						}
					}
				}

			}


		}


	
		


		

		private static IEnumerator RemoveSlow(AIActor aiactor)
		{
			Vector2 offset = new Vector2(0, .25f);
			aiactor.MovementSpeed *= 0.5f;
			var obj = GameObject.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Speed_Status") as GameObject);
			tk2dSprite sprite = obj.GetComponent<tk2dSprite>();
			sprite.PlaceAtPositionByAnchor(aiactor.sprite.WorldTopCenter + offset, tk2dBaseSprite.Anchor.LowerCenter);
			sprite.transform.SetParent(aiactor.transform);
			yield return new WaitForSeconds(3f);
			aiactor.MovementSpeed /= 0.5f;
			UnityEngine.Object.Destroy(sprite);
			yield break;
		}

		public override DebrisObject Drop(PlayerController player)
		{
		
			
			DebrisObject debrisObject = base.Drop(player);
			SlimeAmmolet component = debrisObject.GetComponent<SlimeAmmolet>();
			component.m_pickedUpThisRun = true;
			return debrisObject;
		}

	

		private DamageTypeModifier m_poisImmunity;


	}
}
