using System;
using System.Collections.Generic;

// Token: 0x02001410 RID: 5136
public class DictionaryKeyList<T, S>
{
	// Token: 0x0600A0AE RID: 41134 RVA: 0x003C8B7D File Offset: 0x003C6D7D
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.list.Add(_key);
	}

	// Token: 0x0600A0AF RID: 41135 RVA: 0x003C8B98 File Offset: 0x003C6D98
	public void Remove(T _key)
	{
		this.list.Remove(_key);
		this.dict.Remove(_key);
	}

	// Token: 0x0600A0B0 RID: 41136 RVA: 0x003C8BB4 File Offset: 0x003C6DB4
	public void Replace(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x0600A0B1 RID: 41137 RVA: 0x003C8BD3 File Offset: 0x003C6DD3
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
	}

	// Token: 0x040079A0 RID: 31136
	public Dictionary<T, S> dict = new Dictionary<T, S>();

	// Token: 0x040079A1 RID: 31137
	public List<T> list = new List<T>();
}
