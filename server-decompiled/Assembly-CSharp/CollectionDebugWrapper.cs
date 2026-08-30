using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013B1 RID: 5041
public class CollectionDebugWrapper<T> : EnumerableDebugWrapper<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x06009E52 RID: 40530 RVA: 0x003BB8D7 File Offset: 0x003B9AD7
	public CollectionDebugWrapper(ICollection<T> collection)
		: this(null, collection)
	{
	}

	// Token: 0x06009E53 RID: 40531 RVA: 0x003BB8E1 File Offset: 0x003B9AE1
	public CollectionDebugWrapper(DebugWrapper parent, ICollection<T> collection)
		: base(parent, collection)
	{
		this.m_collection = collection;
	}

	// Token: 0x06009E54 RID: 40532 RVA: 0x003BB8F4 File Offset: 0x003B9AF4
	public void Add(T item)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_collection.Add(item);
		}
	}

	// Token: 0x06009E55 RID: 40533 RVA: 0x003BB930 File Offset: 0x003B9B30
	public void Clear()
	{
		using (base.DebugReadWriteScope())
		{
			this.m_collection.Clear();
		}
	}

	// Token: 0x06009E56 RID: 40534 RVA: 0x003BB96C File Offset: 0x003B9B6C
	public bool Contains(T item)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_collection.Contains(item);
		}
		return flag;
	}

	// Token: 0x06009E57 RID: 40535 RVA: 0x003BB9AC File Offset: 0x003B9BAC
	public void CopyTo(T[] array, int arrayIndex)
	{
		using (base.DebugReadScope())
		{
			this.m_collection.CopyTo(array, arrayIndex);
		}
	}

	// Token: 0x06009E58 RID: 40536 RVA: 0x003BB9EC File Offset: 0x003B9BEC
	public bool Remove(T item)
	{
		bool flag;
		using (base.DebugReadWriteScope())
		{
			flag = this.m_collection.Remove(item);
		}
		return flag;
	}

	// Token: 0x17001294 RID: 4756
	// (get) Token: 0x06009E59 RID: 40537 RVA: 0x003BBA2C File Offset: 0x003B9C2C
	public int Count
	{
		get
		{
			return this.m_collection.Count;
		}
	}

	// Token: 0x17001295 RID: 4757
	// (get) Token: 0x06009E5A RID: 40538 RVA: 0x003BBA39 File Offset: 0x003B9C39
	public bool IsReadOnly
	{
		get
		{
			return this.m_collection.IsReadOnly;
		}
	}

	// Token: 0x040077EA RID: 30698
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ICollection<T> m_collection;
}
