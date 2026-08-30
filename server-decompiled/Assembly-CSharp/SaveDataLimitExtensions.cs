using System;

// Token: 0x02000A01 RID: 2561
public static class SaveDataLimitExtensions
{
	// Token: 0x06004CEF RID: 19695 RVA: 0x001D8560 File Offset: 0x001D6760
	public static bool IsSupported(this SaveDataLimitType saveDataLimitType)
	{
		return !PlatformOptimizations.LimitedSaveData || saveDataLimitType > SaveDataLimitType.Unlimited;
	}

	// Token: 0x06004CF0 RID: 19696 RVA: 0x001D8570 File Offset: 0x001D6770
	[PublicizedFrom(EAccessModifier.Private)]
	public static long GetRegionSizeLimit(this SaveDataLimitType saveDataLimitType)
	{
		if (!saveDataLimitType.IsSupported())
		{
			throw new ArgumentException(string.Format("Unexpected usage of {0}.{1}.{2}() when not supported by the current device.", "SaveDataLimitType", saveDataLimitType, "GetRegionSizeLimit"), "saveDataLimitType");
		}
		long num;
		switch (saveDataLimitType)
		{
		case SaveDataLimitType.Unlimited:
			num = -1L;
			break;
		case SaveDataLimitType.Short:
			num = 33554432L;
			break;
		case SaveDataLimitType.Medium:
			num = 67108864L;
			break;
		case SaveDataLimitType.Long:
			num = 134217728L;
			break;
		case SaveDataLimitType.VeryLong:
			num = 268435456L;
			break;
		default:
			throw new ArgumentOutOfRangeException("saveDataLimitType", saveDataLimitType, null);
		}
		return num;
	}

	// Token: 0x06004CF1 RID: 19697 RVA: 0x001D8604 File Offset: 0x001D6804
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryGetSaveDataLimitType(long regionLimit, out SaveDataLimitType saveDataLimitType)
	{
		if (regionLimit == -1L)
		{
			saveDataLimitType = SaveDataLimitType.Unlimited;
			return true;
		}
		foreach (SaveDataLimitType saveDataLimitType2 in EnumUtils.Values<SaveDataLimitType>())
		{
			if (saveDataLimitType2 != SaveDataLimitType.Unlimited && regionLimit / 1048576L == saveDataLimitType2.GetRegionSizeLimit() / 1048576L)
			{
				saveDataLimitType = saveDataLimitType2;
				return true;
			}
		}
		saveDataLimitType = SaveDataLimitType.Unlimited;
		return false;
	}

	// Token: 0x06004CF2 RID: 19698 RVA: 0x001D867C File Offset: 0x001D687C
	public static long CalculateTotalSize(this SaveDataLimitType saveDataLimitType, Vector2i worldSize)
	{
		if (!saveDataLimitType.IsSupported())
		{
			throw new ArgumentException(string.Format("Unexpected usage of {0}.{1}.{2}() when not supported by the current device.", "SaveDataLimitType", saveDataLimitType, "CalculateTotalSize"), "saveDataLimitType");
		}
		long regionSizeLimit = saveDataLimitType.GetRegionSizeLimit();
		if (regionSizeLimit <= 0L)
		{
			return -1L;
		}
		long num = SaveDataLimitUtils.CalculatePlayerMapSize(worldSize);
		return 104857600L + num + regionSizeLimit;
	}

	// Token: 0x06004CF3 RID: 19699 RVA: 0x001D86D8 File Offset: 0x001D68D8
	public static bool TryCalculateSaveDataLimitType(long reservedBytes, Vector2i worldSize, out SaveDataLimitType saveDataLimitType)
	{
		long num = SaveDataLimitUtils.CalculatePlayerMapSize(worldSize);
		return SaveDataLimitExtensions.TryGetSaveDataLimitType(reservedBytes - 104857600L - num, out saveDataLimitType);
	}

	// Token: 0x04003B87 RID: 15239
	[PublicizedFrom(EAccessModifier.Private)]
	public const long MB = 1048576L;

	// Token: 0x04003B88 RID: 15240
	[PublicizedFrom(EAccessModifier.Private)]
	public const long FLAT_OVERHEAD = 104857600L;
}
