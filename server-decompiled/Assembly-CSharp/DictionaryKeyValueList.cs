using System;
using System.Collections.Generic;

// Token: 0x020013C4 RID: 5060
public class DictionaryKeyValueList<T, S>
{
	// Token: 0x06009EB3 RID: 40627 RVA: 0x003BC930 File Offset: 0x003BAB30
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		this.keyList.Add(_key);
		this.valueList.Add(_value);
	}

	// Token: 0x06009EB4 RID: 40628 RVA: 0x003BC957 File Offset: 0x003BAB57
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x06009EB5 RID: 40629 RVA: 0x003BC978 File Offset: 0x003BAB78
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

	// Token: 0x06009EB6 RID: 40630 RVA: 0x003BC9BB File Offset: 0x003BABBB
	public void Clear()
	{
		this.keyList.Clear();
		this.valueList.Clear();
		this.dict.Clear();
	}

	// Token: 0x0400780D RID: 30733
	public Dictionary<T, S> dict = new Dictionary<T, S>();

	// Token: 0x0400780E RID: 30734
	public List<S> valueList = new List<S>();

	// Token: 0x0400780F RID: 30735
	public List<T> keyList = new List<T>();
}
