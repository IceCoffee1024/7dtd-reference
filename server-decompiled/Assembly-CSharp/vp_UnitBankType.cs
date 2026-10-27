using System;
using UnityEngine;

// Token: 0x020015ED RID: 5613
[Serializable]
public class vp_UnitBankType : vp_ItemType
{
	// Token: 0x0400824E RID: 33358
	[SerializeField]
	public vp_UnitType Unit;

	// Token: 0x0400824F RID: 33359
	[SerializeField]
	public int Capacity = 10;

	// Token: 0x04008250 RID: 33360
	[SerializeField]
	public bool Reloadable = true;

	// Token: 0x04008251 RID: 33361
	[SerializeField]
	public bool RemoveWhenDepleted;
}
