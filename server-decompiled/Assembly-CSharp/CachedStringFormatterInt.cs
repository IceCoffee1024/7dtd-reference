using System;

// Token: 0x020013E9 RID: 5097
public class CachedStringFormatterInt : CachedStringFormatter<int>
{
	// Token: 0x06009FED RID: 40941 RVA: 0x003C5CDB File Offset: 0x003C3EDB
	public CachedStringFormatterInt()
		: base(new Func<int, string>(CachedStringFormatterInt.formatterFunc))
	{
	}

	// Token: 0x06009FEE RID: 40942 RVA: 0x003C5CEF File Offset: 0x003C3EEF
	[PublicizedFrom(EAccessModifier.Private)]
	public static string formatterFunc(int _i)
	{
		return _i.ToString();
	}
}
