using System;

// Token: 0x020013B8 RID: 5048
public class CachedStringFormatterFloat : CachedStringFormatter<float>
{
	// Token: 0x06009EEC RID: 40684 RVA: 0x003BED58 File Offset: 0x003BCF58
	public CachedStringFormatterFloat(string _format = null)
		: base(null)
	{
		this.formatter = new Func<float, string>(this.formatterFunc);
		this.format = _format;
	}

	// Token: 0x06009EED RID: 40685 RVA: 0x003BED7A File Offset: 0x003BCF7A
	[PublicizedFrom(EAccessModifier.Private)]
	public string formatterFunc(float _f)
	{
		return _f.ToCultureInvariantString(this.format);
	}

	// Token: 0x04007874 RID: 30836
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string format;
}
