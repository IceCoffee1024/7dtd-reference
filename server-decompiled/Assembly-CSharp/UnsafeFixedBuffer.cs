using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CDF RID: 3295
public struct UnsafeFixedBuffer<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x06006488 RID: 25736 RVA: 0x002793F4 File Offset: 0x002775F4
	public unsafe int Count
	{
		get
		{
			return this.data->count;
		}
	}

	// Token: 0x17000A99 RID: 2713
	// (get) Token: 0x06006489 RID: 25737 RVA: 0x00279401 File Offset: 0x00277601
	public bool IsCreated
	{
		get
		{
			return this.data != null;
		}
	}

	// Token: 0x0600648A RID: 25738 RVA: 0x00279410 File Offset: 0x00277610
	public unsafe UnsafeFixedBuffer(int _capacity, AllocatorManager.AllocatorHandle _allocator)
	{
		this.data = AllocatorManager.Allocate<UnsafeFixedBuffer<T>.Data>(_allocator, 1);
		this.data->buffer = AllocatorManager.Allocate<T>(_allocator, _capacity);
		this.data->count = 0;
		this.capacity = _capacity;
		this.allocator = _allocator;
	}

	// Token: 0x0600648B RID: 25739 RVA: 0x0027944C File Offset: 0x0027764C
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

	// Token: 0x0600648C RID: 25740 RVA: 0x002794C4 File Offset: 0x002776C4
	public unsafe void Clear()
	{
		this.data->count = 0;
	}

	// Token: 0x0600648D RID: 25741 RVA: 0x002794D2 File Offset: 0x002776D2
	public unsafe NativeArray<T> AsNativeArray()
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.data->buffer, this.data->count, Allocator.Invalid);
	}

	// Token: 0x0600648E RID: 25742 RVA: 0x002794F0 File Offset: 0x002776F0
	public unsafe void Dispose()
	{
		if (this.data != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.data->buffer, this.capacity);
			AllocatorManager.Free<UnsafeFixedBuffer<T>.Data>(this.allocator, this.data, 1);
			this.data = null;
		}
	}

	// Token: 0x0600648F RID: 25743 RVA: 0x0027953D File Offset: 0x0027773D
	public int CalculateOwnedBytes()
	{
		return UnsafeUtility.SizeOf<UnsafeFixedBuffer<T>>() + CollectionHelper.Align(this.capacity * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
	}

	// Token: 0x04004E81 RID: 20097
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe UnsafeFixedBuffer<T>.Data* data;

	// Token: 0x04004E82 RID: 20098
	[PublicizedFrom(EAccessModifier.Private)]
	public int capacity;

	// Token: 0x04004E83 RID: 20099
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;

	// Token: 0x02000CE0 RID: 3296
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Data
	{
		// Token: 0x04004E84 RID: 20100
		public unsafe T* buffer;

		// Token: 0x04004E85 RID: 20101
		public int count;
	}
}
