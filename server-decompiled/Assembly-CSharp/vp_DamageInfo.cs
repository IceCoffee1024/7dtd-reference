using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015AD RID: 5549
[Preserve]
public class vp_DamageInfo
{
	// Token: 0x0600ADAD RID: 44461 RVA: 0x0040BA54 File Offset: 0x00409C54
	public vp_DamageInfo(float damage, Transform source)
	{
		this.Damage = damage;
		this.Source = source;
		this.OriginalSource = source;
	}

	// Token: 0x0600ADAE RID: 44462 RVA: 0x0040BA71 File Offset: 0x00409C71
	public vp_DamageInfo(float damage, Transform source, Transform originalSource)
	{
		this.Damage = damage;
		this.Source = source;
		this.OriginalSource = originalSource;
	}

	// Token: 0x04008128 RID: 33064
	public float Damage;

	// Token: 0x04008129 RID: 33065
	public Transform Source;

	// Token: 0x0400812A RID: 33066
	public Transform OriginalSource;
}
