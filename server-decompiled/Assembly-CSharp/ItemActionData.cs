using System;
using UnityEngine.Scripting;

// Token: 0x020005AE RID: 1454
[Preserve]
public class ItemActionData
{
	// Token: 0x06002FB8 RID: 12216 RVA: 0x00130650 File Offset: 0x0012E850
	public ItemActionData(ItemInventoryData _inventoryData, int _indexInEntityOfAction)
	{
		this.invData = _inventoryData;
		this.indexInEntityOfAction = _indexInEntityOfAction;
		this.ActionTags = FastTags<TagGroup.Global>.Parse((_indexInEntityOfAction == 0) ? "primary" : ((_indexInEntityOfAction == 1) ? "secondary" : "action2"));
		this.EventParms = new MinEventParams();
		this.hitInfo = new WorldRayHitInfo();
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitModelTransforms(ItemInventoryData _invData)
	{
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x001306AC File Offset: 0x0012E8AC
	public WorldRayHitInfo GetUpdatedHitInfo()
	{
		this.hitInfo.CopyFrom(Voxel.voxelRayHitInfo);
		return this.hitInfo;
	}

	// Token: 0x040024DA RID: 9434
	public ItemInventoryData invData;

	// Token: 0x040024DB RID: 9435
	public float lastUseTime;

	// Token: 0x040024DC RID: 9436
	public int indexInEntityOfAction;

	// Token: 0x040024DD RID: 9437
	public bool bWaitForRelease;

	// Token: 0x040024DE RID: 9438
	public ItemActionAttack.AttackHitInfo attackDetails;

	// Token: 0x040024DF RID: 9439
	public FastTags<TagGroup.Global> ActionTags;

	// Token: 0x040024E0 RID: 9440
	public bool HasExecuted;

	// Token: 0x040024E1 RID: 9441
	public bool uiOpenedByMe;

	// Token: 0x040024E2 RID: 9442
	public MinEventParams EventParms;

	// Token: 0x040024E3 RID: 9443
	public WorldRayHitInfo hitInfo;
}
