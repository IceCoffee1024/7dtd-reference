using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000619 RID: 1561
[Preserve]
public class Pulsing : LightState
{
	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x060032D7 RID: 13015 RVA: 0x00149FEC File Offset: 0x001481EC
	public override float LODThreshold
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x060032D8 RID: 13016 RVA: 0x0014A464 File Offset: 0x00148664
	public override float Intensity
	{
		get
		{
			return (Mathf.Sin(6.2831855f * this.lightLOD.StateRate * Time.time) + 1f) / 2f;
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x060032D9 RID: 13017 RVA: 0x0014A48E File Offset: 0x0014868E
	public override float Emissive
	{
		get
		{
			return this.Intensity;
		}
	}
}
