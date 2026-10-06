using System;
using System.Collections.Generic;

// Token: 0x02001413 RID: 5139
public class EnumDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : struct, IConvertible
{
	// Token: 0x0600A107 RID: 41223 RVA: 0x003C8F95 File Offset: 0x003C7195
	public EnumDictionary()
		: base(new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A108 RID: 41224 RVA: 0x003C8FA2 File Offset: 0x003C71A2
	public EnumDictionary(int capacity)
		: base(capacity, new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A109 RID: 41225 RVA: 0x003C8FB0 File Offset: 0x003C71B0
	public EnumDictionary(IDictionary<TKey, TValue> dictionary)
		: base(dictionary, new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A10A RID: 41226 RVA: 0x003C8FBE File Offset: 0x003C71BE
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(IEqualityComparer<TKey> comparer)
		: base(comparer)
	{
	}

	// Token: 0x0600A10B RID: 41227 RVA: 0x003C8FC7 File Offset: 0x003C71C7
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(int capacity, IEqualityComparer<TKey> comparer)
		: base(capacity, comparer)
	{
	}

	// Token: 0x0600A10C RID: 41228 RVA: 0x003C8FD1 File Offset: 0x003C71D1
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		: base(dictionary, comparer)
	{
	}
}
