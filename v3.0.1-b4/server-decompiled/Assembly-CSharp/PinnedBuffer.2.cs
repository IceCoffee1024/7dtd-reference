using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000B58 RID: 2904
public sealed class PinnedBuffer<[IsUnmanaged] T> : PinnedBuffer where T : struct, ValueType
{
	// Token: 0x060057E1 RID: 22497 RVA: 0x002199B4 File Offset: 0x00217BB4
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

	// Token: 0x060057E2 RID: 22498 RVA: 0x002199F3 File Offset: 0x00217BF3
	public PinnedBuffer()
	{
		this.InitBuffer(Array.Empty<T>(), 0);
	}

	// Token: 0x060057E3 RID: 22499 RVA: 0x00219A07 File Offset: 0x00217C07
	public PinnedBuffer(int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.InitBuffer(new T[length], length);
	}

	// Token: 0x060057E4 RID: 22500 RVA: 0x00219A2D File Offset: 0x00217C2D
	public PinnedBuffer(T[] buffer, int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.InitBuffer(buffer, length);
	}

	// Token: 0x060057E5 RID: 22501 RVA: 0x00219A4E File Offset: 0x00217C4E
	public PinnedBuffer(MemoryPooledArray<T> pool, T[] poolArray, int length)
	{
		this.m_pool = pool;
		this.m_poolArray = poolArray;
		this.InitBuffer(poolArray, length);
	}

	// Token: 0x060057E6 RID: 22502 RVA: 0x00219A6C File Offset: 0x00217C6C
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe void InitBuffer(T[] buffer, int length)
	{
		this.m_ptr = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(buffer, out this.m_gcHandle);
		this.m_length = length;
	}

	// Token: 0x060057E7 RID: 22503 RVA: 0x00219A88 File Offset: 0x00217C88
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

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x060057E8 RID: 22504 RVA: 0x00219AE8 File Offset: 0x00217CE8
	public unsafe T* Ptr
	{
		get
		{
			return this.m_ptr;
		}
	}

	// Token: 0x1700093B RID: 2363
	// (get) Token: 0x060057E9 RID: 22505 RVA: 0x00219AF0 File Offset: 0x00217CF0
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x060057EA RID: 22506 RVA: 0x00219AF8 File Offset: 0x00217CF8
	public PinnedBufferRef<T> AsRef()
	{
		return new PinnedBufferRef<T>(this.m_ptr, this.m_length);
	}

	// Token: 0x060057EB RID: 22507 RVA: 0x00219B0B File Offset: 0x00217D0B
	public static implicit operator PinnedBufferRef<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsRef();
	}

	// Token: 0x060057EC RID: 22508 RVA: 0x00219B14 File Offset: 0x00217D14
	public PinnedBufferRef<byte> AsBytes()
	{
		return this.AsRef().AsBytes();
	}

	// Token: 0x060057ED RID: 22509 RVA: 0x00219B2F File Offset: 0x00217D2F
	public static implicit operator PinnedBufferRef<byte>(PinnedBuffer<T> buffer)
	{
		return buffer.AsBytes();
	}

	// Token: 0x060057EE RID: 22510 RVA: 0x00219B37 File Offset: 0x00217D37
	public unsafe Span<T> AsSpan()
	{
		return new Span<T>((void*)this.m_ptr, this.m_length);
	}

	// Token: 0x060057EF RID: 22511 RVA: 0x00219B4A File Offset: 0x00217D4A
	public static implicit operator Span<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsSpan();
	}

	// Token: 0x060057F0 RID: 22512 RVA: 0x00219B52 File Offset: 0x00217D52
	public unsafe ReadOnlySpan<T> AsReadOnlySpan()
	{
		return new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
	}

	// Token: 0x060057F1 RID: 22513 RVA: 0x00219B65 File Offset: 0x00217D65
	public static implicit operator ReadOnlySpan<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsReadOnlySpan();
	}

	// Token: 0x060057F2 RID: 22514 RVA: 0x00219B6D File Offset: 0x00217D6D
	public AtomicSafeHandleScope CreateNativeArray(out NativeArray<T> array)
	{
		return MeshDataUtils.CreateNativeArray<T>(this.m_ptr, this.m_length, out array);
	}

	// Token: 0x040043DA RID: 17370
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledArray<T> m_pool;

	// Token: 0x040043DB RID: 17371
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] m_poolArray;

	// Token: 0x040043DC RID: 17372
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* m_ptr;

	// Token: 0x040043DD RID: 17373
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_length;

	// Token: 0x040043DE RID: 17374
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong m_gcHandle;
}
