using System;
using System.Globalization;

// Token: 0x02001275 RID: 4725
public static class ILaunchPrefExtensions
{
	// Token: 0x060095F2 RID: 38386 RVA: 0x0038A8C5 File Offset: 0x00388AC5
	[PublicizedFrom(EAccessModifier.Private)]
	public static string CreateCommandLineArgument(ILaunchPref pref, string value)
	{
		return "-" + pref.Name + "=" + value;
	}

	// Token: 0x060095F3 RID: 38387 RVA: 0x0038A8DD File Offset: 0x00388ADD
	public static string ToCommandLine(this ILaunchPref<string> pref, string value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value);
	}

	// Token: 0x060095F4 RID: 38388 RVA: 0x0038A8E6 File Offset: 0x00388AE6
	public static string ToCommandLine(this ILaunchPref<long> pref, long value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value.ToString(CultureInfo.InvariantCulture));
	}

	// Token: 0x060095F5 RID: 38389 RVA: 0x0038A8FA File Offset: 0x00388AFA
	public static string ToCommandLine(this ILaunchPref<bool> pref, bool value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value.ToString(CultureInfo.InvariantCulture));
	}
}
