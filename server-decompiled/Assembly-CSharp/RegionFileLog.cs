using System;
using System.Diagnostics;

// Token: 0x02000B9A RID: 2970
public static class RegionFileLog
{
	// Token: 0x060059C3 RID: 22979 RVA: 0x00224CFB File Offset: 0x00222EFB
	[Conditional("DEBUG_REGIONLOG")]
	public static void Region(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Region {1}", GameManager.frameCount, _format);
		Log.Warning(_format, _args);
	}
}
