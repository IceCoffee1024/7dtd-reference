using System;
using UnityEngine;

// Token: 0x020013B9 RID: 5049
public class CachedStringFormatterXuiRgbaColor : CachedStringFormatter<Color32>
{
	// Token: 0x06009EEE RID: 40686 RVA: 0x003BED88 File Offset: 0x003BCF88
	public CachedStringFormatterXuiRgbaColor()
		: base(new Func<Color32, string>(CachedStringFormatterXuiRgbaColor.formatterFunc))
	{
		this.comparer1 = Color32EqualityComparer.Instance;
	}

	// Token: 0x06009EEF RID: 40687 RVA: 0x003BEDA7 File Offset: 0x003BCFA7
	[PublicizedFrom(EAccessModifier.Private)]
	public static string formatterFunc(Color32 _color)
	{
		return _color.ToXuiColorString();
	}
}
