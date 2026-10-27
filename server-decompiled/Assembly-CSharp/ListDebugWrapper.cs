using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001409 RID: 5129
public sealed class ListDebugWrapper<T> : CollectionDebugWrapper<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A08F RID: 41103 RVA: 0x003C841B File Offset: 0x003C661B
	public ListDebugWrapper()
		: this(new List<T>())
	{
	}

	// Token: 0x0600A090 RID: 41104 RVA: 0x003C8428 File Offset: 0x003C6628
	public ListDebugWrapper(IList<T> list)
		: this(null, list)
	{
	}

	// Token: 0x0600A091 RID: 41105 RVA: 0x003C8432 File Offset: 0x003C6632
	public ListDebugWrapper(DebugWrapper parent, IList<T> list)
		: base(parent, list)
	{
		this.m_list = list;
	}

	// Token: 0x0600A092 RID: 41106 RVA: 0x003C8444 File Offset: 0x003C6644
	public int IndexOf(T item)
	{
		int num;
		using (base.DebugReadScope())
		{
			num = this.m_list.IndexOf(item);
		}
		return num;
	}

	// Token: 0x0600A093 RID: 41107 RVA: 0x003C8484 File Offset: 0x003C6684
	public void Insert(int index, T item)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_list.Insert(index, item);
		}
	}

	// Token: 0x0600A094 RID: 41108 RVA: 0x003C84C4 File Offset: 0x003C66C4
	public void RemoveAt(int index)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_list.RemoveAt(index);
		}
	}

	// Token: 0x17001329 RID: 4905
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

	// Token: 0x04007994 RID: 31124
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IList<T> m_list;
}
