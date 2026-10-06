using System;

// Token: 0x02000A1A RID: 2586
public static class SaveDataLimit
{
	// Token: 0x06004D40 RID: 19776 RVA: 0x001DC201 File Offset: 0x001DA401
	public static void SetLimitToPref(long limit)
	{
		GamePrefs.Set(EnumGamePrefs.SaveDataLimit, SaveDataLimit.ToPrefValue(limit));
	}

	// Token: 0x06004D41 RID: 19777 RVA: 0x001DC213 File Offset: 0x001DA413
	public static long GetLimitFromPref()
	{
		return SaveDataLimit.FromPrefValue(GamePrefs.GetInt(EnumGamePrefs.SaveDataLimit));
	}

	// Token: 0x06004D42 RID: 19778 RVA: 0x001DC224 File Offset: 0x001DA424
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

	// Token: 0x06004D43 RID: 19779 RVA: 0x001DC271 File Offset: 0x001DA471
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

	// Token: 0x04003C1D RID: 15389
	public const int SAVE_DATA_LIMIT_DISABLED = -1;

	// Token: 0x04003C1E RID: 15390
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SAVE_DATA_LIMIT_PREF_BYTES_PER = 1048576;
}
