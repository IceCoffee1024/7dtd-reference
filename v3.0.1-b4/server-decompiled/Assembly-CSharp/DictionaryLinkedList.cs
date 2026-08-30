using System;
using System.Collections.Generic;

// Token: 0x020013C5 RID: 5061
public class DictionaryLinkedList<T, S>
{
	// Token: 0x06009EB7 RID: 40631 RVA: 0x003BC9DE File Offset: 0x003BABDE
	public DictionaryLinkedList()
	{
		this.dict = new Dictionary<T, S>();
	}

	// Token: 0x06009EB8 RID: 40632 RVA: 0x003BCA07 File Offset: 0x003BAC07
	public DictionaryLinkedList(IEqualityComparer<T> _comparer)
	{
		this.dict = new Dictionary<T, S>(_comparer);
	}

	// Token: 0x06009EB9 RID: 40633 RVA: 0x003BCA34 File Offset: 0x003BAC34
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		LinkedListNode<S> linkedListNode = this.list.AddLast(_value);
		this.indices.Add(_key, linkedListNode);
	}

	// Token: 0x06009EBA RID: 40634 RVA: 0x003BCA68 File Offset: 0x003BAC68
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x06009EBB RID: 40635 RVA: 0x003BCA88 File Offset: 0x003BAC88
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

	// Token: 0x06009EBC RID: 40636 RVA: 0x003BCAE3 File Offset: 0x003BACE3
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
		this.indices.Clear();
	}

	// Token: 0x170012A0 RID: 4768
	// (get) Token: 0x06009EBD RID: 40637 RVA: 0x003BCB06 File Offset: 0x003BAD06
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x04007810 RID: 30736
	public Dictionary<T, S> dict;

	// Token: 0x04007811 RID: 30737
	public LinkedList<S> list = new LinkedList<S>();

	// Token: 0x04007812 RID: 30738
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<T, LinkedListNode<S>> indices = new Dictionary<T, LinkedListNode<S>>();
}
