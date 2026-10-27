using System;
using UnityEngine;

// Token: 0x020015D6 RID: 5590
public class pulsingLightEmissive : LightState
{
	// Token: 0x17001459 RID: 5209
	// (get) Token: 0x0600AE5F RID: 44639 RVA: 0x0014A464 File Offset: 0x00148664
	public override float Intensity
	{
		get
		{
			return (Mathf.Sin(6.2831855f * this.lightLOD.StateRate * Time.time) + 1f) / 2f;
		}
	}

	// Token: 0x1700145A RID: 5210
	// (get) Token: 0x0600AE60 RID: 44640 RVA: 0x0014A48E File Offset: 0x0014868E
	public override float Emissive
	{
		get
		{
			return this.Intensity;
		}
	}
}
