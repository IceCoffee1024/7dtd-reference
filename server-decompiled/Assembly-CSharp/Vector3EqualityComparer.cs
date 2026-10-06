using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200141A RID: 5146
public class Vector3EqualityComparer : IEqualityComparer<Vector3>
{
	// Token: 0x0600A12B RID: 41259 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3EqualityComparer()
	{
	}

	// Token: 0x0600A12C RID: 41260 RVA: 0x003C9832 File Offset: 0x003C7A32
	public bool Equals(Vector3 _a, Vector3 _b)
	{
		return _a == _b;
	}

	// Token: 0x0600A12D RID: 41261 RVA: 0x003C983B File Offset: 0x003C7A3B
	public int GetHashCode(Vector3 _a)
	{
		return _a.GetHashCode();
	}

	// Token: 0x0400799C RID: 31132
	public static readonly Vector3EqualityComparer Instance = new Vector3EqualityComparer();
}
