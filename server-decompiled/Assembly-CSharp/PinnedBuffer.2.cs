using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000B7C RID: 2940
public sealed class PinnedBuffer<[IsUnmanaged] T> : PinnedBuffer where T : struct, ValueType
{
	// Token: 0x06005883 RID: 22659 RVA: 0x0021F95C File Offset: 0x0021DB5C
	public PinnedBuffer(MemoryPooledArray<T> pool, int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.m_pool = pool;
		this.m_poolArray = this.m_pool.Alloc(length);
		this.InitBuffer(this.m_poolArray, length);
	}

	// Token: 0x06005884 RID: 22660 RVA: 0x0021F99B File Offset: 0x0021DB9B
	public PinnedBuffer()
	{
		this.InitBuffer(Array.Empty<T>(), 0);
	}

	// Token: 0x06005885 RID: 22661 RVA: 0x0021F9AF File Offset: 0x0021DBAF
	public PinnedBuffer(int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.InitBuffer(new T[length], length);
	}

	// Token: 0x06005886 RID: 22662 RVA: 0x0021F9D5 File Offset: 0x0021DBD5
	public PinnedBuffer(T[] buffer, int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.InitBuffer(buffer, length);
	}

	// Token: 0x06005887 RID: 22663 RVA: 0x0021F9F6 File Offset: 0x0021DBF6
	public PinnedBuffer(MemoryPooledArray<T> pool, T[] poolArray, int length)
	{
		this.m_pool = pool;
		this.m_poolArray = poolArray;
		this.InitBuffer(poolArray, length);
	}

	// Token: 0x06005888 RID: 22664 RVA: 0x0021FA14 File Offset: 0x0021DC14
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe void InitBuffer(T[] buffer, int length)
	{
		this.m_ptr = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(buffer, out this.m_gcHandle);
		this.m_length = length;
	}

	// Token: 0x06005889 RID: 22665 RVA: 0x0021FA30 File Offset: 0x0021DC30
	public override void Dispose()
	{
		this.m_length = 0;
		this.m_ptr = null;
		if (this.m_gcHandle > 0UL)
		{
			UnsafeUtility.ReleaseGCObject(this.m_gcHandle);
			this.m_gcHandle = 0UL;
		}
		if (this.m_pool != null)
		{
			this.m_pool.Free(this.m_poolArray);
			this.m_pool = null;
		}
		this.m_poolArray = null;
	}

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x0600588A RID: 22666 RVA: 0x0021FA90 File Offset: 0x0021DC90
	public unsafe T* Ptr
	{
		get
		{
			return this.m_ptr;
		}
	}

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x0600588B RID: 22667 RVA: 0x0021FA98 File Offset: 0x0021DC98
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x0600588C RID: 22668 RVA: 0x0021FAA0 File Offset: 0x0021DCA0
	public PinnedBufferRef<T> AsRef()
	{
		return new PinnedBufferRef<T>(this.m_ptr, this.m_length);
	}

	// Token: 0x0600588D RID: 22669 RVA: 0x0021FAB3 File Offset: 0x0021DCB3
	public static implicit operator PinnedBufferRef<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsRef();
	}

	// Token: 0x0600588E RID: 22670 RVA: 0x0021FABC File Offset: 0x0021DCBC
	public PinnedBufferRef<byte> AsBytes()
	{
		return this.AsRef().AsBytes();
	}

	// Token: 0x0600588F RID: 22671 RVA: 0x0021FAD7 File Offset: 0x0021DCD7
	public static implicit operator PinnedBufferRef<byte>(PinnedBuffer<T> buffer)
	{
		return buffer.AsBytes();
	}

	// Token: 0x06005890 RID: 22672 RVA: 0x0021FADF File Offset: 0x0021DCDF
	public unsafe Span<T> AsSpan()
	{
		return new Span<T>((void*)this.m_ptr, this.m_length);
	}

	// Token: 0x06005891 RID: 22673 RVA: 0x0021FAF2 File Offset: 0x0021DCF2
	public static implicit operator Span<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsSpan();
	}

	// Token: 0x06005892 RID: 22674 RVA: 0x0021FAFA File Offset: 0x0021DCFA
	public unsafe ReadOnlySpan<T> AsReadOnlySpan()
	{
		return new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
	}

	// Token: 0x06005893 RID: 22675 RVA: 0x0021FB0D File Offset: 0x0021DD0D
	public static implicit operator ReadOnlySpan<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsReadOnlySpan();
	}

	// Token: 0x06005894 RID: 22676 RVA: 0x0021FB15 File Offset: 0x0021DD15
	public AtomicSafeHandleScope CreateNativeArray(out NativeArray<T> array)
	{
		return MeshDataUtils.CreateNativeArray<T>(this.m_ptr, this.m_length, out array);
	}

	// Token: 0x040044B3 RID: 17587
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledArray<T> m_pool;

	// Token: 0x040044B4 RID: 17588
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] m_poolArray;

	// Token: 0x040044B5 RID: 17589
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* m_ptr;

	// Token: 0x040044B6 RID: 17590
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_length;

	// Token: 0x040044B7 RID: 17591
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong m_gcHandle;
}
