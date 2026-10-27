using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001496 RID: 5270
public class OneToOneDictionary<TKey, TValue>
{
	// Token: 0x17001398 RID: 5016
	// (get) Token: 0x0600A4CA RID: 42186 RVA: 0x003D8D87 File Offset: 0x003D6F87
	public int Count
	{
		get
		{
			return this._forward.Count;
		}
	}

	// Token: 0x0600A4CB RID: 42187 RVA: 0x003D8D94 File Offset: 0x003D6F94
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

	// Token: 0x0600A4CC RID: 42188 RVA: 0x003D8E14 File Offset: 0x003D7014
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

	// Token: 0x0600A4CD RID: 42189 RVA: 0x003D8E48 File Offset: 0x003D7048
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

	// Token: 0x0600A4CE RID: 42190 RVA: 0x003D8E7C File Offset: 0x003D707C
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

	// Token: 0x0600A4CF RID: 42191 RVA: 0x003D8EB8 File Offset: 0x003D70B8
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

	// Token: 0x0600A4D0 RID: 42192 RVA: 0x003D8EF2 File Offset: 0x003D70F2
	public TValue GetByKey(TKey key)
	{
		return this._forward[new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key)].Value;
	}

	// Token: 0x0600A4D1 RID: 42193 RVA: 0x003D8F0A File Offset: 0x003D710A
	public TKey GetByValue(TValue value)
	{
		return this._reverse[new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value)].Value;
	}

	// Token: 0x0600A4D2 RID: 42194 RVA: 0x003D8F22 File Offset: 0x003D7122
	public bool ContainsKey(TKey key)
	{
		return this._forward.ContainsKey(new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A4D3 RID: 42195 RVA: 0x003D8F35 File Offset: 0x003D7135
	public bool ContainsValue(TValue value)
	{
		return this._reverse.ContainsKey(new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A4D4 RID: 42196 RVA: 0x003D8F48 File Offset: 0x003D7148
	public void Clear()
	{
		this._forward.Clear();
		this._reverse.Clear();
	}

	// Token: 0x17001399 RID: 5017
	// (get) Token: 0x0600A4D5 RID: 42197 RVA: 0x003D8F60 File Offset: 0x003D7160
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

	// Token: 0x1700139A RID: 5018
	// (get) Token: 0x0600A4D6 RID: 42198 RVA: 0x003D8F70 File Offset: 0x003D7170
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

	// Token: 0x04007B58 RID: 31576
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>> _forward = new Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>>();

	// Token: 0x04007B59 RID: 31577
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TValue>, OneToOneDictionary<TKey, TValue>.Entry<TKey>> _reverse = new Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TValue>, OneToOneDictionary<TKey, TValue>.Entry<TKey>>();

	// Token: 0x02001497 RID: 5271
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A4D8 RID: 42200 RVA: 0x003D8F9E File Offset: 0x003D719E
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (OneToOneDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A4D9 RID: 42201 RVA: 0x003D8FDD File Offset: 0x003D71DD
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A4DA RID: 42202 RVA: 0x003D8FE8 File Offset: 0x003D71E8
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

		// Token: 0x04007B5A RID: 31578
		public readonly T Value;

		// Token: 0x04007B5B RID: 31579
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007B5C RID: 31580
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}
}
