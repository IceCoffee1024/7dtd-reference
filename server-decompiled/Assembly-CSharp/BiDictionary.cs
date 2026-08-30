using System;
using System.Collections.Generic;

// Token: 0x020013A1 RID: 5025
public sealed class BiDictionary<TKey, TValue>
{
	// Token: 0x06009E7E RID: 40574 RVA: 0x003BCEA4 File Offset: 0x003BB0A4
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

	// Token: 0x06009E7F RID: 40575 RVA: 0x003BCF07 File Offset: 0x003BB107
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValue.ContainsKey(key);
	}

	// Token: 0x06009E80 RID: 40576 RVA: 0x003BCF15 File Offset: 0x003BB115
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKey.ContainsKey(value);
	}

	// Token: 0x06009E81 RID: 40577 RVA: 0x003BCF23 File Offset: 0x003BB123
	public bool TryGetByKey(TKey key, out TValue value)
	{
		return this.m_keyToValue.TryGetValue(key, out value);
	}

	// Token: 0x06009E82 RID: 40578 RVA: 0x003BCF32 File Offset: 0x003BB132
	public bool TryGetByValue(TValue value, out TKey key)
	{
		return this.m_valueToKey.TryGetValue(value, out key);
	}

	// Token: 0x06009E83 RID: 40579 RVA: 0x003BCF44 File Offset: 0x003BB144
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

	// Token: 0x06009E84 RID: 40580 RVA: 0x003BCF74 File Offset: 0x003BB174
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

	// Token: 0x0400781E RID: 30750
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, TValue> m_keyToValue = new Dictionary<TKey, TValue>();

	// Token: 0x0400781F RID: 30751
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TValue, TKey> m_valueToKey = new Dictionary<TValue, TKey>();
}
