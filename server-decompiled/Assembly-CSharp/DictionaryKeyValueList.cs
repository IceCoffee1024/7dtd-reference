using System;
using System.Collections.Generic;

// Token: 0x020013DF RID: 5087
public class DictionaryKeyValueList<T, S>
{
	// Token: 0x06009FB0 RID: 40880 RVA: 0x003C1C74 File Offset: 0x003BFE74
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.keyList.Add(_key);
		this.valueList.Add(_value);
	}

	// Token: 0x06009FB1 RID: 40881 RVA: 0x003C1C9B File Offset: 0x003BFE9B
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x06009FB2 RID: 40882 RVA: 0x003C1CBC File Offset: 0x003BFEBC
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

	// Token: 0x06009FB3 RID: 40883 RVA: 0x003C1CFF File Offset: 0x003BFEFF
	public void Clear()
	{
		this.keyList.Clear();
		this.valueList.Clear();
		this.dict.Clear();
	}

	// Token: 0x040078E6 RID: 30950
	public Dictionary<T, S> dict = new Dictionary<T, S>();

	// Token: 0x040078E7 RID: 30951
	public List<S> valueList = new List<S>();

	// Token: 0x040078E8 RID: 30952
	public List<T> keyList = new List<T>();
}
