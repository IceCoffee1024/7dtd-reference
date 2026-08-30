using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x02001390 RID: 5008
public class BlockingQueue<T>
{
	// Token: 0x06009DB0 RID: 40368 RVA: 0x003B875C File Offset: 0x003B695C
	public void Enqueue(T item)
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.queue.Enqueue(item);
			Monitor.PulseAll(this.queue);
		}
	}

	// Token: 0x06009DB1 RID: 40369 RVA: 0x003B87B0 File Offset: 0x003B69B0
	public T Dequeue()
	{
		Queue<T> queue = this.queue;
		T t;
		lock (queue)
		{
			while (this.queue.Count == 0)
			{
				if (this.closing)
				{
					t = default(T);
					return t;
				}
				Monitor.Wait(this.queue);
			}
			t = this.queue.Dequeue();
		}
		return t;
	}

	// Token: 0x06009DB2 RID: 40370 RVA: 0x003B8828 File Offset: 0x003B6A28
	public bool HasData()
	{
		Queue<T> queue = this.queue;
		bool flag2;
		lock (queue)
		{
			flag2 = this.queue.Count > 0;
		}
		return flag2;
	}

	// Token: 0x06009DB3 RID: 40371 RVA: 0x003B8874 File Offset: 0x003B6A74
	public void Close()
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.closing = true;
			Monitor.PulseAll(this.queue);
		}
	}

	// Token: 0x06009DB4 RID: 40372 RVA: 0x003B88C0 File Offset: 0x003B6AC0
	public void Clear()
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.queue.Clear();
		}
	}

	// Token: 0x0400776B RID: 30571
	[PublicizedFrom(EAccessModifier.Private)]
	public bool closing;

	// Token: 0x0400776C RID: 30572
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<T> queue = new Queue<T>();
}
