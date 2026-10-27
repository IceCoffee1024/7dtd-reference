using System;

// Token: 0x02000A54 RID: 2644
public sealed class SaveDataPrefsUninitialized : ISaveDataPrefs
{
	// Token: 0x06004F38 RID: 20280 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataPrefsUninitialized()
	{
	}

	// Token: 0x06004F39 RID: 20281 RVA: 0x001E4055 File Offset: 0x001E2255
	public float GetFloat(string key, float defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F3A RID: 20282 RVA: 0x001E4055 File Offset: 0x001E2255
	public void SetFloat(string key, float value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F3B RID: 20283 RVA: 0x001E4055 File Offset: 0x001E2255
	public int GetInt(string key, int defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F3C RID: 20284 RVA: 0x001E4055 File Offset: 0x001E2255
	public void SetInt(string key, int value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F3D RID: 20285 RVA: 0x001E4055 File Offset: 0x001E2255
	public string GetString(string key, string defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F3E RID: 20286 RVA: 0x001E4055 File Offset: 0x001E2255
	public void SetString(string key, string value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F3F RID: 20287 RVA: 0x001E4055 File Offset: 0x001E2255
	public bool HasKey(string key)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F40 RID: 20288 RVA: 0x001E4055 File Offset: 0x001E2255
	public void DeleteKey(string key)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F41 RID: 20289 RVA: 0x001E4055 File Offset: 0x001E2255
	public void DeleteAll()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F42 RID: 20290 RVA: 0x001E4055 File Offset: 0x001E2255
	public void Save()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004F43 RID: 20291 RVA: 0x001E4055 File Offset: 0x001E2255
	public void Load()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x1700089D RID: 2205
	// (get) Token: 0x06004F44 RID: 20292 RVA: 0x001E4055 File Offset: 0x001E2255
	public bool CanLoad
	{
		get
		{
			throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
		}
	}

	// Token: 0x04003D39 RID: 15673
	public static readonly SaveDataPrefsUninitialized INSTANCE = new SaveDataPrefsUninitialized();

	// Token: 0x04003D3A RID: 15674
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ERROR_MESSAGE = "SdPlayerPrefs is being accessed while the pref instance is not initialized.";
}
