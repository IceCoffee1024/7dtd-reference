using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// Token: 0x02001404 RID: 5124
[PublicizedFrom(EAccessModifier.Internal)]
public static class IEnumerableExtensions
{
	// Token: 0x0600A0EA RID: 41194 RVA: 0x003C658D File Offset: 0x003C478D
	[Obsolete("Causes allocations, use Length or Count==0")]
	public static bool IsEmpty<a>(this IEnumerable<a> A_0)
	{
		return !A_0.Any<a>();
	}

	// Token: 0x0600A0EB RID: 41195 RVA: 0x003C6598 File Offset: 0x003C4798
	public static string Join<T>(this IEnumerable<T> A_0)
	{
		return string.Join(", ", A_0.Select<T, string>(new Func<T, string>(IEnumerableExtensions.Join<T>)).ToArray<string>());
	}

	// Token: 0x0600A0EC RID: 41196 RVA: 0x003C65BB File Offset: 0x003C47BB
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public static string Join<T>(T A_0)
	{
		return A_0.ToString();
	}
}
