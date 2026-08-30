using System;
using UnityEngine;

// Token: 0x020015A2 RID: 5538
public class pulsingLightEmissive : LightState
{
	// Token: 0x17001408 RID: 5128
	// (get) Token: 0x0600AD54 RID: 44372 RVA: 0x00148ECC File Offset: 0x001470CC
	public override float Intensity
	{
		get
		{
			return (Mathf.Sin(6.2831855f * this.lightLOD.StateRate * Time.time) + 1f) / 2f;
		}
	}

	// Token: 0x17001409 RID: 5129
	// (get) Token: 0x0600AD55 RID: 44373 RVA: 0x00148EF6 File Offset: 0x001470F6
	public override float Emissive
	{
		get
		{
			return this.Intensity;
		}
	}
}
