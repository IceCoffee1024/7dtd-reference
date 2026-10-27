using System;
using System.Globalization;
using System.Threading;

// Token: 0x02001446 RID: 5190
public class GlobalCultureInfo
{
	// Token: 0x0600A279 RID: 41593 RVA: 0x003CFB55 File Offset: 0x003CDD55
	public static bool SetDefaultCulture(CultureInfo _culture)
	{
		Thread.CurrentThread.CurrentCulture = _culture;
		CultureInfo.DefaultThreadCurrentCulture = _culture;
		return true;
	}
}
