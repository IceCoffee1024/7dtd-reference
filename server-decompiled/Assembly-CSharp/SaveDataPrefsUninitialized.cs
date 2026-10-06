using System;

// Token: 0x02000A4A RID: 2634
public sealed class SaveDataPrefsUninitialized : ISaveDataPrefs
{
	// Token: 0x06004F14 RID: 20244 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataPrefsUninitialized()
	{
	}

	// Token: 0x06004F15 RID: 20245 RVA: 0x001E2F6D File Offset: 0x001E116D
	public float GetFloat(string key, float defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F16 RID: 20246 RVA: 0x001E2F6D File Offset: 0x001E116D
	public void SetFloat(string key, float value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F17 RID: 20247 RVA: 0x001E2F6D File Offset: 0x001E116D
	public int GetInt(string key, int defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F18 RID: 20248 RVA: 0x001E2F6D File Offset: 0x001E116D
	public void SetInt(string key, int value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F19 RID: 20249 RVA: 0x001E2F6D File Offset: 0x001E116D
	public string GetString(string key, string defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F1A RID: 20250 RVA: 0x001E2F6D File Offset: 0x001E116D
	public void SetString(string key, string value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F1B RID: 20251 RVA: 0x001E2F6D File Offset: 0x001E116D
	public bool HasKey(string key)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F1C RID: 20252 RVA: 0x001E2F6D File Offset: 0x001E116D
	public void DeleteKey(string key)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F1D RID: 20253 RVA: 0x001E2F6D File Offset: 0x001E116D
	public void DeleteAll()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F1E RID: 20254 RVA: 0x001E2F6D File Offset: 0x001E116D
	public void Save()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F1F RID: 20255 RVA: 0x001E2F6D File Offset: 0x001E116D
	public void Load()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x17000897 RID: 2199
	// (get) Token: 0x06004F20 RID: 20256 RVA: 0x001E2F6D File Offset: 0x001E116D
	public bool CanLoad
	{
		get
		{
			throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
		}
	}

	// Token: 0x04003D18 RID: 15640
	public static readonly SaveDataPrefsUninitialized INSTANCE = new SaveDataPrefsUninitialized();

	// Token: 0x04003D19 RID: 15641
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ERROR_MESSAGE = "SdPlayerPrefs is being accessed while the pref instance is not initialized.";
}
