using System;
using System.Collections.Generic;

// Token: 0x020013C6 RID: 5062
public class DictionaryList<T, S>
{
	// Token: 0x06009EBE RID: 40638 RVA: 0x003BCB13 File Offset: 0x003BAD13
	public DictionaryList()
	{
		this.dict = new Dictionary<T, S>();
	}

	// Token: 0x06009EBF RID: 40639 RVA: 0x003BCB31 File Offset: 0x003BAD31
	public DictionaryList(IEqualityComparer<T> _comparer)
	{
		this.dict = new Dictionary<T, S>(_comparer);
	}

	// Token: 0x06009EC0 RID: 40640 RVA: 0x003BCB50 File Offset: 0x003BAD50
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.list.Add(_value);
	}

	// Token: 0x06009EC1 RID: 40641 RVA: 0x003BCB6B File Offset: 0x003BAD6B
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x06009EC2 RID: 40642 RVA: 0x003BCB8C File Offset: 0x003BAD8C
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

	// Token: 0x06009EC3 RID: 40643 RVA: 0x003BCBD4 File Offset: 0x003BADD4
	public S Get(T _key)
	{
		S s;
		if (this.dict.TryGetValue(_key, out s))
		{
			return s;
		}
		return default(S);
	}

	// Token: 0x06009EC4 RID: 40644 RVA: 0x003BCBFC File Offset: 0x003BADFC
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
	}

	// Token: 0x170012A1 RID: 4769
	// (get) Token: 0x06009EC5 RID: 40645 RVA: 0x003BCC14 File Offset: 0x003BAE14
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x04007813 RID: 30739
	public Dictionary<T, S> dict;

	// Token: 0x04007814 RID: 30740
	public List<S> list = new List<S>();
}
