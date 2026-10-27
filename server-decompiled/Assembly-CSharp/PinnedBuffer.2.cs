using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000B87 RID: 2951
public sealed class PinnedBuffer<[IsUnmanaged] T> : PinnedBuffer where T : struct, ValueType
{
	// Token: 0x060058A8 RID: 22696 RVA: 0x00220EB8 File Offset: 0x0021F0B8
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

	// Token: 0x060058A9 RID: 22697 RVA: 0x00220EF7 File Offset: 0x0021F0F7
	public PinnedBuffer()
	{
		this.InitBuffer(Array.Empty<T>(), 0);
	}

	// Token: 0x060058AA RID: 22698 RVA: 0x00220F0B File Offset: 0x0021F10B
	public PinnedBuffer(int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.InitBuffer(new T[length], length);
	}

	// Token: 0x060058AB RID: 22699 RVA: 0x00220F31 File Offset: 0x0021F131
	public PinnedBuffer(T[] buffer, int length)
	{
		if (length <= 0)
		{
			this.InitBuffer(Array.Empty<T>(), 0);
			return;
		}
		this.InitBuffer(buffer, length);
	}

	// Token: 0x060058AC RID: 22700 RVA: 0x00220F52 File Offset: 0x0021F152
	public PinnedBuffer(MemoryPooledArray<T> pool, T[] poolArray, int length)
	{
		this.m_pool = pool;
		this.m_poolArray = poolArray;
		this.InitBuffer(poolArray, length);
	}

	// Token: 0x060058AD RID: 22701 RVA: 0x00220F70 File Offset: 0x0021F170
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe void InitBuffer(T[] buffer, int length)
	{
		this.m_ptr = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(buffer, out this.m_gcHandle);
		this.m_length = length;
	}

	// Token: 0x060058AE RID: 22702 RVA: 0x00220F8C File Offset: 0x0021F18C
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

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x060058AF RID: 22703 RVA: 0x00220FEC File Offset: 0x0021F1EC
	public unsafe T* Ptr
	{
		get
		{
			return this.m_ptr;
		}
	}

	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x060058B0 RID: 22704 RVA: 0x00220FF4 File Offset: 0x0021F1F4
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x060058B1 RID: 22705 RVA: 0x00220FFC File Offset: 0x0021F1FC
	public PinnedBufferRef<T> AsRef()
	{
		return new PinnedBufferRef<T>(this.m_ptr, this.m_length);
	}

	// Token: 0x060058B2 RID: 22706 RVA: 0x0022100F File Offset: 0x0021F20F
	public static implicit operator PinnedBufferRef<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsRef();
	}

	// Token: 0x060058B3 RID: 22707 RVA: 0x00221018 File Offset: 0x0021F218
	public PinnedBufferRef<byte> AsBytes()
	{
		return this.AsRef().AsBytes();
	}

	// Token: 0x060058B4 RID: 22708 RVA: 0x00221033 File Offset: 0x0021F233
	public static implicit operator PinnedBufferRef<byte>(PinnedBuffer<T> buffer)
	{
		return buffer.AsBytes();
	}

	// Token: 0x060058B5 RID: 22709 RVA: 0x0022103B File Offset: 0x0021F23B
	public unsafe Span<T> AsSpan()
	{
		return new Span<T>((void*)this.m_ptr, this.m_length);
	}

	// Token: 0x060058B6 RID: 22710 RVA: 0x0022104E File Offset: 0x0021F24E
	public static implicit operator Span<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsSpan();
	}

	// Token: 0x060058B7 RID: 22711 RVA: 0x00221056 File Offset: 0x0021F256
	public unsafe ReadOnlySpan<T> AsReadOnlySpan()
	{
		return new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
	}

	// Token: 0x060058B8 RID: 22712 RVA: 0x00221069 File Offset: 0x0021F269
	public static implicit operator ReadOnlySpan<T>(PinnedBuffer<T> buffer)
	{
		return buffer.AsReadOnlySpan();
	}

	// Token: 0x060058B9 RID: 22713 RVA: 0x00221071 File Offset: 0x0021F271
	public AtomicSafeHandleScope CreateNativeArray(out NativeArray<T> array)
	{
		return MeshDataUtils.CreateNativeArray<T>(this.m_ptr, this.m_length, out array);
	}

	// Token: 0x040044D8 RID: 17624
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledArray<T> m_pool;

	// Token: 0x040044D9 RID: 17625
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] m_poolArray;

	// Token: 0x040044DA RID: 17626
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* m_ptr;

	// Token: 0x040044DB RID: 17627
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_length;

	// Token: 0x040044DC RID: 17628
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong m_gcHandle;
}
