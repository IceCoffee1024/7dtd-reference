using System;
using System.Collections.Generic;

// Token: 0x020013FE RID: 5118
public class DictionaryKeyList<T, S>
{
	// Token: 0x0600A056 RID: 41046 RVA: 0x003C6779 File Offset: 0x003C4979
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.list.Add(_key);
	}

	// Token: 0x0600A057 RID: 41047 RVA: 0x003C6794 File Offset: 0x003C4994
	public void Remove(T _key)
	{
		this.list.Remove(_key);
		this.dict.Remove(_key);
	}

	// Token: 0x0600A058 RID: 41048 RVA: 0x003C67B0 File Offset: 0x003C49B0
	public void Replace(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x0600A059 RID: 41049 RVA: 0x003C67CF File Offset: 0x003C49CF
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
	}

	// Token: 0x0400795F RID: 31071
	public Dictionary<T, S> dict = new Dictionary<T, S>();

	// Token: 0x04007960 RID: 31072
	public List<T> list = new List<T>();
}
