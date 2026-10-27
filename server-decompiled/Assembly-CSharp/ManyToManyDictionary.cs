using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001465 RID: 5221
public sealed class ManyToManyDictionary<TKey, TValue>
{
	// Token: 0x0600A378 RID: 41848 RVA: 0x003D42F3 File Offset: 0x003D24F3
	public ManyToManyDictionary()
	{
		this.m_keyToValues = new Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>>();
		this.m_valueToKeys = new Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>>();
	}

	// Token: 0x17001365 RID: 4965
	// (get) Token: 0x0600A379 RID: 41849 RVA: 0x003D4311 File Offset: 0x003D2511
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

	// Token: 0x17001366 RID: 4966
	// (get) Token: 0x0600A37A RID: 41850 RVA: 0x003D4321 File Offset: 0x003D2521
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

	// Token: 0x17001367 RID: 4967
	// (get) Token: 0x0600A37B RID: 41851 RVA: 0x003D4331 File Offset: 0x003D2531
	public int CountKeys
	{
		get
		{
			return this.m_keyToValues.Count;
		}
	}

	// Token: 0x17001368 RID: 4968
	// (get) Token: 0x0600A37C RID: 41852 RVA: 0x003D433E File Offset: 0x003D253E
	public int CountValues
	{
		get
		{
			return this.m_valueToKeys.Count;
		}
	}

	// Token: 0x0600A37D RID: 41853 RVA: 0x003D434B File Offset: 0x003D254B
	public void Clear()
	{
		this.m_keyToValues.Clear();
		this.m_valueToKeys.Clear();
	}

	// Token: 0x0600A37E RID: 41854 RVA: 0x003D4364 File Offset: 0x003D2564
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

	// Token: 0x0600A37F RID: 41855 RVA: 0x003D43D8 File Offset: 0x003D25D8
	public bool Contains(TKey key, TValue value)
	{
		HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> hashSet;
		return this.m_keyToValues.TryGetValue(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key), out hashSet) && hashSet.Contains(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A380 RID: 41856 RVA: 0x003D4408 File Offset: 0x003D2608
	public bool ContainsKey(TKey key)
	{
		return this.m_keyToValues.ContainsKey(new ManyToManyDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A381 RID: 41857 RVA: 0x003D441B File Offset: 0x003D261B
	public bool ContainsValue(TValue value)
	{
		return this.m_valueToKeys.ContainsKey(new ManyToManyDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A382 RID: 41858 RVA: 0x003D4430 File Offset: 0x003D2630
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

	// Token: 0x0600A383 RID: 41859 RVA: 0x003D44AC File Offset: 0x003D26AC
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

	// Token: 0x0600A384 RID: 41860 RVA: 0x003D453C File Offset: 0x003D273C
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

	// Token: 0x0600A385 RID: 41861 RVA: 0x003D45CC File Offset: 0x003D27CC
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

	// Token: 0x0600A386 RID: 41862 RVA: 0x003D45FC File Offset: 0x003D27FC
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

	// Token: 0x0600A387 RID: 41863 RVA: 0x003D4670 File Offset: 0x003D2870
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

	// Token: 0x04007AB5 RID: 31413
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TKey>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>>> m_keyToValues;

	// Token: 0x04007AB6 RID: 31414
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ManyToManyDictionary<TKey, TValue>.Entry<TValue>, HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>>> m_valueToKeys;

	// Token: 0x02001466 RID: 5222
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A388 RID: 41864 RVA: 0x003D46A0 File Offset: 0x003D28A0
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (ManyToManyDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A389 RID: 41865 RVA: 0x003D46DF File Offset: 0x003D28DF
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A38A RID: 41866 RVA: 0x003D46E8 File Offset: 0x003D28E8
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

		// Token: 0x04007AB7 RID: 31415
		public readonly T Value;

		// Token: 0x04007AB8 RID: 31416
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007AB9 RID: 31417
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}

	// Token: 0x02001467 RID: 5223
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ValuesView : IReadOnlyCollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A38C RID: 41868 RVA: 0x003D4750 File Offset: 0x003D2950
		public ValuesView(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> set)
		{
			this.m_set = set;
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x0600A38D RID: 41869 RVA: 0x003D475F File Offset: 0x003D295F
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A38E RID: 41870 RVA: 0x003D476C File Offset: 0x003D296C
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

		// Token: 0x0600A38F RID: 41871 RVA: 0x003D477B File Offset: 0x003D297B
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007ABA RID: 31418
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TValue>> m_set;
	}

	// Token: 0x02001469 RID: 5225
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class KeysView : IReadOnlyCollection<TKey>, IEnumerable<TKey>, IEnumerable
	{
		// Token: 0x0600A397 RID: 41879 RVA: 0x003D48AB File Offset: 0x003D2AAB
		public KeysView(HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> set)
		{
			this.m_set = set;
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x0600A398 RID: 41880 RVA: 0x003D48BA File Offset: 0x003D2ABA
		public int Count
		{
			get
			{
				return this.m_set.Count;
			}
		}

		// Token: 0x0600A399 RID: 41881 RVA: 0x003D48C7 File Offset: 0x003D2AC7
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

		// Token: 0x0600A39A RID: 41882 RVA: 0x003D48D6 File Offset: 0x003D2AD6
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04007ABF RID: 31423
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ManyToManyDictionary<TKey, TValue>.Entry<TKey>> m_set;
	}
}
