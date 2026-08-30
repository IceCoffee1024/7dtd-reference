using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000607 RID: 1543
[Preserve]
public class Pulsing : LightState
{
	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06003233 RID: 12851 RVA: 0x00146930 File Offset: 0x00144B30
	public override float LODThreshold
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06003234 RID: 12852 RVA: 0x00146DA8 File Offset: 0x00144FA8
	public override float Intensity
	{
		get
		{
			return (Mathf.Sin(6.2831855f * this.lightLOD.StateRate * Time.time) + 1f) / 2f;
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06003235 RID: 12853 RVA: 0x00146DD2 File Offset: 0x00144FD2
	public override float Emissive
	{
		get
		{
			return this.Intensity;
		}
	}
}
