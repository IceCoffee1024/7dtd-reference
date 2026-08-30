using System;
using UnityEngine.Scripting;

// Token: 0x020005A2 RID: 1442
[Preserve]
public class ItemActionAttackData : ItemActionData
{
	// Token: 0x06002F3C RID: 12092 RVA: 0x0012DF90 File Offset: 0x0012C190
	public ItemActionAttackData(ItemInventoryData _inventoryData, int _indexInEntityOfAction)
		: base(_inventoryData, _indexInEntityOfAction)
	{
		this.attackDetails = new ItemActionAttack.AttackHitInfo();
	}

	// Token: 0x0400249C RID: 9372
	public ItemActionAttackData.HitDelegate hitDelegate;

	// Token: 0x020005A3 RID: 1443
	// (Invoke) Token: 0x06002F3E RID: 12094
	public delegate WorldRayHitInfo HitDelegate(out float damageScale);
}
