using System;

// Token: 0x02000A40 RID: 2624
public sealed class SaveDataPrefsUninitialized : ISaveDataPrefs
{
	// Token: 0x06004F45 RID: 20293 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataPrefsUninitialized()
	{
	}

	// Token: 0x06004F46 RID: 20294 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public float GetFloat(string key, float defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F47 RID: 20295 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public void SetFloat(string key, float value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F48 RID: 20296 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public int GetInt(string key, int defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F49 RID: 20297 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public void SetInt(string key, int value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public string GetString(string key, string defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F4B RID: 20299 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public void SetString(string key, string value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F4C RID: 20300 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public bool HasKey(string key)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F4D RID: 20301 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public void DeleteKey(string key)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public void DeleteAll()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F4F RID: 20303 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public void Save()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F50 RID: 20304 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public void Load()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x06004F51 RID: 20305 RVA: 0x001E1CBD File Offset: 0x001DFEBD
	public bool CanLoad
	{
		get
		{
			throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
		}
	}

	// Token: 0x04003D0B RID: 15627
	public static readonly SaveDataPrefsUninitialized INSTANCE = new SaveDataPrefsUninitialized();

	// Token: 0x04003D0C RID: 15628
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ERROR_MESSAGE = "SdPlayerPrefs is being accessed while the pref instance is not initialized.";
}
