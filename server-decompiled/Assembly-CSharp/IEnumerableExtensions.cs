using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// Token: 0x0200141F RID: 5151
[PublicizedFrom(EAccessModifier.Internal)]
public static class IEnumerableExtensions
{
	// Token: 0x0600A1E7 RID: 41447 RVA: 0x003CB8D1 File Offset: 0x003C9AD1
	[Obsolete("Causes allocations, use Length or Count==0")]
	public static bool IsEmpty<a>(this IEnumerable<a> A_0)
	{
		return !A_0.Any<a>();
	}

	// Token: 0x0600A1E8 RID: 41448 RVA: 0x003CB8DC File Offset: 0x003C9ADC
	public static string Join<T>(this IEnumerable<T> A_0)
	{
		return string.Join(", ", A_0.Select<T, string>(new Func<T, string>(IEnumerableExtensions.Join<T>)).ToArray<string>());
	}

	// Token: 0x0600A1E9 RID: 41449 RVA: 0x003CB8FF File Offset: 0x003C9AFF
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public static string Join<T>(T A_0)
	{
		return A_0.ToString();
	}
}
