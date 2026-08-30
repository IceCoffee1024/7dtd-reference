using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x0200145E RID: 5214
public sealed class OneToManyDictionary<TKey, TValue>
{
	// Token: 0x0600A396 RID: 41878 RVA: 0x003D1515 File Offset: 0x003CF715
	public OneToManyDictionary()
	{
		this.m_keyToValues = new Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>>();
		this.m_valueToKey = new Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>>();
	}

	// Token: 0x1700133C RID: 4924
	// (get) Token: 0x0600A397 RID: 41879 RVA: 0x003D1533 File Offset: 0x003CF733
	public IEnumerable<TKey> Keys
	{
		get
		{
			foreach (OneToManyDictionary<TKey, TValue>.Entry<TKey> entry in this.m_keyToValues.Keys)
			{
				yield return entry.Value;
			}
			Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>>.KeyCollection.Enumerator enumerator = default(Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}
	}

	// Token: 0x1700133D RID: 4925
	// (get) Token: 0x0600A398 RID: 41880 RVA: 0x003D1543 File Offset: 0x003CF743
	public IEnumerable<TValue> Values
	{
		get
		{
			foreach (OneToManyDictionary<TKey, TValue>.Entry<TValue> entry in this.m_valueToKey.Keys)
			{
				yield return entry.Value;
			}
			Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>>.KeyCollection.Enumerator enumerator = default(Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}
	}

	// Token: 0x0600A399 RID: 41881 RVA: 0x003D1554 File Offset: 0x003CF754
	public void Add(TKey key, TValue value)
	{
		OneToManyDictionary<TKey, TValue>.Entry<TKey> entry = new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key);
		OneToManyDictionary<TKey, TValue>.Entry<TValue> entry2 = new OneToManyDictionary<TKey, TValue>.Entry<TValue>(value);
		if (this.m_valueToKey.ContainsKey(entry2))
		{
			throw new ArgumentException("Value already in dictionary.", "value");
		}
		HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(entry, out hashSet))
		{
			this.m_keyToValues.Add(entry, hashSet = new HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>());
		}
		hashSet.Add(entry2);
		this.m_valueToKey.Add(entry2, entry);
	}

	// Token: 0x0600A39A RID: 41882 RVA: 0x003D15C5 File Offset: 0x003CF7C5
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValues.ContainsKey(new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A39B RID: 41883 RVA: 0x003D15D8 File Offset: 0x003CF7D8
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKey.ContainsKey(new OneToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x1700133E RID: 4926
	// (get) Token: 0x0600A39C RID: 41884 RVA: 0x003D15EB File Offset: 0x003CF7EB
	public int CountKeys
	{
		get
		{
			return this.m_keyToValues.Count;
		}
	}

	// Token: 0x1700133F RID: 4927
	// (get) Token: 0x0600A39D RID: 41885 RVA: 0x003D15F8 File Offset: 0x003CF7F8
	public int CountValues
	{
		get
		{
			return this.m_valueToKey.Count;
		}
	}

	// Token: 0x0600A39E RID: 41886 RVA: 0x003D1608 File Offset: 0x003CF808
	public bool TryGetByKey(TKey key, out IReadOnlyCollection<TValue> values)
	{
		HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet))
		{
			values = null;
			return false;
		}
		values = new OneToManyDictionary<TKey, TValue>.ValuesView(hashSet);
		return true;
	}

	// Token: 0x0600A39F RID: 41887 RVA: 0x003D1638 File Offset: 0x003CF838
	public int TryGetByKey(TKey key, ICollection<TValue> valuesOut)
	{
		HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet))
		{
			return 0;
		}
		int num = 0;
		foreach (OneToManyDictionary<TKey, TValue>.Entry<TValue> entry in hashSet)
		{
			valuesOut.Add(entry.Value);
			num++;
		}
		return num;
	}

	// Token: 0x0600A3A0 RID: 41888 RVA: 0x003D16AC File Offset: 0x003CF8AC
	public unsafe int TryGetByKey(TKey key, Span<TValue> valuesOut)
	{
		HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet))
		{
			return 0;
		}
		int num = 0;
		foreach (OneToManyDictionary<TKey, TValue>.Entry<TValue> entry in hashSet)
		{
			if (num >= valuesOut.Length)
			{
				break;
			}
			*valuesOut[num] = entry.Value;
			num++;
		}
		return num;
	}

	// Token: 0x0600A3A1 RID: 41889 RVA: 0x003D1730 File Offset: 0x003CF930
	public bool TryGetByValue(TValue value, out TKey key)
	{
		OneToManyDictionary<TKey, TValue>.Entry<TKey> entry;
		if (this.m_valueToKey.TryGetValue(new OneToManyDictionary<TKey, TValue>.Entry<TValue>(value), out entry))
		{
			key = entry.Value;
			return true;
		}
		key = default(TKey);
		return false;
	}

	// Token: 0x0600A3A2 RID: 41890 RVA: 0x003D1768 File Offset: 0x003CF968
	public bool RemoveByKey(TKey key)
	{
		OneToManyDictionary<TKey, TValue>.Entry<TKey> entry = new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key);
		HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(entry, out hashSet))
		{
			return false;
		}
		foreach (OneToManyDictionary<TKey, TValue>.Entry<TValue> entry2 in hashSet)
		{
			this.m_valueToKey.Remove(entry2);
		}
		this.m_keyToValues.Remove(entry);
		return true;
	}

	// Token: 0x0600A3A3 RID: 41891 RVA: 0x003D17E4 File Offset: 0x003CF9E4
	public bool RemoveByValue(TValue value)
	{
		OneToManyDictionary<TKey, TValue>.Entry<TValue> entry = new OneToManyDictionary<TKey, TValue>.Entry<TValue>(value);
		OneToManyDictionary<TKey, TValue>.Entry<TKey> entry2;
		if (!this.m_valueToKey.TryGetValue(entry, out entry2))
		{
			return false;
		}
		HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(entry2, out hashSet))
		{
			return false;
		}
		this.m_valueToKey.Remove(entry);
		hashSet.Remove(entry);
		if (hashSet.Count == 0)
		{
			this.m_keyToValues.Remove(entry2);
		}
		return true;
	}

	// Token: 0x0600A3A4 RID: 41892 RVA: 0x003D1847 File Offset: 0x003CFA47
	public void Clear()
	{
		this.m_keyToValues.Clear();
		this.m_valueToKey.Clear();
	}

	// Token: 0x04007A88 RID: 31368
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>> m_keyToValues;

	// Token: 0x04007A89 RID: 31369
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>> m_valueToKey;

	// Token: 0x0200145F RID: 5215
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A3A5 RID: 41893 RVA: 0x003D185F File Offset: 0x003CFA5F
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (OneToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A3A6 RID: 41894 RVA: 0x003D189E File Offset: 0x003CFA9E
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A3A7 RID: 41895 RVA: 0x003D18A8 File Offset: 0x003CFAA8
		public override bool Equals(object obj)
		{
			OneToManyDictionary<TKey, TValue>.Entry<T> entry = obj as OneToManyDictionary<TKey, TValue>.Entry<T>;
			if (entry == null)
			{
				return false;
			}
			if (OneToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType)
			{
				return this.Value == entry.Value;
			}
			return EqualityComparer<T>.Default.Equals(this.Value, entry.Value);
		}

		// Token: 0x04007A8A RID: 31370
		public readonly T Value;

		// Token: 0x04007A8B RID: 31371
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007A8C RID: 31372
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}

	// Token: 0x02001460 RID: 5216
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ValuesView : IReadOnlyCollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A3A9 RID: 41897 RVA: 0x003D1910 File Offset: 0x003CFB10
		public ValuesView(HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> set)
		{
			this.m_set = set;
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x0600A3AA RID: 41898 RVA: 0x003D191F File Offset: 0x003CFB1F
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A3AB RID: 41899 RVA: 0x003D192C File Offset: 0x003CFB2C
		public IEnumerator<TValue> GetEnumerator()
		{
			foreach (OneToManyDictionary<TKey, TValue>.Entry<TValue> entry in this.m_set)
			{
				yield return entry.Value;
			}
			HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>.Enumerator enumerator = default(HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600A3AC RID: 41900 RVA: 0x003D193B File Offset: 0x003CFB3B
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007A8D RID: 31373
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> m_set;
	}
}
