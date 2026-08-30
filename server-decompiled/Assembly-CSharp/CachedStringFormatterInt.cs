using System;

// Token: 0x020013B7 RID: 5047
public class CachedStringFormatterInt : CachedStringFormatter<int>
{
	// Token: 0x06009EEA RID: 40682 RVA: 0x003BED3B File Offset: 0x003BCF3B
	public CachedStringFormatterInt()
		: base(new Func<int, string>(CachedStringFormatterInt.formatterFunc))
	{
	}

	// Token: 0x06009EEB RID: 40683 RVA: 0x003BED4F File Offset: 0x003BCF4F
	[PublicizedFrom(EAccessModifier.Private)]
	public static string formatterFunc(int _i)
	{
		return _i.ToString();
	}
}
