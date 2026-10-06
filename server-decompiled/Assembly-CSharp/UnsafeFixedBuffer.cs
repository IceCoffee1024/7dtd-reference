using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CD4 RID: 3284
public struct UnsafeFixedBuffer<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x06006463 RID: 25699 RVA: 0x00277ECC File Offset: 0x002760CC
	public unsafe int Count
	{
		get
		{
			return this.data->count;
		}
	}

	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x06006464 RID: 25700 RVA: 0x00277ED9 File Offset: 0x002760D9
	public bool IsCreated
	{
		get
		{
			return this.data != null;
		}
	}

	// Token: 0x06006465 RID: 25701 RVA: 0x00277EE8 File Offset: 0x002760E8
	public unsafe UnsafeFixedBuffer(int _capacity, AllocatorManager.AllocatorHandle _allocator)
	{
		this.data = AllocatorManager.Allocate<UnsafeFixedBuffer<T>.Data>(_allocator, 1);
		this.data->buffer = AllocatorManager.Allocate<T>(_allocator, _capacity);
		this.data->count = 0;
		this.capacity = _capacity;
		this.allocator = _allocator;
	}

	// Token: 0x06006466 RID: 25702 RVA: 0x00277F24 File Offset: 0x00276124
	public unsafe void AddThreadSafe(T item)
	{
		int count;
		for (;;)
		{
			count = this.data->count;
			int num = this.data->count + 1;
			if (num > this.capacity)
			{
				break;
			}
			if (Interlocked.CompareExchange(ref this.data->count, num, count) == count)
			{
				goto Block_1;
			}
		}
		throw new IndexOutOfRangeException(string.Format("Index {0} is outside the UnsafeFixedBuffer capacity {1}", count, this.capacity));
		Block_1:
		UnsafeUtility.WriteArrayElement<T>((void*)this.data->buffer, count, item);
	}

	// Token: 0x06006467 RID: 25703 RVA: 0x00277F9C File Offset: 0x0027619C
	public unsafe void Clear()
	{
		this.data->count = 0;
	}

	// Token: 0x06006468 RID: 25704 RVA: 0x00277FAA File Offset: 0x002761AA
	public unsafe NativeArray<T> AsNativeArray()
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.data->buffer, this.data->count, Allocator.Invalid);
	}

	// Token: 0x06006469 RID: 25705 RVA: 0x00277FC8 File Offset: 0x002761C8
	public unsafe void Dispose()
	{
		if (this.data != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.data->buffer, this.capacity);
			AllocatorManager.Free<UnsafeFixedBuffer<T>.Data>(this.allocator, this.data, 1);
			this.data = null;
		}
	}

	// Token: 0x0600646A RID: 25706 RVA: 0x00278015 File Offset: 0x00276215
	public int CalculateOwnedBytes()
	{
		return UnsafeUtility.SizeOf<UnsafeFixedBuffer<T>>() + CollectionHelper.Align(this.capacity * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
	}

	// Token: 0x04004E5C RID: 20060
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe UnsafeFixedBuffer<T>.Data* data;

	// Token: 0x04004E5D RID: 20061
	[PublicizedFrom(EAccessModifier.Private)]
	public int capacity;

	// Token: 0x04004E5E RID: 20062
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;

	// Token: 0x02000CD5 RID: 3285
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Data
	{
		// Token: 0x04004E5F RID: 20063
		public unsafe T* buffer;

		// Token: 0x04004E60 RID: 20064
		public int count;
	}
}
