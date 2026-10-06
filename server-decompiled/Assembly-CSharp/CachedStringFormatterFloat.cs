using System;

// Token: 0x020013D8 RID: 5080
public class CachedStringFormatterFloat : CachedStringFormatter<float>
{
	// Token: 0x06009F97 RID: 40855 RVA: 0x003C38F4 File Offset: 0x003C1AF4
	public CachedStringFormatterFloat(string _format = null)
		: base(null)
	{
		this.formatter = new Func<float, string>(this.formatterFunc);
		this.format = _format;
	}

	// Token: 0x06009F98 RID: 40856 RVA: 0x003C3916 File Offset: 0x003C1B16
	[PublicizedFrom(EAccessModifier.Private)]
	public string formatterFunc(float _f)
	{
		return _f.ToCultureInvariantString(this.format);
	}

	// Token: 0x040078EF RID: 30959
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string format;
}
