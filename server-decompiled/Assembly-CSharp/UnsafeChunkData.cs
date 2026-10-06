using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CD1 RID: 3281
public struct UnsafeChunkData<[IsUnmanaged] T> : IDisposable where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x17000A90 RID: 2704
	// (get) Token: 0x06006450 RID: 25680 RVA: 0x00277995 File Offset: 0x00275B95
	public bool IsCreated
	{
		get
		{
			return this.layers != null;
		}
	}

	// Token: 0x06006451 RID: 25681 RVA: 0x002779A4 File Offset: 0x00275BA4
	public unsafe UnsafeChunkData(AllocatorManager.AllocatorHandle _allocator)
	{
		this.allocator = _allocator;
		this.layers = AllocatorManager.Allocate<UnsafeChunkData<T>.LayerData>(this.allocator, 64);
		UnsafeUtility.MemClear((void*)this.layers, (long)(UnsafeUtility.SizeOf<UnsafeChunkData<T>.LayerData>() * 64));
		this.sameValues = AllocatorManager.Allocate<T>(this.allocator, 64);
		UnsafeUtility.MemClear((void*)this.sameValues, (long)(UnsafeUtility.SizeOf<T>() * 64));
	}

	// Token: 0x06006452 RID: 25682 RVA: 0x00277A08 File Offset: 0x00275C08
	public unsafe T Get(int _x, int _y, int _z)
	{
		int num = _y / 4;
		UnsafeChunkData<T>.LayerData* ptr = this.layers + (IntPtr)num * (IntPtr)sizeof(UnsafeChunkData<T>.LayerData) / (IntPtr)sizeof(UnsafeChunkData<T>.LayerData);
		if (ptr->items == null)
		{
			return this.sameValues[(IntPtr)num * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}
		int num2 = _x + 16 * _z;
		int num3 = _y % 4;
		int num4 = num2 + num3 * 256;
		return ptr->items[(IntPtr)num4 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
	}

	// Token: 0x06006453 RID: 25683 RVA: 0x00277A74 File Offset: 0x00275C74
	public unsafe T Get(int _chunkIndex)
	{
		int num = _chunkIndex / 256;
		int num2 = num / 4;
		UnsafeChunkData<T>.LayerData* ptr = this.layers + (IntPtr)num2 * (IntPtr)sizeof(UnsafeChunkData<T>.LayerData) / (IntPtr)sizeof(UnsafeChunkData<T>.LayerData);
		if (ptr->items == null)
		{
			return this.sameValues[(IntPtr)num2 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}
		int num3 = _chunkIndex % 256;
		int num4 = num % 4;
		int num5 = num3 + num4 * 256;
		return ptr->items[(IntPtr)num5 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
	}

	// Token: 0x06006454 RID: 25684 RVA: 0x00277AEC File Offset: 0x00275CEC
	public void CheckSameValues()
	{
		for (int i = 0; i < 64; i++)
		{
			this.CheckSameValue(i);
		}
	}

	// Token: 0x06006455 RID: 25685 RVA: 0x00277B10 File Offset: 0x00275D10
	public unsafe void CheckSameValue(int _layerIndex)
	{
		UnsafeChunkData<T>.LayerData* ptr = this.layers + (IntPtr)_layerIndex * (IntPtr)sizeof(UnsafeChunkData<T>.LayerData) / (IntPtr)sizeof(UnsafeChunkData<T>.LayerData);
		T* items = ptr->items;
		if (items == null)
		{
			return;
		}
		if (items != null)
		{
			T t = *items;
			for (int i = 1; i < 1024; i++)
			{
				if (!t.Equals(items[(IntPtr)i * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)]))
				{
					return;
				}
			}
			this.sameValues[(IntPtr)_layerIndex * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = t;
			AllocatorManager.Free<T>(this.allocator, ptr->items, 1024);
			ptr->items = null;
		}
	}

	// Token: 0x06006456 RID: 25686 RVA: 0x00277BAC File Offset: 0x00275DAC
	public unsafe void Set(int _chunkIndex, T _value)
	{
		int num = _chunkIndex / 256;
		int num2 = num / 4;
		UnsafeChunkData<T>.LayerData* ptr = this.layers + (IntPtr)num2 * (IntPtr)sizeof(UnsafeChunkData<T>.LayerData) / (IntPtr)sizeof(UnsafeChunkData<T>.LayerData);
		if (ptr->items == null)
		{
			T* ptr2 = AllocatorManager.Allocate<T>(this.allocator, 1024);
			T t = this.sameValues[(IntPtr)num2 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			for (int i = 0; i < 1024; i++)
			{
				ptr2[(IntPtr)i * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = t;
			}
			ptr->items = ptr2;
		}
		int num3 = _chunkIndex % 256;
		int num4 = num % 4;
		int num5 = num3 + num4 * 256;
		ptr->items[(IntPtr)num5 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = _value;
	}

	// Token: 0x06006457 RID: 25687 RVA: 0x00277C68 File Offset: 0x00275E68
	public unsafe void Clear()
	{
		if (this.layers != null)
		{
			for (int i = 0; i < 64; i++)
			{
				UnsafeChunkData<T>.LayerData* ptr = this.layers + (IntPtr)i * (IntPtr)sizeof(UnsafeChunkData<T>.LayerData) / (IntPtr)sizeof(UnsafeChunkData<T>.LayerData);
				if (ptr->items != null)
				{
					AllocatorManager.Free<T>(this.allocator, ptr->items, 1024);
					ptr->items = null;
				}
			}
		}
		if (this.sameValues != null)
		{
			UnsafeUtility.MemClear((void*)this.sameValues, (long)(UnsafeUtility.SizeOf<T>() * 64));
		}
	}

	// Token: 0x06006458 RID: 25688 RVA: 0x00277CE4 File Offset: 0x00275EE4
	public void Dispose()
	{
		this.Clear();
		if (this.layers != null)
		{
			AllocatorManager.Free<UnsafeChunkData<T>.LayerData>(this.allocator, this.layers, 64);
			this.layers = null;
		}
		if (this.sameValues != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.sameValues, 64);
			this.sameValues = null;
		}
	}

	// Token: 0x06006459 RID: 25689 RVA: 0x00277D44 File Offset: 0x00275F44
	public unsafe int CalculateOwnedBytes()
	{
		int num = UnsafeUtility.SizeOf<UnsafeChunkData<T>>();
		if (this.layers != null)
		{
			num += CollectionHelper.Align(64 * UnsafeUtility.SizeOf<UnsafeChunkData<T>.LayerData>(), UnsafeUtility.AlignOf<UnsafeChunkData<T>.LayerData>());
			for (int i = 0; i < 64; i++)
			{
				if (this.layers[(IntPtr)i * (IntPtr)sizeof(UnsafeChunkData<T>.LayerData) / (IntPtr)sizeof(UnsafeChunkData<T>.LayerData)].items != null)
				{
					num += CollectionHelper.Align(1024 * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
				}
			}
		}
		if (this.sameValues != null)
		{
			num += CollectionHelper.Align(64 * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
		}
		return num;
	}

	// Token: 0x04004E53 RID: 20051
	public const int LAYER_SIZE = 1024;

	// Token: 0x04004E54 RID: 20052
	public const int NUM_LAYERS = 64;

	// Token: 0x04004E55 RID: 20053
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe UnsafeChunkData<T>.LayerData* layers;

	// Token: 0x04004E56 RID: 20054
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* sameValues;

	// Token: 0x04004E57 RID: 20055
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;

	// Token: 0x02000CD2 RID: 3282
	[PublicizedFrom(EAccessModifier.Private)]
	public struct LayerData
	{
		// Token: 0x04004E58 RID: 20056
		public unsafe T* items;
	}
}
