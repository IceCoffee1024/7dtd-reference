using System;
using System.Collections.Generic;

// Token: 0x02001425 RID: 5157
public class EnumDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : struct, IConvertible
{
	// Token: 0x0600A15F RID: 41311 RVA: 0x003CB399 File Offset: 0x003C9599
	public EnumDictionary()
		: base(new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A160 RID: 41312 RVA: 0x003CB3A6 File Offset: 0x003C95A6
	public EnumDictionary(int capacity)
		: base(capacity, new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A161 RID: 41313 RVA: 0x003CB3B4 File Offset: 0x003C95B4
	public EnumDictionary(IDictionary<TKey, TValue> dictionary)
		: base(dictionary, new FastEnumIntEqualityComparer<TKey>())
	{
	}

	// Token: 0x0600A162 RID: 41314 RVA: 0x003CB3C2 File Offset: 0x003C95C2
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(IEqualityComparer<TKey> comparer)
		: base(comparer)
	{
	}

	// Token: 0x0600A163 RID: 41315 RVA: 0x003CB3CB File Offset: 0x003C95CB
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(int capacity, IEqualityComparer<TKey> comparer)
		: base(capacity, comparer)
	{
	}

	// Token: 0x0600A164 RID: 41316 RVA: 0x003CB3D5 File Offset: 0x003C95D5
	[Obsolete("EnumDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public EnumDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		: base(dictionary, comparer)
	{
	}
}
