using System;
using System.Collections.Generic;

// Token: 0x020013E0 RID: 5088
public class DictionaryLinkedList<T, S>
{
	// Token: 0x06009FB4 RID: 40884 RVA: 0x003C1D22 File Offset: 0x003BFF22
	public DictionaryLinkedList()
	{
		this.dict = new Dictionary<T, S>();
	}

	// Token: 0x06009FB5 RID: 40885 RVA: 0x003C1D4B File Offset: 0x003BFF4B
	public DictionaryLinkedList(IEqualityComparer<T> _comparer)
	{
		this.dict = new Dictionary<T, S>(_comparer);
	}

	// Token: 0x06009FB6 RID: 40886 RVA: 0x003C1D78 File Offset: 0x003BFF78
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		LinkedListNode<S> linkedListNode = this.list.AddLast(_value);
		this.indices.Add(_key, linkedListNode);
	}

	// Token: 0x06009FB7 RID: 40887 RVA: 0x003C1DAC File Offset: 0x003BFFAC
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x06009FB8 RID: 40888 RVA: 0x003C1DCC File Offset: 0x003BFFCC
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

	// Token: 0x06009FB9 RID: 40889 RVA: 0x003C1E27 File Offset: 0x003C0027
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
		this.indices.Clear();
	}

	// Token: 0x170012D9 RID: 4825
	// (get) Token: 0x06009FBA RID: 40890 RVA: 0x003C1E4A File Offset: 0x003C004A
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x040078E9 RID: 30953
	public Dictionary<T, S> dict;

	// Token: 0x040078EA RID: 30954
	public LinkedList<S> list = new LinkedList<S>();

	// Token: 0x040078EB RID: 30955
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<T, LinkedListNode<S>> indices = new Dictionary<T, LinkedListNode<S>>();
}
