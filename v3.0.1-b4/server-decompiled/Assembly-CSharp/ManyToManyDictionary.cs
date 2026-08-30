using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001418 RID: 5144
public sealed class ManyToManyDictionary<TKey, TValue>
{
	// Token: 0x0600A178 RID: 41336 RVA: 0x003C800F File Offset: 0x003C620F
	public ManyToManyDictionary()
	{
		this.m_keyToValues = new Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>>();
		this.m_valueToKeys = new Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>>();
	}

	// Token: 0x170012DB RID: 4827
	// (get) Token: 0x0600A179 RID: 41337 RVA: 0x003C802D File Offset: 0x003C622D
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

	// Token: 0x170012DC RID: 4828
	// (get) Token: 0x0600A17A RID: 41338 RVA: 0x003C803D File Offset: 0x003C623D
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

	// Token: 0x170012DD RID: 4829
	// (get) Token: 0x0600A17B RID: 41339 RVA: 0x003C804D File Offset: 0x003C624D
	public int CountKeys
	{
		get
		{
			return this.m_keyToValues.Count;
		}
	}

	// Token: 0x170012DE RID: 4830
	// (get) Token: 0x0600A17C RID: 41340 RVA: 0x003C805A File Offset: 0x003C625A
	public int CountValues
	{
		get
		{
			return this.m_valueToKeys.Count;
		}
	}

	// Token: 0x0600A17D RID: 41341 RVA: 0x003C8067 File Offset: 0x003C6267
	public void Clear()
	{
		this.m_keyToValues.Clear();
		this.m_valueToKeys.Clear();
	}

	// Token: 0x0600A17E RID: 41342 RVA: 0x003C8080 File Offset: 0x003C6280
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

	// Token: 0x0600A17F RID: 41343 RVA: 0x003C80F4 File Offset: 0x003C62F4
	public bool Contains(TKey key, TValue value)
	{
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		return this.m_keyToValues.TryGetValue(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet) && hashSet.Contains(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A180 RID: 41344 RVA: 0x003C8124 File Offset: 0x003C6324
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValues.ContainsKey(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A181 RID: 41345 RVA: 0x003C8137 File Offset: 0x003C6337
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKeys.ContainsKey(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A182 RID: 41346 RVA: 0x003C814C File Offset: 0x003C634C
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

	// Token: 0x0600A183 RID: 41347 RVA: 0x003C81C8 File Offset: 0x003C63C8
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

	// Token: 0x0600A184 RID: 41348 RVA: 0x003C8258 File Offset: 0x003C6458
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

	// Token: 0x0600A185 RID: 41349 RVA: 0x003C82E8 File Offset: 0x003C64E8
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

	// Token: 0x0600A186 RID: 41350 RVA: 0x003C8318 File Offset: 0x003C6518
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

	// Token: 0x04007920 RID: 31008
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>> m_keyToValues;

	// Token: 0x04007921 RID: 31009
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>> m_valueToKeys;

	// Token: 0x02001419 RID: 5145
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A187 RID: 41351 RVA: 0x003C8348 File Offset: 0x003C6548
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (ManyToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A188 RID: 41352 RVA: 0x003C8387 File Offset: 0x003C6587
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A189 RID: 41353 RVA: 0x003C8390 File Offset: 0x003C6590
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

		// Token: 0x04007922 RID: 31010
		public readonly T Value;

		// Token: 0x04007923 RID: 31011
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007924 RID: 31012
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}

	// Token: 0x0200141A RID: 5146
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ValuesView : IReadOnlyCollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A18B RID: 41355 RVA: 0x003C83F8 File Offset: 0x003C65F8
		public ValuesView(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> set)
		{
			this.m_set = set;
		}

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x0600A18C RID: 41356 RVA: 0x003C8407 File Offset: 0x003C6607
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A18D RID: 41357 RVA: 0x003C8414 File Offset: 0x003C6614
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

		// Token: 0x0600A18E RID: 41358 RVA: 0x003C8423 File Offset: 0x003C6623
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007925 RID: 31013
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> m_set;
	}

	// Token: 0x0200141C RID: 5148
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class KeysView : IReadOnlyCollection<TKey>, IEnumerable<TKey>, IEnumerable
	{
		// Token: 0x0600A196 RID: 41366 RVA: 0x003C8553 File Offset: 0x003C6753
		public KeysView(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> set)
		{
			this.m_set = set;
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x0600A197 RID: 41367 RVA: 0x003C8562 File Offset: 0x003C6762
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A198 RID: 41368 RVA: 0x003C856F File Offset: 0x003C676F
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

		// Token: 0x0600A199 RID: 41369 RVA: 0x003C857E File Offset: 0x003C677E
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400792A RID: 31018
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> m_set;
	}
}
