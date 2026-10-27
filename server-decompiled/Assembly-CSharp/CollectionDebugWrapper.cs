using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013FE RID: 5118
public class CollectionDebugWrapper<T> : EnumerableDebugWrapper<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A052 RID: 41042 RVA: 0x003C7BBB File Offset: 0x003C5DBB
	public CollectionDebugWrapper(ICollection<T> collection)
		: this(null, collection)
	{
	}

	// Token: 0x0600A053 RID: 41043 RVA: 0x003C7BC5 File Offset: 0x003C5DC5
	public CollectionDebugWrapper(DebugWrapper parent, ICollection<T> collection)
		: base(parent, collection)
	{
		this.m_collection = collection;
	}

	// Token: 0x0600A054 RID: 41044 RVA: 0x003C7BD8 File Offset: 0x003C5DD8
	public void Add(T item)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_collection.Add(item);
		}
	}

	// Token: 0x0600A055 RID: 41045 RVA: 0x003C7C14 File Offset: 0x003C5E14
	public void Clear()
	{
		using (base.DebugReadWriteScope())
		{
			this.m_collection.Clear();
		}
	}

	// Token: 0x0600A056 RID: 41046 RVA: 0x003C7C50 File Offset: 0x003C5E50
	public bool Contains(T item)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_collection.Contains(item);
		}
		return flag;
	}

	// Token: 0x0600A057 RID: 41047 RVA: 0x003C7C90 File Offset: 0x003C5E90
	public void CopyTo(T[] array, int arrayIndex)
	{
		using (base.DebugReadScope())
		{
			this.m_collection.CopyTo(array, arrayIndex);
		}
	}

	// Token: 0x0600A058 RID: 41048 RVA: 0x003C7CD0 File Offset: 0x003C5ED0
	public bool Remove(T item)
	{
		bool flag;
		using (base.DebugReadWriteScope())
		{
			flag = this.m_collection.Remove(item);
		}
		return flag;
	}

	// Token: 0x1700131E RID: 4894
	// (get) Token: 0x0600A059 RID: 41049 RVA: 0x003C7D10 File Offset: 0x003C5F10
	public int Count
	{
		get
		{
			return this.m_collection.Count;
		}
	}

	// Token: 0x1700131F RID: 4895
	// (get) Token: 0x0600A05A RID: 41050 RVA: 0x003C7D1D File Offset: 0x003C5F1D
	public bool IsReadOnly
	{
		get
		{
			return this.m_collection.IsReadOnly;
		}
	}

	// Token: 0x0400797F RID: 31103
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ICollection<T> m_collection;
}
