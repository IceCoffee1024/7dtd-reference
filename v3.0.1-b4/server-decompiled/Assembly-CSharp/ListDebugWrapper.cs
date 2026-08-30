using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013BC RID: 5052
public sealed class ListDebugWrapper<T> : CollectionDebugWrapper<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x06009E8F RID: 40591 RVA: 0x003BC137 File Offset: 0x003BA337
	public ListDebugWrapper()
		: this(new List<T>())
	{
	}

	// Token: 0x06009E90 RID: 40592 RVA: 0x003BC144 File Offset: 0x003BA344
	public ListDebugWrapper(IList<T> list)
		: this(null, list)
	{
	}

	// Token: 0x06009E91 RID: 40593 RVA: 0x003BC14E File Offset: 0x003BA34E
	public ListDebugWrapper(DebugWrapper parent, IList<T> list)
		: base(parent, list)
	{
		this.m_list = list;
	}

	// Token: 0x06009E92 RID: 40594 RVA: 0x003BC160 File Offset: 0x003BA360
	public int IndexOf(T item)
	{
		int num;
		using (base.DebugReadScope())
		{
			num = this.m_list.IndexOf(item);
		}
		return num;
	}

	// Token: 0x06009E93 RID: 40595 RVA: 0x003BC1A0 File Offset: 0x003BA3A0
	public void Insert(int index, T item)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_list.Insert(index, item);
		}
	}

	// Token: 0x06009E94 RID: 40596 RVA: 0x003BC1E0 File Offset: 0x003BA3E0
	public void RemoveAt(int index)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_list.RemoveAt(index);
		}
	}

	// Token: 0x1700129F RID: 4767
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

	// Token: 0x040077FF RID: 30719
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IList<T> m_list;
}
