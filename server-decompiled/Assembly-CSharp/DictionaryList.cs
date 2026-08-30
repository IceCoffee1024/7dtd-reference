using System;
using System.Collections.Generic;

// Token: 0x020013E1 RID: 5089
public class DictionaryList<T, S>
{
	// Token: 0x06009FBB RID: 40891 RVA: 0x003C1E57 File Offset: 0x003C0057
	public DictionaryList()
	{
		this.dict = new Dictionary<T, S>();
	}

	// Token: 0x06009FBC RID: 40892 RVA: 0x003C1E75 File Offset: 0x003C0075
	public DictionaryList(IEqualityComparer<T> _comparer)
	{
		this.dict = new Dictionary<T, S>(_comparer);
	}

	// Token: 0x06009FBD RID: 40893 RVA: 0x003C1E94 File Offset: 0x003C0094
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.list.Add(_value);
	}

	// Token: 0x06009FBE RID: 40894 RVA: 0x003C1EAF File Offset: 0x003C00AF
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x06009FBF RID: 40895 RVA: 0x003C1ED0 File Offset: 0x003C00D0
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

	// Token: 0x06009FC0 RID: 40896 RVA: 0x003C1F18 File Offset: 0x003C0118
	public S Get(T _key)
	{
		S s;
		if (this.dict.TryGetValue(_key, out s))
		{
			return s;
		}
		return default(S);
	}

	// Token: 0x06009FC1 RID: 40897 RVA: 0x003C1F40 File Offset: 0x003C0140
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
	}

	// Token: 0x170012DA RID: 4826
	// (get) Token: 0x06009FC2 RID: 40898 RVA: 0x003C1F58 File Offset: 0x003C0158
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x040078EC RID: 30956
	public Dictionary<T, S> dict;

	// Token: 0x040078ED RID: 30957
	public List<S> list = new List<S>();
}
