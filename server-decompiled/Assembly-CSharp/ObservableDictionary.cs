using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200148B RID: 5259
public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection
{
	// Token: 0x0600A468 RID: 42088 RVA: 0x003D7FFC File Offset: 0x003D61FC
	public ObservableDictionary()
		: this(new Dictionary<TKey, TValue>())
	{
	}

	// Token: 0x0600A469 RID: 42089 RVA: 0x003D8009 File Offset: 0x003D6209
	public ObservableDictionary(Dictionary<TKey, TValue> dictionary)
	{
		this._dictionary = dictionary;
	}

	// Token: 0x14000104 RID: 260
	// (add) Token: 0x0600A46A RID: 42090 RVA: 0x003D8018 File Offset: 0x003D6218
	// (remove) Token: 0x0600A46B RID: 42091 RVA: 0x003D8050 File Offset: 0x003D6250
	public event DictionaryAddEventHandler<TKey, TValue> EntryAdded;

	// Token: 0x14000105 RID: 261
	// (add) Token: 0x0600A46C RID: 42092 RVA: 0x003D8088 File Offset: 0x003D6288
	// (remove) Token: 0x0600A46D RID: 42093 RVA: 0x003D80C0 File Offset: 0x003D62C0
	public event DictionaryRemoveEventHandler<TKey, TValue> EntryRemoved;

	// Token: 0x14000106 RID: 262
	// (add) Token: 0x0600A46E RID: 42094 RVA: 0x003D80F8 File Offset: 0x003D62F8
	// (remove) Token: 0x0600A46F RID: 42095 RVA: 0x003D8130 File Offset: 0x003D6330
	public event DictionaryUpdatedValueEventHandler<TKey, TValue> EntryUpdatedValue;

	// Token: 0x14000107 RID: 263
	// (add) Token: 0x0600A470 RID: 42096 RVA: 0x003D8168 File Offset: 0x003D6368
	// (remove) Token: 0x0600A471 RID: 42097 RVA: 0x003D81A0 File Offset: 0x003D63A0
	public event DictionaryEntryModifiedEventHandler<TKey, TValue> EntryModified;

	// Token: 0x0600A472 RID: 42098 RVA: 0x003D81D5 File Offset: 0x003D63D5
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

	// Token: 0x0600A473 RID: 42099 RVA: 0x003D81F0 File Offset: 0x003D63F0
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

	// Token: 0x0600A474 RID: 42100 RVA: 0x003D8215 File Offset: 0x003D6415
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

	// Token: 0x0600A475 RID: 42101 RVA: 0x003D823A File Offset: 0x003D643A
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

	// Token: 0x0600A476 RID: 42102 RVA: 0x003D825F File Offset: 0x003D645F
	public void Add(TKey key, TValue value)
	{
		this._dictionary.Add(key, value);
		this.OnEntryAdded(key, value);
	}

	// Token: 0x0600A477 RID: 42103 RVA: 0x003D8278 File Offset: 0x003D6478
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

	// Token: 0x17001381 RID: 4993
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

	// Token: 0x17001382 RID: 4994
	// (get) Token: 0x0600A47A RID: 42106 RVA: 0x003D82F7 File Offset: 0x003D64F7
	public ICollection<TKey> Keys
	{
		get
		{
			return this._dictionary.Keys;
		}
	}

	// Token: 0x17001383 RID: 4995
	// (get) Token: 0x0600A47B RID: 42107 RVA: 0x003D8304 File Offset: 0x003D6504
	public ICollection<TValue> Values
	{
		get
		{
			return this._dictionary.Values;
		}
	}

	// Token: 0x0600A47C RID: 42108 RVA: 0x003D8311 File Offset: 0x003D6511
	public bool ContainsKey(TKey key)
	{
		return this._dictionary.ContainsKey(key);
	}

	// Token: 0x0600A47D RID: 42109 RVA: 0x003D831F File Offset: 0x003D651F
	public bool TryGetValue(TKey key, out TValue value)
	{
		return this._dictionary.TryGetValue(key, out value);
	}

	// Token: 0x0600A47E RID: 42110 RVA: 0x003D832E File Offset: 0x003D652E
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A47F RID: 42111 RVA: 0x003D8344 File Offset: 0x003D6544
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return this.Remove(item.Key);
	}

	// Token: 0x0600A480 RID: 42112 RVA: 0x003D8353 File Offset: 0x003D6553
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return this._dictionary.ContainsKey(item.Key) && EqualityComparer<TValue>.Default.Equals(this._dictionary[item.Key], item.Value);
	}

	// Token: 0x0600A481 RID: 42113 RVA: 0x003D838E File Offset: 0x003D658E
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).CopyTo(array, arrayIndex);
	}

	// Token: 0x17001384 RID: 4996
	// (get) Token: 0x0600A482 RID: 42114 RVA: 0x003D839D File Offset: 0x003D659D
	public int Count
	{
		get
		{
			return this._dictionary.Count;
		}
	}

	// Token: 0x17001385 RID: 4997
	// (get) Token: 0x0600A483 RID: 42115 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A484 RID: 42116 RVA: 0x003D83AC File Offset: 0x003D65AC
	public void Clear()
	{
		foreach (TKey tkey in new List<TKey>(this._dictionary.Keys))
		{
			this.Remove(tkey);
		}
	}

	// Token: 0x0600A485 RID: 42117 RVA: 0x003D840C File Offset: 0x003D660C
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this._dictionary.GetEnumerator();
	}

	// Token: 0x0600A486 RID: 42118 RVA: 0x003D840C File Offset: 0x003D660C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this._dictionary.GetEnumerator();
	}

	// Token: 0x0600A487 RID: 42119 RVA: 0x003D841E File Offset: 0x003D661E
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.ICollection.CopyTo(Array array, int index)
	{
		((ICollection)this._dictionary).CopyTo(array, index);
	}

	// Token: 0x17001386 RID: 4998
	// (get) Token: 0x0600A488 RID: 42120 RVA: 0x003D842D File Offset: 0x003D662D
	public bool System.Collections.ICollection.IsSynchronized
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((ICollection)this._dictionary).IsSynchronized;
		}
	}

	// Token: 0x17001387 RID: 4999
	// (get) Token: 0x0600A489 RID: 42121 RVA: 0x003D843A File Offset: 0x003D663A
	public object System.Collections.ICollection.SyncRoot
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((ICollection)this._dictionary).SyncRoot;
		}
	}

	// Token: 0x17001388 RID: 5000
	// (get) Token: 0x0600A48A RID: 42122 RVA: 0x003D8447 File Offset: 0x003D6647
	public bool System.Collections.IDictionary.IsFixedSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).IsFixedSize;
		}
	}

	// Token: 0x17001389 RID: 5001
	// (get) Token: 0x0600A48B RID: 42123 RVA: 0x003D8454 File Offset: 0x003D6654
	public bool System.Collections.IDictionary.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).IsReadOnly;
		}
	}

	// Token: 0x1700138A RID: 5002
	// (get) Token: 0x0600A48C RID: 42124 RVA: 0x003D8461 File Offset: 0x003D6661
	public ICollection System.Collections.IDictionary.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).Keys;
		}
	}

	// Token: 0x1700138B RID: 5003
	// (get) Token: 0x0600A48D RID: 42125 RVA: 0x003D846E File Offset: 0x003D666E
	public ICollection System.Collections.IDictionary.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).Values;
		}
	}

	// Token: 0x1700138C RID: 5004
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

	// Token: 0x0600A490 RID: 42128 RVA: 0x003D8498 File Offset: 0x003D6698
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.IDictionary.Add(object key, object value)
	{
		((IDictionary)this._dictionary).Add(key, value);
	}

	// Token: 0x0600A491 RID: 42129 RVA: 0x003D84A7 File Offset: 0x003D66A7
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.IDictionary.Contains(object key)
	{
		return ((IDictionary)this._dictionary).Contains(key);
	}

	// Token: 0x0600A492 RID: 42130 RVA: 0x003D84B5 File Offset: 0x003D66B5
	[PublicizedFrom(EAccessModifier.Private)]
	public IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
	{
		return ((IDictionary)this._dictionary).GetEnumerator();
	}

	// Token: 0x0600A493 RID: 42131 RVA: 0x003D84C2 File Offset: 0x003D66C2
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.IDictionary.Remove(object key)
	{
		((IDictionary)this._dictionary).Remove(key);
	}

	// Token: 0x04007B39 RID: 31545
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, TValue> _dictionary;

	// Token: 0x0200148C RID: 5260
	public enum EChangeType
	{
		// Token: 0x04007B3F RID: 31551
		Added,
		// Token: 0x04007B40 RID: 31552
		Removed,
		// Token: 0x04007B41 RID: 31553
		Updated
	}
}
