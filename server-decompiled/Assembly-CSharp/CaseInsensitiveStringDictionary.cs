using System;
using System.Collections.Generic;

// Token: 0x020013BA RID: 5050
public class CaseInsensitiveStringDictionary<TValue> : Dictionary<string, TValue>
{
	// Token: 0x06009EF0 RID: 40688 RVA: 0x003BEDAF File Offset: 0x003BCFAF
	public CaseInsensitiveStringDictionary()
		: base(StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009EF1 RID: 40689 RVA: 0x003BEDBC File Offset: 0x003BCFBC
	public CaseInsensitiveStringDictionary(int _capacity)
		: base(_capacity, StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009EF2 RID: 40690 RVA: 0x003BEDCA File Offset: 0x003BCFCA
	public CaseInsensitiveStringDictionary(IDictionary<string, TValue> _dictionary)
		: base(_dictionary, StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009EF3 RID: 40691 RVA: 0x003BEDD8 File Offset: 0x003BCFD8
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(IEqualityComparer<string> _comparer)
		: base(_comparer)
	{
	}

	// Token: 0x06009EF4 RID: 40692 RVA: 0x003BEDE1 File Offset: 0x003BCFE1
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(int _capacity, IEqualityComparer<string> _comparer)
		: base(_capacity, _comparer)
	{
	}

	// Token: 0x06009EF5 RID: 40693 RVA: 0x003BEDEB File Offset: 0x003BCFEB
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(IDictionary<string, TValue> _dictionary, IEqualityComparer<string> _comparer)
		: base(_dictionary, _comparer)
	{
	}
}
