using System;
using System.Collections.Generic;

// Token: 0x020013C1 RID: 5057
public sealed class BiDictionary<TKey, TValue>
{
	// Token: 0x06009F29 RID: 40745 RVA: 0x003C1A40 File Offset: 0x003BFC40
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

	// Token: 0x06009F2A RID: 40746 RVA: 0x003C1AA3 File Offset: 0x003BFCA3
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValue.ContainsKey(key);
	}

	// Token: 0x06009F2B RID: 40747 RVA: 0x003C1AB1 File Offset: 0x003BFCB1
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKey.ContainsKey(value);
	}

	// Token: 0x06009F2C RID: 40748 RVA: 0x003C1ABF File Offset: 0x003BFCBF
	public bool TryGetByKey(TKey key, out TValue value)
	{
		return this.m_keyToValue.TryGetValue(key, out value);
	}

	// Token: 0x06009F2D RID: 40749 RVA: 0x003C1ACE File Offset: 0x003BFCCE
	public bool TryGetByValue(TValue value, out TKey key)
	{
		return this.m_valueToKey.TryGetValue(value, out key);
	}

	// Token: 0x06009F2E RID: 40750 RVA: 0x003C1AE0 File Offset: 0x003BFCE0
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

	// Token: 0x06009F2F RID: 40751 RVA: 0x003C1B10 File Offset: 0x003BFD10
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

	// Token: 0x04007899 RID: 30873
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, TValue> m_keyToValue = new Dictionary<TKey, TValue>();

	// Token: 0x0400789A RID: 30874
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TValue, TKey> m_valueToKey = new Dictionary<TValue, TKey>();
}
