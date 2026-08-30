using System;
using UnityEngine;

// Token: 0x020015C3 RID: 5571
[Serializable]
public class vp_ItemInstance
{
	// Token: 0x0600AE45 RID: 44613 RVA: 0x0040EA24 File Offset: 0x0040CC24
	public vp_ItemInstance(vp_ItemType type, int id)
	{
		this.ID = id;
		this.Type = type;
	}

	// Token: 0x0600AE46 RID: 44614 RVA: 0x0040EA3A File Offset: 0x0040CC3A
	public virtual void SetUniqueID()
	{
		this.ID = vp_Utility.UniqueID;
	}

	// Token: 0x040081B1 RID: 33201
	[SerializeField]
	public vp_ItemType Type;

	// Token: 0x040081B2 RID: 33202
	[SerializeField]
	public int ID;
}
