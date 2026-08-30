using System;
using System.Collections.Generic;

// Token: 0x020012E6 RID: 4838
public class WorkBatch<T>
{
	// Token: 0x060098FD RID: 39165 RVA: 0x0039FDA4 File Offset: 0x0039DFA4
	public WorkBatch()
	{
		this.queuingList = new List<T>();
		this.workingList = new List<T>();
		this.sync = new object();
	}

	// Token: 0x060098FE RID: 39166 RVA: 0x0039FDD0 File Offset: 0x0039DFD0
	public int Count()
	{
		int num = this.workingList.Count;
		object obj = this.sync;
		lock (obj)
		{
			num += this.queuingList.Count;
		}
		return num;
	}

	// Token: 0x060098FF RID: 39167 RVA: 0x0039FE28 File Offset: 0x0039E028
	public void Clear()
	{
		object obj = this.sync;
		lock (obj)
		{
			this.queuingList.Clear();
		}
		this.workingList.Clear();
	}

	// Token: 0x06009900 RID: 39168 RVA: 0x0039FE78 File Offset: 0x0039E078
	public void DoWork(Action<T> _action)
	{
		this.FlipLists();
		this.workingList.ForEach(_action);
		this.workingList.Clear();
	}

	// Token: 0x06009901 RID: 39169 RVA: 0x0039FE98 File Offset: 0x0039E098
	public void Add(T _item)
	{
		object obj = this.sync;
		lock (obj)
		{
			this.queuingList.Add(_item);
		}
	}

	// Token: 0x06009902 RID: 39170 RVA: 0x0039FEE0 File Offset: 0x0039E0E0
	public bool Remove(T _item)
	{
		object obj = this.sync;
		bool flag2;
		lock (obj)
		{
			flag2 = this.queuingList.Remove(_item);
		}
		return flag2;
	}

	// Token: 0x06009903 RID: 39171 RVA: 0x0039FF28 File Offset: 0x0039E128
	[PublicizedFrom(EAccessModifier.Private)]
	public void FlipLists()
	{
		object obj = this.sync;
		lock (obj)
		{
			List<T> list = this.workingList;
			this.workingList = this.queuingList;
			this.queuingList = list;
		}
	}

	// Token: 0x04007341 RID: 29505
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> queuingList;

	// Token: 0x04007342 RID: 29506
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> workingList;

	// Token: 0x04007343 RID: 29507
	[PublicizedFrom(EAccessModifier.Private)]
	public object sync;
}
