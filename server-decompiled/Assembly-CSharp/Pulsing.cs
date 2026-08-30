using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000610 RID: 1552
[Preserve]
public class Pulsing : LightState
{
	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06003286 RID: 12934 RVA: 0x00148A54 File Offset: 0x00146C54
	public override float LODThreshold
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x06003287 RID: 12935 RVA: 0x00148ECC File Offset: 0x001470CC
	public override float Intensity
	{
		get
		{
			return (Mathf.Sin(6.2831855f * this.lightLOD.StateRate * Time.time) + 1f) / 2f;
		}
	}

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x06003288 RID: 12936 RVA: 0x00148EF6 File Offset: 0x001470F6
	public override float Emissive
	{
		get
		{
			return this.Intensity;
		}
	}
}
