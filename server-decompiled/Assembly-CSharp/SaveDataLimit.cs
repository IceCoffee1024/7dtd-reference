using System;

// Token: 0x02000A24 RID: 2596
public static class SaveDataLimit
{
	// Token: 0x06004D64 RID: 19812 RVA: 0x001DD2E9 File Offset: 0x001DB4E9
	public static void SetLimitToPref(long limit)
	{
		GamePrefs.Set(EnumGamePrefs.SaveDataLimit, SaveDataLimit.ToPrefValue(limit));
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x001DD2FB File Offset: 0x001DB4FB
	public static long GetLimitFromPref()
	{
		return SaveDataLimit.FromPrefValue(GamePrefs.GetInt(EnumGamePrefs.SaveDataLimit));
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x001DD30C File Offset: 0x001DB50C
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

	// Token: 0x06004D67 RID: 19815 RVA: 0x001DD359 File Offset: 0x001DB559
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

	// Token: 0x04003C3E RID: 15422
	public const int SAVE_DATA_LIMIT_DISABLED = -1;

	// Token: 0x04003C3F RID: 15423
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SAVE_DATA_LIMIT_PREF_BYTES_PER = 1048576;
}
