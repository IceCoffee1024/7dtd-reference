using System;
using System.Collections.Generic;

// Token: 0x02001320 RID: 4896
public class WorkBatch<T>
{
	// Token: 0x06009AA4 RID: 39588 RVA: 0x003A9BEC File Offset: 0x003A7DEC
	public WorkBatch()
	{
		this.queuingList = new List<T>();
		this.workingList = new List<T>();
		this.sync = new object();
	}

	// Token: 0x06009AA5 RID: 39589 RVA: 0x003A9C18 File Offset: 0x003A7E18
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

	// Token: 0x06009AA6 RID: 39590 RVA: 0x003A9C70 File Offset: 0x003A7E70
	public void Clear()
	{
		object obj = this.sync;
		lock (obj)
		{
			this.queuingList.Clear();
		}
		this.workingList.Clear();
	}

	// Token: 0x06009AA7 RID: 39591 RVA: 0x003A9CC0 File Offset: 0x003A7EC0
	public void DoWork(Action<T> _action)
	{
		this.FlipLists();
		this.workingList.ForEach(_action);
		this.workingList.Clear();
	}

	// Token: 0x06009AA8 RID: 39592 RVA: 0x003A9CE0 File Offset: 0x003A7EE0
	public void Add(T _item)
	{
		object obj = this.sync;
		lock (obj)
		{
			this.queuingList.Add(_item);
		}
	}

	// Token: 0x06009AA9 RID: 39593 RVA: 0x003A9D28 File Offset: 0x003A7F28
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

	// Token: 0x06009AAA RID: 39594 RVA: 0x003A9D70 File Offset: 0x003A7F70
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

	// Token: 0x04007494 RID: 29844
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> queuingList;

	// Token: 0x04007495 RID: 29845
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> workingList;

	// Token: 0x04007496 RID: 29846
	[PublicizedFrom(EAccessModifier.Private)]
	public object sync;
}
