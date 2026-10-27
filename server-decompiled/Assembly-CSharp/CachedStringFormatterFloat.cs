using System;

// Token: 0x020013EA RID: 5098
public class CachedStringFormatterFloat : CachedStringFormatter<float>
{
	// Token: 0x06009FEF RID: 40943 RVA: 0x003C5CF8 File Offset: 0x003C3EF8
	public CachedStringFormatterFloat(string _format = null)
		: base(null)
	{
		this.formatter = new Func<float, string>(this.formatterFunc);
		this.format = _format;
	}

	// Token: 0x06009FF0 RID: 40944 RVA: 0x003C5D1A File Offset: 0x003C3F1A
	[PublicizedFrom(EAccessModifier.Private)]
	public string formatterFunc(float _f)
	{
		return _f.ToCultureInvariantString(this.format);
	}

	// Token: 0x04007930 RID: 31024
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string format;
}
