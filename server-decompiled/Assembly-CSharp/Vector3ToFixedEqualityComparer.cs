using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013FB RID: 5115
public class Vector3ToFixedEqualityComparer : IEqualityComparer<Vector3>
{
	// Token: 0x0600A084 RID: 41092 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3ToFixedEqualityComparer()
	{
	}

	// Token: 0x0600A085 RID: 41093 RVA: 0x003C4CBA File Offset: 0x003C2EBA
	public bool Equals(Vector3 _a, Vector3 _b)
	{
		return this.GetHashCode(_a) == this.GetHashCode(_b);
	}

	// Token: 0x0600A086 RID: 41094 RVA: 0x003C4CCC File Offset: 0x003C2ECC
	public int GetHashCode(Vector3 _a)
	{
		return this.FloatToFixed(_a.x).GetHashCode() ^ (this.FloatToFixed(_a.y).GetHashCode() << 2) ^ (this.FloatToFixed(_a.z).GetHashCode() >> 2);
	}

	// Token: 0x0600A087 RID: 41095 RVA: 0x003C4D1B File Offset: 0x003C2F1B
	[PublicizedFrom(EAccessModifier.Private)]
	public int FloatToFixed(float a)
	{
		return (int)Math.Round((double)a * 32.0);
	}

	// Token: 0x04007922 RID: 31010
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFixedPointFractionalBits = 5;

	// Token: 0x04007923 RID: 31011
	public static readonly Vector3ToFixedEqualityComparer Instance = new Vector3ToFixedEqualityComparer();
}
