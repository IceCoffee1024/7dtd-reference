using System;
using UnityEngine;

// Token: 0x020015E4 RID: 5604
public class vp_ItemIdentifier : MonoBehaviour
{
	// Token: 0x0600AEF3 RID: 44787 RVA: 0x0041382C File Offset: 0x00411A2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_TargetEventReturn<vp_ItemType>.Register(base.transform, "GetItemType", new Func<vp_ItemType>(this.GetItemType));
		vp_TargetEventReturn<int>.Register(base.transform, "GetItemID", new Func<int>(this.GetItemID));
	}

	// Token: 0x0600AEF4 RID: 44788 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
	}

	// Token: 0x0600AEF5 RID: 44789 RVA: 0x00413868 File Offset: 0x00411A68
	public virtual vp_ItemType GetItemType()
	{
		return this.Type;
	}

	// Token: 0x0600AEF6 RID: 44790 RVA: 0x00413870 File Offset: 0x00411A70
	public virtual int GetItemID()
	{
		return this.ID;
	}

	// Token: 0x04008232 RID: 33330
	public vp_ItemType Type;

	// Token: 0x04008233 RID: 33331
	public int ID;
}
