using System;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x02001371 RID: 4977
public static class BackedArrays
{
	// Token: 0x06009CCE RID: 40142 RVA: 0x003B4664 File Offset: 0x003B2864
	[PublicizedFrom(EAccessModifier.Private)]
	static BackedArrays()
	{
		Log.Out(string.Format("Initial {0} == {1}", "ENABLE_FILE_BACKED_ARRAYS", BackedArrays.ENABLE_FILE_BACKED_ARRAYS));
	}

	// Token: 0x06009CCF RID: 40143 RVA: 0x003B468E File Offset: 0x003B288E
	public static IBackedArray<T> Create<[IsUnmanaged] T>(int length) where T : struct, ValueType
	{
		if (BackedArrays.ENABLE_FILE_BACKED_ARRAYS && length > 0)
		{
			return new FileBackedArray<T>(length);
		}
		return new MemoryBackedArray<T>(length);
	}

	// Token: 0x06009CD0 RID: 40144 RVA: 0x003B46A8 File Offset: 0x003B28A8
	public static IBackedArrayView<T> CreateSingleView<[IsUnmanaged] T>(IBackedArray<T> array, BackedArrayHandleMode mode, int viewLength = 0) where T : struct, ValueType
	{
		MemoryBackedArray<T> memoryBackedArray = array as MemoryBackedArray<T>;
		if (memoryBackedArray != null)
		{
			return new MemoryBackedArray<T>.MemoryBackedArrayView(memoryBackedArray, mode);
		}
		return new BackedArraySingleView<T>(array, mode, viewLength);
	}

	// Token: 0x040076F1 RID: 30449
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag ENABLE_FILE_BACKED_ARRAYS_PLATFORMS = DeviceFlag.XBoxSeriesS;

	// Token: 0x040076F2 RID: 30450
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool ENABLE_FILE_BACKED_ARRAYS = PlatformOptimizations.FileBackedArrays;
}
