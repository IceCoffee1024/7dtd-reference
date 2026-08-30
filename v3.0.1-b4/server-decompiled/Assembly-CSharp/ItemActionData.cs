using System;
using UnityEngine.Scripting;

// Token: 0x020005A1 RID: 1441
[Preserve]
public class ItemActionData
{
	// Token: 0x06002F3A RID: 12090 RVA: 0x0012DF1C File Offset: 0x0012C11C
	public ItemActionData(ItemInventoryData _inventoryData, int _indexInEntityOfAction)
	{
		this.invData = _inventoryData;
		this.indexInEntityOfAction = _indexInEntityOfAction;
		this.ActionTags = FastTags<TagGroup.Global>.Parse((_indexInEntityOfAction == 0) ? "primary" : ((_indexInEntityOfAction == 1) ? "secondary" : "action2"));
		this.EventParms = new MinEventParams();
		this.hitInfo = new WorldRayHitInfo();
	}

	// Token: 0x06002F3B RID: 12091 RVA: 0x0012DF78 File Offset: 0x0012C178
	public WorldRayHitInfo GetUpdatedHitInfo()
	{
		this.hitInfo.CopyFrom(Voxel.voxelRayHitInfo);
		return this.hitInfo;
	}

	// Token: 0x04002492 RID: 9362
	public ItemInventoryData invData;

	// Token: 0x04002493 RID: 9363
	public float lastUseTime;

	// Token: 0x04002494 RID: 9364
	public int indexInEntityOfAction;

	// Token: 0x04002495 RID: 9365
	public bool bWaitForRelease;

	// Token: 0x04002496 RID: 9366
	public ItemActionAttack.AttackHitInfo attackDetails;

	// Token: 0x04002497 RID: 9367
	public FastTags<TagGroup.Global> ActionTags;

	// Token: 0x04002498 RID: 9368
	public bool HasExecuted;

	// Token: 0x04002499 RID: 9369
	public bool uiOpenedByMe;

	// Token: 0x0400249A RID: 9370
	public MinEventParams EventParms;

	// Token: 0x0400249B RID: 9371
	public WorldRayHitInfo hitInfo;
}
