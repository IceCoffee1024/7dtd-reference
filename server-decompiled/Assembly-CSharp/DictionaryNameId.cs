using System;
using System.Collections.Generic;

// Token: 0x020013E2 RID: 5090
public class DictionaryNameId<T>
{
	// Token: 0x06009FC3 RID: 40899 RVA: 0x003C1F65 File Offset: 0x003C0165
	public DictionaryNameId(DictionaryNameIdMapping _mapping)
	{
		this.mapping = _mapping;
	}

	// Token: 0x06009FC4 RID: 40900 RVA: 0x003C1F80 File Offset: 0x003C0180
	public void Add(string _name, T _value)
	{
		int num = this.mapping.Add(_name);
		this.idsToValues[num] = _value;
	}

	// Token: 0x170012DB RID: 4827
	// (get) Token: 0x06009FC5 RID: 40901 RVA: 0x003C1FA7 File Offset: 0x003C01A7
	public int Count
	{
		get
		{
			return this.idsToValues.Count;
		}
	}

	// Token: 0x170012DC RID: 4828
	// (get) Token: 0x06009FC6 RID: 40902 RVA: 0x003C1FB4 File Offset: 0x003C01B4
	public Dictionary<int, T> Dict
	{
		get
		{
			return this.idsToValues;
		}
	}

	// Token: 0x06009FC7 RID: 40903 RVA: 0x003C1FBC File Offset: 0x003C01BC
	public bool Contains(string _name)
	{
		int num = this.mapping.FindId(_name);
		return num != 0 && this.idsToValues.ContainsKey(num);
	}

	// Token: 0x06009FC8 RID: 40904 RVA: 0x003C1FE8 File Offset: 0x003C01E8
	public T Get(int _id)
	{
		T t;
		this.idsToValues.TryGetValue(_id, out t);
		return t;
	}

	// Token: 0x06009FC9 RID: 40905 RVA: 0x003C2008 File Offset: 0x003C0208
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

	// Token: 0x040078EE RID: 30958
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryNameIdMapping mapping;

	// Token: 0x040078EF RID: 30959
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, T> idsToValues = new Dictionary<int, T>();
}
