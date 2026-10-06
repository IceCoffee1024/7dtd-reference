using System;
using System.Collections.Generic;

// Token: 0x020013DA RID: 5082
public class CaseInsensitiveStringDictionary<TValue> : Dictionary<string, TValue>
{
	// Token: 0x06009F9B RID: 40859 RVA: 0x003C394B File Offset: 0x003C1B4B
	public CaseInsensitiveStringDictionary()
		: base(StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009F9C RID: 40860 RVA: 0x003C3958 File Offset: 0x003C1B58
	public CaseInsensitiveStringDictionary(int _capacity)
		: base(_capacity, StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009F9D RID: 40861 RVA: 0x003C3966 File Offset: 0x003C1B66
	public CaseInsensitiveStringDictionary(IDictionary<string, TValue> _dictionary)
		: base(_dictionary, StringComparer.OrdinalIgnoreCase)
	{
	}

	// Token: 0x06009F9E RID: 40862 RVA: 0x003C3974 File Offset: 0x003C1B74
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(IEqualityComparer<string> _comparer)
		: base(_comparer)
	{
	}

	// Token: 0x06009F9F RID: 40863 RVA: 0x003C397D File Offset: 0x003C1B7D
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(int _capacity, IEqualityComparer<string> _comparer)
		: base(_capacity, _comparer)
	{
	}

	// Token: 0x06009FA0 RID: 40864 RVA: 0x003C3987 File Offset: 0x003C1B87
	[Obsolete("CaseInsensitiveStringDictionary constructors with explicit comparer are deprecated in favor of the variants without, as these automatically set the appropriate comparer for the enum key type.", true)]
	public CaseInsensitiveStringDictionary(IDictionary<string, TValue> _dictionary, IEqualityComparer<string> _comparer)
		: base(_dictionary, _comparer)
	{
	}
}
