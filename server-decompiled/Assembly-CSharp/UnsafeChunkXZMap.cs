using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CD3 RID: 3283
public struct UnsafeChunkXZMap<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17000A91 RID: 2705
	// (get) Token: 0x0600645A RID: 25690 RVA: 0x00277DD4 File Offset: 0x00275FD4
	public bool IsCreated
	{
		get
		{
			return this.map != null;
		}
	}

	// Token: 0x0600645B RID: 25691 RVA: 0x00277DE3 File Offset: 0x00275FE3
	public UnsafeChunkXZMap(AllocatorManager.AllocatorHandle _allocator)
	{
		this.allocator = _allocator;
		this.map = AllocatorManager.Allocate<T>(_allocator, 256);
	}

	// Token: 0x0600645C RID: 25692 RVA: 0x00277DFD File Offset: 0x00275FFD
	public unsafe T Get(int _x, int _z)
	{
		return this.map[(IntPtr)UnsafeChunkXZMap<T>.GetMapIndex(_x, _z) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
	}

	// Token: 0x0600645D RID: 25693 RVA: 0x00277E1A File Offset: 0x0027601A
	public unsafe void Set(int _x, int _z, T value)
	{
		this.map[(IntPtr)UnsafeChunkXZMap<T>.GetMapIndex(_x, _z) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
	}

	// Token: 0x0600645E RID: 25694 RVA: 0x00277E38 File Offset: 0x00276038
	public unsafe void Clear()
	{
		if (this.map != null)
		{
			UnsafeUtility.MemClear((void*)this.map, (long)(256 * UnsafeUtility.SizeOf<T>()));
		}
	}

	// Token: 0x0600645F RID: 25695 RVA: 0x00277E5B File Offset: 0x0027605B
	public void Dispose()
	{
		if (this.map != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.map, 256);
		}
	}

	// Token: 0x06006460 RID: 25696 RVA: 0x00277E80 File Offset: 0x00276080
	public int CalculateOwnedBytes()
	{
		int num = UnsafeUtility.SizeOf<UnsafeChunkXZMap<T>>();
		if (this.map != null)
		{
			num += CollectionHelper.Align(256 * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
		}
		return num;
	}

	// Token: 0x06006461 RID: 25697 RVA: 0x00277EB6 File Offset: 0x002760B6
	public static int GetMapIndex(int _x, int _z)
	{
		return _x + 16 * _z;
	}

	// Token: 0x06006462 RID: 25698 RVA: 0x00277EBE File Offset: 0x002760BE
	public static void GetMapCoords(int _index, out int _x, out int _z)
	{
		_z = _index / 16;
		_x = _index % 16;
	}

	// Token: 0x04004E59 RID: 20057
	public const int MAP_SIZE = 256;

	// Token: 0x04004E5A RID: 20058
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* map;

	// Token: 0x04004E5B RID: 20059
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;
}
