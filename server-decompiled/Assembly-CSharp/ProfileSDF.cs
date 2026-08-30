using System;
using System.IO;
using Platform;
using SDF;
using UnityEngine;

// Token: 0x02000A47 RID: 2631
public static class ProfileSDF
{
	// Token: 0x06004FA1 RID: 20385 RVA: 0x001E29C8 File Offset: 0x001E0BC8
	public static void InitializeStatics()
	{
		string text = Path.Combine(GameIO.GetSaveGameRootDir(), "sdcs_profiles.sdf");
		ProfileSDF.profileSDF = new SdfFile(text);
		if (SdFile.Exists(text))
		{
			ProfileSDF.profileSDF.Load();
			return;
		}
		string text2 = Path.Combine(GameIO.GetSaveGameRootDir(UserDataStorageType.DeviceLocal), "sdcs_profiles.sdf");
		if (SdFile.Exists(text2))
		{
			Log.Out("ProfileSDF not found at " + text + ". Migrating from " + text2);
			SdfFile sdfFile = new SdfFile(text2);
			sdfFile.Load();
			ProfileSDF.profileSDF.CopyFrom(sdfFile);
		}
	}

	// Token: 0x06004FA2 RID: 20386 RVA: 0x001E2A4A File Offset: 0x001E0C4A
	public static string CurrentProfileName()
	{
		return ProfileSDF.profileSDF.GetString("selectedProfile") ?? "";
	}

	// Token: 0x06004FA3 RID: 20387 RVA: 0x001E2A64 File Offset: 0x001E0C64
	public static void Save()
	{
		ProfileSDF.profileSDF.Save();
	}

	// Token: 0x06004FA4 RID: 20388 RVA: 0x001E2A70 File Offset: 0x001E0C70
	public static bool ProfileExists(string _profileName)
	{
		string @string = ProfileSDF.profileSDF.GetString("profileNames");
		if (@string == null)
		{
			return false;
		}
		if (!@string.Contains(","))
		{
			return string.Equals(@string, _profileName, StringComparison.OrdinalIgnoreCase);
		}
		string[] array = @string.Split(',', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			if (string.Equals(array[i], _profileName, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004FA5 RID: 20389 RVA: 0x001E2AD0 File Offset: 0x001E0CD0
	public static void DeleteProfile(string _profileName)
	{
		if (!ProfileSDF.ProfileExists(_profileName))
		{
			return;
		}
		foreach (string text in ProfileSDF.profileSDF.GetKeys())
		{
			if (text.StartsWith(_profileName + "."))
			{
				ProfileSDF.profileSDF.Remove(text);
				ProfileSDF.Save();
			}
		}
		ProfileSDF.removeProfileName(_profileName);
		ProfileSDF.Save();
	}

	// Token: 0x06004FA6 RID: 20390 RVA: 0x001E2B31 File Offset: 0x001E0D31
	public static void SaveArchetype(string _archetype, bool _isMale)
	{
		if (!ProfileSDF.ProfileExists(_archetype))
		{
			ProfileSDF.addProfileName(_archetype);
			ProfileSDF.setSex(_archetype, _isMale);
			ProfileSDF.setArchetype(_archetype, _archetype);
		}
		ProfileSDF.Save();
	}

	// Token: 0x06004FA7 RID: 20391 RVA: 0x001E2B54 File Offset: 0x001E0D54
	public static void SaveProfile(string _profileName, string _archetype, bool _isMale, string _raceName, int _variantNumber, string _eyeColorName, string _hairName, string _hairColor, string _mustacheName, string _chopsName, string _beardName)
	{
		if (!ProfileSDF.ProfileExists(_profileName))
		{
			ProfileSDF.addProfileName(_profileName);
		}
		if (_archetype == "")
		{
			if (_isMale)
			{
				_archetype = "BaseMale";
			}
			else
			{
				_archetype = "BaseFemale";
			}
		}
		ProfileSDF.setSex(_profileName, _isMale);
		ProfileSDF.setRace(_profileName, _raceName);
		ProfileSDF.setArchetype(_profileName, _archetype);
		ProfileSDF.setVariant(_profileName, _variantNumber);
		ProfileSDF.setEyeColor(_profileName, _eyeColorName);
		ProfileSDF.setHairName(_profileName, _hairName);
		ProfileSDF.setHairColor(_profileName, _hairColor);
		ProfileSDF.setMustacheName(_profileName, _mustacheName);
		ProfileSDF.setChopsName(_profileName, _chopsName);
		ProfileSDF.setBeardName(_profileName, _beardName);
		ProfileSDF.SetSelectedProfile(_profileName);
		ProfileSDF.Save();
	}

	// Token: 0x06004FA8 RID: 20392 RVA: 0x001E2BE8 File Offset: 0x001E0DE8
	public static Archetype CreateTempArchetype(string _profileName)
	{
		Archetype archetype = new Archetype(_profileName, ProfileSDF.GetIsMale(_profileName), true);
		archetype.Race = ProfileSDF.GetRaceName(_profileName);
		archetype.Variant = ProfileSDF.GetVariantNumber(_profileName);
		archetype.Hair = ProfileSDF.GetHairName(_profileName);
		archetype.HairColor = ProfileSDF.GetHairColorName(_profileName);
		archetype.MustacheName = ProfileSDF.GetMustacheName(_profileName);
		archetype.ChopsName = ProfileSDF.GetChopsName(_profileName);
		archetype.BeardName = ProfileSDF.GetBeardName(_profileName);
		archetype.EyeColorName = ProfileSDF.GetEyeColorName(_profileName);
		archetype.AddEquipmentSlot(new SDCSUtils.SlotData
		{
			PartName = "body",
			PrefabName = "@:Entities/Player/{sex}/Gear/LumberJack/gear{sex}LumberJackPrefab.prefab",
			BaseToTurnOff = "body"
		});
		archetype.AddEquipmentSlot(new SDCSUtils.SlotData
		{
			PartName = "hands",
			PrefabName = "@:Entities/Player/{sex}/Gear/LumberJack/gear{sex}LumberJackPrefab.prefab",
			BaseToTurnOff = "hands"
		});
		archetype.AddEquipmentSlot(new SDCSUtils.SlotData
		{
			PartName = "feet",
			PrefabName = "@:Entities/Player/{sex}/Gear/LumberJack/gear{sex}LumberJackPrefab.prefab",
			BaseToTurnOff = "feet"
		});
		return archetype;
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x001E2CE6 File Offset: 0x001E0EE6
	public static void SetSelectedProfile(string _profileName)
	{
		ProfileSDF.profileSDF.Set("selectedProfile", _profileName);
	}

	// Token: 0x06004FAA RID: 20394 RVA: 0x001E2CF8 File Offset: 0x001E0EF8
	public static void addProfileName(string _profileName)
	{
		string @string = ProfileSDF.profileSDF.GetString("profileNames");
		if (@string == null || @string.Length == 0)
		{
			ProfileSDF.profileSDF.Set("profileNames", _profileName, false);
			return;
		}
		ProfileSDF.profileSDF.Set("profileNames", ProfileSDF.profileSDF.GetString("profileNames") + "," + _profileName, false);
	}

	// Token: 0x06004FAB RID: 20395 RVA: 0x001E2D5C File Offset: 0x001E0F5C
	public static void removeProfileName(string _profileName)
	{
		string @string = ProfileSDF.profileSDF.GetString("profileNames");
		if (@string == null || @string.Length == 0)
		{
			ProfileSDF.profileSDF.Set("profileNames", "", false);
			return;
		}
		string text = "";
		bool flag = true;
		foreach (string text2 in @string.Split(',', StringSplitOptions.None))
		{
			if (text2 != _profileName)
			{
				if (!flag)
				{
					text += ",";
				}
				text += text2;
				flag = false;
			}
		}
		ProfileSDF.profileSDF.Set("profileNames", text, false);
	}

	// Token: 0x06004FAC RID: 20396 RVA: 0x001E2DF9 File Offset: 0x001E0FF9
	public static void setSex(string _profileName, bool _isMale)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".isMale", _isMale);
	}

	// Token: 0x06004FAD RID: 20397 RVA: 0x001E2E11 File Offset: 0x001E1011
	public static void setRace(string _profileName, string _raceName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".race", _raceName);
	}

	// Token: 0x06004FAE RID: 20398 RVA: 0x001E2E29 File Offset: 0x001E1029
	public static void setVariant(string _profileName, int _variantNumber)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".variant", _variantNumber);
	}

	// Token: 0x06004FAF RID: 20399 RVA: 0x001E2E44 File Offset: 0x001E1044
	public static void setColor(string _profileName, string _colorName, Color _color)
	{
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".r", _color.r);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".g", _color.g);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".b", _color.b);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".a", _color.a);
	}

	// Token: 0x06004FB0 RID: 20400 RVA: 0x001E2ED5 File Offset: 0x001E10D5
	public static void setEyeColor(string _profileName, string _eyeColorName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".eyeColor", _eyeColorName);
	}

	// Token: 0x06004FB1 RID: 20401 RVA: 0x001E2EED File Offset: 0x001E10ED
	public static void setHairName(string _profileName, string _hairName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".hairName", _hairName);
	}

	// Token: 0x06004FB2 RID: 20402 RVA: 0x001E2F05 File Offset: 0x001E1105
	public static void setHairColor(string _profileName, string _hairColorName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".hairColor", _hairColorName);
	}

	// Token: 0x06004FB3 RID: 20403 RVA: 0x001E2F1D File Offset: 0x001E111D
	public static void setMustacheName(string _profileName, string _mustacheName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".mustacheName", _mustacheName);
	}

	// Token: 0x06004FB4 RID: 20404 RVA: 0x001E2F35 File Offset: 0x001E1135
	public static void setChopsName(string _profileName, string _chopsName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".chopsName", _chopsName);
	}

	// Token: 0x06004FB5 RID: 20405 RVA: 0x001E2F4D File Offset: 0x001E114D
	public static void setBeardName(string _profileName, string _beardName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".beardName", _beardName);
	}

	// Token: 0x06004FB6 RID: 20406 RVA: 0x001E2F65 File Offset: 0x001E1165
	public static void setEyebrowName(string _profileName, string _eyebrowName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".eyebrowName", _eyebrowName);
	}

	// Token: 0x06004FB7 RID: 20407 RVA: 0x001E2F7D File Offset: 0x001E117D
	public static void setArchetype(string _profileName, string _archetype)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".archetype", _archetype);
	}

	// Token: 0x06004FB8 RID: 20408 RVA: 0x001E2F98 File Offset: 0x001E1198
	public static Color GetSkinColor(string _profileName)
	{
		float num = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.r").GetValueOrDefault();
		float num2 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.g").GetValueOrDefault();
		float num3 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.b").GetValueOrDefault();
		float num4 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.a").GetValueOrDefault();
		return new Color(num, num2, num3, num4);
	}

	// Token: 0x06004FB9 RID: 20409 RVA: 0x001E3028 File Offset: 0x001E1228
	public static Color GetEyebrowColor(string _profileName)
	{
		float num = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.r").GetValueOrDefault();
		float num2 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.g").GetValueOrDefault();
		float num3 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.b").GetValueOrDefault();
		float num4 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.a").GetValueOrDefault();
		return new Color(num, num2, num3, num4);
	}

	// Token: 0x06004FBA RID: 20410 RVA: 0x001E30B8 File Offset: 0x001E12B8
	public static bool GetIsMale(string _profileName)
	{
		return ProfileSDF.profileSDF.GetBool(_profileName + ".isMale").GetValueOrDefault();
	}

	// Token: 0x06004FBB RID: 20411 RVA: 0x001E30E4 File Offset: 0x001E12E4
	public static string GetRaceName(string _profileName)
	{
		string @string = ProfileSDF.profileSDF.GetString(_profileName + ".race");
		if (@string == null)
		{
			return "White";
		}
		return @string;
	}

	// Token: 0x06004FBC RID: 20412 RVA: 0x001E3114 File Offset: 0x001E1314
	public static int GetVariantNumber(string _profileName)
	{
		return ProfileSDF.profileSDF.GetInt(_profileName + ".variant").GetValueOrDefault();
	}

	// Token: 0x06004FBD RID: 20413 RVA: 0x001E3140 File Offset: 0x001E1340
	public static string GetArchetype(string _profileName)
	{
		string @string = ProfileSDF.profileSDF.GetString(_profileName + ".archetype");
		if (@string != null)
		{
			return @string;
		}
		if (!ProfileSDF.GetIsMale(_profileName))
		{
			return "BaseFemale";
		}
		return "BaseMale";
	}

	// Token: 0x06004FBE RID: 20414 RVA: 0x001E317C File Offset: 0x001E137C
	public static float GetBodyDna(string _profileName, string _bodyPartName)
	{
		return Mathf.Clamp01(ProfileSDF.profileSDF.GetFloat(_profileName + ".bodyData." + _bodyPartName).GetValueOrDefault());
	}

	// Token: 0x06004FBF RID: 20415 RVA: 0x001E31AC File Offset: 0x001E13AC
	public static string GetEyeColorName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".eyeColor");
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x001E31C3 File Offset: 0x001E13C3
	public static string GetHairName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".hairName");
	}

	// Token: 0x06004FC1 RID: 20417 RVA: 0x001E31DA File Offset: 0x001E13DA
	public static string GetHairColorName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".hairColor");
	}

	// Token: 0x06004FC2 RID: 20418 RVA: 0x001E31F1 File Offset: 0x001E13F1
	public static string GetMustacheName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".mustacheName");
	}

	// Token: 0x06004FC3 RID: 20419 RVA: 0x001E3208 File Offset: 0x001E1408
	public static string GetChopsName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".chopsName");
	}

	// Token: 0x06004FC4 RID: 20420 RVA: 0x001E321F File Offset: 0x001E141F
	public static string GetBeardName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".beardName");
	}

	// Token: 0x06004FC5 RID: 20421 RVA: 0x001E3236 File Offset: 0x001E1436
	public static string GetEyebrowName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".eyebrowName");
	}

	// Token: 0x06004FC6 RID: 20422 RVA: 0x001E3250 File Offset: 0x001E1450
	public static string[] GetProfiles()
	{
		string @string = ProfileSDF.profileSDF.GetString("profileNames");
		if (@string == null)
		{
			return new string[0];
		}
		if (@string.Contains(","))
		{
			return ProfileSDF.profileSDF.GetString("profileNames").Split(',', StringSplitOptions.None);
		}
		return new string[] { @string };
	}

	// Token: 0x04003D34 RID: 15668
	[PublicizedFrom(EAccessModifier.Private)]
	public static SdfFile profileSDF;

	// Token: 0x04003D35 RID: 15669
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PROFILE_NAMES = "profileNames";

	// Token: 0x04003D36 RID: 15670
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PROFILE_FILENAME = "sdcs_profiles.sdf";
}
