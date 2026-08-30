using System;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x0200138B RID: 5003
public static class BackedArrays
{
	// Token: 0x06009DC9 RID: 40393 RVA: 0x003B9918 File Offset: 0x003B7B18
	[PublicizedFrom(EAccessModifier.Private)]
	static BackedArrays()
	{
		Log.Out(string.Format("Initial {0} == {1}", "ENABLE_FILE_BACKED_ARRAYS", BackedArrays.ENABLE_FILE_BACKED_ARRAYS));
	}

	// Token: 0x06009DCA RID: 40394 RVA: 0x003B9942 File Offset: 0x003B7B42
	public static IBackedArray<T> Create<[IsUnmanaged] T>(int length) where T : struct, ValueType
	{
		if (BackedArrays.ENABLE_FILE_BACKED_ARRAYS && length > 0)
		{
			return new FileBackedArray<T>(length);
		}
		return new MemoryBackedArray<T>(length);
	}

	// Token: 0x06009DCB RID: 40395 RVA: 0x003B995C File Offset: 0x003B7B5C
	public static IBackedArrayView<T> CreateSingleView<[IsUnmanaged] T>(IBackedArray<T> array, BackedArrayHandleMode mode, int viewLength = 0) where T : struct, ValueType
	{
		MemoryBackedArray<T> memoryBackedArray = array as MemoryBackedArray<T>;
		if (memoryBackedArray != null)
		{
			return new MemoryBackedArray<T>.MemoryBackedArrayView(memoryBackedArray, mode);
		}
		return new BackedArraySingleView<T>(array, mode, viewLength);
	}

	// Token: 0x040077C9 RID: 30665
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag ENABLE_FILE_BACKED_ARRAYS_PLATFORMS = DeviceFlag.XBoxSeriesS;

	// Token: 0x040077CA RID: 30666
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool ENABLE_FILE_BACKED_ARRAYS = PlatformOptimizations.FileBackedArrays;
}
