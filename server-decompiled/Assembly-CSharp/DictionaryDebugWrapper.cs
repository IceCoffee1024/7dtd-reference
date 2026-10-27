using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001406 RID: 5126
public sealed class DictionaryDebugWrapper<TKey, TValue> : CollectionDebugWrapper<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	// Token: 0x0600A07E RID: 41086 RVA: 0x003C81DA File Offset: 0x003C63DA
	public DictionaryDebugWrapper()
		: this(new Dictionary<TKey, TValue>())
	{
	}

	// Token: 0x0600A07F RID: 41087 RVA: 0x003C81E7 File Offset: 0x003C63E7
	public DictionaryDebugWrapper(IDictionary<TKey, TValue> dictionary)
		: this(null, dictionary)
	{
	}

	// Token: 0x0600A080 RID: 41088 RVA: 0x003C81F1 File Offset: 0x003C63F1
	public DictionaryDebugWrapper(DebugWrapper parent, IDictionary<TKey, TValue> dictionary)
		: base(parent, dictionary)
	{
		this.m_dictionary = dictionary;
		this.Keys = new CollectionDebugWrapper<TKey>(this, this.m_dictionary.Keys);
		this.Values = new CollectionDebugWrapper<TValue>(this, this.m_dictionary.Values);
	}

	// Token: 0x0600A081 RID: 41089 RVA: 0x003C8230 File Offset: 0x003C6430
	public void Add(TKey key, TValue value)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_dictionary.Add(key, value);
		}
	}

	// Token: 0x0600A082 RID: 41090 RVA: 0x003C8270 File Offset: 0x003C6470
	public bool ContainsKey(TKey key)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_dictionary.ContainsKey(key);
		}
		return flag;
	}

	// Token: 0x0600A083 RID: 41091 RVA: 0x003C82B0 File Offset: 0x003C64B0
	public bool Remove(TKey key)
	{
		bool flag;
		using (base.DebugReadWriteScope())
		{
			flag = this.m_dictionary.Remove(key);
		}
		return flag;
	}

	// Token: 0x0600A084 RID: 41092 RVA: 0x003C82F0 File Offset: 0x003C64F0
	public bool TryGetValue(TKey key, out TValue value)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_dictionary.TryGetValue(key, out value);
		}
		return flag;
	}

	// Token: 0x17001326 RID: 4902
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

	// Token: 0x17001327 RID: 4903
	// (get) Token: 0x0600A087 RID: 41095 RVA: 0x003C83B0 File Offset: 0x003C65B0
	public ICollection<TKey> Keys { get; }

	// Token: 0x17001328 RID: 4904
	// (get) Token: 0x0600A088 RID: 41096 RVA: 0x003C83B8 File Offset: 0x003C65B8
	public ICollection<TValue> Values { get; }

	// Token: 0x0400798F RID: 31119
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IDictionary<TKey, TValue> m_dictionary;
}
