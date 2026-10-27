using System;
using UnityEngine.Scripting;

// Token: 0x020004FD RID: 1277
[Preserve]
public class EntityZombie : EntityHuman
{
	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x0600298A RID: 10634 RVA: 0x00010E62 File Offset: 0x0000F062
	// (set) Token: 0x0600298B RID: 10635 RVA: 0x000027FC File Offset: 0x000009FC
	public override bool AimingGun
	{
		get
		{
			return false;
		}
		set
		{
		}
	}
}
