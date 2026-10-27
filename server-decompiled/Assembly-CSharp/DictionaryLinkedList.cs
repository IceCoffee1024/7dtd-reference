using System;
using System.Collections.Generic;

// Token: 0x02001412 RID: 5138
public class DictionaryLinkedList<T, S>
{
	// Token: 0x0600A0B7 RID: 41143 RVA: 0x003C8CC2 File Offset: 0x003C6EC2
	public DictionaryLinkedList()
	{
		this.dict = new Dictionary<T, S>();
	}

	// Token: 0x0600A0B8 RID: 41144 RVA: 0x003C8CEB File Offset: 0x003C6EEB
	public DictionaryLinkedList(IEqualityComparer<T> _comparer)
	{
		this.dict = new Dictionary<T, S>(_comparer);
	}

	// Token: 0x0600A0B9 RID: 41145 RVA: 0x003C8D18 File Offset: 0x003C6F18
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		LinkedListNode<S> linkedListNode = this.list.AddLast(_value);
		this.indices.Add(_key, linkedListNode);
	}

	// Token: 0x0600A0BA RID: 41146 RVA: 0x003C8D4C File Offset: 0x003C6F4C
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x0600A0BB RID: 41147 RVA: 0x003C8D6C File Offset: 0x003C6F6C
	public void Remove(T _key)
	{
		if (this.dict.ContainsKey(_key))
		{
			S s = this.dict[_key];
			this.dict.Remove(_key);
			LinkedListNode<S> linkedListNode = this.indices[_key];
			this.list.Remove(linkedListNode);
			this.indices.Remove(_key);
		}
	}

	// Token: 0x0600A0BC RID: 41148 RVA: 0x003C8DC7 File Offset: 0x003C6FC7
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
		this.indices.Clear();
	}

	// Token: 0x1700132A RID: 4906
	// (get) Token: 0x0600A0BD RID: 41149 RVA: 0x003C8DEA File Offset: 0x003C6FEA
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x040079A5 RID: 31141
	public Dictionary<T, S> dict;

	// Token: 0x040079A6 RID: 31142
	public LinkedList<S> list = new LinkedList<S>();

	// Token: 0x040079A7 RID: 31143
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<T, LinkedListNode<S>> indices = new Dictionary<T, LinkedListNode<S>>();
}
