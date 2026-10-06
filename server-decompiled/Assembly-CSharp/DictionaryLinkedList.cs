using System;
using System.Collections.Generic;

// Token: 0x02001400 RID: 5120
public class DictionaryLinkedList<T, S>
{
	// Token: 0x0600A05F RID: 41055 RVA: 0x003C68BE File Offset: 0x003C4ABE
	public DictionaryLinkedList()
	{
		this.dict = new Dictionary<T, S>();
	}

	// Token: 0x0600A060 RID: 41056 RVA: 0x003C68E7 File Offset: 0x003C4AE7
	public DictionaryLinkedList(IEqualityComparer<T> _comparer)
	{
		this.dict = new Dictionary<T, S>(_comparer);
	}

	// Token: 0x0600A061 RID: 41057 RVA: 0x003C6914 File Offset: 0x003C4B14
	public void Add(T _key, S _value)
	{
		this.dict.Add(_key, _value);
		LinkedListNode<S> linkedListNode = this.list.AddLast(_value);
		this.indices.Add(_key, linkedListNode);
	}

	// Token: 0x0600A062 RID: 41058 RVA: 0x003C6948 File Offset: 0x003C4B48
	public void Set(T _key, S _value)
	{
		if (this.dict.ContainsKey(_key))
		{
			this.Remove(_key);
		}
		this.Add(_key, _value);
	}

	// Token: 0x0600A063 RID: 41059 RVA: 0x003C6968 File Offset: 0x003C4B68
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

	// Token: 0x0600A064 RID: 41060 RVA: 0x003C69C3 File Offset: 0x003C4BC3
	public void Clear()
	{
		this.list.Clear();
		this.dict.Clear();
		this.indices.Clear();
	}

	// Token: 0x1700131E RID: 4894
	// (get) Token: 0x0600A065 RID: 41061 RVA: 0x003C69E6 File Offset: 0x003C4BE6
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x04007964 RID: 31076
	public Dictionary<T, S> dict;

	// Token: 0x04007965 RID: 31077
	public LinkedList<S> list = new LinkedList<S>();

	// Token: 0x04007966 RID: 31078
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<T, LinkedListNode<S>> indices = new Dictionary<T, LinkedListNode<S>>();
}
