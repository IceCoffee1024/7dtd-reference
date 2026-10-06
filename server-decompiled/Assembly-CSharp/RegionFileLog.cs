using System;
using System.Diagnostics;

// Token: 0x02000BAC RID: 2988
public static class RegionFileLog
{
	// Token: 0x060059DD RID: 23005 RVA: 0x00227F8B File Offset: 0x0022618B
	[Conditional("DEBUG_REGIONLOG")]
	public static void Region(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Region {1}", GameManager.frameCount, _format);
		Log.Warning(_format, _args);
	}
}
