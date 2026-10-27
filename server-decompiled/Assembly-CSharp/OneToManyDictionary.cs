using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001490 RID: 5264
public sealed class OneToManyDictionary<TKey, TValue>
{
	// Token: 0x0600A49A RID: 42138 RVA: 0x003D8529 File Offset: 0x003D6729
	public OneToManyDictionary()
	{
		this.m_keyToValues = new Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>>();
		this.m_valueToKey = new Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>>();
	}

	// Token: 0x1700138D RID: 5005
	// (get) Token: 0x0600A49B RID: 42139 RVA: 0x003D8547 File Offset: 0x003D6747
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

	// Token: 0x1700138E RID: 5006
	// (get) Token: 0x0600A49C RID: 42140 RVA: 0x003D8557 File Offset: 0x003D6757
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

	// Token: 0x0600A49D RID: 42141 RVA: 0x003D8568 File Offset: 0x003D6768
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

	// Token: 0x0600A49E RID: 42142 RVA: 0x003D85D9 File Offset: 0x003D67D9
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValues.ContainsKey(new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A49F RID: 42143 RVA: 0x003D85EC File Offset: 0x003D67EC
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKey.ContainsKey(new OneToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x1700138F RID: 5007
	// (get) Token: 0x0600A4A0 RID: 42144 RVA: 0x003D85FF File Offset: 0x003D67FF
	public int CountKeys
	{
		get
		{
			return this.m_keyToValues.Count;
		}
	}

	// Token: 0x17001390 RID: 5008
	// (get) Token: 0x0600A4A1 RID: 42145 RVA: 0x003D860C File Offset: 0x003D680C
	public int CountValues
	{
		get
		{
			return this.m_valueToKey.Count;
		}
	}

	// Token: 0x0600A4A2 RID: 42146 RVA: 0x003D861C File Offset: 0x003D681C
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

	// Token: 0x0600A4A3 RID: 42147 RVA: 0x003D864C File Offset: 0x003D684C
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

	// Token: 0x0600A4A4 RID: 42148 RVA: 0x003D86C0 File Offset: 0x003D68C0
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

	// Token: 0x0600A4A5 RID: 42149 RVA: 0x003D8744 File Offset: 0x003D6944
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

	// Token: 0x0600A4A6 RID: 42150 RVA: 0x003D877C File Offset: 0x003D697C
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

	// Token: 0x0600A4A7 RID: 42151 RVA: 0x003D87F8 File Offset: 0x003D69F8
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

	// Token: 0x0600A4A8 RID: 42152 RVA: 0x003D885B File Offset: 0x003D6A5B
	public void Clear()
	{
		this.m_keyToValues.Clear();
		this.m_valueToKey.Clear();
	}

	// Token: 0x04007B44 RID: 31556
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>> m_keyToValues;

	// Token: 0x04007B45 RID: 31557
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>> m_valueToKey;

	// Token: 0x02001491 RID: 5265
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A4A9 RID: 42153 RVA: 0x003D8873 File Offset: 0x003D6A73
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (OneToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A4AA RID: 42154 RVA: 0x003D88B2 File Offset: 0x003D6AB2
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A4AB RID: 42155 RVA: 0x003D88BC File Offset: 0x003D6ABC
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

		// Token: 0x04007B46 RID: 31558
		public readonly T Value;

		// Token: 0x04007B47 RID: 31559
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007B48 RID: 31560
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}

	// Token: 0x02001492 RID: 5266
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ValuesView : IReadOnlyCollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A4AD RID: 42157 RVA: 0x003D8924 File Offset: 0x003D6B24
		public ValuesView(HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> set)
		{
			this.m_set = set;
		}

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x0600A4AE RID: 42158 RVA: 0x003D8933 File Offset: 0x003D6B33
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A4AF RID: 42159 RVA: 0x003D8940 File Offset: 0x003D6B40
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

		// Token: 0x0600A4B0 RID: 42160 RVA: 0x003D894F File Offset: 0x003D6B4F
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007B49 RID: 31561
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> m_set;
	}
}
