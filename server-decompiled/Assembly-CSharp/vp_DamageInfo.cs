using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015CF RID: 5583
[Preserve]
public class vp_DamageInfo
{
	// Token: 0x0600AE60 RID: 44640 RVA: 0x004108A8 File Offset: 0x0040EAA8
	public vp_DamageInfo(float damage, Transform source)
	{
		this.Damage = damage;
		this.Source = source;
		this.OriginalSource = source;
	}

	// Token: 0x0600AE61 RID: 44641 RVA: 0x004108C5 File Offset: 0x0040EAC5
	public vp_DamageInfo(float damage, Transform source, Transform originalSource)
	{
		this.Damage = damage;
		this.Source = source;
		this.OriginalSource = originalSource;
	}

	// Token: 0x040081AB RID: 33195
	public float Damage;

	// Token: 0x040081AC RID: 33196
	public Transform Source;

	// Token: 0x040081AD RID: 33197
	public Transform OriginalSource;
}
