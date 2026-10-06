using System;
using System.IO;
using Platform;
using SDF;
using UnityEngine;

// Token: 0x02000A51 RID: 2641
public static class ProfileSDF
{
	// Token: 0x06004F70 RID: 20336 RVA: 0x001E3C78 File Offset: 0x001E1E78
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

	// Token: 0x06004F71 RID: 20337 RVA: 0x001E3CFA File Offset: 0x001E1EFA
	public static string CurrentProfileName()
	{
		return ProfileSDF.profileSDF.GetString("selectedProfile") ?? "";
	}

	// Token: 0x06004F72 RID: 20338 RVA: 0x001E3D14 File Offset: 0x001E1F14
	public static void Save()
	{
		ProfileSDF.profileSDF.Save();
	}

	// Token: 0x06004F73 RID: 20339 RVA: 0x001E3D20 File Offset: 0x001E1F20
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

	// Token: 0x06004F74 RID: 20340 RVA: 0x001E3D80 File Offset: 0x001E1F80
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

	// Token: 0x06004F75 RID: 20341 RVA: 0x001E3DE1 File Offset: 0x001E1FE1
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

	// Token: 0x06004F76 RID: 20342 RVA: 0x001E3E04 File Offset: 0x001E2004
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

	// Token: 0x06004F77 RID: 20343 RVA: 0x001E3E98 File Offset: 0x001E2098
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

	// Token: 0x06004F78 RID: 20344 RVA: 0x001E3F96 File Offset: 0x001E2196
	public static void SetSelectedProfile(string _profileName)
	{
		ProfileSDF.profileSDF.Set("selectedProfile", _profileName);
	}

	// Token: 0x06004F79 RID: 20345 RVA: 0x001E3FA8 File Offset: 0x001E21A8
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

	// Token: 0x06004F7A RID: 20346 RVA: 0x001E400C File Offset: 0x001E220C
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

	// Token: 0x06004F7B RID: 20347 RVA: 0x001E40A9 File Offset: 0x001E22A9
	public static void setSex(string _profileName, bool _isMale)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".isMale", _isMale);
	}

	// Token: 0x06004F7C RID: 20348 RVA: 0x001E40C1 File Offset: 0x001E22C1
	public static void setRace(string _profileName, string _raceName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".race", _raceName);
	}

	// Token: 0x06004F7D RID: 20349 RVA: 0x001E40D9 File Offset: 0x001E22D9
	public static void setVariant(string _profileName, int _variantNumber)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".variant", _variantNumber);
	}

	// Token: 0x06004F7E RID: 20350 RVA: 0x001E40F4 File Offset: 0x001E22F4
	public static void setColor(string _profileName, string _colorName, Color _color)
	{
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".r", _color.r);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".g", _color.g);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".b", _color.b);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".a", _color.a);
	}

	// Token: 0x06004F7F RID: 20351 RVA: 0x001E4185 File Offset: 0x001E2385
	public static void setEyeColor(string _profileName, string _eyeColorName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".eyeColor", _eyeColorName);
	}

	// Token: 0x06004F80 RID: 20352 RVA: 0x001E419D File Offset: 0x001E239D
	public static void setHairName(string _profileName, string _hairName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".hairName", _hairName);
	}

	// Token: 0x06004F81 RID: 20353 RVA: 0x001E41B5 File Offset: 0x001E23B5
	public static void setHairColor(string _profileName, string _hairColorName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".hairColor", _hairColorName);
	}

	// Token: 0x06004F82 RID: 20354 RVA: 0x001E41CD File Offset: 0x001E23CD
	public static void setMustacheName(string _profileName, string _mustacheName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".mustacheName", _mustacheName);
	}

	// Token: 0x06004F83 RID: 20355 RVA: 0x001E41E5 File Offset: 0x001E23E5
	public static void setChopsName(string _profileName, string _chopsName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".chopsName", _chopsName);
	}

	// Token: 0x06004F84 RID: 20356 RVA: 0x001E41FD File Offset: 0x001E23FD
	public static void setBeardName(string _profileName, string _beardName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".beardName", _beardName);
	}

	// Token: 0x06004F85 RID: 20357 RVA: 0x001E4215 File Offset: 0x001E2415
	public static void setEyebrowName(string _profileName, string _eyebrowName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".eyebrowName", _eyebrowName);
	}

	// Token: 0x06004F86 RID: 20358 RVA: 0x001E422D File Offset: 0x001E242D
	public static void setArchetype(string _profileName, string _archetype)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".archetype", _archetype);
	}

	// Token: 0x06004F87 RID: 20359 RVA: 0x001E4248 File Offset: 0x001E2448
	public static Color GetSkinColor(string _profileName)
	{
		float num = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.r").GetValueOrDefault();
		float num2 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.g").GetValueOrDefault();
		float num3 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.b").GetValueOrDefault();
		float num4 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.a").GetValueOrDefault();
		return new Color(num, num2, num3, num4);
	}

	// Token: 0x06004F88 RID: 20360 RVA: 0x001E42D8 File Offset: 0x001E24D8
	public static Color GetEyebrowColor(string _profileName)
	{
		float num = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.r").GetValueOrDefault();
		float num2 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.g").GetValueOrDefault();
		float num3 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.b").GetValueOrDefault();
		float num4 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.a").GetValueOrDefault();
		return new Color(num, num2, num3, num4);
	}

	// Token: 0x06004F89 RID: 20361 RVA: 0x001E4368 File Offset: 0x001E2568
	public static bool GetIsMale(string _profileName)
	{
		return ProfileSDF.profileSDF.GetBool(_profileName + ".isMale").GetValueOrDefault();
	}

	// Token: 0x06004F8A RID: 20362 RVA: 0x001E4394 File Offset: 0x001E2594
	public static string GetRaceName(string _profileName)
	{
		string @string = ProfileSDF.profileSDF.GetString(_profileName + ".race");
		if (@string == null)
		{
			return "White";
		}
		return @string;
	}

	// Token: 0x06004F8B RID: 20363 RVA: 0x001E43C4 File Offset: 0x001E25C4
	public static int GetVariantNumber(string _profileName)
	{
		return ProfileSDF.profileSDF.GetInt(_profileName + ".variant").GetValueOrDefault();
	}

	// Token: 0x06004F8C RID: 20364 RVA: 0x001E43F0 File Offset: 0x001E25F0
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

	// Token: 0x06004F8D RID: 20365 RVA: 0x001E442C File Offset: 0x001E262C
	public static float GetBodyDna(string _profileName, string _bodyPartName)
	{
		return Mathf.Clamp01(ProfileSDF.profileSDF.GetFloat(_profileName + ".bodyData." + _bodyPartName).GetValueOrDefault());
	}

	// Token: 0x06004F8E RID: 20366 RVA: 0x001E445C File Offset: 0x001E265C
	public static string GetEyeColorName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".eyeColor");
	}

	// Token: 0x06004F8F RID: 20367 RVA: 0x001E4473 File Offset: 0x001E2673
	public static string GetHairName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".hairName");
	}

	// Token: 0x06004F90 RID: 20368 RVA: 0x001E448A File Offset: 0x001E268A
	public static string GetHairColorName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".hairColor");
	}

	// Token: 0x06004F91 RID: 20369 RVA: 0x001E44A1 File Offset: 0x001E26A1
	public static string GetMustacheName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".mustacheName");
	}

	// Token: 0x06004F92 RID: 20370 RVA: 0x001E44B8 File Offset: 0x001E26B8
	public static string GetChopsName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".chopsName");
	}

	// Token: 0x06004F93 RID: 20371 RVA: 0x001E44CF File Offset: 0x001E26CF
	public static string GetBeardName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".beardName");
	}

	// Token: 0x06004F94 RID: 20372 RVA: 0x001E44E6 File Offset: 0x001E26E6
	public static string GetEyebrowName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".eyebrowName");
	}

	// Token: 0x06004F95 RID: 20373 RVA: 0x001E4500 File Offset: 0x001E2700
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

	// Token: 0x04003D41 RID: 15681
	[PublicizedFrom(EAccessModifier.Private)]
	public static SdfFile profileSDF;

	// Token: 0x04003D42 RID: 15682
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PROFILE_NAMES = "profileNames";

	// Token: 0x04003D43 RID: 15683
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PROFILE_FILENAME = "sdcs_profiles.sdf";
}
