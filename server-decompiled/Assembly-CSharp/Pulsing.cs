using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000612 RID: 1554
[Preserve]
public class Pulsing : LightState
{
	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x060032C9 RID: 13001 RVA: 0x001493F8 File Offset: 0x001475F8
	public override float LODThreshold
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x060032CA RID: 13002 RVA: 0x00149870 File Offset: 0x00147A70
	public override float Intensity
	{
		get
		{
			return (Mathf.Sin(6.2831855f * this.lightLOD.StateRate * Time.time) + 1f) / 2f;
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x060032CB RID: 13003 RVA: 0x0014989A File Offset: 0x00147A9A
	public override float Emissive
	{
		get
		{
			return this.Intensity;
		}
	}
}
