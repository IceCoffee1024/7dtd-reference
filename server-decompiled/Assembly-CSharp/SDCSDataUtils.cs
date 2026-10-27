using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000A65 RID: 2661
public static class SDCSDataUtils
{
	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06004FE3 RID: 20451 RVA: 0x001E6A96 File Offset: 0x001E4C96
	public static string baseHairColorLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "AssetBundles/Player/Common/HairColorSwatches";
		}
	}

	// Token: 0x06004FE4 RID: 20452 RVA: 0x001E6A9D File Offset: 0x001E4C9D
	public static void SetupData()
	{
		SDCSDataUtils.Load();
	}

	// Token: 0x06004FE5 RID: 20453 RVA: 0x001E6AA4 File Offset: 0x001E4CA4
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

	// Token: 0x06004FE6 RID: 20454 RVA: 0x001E6B38 File Offset: 0x001E4D38
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

	// Token: 0x06004FE7 RID: 20455 RVA: 0x001E6BE4 File Offset: 0x001E4DE4
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

	// Token: 0x06004FE8 RID: 20456 RVA: 0x001E6C8C File Offset: 0x001E4E8C
	public static List<string> GetEyeColorNames()
	{
		return SDCSDataUtils.EyeColorList;
	}

	// Token: 0x06004FE9 RID: 20457 RVA: 0x001E6C94 File Offset: 0x001E4E94
	public static List<SDCSDataUtils.HairColorData> GetHairColorNames()
	{
		List<SDCSDataUtils.HairColorData> list = new List<SDCSDataUtils.HairColorData>();
		foreach (SDCSDataUtils.HairColorData hairColorData in SDCSDataUtils.HairColorDictionary.Values)
		{
			list.Add(hairColorData);
		}
		return list;
	}

	// Token: 0x06004FEA RID: 20458 RVA: 0x001E6CF4 File Offset: 0x001E4EF4
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

	// Token: 0x06004FEB RID: 20459 RVA: 0x001E6D46 File Offset: 0x001E4F46
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LoadRaceDataFromResources()
	{
		SDCSDataUtils.VariantData.Clear();
		SDCSDataUtils.ParseRaceVariantFromResources(true);
		SDCSDataUtils.ParseRaceVariantFromResources(false);
	}

	// Token: 0x06004FEC RID: 20460 RVA: 0x001E6D60 File Offset: 0x001E4F60
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

	// Token: 0x06004FED RID: 20461 RVA: 0x001E6E30 File Offset: 0x001E5030
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

	// Token: 0x06004FEE RID: 20462 RVA: 0x001E6EDC File Offset: 0x001E50DC
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

	// Token: 0x06004FEF RID: 20463 RVA: 0x001E6F58 File Offset: 0x001E5158
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

	// Token: 0x06004FF0 RID: 20464 RVA: 0x001E6FF0 File Offset: 0x001E51F0
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

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001E7060 File Offset: 0x001E5260
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

	// Token: 0x06004FF2 RID: 20466 RVA: 0x001E70D8 File Offset: 0x001E52D8
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

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001E7438 File Offset: 0x001E5638
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

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001E77F0 File Offset: 0x001E59F0
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

	// Token: 0x04003DAA RID: 15786
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, List<int>> VariantData = new Dictionary<SDCSDataUtils.GenderKey, List<int>>();

	// Token: 0x04003DAB RID: 15787
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> HairDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003DAC RID: 15788
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> MustacheDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003DAD RID: 15789
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> ChopsDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003DAE RID: 15790
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData> BeardDictionary = new Dictionary<SDCSDataUtils.GenderKey, SDCSDataUtils.HairData>();

	// Token: 0x04003DAF RID: 15791
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<string> EyeColorList = new List<string>();

	// Token: 0x04003DB0 RID: 15792
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, SDCSDataUtils.HairColorData> HairColorDictionary = new Dictionary<string, SDCSDataUtils.HairColorData>();

	// Token: 0x02000A66 RID: 2662
	public struct HairColorData
	{
		// Token: 0x04003DB1 RID: 15793
		public int Index;

		// Token: 0x04003DB2 RID: 15794
		public string Name;

		// Token: 0x04003DB3 RID: 15795
		public string PrefabName;
	}

	// Token: 0x02000A67 RID: 2663
	public struct HairData
	{
		// Token: 0x04003DB4 RID: 15796
		public string Name;

		// Token: 0x04003DB5 RID: 15797
		public bool IsMale;
	}

	// Token: 0x02000A68 RID: 2664
	public struct GenderKey
	{
		// Token: 0x06004FF6 RID: 20470 RVA: 0x001E78BF File Offset: 0x001E5ABF
		public GenderKey(string name, bool isMale)
		{
			this.Name = name;
			this.IsMale = isMale;
		}

		// Token: 0x04003DB6 RID: 15798
		public string Name;

		// Token: 0x04003DB7 RID: 15799
		public bool IsMale;
	}

	// Token: 0x02000A69 RID: 2665
	public enum HairTypes
	{
		// Token: 0x04003DB9 RID: 15801
		Hair,
		// Token: 0x04003DBA RID: 15802
		Mustache,
		// Token: 0x04003DBB RID: 15803
		Chops,
		// Token: 0x04003DBC RID: 15804
		Beard
	}
}
