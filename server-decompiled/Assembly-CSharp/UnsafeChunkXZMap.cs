using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CC5 RID: 3269
public struct UnsafeChunkXZMap<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x06006467 RID: 25703 RVA: 0x002749D0 File Offset: 0x00272BD0
	public bool IsCreated
	{
		get
		{
			return this.map != null;
		}
	}

	// Token: 0x06006468 RID: 25704 RVA: 0x002749DF File Offset: 0x00272BDF
	public UnsafeChunkXZMap(AllocatorManager.AllocatorHandle _allocator)
	{
		this.allocator = _allocator;
		this.map = AllocatorManager.Allocate<T>(_allocator, 256);
	}

	// Token: 0x06006469 RID: 25705 RVA: 0x002749F9 File Offset: 0x00272BF9
	public unsafe T Get(int _x, int _z)
	{
		return this.map[(IntPtr)UnsafeChunkXZMap<T>.GetMapIndex(_x, _z) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
	}

	// Token: 0x0600646A RID: 25706 RVA: 0x00274A16 File Offset: 0x00272C16
	public unsafe void Set(int _x, int _z, T value)
	{
		this.map[(IntPtr)UnsafeChunkXZMap<T>.GetMapIndex(_x, _z) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
	}

	// Token: 0x0600646B RID: 25707 RVA: 0x00274A34 File Offset: 0x00272C34
	public unsafe void Clear()
	{
		if (this.map != null)
		{
			UnsafeUtility.MemClear((void*)this.map, (long)(256 * UnsafeUtility.SizeOf<T>()));
		}
	}

	// Token: 0x0600646C RID: 25708 RVA: 0x00274A57 File Offset: 0x00272C57
	public void Dispose()
	{
		if (this.map != null)
		{
			AllocatorManager.Free<T>(this.allocator, this.map, 256);
		}
	}

	// Token: 0x0600646D RID: 25709 RVA: 0x00274A7C File Offset: 0x00272C7C
	public int CalculateOwnedBytes()
	{
		int num = UnsafeUtility.SizeOf<UnsafeChunkXZMap<T>>();
		if (this.map != null)
		{
			num += CollectionHelper.Align(256 * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
		}
		return num;
	}

	// Token: 0x0600646E RID: 25710 RVA: 0x00274AB2 File Offset: 0x00272CB2
	public static int GetMapIndex(int _x, int _z)
	{
		return _x + 16 * _z;
	}

	// Token: 0x0600646F RID: 25711 RVA: 0x00274ABA File Offset: 0x00272CBA
	public static void GetMapCoords(int _index, out int _x, out int _z)
	{
		_z = _index / 16;
		_x = _index % 16;
	}

	// Token: 0x04004E23 RID: 20003
	public const int MAP_SIZE = 256;

	// Token: 0x04004E24 RID: 20004
	[NativeDisableUnsafePtrRestriction]
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe T* map;

	// Token: 0x04004E25 RID: 20005
	[PublicizedFrom(EAccessModifier.Private)]
	public AllocatorManager.AllocatorHandle allocator;
}
