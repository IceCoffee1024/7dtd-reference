using System;
using System.Globalization;
using System.Threading;

// Token: 0x020013F9 RID: 5113
public class GlobalCultureInfo
{
	// Token: 0x0600A079 RID: 41081 RVA: 0x003C3871 File Offset: 0x003C1A71
	public static bool SetDefaultCulture(CultureInfo _culture)
	{
		Thread.CurrentThread.CurrentCulture = _culture;
		CultureInfo.DefaultThreadCurrentCulture = _culture;
		return true;
	}
}
