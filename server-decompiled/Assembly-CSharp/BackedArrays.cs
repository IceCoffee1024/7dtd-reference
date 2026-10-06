using System;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x020013AB RID: 5035
public static class BackedArrays
{
	// Token: 0x06009E74 RID: 40564 RVA: 0x003BE494 File Offset: 0x003BC694
	[PublicizedFrom(EAccessModifier.Private)]
	static BackedArrays()
	{
		Log.Out(string.Format("Initial {0} == {1}", "ENABLE_FILE_BACKED_ARRAYS", BackedArrays.ENABLE_FILE_BACKED_ARRAYS));
	}

	// Token: 0x06009E75 RID: 40565 RVA: 0x003BE4BE File Offset: 0x003BC6BE
	public static IBackedArray<T> Create<[IsUnmanaged] T>(int length) where T : struct, ValueType
	{
		if (BackedArrays.ENABLE_FILE_BACKED_ARRAYS && length > 0)
		{
			return new FileBackedArray<T>(length);
		}
		return new MemoryBackedArray<T>(length);
	}

	// Token: 0x06009E76 RID: 40566 RVA: 0x003BE4D8 File Offset: 0x003BC6D8
	public static IBackedArrayView<T> CreateSingleView<[IsUnmanaged] T>(IBackedArray<T> array, BackedArrayHandleMode mode, int viewLength = 0) where T : struct, ValueType
	{
		MemoryBackedArray<T> memoryBackedArray = array as MemoryBackedArray<T>;
		if (memoryBackedArray != null)
		{
			return new MemoryBackedArray<T>.MemoryBackedArrayView(memoryBackedArray, mode);
		}
		return new BackedArraySingleView<T>(array, mode, viewLength);
	}

	// Token: 0x04007844 RID: 30788
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag ENABLE_FILE_BACKED_ARRAYS_PLATFORMS = DeviceFlag.XBoxSeriesS;

	// Token: 0x04007845 RID: 30789
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool ENABLE_FILE_BACKED_ARRAYS = PlatformOptimizations.FileBackedArrays;
}
