using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001443 RID: 5187
public sealed class OneToManyDictionary<TKey, TValue>
{
	// Token: 0x0600A299 RID: 41625 RVA: 0x003CC1D1 File Offset: 0x003CA3D1
	public OneToManyDictionary()
	{
		this.m_keyToValues = new Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>>();
		this.m_valueToKey = new Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>>();
	}

	// Token: 0x17001303 RID: 4867
	// (get) Token: 0x0600A29A RID: 41626 RVA: 0x003CC1EF File Offset: 0x003CA3EF
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

	// Token: 0x17001304 RID: 4868
	// (get) Token: 0x0600A29B RID: 41627 RVA: 0x003CC1FF File Offset: 0x003CA3FF
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

	// Token: 0x0600A29C RID: 41628 RVA: 0x003CC210 File Offset: 0x003CA410
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

	// Token: 0x0600A29D RID: 41629 RVA: 0x003CC281 File Offset: 0x003CA481
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValues.ContainsKey(new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A29E RID: 41630 RVA: 0x003CC294 File Offset: 0x003CA494
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKey.ContainsKey(new OneToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x17001305 RID: 4869
	// (get) Token: 0x0600A29F RID: 41631 RVA: 0x003CC2A7 File Offset: 0x003CA4A7
	public int CountKeys
	{
		get
		{
			return this.m_keyToValues.Count;
		}
	}

	// Token: 0x17001306 RID: 4870
	// (get) Token: 0x0600A2A0 RID: 41632 RVA: 0x003CC2B4 File Offset: 0x003CA4B4
	public int CountValues
	{
		get
		{
			return this.m_valueToKey.Count;
		}
	}

	// Token: 0x0600A2A1 RID: 41633 RVA: 0x003CC2C4 File Offset: 0x003CA4C4
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

	// Token: 0x0600A2A2 RID: 41634 RVA: 0x003CC2F4 File Offset: 0x003CA4F4
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

	// Token: 0x0600A2A3 RID: 41635 RVA: 0x003CC368 File Offset: 0x003CA568
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

	// Token: 0x0600A2A4 RID: 41636 RVA: 0x003CC3EC File Offset: 0x003CA5EC
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

	// Token: 0x0600A2A5 RID: 41637 RVA: 0x003CC424 File Offset: 0x003CA624
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

	// Token: 0x0600A2A6 RID: 41638 RVA: 0x003CC4A0 File Offset: 0x003CA6A0
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

	// Token: 0x0600A2A7 RID: 41639 RVA: 0x003CC503 File Offset: 0x003CA703
	public void Clear()
	{
		this.m_keyToValues.Clear();
		this.m_valueToKey.Clear();
	}

	// Token: 0x040079AF RID: 31151
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>> m_keyToValues;

	// Token: 0x040079B0 RID: 31152
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>> m_valueToKey;

	// Token: 0x02001444 RID: 5188
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A2A8 RID: 41640 RVA: 0x003CC51B File Offset: 0x003CA71B
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (OneToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A2A9 RID: 41641 RVA: 0x003CC55A File Offset: 0x003CA75A
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A2AA RID: 41642 RVA: 0x003CC564 File Offset: 0x003CA764
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

		// Token: 0x040079B1 RID: 31153
		public readonly T Value;

		// Token: 0x040079B2 RID: 31154
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x040079B3 RID: 31155
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}

	// Token: 0x02001445 RID: 5189
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ValuesView : IReadOnlyCollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A2AC RID: 41644 RVA: 0x003CC5CC File Offset: 0x003CA7CC
		public ValuesView(HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> set)
		{
			this.m_set = set;
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x0600A2AD RID: 41645 RVA: 0x003CC5DB File Offset: 0x003CA7DB
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A2AE RID: 41646 RVA: 0x003CC5E8 File Offset: 0x003CA7E8
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

		// Token: 0x0600A2AF RID: 41647 RVA: 0x003CC5F7 File Offset: 0x003CA7F7
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040079B4 RID: 31156
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> m_set;
	}
}
