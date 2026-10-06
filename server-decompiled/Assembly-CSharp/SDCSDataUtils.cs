using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000A5B RID: 2651
public static class SDCSDataUtils
{
	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06004FBF RID: 20415 RVA: 0x001E59AE File Offset: 0x001E3BAE
	public static string baseHairColorLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "AssetBundles/Player/Common/HairColorSwatches";
		}
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x001E59B5 File Offset: 0x001E3BB5
	public static void SetupData()
	{
		SDCSDataUtils.Load();
	}

	// Token: 0x06004FC1 RID: 20417 RVA: 0x001E59BC File Offset: 0x001E3BBC
	public static List<string> GetRaceList(bool isMale)
	{
		List<string> list = new List<string>();
		foreach (SDCSDataUtils.GenderKey genderKey in SDCSDataUtils.VariantData.Keys)
		{
			if (genderKey.IsMale == isMale)
			{
				list.Add(genderKey.Name);
			}
		}
		list.Sort((string a, string b) => b.CompareTo(a));
		return list;
	}

	// Token: 0x06004FC2 RID: 20418 RVA: 0x001E5A50 File Offset: 0x001E3C50
	public static List<string> GetVariantList(bool isMale, string raceName)
	{
		List<string> list = new List<string>();
		foreach (SDCSDataUtils.GenderKey genderKey in SDCSDataUtils.VariantData.Keys)
		{
			if (genderKey.IsMale == isMale && genderKey.Name.EqualsCaseInsensitive(raceName))
			{
				for (int i = 0; i < SDCSDataUtils.VariantData[genderKey].Count; i++)
				{
					list.Add(SDCSDataUtils.VariantData[genderKey][i].ToString());
				}
			}
		}
		return list;
	}

	// Token: 0x06004FC3 RID: 20419 RVA: 0x001E5AFC File Offset: 0x001E3CFC
	public static List<string> GetHairNames(bool isMale, SDCSDataUtils.HairTypes hairType)
	{
		List<string> list = new List<string>();
		Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> dictionary = null;
		switch (hairType)
		{
		case SDCSDataUtils.HairTypes.Hair:
			dictionary = SDCSDataUtils.HairDictionary;
			break;
		case SDCSDataUtils.HairTypes.Mustache:
			dictionary = SDCSDataUtils.MustacheDictionary;
			break;
		case SDCSDataUtils.HairTypes.Chops:
			dictionary = SDCSDataUtils.ChopsDictionary;
			break;
		case SDCSDataUtils.HairTypes.Beard:
			dictionary = SDCSDataUtils.BeardDictionary;
			break;
		}
		foreach (SDCSDataUtils.GenderKey genderKey in dictionary.Keys)
		{
			SDCSDataUtils.HairData hairData = dictionary[genderKey];
			if (genderKey.IsMale == isMale)
			{
				list.Add(genderKey.Name);
			}
		}
		return list;
	}

	// Token: 0x06004FC4 RID: 20420 RVA: 0x001E5BA4 File Offset: 0x001E3DA4
	public static List<string> GetEyeColorNames()
	{
		return SDCSDataUtils.EyeColorList;
	}

	// Token: 0x06004FC5 RID: 20421 RVA: 0x001E5BAC File Offset: 0x001E3DAC
	public static List<SDCSDataUtils.HairColorData> GetHairColorNames()
	{
		List<SDCSDataUtils.HairColorData> list = new List<SDCSDataUtils.HairColorData>();
		foreach (SDCSDataUtils.HairColorData hairColorData in SDCSDataUtils.HairColorDictionary.Values)
		{
			list.Add(hairColorData);
		}
		return list;
	}

	// Token: 0x06004FC6 RID: 20422 RVA: 0x001E5C0C File Offset: 0x001E3E0C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetupDataFromResources()
	{
		SDCSDataUtils.LoadRaceDataFromResources();
		SDCSDataUtils.EyeColorList = SDCSDataUtils.GetEyeColorNamesFromResources();
		SDCSDataUtils.LoadHairTypeFromResources(SDCSDataUtils.HairDictionary, SDCSDataUtils.HairTypes.Hair);
		SDCSDataUtils.LoadHairTypeFromResources(SDCSDataUtils.MustacheDictionary, SDCSDataUtils.HairTypes.Mustache);
		SDCSDataUtils.LoadHairTypeFromResources(SDCSDataUtils.ChopsDictionary, SDCSDataUtils.HairTypes.Chops);
		SDCSDataUtils.LoadHairTypeFromResources(SDCSDataUtils.BeardDictionary, SDCSDataUtils.HairTypes.Beard);
		SDCSDataUtils.LoadHairColorFromResources(SDCSDataUtils.HairColorDictionary);
	}

	// Token: 0x06004FC7 RID: 20423 RVA: 0x001E5C5E File Offset: 0x001E3E5E
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LoadRaceDataFromResources()
	{
		SDCSDataUtils.VariantData.Clear();
		SDCSDataUtils.ParseRaceVariantFromResources(true);
		SDCSDataUtils.ParseRaceVariantFromResources(false);
	}

	// Token: 0x06004FC8 RID: 20424 RVA: 0x001E5C78 File Offset: 0x001E3E78
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseRaceVariantFromResources(bool isMale)
	{
		string text = (isMale ? "Male" : "Female");
		foreach (DirectoryInfo directoryInfo in new DirectoryInfo(Application.dataPath + "/AssetBundles/Player/" + text + "/Heads/").GetDirectories())
		{
			SDCSDataUtils.GenderKey genderKey = new SDCSDataUtils.GenderKey(directoryInfo.Name, isMale);
			if (!SDCSDataUtils.VariantData.ContainsKey(genderKey))
			{
				SDCSDataUtils.VariantData.Add(genderKey, new List<int>());
			}
			foreach (DirectoryInfo directoryInfo2 in new DirectoryInfo(directoryInfo.FullName).GetDirectories())
			{
				SDCSDataUtils.VariantData[genderKey].Add(StringParsers.ParseSInt32(directoryInfo2.Name, 0, -1, NumberStyles.Integer));
			}
		}
	}

	// Token: 0x06004FC9 RID: 20425 RVA: 0x001E5D48 File Offset: 0x001E3F48
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LoadHairTypeFromResources(Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> dict, SDCSDataUtils.HairTypes hairType)
	{
		dict.Clear();
		List<string> list = SDCSDataUtils.GetHairNamesFromResources(true, hairType);
		for (int i = 0; i < list.Count; i++)
		{
			dict.Add(new SDCSDataUtils.GenderKey(list[i], true), new SDCSDataUtils.HairData
			{
				Name = list[i],
				IsMale = true
			});
		}
		list = SDCSDataUtils.GetHairNamesFromResources(false, hairType);
		for (int j = 0; j < list.Count; j++)
		{
			dict.Add(new SDCSDataUtils.GenderKey(list[j], false), new SDCSDataUtils.HairData
			{
				Name = list[j],
				IsMale = false
			});
		}
	}

	// Token: 0x06004FCA RID: 20426 RVA: 0x001E5DF4 File Offset: 0x001E3FF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LoadHairColorFromResources(Dictionary<string, SDCSDataUtils.HairColorData> dict)
	{
		dict.Clear();
		List<string> hairColorNamesFromResources = SDCSDataUtils.GetHairColorNamesFromResources();
		for (int i = 0; i < hairColorNamesFromResources.Count; i++)
		{
			string text = hairColorNamesFromResources[i];
			int num = StringParsers.ParseSInt32(text.Substring(0, 2), 0, -1, NumberStyles.Integer);
			string text2 = text.Substring(3);
			dict.Add(hairColorNamesFromResources[i], new SDCSDataUtils.HairColorData
			{
				Index = num,
				Name = text2,
				PrefabName = text
			});
		}
	}

	// Token: 0x06004FCB RID: 20427 RVA: 0x001E5E70 File Offset: 0x001E4070
	public static List<string> GetHairNamesFromResources(bool isMale, SDCSDataUtils.HairTypes hairType)
	{
		List<string> list = new List<string>();
		string text = (isMale ? "Male" : "Female");
		string text2 = Application.dataPath + "/AssetBundles/Player/" + text + "/";
		string text3 = ((hairType == SDCSDataUtils.HairTypes.Hair) ? (text2 + "Hair/") : string.Format("{0}FacialHair/{1}/", text2, hairType));
		if (Directory.Exists(text3))
		{
			foreach (DirectoryInfo directoryInfo in new DirectoryInfo(text3).GetDirectories())
			{
				list.Add(directoryInfo.Name);
			}
		}
		return list;
	}

	// Token: 0x06004FCC RID: 20428 RVA: 0x001E5F08 File Offset: 0x001E4108
	public static List<string> GetEyeColorNamesFromResources()
	{
		List<string> list = new List<string>();
		foreach (FileInfo fileInfo in new DirectoryInfo(Application.dataPath + "/AssetBundles/Player/Common/Eyes/Materials/").GetFiles())
		{
			if (!fileInfo.Name.EndsWith(".meta"))
			{
				list.Add(fileInfo.Name.Replace(".mat", ""));
			}
		}
		return list;
	}

	// Token: 0x06004FCD RID: 20429 RVA: 0x001E5F78 File Offset: 0x001E4178
	public static List<string> GetHairColorNamesFromResources()
	{
		List<string> list = new List<string>();
		foreach (FileInfo fileInfo in new DirectoryInfo(Application.dataPath + "/" + SDCSDataUtils.baseHairColorLoc + "/").GetFiles())
		{
			if (!fileInfo.Name.EndsWith(".meta"))
			{
				list.Add(fileInfo.Name.Replace(".asset", ""));
			}
		}
		return list;
	}

	// Token: 0x06004FCE RID: 20430 RVA: 0x001E5FF0 File Offset: 0x001E41F0
	public static void Save()
	{
		SDCSDataUtils.SetupDataFromResources();
		StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/Resources/sdcs.xml");
		string text = "\t";
		streamWriter.WriteLine("<sdcs>");
		foreach (SDCSDataUtils.GenderKey genderKey in SDCSDataUtils.VariantData.Keys)
		{
			if (SDCSDataUtils.VariantData[genderKey].Count > 0)
			{
				for (int i = 0; i < SDCSDataUtils.VariantData[genderKey].Count; i++)
				{
					streamWriter.WriteLine(string.Format("{0}<variant race=\"{1}\" index=\"{2}\" is_male=\"{3}\" />", new object[]
					{
						text,
						genderKey.Name,
						SDCSDataUtils.VariantData[genderKey][i],
						genderKey.IsMale
					}));
				}
			}
		}
		for (int j = 0; j < SDCSDataUtils.EyeColorList.Count; j++)
		{
			streamWriter.WriteLine(string.Format("{0}<eye_color name=\"{1}\" />", text, SDCSDataUtils.EyeColorList[j]));
		}
		foreach (SDCSDataUtils.HairColorData hairColorData in SDCSDataUtils.HairColorDictionary.Values)
		{
			streamWriter.WriteLine(string.Format("{0}<hair_color index=\"{1}\" name=\"{2}\" prefab_name=\"{3}\" />", new object[] { text, hairColorData.Index, hairColorData.Name, hairColorData.PrefabName }));
		}
		foreach (SDCSDataUtils.HairData hairData in SDCSDataUtils.HairDictionary.Values)
		{
			streamWriter.WriteLine(string.Format("{0}<hair name=\"{1}\" is_male=\"{2}\" />", text, hairData.Name, hairData.IsMale));
		}
		foreach (SDCSDataUtils.HairData hairData2 in SDCSDataUtils.MustacheDictionary.Values)
		{
			streamWriter.WriteLine(string.Format("{0}<mustache name=\"{1}\" is_male=\"{2}\" />", text, hairData2.Name, hairData2.IsMale));
		}
		foreach (SDCSDataUtils.HairData hairData3 in SDCSDataUtils.ChopsDictionary.Values)
		{
			streamWriter.WriteLine(string.Format("{0}<chops name=\"{1}\" is_male=\"{2}\" />", text, hairData3.Name, hairData3.IsMale));
		}
		foreach (SDCSDataUtils.HairData hairData4 in SDCSDataUtils.BeardDictionary.Values)
		{
			streamWriter.WriteLine(string.Format("{0}<beard name=\"{1}\" is_male=\"{2}\" />", text, hairData4.Name, hairData4.IsMale));
		}
		streamWriter.WriteLine("</sdcs>");
		streamWriter.Flush();
		streamWriter.Close();
	}

	// Token: 0x06004FCF RID: 20431 RVA: 0x001E6350 File Offset: 0x001E4550
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Load()
	{
		XElement root = XDocument.Parse(((TextAsset)Resources.Load("sdcs")).text, LoadOptions.SetLineInfo).Root;
		if (root == null || !root.HasElements)
		{
			return;
		}
		SDCSDataUtils.VariantData.Clear();
		SDCSDataUtils.EyeColorList.Clear();
		SDCSDataUtils.HairDictionary.Clear();
		SDCSDataUtils.HairColorDictionary.Clear();
		SDCSDataUtils.MustacheDictionary.Clear();
		SDCSDataUtils.ChopsDictionary.Clear();
		SDCSDataUtils.BeardDictionary.Clear();
		foreach (XElement xelement in root.Elements())
		{
			if (xelement.Name == "variant")
			{
				int num = -1;
				string text = "";
				bool flag = false;
				if (xelement.HasAttribute("index"))
				{
					num = StringParsers.ParseSInt32(xelement.GetAttribute("index"), 0, -1, NumberStyles.Integer);
				}
				if (xelement.HasAttribute("race"))
				{
					text = xelement.GetAttribute("race");
				}
				if (xelement.HasAttribute("is_male"))
				{
					flag = StringParsers.ParseBool(xelement.GetAttribute("is_male"), 0, -1, true);
				}
				SDCSDataUtils.GenderKey genderKey = new SDCSDataUtils.GenderKey(text, flag);
				if (!SDCSDataUtils.VariantData.ContainsKey(genderKey))
				{
					SDCSDataUtils.VariantData.Add(genderKey, new List<int>());
				}
				SDCSDataUtils.VariantData[genderKey].Add(num);
			}
			else if (xelement.Name == "eye_color")
			{
				SDCSDataUtils.EyeColorList.Add(xelement.GetAttribute("name"));
			}
			else if (xelement.Name == "hair_color")
			{
				int num2 = -1;
				string text2 = "";
				string text3 = "";
				if (xelement.HasAttribute("index"))
				{
					num2 = StringParsers.ParseSInt32(xelement.GetAttribute("index"), 0, -1, NumberStyles.Integer);
				}
				if (xelement.HasAttribute("name"))
				{
					text2 = xelement.GetAttribute("name");
				}
				if (xelement.HasAttribute("prefab_name"))
				{
					text3 = xelement.GetAttribute("prefab_name");
				}
				SDCSDataUtils.HairColorDictionary.Add(text2, new SDCSDataUtils.HairColorData
				{
					Index = num2,
					Name = text2,
					PrefabName = text3
				});
			}
			else if (xelement.Name == "hair")
			{
				SDCSDataUtils.HairData hairData = SDCSDataUtils.ParseHair(xelement);
				SDCSDataUtils.HairDictionary.Add(new SDCSDataUtils.GenderKey(hairData.Name, hairData.IsMale), hairData);
			}
			else if (xelement.Name == "mustache")
			{
				SDCSDataUtils.HairData hairData2 = SDCSDataUtils.ParseHair(xelement);
				SDCSDataUtils.MustacheDictionary.Add(new SDCSDataUtils.GenderKey(hairData2.Name, hairData2.IsMale), hairData2);
			}
			else if (xelement.Name == "chops")
			{
				SDCSDataUtils.HairData hairData3 = SDCSDataUtils.ParseHair(xelement);
				SDCSDataUtils.ChopsDictionary.Add(new SDCSDataUtils.GenderKey(hairData3.Name, hairData3.IsMale), hairData3);
			}
			else if (xelement.Name == "beard")
			{
				SDCSDataUtils.HairData hairData4 = SDCSDataUtils.ParseHair(xelement);
				SDCSDataUtils.BeardDictionary.Add(new SDCSDataUtils.GenderKey(hairData4.Name, hairData4.IsMale), hairData4);
			}
		}
	}

	// Token: 0x06004FD0 RID: 20432 RVA: 0x001E6708 File Offset: 0x001E4908
	[PublicizedFrom(EAccessModifier.Private)]
	public static SDCSDataUtils.HairData ParseHair(XElement element)
	{
		string text = "";
		bool flag = false;
		if (element.HasAttribute("name"))
		{
			text = element.GetAttribute("name");
		}
		if (element.HasAttribute("is_male"))
		{
			flag = StringParsers.ParseBool(element.GetAttribute("is_male"), 0, -1, true);
		}
		return new SDCSDataUtils.HairData
		{
			Name = text,
			IsMale = flag
		};
	}

	// Token: 0x04003D89 RID: 15753
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, List<int>> VariantData = new Dictionary<SDCSDataUtils.GenderKey, List<int>>();

	// Token: 0x04003D8A RID: 15754
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> HairDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003D8B RID: 15755
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> MustacheDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003D8C RID: 15756
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> ChopsDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003D8D RID: 15757
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> BeardDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003D8E RID: 15758
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<string> EyeColorList = new List<string>();

	// Token: 0x04003D8F RID: 15759
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, SDCSDataUtils.HairColorData> HairColorDictionary = new Dictionary<string, SDCSDataUtils.HairColorData>();

	// Token: 0x02000A5C RID: 2652
	public struct HairColorData
	{
		// Token: 0x04003D90 RID: 15760
		public int Index;

		// Token: 0x04003D91 RID: 15761
		public string Name;

		// Token: 0x04003D92 RID: 15762
		public string PrefabName;
	}

	// Token: 0x02000A5D RID: 2653
	public struct HairData
	{
		// Token: 0x04003D93 RID: 15763
		public string Name;

		// Token: 0x04003D94 RID: 15764
		public bool IsMale;
	}

	// Token: 0x02000A5E RID: 2654
	public struct GenderKey
	{
		// Token: 0x06004FD2 RID: 20434 RVA: 0x001E67D7 File Offset: 0x001E49D7
		public GenderKey(string name, bool isMale)
		{
			this.Name = name;
			this.IsMale = isMale;
		}

		// Token: 0x04003D95 RID: 15765
		public string Name;

		// Token: 0x04003D96 RID: 15766
		public bool IsMale;
	}

	// Token: 0x02000A5F RID: 2655
	public enum HairTypes
	{
		// Token: 0x04003D98 RID: 15768
		Hair,
		// Token: 0x04003D99 RID: 15769
		Mustache,
		// Token: 0x04003D9A RID: 15770
		Chops,
		// Token: 0x04003D9B RID: 15771
		Beard
	}
}
