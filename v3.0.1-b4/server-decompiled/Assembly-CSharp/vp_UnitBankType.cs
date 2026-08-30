using System;
using UnityEngine;

// Token: 0x0200159E RID: 5534
[Serializable]
public class vp_UnitBankType : vp_ItemType
{
	// Token: 0x040080B1 RID: 32945
	[SerializeField]
	public vp_UnitType Unit;

	// Token: 0x040080B2 RID: 32946
	[SerializeField]
	public int Capacity = 10;

	// Token: 0x040080B3 RID: 32947
	[SerializeField]
	public bool Reloadable = true;

	// Token: 0x040080B4 RID: 32948
	[SerializeField]
	public bool RemoveWhenDepleted;
}
