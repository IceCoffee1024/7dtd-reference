using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CDE RID: 3294
public struct UnsafeChunkXZMap<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x0600647F RID: 25727 RVA: 0x002792FC File Offset: 0x002774FC
	public bool IsCreated
	{
		get
		{
			return this.map != null;
		}
	}

	// Token: 0x06006480 RID: 25728 RVA: 0x0027930B File Offset: 0x0027750B
	public UnsafeChunkXZMap(AllocatorManager.AllocatorHandle _allocator)
	{
		this.allocator = _allocator;
		this.map = AllocatorManager.Allocate<T>(_allocator, 256);
	}

	// Token: 0x06006481 RID: 25729 RVA: 0x00279325 File Offset: 0x00277525
	public unsafe T Get(int _x, int _z)
	{
		return this.map[(IntPtr)UnsafeChunkXZMap<T>.GetMapIndex(_x, _z) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
	}

	// Token: 0x06006482 RID: 25730 RVA: 0x00279342 File Offset: 0x00277542
	public unsafe void Set(int _x, int _z, T value)
	{
		this.map[(IntPtr)UnsafeChunkXZMap<T>.GetMapIndex(_x, _z) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
	}

	// Token: 0x06006483 RID: 25731 RVA: 0x00279360 File Offset: 0x00277560
	public unsafe void Clear()
	{
		if (this.map != null)
		{
			UnsafeUtility.MemClear((void*)this.map, (long)(256 * UnsafeUtility.SizeOf<T>()));
		}
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x00279383 File Offset: 0x00277583
	public void Dispose()
	{
		if (this.map != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.map, 256);
		}
	}

	// Token: 0x06006485 RID: 25733 RVA: 0x002793A8 File Offset: 0x002775A8
	public int CalculateOwnedBytes()
	{
		int num = UnsafeUtility.SizeOf<UnsafeChunkXZMap<T>>();
		if (this.map != null)
		{
			num += CollectionHelper.Align(256 * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
		}
		return num;
	}

	// Token: 0x06006486 RID: 25734 RVA: 0x002793DE File Offset: 0x002775DE
	public static int GetMapIndex(int _x, int _z)
	{
		return _x + 16 * _z;
	}

	// Token: 0x06006487 RID: 25735 RVA: 0x002793E6 File Offset: 0x002775E6
	public static void GetMapCoords(int _index, out int _x, out int _z)
	{
		_z = _index / 16;
		_x = _index % 16;
	}

	// Token: 0x04004E7E RID: 20094
	public const int MAP_SIZE = 256;

	// Token: 0x04004E7F RID: 20095
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* map;

	// Token: 0x04004E80 RID: 20096
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;
}
