using System;
using UnityEngine;

// Token: 0x020013EB RID: 5099
public class CachedStringFormatterXuiRgbaColor : CachedStringFormatter<Color32>
{
	// Token: 0x06009FF1 RID: 40945 RVA: 0x003C5D28 File Offset: 0x003C3F28
	public CachedStringFormatterXuiRgbaColor()
		: base(new Func<Color32, string>(CachedStringFormatterXuiRgbaColor.formatterFunc))
	{
		this.comparer1 = Color32EqualityComparer.Instance;
	}

	// Token: 0x06009FF2 RID: 40946 RVA: 0x003C5D47 File Offset: 0x003C3F47
	[PublicizedFrom(EAccessModifier.Private)]
	public static string formatterFunc(Color32 _color)
	{
		return _color.ToXuiColorString();
	}
}
