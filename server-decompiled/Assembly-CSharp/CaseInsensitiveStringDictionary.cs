using System;
using System.Collections.Generic;

// Token: 0x0200139F RID: 5023
public class CaseInsensitiveStringDictionary<TValue> : Dictionary<string, TValue>
{
	// Token: 0x06009DF3 RID: 40435 RVA: 0x003B9A6B File Offset: 0x003B7C6B
	public CaseInsensitiveStringDictionary()
		: base(StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009DF4 RID: 40436 RVA: 0x003B9A78 File Offset: 0x003B7C78
	public CaseInsensitiveStringDictionary(int _capacity)
		: base(_capacity, StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009DF5 RID: 40437 RVA: 0x003B9A86 File Offset: 0x003B7C86
	public CaseInsensitiveStringDictionary(IDictionary<string, TValue> _dictionary)
		: base(_dictionary, StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009DF6 RID: 40438 RVA: 0x003B9A94 File Offset: 0x003B7C94
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(IEqualityComparer<string> _comparer)
		: base(_comparer)
	{
	}

	// Token: 0x06009DF7 RID: 40439 RVA: 0x003B9A9D File Offset: 0x003B7C9D
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(int _capacity, IEqualityComparer<string> _comparer)
		: base(_capacity, _comparer)
	{
	}

	// Token: 0x06009DF8 RID: 40440 RVA: 0x003B9AA7 File Offset: 0x003B7CA7
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(IDictionary<string, TValue> _dictionary, IEqualityComparer<string> _comparer)
		: base(_dictionary, _comparer)
	{
	}
}
