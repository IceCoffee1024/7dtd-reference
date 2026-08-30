using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013E1 RID: 5089
public class Vector2EqualityComparer : IEqualityComparer<Vector2>
{
	// Token: 0x06009F8C RID: 40844 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2EqualityComparer()
	{
	}

	// Token: 0x06009F8D RID: 40845 RVA: 0x003BF9F7 File Offset: 0x003BDBF7
	public bool Equals(Vector2 _a, Vector2 _b)
	{
		return _a == _b;
	}

	// Token: 0x06009F8E RID: 40846 RVA: 0x003BFA00 File Offset: 0x003BDC00
	public int GetHashCode(Vector2 _a)
	{
		return _a.GetHashCode();
	}

	// Token: 0x0400784B RID: 30795
	public static readonly Vector2EqualityComparer Instance = new Vector2EqualityComparer();
}
