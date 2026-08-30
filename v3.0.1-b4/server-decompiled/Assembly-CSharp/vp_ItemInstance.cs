using System;
using UnityEngine;

// Token: 0x020015A8 RID: 5544
[Serializable]
public class vp_ItemInstance
{
	// Token: 0x0600AD48 RID: 44360 RVA: 0x004096E4 File Offset: 0x004078E4
	public vp_ItemInstance(vp_ItemType type, int id)
	{
		this.ID = id;
		this.Type = type;
	}

	// Token: 0x0600AD49 RID: 44361 RVA: 0x004096FA File Offset: 0x004078FA
	public virtual void SetUniqueID()
	{
		this.ID = vp_Utility.UniqueID;
	}

	// Token: 0x040080D8 RID: 32984
	[SerializeField]
	public vp_ItemType Type;

	// Token: 0x040080D9 RID: 32985
	[SerializeField]
	public int ID;
}
