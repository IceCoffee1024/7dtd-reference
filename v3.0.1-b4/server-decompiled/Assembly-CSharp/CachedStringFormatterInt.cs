using System;

// Token: 0x0200139C RID: 5020
public class CachedStringFormatterInt : CachedStringFormatter<int>
{
	// Token: 0x06009DED RID: 40429 RVA: 0x003B99F7 File Offset: 0x003B7BF7
	public CachedStringFormatterInt()
		: base(new Func<int, string>(CachedStringFormatterInt.formatterFunc))
	{
	}

	// Token: 0x06009DEE RID: 40430 RVA: 0x003B9A0B File Offset: 0x003B7C0B
	[PublicizedFrom(EAccessModifier.Private)]
	public static string formatterFunc(int _i)
	{
		return _i.ToString();
	}
}
