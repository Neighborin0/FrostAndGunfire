using System;
using System.Collections;
using Dungeonator;
using FrostAndGunfireItems;
using ItemAPI;
using UnityEngine;
// Token: 0x02001337 RID: 4919
public class CC : PlayerItem
{

	public CC()
	{
		this.Lifespan = 60f;
	
	}

	public static void Init()
	{
		string name = "Creature Capsule";
		string resourcePath = "FrostAndGunfireItems/Resources/capsule";
		GameObject gameObject = new GameObject();
		CC cc = gameObject.AddComponent<CC>();
		ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
		string shortDesc = "Go!";
		string longDesc = "An ancient device to used capture creatures. Previously owned by the famed monster tamer, Brash Ketchup, this capsule contains some rare creatures from the Gungeon that will happily fight for you.";
		ItemBuilder.SetupItem(cc, shortDesc, longDesc, "kp");
		cc.quality = PickupObject.ItemQuality.EXCLUDED;
		cc.CompanionGuid = Frog.guid;
		cc.CompanionGuid1 = FChamber.guid;
		cc.CompanionGuid2 = Mushroom.guid;
		cc.IsTimed = true;
		cc.Lifespan = 60f;
		ItemBuilder.SetCooldownType(cc, ItemBuilder.CooldownType.Damage, 350f);
		
	}

	

	// Token: 0x06006F91 RID: 28561 RVA: 0x002B5094 File Offset: 0x002B3294
	private void CreateCompanion(PlayerController owner)
	{
		int IChoose = UnityEngine.Random.Range(0, 3);
		bool flag = IChoose == 0;
		if (flag)
		{
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
			IntVector2 value = IntVector2.Max(this.CustomClearance, orLoadByGuid.Clearance);
			IntVector2? nearestAvailableCell = owner.CurrentRoom.GetNearestAvailableCell(owner.transform.position.XY(), new IntVector2?(value), new CellTypes?(CellTypes.FLOOR), false, null);
			if (nearestAvailableCell == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, (nearestAvailableCell.Value.ToVector2() + this.CustomOffset).ToVector3ZUp(0f), Quaternion.identity);
			this.m_extantCompanion = gameObject;
			CompanionController orAddComponent = this.m_extantCompanion.GetOrAddComponent<CompanionController>();
			orAddComponent.Initialize(owner);
			if (this.IsTimed)
			{
				owner.StartCoroutine(this.HandleLifespan(gameObject, owner));
			}
		}
		bool flag2 = IChoose == 1;
		if (flag2)
		{
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid1);
			IntVector2 value = IntVector2.Max(this.CustomClearance, orLoadByGuid.Clearance);
			IntVector2? nearestAvailableCell = owner.CurrentRoom.GetNearestAvailableCell(owner.transform.position.XY(), new IntVector2?(value), new CellTypes?(CellTypes.FLOOR), false, null);
			if (nearestAvailableCell == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, (nearestAvailableCell.Value.ToVector2() + this.CustomOffset).ToVector3ZUp(0f), Quaternion.identity);
			this.m_extantCompanion = gameObject;
			CompanionController orAddComponent = this.m_extantCompanion.GetOrAddComponent<CompanionController>();
			orAddComponent.Initialize(owner);
			if (this.IsTimed)
			{
				owner.StartCoroutine(this.HandleLifespan(gameObject, owner));
			}
		}
		bool flag3 = IChoose == 2;
		if (flag3)
		{
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid2);
			IntVector2 value = IntVector2.Max(this.CustomClearance, orLoadByGuid.Clearance);
			IntVector2? nearestAvailableCell = owner.CurrentRoom.GetNearestAvailableCell(owner.transform.position.XY(), new IntVector2?(value), new CellTypes?(CellTypes.FLOOR), false, null);
			if (nearestAvailableCell == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, (nearestAvailableCell.Value.ToVector2() + this.CustomOffset).ToVector3ZUp(0f), Quaternion.identity);
			this.m_extantCompanion = gameObject;
			CompanionController orAddComponent = this.m_extantCompanion.GetOrAddComponent<CompanionController>();
			orAddComponent.Initialize(owner);
			if (this.IsTimed)
			{
				owner.StartCoroutine(this.HandleLifespan(gameObject, owner));
			}
		}
		
		
	}

	// Token: 0x06006F92 RID: 28562 RVA: 0x002B5284 File Offset: 0x002B3484
	private void DestroyCompanion()
	{
		if (this.m_extantCompanion)
		{
			if (!string.IsNullOrEmpty(this.OutroDirectionalAnimation))
			{
				AIAnimator component = this.m_extantCompanion.GetComponent<AIAnimator>();
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleDeparture(true, component));
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_extantCompanion);
				this.m_extantCompanion = null;
			}
		}
		if (this.m_extantSecondCompanion)
		{
			if (!string.IsNullOrEmpty(this.OutroDirectionalAnimation))
			{
				AIAnimator component2 = this.m_extantSecondCompanion.GetComponent<AIAnimator>();
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleDeparture(false, component2));
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_extantSecondCompanion);
				this.m_extantSecondCompanion = null;
			}
		}
	}

	// Token: 0x06006F93 RID: 28563 RVA: 0x002B5348 File Offset: 0x002B3548
	private IEnumerator HandleDeparture(bool isPrimary, AIAnimator anim)
	{
		anim.behaviorSpeculator.enabled = false;
		anim.specRigidbody.Velocity = Vector2.zero;
		anim.aiActor.ClearPath();
		anim.PlayForDuration(this.OutroDirectionalAnimation, 3f, true, null, -1f, false);
		float animLength = anim.GetDirectionalAnimationLength(this.OutroDirectionalAnimation);
		GameObject extantCompanion;
		if (isPrimary)
		{
			extantCompanion = this.m_extantCompanion;
			this.m_extantCompanion = null;
		}
		else
		{
			extantCompanion = this.m_extantSecondCompanion;
			this.m_extantSecondCompanion = null;
		}
		float elapsed = 0f;
		while (elapsed < animLength)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		GameObject instanceVFXObject = null;
		if (anim)
		{
			instanceVFXObject = UnityEngine.Object.Instantiate<GameObject>(this.DepartureVFXPrefab);
			tk2dBaseSprite component = instanceVFXObject.GetComponent<tk2dBaseSprite>();
			component.transform.position = anim.sprite.transform.position;
		}
		UnityEngine.Object.Destroy(extantCompanion);
		if (instanceVFXObject)
		{
			Vector3 startPosition = instanceVFXObject.transform.position;
			elapsed = 0f;
			while (elapsed < 1.5f)
			{
				elapsed += BraveTime.DeltaTime;
				instanceVFXObject.transform.position = Vector3.Lerp(startPosition, startPosition + new Vector3(0f, 75f, 0f), elapsed / 1.5f);
				yield return null;
			}
			UnityEngine.Object.Destroy(instanceVFXObject);
		}
		yield break;
	}

	// Token: 0x06006F94 RID: 28564 RVA: 0x002B5371 File Offset: 0x002B3571
	protected override void DoEffect(PlayerController user)
	{
		this.DestroyCompanion();
		this.CreateCompanion(user);
		AkSoundEngine.PostEvent("Play_OBJ_teleport_arrive_01", this.LastOwner.gameObject);
		DoWarpEffect(false);
	}

	private void DoWarpEffect(bool playSound = true)
	{
		if (!m_extantCompanion) return;

		var position = m_extantCompanion.GetComponent<tk2dSprite>().WorldBottomCenter;
		GameObject warpFXPrefab = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
		if (position != null)
		{
			if (playSound)
				AkSoundEngine.PostEvent("Play_OBJ_teleport_arrive_01", this.LastOwner.gameObject);
			GameObject warpFX = GameObject.Instantiate<GameObject>(warpFXPrefab);
			warpFX.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(position, tk2dBaseSprite.Anchor.LowerCenter);
			warpFX.transform.position = warpFX.transform.position.Quantize(0.0625f);
			warpFX.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		}
	}
	// Token: 0x06006F95 RID: 28565 RVA: 0x002B5380 File Offset: 0x002B3580
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
		if (base.IsCurrentlyActive)
		{
			base.IsCurrentlyActive = false;
			if (this.m_extantCompanion)
			{
				this.DestroyCompanion();
			}
		}
	}

	// Token: 0x06006F96 RID: 28566 RVA: 0x002B53B4 File Offset: 0x002B35B4
	private IEnumerator HandleLifespan(GameObject targetCompanion, PlayerController owner)
	{
		this.IsCurrentlyActive = true;
		float elapsed = 0f;
		this.m_activeDuration = this.Lifespan;
		this.m_activeElapsed = 0f;
		while (elapsed < this.Lifespan)
		{
			elapsed += BraveTime.DeltaTime;
			this.m_activeElapsed = elapsed;
			yield return null;
		}
		this.IsCurrentlyActive = false;
		if (this.m_extantCompanion == targetCompanion)
		{
			this.DestroyCompanion();
		}
		yield break;
	}

	// Token: 0x06006F97 RID: 28567 RVA: 0x002B53DD File Offset: 0x002B35DD
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
