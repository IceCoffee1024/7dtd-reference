using System;
using System.Collections.Generic;

// Token: 0x020013D8 RID: 5080
public class EnumDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : struct, IConvertible
{
	// Token: 0x06009F5F RID: 40799 RVA: 0x003BF0B5 File Offset: 0x003BD2B5
	public EnumDictionary()
		: base(new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x06009F60 RID: 40800 RVA: 0x003BF0C2 File Offset: 0x003BD2C2
	public EnumDictionary(int capacity)
		: base(capacity, new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x06009F61 RID: 40801 RVA: 0x003BF0D0 File Offset: 0x003BD2D0
	public EnumDictionary(IDictionary<TKey, TValue> dictionary)
		: base(dictionary, new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x06009F62 RID: 40802 RVA: 0x003BF0DE File Offset: 0x003BD2DE
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(IEqualityComparer<TKey> comparer)
		: base(comparer)
	{
	}

	// Token: 0x06009F63 RID: 40803 RVA: 0x003BF0E7 File Offset: 0x003BD2E7
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(int capacity, IEqualityComparer<TKey> comparer)
		: base(capacity, comparer)
	{
	}

	// Token: 0x06009F64 RID: 40804 RVA: 0x003BF0F1 File Offset: 0x003BD2F1
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		: base(dictionary, comparer)
	{
	}
}
