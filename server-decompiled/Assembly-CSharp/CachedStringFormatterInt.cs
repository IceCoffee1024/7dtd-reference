using System;

// Token: 0x020013D7 RID: 5079
public class CachedStringFormatterInt : CachedStringFormatter<int>
{
	// Token: 0x06009F95 RID: 40853 RVA: 0x003C38D7 File Offset: 0x003C1AD7
	public CachedStringFormatterInt()
		: base(new Func<int, string>(CachedStringFormatterInt.formatterFunc))
	{
	}

	// Token: 0x06009F96 RID: 40854 RVA: 0x003C38EB File Offset: 0x003C1AEB
	[PublicizedFrom(EAccessModifier.Private)]
	public static string formatterFunc(int _i)
	{
		return _i.ToString();
	}
}
