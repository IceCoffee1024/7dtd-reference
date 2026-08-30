using System;
using UnityEngine;

// Token: 0x0200139E RID: 5022
public class CachedStringFormatterXuiRgbaColor : CachedStringFormatter<Color32>
{
	// Token: 0x06009DF1 RID: 40433 RVA: 0x003B9A44 File Offset: 0x003B7C44
	public CachedStringFormatterXuiRgbaColor()
		: base(new Func<Color32, string>(CachedStringFormatterXuiRgbaColor.formatterFunc))
	{
		this.comparer1 = Color32EqualityComparer.Instance;
	}

	// Token: 0x06009DF2 RID: 40434 RVA: 0x003B9A63 File Offset: 0x003B7C63
	[PublicizedFrom(EAccessModifier.Private)]
	public static string formatterFunc(Color32 _color)
	{
		return _color.ToXuiColorString();
	}
}
