using System;
using UnityEngine;

// Token: 0x02000A41 RID: 2625
public sealed class SaveDataPrefsUnity : ISaveDataPrefs
{
	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x06004F53 RID: 20307 RVA: 0x001E1CD5 File Offset: 0x001DFED5
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

	// Token: 0x06004F54 RID: 20308 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataPrefsUnity()
	{
	}

	// Token: 0x06004F55 RID: 20309 RVA: 0x001E1CEB File Offset: 0x001DFEEB
	public float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x06004F56 RID: 20310 RVA: 0x001E1CF4 File Offset: 0x001DFEF4
	public void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	// Token: 0x06004F57 RID: 20311 RVA: 0x001E1CFD File Offset: 0x001DFEFD
	public int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x06004F58 RID: 20312 RVA: 0x001E1D06 File Offset: 0x001DFF06
	public void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	// Token: 0x06004F59 RID: 20313 RVA: 0x001E1D0F File Offset: 0x001DFF0F
	public string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	// Token: 0x06004F5A RID: 20314 RVA: 0x001E1D18 File Offset: 0x001DFF18
	public void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	// Token: 0x06004F5B RID: 20315 RVA: 0x001E1D21 File Offset: 0x001DFF21
	public bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	// Token: 0x06004F5C RID: 20316 RVA: 0x001E1D29 File Offset: 0x001DFF29
	public void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	// Token: 0x06004F5D RID: 20317 RVA: 0x001E1D31 File Offset: 0x001DFF31
	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x06004F5E RID: 20318 RVA: 0x001E1D38 File Offset: 0x001DFF38
	public void Save()
	{
		PlayerPrefs.Save();
	}

	// Token: 0x06004F5F RID: 20319 RVA: 0x001E1D3F File Offset: 0x001DFF3F
	public void Load()
	{
		throw new NotSupportedException("Unity PlayerPrefs does not support explicit loading.");
	}

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x06004F60 RID: 20320 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool CanLoad
	{
		get
		{
			return false;
		}
	}

	// Token: 0x04003D0D RID: 15629
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataPrefsUnity s_instance;
}
