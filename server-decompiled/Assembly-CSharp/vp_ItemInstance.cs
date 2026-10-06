using System;
using UnityEngine;

// Token: 0x020015E5 RID: 5605
[Serializable]
public class vp_ItemInstance
{
	// Token: 0x0600AEF8 RID: 44792 RVA: 0x00413878 File Offset: 0x00411A78
	public vp_ItemInstance(vp_ItemType type, int id)
	{
		this.ID = id;
		this.Type = type;
	}

	// Token: 0x0600AEF9 RID: 44793 RVA: 0x0041388E File Offset: 0x00411A8E
	public virtual void SetUniqueID()
	{
		this.ID = vp_Utility.UniqueID;
	}

	// Token: 0x04008234 RID: 33332
	[SerializeField]
	public vp_ItemType Type;

	// Token: 0x04008235 RID: 33333
	[SerializeField]
	public int ID;
}
