using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x0200147E RID: 5246
public sealed class OneToManyDictionary<TKey, TValue>
{
	// Token: 0x0600A442 RID: 42050 RVA: 0x003D6125 File Offset: 0x003D4325
	public OneToManyDictionary()
	{
		this.m_keyToValues = new Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>>();
		this.m_valueToKey = new Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>>();
	}

	// Token: 0x17001381 RID: 4993
	// (get) Token: 0x0600A443 RID: 42051 RVA: 0x003D6143 File Offset: 0x003D4343
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

	// Token: 0x17001382 RID: 4994
	// (get) Token: 0x0600A444 RID: 42052 RVA: 0x003D6153 File Offset: 0x003D4353
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

	// Token: 0x0600A445 RID: 42053 RVA: 0x003D6164 File Offset: 0x003D4364
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

	// Token: 0x0600A446 RID: 42054 RVA: 0x003D61D5 File Offset: 0x003D43D5
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValues.ContainsKey(new OneToManyDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A447 RID: 42055 RVA: 0x003D61E8 File Offset: 0x003D43E8
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKey.ContainsKey(new OneToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x17001383 RID: 4995
	// (get) Token: 0x0600A448 RID: 42056 RVA: 0x003D61FB File Offset: 0x003D43FB
	public int CountKeys
	{
		get
		{
			return this.m_keyToValues.Count;
		}
	}

	// Token: 0x17001384 RID: 4996
	// (get) Token: 0x0600A449 RID: 42057 RVA: 0x003D6208 File Offset: 0x003D4408
	public int CountValues
	{
		get
		{
			return this.m_valueToKey.Count;
		}
	}

	// Token: 0x0600A44A RID: 42058 RVA: 0x003D6218 File Offset: 0x003D4418
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

	// Token: 0x0600A44B RID: 42059 RVA: 0x003D6248 File Offset: 0x003D4448
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

	// Token: 0x0600A44C RID: 42060 RVA: 0x003D62BC File Offset: 0x003D44BC
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

	// Token: 0x0600A44D RID: 42061 RVA: 0x003D6340 File Offset: 0x003D4540
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

	// Token: 0x0600A44E RID: 42062 RVA: 0x003D6378 File Offset: 0x003D4578
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

	// Token: 0x0600A44F RID: 42063 RVA: 0x003D63F4 File Offset: 0x003D45F4
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

	// Token: 0x0600A450 RID: 42064 RVA: 0x003D6457 File Offset: 0x003D4657
	public void Clear()
	{
		this.m_keyToValues.Clear();
		this.m_valueToKey.Clear();
	}

	// Token: 0x04007B03 RID: 31491
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>>> m_keyToValues;

	// Token: 0x04007B04 RID: 31492
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToManyDictionary<TKey, TValue>.Entry<TValue>, OneToManyDictionary<TKey, TValue>.Entry<TKey>> m_valueToKey;

	// Token: 0x0200147F RID: 5247
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A451 RID: 42065 RVA: 0x003D646F File Offset: 0x003D466F
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (OneToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A452 RID: 42066 RVA: 0x003D64AE File Offset: 0x003D46AE
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A453 RID: 42067 RVA: 0x003D64B8 File Offset: 0x003D46B8
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

		// Token: 0x04007B05 RID: 31493
		public readonly T Value;

		// Token: 0x04007B06 RID: 31494
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007B07 RID: 31495
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}

	// Token: 0x02001480 RID: 5248
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ValuesView : IReadOnlyCollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A455 RID: 42069 RVA: 0x003D6520 File Offset: 0x003D4720
		public ValuesView(HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> set)
		{
			this.m_set = set;
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x0600A456 RID: 42070 RVA: 0x003D652F File Offset: 0x003D472F
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A457 RID: 42071 RVA: 0x003D653C File Offset: 0x003D473C
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

		// Token: 0x0600A458 RID: 42072 RVA: 0x003D654B File Offset: 0x003D474B
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007B08 RID: 31496
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<OneToManyDictionary<TKey, TValue>.Entry<TValue>> m_set;
	}
}
