using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CB3 RID: 3251
public struct UnsafeChunkXZMap<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x060063DB RID: 25563 RVA: 0x00271AD4 File Offset: 0x0026FCD4
	public bool IsCreated
	{
		get
		{
			return this.map != null;
		}
	}

	// Token: 0x060063DC RID: 25564 RVA: 0x00271AE3 File Offset: 0x0026FCE3
	public UnsafeChunkXZMap(AllocatorManager.AllocatorHandle _allocator)
	{
		this.allocator = _allocator;
		this.map = AllocatorManager.Allocate<T>(_allocator, 256);
	}

	// Token: 0x060063DD RID: 25565 RVA: 0x00271AFD File Offset: 0x0026FCFD
	public unsafe T Get(int _x, int _z)
	{
		return this.map[(IntPtr)UnsafeChunkXZMap<T>.GetMapIndex(_x, _z) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
	}

	// Token: 0x060063DE RID: 25566 RVA: 0x00271B1A File Offset: 0x0026FD1A
	public unsafe void Set(int _x, int _z, T value)
	{
		this.map[(IntPtr)UnsafeChunkXZMap<T>.GetMapIndex(_x, _z) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
	}

	// Token: 0x060063DF RID: 25567 RVA: 0x00271B38 File Offset: 0x0026FD38
	public unsafe void Clear()
	{
		if (this.map != null)
		{
			UnsafeUtility.MemClear((void*)this.map, (long)(256 * UnsafeUtility.SizeOf<T>()));
		}
	}

	// Token: 0x060063E0 RID: 25568 RVA: 0x00271B5B File Offset: 0x0026FD5B
	public void Dispose()
	{
		if (this.map != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.map, 256);
		}
	}

	// Token: 0x060063E1 RID: 25569 RVA: 0x00271B80 File Offset: 0x0026FD80
	public int CalculateOwnedBytes()
	{
		int num = UnsafeUtility.SizeOf<UnsafeChunkXZMap<T>>();
		if (this.map != null)
		{
			num += CollectionHelper.Align(256 * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
		}
		return num;
	}

	// Token: 0x060063E2 RID: 25570 RVA: 0x00271BB6 File Offset: 0x0026FDB6
	public static int GetMapIndex(int _x, int _z)
	{
		return _x + 16 * _z;
	}

	// Token: 0x060063E3 RID: 25571 RVA: 0x00271BBE File Offset: 0x0026FDBE
	public static void GetMapCoords(int _index, out int _x, out int _z)
	{
		_z = _index / 16;
		_x = _index % 16;
	}

	// Token: 0x04004D85 RID: 19845
	public const int MAP_SIZE = 256;

	// Token: 0x04004D86 RID: 19846
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* map;

	// Token: 0x04004D87 RID: 19847
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;
}
