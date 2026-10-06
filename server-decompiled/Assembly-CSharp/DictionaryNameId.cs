using System;
using System.Collections.Generic;

// Token: 0x02001402 RID: 5122
public class DictionaryNameId<T>
{
	// Token: 0x0600A06E RID: 41070 RVA: 0x003C6B01 File Offset: 0x003C4D01
	public DictionaryNameId(DictionaryNameIdMapping _mapping)
	{
		this.mapping = _mapping;
	}

	// Token: 0x0600A06F RID: 41071 RVA: 0x003C6B1C File Offset: 0x003C4D1C
	public void Add(string _name, T _value)
	{
		int num = this.mapping.Add(_name);
		this.idsToValues[num] = _value;
	}

	// Token: 0x17001320 RID: 4896
	// (get) Token: 0x0600A070 RID: 41072 RVA: 0x003C6B43 File Offset: 0x003C4D43
	public int Count
	{
		get
		{
			return this.idsToValues.Count;
		}
	}

	// Token: 0x17001321 RID: 4897
	// (get) Token: 0x0600A071 RID: 41073 RVA: 0x003C6B50 File Offset: 0x003C4D50
	public Dictionary<int, T> Dict
	{
		get
		{
			return this.idsToValues;
		}
	}

	// Token: 0x0600A072 RID: 41074 RVA: 0x003C6B58 File Offset: 0x003C4D58
	public bool Contains(string _name)
	{
		int num = this.mapping.FindId(_name);
		return num != 0 && this.idsToValues.ContainsKey(num);
	}

	// Token: 0x0600A073 RID: 41075 RVA: 0x003C6B84 File Offset: 0x003C4D84
	public T Get(int _id)
	{
		T t;
		this.idsToValues.TryGetValue(_id, out t);
		return t;
	}

	// Token: 0x0600A074 RID: 41076 RVA: 0x003C6BA4 File Offset: 0x003C4DA4
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

	// Token: 0x04007969 RID: 31081
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryNameIdMapping mapping;

	// Token: 0x0400796A RID: 31082
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, T> idsToValues = new Dictionary<int, T>();
}
