using System;
using UnityEngine.Scripting;

// Token: 0x020005A7 RID: 1447
[Preserve]
public class ItemActionAttackData : ItemActionData
{
	// Token: 0x06002F6A RID: 12138 RVA: 0x0012F000 File Offset: 0x0012D200
	public ItemActionAttackData(ItemInventoryData _inventoryData, int _indexInEntityOfAction)
		: base(_inventoryData, _indexInEntityOfAction)
	{
		this.attackDetails = new ItemActionAttack.AttackHitInfo();
	}

	// Token: 0x040024D0 RID: 9424
	public ItemActionAttackData.HitDelegate hitDelegate;

	// Token: 0x020005A8 RID: 1448
	// (Invoke) Token: 0x06002F6C RID: 12140
	public delegate WorldRayHitInfo HitDelegate(out float damageScale);
}
