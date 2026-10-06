using System;
using UnityEngine;

// Token: 0x020015C4 RID: 5572
public class pulsingLightEmissive : LightState
{
	// Token: 0x1700144D RID: 5197
	// (get) Token: 0x0600AE07 RID: 44551 RVA: 0x00149870 File Offset: 0x00147A70
	public override float Intensity
	{
		get
		{
			return (Mathf.Sin(6.2831855f * this.lightLOD.StateRate * Time.time) + 1f) / 2f;
		}
	}

	// Token: 0x1700144E RID: 5198
	// (get) Token: 0x0600AE08 RID: 44552 RVA: 0x0014989A File Offset: 0x00147A9A
	public override float Emissive
	{
		get
		{
			return this.Intensity;
		}
	}
}
