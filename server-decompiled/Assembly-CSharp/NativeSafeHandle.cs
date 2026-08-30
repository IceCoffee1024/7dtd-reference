using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CAF RID: 3247
[NativeContainer]
public struct NativeSafeHandle<T> : IDisposable where T : struct, IDisposable
{
	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x060063C8 RID: 25544 RVA: 0x002713FE File Offset: 0x0026F5FE
	public T Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x060063C9 RID: 25545 RVA: 0x00271406 File Offset: 0x0026F606
	public NativeSafeHandle(ref T _target, Allocator allocator)
	{
		this.target = _target;
	}

	// Token: 0x060063CA RID: 25546 RVA: 0x00271414 File Offset: 0x0026F614
	public void Dispose()
	{
		this.target.Dispose();
	}

	// Token: 0x04004D76 RID: 19830
	[PublicizedFrom(EAccessModifier.Private)]
	public T target;
}
