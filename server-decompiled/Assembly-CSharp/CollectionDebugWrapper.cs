using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013EC RID: 5100
public class CollectionDebugWrapper<T> : EnumerableDebugWrapper<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x06009FFA RID: 40954 RVA: 0x003C57B7 File Offset: 0x003C39B7
	public CollectionDebugWrapper(ICollection<T> collection)
		: this(null, collection)
	{
	}

	// Token: 0x06009FFB RID: 40955 RVA: 0x003C57C1 File Offset: 0x003C39C1
	public CollectionDebugWrapper(DebugWrapper parent, ICollection<T> collection)
		: base(parent, collection)
	{
		this.m_collection = collection;
	}

	// Token: 0x06009FFC RID: 40956 RVA: 0x003C57D4 File Offset: 0x003C39D4
	public void Add(T item)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_collection.Add(item);
		}
	}

	// Token: 0x06009FFD RID: 40957 RVA: 0x003C5810 File Offset: 0x003C3A10
	public void Clear()
	{
		using (base.DebugReadWriteScope())
		{
			this.m_collection.Clear();
		}
	}

	// Token: 0x06009FFE RID: 40958 RVA: 0x003C584C File Offset: 0x003C3A4C
	public bool Contains(T item)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_collection.Contains(item);
		}
		return flag;
	}

	// Token: 0x06009FFF RID: 40959 RVA: 0x003C588C File Offset: 0x003C3A8C
	public void CopyTo(T[] array, int arrayIndex)
	{
		using (base.DebugReadScope())
		{
			this.m_collection.CopyTo(array, arrayIndex);
		}
	}

	// Token: 0x0600A000 RID: 40960 RVA: 0x003C58CC File Offset: 0x003C3ACC
	public bool Remove(T item)
	{
		bool flag;
		using (base.DebugReadWriteScope())
		{
			flag = this.m_collection.Remove(item);
		}
		return flag;
	}

	// Token: 0x17001312 RID: 4882
	// (get) Token: 0x0600A001 RID: 40961 RVA: 0x003C590C File Offset: 0x003C3B0C
	public int Count
	{
		get
		{
			return this.m_collection.Count;
		}
	}

	// Token: 0x17001313 RID: 4883
	// (get) Token: 0x0600A002 RID: 40962 RVA: 0x003C5919 File Offset: 0x003C3B19
	public bool IsReadOnly
	{
		get
		{
			return this.m_collection.IsReadOnly;
		}
	}

	// Token: 0x0400793E RID: 31038
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ICollection<T> m_collection;
}
