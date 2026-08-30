using System;
using System.Collections.Generic;

// Token: 0x020013C7 RID: 5063
public class DictionaryNameId<T>
{
	// Token: 0x06009EC6 RID: 40646 RVA: 0x003BCC21 File Offset: 0x003BAE21
	public DictionaryNameId(DictionaryNameIdMapping _mapping)
	{
		this.mapping = _mapping;
	}

	// Token: 0x06009EC7 RID: 40647 RVA: 0x003BCC3C File Offset: 0x003BAE3C
	public void Add(string _name, T _value)
	{
		int num = this.mapping.Add(_name);
		this.idsToValues[num] = _value;
	}

	// Token: 0x170012A2 RID: 4770
	// (get) Token: 0x06009EC8 RID: 40648 RVA: 0x003BCC63 File Offset: 0x003BAE63
	public int Count
	{
		get
		{
			return this.idsToValues.Count;
		}
	}

	// Token: 0x170012A3 RID: 4771
	// (get) Token: 0x06009EC9 RID: 40649 RVA: 0x003BCC70 File Offset: 0x003BAE70
	public Dictionary<int, T> Dict
	{
		get
		{
			return this.idsToValues;
		}
	}

	// Token: 0x06009ECA RID: 40650 RVA: 0x003BCC78 File Offset: 0x003BAE78
	public bool Contains(string _name)
	{
		int num = this.mapping.FindId(_name);
		return num != 0 && this.idsToValues.ContainsKey(num);
	}

	// Token: 0x06009ECB RID: 40651 RVA: 0x003BCCA4 File Offset: 0x003BAEA4
	public T Get(int _id)
	{
		T t;
		this.idsToValues.TryGetValue(_id, out t);
		return t;
	}

	// Token: 0x06009ECC RID: 40652 RVA: 0x003BCCC4 File Offset: 0x003BAEC4
	public T Get(string _name)
	{
		int num = this.mapping.FindId(_name);
		if (num == 0)
		{
			return default(T);
		}
		T t;
		this.idsToValues.TryGetValue(num, out t);
		return t;
	}

	// Token: 0x04007815 RID: 30741
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryNameIdMapping mapping;

	// Token: 0x04007816 RID: 30742
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, T> idsToValues = new Dictionary<int, T>();
}
