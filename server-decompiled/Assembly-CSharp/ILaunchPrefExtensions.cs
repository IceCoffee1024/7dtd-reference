using System;
using System.Globalization;

// Token: 0x02001263 RID: 4707
public static class ILaunchPrefExtensions
{
	// Token: 0x0600959B RID: 38299 RVA: 0x003884C9 File Offset: 0x003866C9
	[PublicizedFrom(EAccessModifier.Private)]
	public static string CreateCommandLineArgument(ILaunchPref pref, string value)
	{
		return "-" + pref.Name + "=" + value;
	}

	// Token: 0x0600959C RID: 38300 RVA: 0x003884E1 File Offset: 0x003866E1
	public static string ToCommandLine(this ILaunchPref<string> pref, string value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value);
	}

	// Token: 0x0600959D RID: 38301 RVA: 0x003884EA File Offset: 0x003866EA
	public static string ToCommandLine(this ILaunchPref<long> pref, long value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value.ToString(CultureInfo.InvariantCulture));
	}

	// Token: 0x0600959E RID: 38302 RVA: 0x003884FE File Offset: 0x003866FE
	public static string ToCommandLine(this ILaunchPref<bool> pref, bool value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value.ToString(CultureInfo.InvariantCulture));
	}
}
