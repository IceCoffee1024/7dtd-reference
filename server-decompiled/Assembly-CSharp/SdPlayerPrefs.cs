using System;

// Token: 0x02000A0B RID: 2571
public static class SdPlayerPrefs
{
	// Token: 0x06004D59 RID: 19801 RVA: 0x001DA994 File Offset: 0x001D8B94
	public static float GetFloat(string key)
	{
		float @float;
		try
		{
			@float = SaveDataUtils.SaveDataPrefs.GetFloat(key);
		}
		catch (Exception ex)
		{
			throw new Exception("GetFloat(\"" + key + "\")", ex);
		}
		return @float;
	}

	// Token: 0x06004D5A RID: 19802 RVA: 0x001DA9D8 File Offset: 0x001D8BD8
	public static float GetFloat(string key, float defaultValue)
	{
		float @float;
		try
		{
			@float = SaveDataUtils.SaveDataPrefs.GetFloat(key, defaultValue);
		}
		catch (Exception ex)
		{
			throw new Exception(string.Format("{0}(\"{1}\", {2})", "GetFloat", key, defaultValue), ex);
		}
		return @float;
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x001DAA24 File Offset: 0x001D8C24
	public static void SetFloat(string key, float value)
	{
		try
		{
			SaveDataUtils.SaveDataPrefs.SetFloat(key, value);
		}
		catch (Exception ex)
		{
			throw new Exception(string.Format("{0}(\"{1}\", {2})", "SetFloat", key, value), ex);
		}
	}

	// Token: 0x06004D5C RID: 19804 RVA: 0x001DAA70 File Offset: 0x001D8C70
	public static int GetInt(string key)
	{
		int @int;
		try
		{
			@int = SaveDataUtils.SaveDataPrefs.GetInt(key);
		}
		catch (Exception ex)
		{
			throw new Exception("GetInt(\"" + key + "\")", ex);
		}
		return @int;
	}

	// Token: 0x06004D5D RID: 19805 RVA: 0x001DAAB4 File Offset: 0x001D8CB4
	public static int GetInt(string key, int defaultValue)
	{
		int @int;
		try
		{
			@int = SaveDataUtils.SaveDataPrefs.GetInt(key, defaultValue);
		}
		catch (Exception ex)
		{
			throw new Exception(string.Format("{0}(\"{1}\", {2})", "GetInt", key, defaultValue), ex);
		}
		return @int;
	}

	// Token: 0x06004D5E RID: 19806 RVA: 0x001DAB00 File Offset: 0x001D8D00
	public static void SetInt(string key, int value)
	{
		try
		{
			SaveDataUtils.SaveDataPrefs.SetInt(key, value);
		}
		catch (Exception ex)
		{
			throw new Exception(string.Format("{0}(\"{1}\", {2})", "SetInt", key, value), ex);
		}
	}

	// Token: 0x06004D5F RID: 19807 RVA: 0x001DAB4C File Offset: 0x001D8D4C
	public static string GetString(string key)
	{
		string @string;
		try
		{
			@string = SaveDataUtils.SaveDataPrefs.GetString(key);
		}
		catch (Exception ex)
		{
			throw new Exception("GetString(\"" + key + "\")", ex);
		}
		return @string;
	}

	// Token: 0x06004D60 RID: 19808 RVA: 0x001DAB90 File Offset: 0x001D8D90
	public static string GetString(string key, string defaultValue)
	{
		string @string;
		try
		{
			@string = SaveDataUtils.SaveDataPrefs.GetString(key, defaultValue);
		}
		catch (Exception ex)
		{
			throw new Exception(string.Concat(new string[] { "GetString(\"", key, "\", \"", defaultValue, "\")" }), ex);
		}
		return @string;
	}

	// Token: 0x06004D61 RID: 19809 RVA: 0x001DABF0 File Offset: 0x001D8DF0
	public static void SetString(string key, string value)
	{
		try
		{
			SaveDataUtils.SaveDataPrefs.SetString(key, value);
		}
		catch (Exception ex)
		{
			throw new Exception(string.Concat(new string[] { "SetString(\"", key, "\", \"", value, "\")" }), ex);
		}
	}

	// Token: 0x06004D62 RID: 19810 RVA: 0x001DAC50 File Offset: 0x001D8E50
	public static bool HasKey(string key)
	{
		bool flag;
		try
		{
			flag = SaveDataUtils.SaveDataPrefs.HasKey(key);
		}
		catch (Exception ex)
		{
			throw new Exception("HasKey(\"" + key + "\")", ex);
		}
		return flag;
	}

	// Token: 0x06004D63 RID: 19811 RVA: 0x001DAC94 File Offset: 0x001D8E94
	public static void DeleteKey(string key)
	{
		try
		{
			SaveDataUtils.SaveDataPrefs.DeleteKey(key);
		}
		catch (Exception ex)
		{
			throw new Exception("DeleteKey(\"" + key + "\")", ex);
		}
	}

	// Token: 0x06004D64 RID: 19812 RVA: 0x001DACD8 File Offset: 0x001D8ED8
	public static void DeleteAll()
	{
		SaveDataUtils.SaveDataPrefs.DeleteAll();
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x001DACE4 File Offset: 0x001D8EE4
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

	// Token: 0x06004D66 RID: 19814 RVA: 0x001DACFF File Offset: 0x001D8EFF
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void Load()
	{
		SaveDataUtils.SaveDataPrefs.Load();
	}

	// Token: 0x1700083D RID: 2109
	// (get) Token: 0x06004D67 RID: 19815 RVA: 0x001DAD0B File Offset: 0x001D8F0B
	public static bool CanLoad
	{
		[PublicizedFrom(EAccessModifier.Internal)]
		get
		{
			return SaveDataUtils.SaveDataPrefs.CanLoad;
		}
	}
}
