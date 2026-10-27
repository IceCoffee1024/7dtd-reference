using System;
using System.Collections.Generic;

// Token: 0x02001332 RID: 4914
public class WorkBatch<T>
{
	// Token: 0x06009AFC RID: 39676 RVA: 0x003ABFF0 File Offset: 0x003AA1F0
	public WorkBatch()
	{
		this.queuingList = new List<T>();
		this.workingList = new List<T>();
		this.sync = new object();
	}

	// Token: 0x06009AFD RID: 39677 RVA: 0x003AC01C File Offset: 0x003AA21C
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

	// Token: 0x06009AFE RID: 39678 RVA: 0x003AC074 File Offset: 0x003AA274
	public void Clear()
	{
		object obj = this.sync;
		lock (obj)
		{
			this.queuingList.Clear();
		}
		this.workingList.Clear();
	}

	// Token: 0x06009AFF RID: 39679 RVA: 0x003AC0C4 File Offset: 0x003AA2C4
	public void DoWork(Action<T> _action)
	{
		this.FlipLists();
		this.workingList.ForEach(_action);
		this.workingList.Clear();
	}

	// Token: 0x06009B00 RID: 39680 RVA: 0x003AC0E4 File Offset: 0x003AA2E4
	public void Add(T _item)
	{
		object obj = this.sync;
		lock (obj)
		{
			this.queuingList.Add(_item);
		}
	}

	// Token: 0x06009B01 RID: 39681 RVA: 0x003AC12C File Offset: 0x003AA32C
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

	// Token: 0x06009B02 RID: 39682 RVA: 0x003AC174 File Offset: 0x003AA374
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

	// Token: 0x040074D5 RID: 29909
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> queuingList;

	// Token: 0x040074D6 RID: 29910
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> workingList;

	// Token: 0x040074D7 RID: 29911
	[PublicizedFrom(EAccessModifier.Private)]
	public object sync;
}
