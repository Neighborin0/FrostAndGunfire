using System;
using System.Collections;
using Dungeonator;
using FrostAndGunfireItems;
using ItemAPI;
using UnityEngine;
// Token: 0x02001337 RID: 4919
public class PlantCompanionItem : PlayerItem
{

	
	public static void Init()
	{
		string name = "Germinate";
		string resourcePath = "FrostAndGunfireItems/Resources/capsule";
		GameObject gameObject = new GameObject();
		PlantCompanionItem cc = gameObject.AddComponent<PlantCompanionItem>();
		ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
		string shortDesc = "Go!";
		string longDesc = "An ancient device to used capture creatures. Previously owned by the famed monster tamer, Brash Ketchup, this capsule contains some rare creatures from the Gungeon that will happily fight for you.";
		ItemBuilder.SetupItem(cc, shortDesc, longDesc, "kp");
		cc.quality = PickupObject.ItemQuality.EXCLUDED;
		cc.CompanionGuid = Spike.guid;
		ItemBuilder.SetCooldownType(cc, ItemBuilder.CooldownType.Timed, 1f);
		
	}

	

	private void CreateCompanion(PlayerController owner)
	{

			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
			IntVector2 value = IntVector2.Max(this.CustomClearance, orLoadByGuid.Clearance);
			orLoadByGuid.aiShooter.equippedGunId = owner.CurrentGun.PickupObjectId;
			owner.inventory.DestroyCurrentGun();
			IntVector2? nearestAvailableCell = owner.CurrentRoom.GetNearestAvailableCell(owner.transform.position.XY(), new IntVector2?(value), new CellTypes?(CellTypes.FLOOR), false, null);
			if (nearestAvailableCell == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, (nearestAvailableCell.Value.ToVector2() + this.CustomOffset).ToVector3ZUp(0f), Quaternion.identity);
			this.m_extantCompanion = gameObject;
			CompanionController orAddComponent = this.m_extantCompanion.GetOrAddComponent<CompanionController>();
			orAddComponent.Initialize(owner);
	}

	

	// Token: 0x06006F94 RID: 28564 RVA: 0x002B5371 File Offset: 0x002B3571
	protected override void DoEffect(PlayerController user)
	{
		this.CreateCompanion(user);

	}
	public override bool CanBeUsed(PlayerController user)
    {
		return user.CurrentGun != null && user.CurrentGun.CanActuallyBeDropped(user) && !user.CurrentGun.InfiniteAmmo && user.GetComponent<BasicBeamController>() == null;
	}

	// Token: 0x06006F95 RID: 28565 RVA: 0x002B5380 File Offset: 0x002B3580
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
	}

	protected override void OnDestroy()
	{

		base.OnDestroy();
	}

	// Token: 0x04006EBD RID: 28349
	[EnemyIdentifier]
	public string CompanionGuid;

	public string CompanionGuid1;

	public string CompanionGuid2;



	// Token: 0x04006EC0 RID: 28352
	public IntVector2 CustomClearance;

	// Token: 0x04006EC1 RID: 28353
	public Vector2 CustomOffset;

	// Token: 0x04006EC2 RID: 28354
	public bool IsTimed;

	// Token: 0x04006EC3 RID: 28355
	public float Lifespan;

	// Token: 0x04006EC4 RID: 28356
	public string IntroDirectionalAnimation;

	// Token: 0x04006EC5 RID: 28357
	public string OutroDirectionalAnimation;

	// Token: 0x04006EC6 RID: 28358
	public GameObject DepartureVFXPrefab;

	// Token: 0x04006EC7 RID: 28359
	private GameObject m_extantCompanion;

	// Token: 0x04006EC8 RID: 28360
	private GameObject m_extantSecondCompanion;

	// Token: 0x04006EC9 RID: 28361
	
}
