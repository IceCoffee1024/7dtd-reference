using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015E1 RID: 5601
[Preserve]
public class vp_DamageInfo
{
	// Token: 0x0600AEB8 RID: 44728 RVA: 0x00412CAC File Offset: 0x00410EAC
	public vp_DamageInfo(float damage, Transform source)
	{
		this.Damage = damage;
		this.Source = source;
		this.OriginalSource = source;
	}

	// Token: 0x0600AEB9 RID: 44729 RVA: 0x00412CC9 File Offset: 0x00410EC9
	public vp_DamageInfo(float damage, Transform source, Transform originalSource)
	{
		this.Damage = damage;
		this.Source = source;
		this.OriginalSource = originalSource;
	}

	// Token: 0x040081EC RID: 33260
	public float Damage;

	// Token: 0x040081ED RID: 33261
	public Transform Source;

	// Token: 0x040081EE RID: 33262
	public Transform OriginalSource;
}
