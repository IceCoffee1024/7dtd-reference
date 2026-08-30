using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000B6A RID: 2922
public sealed class PinnedBuffer<[IsUnmanaged] T> : PinnedBuffer where T : struct, ValueType
{
	// Token: 0x06005869 RID: 22633 RVA: 0x0021C6D0 File Offset: 0x0021A8D0
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

	// Token: 0x0600586A RID: 22634 RVA: 0x0021C70F File Offset: 0x0021A90F
	public PinnedBuffer()
	{
		this.InitBuffer(Array.Empty<T>(), 0);
	}

	// Token: 0x0600586B RID: 22635 RVA: 0x0021C723 File Offset: 0x0021A923
	public PinnedBuffer(int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.InitBuffer(new T[length], length);
	}

	// Token: 0x0600586C RID: 22636 RVA: 0x0021C749 File Offset: 0x0021A949
	public PinnedBuffer(T[] buffer, int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.InitBuffer(buffer, length);
	}

	// Token: 0x0600586D RID: 22637 RVA: 0x0021C76A File Offset: 0x0021A96A
	public PinnedBuffer(MemoryPooledArray<T> pool, T[] poolArray, int length)
	{
		this.m_pool = pool;
		this.m_poolArray = poolArray;
		this.InitBuffer(poolArray, length);
	}

	// Token: 0x0600586E RID: 22638 RVA: 0x0021C788 File Offset: 0x0021A988
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe void InitBuffer(T[] buffer, int length)
	{
		this.m_ptr = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(buffer, out this.m_gcHandle);
		this.m_length = length;
	}

	// Token: 0x0600586F RID: 22639 RVA: 0x0021C7A4 File Offset: 0x0021A9A4
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

	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x06005870 RID: 22640 RVA: 0x0021C804 File Offset: 0x0021AA04
	public unsafe T* Ptr
	{
		get
		{
			return this.m_ptr;
		}
	}

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x06005871 RID: 22641 RVA: 0x0021C80C File Offset: 0x0021AA0C
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x06005872 RID: 22642 RVA: 0x0021C814 File Offset: 0x0021AA14
	public PinnedBufferRef<T> AsRef()
	{
		return new PinnedBufferRef<T>(this.m_ptr, this.m_length);
	}

	// Token: 0x06005873 RID: 22643 RVA: 0x0021C827 File Offset: 0x0021AA27
	public static implicit operator PinnedBufferRef<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsRef();
	}

	// Token: 0x06005874 RID: 22644 RVA: 0x0021C830 File Offset: 0x0021AA30
	public PinnedBufferRef<byte> AsBytes()
	{
		return this.AsRef().AsBytes();
	}

	// Token: 0x06005875 RID: 22645 RVA: 0x0021C84B File Offset: 0x0021AA4B
	public static implicit operator PinnedBufferRef<byte>(PinnedBuffer<T> buffer)
	{
		return buffer.AsBytes();
	}

	// Token: 0x06005876 RID: 22646 RVA: 0x0021C853 File Offset: 0x0021AA53
	public unsafe Span<T> AsSpan()
	{
		return new Span<T>((void*)this.m_ptr, this.m_length);
	}

	// Token: 0x06005877 RID: 22647 RVA: 0x0021C866 File Offset: 0x0021AA66
	public static implicit operator Span<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsSpan();
	}

	// Token: 0x06005878 RID: 22648 RVA: 0x0021C86E File Offset: 0x0021AA6E
	public unsafe ReadOnlySpan<T> AsReadOnlySpan()
	{
		return new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
	}

	// Token: 0x06005879 RID: 22649 RVA: 0x0021C881 File Offset: 0x0021AA81
	public static implicit operator ReadOnlySpan<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsReadOnlySpan();
	}

	// Token: 0x0600587A RID: 22650 RVA: 0x0021C889 File Offset: 0x0021AA89
	public AtomicSafeHandleScope CreateNativeArray(out NativeArray<T> array)
	{
		return MeshDataUtils.CreateNativeArray<T>(this.m_ptr, this.m_length, out array);
	}

	// Token: 0x04004475 RID: 17525
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledArray<T> m_pool;

	// Token: 0x04004476 RID: 17526
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] m_poolArray;

	// Token: 0x04004477 RID: 17527
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* m_ptr;

	// Token: 0x04004478 RID: 17528
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_length;

	// Token: 0x04004479 RID: 17529
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong m_gcHandle;
}
