using System;
using UnityEngine.Scripting;

// Token: 0x020005A6 RID: 1446
[Preserve]
public class ItemActionData
{
	// Token: 0x06002F68 RID: 12136 RVA: 0x0012EF8C File Offset: 0x0012D18C
	public ItemActionData(ItemInventoryData _inventoryData, int _indexInEntityOfAction)
	{
		this.invData = _inventoryData;
		this.indexInEntityOfAction = _indexInEntityOfAction;
		this.ActionTags = FastTags<TagGroup.Global>.Parse((_indexInEntityOfAction == 0) ? "primary" : ((_indexInEntityOfAction == 1) ? "secondary" : "action2"));
		this.EventParms = new MinEventParams();
		this.hitInfo = new WorldRayHitInfo();
	}

	// Token: 0x06002F69 RID: 12137 RVA: 0x0012EFE8 File Offset: 0x0012D1E8
	public WorldRayHitInfo GetUpdatedHitInfo()
	{
		this.hitInfo.CopyFrom(Voxel.voxelRayHitInfo);
		return this.hitInfo;
	}

	// Token: 0x040024C6 RID: 9414
	public ItemInventoryData invData;

	// Token: 0x040024C7 RID: 9415
	public float lastUseTime;

	// Token: 0x040024C8 RID: 9416
	public int indexInEntityOfAction;

	// Token: 0x040024C9 RID: 9417
	public bool bWaitForRelease;

	// Token: 0x040024CA RID: 9418
	public ItemActionAttack.AttackHitInfo attackDetails;

	// Token: 0x040024CB RID: 9419
	public FastTags<TagGroup.Global> ActionTags;

	// Token: 0x040024CC RID: 9420
	public bool HasExecuted;

	// Token: 0x040024CD RID: 9421
	public bool uiOpenedByMe;

	// Token: 0x040024CE RID: 9422
	public MinEventParams EventParms;

	// Token: 0x040024CF RID: 9423
	public WorldRayHitInfo hitInfo;
}
