using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013DF RID: 5087
public class Vector3EqualityComparer : IEqualityComparer<Vector3>
{
	// Token: 0x06009F83 RID: 40835 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3EqualityComparer()
	{
	}

	// Token: 0x06009F84 RID: 40836 RVA: 0x003BF952 File Offset: 0x003BDB52
	public bool Equals(Vector3 _a, Vector3 _b)
	{
		return _a == _b;
	}

	// Token: 0x06009F85 RID: 40837 RVA: 0x003BF95B File Offset: 0x003BDB5B
	public int GetHashCode(Vector3 _a)
	{
		return _a.GetHashCode();
	}

	// Token: 0x04007848 RID: 30792
	public static readonly Vector3EqualityComparer Instance = new Vector3EqualityComparer();
}
