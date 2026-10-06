using System;
using UnityEngine.Scripting;

// Token: 0x020005A8 RID: 1448
[Preserve]
public class ItemActionAttackData : ItemActionData
{
	// Token: 0x06002FAD RID: 12205 RVA: 0x0012FAD0 File Offset: 0x0012DCD0
	public ItemActionAttackData(ItemInventoryData _inventoryData, int _indexInEntityOfAction)
		: base(_inventoryData, _indexInEntityOfAction)
	{
		this.attackDetails = new ItemActionAttack.AttackHitInfo();
	}

	// Token: 0x040024C9 RID: 9417
	public ItemActionAttackData.HitDelegate hitDelegate;

	// Token: 0x020005A9 RID: 1449
	// (Invoke) Token: 0x06002FAF RID: 12207
	public delegate WorldRayHitInfo HitDelegate(out float damageScale);
}
