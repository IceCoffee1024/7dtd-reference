using System;
using System.Threading;

// Token: 0x02000BB6 RID: 2998
public class ThreadSafeSemantics : IThreadingSemantics
{
	// Token: 0x06005A78 RID: 23160 RVA: 0x0022D2DB File Offset: 0x0022B4DB
	public ThreadSafeSemantics()
	{
		this.monitor = this;
	}

	// Token: 0x06005A79 RID: 23161 RVA: 0x0022D2EA File Offset: 0x0022B4EA
	public ThreadSafeSemantics(object _monitor)
	{
		this.monitor = _monitor;
	}

	// Token: 0x06005A7A RID: 23162 RVA: 0x0022D2FC File Offset: 0x0022B4FC
	public void Synchronize(AtomicActionDelegate _delegate)
	{
		object obj = this.monitor;
		lock (obj)
		{
			_delegate();
		}
	}

	// Token: 0x06005A7B RID: 23163 RVA: 0x0022D33C File Offset: 0x0022B53C
	public int InterlockedAdd(ref int _number, int _add)
	{
		return Interlocked.Add(ref _number, _add);
	}

	// Token: 0x06005A7C RID: 23164 RVA: 0x0022D348 File Offset: 0x0022B548
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

	// Token: 0x04004614 RID: 17940
	[PublicizedFrom(EAccessModifier.Private)]
	public object monitor;
}
