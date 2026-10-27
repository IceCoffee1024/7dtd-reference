using System;
using UnityEngine;

// Token: 0x020015F6 RID: 5622
public class vp_ItemIdentifier : MonoBehaviour
{
	// Token: 0x0600AF4B RID: 44875 RVA: 0x00415C30 File Offset: 0x00413E30
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_TargetEventReturn<vp_ItemType>.Register(base.transform, "GetItemType", new Func<vp_ItemType>(this.GetItemType));
		vp_TargetEventReturn<int>.Register(base.transform, "GetItemID", new Func<int>(this.GetItemID));
	}

	// Token: 0x0600AF4C RID: 44876 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
	}

	// Token: 0x0600AF4D RID: 44877 RVA: 0x00415C6C File Offset: 0x00413E6C
	public virtual vp_ItemType GetItemType()
	{
		return this.Type;
	}

	// Token: 0x0600AF4E RID: 44878 RVA: 0x00415C74 File Offset: 0x00413E74
	public virtual int GetItemID()
	{
		return this.ID;
	}

	// Token: 0x04008273 RID: 33395
	public vp_ItemType Type;

	// Token: 0x04008274 RID: 33396
	public int ID;
}
