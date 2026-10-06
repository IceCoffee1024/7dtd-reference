using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001484 RID: 5252
public class OneToOneDictionary<TKey, TValue>
{
	// Token: 0x1700138C RID: 5004
	// (get) Token: 0x0600A472 RID: 42098 RVA: 0x003D6983 File Offset: 0x003D4B83
	public int Count
	{
		get
		{
			return this._forward.Count;
		}
	}

	// Token: 0x0600A473 RID: 42099 RVA: 0x003D6990 File Offset: 0x003D4B90
	public void Add(TKey key, TValue value)
	{
		OneToOneDictionary<TKey, TValue>.Entry<TKey> entry = new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key);
		OneToOneDictionary<TKey, TValue>.Entry<TValue> entry2 = new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value);
		if (this._forward.ContainsKey(entry))
		{
			throw new ArgumentException(string.Format("[TwoWayDictionary] Duplicate key: {0}", key));
		}
		if (this._reverse.ContainsKey(entry2))
		{
			throw new ArgumentException(string.Format("[TwoWayDictionary] Duplicate value: {0}", value));
		}
		this._forward[entry] = entry2;
		this._reverse[entry2] = entry;
	}

	// Token: 0x0600A474 RID: 42100 RVA: 0x003D6A10 File Offset: 0x003D4C10
	public bool RemoveByKey(TKey key)
	{
		OneToOneDictionary<TKey, TValue>.Entry<TKey> entry = new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key);
		OneToOneDictionary<TKey, TValue>.Entry<TValue> entry2;
		if (this._forward.Remove(entry, out entry2))
		{
			this._reverse.Remove(entry2);
			return true;
		}
		return false;
	}

	// Token: 0x0600A475 RID: 42101 RVA: 0x003D6A44 File Offset: 0x003D4C44
	public bool RemoveByValue(TValue value)
	{
		OneToOneDictionary<TKey, TValue>.Entry<TValue> entry = new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value);
		OneToOneDictionary<TKey, TValue>.Entry<TKey> entry2;
		if (this._reverse.Remove(entry, out entry2))
		{
			this._forward.Remove(entry2);
			return true;
		}
		return false;
	}

	// Token: 0x0600A476 RID: 42102 RVA: 0x003D6A78 File Offset: 0x003D4C78
	public bool TryGetByKey(TKey key, out TValue value)
	{
		OneToOneDictionary<TKey, TValue>.Entry<TKey> entry = new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key);
		OneToOneDictionary<TKey, TValue>.Entry<TValue> entry2;
		if (this._forward.TryGetValue(entry, out entry2))
		{
			value = entry2.Value;
			return true;
		}
		value = default(TValue);
		return false;
	}

	// Token: 0x0600A477 RID: 42103 RVA: 0x003D6AB4 File Offset: 0x003D4CB4
	public bool TryGetByValue(TValue value, out TKey key)
	{
		OneToOneDictionary<TKey, TValue>.Entry<TValue> entry = new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value);
		OneToOneDictionary<TKey, TValue>.Entry<TKey> entry2;
		if (this._reverse.TryGetValue(entry, out entry2))
		{
			key = entry2.Value;
			return true;
		}
		key = default(TKey);
		return false;
	}

	// Token: 0x0600A478 RID: 42104 RVA: 0x003D6AEE File Offset: 0x003D4CEE
	public TValue GetByKey(TKey key)
	{
		return this._forward[new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key)].Value;
	}

	// Token: 0x0600A479 RID: 42105 RVA: 0x003D6B06 File Offset: 0x003D4D06
	public TKey GetByValue(TValue value)
	{
		return this._reverse[new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value)].Value;
	}

	// Token: 0x0600A47A RID: 42106 RVA: 0x003D6B1E File Offset: 0x003D4D1E
	public bool ContainsKey(TKey key)
	{
		return this._forward.ContainsKey(new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A47B RID: 42107 RVA: 0x003D6B31 File Offset: 0x003D4D31
	public bool ContainsValue(TValue value)
	{
		return this._reverse.ContainsKey(new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A47C RID: 42108 RVA: 0x003D6B44 File Offset: 0x003D4D44
	public void Clear()
	{
		this._forward.Clear();
		this._reverse.Clear();
	}

	// Token: 0x1700138D RID: 5005
	// (get) Token: 0x0600A47D RID: 42109 RVA: 0x003D6B5C File Offset: 0x003D4D5C
	public IEnumerable<TKey> Keys
	{
		get
		{
			foreach (OneToOneDictionary<TKey, TValue>.Entry<TKey> entry in this._forward.Keys)
			{
				yield return entry.Value;
			}
			Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>>.KeyCollection.Enumerator enumerator = default(Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}
	}

	// Token: 0x1700138E RID: 5006
	// (get) Token: 0x0600A47E RID: 42110 RVA: 0x003D6B6C File Offset: 0x003D4D6C
	public IEnumerable<TValue> Values
	{
		get
		{
			foreach (OneToOneDictionary<TKey, TValue>.Entry<TValue> entry in this._forward.Values)
			{
				yield return entry.Value;
			}
			Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>>.ValueCollection.Enumerator enumerator = default(Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}
	}

	// Token: 0x04007B17 RID: 31511
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>> _forward = new Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>>();

	// Token: 0x04007B18 RID: 31512
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TValue>, OneToOneDictionary<TKey, TValue>.Entry<TKey>> _reverse = new Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TValue>, OneToOneDictionary<TKey, TValue>.Entry<TKey>>();

	// Token: 0x02001485 RID: 5253
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A480 RID: 42112 RVA: 0x003D6B9A File Offset: 0x003D4D9A
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (OneToOneDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A481 RID: 42113 RVA: 0x003D6BD9 File Offset: 0x003D4DD9
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A482 RID: 42114 RVA: 0x003D6BE4 File Offset: 0x003D4DE4
		public override bool Equals(object obj)
		{
			OneToOneDictionary<TKey, TValue>.Entry<T> entry = obj as OneToOneDictionary<TKey, TValue>.Entry<T>;
			if (entry == null)
			{
				return false;
			}
			if (OneToOneDictionary<TKey, TValue>.Entry<T>.s_isRefType)
			{
				return this.Value == entry.Value;
			}
			return EqualityComparer<T>.Default.Equals(this.Value, entry.Value);
		}

		// Token: 0x04007B19 RID: 31513
		public readonly T Value;

		// Token: 0x04007B1A RID: 31514
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007B1B RID: 31515
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}
}
