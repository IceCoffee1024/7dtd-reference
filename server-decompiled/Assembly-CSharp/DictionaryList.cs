using System;
using System.Collections.Generic;

// Token: 0x02001401 RID: 5121
public class DictionaryList<T, S>
{
	// Token: 0x0600A066 RID: 41062 RVA: 0x003C69F3 File Offset: 0x003C4BF3
	public DictionaryList()
	{
		this.dict = new Dictionary<T, S>();
	}

	// Token: 0x0600A067 RID: 41063 RVA: 0x003C6A11 File Offset: 0x003C4C11
	public DictionaryList(IEqualityComparer<T> _comparer)
	{
		this.dict = new Dictionary<T, S>(_comparer);
	}

	// Token: 0x0600A068 RID: 41064 RVA: 0x003C6A30 File Offset: 0x003C4C30
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.list.Add(_value);
	}

	// Token: 0x0600A069 RID: 41065 RVA: 0x003C6A4B File Offset: 0x003C4C4B
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x0600A06A RID: 41066 RVA: 0x003C6A6C File Offset: 0x003C4C6C
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

	// Token: 0x0600A06B RID: 41067 RVA: 0x003C6AB4 File Offset: 0x003C4CB4
	public S Get(T _key)
	{
		S s;
		if (this.dict.TryGetValue(_key, out s))
		{
			return s;
		}
		return default(S);
	}

	// Token: 0x0600A06C RID: 41068 RVA: 0x003C6ADC File Offset: 0x003C4CDC
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
	}

	// Token: 0x1700131F RID: 4895
	// (get) Token: 0x0600A06D RID: 41069 RVA: 0x003C6AF4 File Offset: 0x003C4CF4
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x04007967 RID: 31079
	public Dictionary<T, S> dict;

	// Token: 0x04007968 RID: 31080
	public List<S> list = new List<S>();
}
