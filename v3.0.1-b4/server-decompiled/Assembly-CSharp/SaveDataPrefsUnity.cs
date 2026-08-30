using System;
using UnityEngine;

// Token: 0x02000A30 RID: 2608
public sealed class SaveDataPrefsUnity : ISaveDataPrefs
{
	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x06004ECD RID: 20173 RVA: 0x001DF23D File Offset: 0x001DD43D
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

	// Token: 0x06004ECE RID: 20174 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataPrefsUnity()
	{
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x001DF253 File Offset: 0x001DD453
	public float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001DF25C File Offset: 0x001DD45C
	public void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	// Token: 0x06004ED1 RID: 20177 RVA: 0x001DF265 File Offset: 0x001DD465
	public int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x001DF26E File Offset: 0x001DD46E
	public void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	// Token: 0x06004ED3 RID: 20179 RVA: 0x001DF277 File Offset: 0x001DD477
	public string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001DF280 File Offset: 0x001DD480
	public void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x001DF289 File Offset: 0x001DD489
	public bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x001DF291 File Offset: 0x001DD491
	public void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	// Token: 0x06004ED7 RID: 20183 RVA: 0x001DF299 File Offset: 0x001DD499
	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x001DF2A0 File Offset: 0x001DD4A0
	public void Save()
	{
		PlayerPrefs.Save();
	}

	// Token: 0x06004ED9 RID: 20185 RVA: 0x001DF2A7 File Offset: 0x001DD4A7
	public void Load()
	{
		throw new NotSupportedException("Unity PlayerPrefs does not support explicit loading.");
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x06004EDA RID: 20186 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool CanLoad
	{
		get
		{
			return false;
		}
	}

	// Token: 0x04003C7C RID: 15484
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataPrefsUnity s_instance;
}
