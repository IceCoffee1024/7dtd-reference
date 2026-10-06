using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class SpinningBladeTrapBladeController : MonoBehaviour
{
	// Token: 0x06001DB9 RID: 7609 RVA: 0x000B4D2C File Offset: 0x000B2F2C
	public void Init(DynamicProperties _properties, Block _block)
	{
		this.entityDamage = 20f;
		if (_block.Damage > 0f)
		{
			this.entityDamage = _block.Damage;
		}
		this.selfDamage = 0.1f;
		_properties.ParseFloat("DamageReceived", ref this.selfDamage);
		_properties.ParseString("ImpactSound", ref this.bladeImpactSound);
		this.brokenPercentage = 0.25f;
		_properties.ParseFloat("BrokenPercentage", ref this.brokenPercentage);
		this.brokenPercentage = Mathf.Clamp01(this.brokenPercentage);
		this.blockDamage = 0f;
		if (_properties.Values.ContainsKey("Buff"))
		{
			this.buffActions = new List<string>();
			string[] array = _properties.Values["Buff"].Replace(" ", "").Split(',', StringSplitOptions.None);
			this.buffActions.AddRange(array);
		}
	}

	// Token: 0x06001DBA RID: 7610 RVA: 0x000B4E13 File Offset: 0x000B3013
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		if (this.CollidersThisFrame == null)
		{
			this.CollidersThisFrame = new List<Collider>();
		}
		if (!this.CollidersThisFrame.Contains(other))
		{
			this.CollidersThisFrame.Add(other);
		}
	}

	// Token: 0x06001DBB RID: 7611 RVA: 0x000B4E42 File Offset: 0x000B3042
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		if (this.CollidersThisFrame == null)
		{
			this.CollidersThisFrame = new List<Collider>();
		}
		if (!this.CollidersThisFrame.Contains(other))
		{
			this.CollidersThisFrame.Remove(other);
		}
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x000B4E74 File Offset: 0x000B3074
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (this.entityHitCount == null)
		{
			this.entityHitCount = new Dictionary<int, float>();
		}
		if (!this.IsOn)
		{
			this.entityHitCount.Clear();
			return;
		}
		if (this.controller.HealthRatio <= this.brokenPercentage)
		{
			return;
		}
		this.entityHitList.Clear();
		GameManager.Instance.World.GetEntitiesInBounds(typeof(EntityAlive), new Bounds(base.transform.position + Origin.position, new Vector3(3f, 3f, 3f)), this.entityHitList);
		if (this.entityHitList.Count == 0)
		{
			return;
		}
		DamageMultiplier damageMultiplier = new DamageMultiplier();
		bool flag = false;
		Vector3 vector = this.BladeCenter.position + Origin.position + new Vector3(0f, 0.2f, 0f);
		for (int i = 0; i < this.Blades.Length; i++)
		{
			Vector3 vector2 = this.Blades[i].position + Origin.position - vector;
			Ray ray = new Ray(vector, vector2);
			Voxel.Raycast(GameManager.Instance.World, ray, 1.24f, -538750989, 128, 0.1f);
			WorldRayHitInfo worldRayHitInfo = Voxel.voxelRayHitInfo.Clone();
			EntityAlive entityFromCollider = this.GetEntityFromCollider(Voxel.voxelRayHitInfo.hitCollider);
			if (entityFromCollider != null && entityFromCollider.IsAlive())
			{
				bool flag2;
				if (this.entityHitCount.ContainsKey(entityFromCollider.entityId))
				{
					Dictionary<int, float> dictionary = this.entityHitCount;
					int entityId = entityFromCollider.entityId;
					dictionary[entityId] += Time.deltaTime;
					flag2 = this.entityHitCount[entityFromCollider.entityId] >= this.entityHitTime;
					if (flag2)
					{
						this.entityHitCount[entityFromCollider.entityId] = 0f;
					}
				}
				else
				{
					this.entityHitCount.Add(entityFromCollider.entityId, 0f);
					flag2 = true;
				}
				if (flag2)
				{
					flag = true;
					ItemActionAttack.Hit(worldRayHitInfo, (this.OwnerTE.OwnerEntityID == entityFromCollider.entityId) ? (-1) : this.OwnerTE.OwnerEntityID, EnumDamageTypes.Slashing, this.blockDamage, this.entityDamage, 1f, 1f, 0f, 0.05f, "metal", damageMultiplier, this.buffActions, new ItemActionAttack.AttackHitInfo(), 3, 0, 0f, null, null, ItemActionAttack.EnumAttackMode.RealNoHarvesting, null, -2, this.OwnerTE.blockValue.ToItemValue(), false, false, false, null);
				}
			}
		}
		if (flag)
		{
			this.controller.DamageSelf(this.selfDamage);
			Manager.BroadcastPlay(this.controller.BlockPosition.ToVector3(), this.bladeImpactSound, 0f);
		}
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x000B5158 File Offset: 0x000B3358
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive GetEntityFromCollider(Collider collider)
	{
		if (collider == null || collider.transform == null)
		{
			return null;
		}
		EntityAlive entityAlive = collider.transform.GetComponent<EntityAlive>();
		if (entityAlive == null)
		{
			entityAlive = collider.transform.GetComponentInParent<EntityAlive>();
		}
		if (entityAlive == null && collider.transform.parent != null)
		{
			entityAlive = collider.transform.parent.GetComponentInChildren<EntityAlive>();
		}
		if (entityAlive == null)
		{
			entityAlive = collider.transform.GetComponentInChildren<EntityAlive>();
		}
		return entityAlive;
	}

	// Token: 0x040013B8 RID: 5048
	public SpinningBladeTrapController controller;

	// Token: 0x040013B9 RID: 5049
	public Transform[] Blades;

	// Token: 0x040013BA RID: 5050
	public Transform BladeCenter;

	// Token: 0x040013BB RID: 5051
	public bool IsOn;

	// Token: 0x040013BC RID: 5052
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string bladeImpactSound = "Electricity/BladeTrap/bladetrap_impact";

	// Token: 0x040013BD RID: 5053
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float entityDamage;

	// Token: 0x040013BE RID: 5054
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float blockDamage;

	// Token: 0x040013BF RID: 5055
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float selfDamage;

	// Token: 0x040013C0 RID: 5056
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float brokenPercentage;

	// Token: 0x040013C1 RID: 5057
	public TileEntityPoweredMeleeTrap OwnerTE;

	// Token: 0x040013C2 RID: 5058
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<string> buffActions;

	// Token: 0x040013C3 RID: 5059
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Collider> CollidersThisFrame;

	// Token: 0x040013C4 RID: 5060
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<int, float> entityHitCount;

	// Token: 0x040013C5 RID: 5061
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float entityHitTime = 0.05f;

	// Token: 0x040013C6 RID: 5062
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Entity> entityHitList = new List<Entity>();
}
