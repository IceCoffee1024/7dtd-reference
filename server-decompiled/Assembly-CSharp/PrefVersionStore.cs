using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A0A RID: 2570
public class PrefVersionStore
{
	// Token: 0x06004C54 RID: 19540 RVA: 0x001D9C55 File Offset: 0x001D7E55
	public PrefVersionStore(string saveDir)
	{
		this.saveDir = saveDir;
	}

	// Token: 0x06004C55 RID: 19541 RVA: 0x001D9C88 File Offset: 0x001D7E88
	public void AddPref(string id, string prefName, PrefType type)
	{
		if (this.prefNameToFile.ContainsKey(prefName))
		{
			throw new ArgumentException(prefName + " already exists in another group");
		}
		ISaveDataPrefs saveDataPrefs;
		if (!this.idToFile.TryGetValue(id, out saveDataPrefs))
		{
			saveDataPrefs = new SaveDataPrefsFile(Path.Combine(this.saveDir, id) + ".cfg");
			this.idToFile.Add(id, saveDataPrefs);
		}
		List<ValueTuple<string, PrefType>> list;
		if (!this.prefs.TryGetValue(id, out list))
		{
			list = new List<ValueTuple<string, PrefType>>();
			this.prefs.Add(id, list);
		}
		list.Add(new ValueTuple<string, PrefType>(prefName, type));
		this.prefNameToFile[prefName] = saveDataPrefs;
	}

	// Token: 0x06004C56 RID: 19542 RVA: 0x001D9D2C File Offset: 0x001D7F2C
	public void Save()
	{
		foreach (KeyValuePair<string, List<ValueTuple<string, PrefType>>> keyValuePair in this.prefs)
		{
			string text;
			List<ValueTuple<string, PrefType>> list;
			keyValuePair.Deconstruct(out text, out list);
			string text2 = text;
			List<ValueTuple<string, PrefType>> list2 = list;
			ISaveDataPrefs saveDataPrefs;
			if (!this.idToFile.TryGetValue(text2, out saveDataPrefs))
			{
				Log.Error("Expected to have pref file for " + text2);
			}
			else
			{
				foreach (ValueTuple<string, PrefType> valueTuple in list2)
				{
					switch (valueTuple.Item2)
					{
					case PrefType.Float:
						saveDataPrefs.SetFloat(valueTuple.Item1, SdPlayerPrefs.GetFloat(valueTuple.Item1));
						break;
					case PrefType.Int:
						saveDataPrefs.SetInt(valueTuple.Item1, SdPlayerPrefs.GetInt(valueTuple.Item1));
						break;
					case PrefType.String:
						saveDataPrefs.SetString(valueTuple.Item1, SdPlayerPrefs.GetString(valueTuple.Item1));
						break;
					}
				}
				saveDataPrefs.Save();
			}
		}
	}

	// Token: 0x06004C57 RID: 19543 RVA: 0x001D9E68 File Offset: 0x001D8068
	public void Apply()
	{
		foreach (KeyValuePair<string, List<ValueTuple<string, PrefType>>> keyValuePair in this.prefs)
		{
			string text;
			List<ValueTuple<string, PrefType>> list;
			keyValuePair.Deconstruct(out text, out list);
			string text2 = text;
			List<ValueTuple<string, PrefType>> list2 = list;
			ISaveDataPrefs saveDataPrefs;
			if (!this.idToFile.TryGetValue(text2, out saveDataPrefs))
			{
				Log.Error("Expected to have pref file for " + text2);
			}
			else
			{
				foreach (ValueTuple<string, PrefType> valueTuple in list2)
				{
					if (saveDataPrefs.HasKey(valueTuple.Item1))
					{
						switch (valueTuple.Item2)
						{
						case PrefType.Float:
							SdPlayerPrefs.SetFloat(valueTuple.Item1, saveDataPrefs.GetFloat(valueTuple.Item1));
							break;
						case PrefType.Int:
							SdPlayerPrefs.SetInt(valueTuple.Item1, saveDataPrefs.GetInt(valueTuple.Item1));
							break;
						case PrefType.String:
							SdPlayerPrefs.SetString(valueTuple.Item1, saveDataPrefs.GetString(valueTuple.Item1));
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x06004C58 RID: 19544 RVA: 0x001D9FC8 File Offset: 0x001D81C8
	public bool HasKey(string key)
	{
		return this.prefNameToFile.ContainsKey(key);
	}

	// Token: 0x06004C59 RID: 19545 RVA: 0x001D9FD8 File Offset: 0x001D81D8
	public bool TryGetFloat(string key, out float value)
	{
		ISaveDataPrefs saveDataPrefs;
		if (this.prefNameToFile.TryGetValue(key, out saveDataPrefs) && saveDataPrefs.HasKey(key))
		{
			value = saveDataPrefs.GetFloat(key);
			return true;
		}
		value = 0f;
		return false;
	}

	// Token: 0x06004C5A RID: 19546 RVA: 0x001DA014 File Offset: 0x001D8214
	public bool TryGetInt(string key, out int value)
	{
		ISaveDataPrefs saveDataPrefs;
		if (this.prefNameToFile.TryGetValue(key, out saveDataPrefs) && saveDataPrefs.HasKey(key))
		{
			value = saveDataPrefs.GetInt(key);
			return true;
		}
		value = 0;
		return false;
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x001DA04C File Offset: 0x001D824C
	public bool TryGetString(string key, out string value)
	{
		ISaveDataPrefs saveDataPrefs;
		if (this.prefNameToFile.TryGetValue(key, out saveDataPrefs) && saveDataPrefs.HasKey(key))
		{
			value = saveDataPrefs.GetString(key);
			return true;
		}
		value = null;
		return false;
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x001DA084 File Offset: 0x001D8284
	public bool TryGetGamePref(EnumGamePrefs enumGamePref, out object value)
	{
		GamePrefs.EnumType? prefType = GamePrefs.GetPrefType(enumGamePref);
		if (prefType == null)
		{
			Log.Error(string.Format("Unknown enum type for {0}", enumGamePref));
			value = null;
			return false;
		}
		string text = enumGamePref.ToStringCached<EnumGamePrefs>();
		switch (prefType.Value)
		{
		case GamePrefs.EnumType.Int:
		{
			int num;
			if (this.TryGetInt(text, out num))
			{
				value = num;
				return true;
			}
			break;
		}
		case GamePrefs.EnumType.Float:
		{
			float num2;
			if (this.TryGetFloat(text, out num2))
			{
				value = num2;
				return true;
			}
			break;
		}
		case GamePrefs.EnumType.String:
		{
			string text2;
			if (this.TryGetString(text, out text2))
			{
				value = text2;
				return true;
			}
			break;
		}
		case GamePrefs.EnumType.Bool:
		{
			int num3;
			if (this.TryGetInt(text, out num3))
			{
				value = num3 != 0;
				return true;
			}
			break;
		}
		case GamePrefs.EnumType.Binary:
		{
			string text3;
			if (this.TryGetString(text, out text3))
			{
				value = Utils.FromBase64(text3);
				return true;
			}
			break;
		}
		}
		value = null;
		return false;
	}

	// Token: 0x04003BF5 RID: 15349
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ext = ".cfg";

	// Token: 0x04003BF6 RID: 15350
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string saveDir;

	// Token: 0x04003BF7 RID: 15351
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, List<ValueTuple<string, PrefType>>> prefs = new Dictionary<string, List<ValueTuple<string, PrefType>>>();

	// Token: 0x04003BF8 RID: 15352
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, ISaveDataPrefs> idToFile = new Dictionary<string, ISaveDataPrefs>();

	// Token: 0x04003BF9 RID: 15353
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, ISaveDataPrefs> prefNameToFile = new Dictionary<string, ISaveDataPrefs>();
}
