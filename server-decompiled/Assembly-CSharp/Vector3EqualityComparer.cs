using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013FA RID: 5114
public class Vector3EqualityComparer : IEqualityComparer<Vector3>
{
	// Token: 0x0600A080 RID: 41088 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3EqualityComparer()
	{
	}

	// Token: 0x0600A081 RID: 41089 RVA: 0x003C4C96 File Offset: 0x003C2E96
	public bool Equals(Vector3 _a, Vector3 _b)
	{
		return _a == _b;
	}

	// Token: 0x0600A082 RID: 41090 RVA: 0x003C4C9F File Offset: 0x003C2E9F
	public int GetHashCode(Vector3 _a)
	{
		return _a.GetHashCode();
	}

	// Token: 0x04007921 RID: 31009
	public static readonly Vector3EqualityComparer Instance = new Vector3EqualityComparer();
}
