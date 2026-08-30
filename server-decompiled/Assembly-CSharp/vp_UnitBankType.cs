using System;
using UnityEngine;

// Token: 0x020015B9 RID: 5561
[Serializable]
public class vp_UnitBankType : vp_ItemType
{
	// Token: 0x0400818A RID: 33162
	[SerializeField]
	public vp_UnitType Unit;

	// Token: 0x0400818B RID: 33163
	[SerializeField]
	public int Capacity = 10;

	// Token: 0x0400818C RID: 33164
	[SerializeField]
	public bool Reloadable = true;

	// Token: 0x0400818D RID: 33165
	[SerializeField]
	public bool RemoveWhenDepleted;
}
