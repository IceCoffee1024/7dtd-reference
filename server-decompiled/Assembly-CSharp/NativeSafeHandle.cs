using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CDA RID: 3290
[NativeContainer]
public struct NativeSafeHandle<T> : IDisposable where T : struct, IDisposable
{
	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x0600646C RID: 25708 RVA: 0x00278C26 File Offset: 0x00276E26
	public T Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x0600646D RID: 25709 RVA: 0x00278C2E File Offset: 0x00276E2E
	public NativeSafeHandle(ref T _target, Allocator allocator)
	{
		this.target = _target;
	}

	// Token: 0x0600646E RID: 25710 RVA: 0x00278C3C File Offset: 0x00276E3C
	public void Dispose()
	{
		this.target.Dispose();
	}

	// Token: 0x04004E6F RID: 20079
	[PublicizedFrom(EAccessModifier.Private)]
	public T target;
}
