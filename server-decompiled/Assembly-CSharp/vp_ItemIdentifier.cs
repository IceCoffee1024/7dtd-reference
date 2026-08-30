using System;
using UnityEngine;

// Token: 0x020015C2 RID: 5570
public class vp_ItemIdentifier : MonoBehaviour
{
	// Token: 0x0600AE40 RID: 44608 RVA: 0x0040E9D8 File Offset: 0x0040CBD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_TargetEventReturn<vp_ItemType>.Register(base.transform, "GetItemType", new Func<vp_ItemType>(this.GetItemType));
		vp_TargetEventReturn<int>.Register(base.transform, "GetItemID", new Func<int>(this.GetItemID));
	}

	// Token: 0x0600AE41 RID: 44609 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
	}

	// Token: 0x0600AE42 RID: 44610 RVA: 0x0040EA14 File Offset: 0x0040CC14
	public virtual vp_ItemType GetItemType()
	{
		return this.Type;
	}

	// Token: 0x0600AE43 RID: 44611 RVA: 0x0040EA1C File Offset: 0x0040CC1C
	public virtual int GetItemID()
	{
		return this.ID;
	}

	// Token: 0x040081AF RID: 33199
	public vp_ItemType Type;

	// Token: 0x040081B0 RID: 33200
	public int ID;
}
