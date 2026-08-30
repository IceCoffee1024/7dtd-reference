using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02001464 RID: 5220
public class OneToOneDictionary<TKey, TValue>
{
	// Token: 0x17001347 RID: 4935
	// (get) Token: 0x0600A3C6 RID: 41926 RVA: 0x003D1D73 File Offset: 0x003CFF73
	public int Count
	{
		get
		{
			return this._forward.Count;
		}
	}

	// Token: 0x0600A3C7 RID: 41927 RVA: 0x003D1D80 File Offset: 0x003CFF80
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

	// Token: 0x0600A3C8 RID: 41928 RVA: 0x003D1E00 File Offset: 0x003D0000
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

	// Token: 0x0600A3C9 RID: 41929 RVA: 0x003D1E34 File Offset: 0x003D0034
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

	// Token: 0x0600A3CA RID: 41930 RVA: 0x003D1E68 File Offset: 0x003D0068
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

	// Token: 0x0600A3CB RID: 41931 RVA: 0x003D1EA4 File Offset: 0x003D00A4
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

	// Token: 0x0600A3CC RID: 41932 RVA: 0x003D1EDE File Offset: 0x003D00DE
	public TValue GetByKey(TKey key)
	{
		return this._forward[new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key)].Value;
	}

	// Token: 0x0600A3CD RID: 41933 RVA: 0x003D1EF6 File Offset: 0x003D00F6
	public TKey GetByValue(TValue value)
	{
		return this._reverse[new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value)].Value;
	}

	// Token: 0x0600A3CE RID: 41934 RVA: 0x003D1F0E File Offset: 0x003D010E
	public bool ContainsKey(TKey key)
	{
		return this._forward.ContainsKey(new OneToOneDictionary<TKey, TValue>.Entry<TKey>(key));
	}

	// Token: 0x0600A3CF RID: 41935 RVA: 0x003D1F21 File Offset: 0x003D0121
	public bool ContainsValue(TValue value)
	{
		return this._reverse.ContainsKey(new OneToOneDictionary<TKey, TValue>.Entry<TValue>(value));
	}

	// Token: 0x0600A3D0 RID: 41936 RVA: 0x003D1F34 File Offset: 0x003D0134
	public void Clear()
	{
		this._forward.Clear();
		this._reverse.Clear();
	}

	// Token: 0x17001348 RID: 4936
	// (get) Token: 0x0600A3D1 RID: 41937 RVA: 0x003D1F4C File Offset: 0x003D014C
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

	// Token: 0x17001349 RID: 4937
	// (get) Token: 0x0600A3D2 RID: 41938 RVA: 0x003D1F5C File Offset: 0x003D015C
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

	// Token: 0x04007A9C RID: 31388
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>> _forward = new Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TKey>, OneToOneDictionary<TKey, TValue>.Entry<TValue>>();

	// Token: 0x04007A9D RID: 31389
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TValue>, OneToOneDictionary<TKey, TValue>.Entry<TKey>> _reverse = new Dictionary<OneToOneDictionary<TKey, TValue>.Entry<TValue>, OneToOneDictionary<TKey, TValue>.Entry<TKey>>();

	// Token: 0x02001465 RID: 5221
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry<T>
	{
		// Token: 0x0600A3D4 RID: 41940 RVA: 0x003D1F8A File Offset: 0x003D018A
		public Entry(T value)
		{
			this.Value = value;
			this.stableHash = (OneToOneDictionary<TKey, TValue>.Entry<T>.s_isRefType ? ((value == null) ? 0 : RuntimeHelpers.GetHashCode(value)) : EqualityComparer<T>.Default.GetHashCode(value));
		}

		// Token: 0x0600A3D5 RID: 41941 RVA: 0x003D1FC9 File Offset: 0x003D01C9
		public override int GetHashCode()
		{
			return this.stableHash;
		}

		// Token: 0x0600A3D6 RID: 41942 RVA: 0x003D1FD4 File Offset: 0x003D01D4
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

		// Token: 0x04007A9E RID: 31390
		public readonly T Value;

		// Token: 0x04007A9F RID: 31391
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int stableHash;

		// Token: 0x04007AA0 RID: 31392
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly bool s_isRefType = !typeof(T).IsValueType;
	}
}
