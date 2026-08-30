using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CB4 RID: 3252
public struct UnsafeFixedBuffer<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x060063E4 RID: 25572 RVA: 0x00271BCC File Offset: 0x0026FDCC
	public unsafe int Count
	{
		get
		{
			return this.data->count;
		}
	}

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x060063E5 RID: 25573 RVA: 0x00271BD9 File Offset: 0x0026FDD9
	public bool IsCreated
	{
		get
		{
			return this.data != null;
		}
	}

	// Token: 0x060063E6 RID: 25574 RVA: 0x00271BE8 File Offset: 0x0026FDE8
	public unsafe UnsafeFixedBuffer(int _capacity, AllocatorManager.AllocatorHandle _allocator)
	{
		this.data = AllocatorManager.Allocate<UnsafeFixedBuffer<T>.Data>(_allocator, 1);
		this.data->buffer = AllocatorManager.Allocate<T>(_allocator, _capacity);
		this.data->count = 0;
		this.capacity = _capacity;
		this.allocator = _allocator;
	}

	// Token: 0x060063E7 RID: 25575 RVA: 0x00271C24 File Offset: 0x0026FE24
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

	// Token: 0x060063E8 RID: 25576 RVA: 0x00271C9C File Offset: 0x0026FE9C
	public unsafe void Clear()
	{
		this.data->count = 0;
	}

	// Token: 0x060063E9 RID: 25577 RVA: 0x00271CAA File Offset: 0x0026FEAA
	public unsafe NativeArray<T> AsNativeArray()
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.data->buffer, this.data->count, Allocator.Invalid);
	}

	// Token: 0x060063EA RID: 25578 RVA: 0x00271CC8 File Offset: 0x0026FEC8
	public unsafe void Dispose()
	{
		if (this.data != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.data->buffer, this.capacity);
			AllocatorManager.Free<UnsafeFixedBuffer<T>.Data>(this.allocator, this.data, 1);
			this.data = null;
		}
	}

	// Token: 0x060063EB RID: 25579 RVA: 0x00271D15 File Offset: 0x0026FF15
	public int CalculateOwnedBytes()
	{
		return UnsafeUtility.SizeOf<UnsafeFixedBuffer<T>>() + CollectionHelper.Align(this.capacity * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
	}

	// Token: 0x04004D88 RID: 19848
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe UnsafeFixedBuffer<T>.Data* data;

	// Token: 0x04004D89 RID: 19849
	[PublicizedFrom(EAccessModifier.Private)]
	public int capacity;

	// Token: 0x04004D8A RID: 19850
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;

	// Token: 0x02000CB5 RID: 3253
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Data
	{
		// Token: 0x04004D8B RID: 19851
		public unsafe T* buffer;

		// Token: 0x04004D8C RID: 19852
		public int count;
	}
}
