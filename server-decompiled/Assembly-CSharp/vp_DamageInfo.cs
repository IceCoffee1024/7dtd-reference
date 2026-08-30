using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001592 RID: 5522
[Preserve]
public class vp_DamageInfo
{
	// Token: 0x0600ACB0 RID: 44208 RVA: 0x00406714 File Offset: 0x00404914
	public vp_DamageInfo(float damage, Transform source)
	{
		this.Damage = damage;
		this.Source = source;
		this.OriginalSource = source;
	}

	// Token: 0x0600ACB1 RID: 44209 RVA: 0x00406731 File Offset: 0x00404931
	public vp_DamageInfo(float damage, Transform source, Transform originalSource)
	{
		this.Damage = damage;
		this.Source = source;
		this.OriginalSource = originalSource;
	}

	// Token: 0x0400804F RID: 32847
	public float Damage;

	// Token: 0x04008050 RID: 32848
	public Transform Source;

	// Token: 0x04008051 RID: 32849
	public Transform OriginalSource;
}
