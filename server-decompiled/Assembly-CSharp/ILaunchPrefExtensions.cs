using System;
using System.Globalization;

// Token: 0x0200124B RID: 4683
public static class ILaunchPrefExtensions
{
	// Token: 0x0600950D RID: 38157 RVA: 0x00383C81 File Offset: 0x00381E81
	[PublicizedFrom(EAccessModifier.Private)]
	public static string CreateCommandLineArgument(ILaunchPref pref, string value)
	{
		return "-" + pref.Name + "=" + value;
	}

	// Token: 0x0600950E RID: 38158 RVA: 0x00383C99 File Offset: 0x00381E99
	public static string ToCommandLine(this ILaunchPref<string> pref, string value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value);
	}

	// Token: 0x0600950F RID: 38159 RVA: 0x00383CA2 File Offset: 0x00381EA2
	public static string ToCommandLine(this ILaunchPref<long> pref, long value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value.ToString(CultureInfo.InvariantCulture));
	}

	// Token: 0x06009510 RID: 38160 RVA: 0x00383CB6 File Offset: 0x00381EB6
	public static string ToCommandLine(this ILaunchPref<bool> pref, bool value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value.ToString(CultureInfo.InvariantCulture));
	}
}
