using System;

// Token: 0x02000A15 RID: 2581
public static class SdPlayerPrefs
{
	// Token: 0x06004D28 RID: 19752 RVA: 0x001DBC44 File Offset: 0x001D9E44
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

	// Token: 0x06004D29 RID: 19753 RVA: 0x001DBC88 File Offset: 0x001D9E88
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

	// Token: 0x06004D2A RID: 19754 RVA: 0x001DBCD4 File Offset: 0x001D9ED4
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

	// Token: 0x06004D2B RID: 19755 RVA: 0x001DBD20 File Offset: 0x001D9F20
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

	// Token: 0x06004D2C RID: 19756 RVA: 0x001DBD64 File Offset: 0x001D9F64
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

	// Token: 0x06004D2D RID: 19757 RVA: 0x001DBDB0 File Offset: 0x001D9FB0
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

	// Token: 0x06004D2E RID: 19758 RVA: 0x001DBDFC File Offset: 0x001D9FFC
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

	// Token: 0x06004D2F RID: 19759 RVA: 0x001DBE40 File Offset: 0x001DA040
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

	// Token: 0x06004D30 RID: 19760 RVA: 0x001DBEA0 File Offset: 0x001DA0A0
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

	// Token: 0x06004D31 RID: 19761 RVA: 0x001DBF00 File Offset: 0x001DA100
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

	// Token: 0x06004D32 RID: 19762 RVA: 0x001DBF44 File Offset: 0x001DA144
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

	// Token: 0x06004D33 RID: 19763 RVA: 0x001DBF88 File Offset: 0x001DA188
	public static void DeleteAll()
	{
		SaveDataUtils.SaveDataPrefs.DeleteAll();
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x001DBF94 File Offset: 0x001DA194
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

	// Token: 0x06004D35 RID: 19765 RVA: 0x001DBFAF File Offset: 0x001DA1AF
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void Load()
	{
		SaveDataUtils.SaveDataPrefs.Load();
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x06004D36 RID: 19766 RVA: 0x001DBFBB File Offset: 0x001DA1BB
	public static bool CanLoad
	{
		[PublicizedFrom(EAccessModifier.Internal)]
		get
		{
			return SaveDataUtils.SaveDataPrefs.CanLoad;
		}
	}
}
