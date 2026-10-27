using System;
using System.Collections.Generic;

// Token: 0x020013EC RID: 5100
public class CaseInsensitiveStringDictionary<TValue> : Dictionary<string, TValue>
{
	// Token: 0x06009FF3 RID: 40947 RVA: 0x003C5D4F File Offset: 0x003C3F4F
	public CaseInsensitiveStringDictionary()
		: base(StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009FF4 RID: 40948 RVA: 0x003C5D5C File Offset: 0x003C3F5C
	public CaseInsensitiveStringDictionary(int _capacity)
		: base(_capacity, StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009FF5 RID: 40949 RVA: 0x003C5D6A File Offset: 0x003C3F6A
	public CaseInsensitiveStringDictionary(IDictionary<string, TValue> _dictionary)
		: base(_dictionary, StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009FF6 RID: 40950 RVA: 0x003C5D78 File Offset: 0x003C3F78
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(IEqualityComparer<string> _comparer)
		: base(_comparer)
	{
	}

	// Token: 0x06009FF7 RID: 40951 RVA: 0x003C5D81 File Offset: 0x003C3F81
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(int _capacity, IEqualityComparer<string> _comparer)
		: base(_capacity, _comparer)
	{
	}

	// Token: 0x06009FF8 RID: 40952 RVA: 0x003C5D8B File Offset: 0x003C3F8B
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(IDictionary<string, TValue> _dictionary, IEqualityComparer<string> _comparer)
		: base(_dictionary, _comparer)
	{
	}
}
