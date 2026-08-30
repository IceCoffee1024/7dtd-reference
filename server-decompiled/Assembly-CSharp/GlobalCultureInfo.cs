using System;
using System.Globalization;
using System.Threading;

// Token: 0x02001414 RID: 5140
public class GlobalCultureInfo
{
	// Token: 0x0600A176 RID: 41334 RVA: 0x003C8BB5 File Offset: 0x003C6DB5
	public static bool SetDefaultCulture(CultureInfo _culture)
	{
		Thread.CurrentThread.CurrentCulture = _culture;
		CultureInfo.DefaultThreadCurrentCulture = _culture;
		return true;
	}
}
