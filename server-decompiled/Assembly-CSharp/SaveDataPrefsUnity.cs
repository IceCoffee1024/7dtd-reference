using System;
using UnityEngine;

// Token: 0x02000A55 RID: 2645
public sealed class SaveDataPrefsUnity : ISaveDataPrefs
{
	// Token: 0x1700089E RID: 2206
	// (get) Token: 0x06004F46 RID: 20294 RVA: 0x001E406D File Offset: 0x001E226D
	public static SaveDataPrefsUnity INSTANCE
	{
		get
		{
			SaveDataPrefsUnity saveDataPrefsUnity;
			if ((saveDataPrefsUnity = SaveDataPrefsUnity.s_instance) == null)
			{
				saveDataPrefsUnity = (SaveDataPrefsUnity.s_instance = new SaveDataPrefsUnity());
			}
			return saveDataPrefsUnity;
		}
	}

	// Token: 0x06004F47 RID: 20295 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataPrefsUnity()
	{
	}

	// Token: 0x06004F48 RID: 20296 RVA: 0x001E4083 File Offset: 0x001E2283
	public float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x06004F49 RID: 20297 RVA: 0x001E408C File Offset: 0x001E228C
	public void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x001E4095 File Offset: 0x001E2295
	public int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x06004F4B RID: 20299 RVA: 0x001E409E File Offset: 0x001E229E
	public void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	// Token: 0x06004F4C RID: 20300 RVA: 0x001E40A7 File Offset: 0x001E22A7
	public string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	// Token: 0x06004F4D RID: 20301 RVA: 0x001E40B0 File Offset: 0x001E22B0
	public void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x001E40B9 File Offset: 0x001E22B9
	public bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	// Token: 0x06004F4F RID: 20303 RVA: 0x001E40C1 File Offset: 0x001E22C1
	public void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	// Token: 0x06004F50 RID: 20304 RVA: 0x001E40C9 File Offset: 0x001E22C9
	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x06004F51 RID: 20305 RVA: 0x001E40D0 File Offset: 0x001E22D0
	public void Save()
	{
		PlayerPrefs.Save();
	}

	// Token: 0x06004F52 RID: 20306 RVA: 0x001E40D7 File Offset: 0x001E22D7
	public void Load()
	{
		throw new NotSupportedException("Unity PlayerPrefs does not support explicit loading.");
	}

	// Token: 0x1700089F RID: 2207
	// (get) Token: 0x06004F53 RID: 20307 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool CanLoad
	{
		get
		{
			return false;
		}
	}

	// Token: 0x04003D3B RID: 15675
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataPrefsUnity s_instance;
}
