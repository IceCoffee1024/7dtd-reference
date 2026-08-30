using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013D4 RID: 5076
public sealed class DictionaryDebugWrapper<TKey, TValue> : CollectionDebugWrapper<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	// Token: 0x06009F7B RID: 40827 RVA: 0x003C123A File Offset: 0x003BF43A
	public DictionaryDebugWrapper()
		: this(new Dictionary<TKey, TValue>())
	{
	}

	// Token: 0x06009F7C RID: 40828 RVA: 0x003C1247 File Offset: 0x003BF447
	public DictionaryDebugWrapper(IDictionary<TKey, TValue> dictionary)
		: this(null, dictionary)
	{
	}

	// Token: 0x06009F7D RID: 40829 RVA: 0x003C1251 File Offset: 0x003BF451
	public DictionaryDebugWrapper(DebugWrapper parent, IDictionary<TKey, TValue> dictionary)
		: base(parent, dictionary)
	{
		this.m_dictionary = dictionary;
		this.Keys = new CollectionDebugWrapper<TKey>(this, this.m_dictionary.Keys);
		this.Values = new CollectionDebugWrapper<TValue>(this, this.m_dictionary.Values);
	}

	// Token: 0x06009F7E RID: 40830 RVA: 0x003C1290 File Offset: 0x003BF490
	public void Add(TKey key, TValue value)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_dictionary.Add(key, value);
		}
	}

	// Token: 0x06009F7F RID: 40831 RVA: 0x003C12D0 File Offset: 0x003BF4D0
	public bool ContainsKey(TKey key)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_dictionary.ContainsKey(key);
		}
		return flag;
	}

	// Token: 0x06009F80 RID: 40832 RVA: 0x003C1310 File Offset: 0x003BF510
	public bool Remove(TKey key)
	{
		bool flag;
		using (base.DebugReadWriteScope())
		{
			flag = this.m_dictionary.Remove(key);
		}
		return flag;
	}

	// Token: 0x06009F81 RID: 40833 RVA: 0x003C1350 File Offset: 0x003BF550
	public bool TryGetValue(TKey key, out TValue value)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_dictionary.TryGetValue(key, out value);
		}
		return flag;
	}

	// Token: 0x170012D5 RID: 4821
	public TValue this[TKey key]
	{
		get
		{
			TValue tvalue;
			using (base.DebugReadScope())
			{
				tvalue = this.m_dictionary[key];
			}
			return tvalue;
		}
		set
		{
			using (base.DebugReadWriteScope())
			{
				this.m_dictionary[key] = value;
			}
		}
	}

	// Token: 0x170012D6 RID: 4822
	// (get) Token: 0x06009F84 RID: 40836 RVA: 0x003C1410 File Offset: 0x003BF610
	public ICollection<TKey> Keys { get; }

	// Token: 0x170012D7 RID: 4823
	// (get) Token: 0x06009F85 RID: 40837 RVA: 0x003C1418 File Offset: 0x003BF618
	public ICollection<TValue> Values { get; }

	// Token: 0x040078D3 RID: 30931
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IDictionary<TKey, TValue> m_dictionary;
}
