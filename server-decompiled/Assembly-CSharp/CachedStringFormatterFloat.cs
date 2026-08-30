using System;

// Token: 0x0200139D RID: 5021
public class CachedStringFormatterFloat : CachedStringFormatter<float>
{
	// Token: 0x06009DEF RID: 40431 RVA: 0x003B9A14 File Offset: 0x003B7C14
	public CachedStringFormatterFloat(string _format = null)
		: base(null)
	{
		this.formatter = new Func<float, string>(this.formatterFunc);
		this.format = _format;
	}

	// Token: 0x06009DF0 RID: 40432 RVA: 0x003B9A36 File Offset: 0x003B7C36
	[PublicizedFrom(EAccessModifier.Private)]
	public string formatterFunc(float _f)
	{
		return _f.ToCultureInvariantString(this.format);
	}

	// Token: 0x0400779B RID: 30619
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string format;
}
