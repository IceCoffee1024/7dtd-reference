using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CCF RID: 3279
[NativeContainer]
public struct NativeSafeHandle<T> : IDisposable where T : struct, IDisposable
{
	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x06006447 RID: 25671 RVA: 0x002776FE File Offset: 0x002758FE
	public T Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x06006448 RID: 25672 RVA: 0x00277706 File Offset: 0x00275906
	public NativeSafeHandle(ref T _target, Allocator allocator)
	{
		this.target = _target;
	}

	// Token: 0x06006449 RID: 25673 RVA: 0x00277714 File Offset: 0x00275914
	public void Dispose()
	{
		this.target.Dispose();
	}

	// Token: 0x04004E4A RID: 20042
	[PublicizedFrom(EAccessModifier.Private)]
	public T target;
}
