using System;
using System.Collections.Generic;

// Token: 0x02001411 RID: 5137
public class DictionaryKeyValueList<T, S>
{
	// Token: 0x0600A0B3 RID: 41139 RVA: 0x003C8C14 File Offset: 0x003C6E14
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.keyList.Add(_key);
		this.valueList.Add(_value);
	}

	// Token: 0x0600A0B4 RID: 41140 RVA: 0x003C8C3B File Offset: 0x003C6E3B
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x0600A0B5 RID: 41141 RVA: 0x003C8C5C File Offset: 0x003C6E5C
	public void Remove(T _key)
	{
		int num = this.keyList.IndexOf(_key);
		if (num >= 0)
		{
			this.keyList.RemoveAt(num);
			this.valueList.RemoveAt(num);
			this.dict.Remove(_key);
		}
	}

	// Token: 0x0600A0B6 RID: 41142 RVA: 0x003C8C9F File Offset: 0x003C6E9F
	public void Clear()
	{
		this.keyList.Clear();
		this.valueList.Clear();
		this.dict.Clear();
	}

	// Token: 0x040079A2 RID: 31138
	public Dictionary<T, S> dict = new Dictionary<T, S>();

	// Token: 0x040079A3 RID: 31139
	public List<S> valueList = new List<S>();

	// Token: 0x040079A4 RID: 31140
	public List<T> keyList = new List<T>();
}
