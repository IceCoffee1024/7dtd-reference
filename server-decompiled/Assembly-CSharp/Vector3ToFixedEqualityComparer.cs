using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200142D RID: 5165
public class Vector3ToFixedEqualityComparer : IEqualityComparer<Vector3>
{
	// Token: 0x0600A187 RID: 41351 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3ToFixedEqualityComparer()
	{
	}

	// Token: 0x0600A188 RID: 41352 RVA: 0x003CBC5A File Offset: 0x003C9E5A
	public bool Equals(Vector3 _a, Vector3 _b)
	{
		return this.GetHashCode(_a) == this.GetHashCode(_b);
	}

	// Token: 0x0600A189 RID: 41353 RVA: 0x003CBC6C File Offset: 0x003C9E6C
	public int GetHashCode(Vector3 _a)
	{
		return this.FloatToFixed(_a.x).GetHashCode() ^ (this.FloatToFixed(_a.y).GetHashCode() << 2) ^ (this.FloatToFixed(_a.z).GetHashCode() >> 2);
	}

	// Token: 0x0600A18A RID: 41354 RVA: 0x003CBCBB File Offset: 0x003C9EBB
	[PublicizedFrom(EAccessModifier.Private)]
	public int FloatToFixed(float a)
	{
		return (int)Math.Round((double)a * 32.0);
	}

	// Token: 0x040079DE RID: 31198
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFixedPointFractionalBits = 5;

	// Token: 0x040079DF RID: 31199
	public static readonly Vector3ToFixedEqualityComparer Instance = new Vector3ToFixedEqualityComparer();
}
