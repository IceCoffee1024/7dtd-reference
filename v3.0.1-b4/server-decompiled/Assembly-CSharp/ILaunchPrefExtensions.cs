using System;
using System.Globalization;

// Token: 0x0200122F RID: 4655
public static class ILaunchPrefExtensions
{
	// Token: 0x06009409 RID: 37897 RVA: 0x0037E971 File Offset: 0x0037CB71
	[PublicizedFrom(EAccessModifier.Private)]
	public static string CreateCommandLineArgument(ILaunchPref pref, string value)
	{
		return "-" + pref.Name + "=" + value;
	}

	// Token: 0x0600940A RID: 37898 RVA: 0x0037E989 File Offset: 0x0037CB89
	public static string ToCommandLine(this ILaunchPref<string> pref, string value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value);
	}

	// Token: 0x0600940B RID: 37899 RVA: 0x0037E992 File Offset: 0x0037CB92
	public static string ToCommandLine(this ILaunchPref<long> pref, long value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value.ToString(CultureInfo.InvariantCulture));
	}

	// Token: 0x0600940C RID: 37900 RVA: 0x0037E9A6 File Offset: 0x0037CBA6
	public static string ToCommandLine(this ILaunchPref<bool> pref, bool value)
	{
		return ILaunchPrefExtensions.CreateCommandLineArgument(pref, value.ToString(CultureInfo.InvariantCulture));
	}
}
