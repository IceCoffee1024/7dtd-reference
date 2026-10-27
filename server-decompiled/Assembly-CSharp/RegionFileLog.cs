using System;
using System.Diagnostics;

// Token: 0x02000BB7 RID: 2999
public static class RegionFileLog
{
	// Token: 0x06005A02 RID: 23042 RVA: 0x002294E7 File Offset: 0x002276E7
	[Conditional("DEBUG_REGIONLOG")]
	public static void Region(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Region {1}", GameManager.frameCount, _format);
		Log.Warning(_format, _args);
	}
}
