using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000B59 RID: 2905
public readonly struct PinnedBufferRef<[IsUnmanaged] T> where T : struct, ValueType
{
	// Token: 0x060057F3 RID: 22515 RVA: 0x00219B81 File Offset: 0x00217D81
	public unsafe PinnedBufferRef(T* ptr, int length)
	{
		this.Ptr = ptr;
		this.Length = length;
	}

	// Token: 0x060057F4 RID: 22516 RVA: 0x00219B91 File Offset: 0x00217D91
	public unsafe Span<T> AsSpan()
	{
		return new Span<T>((void*)this.Ptr, this.Length);
	}

	// Token: 0x060057F5 RID: 22517 RVA: 0x00219BA4 File Offset: 0x00217DA4
	public static implicit operator Span<T>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsSpan();
	}

	// Token: 0x060057F6 RID: 22518 RVA: 0x00219BAD File Offset: 0x00217DAD
	public unsafe ReadOnlySpan<T> AsReadOnlySpan()
	{
		return new ReadOnlySpan<T>((void*)this.Ptr, this.Length);
	}

	// Token: 0x060057F7 RID: 22519 RVA: 0x00219BC0 File Offset: 0x00217DC0
	public static implicit operator ReadOnlySpan<T>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsReadOnlySpan();
	}

	// Token: 0x060057F8 RID: 22520 RVA: 0x00219BC9 File Offset: 0x00217DC9
	public unsafe PinnedBufferRef<byte> AsBytes()
	{
		return new PinnedBufferRef<byte>((byte*)this.Ptr, this.Length * sizeof(T));
	}

	// Token: 0x060057F9 RID: 22521 RVA: 0x00219BE3 File Offset: 0x00217DE3
	public static implicit operator PinnedBufferRef<byte>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsBytes();
	}

	// Token: 0x060057FA RID: 22522 RVA: 0x00219BEC File Offset: 0x00217DEC
	public AtomicSafeHandleScope CreateNativeArray(out NativeArray<T> array)
	{
		return MeshDataUtils.CreateNativeArray<T>(this.Ptr, this.Length, out array);
	}

	// Token: 0x040043DF RID: 17375
	[NativeDisableUnsafePtrRestriction]
	public unsafe readonly T* Ptr;

	// Token: 0x040043E0 RID: 17376
	public readonly int Length;
}
