using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CC1 RID: 3265
[NativeContainer]
public struct NativeSafeHandle<T> : IDisposable where T : struct, IDisposable
{
	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x06006454 RID: 25684 RVA: 0x002742FA File Offset: 0x002724FA
	public T Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x06006455 RID: 25685 RVA: 0x00274302 File Offset: 0x00272502
	public NativeSafeHandle(ref T _target, Allocator allocator)
	{
		this.target = _target;
	}

	// Token: 0x06006456 RID: 25686 RVA: 0x00274310 File Offset: 0x00272510
	public void Dispose()
	{
		this.target.Dispose();
	}

	// Token: 0x04004E14 RID: 19988
	[PublicizedFrom(EAccessModifier.Private)]
	public T target;
}
