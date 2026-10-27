using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200142E RID: 5166
public class Vector2EqualityComparer : IEqualityComparer<Vector2>
{
	// Token: 0x0600A18C RID: 41356 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2EqualityComparer()
	{
	}

	// Token: 0x0600A18D RID: 41357 RVA: 0x003CBCDB File Offset: 0x003C9EDB
	public bool Equals(Vector2 _a, Vector2 _b)
	{
		return _a == _b;
	}

	// Token: 0x0600A18E RID: 41358 RVA: 0x003CBCE4 File Offset: 0x003C9EE4
	public int GetHashCode(Vector2 _a)
	{
		return _a.GetHashCode();
	}

	// Token: 0x040079E0 RID: 31200
	public static readonly Vector2EqualityComparer Instance = new Vector2EqualityComparer();
}
