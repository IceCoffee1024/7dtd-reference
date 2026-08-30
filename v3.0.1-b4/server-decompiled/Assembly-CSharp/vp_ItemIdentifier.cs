using System;
using UnityEngine;

// Token: 0x020015A7 RID: 5543
public class vp_ItemIdentifier : MonoBehaviour
{
	// Token: 0x0600AD43 RID: 44355 RVA: 0x00409698 File Offset: 0x00407898
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_TargetEventReturn<vp_ItemType>.Register(base.transform, "GetItemType", new Func<vp_ItemType>(this.GetItemType));
		vp_TargetEventReturn<int>.Register(base.transform, "GetItemID", new Func<int>(this.GetItemID));
	}

	// Token: 0x0600AD44 RID: 44356 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
	}

	// Token: 0x0600AD45 RID: 44357 RVA: 0x004096D4 File Offset: 0x004078D4
	public virtual vp_ItemType GetItemType()
	{
		return this.Type;
	}

	// Token: 0x0600AD46 RID: 44358 RVA: 0x004096DC File Offset: 0x004078DC
	public virtual int GetItemID()
	{
		return this.ID;
	}

	// Token: 0x040080D6 RID: 32982
	public vp_ItemType Type;

	// Token: 0x040080D7 RID: 32983
	public int ID;
}
