using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000B7D RID: 2941
public readonly struct PinnedBufferRef<[IsUnmanaged] T> where T : struct, ValueType
{
	// Token: 0x06005895 RID: 22677 RVA: 0x0021FB29 File Offset: 0x0021DD29
	public unsafe PinnedBufferRef(T* ptr, int length)
	{
		this.Ptr = ptr;
		this.Length = length;
	}

	// Token: 0x06005896 RID: 22678 RVA: 0x0021FB39 File Offset: 0x0021DD39
	public unsafe Span<T> AsSpan()
	{
		return new Span<T>((void*)this.Ptr, this.Length);
	}

	// Token: 0x06005897 RID: 22679 RVA: 0x0021FB4C File Offset: 0x0021DD4C
	public static implicit operator Span<T>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsSpan();
	}

	// Token: 0x06005898 RID: 22680 RVA: 0x0021FB55 File Offset: 0x0021DD55
	public unsafe ReadOnlySpan<T> AsReadOnlySpan()
	{
		return new ReadOnlySpan<T>((void*)this.Ptr, this.Length);
	}

	// Token: 0x06005899 RID: 22681 RVA: 0x0021FB68 File Offset: 0x0021DD68
	public static implicit operator ReadOnlySpan<T>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsReadOnlySpan();
	}

	// Token: 0x0600589A RID: 22682 RVA: 0x0021FB71 File Offset: 0x0021DD71
	public unsafe PinnedBufferRef<byte> AsBytes()
	{
		return new PinnedBufferRef<byte>((byte*)this.Ptr, this.Length * sizeof(T));
	}

	// Token: 0x0600589B RID: 22683 RVA: 0x0021FB8B File Offset: 0x0021DD8B
	public static implicit operator PinnedBufferRef<byte>(PinnedBufferRef<T> buffer)
	{
		return buffer.AsBytes();
	}

	// Token: 0x0600589C RID: 22684 RVA: 0x0021FB94 File Offset: 0x0021DD94
	public AtomicSafeHandleScope CreateNativeArray(out NativeArray<T> array)
	{
		return MeshDataUtils.CreateNativeArray<T>(this.Ptr, this.Length, out array);
	}

	// Token: 0x040044B8 RID: 17592
	[NativeDisableUnsafePtrRestriction]
	public unsafe readonly T* Ptr;

	// Token: 0x040044B9 RID: 17593
	public readonly int Length;
}
