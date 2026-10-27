using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200142B RID: 5163
public class Color32EqualityComparer : IEqualityComparer<Color32>
{
	// Token: 0x0600A17F RID: 41343 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32EqualityComparer()
	{
	}

	// Token: 0x0600A180 RID: 41344 RVA: 0x003CBB9A File Offset: 0x003C9D9A
	public bool Equals(Color32 _a, Color32 _b)
	{
		return _a.r == _b.r && _a.g == _b.g && _a.b == _b.b && _a.a == _b.a;
	}

	// Token: 0x0600A181 RID: 41345 RVA: 0x003CBBD8 File Offset: 0x003C9DD8
	public int GetHashCode(Color32 _a)
	{
		return (((((_a.r.GetHashCode() * 397) ^ _a.g.GetHashCode()) * 397) ^ _a.b.GetHashCode()) * 397) ^ _a.a.GetHashCode();
	}

	// Token: 0x040079DC RID: 31196
	public static readonly Color32EqualityComparer Instance = new Color32EqualityComparer();
}
