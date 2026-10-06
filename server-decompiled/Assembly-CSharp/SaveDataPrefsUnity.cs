using System;
using UnityEngine;

// Token: 0x02000A4B RID: 2635
public sealed class SaveDataPrefsUnity : ISaveDataPrefs
{
	// Token: 0x17000898 RID: 2200
	// (get) Token: 0x06004F22 RID: 20258 RVA: 0x001E2F85 File Offset: 0x001E1185
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

	// Token: 0x06004F23 RID: 20259 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataPrefsUnity()
	{
	}

	// Token: 0x06004F24 RID: 20260 RVA: 0x001E2F9B File Offset: 0x001E119B
	public float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x06004F25 RID: 20261 RVA: 0x001E2FA4 File Offset: 0x001E11A4
	public void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	// Token: 0x06004F26 RID: 20262 RVA: 0x001E2FAD File Offset: 0x001E11AD
	public int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x06004F27 RID: 20263 RVA: 0x001E2FB6 File Offset: 0x001E11B6
	public void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	// Token: 0x06004F28 RID: 20264 RVA: 0x001E2FBF File Offset: 0x001E11BF
	public string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	// Token: 0x06004F29 RID: 20265 RVA: 0x001E2FC8 File Offset: 0x001E11C8
	public void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	// Token: 0x06004F2A RID: 20266 RVA: 0x001E2FD1 File Offset: 0x001E11D1
	public bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	// Token: 0x06004F2B RID: 20267 RVA: 0x001E2FD9 File Offset: 0x001E11D9
	public void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	// Token: 0x06004F2C RID: 20268 RVA: 0x001E2FE1 File Offset: 0x001E11E1
	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x06004F2D RID: 20269 RVA: 0x001E2FE8 File Offset: 0x001E11E8
	public void Save()
	{
		PlayerPrefs.Save();
	}

	// Token: 0x06004F2E RID: 20270 RVA: 0x001E2FEF File Offset: 0x001E11EF
	public void Load()
	{
		throw new NotSupportedException("Unity PlayerPrefs does not support explicit loading.");
	}

	// Token: 0x17000899 RID: 2201
	// (get) Token: 0x06004F2F RID: 20271 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool CanLoad
	{
		get
		{
			return false;
		}
	}

	// Token: 0x04003D1A RID: 15642
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataPrefsUnity s_instance;
}
