using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200144A RID: 5194
public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
{
	// Token: 0x0600A2D5 RID: 41685 RVA: 0x003D11AE File Offset: 0x003CF3AE
	public LinkedDictionary()
	{
		this.m_dictionary = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();
		this.m_list = new LinkedList<KeyValuePair<TKey, TValue>>();
	}

	// Token: 0x0600A2D6 RID: 41686 RVA: 0x003D11CC File Offset: 0x003CF3CC
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x0600A2D7 RID: 41687 RVA: 0x003D11DE File Offset: 0x003CF3DE
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600A2D8 RID: 41688 RVA: 0x003D11E6 File Offset: 0x003CF3E6
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A2D9 RID: 41689 RVA: 0x003D11FC File Offset: 0x003CF3FC
	public void Clear()
	{
		this.m_dictionary.Clear();
		this.m_list.Clear();
	}

	// Token: 0x0600A2DA RID: 41690 RVA: 0x003D1214 File Offset: 0x003CF414
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Contains(KeyValuePair<TKey, TValue> item)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		return this.m_dictionary.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item.Value);
	}

	// Token: 0x0600A2DB RID: 41691 RVA: 0x003D1258 File Offset: 0x003CF458
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

	// Token: 0x0600A2DC RID: 41692 RVA: 0x003D12DC File Offset: 0x003CF4DC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Remove(KeyValuePair<TKey, TValue> item)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		return this.m_dictionary.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item.Value) && this.Remove(item.Key);
	}

	// Token: 0x17001346 RID: 4934
	// (get) Token: 0x0600A2DD RID: 41693 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return false;
		}
	}

	// Token: 0x0600A2DE RID: 41694 RVA: 0x003D1331 File Offset: 0x003CF531
	public void Add(TKey key, TValue value)
	{
		if (this.m_dictionary.ContainsKey(key))
		{
			throw new ArgumentException("Already contains key.", "key");
		}
		this.m_dictionary.Add(key, this.m_list.AddLast(new KeyValuePair<TKey, TValue>(key, value)));
	}

	// Token: 0x0600A2DF RID: 41695 RVA: 0x003D136F File Offset: 0x003CF56F
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.IDictionary<TKey,TValue>.ContainsKey(TKey key)
	{
		return this.m_dictionary.ContainsKey(key);
	}

	// Token: 0x0600A2E0 RID: 41696 RVA: 0x003D1380 File Offset: 0x003CF580
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

	// Token: 0x0600A2E1 RID: 41697 RVA: 0x003D136F File Offset: 0x003CF56F
	public bool ContainsKey(TKey key)
	{
		return this.m_dictionary.ContainsKey(key);
	}

	// Token: 0x0600A2E2 RID: 41698 RVA: 0x003D13AC File Offset: 0x003CF5AC
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

	// Token: 0x17001347 RID: 4935
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

	// Token: 0x17001348 RID: 4936
	// (get) Token: 0x0600A2E5 RID: 41701 RVA: 0x003D1459 File Offset: 0x003CF659
	public IEnumerable<TKey> System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Keys;
		}
	}

	// Token: 0x17001349 RID: 4937
	// (get) Token: 0x0600A2E6 RID: 41702 RVA: 0x003D1461 File Offset: 0x003CF661
	public IEnumerable<TValue> System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Values;
		}
	}

	// Token: 0x1700134A RID: 4938
	// (get) Token: 0x0600A2E7 RID: 41703 RVA: 0x003D1469 File Offset: 0x003CF669
	public ICollection<TKey> Keys
	{
		get
		{
			return new LinkedDictionary<TKey, TValue>.KeyCollection(this);
		}
	}

	// Token: 0x1700134B RID: 4939
	// (get) Token: 0x0600A2E8 RID: 41704 RVA: 0x003D1476 File Offset: 0x003CF676
	public ICollection<TValue> Values
	{
		get
		{
			return new LinkedDictionary<TKey, TValue>.ValueCollection(this);
		}
	}

	// Token: 0x1700134C RID: 4940
	// (get) Token: 0x0600A2E9 RID: 41705 RVA: 0x003D1483 File Offset: 0x003CF683
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x04007A61 RID: 31329
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> m_dictionary;

	// Token: 0x04007A62 RID: 31330
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<KeyValuePair<TKey, TValue>> m_list;

	// Token: 0x0200144B RID: 5195
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable
	{
		// Token: 0x0600A2EA RID: 41706 RVA: 0x003D1490 File Offset: 0x003CF690
		public KeyCollection(LinkedDictionary<TKey, TValue> parent)
		{
			this.m_parent = parent;
		}

		// Token: 0x0600A2EB RID: 41707 RVA: 0x003D1499 File Offset: 0x003CF699
		public IEnumerator<TKey> GetEnumerator()
		{
			return this.m_parent.m_list.Select<KeyValuePair<TKey, TValue>, TKey>((KeyValuePair<TKey, TValue> pair) => pair.Key).GetEnumerator();
		}

		// Token: 0x0600A2EC RID: 41708 RVA: 0x003D14CF File Offset: 0x003CF6CF
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600A2ED RID: 41709 RVA: 0x00006566 File Offset: 0x00004766
		public void Add(TKey item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600A2EE RID: 41710 RVA: 0x003D14D7 File Offset: 0x003CF6D7
		public void Clear()
		{
			this.m_parent.Clear();
		}

		// Token: 0x0600A2EF RID: 41711 RVA: 0x003D14E4 File Offset: 0x003CF6E4
		public bool Contains(TKey item)
		{
			return this.m_parent.ContainsKey(item);
		}

		// Token: 0x0600A2F0 RID: 41712 RVA: 0x003D14F4 File Offset: 0x003CF6F4
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

		// Token: 0x0600A2F1 RID: 41713 RVA: 0x003D1588 File Offset: 0x003CF788
		public bool Remove(TKey item)
		{
			return item != null && this.m_parent.Remove(item);
		}

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x0600A2F2 RID: 41714 RVA: 0x003D15A0 File Offset: 0x003CF7A0
		public int Count
		{
			get
			{
				return this.m_parent.m_list.Count;
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x0600A2F3 RID: 41715 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04007A63 RID: 31331
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedDictionary<TKey, TValue> m_parent;
	}

	// Token: 0x0200144D RID: 5197
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A2F7 RID: 41719 RVA: 0x003D15C7 File Offset: 0x003CF7C7
		public ValueCollection(LinkedDictionary<TKey, TValue> parent)
		{
			this.m_parent = parent;
		}

		// Token: 0x0600A2F8 RID: 41720 RVA: 0x003D15D0 File Offset: 0x003CF7D0
		public IEnumerator<TValue> GetEnumerator()
		{
			return this.m_parent.m_list.Select<KeyValuePair<TKey, TValue>, TValue>((KeyValuePair<TKey, TValue> pair) => pair.Value).GetEnumerator();
		}

		// Token: 0x0600A2F9 RID: 41721 RVA: 0x003D1606 File Offset: 0x003CF806
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600A2FA RID: 41722 RVA: 0x00006566 File Offset: 0x00004766
		public void Add(TValue item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600A2FB RID: 41723 RVA: 0x003D160E File Offset: 0x003CF80E
		public void Clear()
		{
			this.m_parent.Clear();
		}

		// Token: 0x0600A2FC RID: 41724 RVA: 0x003D161C File Offset: 0x003CF81C
		public bool Contains(TValue item)
		{
			return this.m_parent.m_list.Any<KeyValuePair<TKey, TValue>>((KeyValuePair<TKey, TValue> pair) => !EqualityComparer<TValue>.Default.Equals(pair.Value, item));
		}

		// Token: 0x0600A2FD RID: 41725 RVA: 0x003D1654 File Offset: 0x003CF854
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

		// Token: 0x0600A2FE RID: 41726 RVA: 0x003D16E8 File Offset: 0x003CF8E8
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

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x0600A2FF RID: 41727 RVA: 0x003D174C File Offset: 0x003CF94C
		public int Count
		{
			get
			{
				return this.m_parent.m_list.Count;
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x0600A300 RID: 41728 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04007A66 RID: 31334
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedDictionary<TKey, TValue> m_parent;
	}
}
