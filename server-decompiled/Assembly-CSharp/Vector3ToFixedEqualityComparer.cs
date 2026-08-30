using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013E0 RID: 5088
public class Vector3ToFixedEqualityComparer : IEqualityComparer<Vector3>
{
	// Token: 0x06009F87 RID: 40839 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3ToFixedEqualityComparer()
	{
	}

	// Token: 0x06009F88 RID: 40840 RVA: 0x003BF976 File Offset: 0x003BDB76
	public bool Equals(Vector3 _a, Vector3 _b)
	{
		return this.GetHashCode(_a) == this.GetHashCode(_b);
	}

	// Token: 0x06009F89 RID: 40841 RVA: 0x003BF988 File Offset: 0x003BDB88
	public int GetHashCode(Vector3 _a)
	{
		return this.FloatToFixed(_a.x).GetHashCode() ^ (this.FloatToFixed(_a.y).GetHashCode() << 2) ^ (this.FloatToFixed(_a.z).GetHashCode() >> 2);
	}

	// Token: 0x06009F8A RID: 40842 RVA: 0x003BF9D7 File Offset: 0x003BDBD7
	[PublicizedFrom(EAccessModifier.Private)]
	public int FloatToFixed(float a)
	{
		return (int)Math.Round((double)a * 32.0);
	}

	// Token: 0x04007849 RID: 30793
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFixedPointFractionalBits = 5;

	// Token: 0x0400784A RID: 30794
	public static readonly Vector3ToFixedEqualityComparer Instance = new Vector3ToFixedEqualityComparer();
}
