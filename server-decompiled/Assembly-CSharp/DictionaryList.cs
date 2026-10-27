using System;
using System.Collections.Generic;

// Token: 0x02001413 RID: 5139
public class DictionaryList<T, S>
{
	// Token: 0x0600A0BE RID: 41150 RVA: 0x003C8DF7 File Offset: 0x003C6FF7
	public DictionaryList()
	{
		this.dict = new Dictionary<T, S>();
	}

	// Token: 0x0600A0BF RID: 41151 RVA: 0x003C8E15 File Offset: 0x003C7015
	public DictionaryList(IEqualityComparer<T> _comparer)
	{
		this.dict = new Dictionary<T, S>(_comparer);
	}

	// Token: 0x0600A0C0 RID: 41152 RVA: 0x003C8E34 File Offset: 0x003C7034
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.list.Add(_value);
	}

	// Token: 0x0600A0C1 RID: 41153 RVA: 0x003C8E4F File Offset: 0x003C704F
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x0600A0C2 RID: 41154 RVA: 0x003C8E70 File Offset: 0x003C7070
	public bool Remove(T _key)
	{
		if (this.dict.ContainsKey(_key))
		{
			S s = this.dict[_key];
			this.dict.Remove(_key);
			this.list.Remove(s);
			return true;
		}
		return false;
	}

	// Token: 0x0600A0C3 RID: 41155 RVA: 0x003C8EB8 File Offset: 0x003C70B8
	public S Get(T _key)
	{
		S s;
		if (this.dict.TryGetValue(_key, out s))
		{
			return s;
		}
		return default(S);
	}

	// Token: 0x0600A0C4 RID: 41156 RVA: 0x003C8EE0 File Offset: 0x003C70E0
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
	}

	// Token: 0x1700132B RID: 4907
	// (get) Token: 0x0600A0C5 RID: 41157 RVA: 0x003C8EF8 File Offset: 0x003C70F8
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x040079A8 RID: 31144
	public Dictionary<T, S> dict;

	// Token: 0x040079A9 RID: 31145
	public List<S> list = new List<S>();
}
