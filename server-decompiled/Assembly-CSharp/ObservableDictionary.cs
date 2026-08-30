using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001459 RID: 5209
public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection
{
	// Token: 0x0600A364 RID: 41828 RVA: 0x003D0FE8 File Offset: 0x003CF1E8
	public ObservableDictionary()
		: this(new Dictionary<TKey, TValue>())
	{
	}

	// Token: 0x0600A365 RID: 41829 RVA: 0x003D0FF5 File Offset: 0x003CF1F5
	public ObservableDictionary(Dictionary<TKey, TValue> dictionary)
	{
		this._dictionary = dictionary;
	}

	// Token: 0x14000105 RID: 261
	// (add) Token: 0x0600A366 RID: 41830 RVA: 0x003D1004 File Offset: 0x003CF204
	// (remove) Token: 0x0600A367 RID: 41831 RVA: 0x003D103C File Offset: 0x003CF23C
	public event DictionaryAddEventHandler<TKey, TValue> EntryAdded;

	// Token: 0x14000106 RID: 262
	// (add) Token: 0x0600A368 RID: 41832 RVA: 0x003D1074 File Offset: 0x003CF274
	// (remove) Token: 0x0600A369 RID: 41833 RVA: 0x003D10AC File Offset: 0x003CF2AC
	public event DictionaryRemoveEventHandler<TKey, TValue> EntryRemoved;

	// Token: 0x14000107 RID: 263
	// (add) Token: 0x0600A36A RID: 41834 RVA: 0x003D10E4 File Offset: 0x003CF2E4
	// (remove) Token: 0x0600A36B RID: 41835 RVA: 0x003D111C File Offset: 0x003CF31C
	public event DictionaryUpdatedValueEventHandler<TKey, TValue> EntryUpdatedValue;

	// Token: 0x14000108 RID: 264
	// (add) Token: 0x0600A36C RID: 41836 RVA: 0x003D1154 File Offset: 0x003CF354
	// (remove) Token: 0x0600A36D RID: 41837 RVA: 0x003D118C File Offset: 0x003CF38C
	public event DictionaryEntryModifiedEventHandler<TKey, TValue> EntryModified;

	// Token: 0x0600A36E RID: 41838 RVA: 0x003D11C1 File Offset: 0x003CF3C1
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

	// Token: 0x0600A36F RID: 41839 RVA: 0x003D11DC File Offset: 0x003CF3DC
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

	// Token: 0x0600A370 RID: 41840 RVA: 0x003D1201 File Offset: 0x003CF401
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

	// Token: 0x0600A371 RID: 41841 RVA: 0x003D1226 File Offset: 0x003CF426
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

	// Token: 0x0600A372 RID: 41842 RVA: 0x003D124B File Offset: 0x003CF44B
	public void Add(TKey key, TValue value)
	{
		this._dictionary.Add(key, value);
		this.OnEntryAdded(key, value);
	}

	// Token: 0x0600A373 RID: 41843 RVA: 0x003D1264 File Offset: 0x003CF464
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

	// Token: 0x17001330 RID: 4912
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

	// Token: 0x17001331 RID: 4913
	// (get) Token: 0x0600A376 RID: 41846 RVA: 0x003D12E3 File Offset: 0x003CF4E3
	public ICollection<TKey> Keys
	{
		get
		{
			return this._dictionary.Keys;
		}
	}

	// Token: 0x17001332 RID: 4914
	// (get) Token: 0x0600A377 RID: 41847 RVA: 0x003D12F0 File Offset: 0x003CF4F0
	public ICollection<TValue> Values
	{
		get
		{
			return this._dictionary.Values;
		}
	}

	// Token: 0x0600A378 RID: 41848 RVA: 0x003D12FD File Offset: 0x003CF4FD
	public bool ContainsKey(TKey key)
	{
		return this._dictionary.ContainsKey(key);
	}

	// Token: 0x0600A379 RID: 41849 RVA: 0x003D130B File Offset: 0x003CF50B
	public bool TryGetValue(TKey key, out TValue value)
	{
		return this._dictionary.TryGetValue(key, out value);
	}

	// Token: 0x0600A37A RID: 41850 RVA: 0x003D131A File Offset: 0x003CF51A
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A37B RID: 41851 RVA: 0x003D1330 File Offset: 0x003CF530
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return this.Remove(item.Key);
	}

	// Token: 0x0600A37C RID: 41852 RVA: 0x003D133F File Offset: 0x003CF53F
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return this._dictionary.ContainsKey(item.Key) && EqualityComparer<TValue>.Default.Equals(this._dictionary[item.Key], item.Value);
	}

	// Token: 0x0600A37D RID: 41853 RVA: 0x003D137A File Offset: 0x003CF57A
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).CopyTo(array, arrayIndex);
	}

	// Token: 0x17001333 RID: 4915
	// (get) Token: 0x0600A37E RID: 41854 RVA: 0x003D1389 File Offset: 0x003CF589
	public int Count
	{
		get
		{
			return this._dictionary.Count;
		}
	}

	// Token: 0x17001334 RID: 4916
	// (get) Token: 0x0600A37F RID: 41855 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A380 RID: 41856 RVA: 0x003D1398 File Offset: 0x003CF598
	public void Clear()
	{
		foreach (TKey tkey in new List<TKey>(this._dictionary.Keys))
		{
			this.Remove(tkey);
		}
	}

	// Token: 0x0600A381 RID: 41857 RVA: 0x003D13F8 File Offset: 0x003CF5F8
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this._dictionary.GetEnumerator();
	}

	// Token: 0x0600A382 RID: 41858 RVA: 0x003D13F8 File Offset: 0x003CF5F8
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this._dictionary.GetEnumerator();
	}

	// Token: 0x0600A383 RID: 41859 RVA: 0x003D140A File Offset: 0x003CF60A
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.ICollection.CopyTo(Array array, int index)
	{
		((ICollection)this._dictionary).CopyTo(array, index);
	}

	// Token: 0x17001335 RID: 4917
	// (get) Token: 0x0600A384 RID: 41860 RVA: 0x003D1419 File Offset: 0x003CF619
	public bool System.Collections.ICollection.IsSynchronized
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((ICollection)this._dictionary).IsSynchronized;
		}
	}

	// Token: 0x17001336 RID: 4918
	// (get) Token: 0x0600A385 RID: 41861 RVA: 0x003D1426 File Offset: 0x003CF626
	public object System.Collections.ICollection.SyncRoot
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((ICollection)this._dictionary).SyncRoot;
		}
	}

	// Token: 0x17001337 RID: 4919
	// (get) Token: 0x0600A386 RID: 41862 RVA: 0x003D1433 File Offset: 0x003CF633
	public bool System.Collections.IDictionary.IsFixedSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).IsFixedSize;
		}
	}

	// Token: 0x17001338 RID: 4920
	// (get) Token: 0x0600A387 RID: 41863 RVA: 0x003D1440 File Offset: 0x003CF640
	public bool System.Collections.IDictionary.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).IsReadOnly;
		}
	}

	// Token: 0x17001339 RID: 4921
	// (get) Token: 0x0600A388 RID: 41864 RVA: 0x003D144D File Offset: 0x003CF64D
	public ICollection System.Collections.IDictionary.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).Keys;
		}
	}

	// Token: 0x1700133A RID: 4922
	// (get) Token: 0x0600A389 RID: 41865 RVA: 0x003D145A File Offset: 0x003CF65A
	public ICollection System.Collections.IDictionary.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).Values;
		}
	}

	// Token: 0x1700133B RID: 4923
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

	// Token: 0x0600A38C RID: 41868 RVA: 0x003D1484 File Offset: 0x003CF684
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.IDictionary.Add(object key, object value)
	{
		((IDictionary)this._dictionary).Add(key, value);
	}

	// Token: 0x0600A38D RID: 41869 RVA: 0x003D1493 File Offset: 0x003CF693
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.IDictionary.Contains(object key)
	{
		return ((IDictionary)this._dictionary).Contains(key);
	}

	// Token: 0x0600A38E RID: 41870 RVA: 0x003D14A1 File Offset: 0x003CF6A1
	[PublicizedFrom(EAccessModifier.Private)]
	public IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
	{
		return ((IDictionary)this._dictionary).GetEnumerator();
	}

	// Token: 0x0600A38F RID: 41871 RVA: 0x003D14AE File Offset: 0x003CF6AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.IDictionary.Remove(object key)
	{
		((IDictionary)this._dictionary).Remove(key);
	}

	// Token: 0x04007A7D RID: 31357
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, TValue> _dictionary;

	// Token: 0x0200145A RID: 5210
	public enum EChangeType
	{
		// Token: 0x04007A83 RID: 31363
		Added,
		// Token: 0x04007A84 RID: 31364
		Removed,
		// Token: 0x04007A85 RID: 31365
		Updated
	}
}
