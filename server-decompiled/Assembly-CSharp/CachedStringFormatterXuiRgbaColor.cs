using System;
using UnityEngine;

// Token: 0x020013D9 RID: 5081
public class CachedStringFormatterXuiRgbaColor : CachedStringFormatter<Color32>
{
	// Token: 0x06009F99 RID: 40857 RVA: 0x003C3924 File Offset: 0x003C1B24
	public CachedStringFormatterXuiRgbaColor()
		: base(new Func<Color32, string>(CachedStringFormatterXuiRgbaColor.formatterFunc))
	{
		this.comparer1 = Color32EqualityComparer.Instance;
	}

	// Token: 0x06009F9A RID: 40858 RVA: 0x003C3943 File Offset: 0x003C1B43
	[PublicizedFrom(EAccessModifier.Private)]
	public static string formatterFunc(Color32 _color)
	{
		return _color.ToXuiColorString();
	}
}
