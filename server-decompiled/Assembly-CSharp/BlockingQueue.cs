using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x020013CB RID: 5067
public class BlockingQueue<T>
{
	// Token: 0x06009F58 RID: 40792 RVA: 0x003C263C File Offset: 0x003C083C
	public void Enqueue(T item)
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.queue.Enqueue(item);
			Monitor.PulseAll(this.queue);
		}
	}

	// Token: 0x06009F59 RID: 40793 RVA: 0x003C2690 File Offset: 0x003C0890
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

	// Token: 0x06009F5A RID: 40794 RVA: 0x003C2708 File Offset: 0x003C0908
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

	// Token: 0x06009F5B RID: 40795 RVA: 0x003C2754 File Offset: 0x003C0954
	public void Close()
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.closing = true;
			Monitor.PulseAll(this.queue);
		}
	}

	// Token: 0x06009F5C RID: 40796 RVA: 0x003C27A0 File Offset: 0x003C09A0
	public void Clear()
	{
		Queue<T> queue = this.queue;
		lock (queue)
		{
			this.queue.Clear();
		}
	}

	// Token: 0x040078BF RID: 30911
	[PublicizedFrom(EAccessModifier.Private)]
	public bool closing;

	// Token: 0x040078C0 RID: 30912
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<T> queue = new Queue<T>();
}
