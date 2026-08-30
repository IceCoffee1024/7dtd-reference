using System;
using System.Collections.Generic;

// Token: 0x02001386 RID: 4998
public sealed class BiDictionary<TKey, TValue>
{
	// Token: 0x06009D81 RID: 40321 RVA: 0x003B7B60 File Offset: 0x003B5D60
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

	// Token: 0x06009D82 RID: 40322 RVA: 0x003B7BC3 File Offset: 0x003B5DC3
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValue.ContainsKey(key);
	}

	// Token: 0x06009D83 RID: 40323 RVA: 0x003B7BD1 File Offset: 0x003B5DD1
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKey.ContainsKey(value);
	}

	// Token: 0x06009D84 RID: 40324 RVA: 0x003B7BDF File Offset: 0x003B5DDF
	public bool TryGetByKey(TKey key, out TValue value)
	{
		return this.m_keyToValue.TryGetValue(key, out value);
	}

	// Token: 0x06009D85 RID: 40325 RVA: 0x003B7BEE File Offset: 0x003B5DEE
	public bool TryGetByValue(TValue value, out TKey key)
	{
		return this.m_valueToKey.TryGetValue(value, out key);
	}

	// Token: 0x06009D86 RID: 40326 RVA: 0x003B7C00 File Offset: 0x003B5E00
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

	// Token: 0x06009D87 RID: 40327 RVA: 0x003B7C30 File Offset: 0x003B5E30
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

	// Token: 0x04007745 RID: 30533
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, TValue> m_keyToValue = new Dictionary<TKey, TValue>();

	// Token: 0x04007746 RID: 30534
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TValue, TKey> m_valueToKey = new Dictionary<TValue, TKey>();
}
