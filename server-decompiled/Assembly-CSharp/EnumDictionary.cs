using System;
using System.Collections.Generic;

// Token: 0x020013F3 RID: 5107
public class EnumDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : struct, IConvertible
{
	// Token: 0x0600A05C RID: 41052 RVA: 0x003C43F9 File Offset: 0x003C25F9
	public EnumDictionary()
		: base(new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A05D RID: 41053 RVA: 0x003C4406 File Offset: 0x003C2606
	public EnumDictionary(int capacity)
		: base(capacity, new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A05E RID: 41054 RVA: 0x003C4414 File Offset: 0x003C2614
	public EnumDictionary(IDictionary<TKey, TValue> dictionary)
		: base(dictionary, new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A05F RID: 41055 RVA: 0x003C4422 File Offset: 0x003C2622
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(IEqualityComparer<TKey> comparer)
		: base(comparer)
	{
	}

	// Token: 0x0600A060 RID: 41056 RVA: 0x003C442B File Offset: 0x003C262B
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(int capacity, IEqualityComparer<TKey> comparer)
		: base(capacity, comparer)
	{
	}

	// Token: 0x0600A061 RID: 41057 RVA: 0x003C4435 File Offset: 0x003C2635
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		: base(dictionary, comparer)
	{
	}
}
