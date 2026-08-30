using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200143E RID: 5182
public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection
{
	// Token: 0x0600A267 RID: 41575 RVA: 0x003CBCA4 File Offset: 0x003C9EA4
	public ObservableDictionary()
		: this(new Dictionary<TKey, TValue>())
	{
	}

	// Token: 0x0600A268 RID: 41576 RVA: 0x003CBCB1 File Offset: 0x003C9EB1
	public ObservableDictionary(Dictionary<TKey, TValue> dictionary)
	{
		this._dictionary = dictionary;
	}

	// Token: 0x14000104 RID: 260
	// (add) Token: 0x0600A269 RID: 41577 RVA: 0x003CBCC0 File Offset: 0x003C9EC0
	// (remove) Token: 0x0600A26A RID: 41578 RVA: 0x003CBCF8 File Offset: 0x003C9EF8
	public event DictionaryAddEventHandler<TKey, TValue> EntryAdded;

	// Token: 0x14000105 RID: 261
	// (add) Token: 0x0600A26B RID: 41579 RVA: 0x003CBD30 File Offset: 0x003C9F30
	// (remove) Token: 0x0600A26C RID: 41580 RVA: 0x003CBD68 File Offset: 0x003C9F68
	public event DictionaryRemoveEventHandler<TKey, TValue> EntryRemoved;

	// Token: 0x14000106 RID: 262
	// (add) Token: 0x0600A26D RID: 41581 RVA: 0x003CBDA0 File Offset: 0x003C9FA0
	// (remove) Token: 0x0600A26E RID: 41582 RVA: 0x003CBDD8 File Offset: 0x003C9FD8
	public event DictionaryUpdatedValueEventHandler<TKey, TValue> EntryUpdatedValue;

	// Token: 0x14000107 RID: 263
	// (add) Token: 0x0600A26F RID: 41583 RVA: 0x003CBE10 File Offset: 0x003CA010
	// (remove) Token: 0x0600A270 RID: 41584 RVA: 0x003CBE48 File Offset: 0x003CA048
	public event DictionaryEntryModifiedEventHandler<TKey, TValue> EntryModified;

	// Token: 0x0600A271 RID: 41585 RVA: 0x003CBE7D File Offset: 0x003CA07D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEntryModified(TKey key, TValue value, ObservableDictionary<TKey, TValue>.EChangeType action)
	{
		DictionaryEntryModifiedEventHandler<TKey, TValue> entryModified = this.EntryModified;
		if (entryModified == null)
		{
			return;
		}
		entryModified(this, new DictionaryChangedEventArgs<TKey, TValue>(key, value, action));
	}

	// Token: 0x0600A272 RID: 41586 RVA: 0x003CBE98 File Offset: 0x003CA098
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEntryAdded(TKey key, TValue value)
	{
		DictionaryAddEventHandler<TKey, TValue> entryAdded = this.EntryAdded;
		if (entryAdded != null)
		{
			entryAdded(this, new DictionaryChangedEventArgs<TKey, TValue>(key, value, ObservableDictionary<TKey, TValue>.EChangeType.Added));
		}
		this.OnEntryModified(key, value, ObservableDictionary<TKey, TValue>.EChangeType.Added);
	}

	// Token: 0x0600A273 RID: 41587 RVA: 0x003CBEBD File Offset: 0x003CA0BD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEntryRemoved(TKey key, TValue value)
	{
		DictionaryRemoveEventHandler<TKey, TValue> entryRemoved = this.EntryRemoved;
		if (entryRemoved != null)
		{
			entryRemoved(this, new DictionaryChangedEventArgs<TKey, TValue>(key, value, ObservableDictionary<TKey, TValue>.EChangeType.Removed));
		}
		this.OnEntryModified(key, value, ObservableDictionary<TKey, TValue>.EChangeType.Removed);
	}

	// Token: 0x0600A274 RID: 41588 RVA: 0x003CBEE2 File Offset: 0x003CA0E2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEntryUpdated(TKey key, TValue value)
	{
		DictionaryUpdatedValueEventHandler<TKey, TValue> entryUpdatedValue = this.EntryUpdatedValue;
		if (entryUpdatedValue != null)
		{
			entryUpdatedValue(this, new DictionaryChangedEventArgs<TKey, TValue>(key, value, ObservableDictionary<TKey, TValue>.EChangeType.Updated));
		}
		this.OnEntryModified(key, value, ObservableDictionary<TKey, TValue>.EChangeType.Updated);
	}

	// Token: 0x0600A275 RID: 41589 RVA: 0x003CBF07 File Offset: 0x003CA107
	public void Add(TKey key, TValue value)
	{
		this._dictionary.Add(key, value);
		this.OnEntryAdded(key, value);
	}

	// Token: 0x0600A276 RID: 41590 RVA: 0x003CBF20 File Offset: 0x003CA120
	public bool Remove(TKey key)
	{
		TValue tvalue;
		if (this._dictionary.TryGetValue(key, out tvalue) && this._dictionary.Remove(key))
		{
			this.OnEntryRemoved(key, tvalue);
			return true;
		}
		return false;
	}

	// Token: 0x170012F7 RID: 4855
	public TValue this[TKey key]
	{
		get
		{
			return this._dictionary[key];
		}
		set
		{
			if (this._dictionary.ContainsKey(key))
			{
				this._dictionary[key] = value;
				this.OnEntryUpdated(key, value);
				return;
			}
			this._dictionary[key] = value;
			this.OnEntryAdded(key, value);
		}
	}

	// Token: 0x170012F8 RID: 4856
	// (get) Token: 0x0600A279 RID: 41593 RVA: 0x003CBF9F File Offset: 0x003CA19F
	public ICollection<TKey> Keys
	{
		get
		{
			return this._dictionary.Keys;
		}
	}

	// Token: 0x170012F9 RID: 4857
	// (get) Token: 0x0600A27A RID: 41594 RVA: 0x003CBFAC File Offset: 0x003CA1AC
	public ICollection<TValue> Values
	{
		get
		{
			return this._dictionary.Values;
		}
	}

	// Token: 0x0600A27B RID: 41595 RVA: 0x003CBFB9 File Offset: 0x003CA1B9
	public bool ContainsKey(TKey key)
	{
		return this._dictionary.ContainsKey(key);
	}

	// Token: 0x0600A27C RID: 41596 RVA: 0x003CBFC7 File Offset: 0x003CA1C7
	public bool TryGetValue(TKey key, out TValue value)
	{
		return this._dictionary.TryGetValue(key, out value);
	}

	// Token: 0x0600A27D RID: 41597 RVA: 0x003CBFD6 File Offset: 0x003CA1D6
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A27E RID: 41598 RVA: 0x003CBFEC File Offset: 0x003CA1EC
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return this.Remove(item.Key);
	}

	// Token: 0x0600A27F RID: 41599 RVA: 0x003CBFFB File Offset: 0x003CA1FB
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return this._dictionary.ContainsKey(item.Key) && EqualityComparer<TValue>.Default.Equals(this._dictionary[item.Key], item.Value);
	}

	// Token: 0x0600A280 RID: 41600 RVA: 0x003CC036 File Offset: 0x003CA236
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).CopyTo(array, arrayIndex);
	}

	// Token: 0x170012FA RID: 4858
	// (get) Token: 0x0600A281 RID: 41601 RVA: 0x003CC045 File Offset: 0x003CA245
	public int Count
	{
		get
		{
			return this._dictionary.Count;
		}
	}

	// Token: 0x170012FB RID: 4859
	// (get) Token: 0x0600A282 RID: 41602 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A283 RID: 41603 RVA: 0x003CC054 File Offset: 0x003CA254
	public void Clear()
	{
		foreach (TKey tkey in new List<TKey>(this._dictionary.Keys))
		{
			this.Remove(tkey);
		}
	}

	// Token: 0x0600A284 RID: 41604 RVA: 0x003CC0B4 File Offset: 0x003CA2B4
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this._dictionary.GetEnumerator();
	}

	// Token: 0x0600A285 RID: 41605 RVA: 0x003CC0B4 File Offset: 0x003CA2B4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this._dictionary.GetEnumerator();
	}

	// Token: 0x0600A286 RID: 41606 RVA: 0x003CC0C6 File Offset: 0x003CA2C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.ICollection.CopyTo(Array array, int index)
	{
		((ICollection)this._dictionary).CopyTo(array, index);
	}

	// Token: 0x170012FC RID: 4860
	// (get) Token: 0x0600A287 RID: 41607 RVA: 0x003CC0D5 File Offset: 0x003CA2D5
	public bool System.Collections.ICollection.IsSynchronized
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((ICollection)this._dictionary).IsSynchronized;
		}
	}

	// Token: 0x170012FD RID: 4861
	// (get) Token: 0x0600A288 RID: 41608 RVA: 0x003CC0E2 File Offset: 0x003CA2E2
	public object System.Collections.ICollection.SyncRoot
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((ICollection)this._dictionary).SyncRoot;
		}
	}

	// Token: 0x170012FE RID: 4862
	// (get) Token: 0x0600A289 RID: 41609 RVA: 0x003CC0EF File Offset: 0x003CA2EF
	public bool System.Collections.IDictionary.IsFixedSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).IsFixedSize;
		}
	}

	// Token: 0x170012FF RID: 4863
	// (get) Token: 0x0600A28A RID: 41610 RVA: 0x003CC0FC File Offset: 0x003CA2FC
	public bool System.Collections.IDictionary.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).IsReadOnly;
		}
	}

	// Token: 0x17001300 RID: 4864
	// (get) Token: 0x0600A28B RID: 41611 RVA: 0x003CC109 File Offset: 0x003CA309
	public ICollection System.Collections.IDictionary.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).Keys;
		}
	}

	// Token: 0x17001301 RID: 4865
	// (get) Token: 0x0600A28C RID: 41612 RVA: 0x003CC116 File Offset: 0x003CA316
	public ICollection System.Collections.IDictionary.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).Values;
		}
	}

	// Token: 0x17001302 RID: 4866
	public object this[object key]
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary)[key];
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			((IDictionary)this._dictionary)[key] = value;
		}
	}

	// Token: 0x0600A28F RID: 41615 RVA: 0x003CC140 File Offset: 0x003CA340
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.IDictionary.Add(object key, object value)
	{
		((IDictionary)this._dictionary).Add(key, value);
	}

	// Token: 0x0600A290 RID: 41616 RVA: 0x003CC14F File Offset: 0x003CA34F
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.IDictionary.Contains(object key)
	{
		return ((IDictionary)this._dictionary).Contains(key);
	}

	// Token: 0x0600A291 RID: 41617 RVA: 0x003CC15D File Offset: 0x003CA35D
	[PublicizedFrom(EAccessModifier.Private)]
	public IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
	{
		return ((IDictionary)this._dictionary).GetEnumerator();
	}

	// Token: 0x0600A292 RID: 41618 RVA: 0x003CC16A File Offset: 0x003CA36A
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.IDictionary.Remove(object key)
	{
		((IDictionary)this._dictionary).Remove(key);
	}

	// Token: 0x040079A4 RID: 31140
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, TValue> _dictionary;

	// Token: 0x0200143F RID: 5183
	public enum EChangeType
	{
		// Token: 0x040079AA RID: 31146
		Added,
		// Token: 0x040079AB RID: 31147
		Removed,
		// Token: 0x040079AC RID: 31148
		Updated
	}
}
