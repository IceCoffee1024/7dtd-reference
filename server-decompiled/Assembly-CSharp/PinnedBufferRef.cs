using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000B88 RID: 2952
public readonly struct PinnedBufferRef<[IsUnmanaged] T> where T : struct, ValueType
{
	// Token: 0x060058BA RID: 22714 RVA: 0x00221085 File Offset: 0x0021F285
	public unsafe PinnedBufferRef(T* ptr, int length)
	{
		this.Ptr = ptr;
		this.Length = length;
	}

	// Token: 0x060058BB RID: 22715 RVA: 0x00221095 File Offset: 0x0021F295
	public unsafe Span<T> AsSpan()
	{
		return new Span<T>((void*)this.Ptr, this.Length);
	}

	// Token: 0x060058BC RID: 22716 RVA: 0x002210A8 File Offset: 0x0021F2A8
	public static implicit operator Span<T>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsSpan();
	}

	// Token: 0x060058BD RID: 22717 RVA: 0x002210B1 File Offset: 0x0021F2B1
	public unsafe ReadOnlySpan<T> AsReadOnlySpan()
	{
		return new ReadOnlySpan<T>((void*)this.Ptr, this.Length);
	}

	// Token: 0x060058BE RID: 22718 RVA: 0x002210C4 File Offset: 0x0021F2C4
	public static implicit operator ReadOnlySpan<T>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsReadOnlySpan();
	}

	// Token: 0x060058BF RID: 22719 RVA: 0x002210CD File Offset: 0x0021F2CD
	public unsafe PinnedBufferRef<byte> AsBytes()
	{
		return new PinnedBufferRef<byte>((byte*)this.Ptr, this.Length * sizeof(T));
	}

	// Token: 0x060058C0 RID: 22720 RVA: 0x002210E7 File Offset: 0x0021F2E7
	public static implicit operator PinnedBufferRef<byte>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsBytes();
	}

	// Token: 0x060058C1 RID: 22721 RVA: 0x002210F0 File Offset: 0x0021F2F0
	public AtomicSafeHandleScope CreateNativeArray(out NativeArray<T> array)
	{
		return MeshDataUtils.CreateNativeArray<T>(this.Ptr, this.Length, out array);
	}

	// Token: 0x040044DD RID: 17629
	[NativeDisableUnsafePtrRestriction]
	public unsafe readonly T* Ptr;

	// Token: 0x040044DE RID: 17630
	public readonly int Length;
}
