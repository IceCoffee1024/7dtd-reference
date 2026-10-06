using System;

// Token: 0x02000A1C RID: 2588
public static class SaveDataLimitExtensions
{
	// Token: 0x06004D44 RID: 19780 RVA: 0x001DC2A8 File Offset: 0x001DA4A8
	public static bool IsSupported(this SaveDataLimitType saveDataLimitType)
	{
		return !PlatformOptimizations.LimitedSaveData || saveDataLimitType > SaveDataLimitType.Unlimited;
	}

	// Token: 0x06004D45 RID: 19781 RVA: 0x001DC2B8 File Offset: 0x001DA4B8
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

	// Token: 0x06004D46 RID: 19782 RVA: 0x001DC34C File Offset: 0x001DA54C
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

	// Token: 0x06004D47 RID: 19783 RVA: 0x001DC3C4 File Offset: 0x001DA5C4
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

	// Token: 0x06004D48 RID: 19784 RVA: 0x001DC420 File Offset: 0x001DA620
	public static bool TryCalculateSaveDataLimitType(long reservedBytes, Vector2i worldSize, out SaveDataLimitType saveDataLimitType)
	{
		long num = SaveDataLimitUtils.CalculatePlayerMapSize(worldSize);
		return SaveDataLimitExtensions.TryGetSaveDataLimitType(reservedBytes - 104857600L - num, out saveDataLimitType);
	}

	// Token: 0x04003C25 RID: 15397
	[PublicizedFrom(EAccessModifier.Private)]
	public const long MB = 1048576L;

	// Token: 0x04003C26 RID: 15398
	[PublicizedFrom(EAccessModifier.Private)]
	public const long FLAT_OVERHEAD = 104857600L;
}
