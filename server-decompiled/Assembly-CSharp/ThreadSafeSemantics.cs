using System;
using System.Threading;

// Token: 0x02000BC8 RID: 3016
public class ThreadSafeSemantics : IThreadingSemantics
{
	// Token: 0x06005B00 RID: 23296 RVA: 0x00230043 File Offset: 0x0022E243
	public ThreadSafeSemantics()
	{
		this.monitor = this;
	}

	// Token: 0x06005B01 RID: 23297 RVA: 0x00230052 File Offset: 0x0022E252
	public ThreadSafeSemantics(object _monitor)
	{
		this.monitor = _monitor;
	}

	// Token: 0x06005B02 RID: 23298 RVA: 0x00230064 File Offset: 0x0022E264
	public void Synchronize(AtomicActionDelegate _delegate)
	{
		object obj = this.monitor;
		lock (obj)
		{
			_delegate();
		}
	}

	// Token: 0x06005B03 RID: 23299 RVA: 0x002300A4 File Offset: 0x0022E2A4
	public int InterlockedAdd(ref int _number, int _add)
	{
		return Interlocked.Add(ref _number, _add);
	}

	// Token: 0x06005B04 RID: 23300 RVA: 0x002300B0 File Offset: 0x0022E2B0
	public T Synchronize<T>(Func<T> _delegate)
	{
		object obj = this.monitor;
		T t;
		lock (obj)
		{
			t = _delegate();
		}
		return t;
	}

	// Token: 0x040046B0 RID: 18096
	[PublicizedFrom(EAccessModifier.Private)]
	public object monitor;
}
