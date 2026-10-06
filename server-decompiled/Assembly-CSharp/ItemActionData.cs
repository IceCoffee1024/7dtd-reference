using System;
using UnityEngine.Scripting;

// Token: 0x020005A7 RID: 1447
[Preserve]
public class ItemActionData
{
	// Token: 0x06002FAA RID: 12202 RVA: 0x0012FA5C File Offset: 0x0012DC5C
	public ItemActionData(ItemInventoryData _inventoryData, int _indexInEntityOfAction)
	{
		this.invData = _inventoryData;
		this.indexInEntityOfAction = _indexInEntityOfAction;
		this.ActionTags = FastTags<TagGroup.Global>.Parse((_indexInEntityOfAction == 0) ? "primary" : ((_indexInEntityOfAction == 1) ? "secondary" : "action2"));
		this.EventParms = new MinEventParams();
		this.hitInfo = new WorldRayHitInfo();
	}

	// Token: 0x06002FAB RID: 12203 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitModelTransforms(ItemInventoryData _invData)
	{
	}

	// Token: 0x06002FAC RID: 12204 RVA: 0x0012FAB8 File Offset: 0x0012DCB8
	public WorldRayHitInfo GetUpdatedHitInfo()
	{
		this.hitInfo.CopyFrom(Voxel.voxelRayHitInfo);
		return this.hitInfo;
	}

	// Token: 0x040024BF RID: 9407
	public ItemInventoryData invData;

	// Token: 0x040024C0 RID: 9408
	public float lastUseTime;

	// Token: 0x040024C1 RID: 9409
	public int indexInEntityOfAction;

	// Token: 0x040024C2 RID: 9410
	public bool bWaitForRelease;

	// Token: 0x040024C3 RID: 9411
	public ItemActionAttack.AttackHitInfo attackDetails;

	// Token: 0x040024C4 RID: 9412
	public FastTags<TagGroup.Global> ActionTags;

	// Token: 0x040024C5 RID: 9413
	public bool HasExecuted;

	// Token: 0x040024C6 RID: 9414
	public bool uiOpenedByMe;

	// Token: 0x040024C7 RID: 9415
	public MinEventParams EventParms;

	// Token: 0x040024C8 RID: 9416
	public WorldRayHitInfo hitInfo;
}
