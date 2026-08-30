using System;
using System.Collections.Generic;

// Token: 0x020013DE RID: 5086
public class DictionaryKeyList<T, S>
{
	// Token: 0x06009FAB RID: 40875 RVA: 0x003C1BDD File Offset: 0x003BFDDD
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.list.Add(_key);
	}

	// Token: 0x06009FAC RID: 40876 RVA: 0x003C1BF8 File Offset: 0x003BFDF8
	public void Remove(T _key)
	{
		this.list.Remove(_key);
		this.dict.Remove(_key);
	}

	// Token: 0x06009FAD RID: 40877 RVA: 0x003C1C14 File Offset: 0x003BFE14
	public void Replace(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x06009FAE RID: 40878 RVA: 0x003C1C33 File Offset: 0x003BFE33
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
	}

	// Token: 0x040078E4 RID: 30948
	public Dictionary<T, S> dict = new Dictionary<T, S>();

	// Token: 0x040078E5 RID: 30949
	public List<T> list = new List<T>();
}
