using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// Token: 0x02001451 RID: 5201
[PublicizedFrom(EAccessModifier.Internal)]
public static class IEnumerableExtensions
{
	// Token: 0x0600A2EA RID: 41706 RVA: 0x003D2871 File Offset: 0x003D0A71
	[Obsolete("Causes allocations, use Length or Count==0")]
	public static bool IsEmpty<a>(this IEnumerable<a> A_0)
	{
		return !A_0.Any<a>();
	}

	// Token: 0x0600A2EB RID: 41707 RVA: 0x003D287C File Offset: 0x003D0A7C
	public static string Join<T>(this IEnumerable<T> A_0)
	{
		return string.Join(", ", A_0.Select<T, string>(new Func<T, string>(IEnumerableExtensions.Join<T>)).ToArray<string>());
	}

	// Token: 0x0600A2EC RID: 41708 RVA: 0x003D289F File Offset: 0x003D0A9F
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public static string Join<T>(T A_0)
	{
		return A_0.ToString();
	}
}
