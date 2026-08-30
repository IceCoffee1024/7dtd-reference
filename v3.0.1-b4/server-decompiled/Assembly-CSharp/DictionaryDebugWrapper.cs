using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013B9 RID: 5049
public sealed class DictionaryDebugWrapper<TKey, TValue> : CollectionDebugWrapper<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	// Token: 0x06009E7E RID: 40574 RVA: 0x003BBEF6 File Offset: 0x003BA0F6
	public DictionaryDebugWrapper()
		: this(new Dictionary<TKey, TValue>())
	{
	}

	// Token: 0x06009E7F RID: 40575 RVA: 0x003BBF03 File Offset: 0x003BA103
	public DictionaryDebugWrapper(IDictionary<TKey, TValue> dictionary)
		: this(null, dictionary)
	{
	}

	// Token: 0x06009E80 RID: 40576 RVA: 0x003BBF0D File Offset: 0x003BA10D
	public DictionaryDebugWrapper(DebugWrapper parent, IDictionary<TKey, TValue> dictionary)
		: base(parent, dictionary)
	{
		this.m_dictionary = dictionary;
		this.Keys = new CollectionDebugWrapper<TKey>(this, this.m_dictionary.Keys);
		this.Values = new CollectionDebugWrapper<TValue>(this, this.m_dictionary.Values);
	}

	// Token: 0x06009E81 RID: 40577 RVA: 0x003BBF4C File Offset: 0x003BA14C
	public void Add(TKey key, TValue value)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_dictionary.Add(key, value);
		}
	}

	// Token: 0x06009E82 RID: 40578 RVA: 0x003BBF8C File Offset: 0x003BA18C
	public bool ContainsKey(TKey key)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_dictionary.ContainsKey(key);
		}
		return flag;
	}

	// Token: 0x06009E83 RID: 40579 RVA: 0x003BBFCC File Offset: 0x003BA1CC
	public bool Remove(TKey key)
	{
		bool flag;
		using (base.DebugReadWriteScope())
		{
			flag = this.m_dictionary.Remove(key);
		}
		return flag;
	}

	// Token: 0x06009E84 RID: 40580 RVA: 0x003BC00C File Offset: 0x003BA20C
	public bool TryGetValue(TKey key, out TValue value)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_dictionary.TryGetValue(key, out value);
		}
		return flag;
	}

	// Token: 0x1700129C RID: 4764
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

	// Token: 0x1700129D RID: 4765
	// (get) Token: 0x06009E87 RID: 40583 RVA: 0x003BC0CC File Offset: 0x003BA2CC
	public ICollection<TKey> Keys { get; }

	// Token: 0x1700129E RID: 4766
	// (get) Token: 0x06009E88 RID: 40584 RVA: 0x003BC0D4 File Offset: 0x003BA2D4
	public ICollection<TValue> Values { get; }

	// Token: 0x040077FA RID: 30714
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IDictionary<TKey, TValue> m_dictionary;
}
