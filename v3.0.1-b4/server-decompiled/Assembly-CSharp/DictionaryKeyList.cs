using System;
using System.Collections.Generic;

// Token: 0x020013C3 RID: 5059
public class DictionaryKeyList<T, S>
{
	// Token: 0x06009EAE RID: 40622 RVA: 0x003BC899 File Offset: 0x003BAA99
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.list.Add(_key);
	}

	// Token: 0x06009EAF RID: 40623 RVA: 0x003BC8B4 File Offset: 0x003BAAB4
	public void Remove(T _key)
	{
		this.list.Remove(_key);
		this.dict.Remove(_key);
	}

	// Token: 0x06009EB0 RID: 40624 RVA: 0x003BC8D0 File Offset: 0x003BAAD0
	public void Replace(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x06009EB1 RID: 40625 RVA: 0x003BC8EF File Offset: 0x003BAAEF
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
	}

	// Token: 0x0400780B RID: 30731
	public Dictionary<T, S> dict = new Dictionary<T, S>();

	// Token: 0x0400780C RID: 30732
	public List<T> list = new List<T>();
}
