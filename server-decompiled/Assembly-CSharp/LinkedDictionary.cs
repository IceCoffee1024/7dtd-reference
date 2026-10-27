using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200145C RID: 5212
public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
{
	// Token: 0x0600A32D RID: 41773 RVA: 0x003D35B2 File Offset: 0x003D17B2
	public LinkedDictionary()
	{
		this.m_dictionary = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();
		this.m_list = new LinkedList<KeyValuePair<TKey, TValue>>();
	}

	// Token: 0x0600A32E RID: 41774 RVA: 0x003D35D0 File Offset: 0x003D17D0
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x0600A32F RID: 41775 RVA: 0x003D35E2 File Offset: 0x003D17E2
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600A330 RID: 41776 RVA: 0x003D35EA File Offset: 0x003D17EA
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A331 RID: 41777 RVA: 0x003D3600 File Offset: 0x003D1800
	public void Clear()
	{
		this.m_dictionary.Clear();
		this.m_list.Clear();
	}

	// Token: 0x0600A332 RID: 41778 RVA: 0x003D3618 File Offset: 0x003D1818
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Contains(KeyValuePair<TKey, TValue> item)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		return this.m_dictionary.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item.Value);
	}

	// Token: 0x0600A333 RID: 41779 RVA: 0x003D365C File Offset: 0x003D185C
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

	// Token: 0x0600A334 RID: 41780 RVA: 0x003D36E0 File Offset: 0x003D18E0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Remove(KeyValuePair<TKey, TValue> item)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		return this.m_dictionary.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item.Value) && this.Remove(item.Key);
	}

	// Token: 0x17001352 RID: 4946
	// (get) Token: 0x0600A335 RID: 41781 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return false;
		}
	}

	// Token: 0x0600A336 RID: 41782 RVA: 0x003D3735 File Offset: 0x003D1935
	public void Add(TKey key, TValue value)
	{
		if (this.m_dictionary.ContainsKey(key))
		{
			throw new ArgumentException("Already contains key.", "key");
		}
		this.m_dictionary.Add(key, this.m_list.AddLast(new KeyValuePair<TKey, TValue>(key, value)));
	}

	// Token: 0x0600A337 RID: 41783 RVA: 0x003D3773 File Offset: 0x003D1973
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.IDictionary<TKey,TValue>.ContainsKey(TKey key)
	{
		return this.m_dictionary.ContainsKey(key);
	}

	// Token: 0x0600A338 RID: 41784 RVA: 0x003D3784 File Offset: 0x003D1984
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

	// Token: 0x0600A339 RID: 41785 RVA: 0x003D3773 File Offset: 0x003D1973
	public bool ContainsKey(TKey key)
	{
		return this.m_dictionary.ContainsKey(key);
	}

	// Token: 0x0600A33A RID: 41786 RVA: 0x003D37B0 File Offset: 0x003D19B0
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

	// Token: 0x17001353 RID: 4947
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

	// Token: 0x17001354 RID: 4948
	// (get) Token: 0x0600A33D RID: 41789 RVA: 0x003D385D File Offset: 0x003D1A5D
	public IEnumerable<TKey> System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Keys;
		}
	}

	// Token: 0x17001355 RID: 4949
	// (get) Token: 0x0600A33E RID: 41790 RVA: 0x003D3865 File Offset: 0x003D1A65
	public IEnumerable<TValue> System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Values;
		}
	}

	// Token: 0x17001356 RID: 4950
	// (get) Token: 0x0600A33F RID: 41791 RVA: 0x003D386D File Offset: 0x003D1A6D
	public ICollection<TKey> Keys
	{
		get
		{
			return new LinkedDictionary<TKey, TValue>.KeyCollection(this);
		}
	}

	// Token: 0x17001357 RID: 4951
	// (get) Token: 0x0600A340 RID: 41792 RVA: 0x003D387A File Offset: 0x003D1A7A
	public ICollection<TValue> Values
	{
		get
		{
			return new LinkedDictionary<TKey, TValue>.ValueCollection(this);
		}
	}

	// Token: 0x17001358 RID: 4952
	// (get) Token: 0x0600A341 RID: 41793 RVA: 0x003D3887 File Offset: 0x003D1A87
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x04007AA2 RID: 31394
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> m_dictionary;

	// Token: 0x04007AA3 RID: 31395
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<KeyValuePair<TKey, TValue>> m_list;

	// Token: 0x0200145D RID: 5213
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable
	{
		// Token: 0x0600A342 RID: 41794 RVA: 0x003D3894 File Offset: 0x003D1A94
		public KeyCollection(LinkedDictionary<TKey, TValue> parent)
		{
			this.m_parent = parent;
		}

		// Token: 0x0600A343 RID: 41795 RVA: 0x003D389D File Offset: 0x003D1A9D
		public IEnumerator<TKey> GetEnumerator()
		{
			return this.m_parent.m_list.Select<KeyValuePair<TKey, TValue>, TKey>((KeyValuePair<TKey, TValue> pair) => pair.Key).GetEnumerator();
		}

		// Token: 0x0600A344 RID: 41796 RVA: 0x003D38D3 File Offset: 0x003D1AD3
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600A345 RID: 41797 RVA: 0x00006566 File Offset: 0x00004766
		public void Add(TKey item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600A346 RID: 41798 RVA: 0x003D38DB File Offset: 0x003D1ADB
		public void Clear()
		{
			this.m_parent.Clear();
		}

		// Token: 0x0600A347 RID: 41799 RVA: 0x003D38E8 File Offset: 0x003D1AE8
		public bool Contains(TKey item)
		{
			return this.m_parent.ContainsKey(item);
		}

		// Token: 0x0600A348 RID: 41800 RVA: 0x003D38F8 File Offset: 0x003D1AF8
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

		// Token: 0x0600A349 RID: 41801 RVA: 0x003D398C File Offset: 0x003D1B8C
		public bool Remove(TKey item)
		{
			return item != null && this.m_parent.Remove(item);
		}

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x0600A34A RID: 41802 RVA: 0x003D39A4 File Offset: 0x003D1BA4
		public int Count
		{
			get
			{
				return this.m_parent.m_list.Count;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x0600A34B RID: 41803 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04007AA4 RID: 31396
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedDictionary<TKey, TValue> m_parent;
	}

	// Token: 0x0200145F RID: 5215
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A34F RID: 41807 RVA: 0x003D39CB File Offset: 0x003D1BCB
		public ValueCollection(LinkedDictionary<TKey, TValue> parent)
		{
			this.m_parent = parent;
		}

		// Token: 0x0600A350 RID: 41808 RVA: 0x003D39D4 File Offset: 0x003D1BD4
		public IEnumerator<TValue> GetEnumerator()
		{
			return this.m_parent.m_list.Select<KeyValuePair<TKey, TValue>, TValue>((KeyValuePair<TKey, TValue> pair) => pair.Value).GetEnumerator();
		}

		// Token: 0x0600A351 RID: 41809 RVA: 0x003D3A0A File Offset: 0x003D1C0A
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600A352 RID: 41810 RVA: 0x00006566 File Offset: 0x00004766
		public void Add(TValue item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600A353 RID: 41811 RVA: 0x003D3A12 File Offset: 0x003D1C12
		public void Clear()
		{
			this.m_parent.Clear();
		}

		// Token: 0x0600A354 RID: 41812 RVA: 0x003D3A20 File Offset: 0x003D1C20
		public bool Contains(TValue item)
		{
			return this.m_parent.m_list.Any<KeyValuePair<TKey, TValue>>((KeyValuePair<TKey, TValue> pair) => !EqualityComparer<TValue>.Default.Equals(pair.Value, item));
		}

		// Token: 0x0600A355 RID: 41813 RVA: 0x003D3A58 File Offset: 0x003D1C58
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

		// Token: 0x0600A356 RID: 41814 RVA: 0x003D3AEC File Offset: 0x003D1CEC
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

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x0600A357 RID: 41815 RVA: 0x003D3B50 File Offset: 0x003D1D50
		public int Count
		{
			get
			{
				return this.m_parent.m_list.Count;
			}
		}

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x0600A358 RID: 41816 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04007AA7 RID: 31399
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedDictionary<TKey, TValue> m_parent;
	}
}
