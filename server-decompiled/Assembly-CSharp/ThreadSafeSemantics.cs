using System;
using System.Threading;

// Token: 0x02000BDA RID: 3034
public class ThreadSafeSemantics : IThreadingSemantics
{
	// Token: 0x06005B19 RID: 23321 RVA: 0x0023333F File Offset: 0x0023153F
	public ThreadSafeSemantics()
	{
		this.monitor = this;
	}

	// Token: 0x06005B1A RID: 23322 RVA: 0x0023334E File Offset: 0x0023154E
	public ThreadSafeSemantics(object _monitor)
	{
		this.monitor = _monitor;
	}

	// Token: 0x06005B1B RID: 23323 RVA: 0x00233360 File Offset: 0x00231560
	public void Synchronize(AtomicActionDelegate _delegate)
	{
		object obj = this.monitor;
		lock (obj)
		{
			_delegate();
		}
	}

	// Token: 0x06005B1C RID: 23324 RVA: 0x002333A0 File Offset: 0x002315A0
	public int InterlockedAdd(ref int _number, int _add)
	{
		return Interlocked.Add(ref _number, _add);
	}

	// Token: 0x06005B1D RID: 23325 RVA: 0x002333AC File Offset: 0x002315AC
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

	// Token: 0x040046EF RID: 18159
	[PublicizedFrom(EAccessModifier.Private)]
	public object monitor;
}
