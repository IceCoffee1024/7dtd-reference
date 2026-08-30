using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013CC RID: 5068
public class CollectionDebugWrapper<T> : EnumerableDebugWrapper<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x06009F4F RID: 40783 RVA: 0x003C0C1B File Offset: 0x003BEE1B
	public CollectionDebugWrapper(ICollection<T> collection)
		: this(null, collection)
	{
	}

	// Token: 0x06009F50 RID: 40784 RVA: 0x003C0C25 File Offset: 0x003BEE25
	public CollectionDebugWrapper(DebugWrapper parent, ICollection<T> collection)
		: base(parent, collection)
	{
		this.m_collection = collection;
	}

	// Token: 0x06009F51 RID: 40785 RVA: 0x003C0C38 File Offset: 0x003BEE38
	public void Add(T item)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_collection.Add(item);
		}
	}

	// Token: 0x06009F52 RID: 40786 RVA: 0x003C0C74 File Offset: 0x003BEE74
	public void Clear()
	{
		using (base.DebugReadWriteScope())
		{
			this.m_collection.Clear();
		}
	}

	// Token: 0x06009F53 RID: 40787 RVA: 0x003C0CB0 File Offset: 0x003BEEB0
	public bool Contains(T item)
	{
		bool flag;
		using (base.DebugReadScope())
		{
			flag = this.m_collection.Contains(item);
		}
		return flag;
	}

	// Token: 0x06009F54 RID: 40788 RVA: 0x003C0CF0 File Offset: 0x003BEEF0
	public void CopyTo(T[] array, int arrayIndex)
	{
		using (base.DebugReadScope())
		{
			this.m_collection.CopyTo(array, arrayIndex);
		}
	}

	// Token: 0x06009F55 RID: 40789 RVA: 0x003C0D30 File Offset: 0x003BEF30
	public bool Remove(T item)
	{
		bool flag;
		using (base.DebugReadWriteScope())
		{
			flag = this.m_collection.Remove(item);
		}
		return flag;
	}

	// Token: 0x170012CD RID: 4813
	// (get) Token: 0x06009F56 RID: 40790 RVA: 0x003C0D70 File Offset: 0x003BEF70
	public int Count
	{
		get
		{
			return this.m_collection.Count;
		}
	}

	// Token: 0x170012CE RID: 4814
	// (get) Token: 0x06009F57 RID: 40791 RVA: 0x003C0D7D File Offset: 0x003BEF7D
	public bool IsReadOnly
	{
		get
		{
			return this.m_collection.IsReadOnly;
		}
	}

	// Token: 0x040078C3 RID: 30915
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ICollection<T> m_collection;
}
