using System;
using Platform;
using UnityEngine;

// Token: 0x02000060 RID: 96
public static class PlatformOptimizations
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060001D7 RID: 471 RVA: 0x00010CBB File Offset: 0x0000EEBB
	public static int MaxWorldSizeHost
	{
		get
		{
			return LaunchPrefs.MaxWorldSizeHost.Value;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060001D8 RID: 472 RVA: 0x00010CC7 File Offset: 0x0000EEC7
	public static bool EnforceMaxWorldSizeHost
	{
		get
		{
			return PlatformOptimizations.MaxWorldSizeHost >= 0;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060001D9 RID: 473 RVA: 0x00010CD4 File Offset: 0x0000EED4
	public static int MaxWorldSizeClient
	{
		get
		{
			return LaunchPrefs.MaxWorldSizeClient.Value;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060001DA RID: 474 RVA: 0x00010CE0 File Offset: 0x0000EEE0
	public static bool EnforceMaxWorldSizeClient
	{
		get
		{
			return PlatformOptimizations.MaxWorldSizeClient >= 0;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060001DB RID: 475 RVA: 0x00010CED File Offset: 0x0000EEED
	public static XUiC_WorldGenerationWindow.PreviewQuality MaxRWGPreviewQuality
	{
		get
		{
			if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() && Submission.Enabled)
			{
				return XUiC_WorldGenerationWindow.PreviewQuality.High;
			}
			if (SystemInfo.systemMemorySize < 22000)
			{
				return XUiC_WorldGenerationWindow.PreviewQuality.High;
			}
			return EnumUtils.MaxValue<XUiC_WorldGenerationWindow.PreviewQuality>();
		}
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00010D14 File Offset: 0x0000EF14
	public static void ConfigureGameObjectPoolForPlatform(GameObjectPool pool)
	{
		if (!(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
		{
			return;
		}
		pool.MaxPooledInstancesPerItem = 500;
		pool.MaxDestroysPerUpdate = int.MaxValue;
		pool.ShrinkThresholdHigh = new GameObjectPool.ShrinkThreshold(100, 10, 0f);
		pool.ShrinkThresholdMedium = new GameObjectPool.ShrinkThreshold(50, 2, 0.1f);
	}

	// Token: 0x060001DD RID: 477 RVA: 0x000027FC File Offset: 0x000009FC
	public static void Init()
	{
	}

	// Token: 0x0400028E RID: 654
	[PublicizedFrom(EAccessModifier.Private)]
	public const ulong kB = 1024UL;

	// Token: 0x0400028F RID: 655
	[PublicizedFrom(EAccessModifier.Private)]
	public const ulong MB = 1048576UL;

	// Token: 0x04000290 RID: 656
	[PublicizedFrom(EAccessModifier.Private)]
	public const ulong GB = 1073741824UL;

	// Token: 0x04000291 RID: 657
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag ENABLE_FILE_BACKED_ARRAYS_PLATFORMS = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

	// Token: 0x04000292 RID: 658
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag ENABLE_FILE_BACKED_BLOCK_PROPERTIES_PLATFORMS = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

	// Token: 0x04000293 RID: 659
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag ENABLE_FILE_BACKED_TERRAIN_TILES_PLATFORMS = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

	// Token: 0x04000294 RID: 660
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag ENABLE_FILE_BACKED_RADIATION_TILES_PLATFORMS = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

	// Token: 0x04000295 RID: 661
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag LOAD_HALF_RES_ASSETS = DeviceFlag.XBoxSeriesS;

	// Token: 0x04000296 RID: 662
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag RESTART_PROCESS_SUPPORTED = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

	// Token: 0x04000297 RID: 663
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag RESTART_AFTER_RWG = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

	// Token: 0x04000298 RID: 664
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag LIMITED_SAVE_DATA = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

	// Token: 0x04000299 RID: 665
	[PublicizedFrom(EAccessModifier.Private)]
	public const DeviceFlag GC_FORCE_UNMAP_ON_GCOLLECT = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

	// Token: 0x0400029A RID: 666
	public static readonly bool FileBackedArrays = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();

	// Token: 0x0400029B RID: 667
	public static readonly bool FileBackedBlockProperties = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();

	// Token: 0x0400029C RID: 668
	public static readonly bool FileBackedTerrainTiles = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();

	// Token: 0x0400029D RID: 669
	public static readonly bool FileBackedRadiationTiles = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();

	// Token: 0x0400029E RID: 670
	public static readonly bool LoadHalfResAssets = DeviceFlag.XBoxSeriesS.IsCurrent();

	// Token: 0x0400029F RID: 671
	public static readonly bool RestartProcessSupported = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();

	// Token: 0x040002A0 RID: 672
	public static readonly bool RestartAfterRwg = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();

	// Token: 0x040002A1 RID: 673
	public static readonly bool LimitedSaveData = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();

	// Token: 0x040002A2 RID: 674
	public static readonly bool GCForceUnmapOnGCollect = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();

	// Token: 0x040002A3 RID: 675
	public static readonly int DefaultMaxWorldSizeHost = ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() ? 8192 : (-1));

	// Token: 0x040002A4 RID: 676
	public const int DefaultMaxWorldSizeClient = -1;

	// Token: 0x040002A5 RID: 677
	public static readonly int DefaultGameCoreBlobOperationQueueMaxTotalSize = 268435456;
}
