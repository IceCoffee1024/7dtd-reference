using System;

// Token: 0x020009FA RID: 2554
public static class SdPlayerPrefs
{
	// Token: 0x06004CD3 RID: 19667 RVA: 0x001D81AA File Offset: 0x001D63AA
	public static float GetFloat(string key)
	{
		return SaveDataUtils.SaveDataPrefs.GetFloat(key);
	}

	// Token: 0x06004CD4 RID: 19668 RVA: 0x001D81B7 File Offset: 0x001D63B7
	public static float GetFloat(string key, float defaultValue)
	{
		return SaveDataUtils.SaveDataPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x06004CD5 RID: 19669 RVA: 0x001D81C5 File Offset: 0x001D63C5
	public static void SetFloat(string key, float value)
	{
		SaveDataUtils.SaveDataPrefs.SetFloat(key, value);
	}

	// Token: 0x06004CD6 RID: 19670 RVA: 0x001D81D3 File Offset: 0x001D63D3
	public static int GetInt(string key)
	{
		return SaveDataUtils.SaveDataPrefs.GetInt(key);
	}

	// Token: 0x06004CD7 RID: 19671 RVA: 0x001D81E0 File Offset: 0x001D63E0
	public static int GetInt(string key, int defaultValue)
	{
		return SaveDataUtils.SaveDataPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x06004CD8 RID: 19672 RVA: 0x001D81EE File Offset: 0x001D63EE
	public static void SetInt(string key, int value)
	{
		SaveDataUtils.SaveDataPrefs.SetInt(key, value);
	}

	// Token: 0x06004CD9 RID: 19673 RVA: 0x001D81FC File Offset: 0x001D63FC
	public static string GetString(string key)
	{
		return SaveDataUtils.SaveDataPrefs.GetString(key);
	}

	// Token: 0x06004CDA RID: 19674 RVA: 0x001D8209 File Offset: 0x001D6409
	public static string GetString(string key, string defaultValue)
	{
		return SaveDataUtils.SaveDataPrefs.GetString(key, defaultValue);
	}

	// Token: 0x06004CDB RID: 19675 RVA: 0x001D8217 File Offset: 0x001D6417
	public static void SetString(string key, string value)
	{
		SaveDataUtils.SaveDataPrefs.SetString(key, value);
	}

	// Token: 0x06004CDC RID: 19676 RVA: 0x001D8225 File Offset: 0x001D6425
	public static bool HasKey(string key)
	{
		return SaveDataUtils.SaveDataPrefs.HasKey(key);
	}

	// Token: 0x06004CDD RID: 19677 RVA: 0x001D8232 File Offset: 0x001D6432
	public static void DeleteKey(string key)
	{
		SaveDataUtils.SaveDataPrefs.DeleteKey(key);
	}

	// Token: 0x06004CDE RID: 19678 RVA: 0x001D823F File Offset: 0x001D643F
	public static void DeleteAll()
	{
		SaveDataUtils.SaveDataPrefs.DeleteAll();
	}

	// Token: 0x06004CDF RID: 19679 RVA: 0x001D824B File Offset: 0x001D644B
	public static void Save()
	{
		SaveDataUtils.SaveDataPrefs.Save();
		PrefVersionStore store = RoamingPrefs.Store;
		if (store == null)
		{
			return;
		}
		store.Save();
	}

	// Token: 0x06004CE0 RID: 19680 RVA: 0x001D8266 File Offset: 0x001D6466
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void Load()
	{
		SaveDataUtils.SaveDataPrefs.Load();
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x06004CE1 RID: 19681 RVA: 0x001D8272 File Offset: 0x001D6472
	public static bool CanLoad
	{
		[PublicizedFrom(EAccessModifier.Internal)]
		get
		{
			return SaveDataUtils.SaveDataPrefs.CanLoad;
		}
	}
}
