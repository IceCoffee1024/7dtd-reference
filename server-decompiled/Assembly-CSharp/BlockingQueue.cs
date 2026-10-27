using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x020013DD RID: 5085
public class BlockingQueue<T>
{
	// Token: 0x06009FB0 RID: 40880 RVA: 0x003C4A40 File Offset: 0x003C2C40
	public void Enqueue(T item)
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.queue.Enqueue(item);
			Monitor.PulseAll(this.queue);
		}
	}

	// Token: 0x06009FB1 RID: 40881 RVA: 0x003C4A94 File Offset: 0x003C2C94
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

	// Token: 0x06009FB2 RID: 40882 RVA: 0x003C4B0C File Offset: 0x003C2D0C
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

	// Token: 0x06009FB3 RID: 40883 RVA: 0x003C4B58 File Offset: 0x003C2D58
	public void Close()
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.closing = true;
			Monitor.PulseAll(this.queue);
		}
	}

	// Token: 0x06009FB4 RID: 40884 RVA: 0x003C4BA4 File Offset: 0x003C2DA4
	public void Clear()
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.queue.Clear();
		}
	}

	// Token: 0x04007900 RID: 30976
	[PublicizedFrom(EAccessModifier.Private)]
	public bool closing;

	// Token: 0x04007901 RID: 30977
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<T> queue = new Queue<T>();
}
