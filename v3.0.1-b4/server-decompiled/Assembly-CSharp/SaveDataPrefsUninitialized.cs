using System;

// Token: 0x02000A2F RID: 2607
public sealed class SaveDataPrefsUninitialized : ISaveDataPrefs
{
	// Token: 0x06004EBF RID: 20159 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataPrefsUninitialized()
	{
	}

	// Token: 0x06004EC0 RID: 20160 RVA: 0x001DF225 File Offset: 0x001DD425
	public float GetFloat(string key, float defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001DF225 File Offset: 0x001DD425
	public void SetFloat(string key, float value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC2 RID: 20162 RVA: 0x001DF225 File Offset: 0x001DD425
	public int GetInt(string key, int defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC3 RID: 20163 RVA: 0x001DF225 File Offset: 0x001DD425
	public void SetInt(string key, int value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC4 RID: 20164 RVA: 0x001DF225 File Offset: 0x001DD425
	public string GetString(string key, string defaultValue)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC5 RID: 20165 RVA: 0x001DF225 File Offset: 0x001DD425
	public void SetString(string key, string value)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC6 RID: 20166 RVA: 0x001DF225 File Offset: 0x001DD425
	public bool HasKey(string key)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001DF225 File Offset: 0x001DD425
	public void DeleteKey(string key)
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x001DF225 File Offset: 0x001DD425
	public void DeleteAll()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004EC9 RID: 20169 RVA: 0x001DF225 File Offset: 0x001DD425
	public void Save()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x06004ECA RID: 20170 RVA: 0x001DF225 File Offset: 0x001DD425
	public void Load()
	{
		throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
	}

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x06004ECB RID: 20171 RVA: 0x001DF225 File Offset: 0x001DD425
	public bool CanLoad
	{
		get
		{
			throw new NotSupportedException("SdPlayerPrefs is being accessed while the pref instance is not initialized.");
		}
	}

	// Token: 0x04003C7A RID: 15482
	public static readonly SaveDataPrefsUninitialized INSTANCE = new SaveDataPrefsUninitialized();

	// Token: 0x04003C7B RID: 15483
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ERROR_MESSAGE = "SdPlayerPrefs is being accessed while the pref instance is not initialized.";
}
