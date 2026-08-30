using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013DE RID: 5086
public class Color32EqualityComparer : IEqualityComparer<Color32>
{
	// Token: 0x06009F7F RID: 40831 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32EqualityComparer()
	{
	}

	// Token: 0x06009F80 RID: 40832 RVA: 0x003BF8B6 File Offset: 0x003BDAB6
	public bool Equals(Color32 _a, Color32 _b)
	{
		return _a.r == _b.r && _a.g == _b.g && _a.b == _b.b && _a.a == _b.a;
	}

	// Token: 0x06009F81 RID: 40833 RVA: 0x003BF8F4 File Offset: 0x003BDAF4
	public int GetHashCode(Color32 _a)
	{
		return (((((_a.r.GetHashCode() * 397) ^ _a.g.GetHashCode()) * 397) ^ _a.b.GetHashCode()) * 397) ^ _a.a.GetHashCode();
	}

	// Token: 0x04007847 RID: 30791
	public static readonly Color32EqualityComparer Instance = new Color32EqualityComparer();
}
