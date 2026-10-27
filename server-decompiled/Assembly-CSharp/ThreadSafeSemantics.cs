using System;
using System.Threading;

// Token: 0x02000BE5 RID: 3045
public class ThreadSafeSemantics : IThreadingSemantics
{
	// Token: 0x06005B3E RID: 23358 RVA: 0x0023489B File Offset: 0x00232A9B
	public ThreadSafeSemantics()
	{
		this.monitor = this;
	}

	// Token: 0x06005B3F RID: 23359 RVA: 0x002348AA File Offset: 0x00232AAA
	public ThreadSafeSemantics(object _monitor)
	{
		this.monitor = _monitor;
	}

	// Token: 0x06005B40 RID: 23360 RVA: 0x002348BC File Offset: 0x00232ABC
	public void Synchronize(AtomicActionDelegate _delegate)
	{
		object obj = this.monitor;
		lock (obj)
		{
			_delegate();
		}
	}

	// Token: 0x06005B41 RID: 23361 RVA: 0x002348FC File Offset: 0x00232AFC
	public int InterlockedAdd(ref int _number, int _add)
	{
		return Interlocked.Add(ref _number, _add);
	}

	// Token: 0x06005B42 RID: 23362 RVA: 0x00234908 File Offset: 0x00232B08
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

	// Token: 0x04004714 RID: 18196
	[PublicizedFrom(EAccessModifier.Private)]
	public object monitor;
}
