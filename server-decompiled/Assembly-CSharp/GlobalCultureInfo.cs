using System;
using System.Globalization;
using System.Threading;

// Token: 0x02001434 RID: 5172
public class GlobalCultureInfo
{
	// Token: 0x0600A221 RID: 41505 RVA: 0x003CD751 File Offset: 0x003CB951
	public static bool SetDefaultCulture(CultureInfo _culture)
	{
		Thread.CurrentThread.CurrentCulture = _culture;
		CultureInfo.DefaultThreadCurrentCulture = _culture;
		return true;
	}
}
