using System;
using UnityEngine;

// Token: 0x020015F7 RID: 5623
[Serializable]
public class vp_ItemInstance
{
	// Token: 0x0600AF50 RID: 44880 RVA: 0x00415C7C File Offset: 0x00413E7C
	public vp_ItemInstance(vp_ItemType type, int id)
	{
		this.ID = id;
		this.Type = type;
	}

	// Token: 0x0600AF51 RID: 44881 RVA: 0x00415C92 File Offset: 0x00413E92
	public virtual void SetUniqueID()
	{
		this.ID = vp_Utility.UniqueID;
	}

	// Token: 0x04008275 RID: 33397
	[SerializeField]
	public vp_ItemType Type;

	// Token: 0x04008276 RID: 33398
	[SerializeField]
	public int ID;
}
