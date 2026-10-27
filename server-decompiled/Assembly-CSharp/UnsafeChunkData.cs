using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CDC RID: 3292
public struct UnsafeChunkData<[IsUnmanaged] T> : IDisposable where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x06006475 RID: 25717 RVA: 0x00278EBD File Offset: 0x002770BD
	public bool IsCreated
	{
		get
		{
			return this.layers != null;
		}
	}

	// Token: 0x06006476 RID: 25718 RVA: 0x00278ECC File Offset: 0x002770CC
	public unsafe UnsafeChunkData(AllocatorManager.AllocatorHandle _allocator)
	{
		this.allocator = _allocator;
		this.layers = AllocatorManager.Allocate<UnsafeChunkData<T>.LayerData>(this.allocator, 64);
		UnsafeUtility.MemClear((void*)this.layers, (long)(UnsafeUtility.SizeOf<UnsafeChunkData<T>.LayerData>() * 64));
		this.sameValues = AllocatorManager.Allocate<T>(this.allocator, 64);
		UnsafeUtility.MemClear((void*)this.sameValues, (long)(UnsafeUtility.SizeOf<T>() * 64));
	}

	// Token: 0x06006477 RID: 25719 RVA: 0x00278F30 File Offset: 0x00277130
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

	// Token: 0x06006478 RID: 25720 RVA: 0x00278F9C File Offset: 0x0027719C
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

	// Token: 0x06006479 RID: 25721 RVA: 0x00279014 File Offset: 0x00277214
	public void CheckSameValues()
	{
		for (int i = 0; i < 64; i++)
		{
			this.CheckSameValue(i);
		}
	}

	// Token: 0x0600647A RID: 25722 RVA: 0x00279038 File Offset: 0x00277238
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

	// Token: 0x0600647B RID: 25723 RVA: 0x002790D4 File Offset: 0x002772D4
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

	// Token: 0x0600647C RID: 25724 RVA: 0x00279190 File Offset: 0x00277390
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

	// Token: 0x0600647D RID: 25725 RVA: 0x0027920C File Offset: 0x0027740C
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

	// Token: 0x0600647E RID: 25726 RVA: 0x0027926C File Offset: 0x0027746C
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

	// Token: 0x04004E78 RID: 20088
	public const int LAYER_SIZE = 1024;

	// Token: 0x04004E79 RID: 20089
	public const int NUM_LAYERS = 64;

	// Token: 0x04004E7A RID: 20090
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe UnsafeChunkData<T>.LayerData* layers;

	// Token: 0x04004E7B RID: 20091
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* sameValues;

	// Token: 0x04004E7C RID: 20092
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;

	// Token: 0x02000CDD RID: 3293
	[PublicizedFrom(EAccessModifier.Private)]
	public struct LayerData
	{
		// Token: 0x04004E7D RID: 20093
		public unsafe T* items;
	}
}
