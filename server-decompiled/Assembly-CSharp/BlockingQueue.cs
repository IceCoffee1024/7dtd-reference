using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x020013AB RID: 5035
public class BlockingQueue<T>
{
	// Token: 0x06009EAD RID: 40621 RVA: 0x003BDAA0 File Offset: 0x003BBCA0
	public void Enqueue(T item)
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.queue.Enqueue(item);
			Monitor.PulseAll(this.queue);
		}
	}

	// Token: 0x06009EAE RID: 40622 RVA: 0x003BDAF4 File Offset: 0x003BBCF4
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

	// Token: 0x06009EAF RID: 40623 RVA: 0x003BDB6C File Offset: 0x003BBD6C
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

	// Token: 0x06009EB0 RID: 40624 RVA: 0x003BDBB8 File Offset: 0x003BBDB8
	public void Close()
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.closing = true;
			Monitor.PulseAll(this.queue);
		}
	}

	// Token: 0x06009EB1 RID: 40625 RVA: 0x003BDC04 File Offset: 0x003BBE04
	public void Clear()
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.queue.Clear();
		}
	}

	// Token: 0x04007844 RID: 30788
	[PublicizedFrom(EAccessModifier.Private)]
	public bool closing;

	// Token: 0x04007845 RID: 30789
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<T> queue = new Queue<T>();
}
