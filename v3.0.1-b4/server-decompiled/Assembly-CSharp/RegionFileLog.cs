using System;
using System.Diagnostics;

// Token: 0x02000B88 RID: 2952
public static class RegionFileLog
{
	// Token: 0x0600593B RID: 22843 RVA: 0x00221FDF File Offset: 0x002201DF
	[Conditional("DEBUG_REGIONLOG")]
	public static void Region(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Region {1}", GameManager.frameCount, _format);
		Log.Warning(_format, _args);
	}
}
