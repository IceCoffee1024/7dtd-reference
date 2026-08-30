using System;
using System.Collections.Generic;

// Token: 0x02001300 RID: 4864
public class WorkBatch<T>
{
	// Token: 0x060099F8 RID: 39416 RVA: 0x003A503C File Offset: 0x003A323C
	public WorkBatch()
	{
		this.queuingList = new List<T>();
		this.workingList = new List<T>();
		this.sync = new object();
	}

	// Token: 0x060099F9 RID: 39417 RVA: 0x003A5068 File Offset: 0x003A3268
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

	// Token: 0x060099FA RID: 39418 RVA: 0x003A50C0 File Offset: 0x003A32C0
	public void Clear()
	{
		object obj = this.sync;
		lock (obj)
		{
			this.queuingList.Clear();
		}
		this.workingList.Clear();
	}

	// Token: 0x060099FB RID: 39419 RVA: 0x003A5110 File Offset: 0x003A3310
	public void DoWork(Action<T> _action)
	{
		this.FlipLists();
		this.workingList.ForEach(_action);
		this.workingList.Clear();
	}

	// Token: 0x060099FC RID: 39420 RVA: 0x003A5130 File Offset: 0x003A3330
	public void Add(T _item)
	{
		object obj = this.sync;
		lock (obj)
		{
			this.queuingList.Add(_item);
		}
	}

	// Token: 0x060099FD RID: 39421 RVA: 0x003A5178 File Offset: 0x003A3378
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

	// Token: 0x060099FE RID: 39422 RVA: 0x003A51C0 File Offset: 0x003A33C0
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

	// Token: 0x04007419 RID: 29721
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> queuingList;

	// Token: 0x0400741A RID: 29722
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> workingList;

	// Token: 0x0400741B RID: 29723
	[PublicizedFrom(EAccessModifier.Private)]
	public object sync;
}
