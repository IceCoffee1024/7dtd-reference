using System;
using UnityEngine;

// Token: 0x020015DB RID: 5595
[Serializable]
public class vp_UnitBankType : vp_ItemType
{
	// Token: 0x0400820D RID: 33293
	[SerializeField]
	public vp_UnitType Unit;

	// Token: 0x0400820E RID: 33294
	[SerializeField]
	public int Capacity = 10;

	// Token: 0x0400820F RID: 33295
	[SerializeField]
	public bool Reloadable = true;

	// Token: 0x04008210 RID: 33296
	[SerializeField]
	public bool RemoveWhenDepleted;
}
