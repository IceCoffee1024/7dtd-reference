using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200142C RID: 5164
public class Vector3EqualityComparer : IEqualityComparer<Vector3>
{
	// Token: 0x0600A183 RID: 41347 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3EqualityComparer()
	{
	}

	// Token: 0x0600A184 RID: 41348 RVA: 0x003CBC36 File Offset: 0x003C9E36
	public bool Equals(Vector3 _a, Vector3 _b)
	{
		return _a == _b;
	}

	// Token: 0x0600A185 RID: 41349 RVA: 0x003CBC3F File Offset: 0x003C9E3F
	public int GetHashCode(Vector3 _a)
	{
		return _a.GetHashCode();
	}

	// Token: 0x040079DD RID: 31197
	public static readonly Vector3EqualityComparer Instance = new Vector3EqualityComparer();
}
