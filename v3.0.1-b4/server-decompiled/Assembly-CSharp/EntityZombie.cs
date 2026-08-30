using System;
using UnityEngine.Scripting;

// Token: 0x020004F0 RID: 1264
[Preserve]
public class EntityZombie : EntityHuman
{
	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06002964 RID: 10596 RVA: 0x00010E62 File Offset: 0x0000F062
	// (set) Token: 0x06002965 RID: 10597 RVA: 0x000027FC File Offset: 0x000009FC
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
