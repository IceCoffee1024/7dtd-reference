using System;
using System.Collections.Generic;

// Token: 0x020013D3 RID: 5075
public sealed class BiDictionary<TKey, TValue>
{
	// Token: 0x06009F81 RID: 40833 RVA: 0x003C3E44 File Offset: 0x003C2044
	public void Add(TKey key, TValue value)
	{
		if (this.m_keyToValue.ContainsKey(key))
		{
			throw new ArgumentException("Key already in dictionary.", "key");
		}
		if (this.m_valueToKey.ContainsKey(value))
		{
			throw new ArgumentException("Value already in dictionary.", "value");
		}
		this.m_keyToValue.Add(key, value);
		this.m_valueToKey.Add(value, key);
	}

	// Token: 0x06009F82 RID: 40834 RVA: 0x003C3EA7 File Offset: 0x003C20A7
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValue.ContainsKey(key);
	}

	// Token: 0x06009F83 RID: 40835 RVA: 0x003C3EB5 File Offset: 0x003C20B5
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKey.ContainsKey(value);
	}

	// Token: 0x06009F84 RID: 40836 RVA: 0x003C3EC3 File Offset: 0x003C20C3
	public bool TryGetByKey(TKey key, out TValue value)
	{
		return this.m_keyToValue.TryGetValue(key, out value);
	}

	// Token: 0x06009F85 RID: 40837 RVA: 0x003C3ED2 File Offset: 0x003C20D2
	public bool TryGetByValue(TValue value, out TKey key)
	{
		return this.m_valueToKey.TryGetValue(value, out key);
	}

	// Token: 0x06009F86 RID: 40838 RVA: 0x003C3EE4 File Offset: 0x003C20E4
	public bool RemoveByKey(TKey key)
	{
		TValue tvalue;
		if (!this.m_keyToValue.Remove(key, out tvalue))
		{
			return false;
		}
		this.m_valueToKey.Remove(tvalue);
		return true;
	}

	// Token: 0x06009F87 RID: 40839 RVA: 0x003C3F14 File Offset: 0x003C2114
	public bool RemoveByValue(TValue value)
	{
		TKey tkey;
		if (!this.m_valueToKey.Remove(value, out tkey))
		{
			return false;
		}
		this.m_keyToValue.Remove(tkey);
		return true;
	}

	// Token: 0x040078DA RID: 30938
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, TValue> m_keyToValue = new Dictionary<TKey, TValue>();

	// Token: 0x040078DB RID: 30939
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TValue, TKey> m_valueToKey = new Dictionary<TValue, TKey>();
}
