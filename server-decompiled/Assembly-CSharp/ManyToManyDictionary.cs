using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001453 RID: 5203
public sealed class ManyToManyDictionary<TKey, TValue>
{
	// Token: 0x0600A320 RID: 41760 RVA: 0x003D1EEF File Offset: 0x003D00EF
	public ManyToManyDictionary()
	{
		this.m_keyToValues = new Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>>();
		this.m_valueToKeys = new Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>>();
	}

	// Token: 0x17001359 RID: 4953
	// (get) Token: 0x0600A321 RID: 41761 RVA: 0x003D1F0D File Offset: 0x003D010D
	public IEnumerable<TKey> Keys
	{
		get
		{
			foreach (ManyToManyDictionary<TKey, TValue>.Entry<TKey> entry in this.m_keyToValues.Keys)
			{
				yield return entry.Value;
			}
			Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>>.KeyCollection.Enumerator enumerator = default(Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}
	}

	// Token: 0x1700135A RID: 4954
	// (get) Token: 0x0600A322 RID: 41762 RVA: 0x003D1F1D File Offset: 0x003D011D
	public IEnumerable<TValue> Values
	{
		get
		{
			foreach (ManyToManyDictionary<TKey, TValue>.Entry<TValue> entry in this.m_valueToKeys.Keys)
			{
				yield return entry.Value;
			}
			Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>>.KeyCollection.Enumerator enumerator = default(Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}
	}

	// Token: 0x1700135B RID: 4955
	// (get) Token: 0x0600A323 RID: 41763 RVA: 0x003D1F2D File Offset: 0x003D012D
	public int CountKeys
	{
		get
		{
			return this.m_keyToValues.Count;
		}
	}

	// Token: 0x1700135C RID: 4956
	// (get) Token: 0x0600A324 RID: 41764 RVA: 0x003D1F3A File Offset: 0x003D013A
	public int CountValues
	{
		get
		{
			return this.m_valueToKeys.Count;
		}
	}

	// Token: 0x0600A325 RID: 41765 RVA: 0x003D1F47 File Offset: 0x003D0147
	public void Clear()
	{
		this.m_keyToValues.Clear();
		this.m_valueToKeys.Clear();
	}

	// Token: 0x0600A326 RID: 41766 RVA: 0x003D1F60 File Offset: 0x003D0160
	public void Add(TKey key, TValue value)
	{
		ManyToManyDictionary<TKey, TValue>.Entry<TKey> entry = new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key);
		ManyToManyDictionary<TKey, TValue>.Entry<TValue> entry2 = new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value);
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(entry, out hashSet))
		{
			this.m_keyToValues.Add(entry, hashSet = new HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>());
		}
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> hashSet2;
		if (!this.m_valueToKeys.TryGetValue(entry2, out hashSet2))
		{
			this.m_valueToKeys.Add(entry2, hashSet2 = new HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>());
		}
		hashSet.Add(entry2);
		hashSet2.Add(entry);
	}

	// Token: 0x0600A327 RID: 41767 RVA: 0x003D1FD4 File Offset: 0x003D01D4
	public bool Contains(TKey key, TValue value)
	{
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		return this.m_keyToValues.TryGetValue(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet) && hashSet.Contains(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A328 RID: 41768 RVA: 0x003D2004 File Offset: 0x003D0204
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValues.ContainsKey(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A329 RID: 41769 RVA: 0x003D2017 File Offset: 0x003D0217
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKeys.ContainsKey(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A32A RID: 41770 RVA: 0x003D202C File Offset: 0x003D022C
	public bool Remove(TKey key, TValue value)
	{
		ManyToManyDictionary<TKey, TValue>.Entry<TKey> entry = new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key);
		ManyToManyDictionary<TKey, TValue>.Entry<TValue> entry2 = new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value);
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(entry, out hashSet))
		{
			return false;
		}
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> hashSet2;
		if (!this.m_valueToKeys.TryGetValue(entry2, out hashSet2))
		{
			return false;
		}
		bool flag = hashSet.Remove(entry2);
		bool flag2 = hashSet2.Remove(entry);
		if (hashSet.Count == 0)
		{
			this.m_keyToValues.Remove(entry);
		}
		if (hashSet2.Count == 0)
		{
			this.m_valueToKeys.Remove(entry2);
		}
		return flag && flag2;
	}

	// Token: 0x0600A32B RID: 41771 RVA: 0x003D20A8 File Offset: 0x003D02A8
	public bool RemoveByKey(TKey key)
	{
		ManyToManyDictionary<TKey, TValue>.Entry<TKey> entry = new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key);
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.Remove(entry, out hashSet))
		{
			return false;
		}
		foreach (ManyToManyDictionary<TKey, TValue>.Entry<TValue> entry2 in hashSet)
		{
			HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> hashSet2;
			if (this.m_valueToKeys.TryGetValue(entry2, out hashSet2))
			{
				hashSet2.Remove(entry);
				if (hashSet2.Count == 0)
				{
					this.m_valueToKeys.Remove(entry2);
				}
			}
		}
		return true;
	}

	// Token: 0x0600A32C RID: 41772 RVA: 0x003D2138 File Offset: 0x003D0338
	public bool RemoveByValue(TValue value)
	{
		ManyToManyDictionary<TKey, TValue>.Entry<TValue> entry = new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value);
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> hashSet;
		if (!this.m_valueToKeys.Remove(entry, out hashSet))
		{
			return false;
		}
		foreach (ManyToManyDictionary<TKey, TValue>.Entry<TKey> entry2 in hashSet)
		{
			HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet2;
			if (this.m_keyToValues.TryGetValue(entry2, out hashSet2))
			{
				hashSet2.Remove(entry);
				if (hashSet2.Count == 0)
				{
					this.m_keyToValues.Remove(entry2);
				}
			}
		}
		return true;
	}

	// Token: 0x0600A32D RID: 41773 RVA: 0x003D21C8 File Offset: 0x003D03C8
	public bool TryGetByKey(TKey key, out IReadOnlyCollection<TValue> values)
	{
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet))
		{
			values = null;
			return false;
		}
		values = new ManyToManyDictionary<TKey, TValue>.ValuesView(hashSet);
		return true;
	}

	// Token: 0x0600A32E RID: 41774 RVA: 0x003D21F8 File Offset: 0x003D03F8
	public int TryGetByKey(TKey key, ICollection<TValue> valuesOut)
	{
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		if (!this.m_keyToValues.TryGetValue(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet))
		{
			return 0;
		}
		int num = 0;
		foreach (ManyToManyDictionary<TKey, TValue>.Entry<TValue> entry in hashSet)
		{
			valuesOut.Add(entry.Value);
			num++;
		}
		return num;
	}

	// Token: 0x0600A32F RID: 41775 RVA: 0x003D226C File Offset: 0x003D046C
	public bool TryGetByValue(TValue value, out IReadOnlyCollection<TKey> keys)
	{
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> hashSet;
		if (!this.m_valueToKeys.TryGetValue(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value), out hashSet))
		{
			keys = null;
			return false;
		}
		keys = new ManyToManyDictionary<TKey, TValue>.KeysView(hashSet);
		return true;
	}

	// Token: 0x04007A74 RID: 31348
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>> m_keyToValues;

	// Token: 0x04007A75 RID: 31349
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>> m_valueToKeys;

	// Token: 0x02001454 RID: 5204
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A330 RID: 41776 RVA: 0x003D229C File Offset: 0x003D049C
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (ManyToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A331 RID: 41777 RVA: 0x003D22DB File Offset: 0x003D04DB
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A332 RID: 41778 RVA: 0x003D22E4 File Offset: 0x003D04E4
		public override bool Equals(object obj)
		{
			ManyToManyDictionary<TKey, TValue>.Entry<T> entry = obj as ManyToManyDictionary<TKey, TValue>.Entry<T>;
			if (entry == null)
			{
				return false;
			}
			if (ManyToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType)
			{
				return this.Value == entry.Value;
			}
			return EqualityComparer<T>.Default.Equals(this.Value, entry.Value);
		}

		// Token: 0x04007A76 RID: 31350
		public readonly T Value;

		// Token: 0x04007A77 RID: 31351
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007A78 RID: 31352
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}

	// Token: 0x02001455 RID: 5205
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ValuesView : IReadOnlyCollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A334 RID: 41780 RVA: 0x003D234C File Offset: 0x003D054C
		public ValuesView(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> set)
		{
			this.m_set = set;
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x0600A335 RID: 41781 RVA: 0x003D235B File Offset: 0x003D055B
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A336 RID: 41782 RVA: 0x003D2368 File Offset: 0x003D0568
		public IEnumerator<TValue> GetEnumerator()
		{
			foreach (ManyToManyDictionary<TKey, TValue>.Entry<TValue> entry in this.m_set)
			{
				yield return entry.Value;
			}
			HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>.Enumerator enumerator = default(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600A337 RID: 41783 RVA: 0x003D2377 File Offset: 0x003D0577
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007A79 RID: 31353
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> m_set;
	}

	// Token: 0x02001457 RID: 5207
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class KeysView : IReadOnlyCollection<TKey>, IEnumerable<TKey>, IEnumerable
	{
		// Token: 0x0600A33F RID: 41791 RVA: 0x003D24A7 File Offset: 0x003D06A7
		public KeysView(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> set)
		{
			this.m_set = set;
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x0600A340 RID: 41792 RVA: 0x003D24B6 File Offset: 0x003D06B6
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A341 RID: 41793 RVA: 0x003D24C3 File Offset: 0x003D06C3
		public IEnumerator<TKey> GetEnumerator()
		{
			foreach (ManyToManyDictionary<TKey, TValue>.Entry<TKey> entry in this.m_set)
			{
				yield return entry.Value;
			}
			HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>.Enumerator enumerator = default(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600A342 RID: 41794 RVA: 0x003D24D2 File Offset: 0x003D06D2
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007A7E RID: 31358
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> m_set;
	}
}
