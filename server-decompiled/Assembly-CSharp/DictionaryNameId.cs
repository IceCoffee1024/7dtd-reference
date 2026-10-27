using System;
using System.Collections.Generic;

// Token: 0x02001414 RID: 5140
public class DictionaryNameId<T>
{
	// Token: 0x0600A0C6 RID: 41158 RVA: 0x003C8F05 File Offset: 0x003C7105
	public DictionaryNameId(DictionaryNameIdMapping _mapping)
	{
		this.mapping = _mapping;
	}

	// Token: 0x0600A0C7 RID: 41159 RVA: 0x003C8F20 File Offset: 0x003C7120
	public void Add(string _name, T _value)
	{
		int num = this.mapping.Add(_name);
		this.idsToValues[num] = _value;
	}

	// Token: 0x1700132C RID: 4908
	// (get) Token: 0x0600A0C8 RID: 41160 RVA: 0x003C8F47 File Offset: 0x003C7147
	public int Count
	{
		get
		{
			return this.idsToValues.Count;
		}
	}

	// Token: 0x1700132D RID: 4909
	// (get) Token: 0x0600A0C9 RID: 41161 RVA: 0x003C8F54 File Offset: 0x003C7154
	public Dictionary<int, T> Dict
	{
		get
		{
			return this.idsToValues;
		}
	}

	// Token: 0x0600A0CA RID: 41162 RVA: 0x003C8F5C File Offset: 0x003C715C
	public bool Contains(string _name)
	{
		int num = this.mapping.FindId(_name);
		return num != 0 && this.idsToValues.ContainsKey(num);
	}

	// Token: 0x0600A0CB RID: 41163 RVA: 0x003C8F88 File Offset: 0x003C7188
	public T Get(int _id)
	{
		T t;
		this.idsToValues.TryGetValue(_id, out t);
		return t;
	}

	// Token: 0x0600A0CC RID: 41164 RVA: 0x003C8FA8 File Offset: 0x003C71A8
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

	// Token: 0x040079AA RID: 31146
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryNameIdMapping mapping;

	// Token: 0x040079AB RID: 31147
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, T> idsToValues = new Dictionary<int, T>();
}
