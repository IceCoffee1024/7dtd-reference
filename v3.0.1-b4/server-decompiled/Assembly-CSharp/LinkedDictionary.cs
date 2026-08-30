using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200140F RID: 5135
public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
{
	// Token: 0x0600A12D RID: 41261 RVA: 0x003C72CE File Offset: 0x003C54CE
	public LinkedDictionary()
	{
		this.m_dictionary = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();
		this.m_list = new LinkedList<KeyValuePair<TKey, TValue>>();
	}

	// Token: 0x0600A12E RID: 41262 RVA: 0x003C72EC File Offset: 0x003C54EC
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x0600A12F RID: 41263 RVA: 0x003C72FE File Offset: 0x003C54FE
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600A130 RID: 41264 RVA: 0x003C7306 File Offset: 0x003C5506
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A131 RID: 41265 RVA: 0x003C731C File Offset: 0x003C551C
	public void Clear()
	{
		this.m_dictionary.Clear();
		this.m_list.Clear();
	}

	// Token: 0x0600A132 RID: 41266 RVA: 0x003C7334 File Offset: 0x003C5534
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Contains(KeyValuePair<TKey, TValue> item)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		return this.m_dictionary.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item.Value);
	}

	// Token: 0x0600A133 RID: 41267 RVA: 0x003C7378 File Offset: 0x003C5578
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

	// Token: 0x0600A134 RID: 41268 RVA: 0x003C73FC File Offset: 0x003C55FC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Remove(KeyValuePair<TKey, TValue> item)
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
		return this.m_dictionary.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(linkedListNode.Value.Value, item.Value) && this.Remove(item.Key);
	}

	// Token: 0x170012C8 RID: 4808
	// (get) Token: 0x0600A135 RID: 41269 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return false;
		}
	}

	// Token: 0x0600A136 RID: 41270 RVA: 0x003C7451 File Offset: 0x003C5651
	public void Add(TKey key, TValue value)
	{
		if (this.m_dictionary.ContainsKey(key))
		{
			throw new ArgumentException("Already contains key.", "key");
		}
		this.m_dictionary.Add(key, this.m_list.AddLast(new KeyValuePair<TKey, TValue>(key, value)));
	}

	// Token: 0x0600A137 RID: 41271 RVA: 0x003C748F File Offset: 0x003C568F
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.IDictionary<TKey,TValue>.ContainsKey(TKey key)
	{
		return this.m_dictionary.ContainsKey(key);
	}

	// Token: 0x0600A138 RID: 41272 RVA: 0x003C74A0 File Offset: 0x003C56A0
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

	// Token: 0x0600A139 RID: 41273 RVA: 0x003C748F File Offset: 0x003C568F
	public bool ContainsKey(TKey key)
	{
		return this.m_dictionary.ContainsKey(key);
	}

	// Token: 0x0600A13A RID: 41274 RVA: 0x003C74CC File Offset: 0x003C56CC
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

	// Token: 0x170012C9 RID: 4809
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

	// Token: 0x170012CA RID: 4810
	// (get) Token: 0x0600A13D RID: 41277 RVA: 0x003C7579 File Offset: 0x003C5779
	public IEnumerable<TKey> System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Keys;
		}
	}

	// Token: 0x170012CB RID: 4811
	// (get) Token: 0x0600A13E RID: 41278 RVA: 0x003C7581 File Offset: 0x003C5781
	public IEnumerable<TValue> System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Values;
		}
	}

	// Token: 0x170012CC RID: 4812
	// (get) Token: 0x0600A13F RID: 41279 RVA: 0x003C7589 File Offset: 0x003C5789
	public ICollection<TKey> Keys
	{
		get
		{
			return new LinkedDictionary<TKey, TValue>.KeyCollection(this);
		}
	}

	// Token: 0x170012CD RID: 4813
	// (get) Token: 0x0600A140 RID: 41280 RVA: 0x003C7596 File Offset: 0x003C5796
	public ICollection<TValue> Values
	{
		get
		{
			return new LinkedDictionary<TKey, TValue>.ValueCollection(this);
		}
	}

	// Token: 0x170012CE RID: 4814
	// (get) Token: 0x0600A141 RID: 41281 RVA: 0x003C75A3 File Offset: 0x003C57A3
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x0400790D RID: 30989
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> m_dictionary;

	// Token: 0x0400790E RID: 30990
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<KeyValuePair<TKey, TValue>> m_list;

	// Token: 0x02001410 RID: 5136
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable
	{
		// Token: 0x0600A142 RID: 41282 RVA: 0x003C75B0 File Offset: 0x003C57B0
		public KeyCollection(LinkedDictionary<TKey, TValue> parent)
		{
			this.m_parent = parent;
		}

		// Token: 0x0600A143 RID: 41283 RVA: 0x003C75B9 File Offset: 0x003C57B9
		public IEnumerator<TKey> GetEnumerator()
		{
			return this.m_parent.m_list.Select<KeyValuePair<TKey, TValue>, TKey>((KeyValuePair<TKey, TValue> pair) => pair.Key).GetEnumerator();
		}

		// Token: 0x0600A144 RID: 41284 RVA: 0x003C75EF File Offset: 0x003C57EF
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600A145 RID: 41285 RVA: 0x00006566 File Offset: 0x00004766
		public void Add(TKey item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600A146 RID: 41286 RVA: 0x003C75F7 File Offset: 0x003C57F7
		public void Clear()
		{
			this.m_parent.Clear();
		}

		// Token: 0x0600A147 RID: 41287 RVA: 0x003C7604 File Offset: 0x003C5804
		public bool Contains(TKey item)
		{
			return this.m_parent.ContainsKey(item);
		}

		// Token: 0x0600A148 RID: 41288 RVA: 0x003C7614 File Offset: 0x003C5814
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

		// Token: 0x0600A149 RID: 41289 RVA: 0x003C76A8 File Offset: 0x003C58A8
		public bool Remove(TKey item)
		{
			return item != null && this.m_parent.Remove(item);
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x0600A14A RID: 41290 RVA: 0x003C76C0 File Offset: 0x003C58C0
		public int Count
		{
			get
			{
				return this.m_parent.m_list.Count;
			}
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x0600A14B RID: 41291 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400790F RID: 30991
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedDictionary<TKey, TValue> m_parent;
	}

	// Token: 0x02001412 RID: 5138
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x0600A14F RID: 41295 RVA: 0x003C76E7 File Offset: 0x003C58E7
		public ValueCollection(LinkedDictionary<TKey, TValue> parent)
		{
			this.m_parent = parent;
		}

		// Token: 0x0600A150 RID: 41296 RVA: 0x003C76F0 File Offset: 0x003C58F0
		public IEnumerator<TValue> GetEnumerator()
		{
			return this.m_parent.m_list.Select<KeyValuePair<TKey, TValue>, TValue>((KeyValuePair<TKey, TValue> pair) => pair.Value).GetEnumerator();
		}

		// Token: 0x0600A151 RID: 41297 RVA: 0x003C7726 File Offset: 0x003C5926
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600A152 RID: 41298 RVA: 0x00006566 File Offset: 0x00004766
		public void Add(TValue item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600A153 RID: 41299 RVA: 0x003C772E File Offset: 0x003C592E
		public void Clear()
		{
			this.m_parent.Clear();
		}

		// Token: 0x0600A154 RID: 41300 RVA: 0x003C773C File Offset: 0x003C593C
		public bool Contains(TValue item)
		{
			return this.m_parent.m_list.Any<KeyValuePair<TKey, TValue>>((KeyValuePair<TKey, TValue> pair) => !EqualityComparer<TValue>.Default.Equals(pair.Value, item));
		}

		// Token: 0x0600A155 RID: 41301 RVA: 0x003C7774 File Offset: 0x003C5974
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

		// Token: 0x0600A156 RID: 41302 RVA: 0x003C7808 File Offset: 0x003C5A08
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

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x0600A157 RID: 41303 RVA: 0x003C786C File Offset: 0x003C5A6C
		public int Count
		{
			get
			{
				return this.m_parent.m_list.Count;
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x0600A158 RID: 41304 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04007912 RID: 30994
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedDictionary<TKey, TValue> m_parent;
	}
}
