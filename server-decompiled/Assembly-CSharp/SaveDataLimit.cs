using System;

// Token: 0x02000A10 RID: 2576
public static class SaveDataLimit
{
	// Token: 0x06004D71 RID: 19825 RVA: 0x001DAF51 File Offset: 0x001D9151
	public static void SetLimitToPref(long limit)
	{
		GamePrefs.Set(EnumGamePrefs.SaveDataLimit, SaveDataLimit.ToPrefValue(limit));
	}

	// Token: 0x06004D72 RID: 19826 RVA: 0x001DAF63 File Offset: 0x001D9163
	public static long GetLimitFromPref()
	{
		return SaveDataLimit.FromPrefValue(GamePrefs.GetInt(EnumGamePrefs.SaveDataLimit));
	}

	// Token: 0x06004D73 RID: 19827 RVA: 0x001DAF74 File Offset: 0x001D9174
	[PublicizedFrom(EAccessModifier.Private)]
	public static int ToPrefValue(long limit)
	{
		if (limit >= 0L)
		{
			return (int)((limit + 1048576L - 1L) / 1048576L);
		}
		if (PlatformOptimizations.LimitedSaveData)
		{
			Log.Warning(string.Format("[{0}] Expected finite save data limit for {1}, but was: {2}", "SaveDataLimit", "ToPrefValue", limit));
		}
		return -1;
	}

	// Token: 0x06004D74 RID: 19828 RVA: 0x001DAFC1 File Offset: 0x001D91C1
	[PublicizedFrom(EAccessModifier.Private)]
	public static long FromPrefValue(int limit)
	{
		if (limit >= 0)
		{
			return (long)(limit * 1048576);
		}
		if (PlatformOptimizations.LimitedSaveData)
		{
			Log.Warning(string.Format("[{0}] Expected finite save data limit for {1}, but was: {2}", "SaveDataLimit", "FromPrefValue", limit));
		}
		return -1L;
	}

	// Token: 0x04003C10 RID: 15376
	public const int SAVE_DATA_LIMIT_DISABLED = -1;

	// Token: 0x04003C11 RID: 15377
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SAVE_DATA_LIMIT_PREF_BYTES_PER = 1048576;
}
