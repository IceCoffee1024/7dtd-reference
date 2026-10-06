using System;
using System.Collections.Generic;

// Token: 0x020013FF RID: 5119
public class DictionaryKeyValueList<T, S>
{
	// Token: 0x0600A05B RID: 41051 RVA: 0x003C6810 File Offset: 0x003C4A10
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.keyList.Add(_key);
		this.valueList.Add(_value);
	}

	// Token: 0x0600A05C RID: 41052 RVA: 0x003C6837 File Offset: 0x003C4A37
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x0600A05D RID: 41053 RVA: 0x003C6858 File Offset: 0x003C4A58
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

	// Token: 0x0600A05E RID: 41054 RVA: 0x003C689B File Offset: 0x003C4A9B
	public void Clear()
	{
		this.keyList.Clear();
		this.valueList.Clear();
		this.dict.Clear();
	}

	// Token: 0x04007961 RID: 31073
	public Dictionary<T, S> dict = new Dictionary<T, S>();

	// Token: 0x04007962 RID: 31074
	public List<S> valueList = new List<S>();

	// Token: 0x04007963 RID: 31075
	public List<T> keyList = new List<T>();
}
