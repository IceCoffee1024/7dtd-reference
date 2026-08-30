using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001433 RID: 5171
public sealed class ManyToManyDictionary<TKey, TValue>
{
	// Token: 0x0600A275 RID: 41589 RVA: 0x003CD353 File Offset: 0x003CB553
	public ManyToManyDictionary()
	{
		this.m_keyToValues = new Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>>();
		this.m_valueToKeys = new Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>>();
	}

	// Token: 0x17001314 RID: 4884
	// (get) Token: 0x0600A276 RID: 41590 RVA: 0x003CD371 File Offset: 0x003CB571
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

	// Token: 0x17001315 RID: 4885
	// (get) Token: 0x0600A277 RID: 41591 RVA: 0x003CD381 File Offset: 0x003CB581
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

	// Token: 0x17001316 RID: 4886
	// (get) Token: 0x0600A278 RID: 41592 RVA: 0x003CD391 File Offset: 0x003CB591
	public int CountKeys
	{
		get
		{
			return this.m_keyToValues.Count;
		}
	}

	// Token: 0x17001317 RID: 4887
	// (get) Token: 0x0600A279 RID: 41593 RVA: 0x003CD39E File Offset: 0x003CB59E
	public int CountValues
	{
		get
		{
			return this.m_valueToKeys.Count;
		}
	}

	// Token: 0x0600A27A RID: 41594 RVA: 0x003CD3AB File Offset: 0x003CB5AB
	public void Clear()
	{
		this.m_keyToValues.Clear();
		this.m_valueToKeys.Clear();
	}

	// Token: 0x0600A27B RID: 41595 RVA: 0x003CD3C4 File Offset: 0x003CB5C4
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

	// Token: 0x0600A27C RID: 41596 RVA: 0x003CD438 File Offset: 0x003CB638
	public bool Contains(TKey key, TValue value)
	{
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		return this.m_keyToValues.TryGetValue(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet) && hashSet.Contains(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A27D RID: 41597 RVA: 0x003CD468 File Offset: 0x003CB668
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValues.ContainsKey(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A27E RID: 41598 RVA: 0x003CD47B File Offset: 0x003CB67B
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKeys.ContainsKey(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A27F RID: 41599 RVA: 0x003CD490 File Offset: 0x003CB690
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

	// Token: 0x0600A280 RID: 41600 RVA: 0x003CD50C File Offset: 0x003CB70C
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

	// Token: 0x0600A281 RID: 41601 RVA: 0x003CD59C File Offset: 0x003CB79C
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

	// Token: 0x0600A282 RID: 41602 RVA: 0x003CD62C File Offset: 0x003CB82C
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

	// Token: 0x0600A283 RID: 41603 RVA: 0x003CD65C File Offset: 0x003CB85C
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

	// Token: 0x040079F9 RID: 31225
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>> m_keyToValues;

	// Token: 0x040079FA RID: 31226
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>> m_valueToKeys;

	// Token: 0x02001434 RID: 5172
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A284 RID: 41604 RVA: 0x003CD68C File Offset: 0x003CB88C
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (ManyToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A285 RID: 41605 RVA: 0x003CD6CB File Offset: 0x003CB8CB
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A286 RID: 41606 RVA: 0x003CD6D4 File Offset: 0x003CB8D4
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

		// Token: 0x040079FB RID: 31227
		public readonly T Value;

		// Token: 0x040079FC RID: 31228
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x040079FD RID: 31229
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}

	// Token: 0x02001435 RID: 5173
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ValuesView : IReadOnlyCollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A288 RID: 41608 RVA: 0x003CD73C File Offset: 0x003CB93C
		public ValuesView(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> set)
		{
			this.m_set = set;
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x0600A289 RID: 41609 RVA: 0x003CD74B File Offset: 0x003CB94B
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A28A RID: 41610 RVA: 0x003CD758 File Offset: 0x003CB958
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

		// Token: 0x0600A28B RID: 41611 RVA: 0x003CD767 File Offset: 0x003CB967
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040079FE RID: 31230
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> m_set;
	}

	// Token: 0x02001437 RID: 5175
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class KeysView : IReadOnlyCollection<TKey>, IEnumerable<TKey>, IEnumerable
	{
		// Token: 0x0600A293 RID: 41619 RVA: 0x003CD897 File Offset: 0x003CBA97
		public KeysView(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> set)
		{
			this.m_set = set;
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x0600A294 RID: 41620 RVA: 0x003CD8A6 File Offset: 0x003CBAA6
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A295 RID: 41621 RVA: 0x003CD8B3 File Offset: 0x003CBAB3
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

		// Token: 0x0600A296 RID: 41622 RVA: 0x003CD8C2 File Offset: 0x003CBAC2
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007A03 RID: 31235
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> m_set;
	}
}
