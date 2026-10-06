using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// Token: 0x0200143F RID: 5183
[PublicizedFrom(EAccessModifier.Internal)]
public static class IEnumerableExtensions
{
	// Token: 0x0600A292 RID: 41618 RVA: 0x003D046D File Offset: 0x003CE66D
	[Obsolete("Causes allocations, use Length or Count==0")]
	public static bool IsEmpty<a>(this IEnumerable<a> A_0)
	{
		return !A_0.Any<a>();
	}

	// Token: 0x0600A293 RID: 41619 RVA: 0x003D0478 File Offset: 0x003CE678
	public static string Join<T>(this IEnumerable<T> A_0)
	{
		return string.Join(", ", A_0.Select<T, string>(new Func<T, string>(IEnumerableExtensions.Join<T>)).ToArray<string>());
	}

	// Token: 0x0600A294 RID: 41620 RVA: 0x003D049B File Offset: 0x003CE69B
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public static string Join<T>(T A_0)
	{
		return A_0.ToString();
	}
}
