using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CC3 RID: 3267
public struct UnsafeChunkData<[IsUnmanaged] T> : IDisposable where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x0600645D RID: 25693 RVA: 0x00274591 File Offset: 0x00272791
	public bool IsCreated
	{
		get
		{
			return this.layers != null;
		}
	}

	// Token: 0x0600645E RID: 25694 RVA: 0x002745A0 File Offset: 0x002727A0
	public unsafe UnsafeChunkData(AllocatorManager.AllocatorHandle _allocator)
	{
		this.allocator = _allocator;
		this.layers = AllocatorManager.Allocate<UnsafeChunkData<T>.LayerData>(this.allocator, 64);
		UnsafeUtility.MemClear((void*)this.layers, (long)(UnsafeUtility.SizeOf<UnsafeChunkData<T>.LayerData>() * 64));
		this.sameValues = AllocatorManager.Allocate<T>(this.allocator, 64);
		UnsafeUtility.MemClear((void*)this.sameValues, (long)(UnsafeUtility.SizeOf<T>() * 64));
	}

	// Token: 0x0600645F RID: 25695 RVA: 0x00274604 File Offset: 0x00272804
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

	// Token: 0x06006460 RID: 25696 RVA: 0x00274670 File Offset: 0x00272870
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

	// Token: 0x06006461 RID: 25697 RVA: 0x002746E8 File Offset: 0x002728E8
	public void CheckSameValues()
	{
		for (int i = 0; i < 64; i++)
		{
			this.CheckSameValue(i);
		}
	}

	// Token: 0x06006462 RID: 25698 RVA: 0x0027470C File Offset: 0x0027290C
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

	// Token: 0x06006463 RID: 25699 RVA: 0x002747A8 File Offset: 0x002729A8
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

	// Token: 0x06006464 RID: 25700 RVA: 0x00274864 File Offset: 0x00272A64
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

	// Token: 0x06006465 RID: 25701 RVA: 0x002748E0 File Offset: 0x00272AE0
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

	// Token: 0x06006466 RID: 25702 RVA: 0x00274940 File Offset: 0x00272B40
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

	// Token: 0x04004E1D RID: 19997
	public const int LAYER_SIZE = 1024;

	// Token: 0x04004E1E RID: 19998
	public const int NUM_LAYERS = 64;

	// Token: 0x04004E1F RID: 19999
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe UnsafeChunkData<T>.LayerData* layers;

	// Token: 0x04004E20 RID: 20000
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* sameValues;

	// Token: 0x04004E21 RID: 20001
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;

	// Token: 0x02000CC4 RID: 3268
	[PublicizedFrom(EAccessModifier.Private)]
	public struct LayerData
	{
		// Token: 0x04004E22 RID: 20002
		public unsafe T* items;
	}
}
