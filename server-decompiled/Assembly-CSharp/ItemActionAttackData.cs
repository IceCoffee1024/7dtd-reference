using System;
using UnityEngine.Scripting;

// Token: 0x020005AF RID: 1455
[Preserve]
public class ItemActionAttackData : ItemActionData
{
	// Token: 0x06002FBB RID: 12219 RVA: 0x001306C4 File Offset: 0x0012E8C4
	public ItemActionAttackData(ItemInventoryData _inventoryData, int _indexInEntityOfAction)
		: base(_inventoryData, _indexInEntityOfAction)
	{
		this.attackDetails = new ItemActionAttack.AttackHitInfo();
	}

	// Token: 0x040024E4 RID: 9444
	public ItemActionAttackData.HitDelegate hitDelegate;

	// Token: 0x020005B0 RID: 1456
	// (Invoke) Token: 0x06002FBD RID: 12221
	public delegate WorldRayHitInfo HitDelegate(out float damageScale);
}
