using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000A51 RID: 2641
public static class SDCSDataUtils
{
	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x06004FEF RID: 20463 RVA: 0x001E4668 File Offset: 0x001E2868
	public static string baseHairColorLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "AssetBundles/Player/Common/HairColorSwatches";
		}
	}

	// Token: 0x06004FF0 RID: 20464 RVA: 0x001E466F File Offset: 0x001E286F
	public static void SetupData()
	{
		SDCSDataUtils.Load();
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001E4678 File Offset: 0x001E2878
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

	// Token: 0x06004FF2 RID: 20466 RVA: 0x001E470C File Offset: 0x001E290C
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

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001E47B8 File Offset: 0x001E29B8
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

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001E4860 File Offset: 0x001E2A60
	public static List<string> GetEyeColorNames()
	{
		return SDCSDataUtils.EyeColorList;
	}

	// Token: 0x06004FF5 RID: 20469 RVA: 0x001E4868 File Offset: 0x001E2A68
	public static List<SDCSDataUtils.HairColorData> GetHairColorNames()
	{
		List<SDCSDataUtils.HairColorData> list = new List<SDCSDataUtils.HairColorData>();
		foreach (SDCSDataUtils.HairColorData hairColorData in SDCSDataUtils.HairColorDictionary.Values)
		{
			list.Add(hairColorData);
		}
		return list;
	}

	// Token: 0x06004FF6 RID: 20470 RVA: 0x001E48C8 File Offset: 0x001E2AC8
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

	// Token: 0x06004FF7 RID: 20471 RVA: 0x001E491A File Offset: 0x001E2B1A
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LoadRaceDataFromResources()
	{
		SDCSDataUtils.VariantData.Clear();
		SDCSDataUtils.ParseRaceVariantFromResources(true);
		SDCSDataUtils.ParseRaceVariantFromResources(false);
	}

	// Token: 0x06004FF8 RID: 20472 RVA: 0x001E4934 File Offset: 0x001E2B34
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

	// Token: 0x06004FF9 RID: 20473 RVA: 0x001E4A04 File Offset: 0x001E2C04
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

	// Token: 0x06004FFA RID: 20474 RVA: 0x001E4AB0 File Offset: 0x001E2CB0
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

	// Token: 0x06004FFB RID: 20475 RVA: 0x001E4B2C File Offset: 0x001E2D2C
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

	// Token: 0x06004FFC RID: 20476 RVA: 0x001E4BC4 File Offset: 0x001E2DC4
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

	// Token: 0x06004FFD RID: 20477 RVA: 0x001E4C34 File Offset: 0x001E2E34
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

	// Token: 0x06004FFE RID: 20478 RVA: 0x001E4CAC File Offset: 0x001E2EAC
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

	// Token: 0x06004FFF RID: 20479 RVA: 0x001E500C File Offset: 0x001E320C
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

	// Token: 0x06005000 RID: 20480 RVA: 0x001E53C4 File Offset: 0x001E35C4
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

	// Token: 0x04003D7C RID: 15740
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, List<int>> VariantData = new Dictionary<SDCSDataUtils.GenderKey, List<int>>();

	// Token: 0x04003D7D RID: 15741
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> HairDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003D7E RID: 15742
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> MustacheDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003D7F RID: 15743
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> ChopsDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003D80 RID: 15744
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> BeardDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003D81 RID: 15745
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<string> EyeColorList = new List<string>();

	// Token: 0x04003D82 RID: 15746
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, SDCSDataUtils.HairColorData> HairColorDictionary = new Dictionary<string, SDCSDataUtils.HairColorData>();

	// Token: 0x02000A52 RID: 2642
	public struct HairColorData
	{
		// Token: 0x04003D83 RID: 15747
		public int Index;

		// Token: 0x04003D84 RID: 15748
		public string Name;

		// Token: 0x04003D85 RID: 15749
		public string PrefabName;
	}

	// Token: 0x02000A53 RID: 2643
	public struct HairData
	{
		// Token: 0x04003D86 RID: 15750
		public string Name;

		// Token: 0x04003D87 RID: 15751
		public bool IsMale;
	}

	// Token: 0x02000A54 RID: 2644
	public struct GenderKey
	{
		// Token: 0x06005002 RID: 20482 RVA: 0x001E5493 File Offset: 0x001E3693
		public GenderKey(string name, bool isMale)
		{
			this.Name = name;
			this.IsMale = isMale;
		}

		// Token: 0x04003D88 RID: 15752
		public string Name;

		// Token: 0x04003D89 RID: 15753
		public bool IsMale;
	}

	// Token: 0x02000A55 RID: 2645
	public enum HairTypes
	{
		// Token: 0x04003D8B RID: 15755
		Hair,
		// Token: 0x04003D8C RID: 15756
		Mustache,
		// Token: 0x04003D8D RID: 15757
		Chops,
		// Token: 0x04003D8E RID: 15758
		Beard
	}
}
