using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001419 RID: 5145
public class Color32EqualityComparer : IEqualityComparer<Color32>
{
	// Token: 0x0600A127 RID: 41255 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32EqualityComparer()
	{
	}

	// Token: 0x0600A128 RID: 41256 RVA: 0x003C9796 File Offset: 0x003C7996
	public bool Equals(Color32 _a, Color32 _b)
	{
		return _a.r == _b.r && _a.g == _b.g && _a.b == _b.b && _a.a == _b.a;
	}

	// Token: 0x0600A129 RID: 41257 RVA: 0x003C97D4 File Offset: 0x003C79D4
	public int GetHashCode(Color32 _a)
	{
		return (((((_a.r.GetHashCode() * 397) ^ _a.g.GetHashCode()) * 397) ^ _a.b.GetHashCode()) * 397) ^ _a.a.GetHashCode();
	}

	// Token: 0x0400799B RID: 31131
	public static readonly Color32EqualityComparer Instance = new Color32EqualityComparer();
}
