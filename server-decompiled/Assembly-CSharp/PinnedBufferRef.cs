using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000B6B RID: 2923
public readonly struct PinnedBufferRef<[IsUnmanaged] T> where T : struct, ValueType
{
	// Token: 0x0600587B RID: 22651 RVA: 0x0021C89D File Offset: 0x0021AA9D
	public unsafe PinnedBufferRef(T* ptr, int length)
	{
		this.Ptr = ptr;
		this.Length = length;
	}

	// Token: 0x0600587C RID: 22652 RVA: 0x0021C8AD File Offset: 0x0021AAAD
	public unsafe Span<T> AsSpan()
	{
		return new Span<T>((void*)this.Ptr, this.Length);
	}

	// Token: 0x0600587D RID: 22653 RVA: 0x0021C8C0 File Offset: 0x0021AAC0
	public static implicit operator Span<T>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsSpan();
	}

	// Token: 0x0600587E RID: 22654 RVA: 0x0021C8C9 File Offset: 0x0021AAC9
	public unsafe ReadOnlySpan<T> AsReadOnlySpan()
	{
		return new ReadOnlySpan<T>((void*)this.Ptr, this.Length);
	}

	// Token: 0x0600587F RID: 22655 RVA: 0x0021C8DC File Offset: 0x0021AADC
	public static implicit operator ReadOnlySpan<T>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsReadOnlySpan();
	}

	// Token: 0x06005880 RID: 22656 RVA: 0x0021C8E5 File Offset: 0x0021AAE5
	public unsafe PinnedBufferRef<byte> AsBytes()
	{
		return new PinnedBufferRef<byte>((byte*)this.Ptr, this.Length * sizeof(T));
	}

	// Token: 0x06005881 RID: 22657 RVA: 0x0021C8FF File Offset: 0x0021AAFF
	public static implicit operator PinnedBufferRef<byte>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsBytes();
	}

	// Token: 0x06005882 RID: 22658 RVA: 0x0021C908 File Offset: 0x0021AB08
	public AtomicSafeHandleScope CreateNativeArray(out NativeArray<T> array)
	{
		return MeshDataUtils.CreateNativeArray<T>(this.Ptr, this.Length, out array);
	}

	// Token: 0x0400447A RID: 17530
	[NativeDisableUnsafePtrRestriction]
	public unsafe readonly T* Ptr;

	// Token: 0x0400447B RID: 17531
	public readonly int Length;
}
