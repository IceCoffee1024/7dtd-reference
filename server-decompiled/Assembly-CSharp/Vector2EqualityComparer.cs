using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013FC RID: 5116
public class Vector2EqualityComparer : IEqualityComparer<Vector2>
{
	// Token: 0x0600A089 RID: 41097 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2EqualityComparer()
	{
	}

	// Token: 0x0600A08A RID: 41098 RVA: 0x003C4D3B File Offset: 0x003C2F3B
	public bool Equals(Vector2 _a, Vector2 _b)
	{
		return _a == _b;
	}

	// Token: 0x0600A08B RID: 41099 RVA: 0x003C4D44 File Offset: 0x003C2F44
	public int GetHashCode(Vector2 _a)
	{
		return _a.GetHashCode();
	}

	// Token: 0x04007924 RID: 31012
	public static readonly Vector2EqualityComparer Instance = new Vector2EqualityComparer();
}
