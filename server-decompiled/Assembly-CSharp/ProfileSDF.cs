using System;
using System.IO;
using Platform;
using SDF;
using UnityEngine;

// Token: 0x02000A36 RID: 2614
public static class ProfileSDF
{
	// Token: 0x06004F1B RID: 20251 RVA: 0x001DFF30 File Offset: 0x001DE130
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

	// Token: 0x06004F1C RID: 20252 RVA: 0x001DFFB2 File Offset: 0x001DE1B2
	public static string CurrentProfileName()
	{
		return ProfileSDF.profileSDF.GetString("selectedProfile") ?? "";
	}

	// Token: 0x06004F1D RID: 20253 RVA: 0x001DFFCC File Offset: 0x001DE1CC
	public static void Save()
	{
		ProfileSDF.profileSDF.Save();
	}

	// Token: 0x06004F1E RID: 20254 RVA: 0x001DFFD8 File Offset: 0x001DE1D8
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

	// Token: 0x06004F1F RID: 20255 RVA: 0x001E0038 File Offset: 0x001DE238
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

	// Token: 0x06004F20 RID: 20256 RVA: 0x001E0099 File Offset: 0x001DE299
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

	// Token: 0x06004F21 RID: 20257 RVA: 0x001E00BC File Offset: 0x001DE2BC
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

	// Token: 0x06004F22 RID: 20258 RVA: 0x001E0150 File Offset: 0x001DE350
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

	// Token: 0x06004F23 RID: 20259 RVA: 0x001E024E File Offset: 0x001DE44E
	public static void SetSelectedProfile(string _profileName)
	{
		ProfileSDF.profileSDF.Set("selectedProfile", _profileName);
	}

	// Token: 0x06004F24 RID: 20260 RVA: 0x001E0260 File Offset: 0x001DE460
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

	// Token: 0x06004F25 RID: 20261 RVA: 0x001E02C4 File Offset: 0x001DE4C4
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

	// Token: 0x06004F26 RID: 20262 RVA: 0x001E0361 File Offset: 0x001DE561
	public static void setSex(string _profileName, bool _isMale)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".isMale", _isMale);
	}

	// Token: 0x06004F27 RID: 20263 RVA: 0x001E0379 File Offset: 0x001DE579
	public static void setRace(string _profileName, string _raceName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".race", _raceName);
	}

	// Token: 0x06004F28 RID: 20264 RVA: 0x001E0391 File Offset: 0x001DE591
	public static void setVariant(string _profileName, int _variantNumber)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".variant", _variantNumber);
	}

	// Token: 0x06004F29 RID: 20265 RVA: 0x001E03AC File Offset: 0x001DE5AC
	public static void setColor(string _profileName, string _colorName, Color _color)
	{
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".r", _color.r);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".g", _color.g);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".b", _color.b);
		ProfileSDF.profileSDF.Set(_profileName + "." + _colorName + ".a", _color.a);
	}

	// Token: 0x06004F2A RID: 20266 RVA: 0x001E043D File Offset: 0x001DE63D
	public static void setEyeColor(string _profileName, string _eyeColorName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".eyeColor", _eyeColorName);
	}

	// Token: 0x06004F2B RID: 20267 RVA: 0x001E0455 File Offset: 0x001DE655
	public static void setHairName(string _profileName, string _hairName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".hairName", _hairName);
	}

	// Token: 0x06004F2C RID: 20268 RVA: 0x001E046D File Offset: 0x001DE66D
	public static void setHairColor(string _profileName, string _hairColorName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".hairColor", _hairColorName);
	}

	// Token: 0x06004F2D RID: 20269 RVA: 0x001E0485 File Offset: 0x001DE685
	public static void setMustacheName(string _profileName, string _mustacheName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".mustacheName", _mustacheName);
	}

	// Token: 0x06004F2E RID: 20270 RVA: 0x001E049D File Offset: 0x001DE69D
	public static void setChopsName(string _profileName, string _chopsName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".chopsName", _chopsName);
	}

	// Token: 0x06004F2F RID: 20271 RVA: 0x001E04B5 File Offset: 0x001DE6B5
	public static void setBeardName(string _profileName, string _beardName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".beardName", _beardName);
	}

	// Token: 0x06004F30 RID: 20272 RVA: 0x001E04CD File Offset: 0x001DE6CD
	public static void setEyebrowName(string _profileName, string _eyebrowName)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".eyebrowName", _eyebrowName);
	}

	// Token: 0x06004F31 RID: 20273 RVA: 0x001E04E5 File Offset: 0x001DE6E5
	public static void setArchetype(string _profileName, string _archetype)
	{
		ProfileSDF.profileSDF.Set(_profileName + ".archetype", _archetype);
	}

	// Token: 0x06004F32 RID: 20274 RVA: 0x001E0500 File Offset: 0x001DE700
	public static Color GetSkinColor(string _profileName)
	{
		float num = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.r").GetValueOrDefault();
		float num2 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.g").GetValueOrDefault();
		float num3 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.b").GetValueOrDefault();
		float num4 = ProfileSDF.profileSDF.GetFloat(_profileName + ".skin.a").GetValueOrDefault();
		return new Color(num, num2, num3, num4);
	}

	// Token: 0x06004F33 RID: 20275 RVA: 0x001E0590 File Offset: 0x001DE790
	public static Color GetEyebrowColor(string _profileName)
	{
		float num = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.r").GetValueOrDefault();
		float num2 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.g").GetValueOrDefault();
		float num3 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.b").GetValueOrDefault();
		float num4 = ProfileSDF.profileSDF.GetFloat(_profileName + ".eyebrow.a").GetValueOrDefault();
		return new Color(num, num2, num3, num4);
	}

	// Token: 0x06004F34 RID: 20276 RVA: 0x001E0620 File Offset: 0x001DE820
	public static bool GetIsMale(string _profileName)
	{
		return ProfileSDF.profileSDF.GetBool(_profileName + ".isMale").GetValueOrDefault();
	}

	// Token: 0x06004F35 RID: 20277 RVA: 0x001E064C File Offset: 0x001DE84C
	public static string GetRaceName(string _profileName)
	{
		string @string = ProfileSDF.profileSDF.GetString(_profileName + ".race");
		if (@string == null)
		{
			return "White";
		}
		return @string;
	}

	// Token: 0x06004F36 RID: 20278 RVA: 0x001E067C File Offset: 0x001DE87C
	public static int GetVariantNumber(string _profileName)
	{
		return ProfileSDF.profileSDF.GetInt(_profileName + ".variant").GetValueOrDefault();
	}

	// Token: 0x06004F37 RID: 20279 RVA: 0x001E06A8 File Offset: 0x001DE8A8
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

	// Token: 0x06004F38 RID: 20280 RVA: 0x001E06E4 File Offset: 0x001DE8E4
	public static float GetBodyDna(string _profileName, string _bodyPartName)
	{
		return Mathf.Clamp01(ProfileSDF.profileSDF.GetFloat(_profileName + ".bodyData." + _bodyPartName).GetValueOrDefault());
	}

	// Token: 0x06004F39 RID: 20281 RVA: 0x001E0714 File Offset: 0x001DE914
	public static string GetEyeColorName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".eyeColor");
	}

	// Token: 0x06004F3A RID: 20282 RVA: 0x001E072B File Offset: 0x001DE92B
	public static string GetHairName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".hairName");
	}

	// Token: 0x06004F3B RID: 20283 RVA: 0x001E0742 File Offset: 0x001DE942
	public static string GetHairColorName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".hairColor");
	}

	// Token: 0x06004F3C RID: 20284 RVA: 0x001E0759 File Offset: 0x001DE959
	public static string GetMustacheName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".mustacheName");
	}

	// Token: 0x06004F3D RID: 20285 RVA: 0x001E0770 File Offset: 0x001DE970
	public static string GetChopsName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".chopsName");
	}

	// Token: 0x06004F3E RID: 20286 RVA: 0x001E0787 File Offset: 0x001DE987
	public static string GetBeardName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".beardName");
	}

	// Token: 0x06004F3F RID: 20287 RVA: 0x001E079E File Offset: 0x001DE99E
	public static string GetEyebrowName(string _profileName)
	{
		return ProfileSDF.profileSDF.GetString(_profileName + ".eyebrowName");
	}

	// Token: 0x06004F40 RID: 20288 RVA: 0x001E07B8 File Offset: 0x001DE9B8
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

	// Token: 0x04003CA3 RID: 15523
	[PublicizedFrom(EAccessModifier.Private)]
	public static SdfFile profileSDF;

	// Token: 0x04003CA4 RID: 15524
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PROFILE_NAMES = "profileNames";

	// Token: 0x04003CA5 RID: 15525
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PROFILE_FILENAME = "sdcs_profiles.sdf";
}
