using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013F4 RID: 5108
public sealed class DictionaryDebugWrapper<TKey, TValue> : CollectionDebugWrapper<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	// Token: 0x0600A026 RID: 40998 RVA: 0x003C5DD6 File Offset: 0x003C3FD6
	public DictionaryDebugWrapper()
		: this(new Dictionary<TKey, TValue>())
	{
	}

	// Token: 0x0600A027 RID: 40999 RVA: 0x003C5DE3 File Offset: 0x003C3FE3
	public DictionaryDebugWrapper(IDictionary<TKey, TValue> dictionary)
		: this(null, dictionary)
	{
	}

	// Token: 0x0600A028 RID: 41000 RVA: 0x003C5DED File Offset: 0x003C3FED
	public DictionaryDebugWrapper(DebugWrapper parent, IDictionary<TKey, TValue> dictionary)
		: base(parent, dictionary)
	{
		this.m_dictionary = dictionary;
		this.Keys = new CollectionDebugWrapper<TKey>(this, this.m_dictionary.Keys);
		this.Values = new CollectionDebugWrapper<TValue>(this, this.m_dictionary.Values);
	}

	// Token: 0x0600A029 RID: 41001 RVA: 0x003C5E2C File Offset: 0x003C402C
	public void Add(TKey key, TValue value)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_dictionary.Add(key, value);
		}
	}

	// Token: 0x0600A02A RID: 41002 RVA: 0x003C5E6C File Offset: 0x003C406C
	public bool ContainsKey(TKey key)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_dictionary.ContainsKey(key);
		}
		return flag;
	}

	// Token: 0x0600A02B RID: 41003 RVA: 0x003C5EAC File Offset: 0x003C40AC
	public bool Remove(TKey key)
	{
		bool flag;
		using (base.DebugReadWriteScope())
		{
			flag = this.m_dictionary.Remove(key);
		}
		return flag;
	}

	// Token: 0x0600A02C RID: 41004 RVA: 0x003C5EEC File Offset: 0x003C40EC
	public bool TryGetValue(TKey key, out TValue value)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_dictionary.TryGetValue(key, out value);
		}
		return flag;
	}

	// Token: 0x1700131A RID: 4890
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

	// Token: 0x1700131B RID: 4891
	// (get) Token: 0x0600A02F RID: 41007 RVA: 0x003C5FAC File Offset: 0x003C41AC
	public ICollection<TKey> Keys { get; }

	// Token: 0x1700131C RID: 4892
	// (get) Token: 0x0600A030 RID: 41008 RVA: 0x003C5FB4 File Offset: 0x003C41B4
	public ICollection<TValue> Values { get; }

	// Token: 0x0400794E RID: 31054
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IDictionary<TKey, TValue> m_dictionary;
}
