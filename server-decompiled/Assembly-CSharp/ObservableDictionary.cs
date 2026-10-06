using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001479 RID: 5241
public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection
{
	// Token: 0x0600A410 RID: 42000 RVA: 0x003D5BF8 File Offset: 0x003D3DF8
	public ObservableDictionary()
		: this(new Dictionary<TKey, TValue>())
	{
	}

	// Token: 0x0600A411 RID: 42001 RVA: 0x003D5C05 File Offset: 0x003D3E05
	public ObservableDictionary(Dictionary<TKey, TValue> dictionary)
	{
		this._dictionary = dictionary;
	}

	// Token: 0x14000104 RID: 260
	// (add) Token: 0x0600A412 RID: 42002 RVA: 0x003D5C14 File Offset: 0x003D3E14
	// (remove) Token: 0x0600A413 RID: 42003 RVA: 0x003D5C4C File Offset: 0x003D3E4C
	public event DictionaryAddEventHandler<TKey, TValue> EntryAdded;

	// Token: 0x14000105 RID: 261
	// (add) Token: 0x0600A414 RID: 42004 RVA: 0x003D5C84 File Offset: 0x003D3E84
	// (remove) Token: 0x0600A415 RID: 42005 RVA: 0x003D5CBC File Offset: 0x003D3EBC
	public event DictionaryRemoveEventHandler<TKey, TValue> EntryRemoved;

	// Token: 0x14000106 RID: 262
	// (add) Token: 0x0600A416 RID: 42006 RVA: 0x003D5CF4 File Offset: 0x003D3EF4
	// (remove) Token: 0x0600A417 RID: 42007 RVA: 0x003D5D2C File Offset: 0x003D3F2C
	public event DictionaryUpdatedValueEventHandler<TKey, TValue> EntryUpdatedValue;

	// Token: 0x14000107 RID: 263
	// (add) Token: 0x0600A418 RID: 42008 RVA: 0x003D5D64 File Offset: 0x003D3F64
	// (remove) Token: 0x0600A419 RID: 42009 RVA: 0x003D5D9C File Offset: 0x003D3F9C
	public event DictionaryEntryModifiedEventHandler<TKey, TValue> EntryModified;

	// Token: 0x0600A41A RID: 42010 RVA: 0x003D5DD1 File Offset: 0x003D3FD1
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

	// Token: 0x0600A41B RID: 42011 RVA: 0x003D5DEC File Offset: 0x003D3FEC
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

	// Token: 0x0600A41C RID: 42012 RVA: 0x003D5E11 File Offset: 0x003D4011
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

	// Token: 0x0600A41D RID: 42013 RVA: 0x003D5E36 File Offset: 0x003D4036
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

	// Token: 0x0600A41E RID: 42014 RVA: 0x003D5E5B File Offset: 0x003D405B
	public void Add(TKey key, TValue value)
	{
		this._dictionary.Add(key, value);
		this.OnEntryAdded(key, value);
	}

	// Token: 0x0600A41F RID: 42015 RVA: 0x003D5E74 File Offset: 0x003D4074
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

	// Token: 0x17001375 RID: 4981
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

	// Token: 0x17001376 RID: 4982
	// (get) Token: 0x0600A422 RID: 42018 RVA: 0x003D5EF3 File Offset: 0x003D40F3
	public ICollection<TKey> Keys
	{
		get
		{
			return this._dictionary.Keys;
		}
	}

	// Token: 0x17001377 RID: 4983
	// (get) Token: 0x0600A423 RID: 42019 RVA: 0x003D5F00 File Offset: 0x003D4100
	public ICollection<TValue> Values
	{
		get
		{
			return this._dictionary.Values;
		}
	}

	// Token: 0x0600A424 RID: 42020 RVA: 0x003D5F0D File Offset: 0x003D410D
	public bool ContainsKey(TKey key)
	{
		return this._dictionary.ContainsKey(key);
	}

	// Token: 0x0600A425 RID: 42021 RVA: 0x003D5F1B File Offset: 0x003D411B
	public bool TryGetValue(TKey key, out TValue value)
	{
		return this._dictionary.TryGetValue(key, out value);
	}

	// Token: 0x0600A426 RID: 42022 RVA: 0x003D5F2A File Offset: 0x003D412A
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A427 RID: 42023 RVA: 0x003D5F40 File Offset: 0x003D4140
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return this.Remove(item.Key);
	}

	// Token: 0x0600A428 RID: 42024 RVA: 0x003D5F4F File Offset: 0x003D414F
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return this._dictionary.ContainsKey(item.Key) && EqualityComparer<TValue>.Default.Equals(this._dictionary[item.Key], item.Value);
	}

	// Token: 0x0600A429 RID: 42025 RVA: 0x003D5F8A File Offset: 0x003D418A
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).CopyTo(array, arrayIndex);
	}

	// Token: 0x17001378 RID: 4984
	// (get) Token: 0x0600A42A RID: 42026 RVA: 0x003D5F99 File Offset: 0x003D4199
	public int Count
	{
		get
		{
			return this._dictionary.Count;
		}
	}

	// Token: 0x17001379 RID: 4985
	// (get) Token: 0x0600A42B RID: 42027 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A42C RID: 42028 RVA: 0x003D5FA8 File Offset: 0x003D41A8
	public void Clear()
	{
		foreach (TKey tkey in new List<TKey>(this._dictionary.Keys))
		{
			this.Remove(tkey);
		}
	}

	// Token: 0x0600A42D RID: 42029 RVA: 0x003D6008 File Offset: 0x003D4208
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this._dictionary.GetEnumerator();
	}

	// Token: 0x0600A42E RID: 42030 RVA: 0x003D6008 File Offset: 0x003D4208
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this._dictionary.GetEnumerator();
	}

	// Token: 0x0600A42F RID: 42031 RVA: 0x003D601A File Offset: 0x003D421A
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.ICollection.CopyTo(Array array, int index)
	{
		((ICollection)this._dictionary).CopyTo(array, index);
	}

	// Token: 0x1700137A RID: 4986
	// (get) Token: 0x0600A430 RID: 42032 RVA: 0x003D6029 File Offset: 0x003D4229
	public bool System.Collections.ICollection.IsSynchronized
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((ICollection)this._dictionary).IsSynchronized;
		}
	}

	// Token: 0x1700137B RID: 4987
	// (get) Token: 0x0600A431 RID: 42033 RVA: 0x003D6036 File Offset: 0x003D4236
	public object System.Collections.ICollection.SyncRoot
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((ICollection)this._dictionary).SyncRoot;
		}
	}

	// Token: 0x1700137C RID: 4988
	// (get) Token: 0x0600A432 RID: 42034 RVA: 0x003D6043 File Offset: 0x003D4243
	public bool System.Collections.IDictionary.IsFixedSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).IsFixedSize;
		}
	}

	// Token: 0x1700137D RID: 4989
	// (get) Token: 0x0600A433 RID: 42035 RVA: 0x003D6050 File Offset: 0x003D4250
	public bool System.Collections.IDictionary.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).IsReadOnly;
		}
	}

	// Token: 0x1700137E RID: 4990
	// (get) Token: 0x0600A434 RID: 42036 RVA: 0x003D605D File Offset: 0x003D425D
	public ICollection System.Collections.IDictionary.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).Keys;
		}
	}

	// Token: 0x1700137F RID: 4991
	// (get) Token: 0x0600A435 RID: 42037 RVA: 0x003D606A File Offset: 0x003D426A
	public ICollection System.Collections.IDictionary.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ((IDictionary)this._dictionary).Values;
		}
	}

	// Token: 0x17001380 RID: 4992
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

	// Token: 0x0600A438 RID: 42040 RVA: 0x003D6094 File Offset: 0x003D4294
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.IDictionary.Add(object key, object value)
	{
		((IDictionary)this._dictionary).Add(key, value);
	}

	// Token: 0x0600A439 RID: 42041 RVA: 0x003D60A3 File Offset: 0x003D42A3
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.IDictionary.Contains(object key)
	{
		return ((IDictionary)this._dictionary).Contains(key);
	}

	// Token: 0x0600A43A RID: 42042 RVA: 0x003D60B1 File Offset: 0x003D42B1
	[PublicizedFrom(EAccessModifier.Private)]
	public IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
	{
		return ((IDictionary)this._dictionary).GetEnumerator();
	}

	// Token: 0x0600A43B RID: 42043 RVA: 0x003D60BE File Offset: 0x003D42BE
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.IDictionary.Remove(object key)
	{
		((IDictionary)this._dictionary).Remove(key);
	}

	// Token: 0x04007AF8 RID: 31480
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, TValue> _dictionary;

	// Token: 0x0200147A RID: 5242
	public enum EChangeType
	{
		// Token: 0x04007AFE RID: 31486
		Added,
		// Token: 0x04007AFF RID: 31487
		Removed,
		// Token: 0x04007B00 RID: 31488
		Updated
	}
}
