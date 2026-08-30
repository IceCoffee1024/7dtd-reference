using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013F9 RID: 5113
public class Color32EqualityComparer : IEqualityComparer<Color32>
{
	// Token: 0x0600A07C RID: 41084 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32EqualityComparer()
	{
	}

	// Token: 0x0600A07D RID: 41085 RVA: 0x003C4BFA File Offset: 0x003C2DFA
	public bool Equals(Color32 _a, Color32 _b)
	{
		return _a.r == _b.r && _a.g == _b.g && _a.b == _b.b && _a.a == _b.a;
	}

	// Token: 0x0600A07E RID: 41086 RVA: 0x003C4C38 File Offset: 0x003C2E38
	public int GetHashCode(Color32 _a)
	{
		return (((((_a.r.GetHashCode() * 397) ^ _a.g.GetHashCode()) * 397) ^ _a.b.GetHashCode()) * 397) ^ _a.a.GetHashCode();
	}

	// Token: 0x04007920 RID: 31008
	public static readonly Color32EqualityComparer Instance = new Color32EqualityComparer();
}
