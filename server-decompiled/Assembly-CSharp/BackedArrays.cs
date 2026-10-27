using System;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x020013BD RID: 5053
public static class BackedArrays
{
	// Token: 0x06009ECC RID: 40652 RVA: 0x003C0898 File Offset: 0x003BEA98
	[PublicizedFrom(EAccessModifier.Private)]
	static BackedArrays()
	{
		Log.Out(string.Format("Initial {0} == {1}", "ENABLE_FILE_BACKED_ARRAYS", BackedArrays.ENABLE_FILE_BACKED_ARRAYS));
	}

	// Token: 0x06009ECD RID: 40653 RVA: 0x003C08C2 File Offset: 0x003BEAC2
	public static IBackedArray<T> Create<[IsUnmanaged] T>(int length) where T : struct, ValueType
	{
		if (BackedArrays.ENABLE_FILE_BACKED_ARRAYS && length > 0)
		{
			return new FileBackedArray<T>(length);
		}
		return new MemoryBackedArray<T>(length);
	}

	// Token: 0x06009ECE RID: 40654 RVA: 0x003C08DC File Offset: 0x003BEADC
	public static IBackedArrayView<T> CreateSingleView<[IsUnmanaged] T>(IBackedArray<T> array, BackedArrayHandleMode mode, int viewLength = 0) where T : struct, ValueType
	{
		MemoryBackedArray<T> memoryBackedArray = array as MemoryBackedArray<T>;
		if (memoryBackedArray != null)
		{
			return new MemoryBackedArray<T>.MemoryBackedArrayView(memoryBackedArray, mode);
		}
		return new BackedArraySingleView<T>(array, mode, viewLength);
	}

	// Token: 0x04007885 RID: 30853
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag ENABLE_FILE_BACKED_ARRAYS_PLATFORMS = DeviceFlag.XBoxSeriesS;

	// Token: 0x04007886 RID: 30854
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool ENABLE_FILE_BACKED_ARRAYS = PlatformOptimizations.FileBackedArrays;
}
