using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013F7 RID: 5111
public sealed class ListDebugWrapper<T> : CollectionDebugWrapper<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A037 RID: 41015 RVA: 0x003C6017 File Offset: 0x003C4217
	public ListDebugWrapper()
		: this(new List<T>())
	{
	}

	// Token: 0x0600A038 RID: 41016 RVA: 0x003C6024 File Offset: 0x003C4224
	public ListDebugWrapper(IList<T> list)
		: this(null, list)
	{
	}

	// Token: 0x0600A039 RID: 41017 RVA: 0x003C602E File Offset: 0x003C422E
	public ListDebugWrapper(DebugWrapper parent, IList<T> list)
		: base(parent, list)
	{
		this.m_list = list;
	}

	// Token: 0x0600A03A RID: 41018 RVA: 0x003C6040 File Offset: 0x003C4240
	public int IndexOf(T item)
	{
		int num;
		using (base.DebugReadScope())
		{
			num = this.m_list.IndexOf(item);
		}
		return num;
	}

	// Token: 0x0600A03B RID: 41019 RVA: 0x003C6080 File Offset: 0x003C4280
	public void Insert(int index, T item)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_list.Insert(index, item);
		}
	}

	// Token: 0x0600A03C RID: 41020 RVA: 0x003C60C0 File Offset: 0x003C42C0
	public void RemoveAt(int index)
	{
		using (base.DebugReadWriteScope())
		{
			this.m_list.RemoveAt(index);
		}
	}

	// Token: 0x1700131D RID: 4893
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

	// Token: 0x04007953 RID: 31059
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IList<T> m_list;
}
