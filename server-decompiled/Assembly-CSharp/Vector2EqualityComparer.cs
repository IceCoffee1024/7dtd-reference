using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200141C RID: 5148
public class Vector2EqualityComparer : IEqualityComparer<Vector2>
{
	// Token: 0x0600A134 RID: 41268 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2EqualityComparer()
	{
	}

	// Token: 0x0600A135 RID: 41269 RVA: 0x003C98D7 File Offset: 0x003C7AD7
	public bool Equals(Vector2 _a, Vector2 _b)
	{
		return _a == _b;
	}

	// Token: 0x0600A136 RID: 41270 RVA: 0x003C98E0 File Offset: 0x003C7AE0
	public int GetHashCode(Vector2 _a)
	{
		return _a.GetHashCode();
	}

	// Token: 0x0400799F RID: 31135
	public static readonly Vector2EqualityComparer Instance = new Vector2EqualityComparer();
}
