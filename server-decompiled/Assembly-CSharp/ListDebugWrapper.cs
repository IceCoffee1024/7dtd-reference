using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013D7 RID: 5079
public sealed class ListDebugWrapper<T> : CollectionDebugWrapper<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x06009F8C RID: 40844 RVA: 0x003C147B File Offset: 0x003BF67B
	public ListDebugWrapper()
		: this(new List<T>())
	{
	}

	// Token: 0x06009F8D RID: 40845 RVA: 0x003C1488 File Offset: 0x003BF688
	public ListDebugWrapper(IList<T> list)
		: this(null, list)
	{
	}

	// Token: 0x06009F8E RID: 40846 RVA: 0x003C1492 File Offset: 0x003BF692
	public ListDebugWrapper(DebugWrapper parent, IList<T> list)
		: base(parent, list)
	{
		this.m_list = list;
	}

	// Token: 0x06009F8F RID: 40847 RVA: 0x003C14A4 File Offset: 0x003BF6A4
	public int IndexOf(T item)
	{
		int num;
		using (base.DebugReadScope())
		{
			num = this.m_list.IndexOf(item);
		}
		return num;
	}

	// Token: 0x06009F90 RID: 40848 RVA: 0x003C14E4 File Offset: 0x003BF6E4
	public void Insert(int index, T item)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_list.Insert(index, item);
		}
	}

	// Token: 0x06009F91 RID: 40849 RVA: 0x003C1524 File Offset: 0x003BF724
	public void RemoveAt(int index)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_list.RemoveAt(index);
		}
	}

	// Token: 0x170012D8 RID: 4824
	public T this[int index]
	{
		get
		{
			T t;
			using (base.DebugReadScope())
			{
				t = this.m_list[index];
			}
			return t;
		}
		set
		{
			using (base.DebugReadWriteScope())
			{
				this.m_list[index] = value;
			}
		}
	}

	// Token: 0x040078D8 RID: 30936
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IList<T> m_list;
}
