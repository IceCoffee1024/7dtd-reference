using System;

// Token: 0x020009FF RID: 2559
public static class SaveDataLimit
{
	// Token: 0x06004CEB RID: 19691 RVA: 0x001D84B9 File Offset: 0x001D66B9
	public static void SetLimitToPref(long limit)
	{
		GamePrefs.Set(EnumGamePrefs.SaveDataLimit, SaveDataLimit.ToPrefValue(limit));
	}

	// Token: 0x06004CEC RID: 19692 RVA: 0x001D84CB File Offset: 0x001D66CB
	public static long GetLimitFromPref()
	{
		return SaveDataLimit.FromPrefValue(GamePrefs.GetInt(EnumGamePrefs.SaveDataLimit));
	}

	// Token: 0x06004CED RID: 19693 RVA: 0x001D84DC File Offset: 0x001D66DC
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

	// Token: 0x06004CEE RID: 19694 RVA: 0x001D8529 File Offset: 0x001D6729
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

	// Token: 0x04003B7F RID: 15231
	public const int SAVE_DATA_LIMIT_DISABLED = -1;

	// Token: 0x04003B80 RID: 15232
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SAVE_DATA_LIMIT_PREF_BYTES_PER = 1048576;
}
