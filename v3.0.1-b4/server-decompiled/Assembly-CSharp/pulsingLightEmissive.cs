using System;
using UnityEngine;

// Token: 0x02001587 RID: 5511
public class pulsingLightEmissive : LightState
{
	// Token: 0x170013CF RID: 5071
	// (get) Token: 0x0600AC57 RID: 44119 RVA: 0x00146DA8 File Offset: 0x00144FA8
	public override float Intensity
	{
		get
		{
			return (Mathf.Sin(6.2831855f * this.lightLOD.StateRate * Time.time) + 1f) / 2f;
		}
	}

	// Token: 0x170013D0 RID: 5072
	// (get) Token: 0x0600AC58 RID: 44120 RVA: 0x00146DD2 File Offset: 0x00144FD2
	public override float Emissive
	{
		get
		{
			return this.Intensity;
		}
	}
}
