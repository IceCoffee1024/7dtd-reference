using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200141B RID: 5147
public class Vector3ToFixedEqualityComparer : IEqualityComparer<Vector3>
{
	// Token: 0x0600A12F RID: 41263 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3ToFixedEqualityComparer()
	{
	}

	// Token: 0x0600A130 RID: 41264 RVA: 0x003C9856 File Offset: 0x003C7A56
	public bool Equals(Vector3 _a, Vector3 _b)
	{
		return this.GetHashCode(_a) == this.GetHashCode(_b);
	}

	// Token: 0x0600A131 RID: 41265 RVA: 0x003C9868 File Offset: 0x003C7A68
	public int GetHashCode(Vector3 _a)
	{
		return this.FloatToFixed(_a.x).GetHashCode() ^ (this.FloatToFixed(_a.y).GetHashCode() << 2) ^ (this.FloatToFixed(_a.z).GetHashCode() >> 2);
	}

	// Token: 0x0600A132 RID: 41266 RVA: 0x003C98B7 File Offset: 0x003C7AB7
	[PublicizedFrom(EAccessModifier.Private)]
	public int FloatToFixed(float a)
	{
		return (int)Math.Round((double)a * 32.0);
	}

	// Token: 0x0400799D RID: 31133
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFixedPointFractionalBits = 5;

	// Token: 0x0400799E RID: 31134
	public static readonly Vector3ToFixedEqualityComparer Instance = new Vector3ToFixedEqualityComparer();
}
