using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200142A RID: 5162
public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
{
	// Token: 0x0600A22A RID: 41514 RVA: 0x003CC612 File Offset: 0x003CA812
	public LinkedDictionary()
	{
		this.m_dictionary = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();
		this.m_list = new LinkedList<KeyValuePair<TKey, TValue>>();
	}

	// Token: 0x0600A22B RID: 41515 RVA: 0x003CC630 File Offset: 0x003CA830
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x0600A22C RID: 41516 RVA: 0x003CC642 File Offset: 0x003CA842
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600A22D RID: 41517 RVA: 0x003CC64A File Offset: 0x003CA84A
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A22E RID: 41518 RVA: 0x003CC660 File Offset: 0x003CA860
	public void Clear()
	{
		this.m_dictionary.Clear();
		this.m_list.Clear();
	}

	// Token: 0x0600A22F RID: 41519 RVA: 0x003CC678 File Offset: 0x003CA878
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Contains(KeyValuePair<TKey, TValue> item)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		return this.m_dictionary.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item.Value);
	}

	// Token: 0x0600A230 RID: 41520 RVA: 0x003CC6BC File Offset: 0x003CA8BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (arrayIndex < 0 || arrayIndex > array.Length)
		{
			throw new ArgumentOutOfRangeException("arrayIndex");
		}
		if (array.Length - arrayIndex < this.m_list.Count)
		{
			throw new ArgumentException("Not have enough space after the given arrayIndex.", "array");
		}
		LinkedListNode<KeyValuePair<TKey, TValue>> first = this.m_list.First;
		int i = arrayIndex;
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode = first;
		while (i < array.Length)
		{
			array[arrayIndex] = linkedListNode.Value;
			i++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x0600A231 RID: 41521 RVA: 0x003CC740 File Offset: 0x003CA940
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Remove(KeyValuePair<TKey, TValue> item)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		return this.m_dictionary.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item.Value) && this.Remove(item.Key);
	}

	// Token: 0x17001301 RID: 4865
	// (get) Token: 0x0600A232 RID: 41522 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return false;
		}
	}

	// Token: 0x0600A233 RID: 41523 RVA: 0x003CC795 File Offset: 0x003CA995
	public void Add(TKey key, TValue value)
	{
		if (this.m_dictionary.ContainsKey(key))
		{
			throw new ArgumentException("Already contains key.", "key");
		}
		this.m_dictionary.Add(key, this.m_list.AddLast(new KeyValuePair<TKey, TValue>(key, value)));
	}

	// Token: 0x0600A234 RID: 41524 RVA: 0x003CC7D3 File Offset: 0x003CA9D3
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.IDictionary<TKey,TValue>.ContainsKey(TKey key)
	{
		return this.m_dictionary.ContainsKey(key);
	}

	// Token: 0x0600A235 RID: 41525 RVA: 0x003CC7E4 File Offset: 0x003CA9E4
	public bool Remove(TKey key)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		if (!this.m_dictionary.Remove(key, out linkedListNode))
		{
			return false;
		}
		this.m_list.Remove(linkedListNode);
		return true;
	}

	// Token: 0x0600A236 RID: 41526 RVA: 0x003CC7D3 File Offset: 0x003CA9D3
	public bool ContainsKey(TKey key)
	{
		return this.m_dictionary.ContainsKey(key);
	}

	// Token: 0x0600A237 RID: 41527 RVA: 0x003CC810 File Offset: 0x003CAA10
	public bool TryGetValue(TKey key, out TValue value)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		if (!this.m_dictionary.TryGetValue(key, out linkedListNode))
		{
			value = default(TValue);
			return false;
		}
		value = linkedListNode.Value.Value;
		return true;
	}

	// Token: 0x17001302 RID: 4866
	public TValue this[TKey key]
	{
		get
		{
			return this.m_dictionary[key].Value.Value;
		}
		set
		{
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
			if (this.m_dictionary.TryGetValue(key, out linkedListNode))
			{
				linkedListNode.Value = new KeyValuePair<TKey, TValue>(key, value);
				return;
			}
			this.m_dictionary.Add(key, this.m_list.AddLast(new KeyValuePair<TKey, TValue>(key, value)));
		}
	}

	// Token: 0x17001303 RID: 4867
	// (get) Token: 0x0600A23A RID: 41530 RVA: 0x003CC8BD File Offset: 0x003CAABD
	public IEnumerable<TKey> System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Keys;
		}
	}

	// Token: 0x17001304 RID: 4868
	// (get) Token: 0x0600A23B RID: 41531 RVA: 0x003CC8C5 File Offset: 0x003CAAC5
	public IEnumerable<TValue> System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Values;
		}
	}

	// Token: 0x17001305 RID: 4869
	// (get) Token: 0x0600A23C RID: 41532 RVA: 0x003CC8CD File Offset: 0x003CAACD
	public ICollection<TKey> Keys
	{
		get
		{
			return new LinkedDictionary<TKey, TValue>.KeyCollection(this);
		}
	}

	// Token: 0x17001306 RID: 4870
	// (get) Token: 0x0600A23D RID: 41533 RVA: 0x003CC8DA File Offset: 0x003CAADA
	public ICollection<TValue> Values
	{
		get
		{
			return new LinkedDictionary<TKey, TValue>.ValueCollection(this);
		}
	}

	// Token: 0x17001307 RID: 4871
	// (get) Token: 0x0600A23E RID: 41534 RVA: 0x003CC8E7 File Offset: 0x003CAAE7
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x040079E6 RID: 31206
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> m_dictionary;

	// Token: 0x040079E7 RID: 31207
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<KeyValuePair<TKey, TValue>> m_list;

	// Token: 0x0200142B RID: 5163
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable
	{
		// Token: 0x0600A23F RID: 41535 RVA: 0x003CC8F4 File Offset: 0x003CAAF4
		public KeyCollection(LinkedDictionary<TKey, TValue> parent)
		{
			this.m_parent = parent;
		}

		// Token: 0x0600A240 RID: 41536 RVA: 0x003CC8FD File Offset: 0x003CAAFD
		public IEnumerator<TKey> GetEnumerator()
		{
			return this.m_parent.m_list.Select<KeyValuePair<TKey, TValue>, TKey>((KeyValuePair<TKey, TValue> pair) => pair.Key).GetEnumerator();
		}

		// Token: 0x0600A241 RID: 41537 RVA: 0x003CC933 File Offset: 0x003CAB33
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600A242 RID: 41538 RVA: 0x00006566 File Offset: 0x00004766
		public void Add(TKey item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600A243 RID: 41539 RVA: 0x003CC93B File Offset: 0x003CAB3B
		public void Clear()
		{
			this.m_parent.Clear();
		}

		// Token: 0x0600A244 RID: 41540 RVA: 0x003CC948 File Offset: 0x003CAB48
		public bool Contains(TKey item)
		{
			return this.m_parent.ContainsKey(item);
		}

		// Token: 0x0600A245 RID: 41541 RVA: 0x003CC958 File Offset: 0x003CAB58
		public void CopyTo(TKey[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array.Length - arrayIndex < this.m_parent.m_list.Count)
			{
				throw new ArgumentException("Not have enough space after the given arrayIndex.", "array");
			}
			LinkedListNode<KeyValuePair<TKey, TValue>> first = this.m_parent.m_list.First;
			int i = arrayIndex;
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode = first;
			while (i < array.Length)
			{
				array[arrayIndex] = linkedListNode.Value.Key;
				i++;
				linkedListNode = linkedListNode.Next;
			}
		}

		// Token: 0x0600A246 RID: 41542 RVA: 0x003CC9EC File Offset: 0x003CABEC
		public bool Remove(TKey item)
		{
			return item != null && this.m_parent.Remove(item);
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x0600A247 RID: 41543 RVA: 0x003CCA04 File Offset: 0x003CAC04
		public int Count
		{
			get
			{
				return this.m_parent.m_list.Count;
			}
		}

		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x0600A248 RID: 41544 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040079E8 RID: 31208
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedDictionary<TKey, TValue> m_parent;
	}

	// Token: 0x0200142D RID: 5165
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A24C RID: 41548 RVA: 0x003CCA2B File Offset: 0x003CAC2B
		public ValueCollection(LinkedDictionary<TKey, TValue> parent)
		{
			this.m_parent = parent;
		}

		// Token: 0x0600A24D RID: 41549 RVA: 0x003CCA34 File Offset: 0x003CAC34
		public IEnumerator<TValue> GetEnumerator()
		{
			return this.m_parent.m_list.Select<KeyValuePair<TKey, TValue>, TValue>((KeyValuePair<TKey, TValue> pair) => pair.Value).GetEnumerator();
		}

		// Token: 0x0600A24E RID: 41550 RVA: 0x003CCA6A File Offset: 0x003CAC6A
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600A24F RID: 41551 RVA: 0x00006566 File Offset: 0x00004766
		public void Add(TValue item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600A250 RID: 41552 RVA: 0x003CCA72 File Offset: 0x003CAC72
		public void Clear()
		{
			this.m_parent.Clear();
		}

		// Token: 0x0600A251 RID: 41553 RVA: 0x003CCA80 File Offset: 0x003CAC80
		public bool Contains(TValue item)
		{
			return this.m_parent.m_list.Any<KeyValuePair<TKey, TValue>>((KeyValuePair<TKey, TValue> pair) => !EqualityComparer<TValue>.Default.Equals(pair.Value, item));
		}

		// Token: 0x0600A252 RID: 41554 RVA: 0x003CCAB8 File Offset: 0x003CACB8
		public void CopyTo(TValue[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array.Length - arrayIndex < this.m_parent.m_list.Count)
			{
				throw new ArgumentException("Not have enough space after the given arrayIndex.", "array");
			}
			LinkedListNode<KeyValuePair<TKey, TValue>> first = this.m_parent.m_list.First;
			int i = arrayIndex;
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode = first;
			while (i < array.Length)
			{
				array[arrayIndex] = linkedListNode.Value.Value;
				i++;
				linkedListNode = linkedListNode.Next;
			}
		}

		// Token: 0x0600A253 RID: 41555 RVA: 0x003CCB4C File Offset: 0x003CAD4C
		public bool Remove(TValue item)
		{
			for (LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode = this.m_parent.m_list.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item))
				{
					this.m_parent.Remove(linkedListNode.Value.Key);
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x0600A254 RID: 41556 RVA: 0x003CCBB0 File Offset: 0x003CADB0
		public int Count
		{
			get
			{
				return this.m_parent.m_list.Count;
			}
		}

		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x0600A255 RID: 41557 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040079EB RID: 31211
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedDictionary<TKey, TValue> m_parent;
	}
}
