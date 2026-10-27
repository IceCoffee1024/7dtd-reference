using System;

// Token: 0x02000A1F RID: 2591
public static class SdPlayerPrefs
{
	// Token: 0x06004D4C RID: 19788 RVA: 0x001DCD2C File Offset: 0x001DAF2C
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

	// Token: 0x06004D4D RID: 19789 RVA: 0x001DCD70 File Offset: 0x001DAF70
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

	// Token: 0x06004D4E RID: 19790 RVA: 0x001DCDBC File Offset: 0x001DAFBC
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

	// Token: 0x06004D4F RID: 19791 RVA: 0x001DCE08 File Offset: 0x001DB008
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

	// Token: 0x06004D50 RID: 19792 RVA: 0x001DCE4C File Offset: 0x001DB04C
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

	// Token: 0x06004D51 RID: 19793 RVA: 0x001DCE98 File Offset: 0x001DB098
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

	// Token: 0x06004D52 RID: 19794 RVA: 0x001DCEE4 File Offset: 0x001DB0E4
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

	// Token: 0x06004D53 RID: 19795 RVA: 0x001DCF28 File Offset: 0x001DB128
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

	// Token: 0x06004D54 RID: 19796 RVA: 0x001DCF88 File Offset: 0x001DB188
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

	// Token: 0x06004D55 RID: 19797 RVA: 0x001DCFE8 File Offset: 0x001DB1E8
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

	// Token: 0x06004D56 RID: 19798 RVA: 0x001DD02C File Offset: 0x001DB22C
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

	// Token: 0x06004D57 RID: 19799 RVA: 0x001DD070 File Offset: 0x001DB270
	public static void DeleteAll()
	{
		SaveDataUtils.SaveDataPrefs.DeleteAll();
	}

	// Token: 0x06004D58 RID: 19800 RVA: 0x001DD07C File Offset: 0x001DB27C
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

	// Token: 0x06004D59 RID: 19801 RVA: 0x001DD097 File Offset: 0x001DB297
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void Load()
	{
		SaveDataUtils.SaveDataPrefs.Load();
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06004D5A RID: 19802 RVA: 0x001DD0A3 File Offset: 0x001DB2A3
	public static bool CanLoad
	{
		[PublicizedFrom(EAccessModifier.Internal)]
		get
		{
			return SaveDataUtils.SaveDataPrefs.CanLoad;
		}
	}
}
