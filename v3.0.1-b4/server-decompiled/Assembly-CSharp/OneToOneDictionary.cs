using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001449 RID: 5193
public class OneToOneDictionary<TKey, TValue>
{
	// Token: 0x1700130E RID: 4878
	// (get) Token: 0x0600A2C9 RID: 41673 RVA: 0x003CCA2F File Offset: 0x003CAC2F
	public int Count
	{
		get
		{
			return this._forward.Count;
		}
	}

	// Token: 0x0600A2CA RID: 41674 RVA: 0x003CCA3C File Offset: 0x003CAC3C
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

	// Token: 0x0600A2CB RID: 41675 RVA: 0x003CCABC File Offset: 0x003CACBC
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

	// Token: 0x0600A2CC RID: 41676 RVA: 0x003CCAF0 File Offset: 0x003CACF0
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

	// Token: 0x0600A2CD RID: 41677 RVA: 0x003CCB24 File Offset: 0x003CAD24
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

	// Token: 0x0600A2CE RID: 41678 RVA: 0x003CCB60 File Offset: 0x003CAD60
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

	// Token: 0x0600A2CF RID: 41679 RVA: 0x003CCB9A File Offset: 0x003CAD9A
	public TValue GetByKey(TKey key)
	{
		return this._forward[new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key)].Value;
	}

	// Token: 0x0600A2D0 RID: 41680 RVA: 0x003CCBB2 File Offset: 0x003CADB2
	public TKey GetByValue(TValue value)
	{
		return this._reverse[new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value)].Value;
	}

	// Token: 0x0600A2D1 RID: 41681 RVA: 0x003CCBCA File Offset: 0x003CADCA
	public bool ContainsKey(TKey key)
	{
		return this._forward.ContainsKey(new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A2D2 RID: 41682 RVA: 0x003CCBDD File Offset: 0x003CADDD
	public bool ContainsValue(TValue value)
	{
		return this._reverse.ContainsKey(new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A2D3 RID: 41683 RVA: 0x003CCBF0 File Offset: 0x003CADF0
	public void Clear()
	{
		this._forward.Clear();
		this._reverse.Clear();
	}

	// Token: 0x1700130F RID: 4879
	// (get) Token: 0x0600A2D4 RID: 41684 RVA: 0x003CCC08 File Offset: 0x003CAE08
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

	// Token: 0x17001310 RID: 4880
	// (get) Token: 0x0600A2D5 RID: 41685 RVA: 0x003CCC18 File Offset: 0x003CAE18
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

	// Token: 0x040079C3 RID: 31171
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>> _forward = new Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>>();

	// Token: 0x040079C4 RID: 31172
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TValue>, OneToOneDictionary<TKey, TValue>.Entry<TKey>> _reverse = new Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TValue>, OneToOneDictionary<TKey, TValue>.Entry<TKey>>();

	// Token: 0x0200144A RID: 5194
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A2D7 RID: 41687 RVA: 0x003CCC46 File Offset: 0x003CAE46
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (OneToOneDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A2D8 RID: 41688 RVA: 0x003CCC85 File Offset: 0x003CAE85
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A2D9 RID: 41689 RVA: 0x003CCC90 File Offset: 0x003CAE90
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

		// Token: 0x040079C5 RID: 31173
		public readonly T Value;

		// Token: 0x040079C6 RID: 31174
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x040079C7 RID: 31175
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}
}
