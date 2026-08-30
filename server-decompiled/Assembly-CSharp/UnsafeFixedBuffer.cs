using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CC6 RID: 3270
public struct UnsafeFixedBuffer<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x06006470 RID: 25712 RVA: 0x00274AC8 File Offset: 0x00272CC8
	public unsafe int Count
	{
		get
		{
			return this.data->count;
		}
	}

	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x06006471 RID: 25713 RVA: 0x00274AD5 File Offset: 0x00272CD5
	public bool IsCreated
	{
		get
		{
			return this.data != null;
		}
	}

	// Token: 0x06006472 RID: 25714 RVA: 0x00274AE4 File Offset: 0x00272CE4
	public unsafe UnsafeFixedBuffer(int _capacity, AllocatorManager.AllocatorHandle _allocator)
	{
		this.data = AllocatorManager.Allocate<UnsafeFixedBuffer<T>.Data>(_allocator, 1);
		this.data->buffer = AllocatorManager.Allocate<T>(_allocator, _capacity);
		this.data->count = 0;
		this.capacity = _capacity;
		this.allocator = _allocator;
	}

	// Token: 0x06006473 RID: 25715 RVA: 0x00274B20 File Offset: 0x00272D20
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

	// Token: 0x06006474 RID: 25716 RVA: 0x00274B98 File Offset: 0x00272D98
	public unsafe void Clear()
	{
		this.data->count = 0;
	}

	// Token: 0x06006475 RID: 25717 RVA: 0x00274BA6 File Offset: 0x00272DA6
	public unsafe NativeArray<T> AsNativeArray()
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.data->buffer, this.data->count, Allocator.Invalid);
	}

	// Token: 0x06006476 RID: 25718 RVA: 0x00274BC4 File Offset: 0x00272DC4
	public unsafe void Dispose()
	{
		if (this.data != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.data->buffer, this.capacity);
			AllocatorManager.Free<UnsafeFixedBuffer<T>.Data>(this.allocator, this.data, 1);
			this.data = null;
		}
	}

	// Token: 0x06006477 RID: 25719 RVA: 0x00274C11 File Offset: 0x00272E11
	public int CalculateOwnedBytes()
	{
		return UnsafeUtility.SizeOf<UnsafeFixedBuffer<T>>() + CollectionHelper.Align(this.capacity * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
	}

	// Token: 0x04004E26 RID: 20006
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe UnsafeFixedBuffer<T>.Data* data;

	// Token: 0x04004E27 RID: 20007
	[PublicizedFrom(EAccessModifier.Private)]
	public int capacity;

	// Token: 0x04004E28 RID: 20008
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;

	// Token: 0x02000CC7 RID: 3271
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Data
	{
		// Token: 0x04004E29 RID: 20009
		public unsafe T* buffer;

		// Token: 0x04004E2A RID: 20010
		public int count;
	}
}
