using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

// Token: 0x02000CE0 RID: 3296
public class SDCSArchetypesFromXml
{
	// Token: 0x060064E1 RID: 25825 RVA: 0x00276BF4 File Offset: 0x00274DF4
	public static void Load(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (root == null || !root.HasElements)
		{
			return;
		}
		foreach (XElement xelement in root.Elements())
		{
			if (xelement.Name == "archetype")
			{
				SDCSArchetypesFromXml.parseArchetype(xelement);
			}
		}
	}

	// Token: 0x060064E2 RID: 25826 RVA: 0x00276C70 File Offset: 0x00274E70
	public static void Save(string _filename, List<Archetype> _archetypes)
	{
		StreamWriter streamWriter = new StreamWriter(GameIO.GetGameDir("Data/Config") + "/" + _filename + ".xml");
		string text = "\t";
		streamWriter.WriteLine("<archetypes>");
		for (int i = 0; i < _archetypes.Count; i++)
		{
			Archetype archetype = _archetypes[i];
			streamWriter.WriteLine(string.Format("{0}<archetype name=\"{1}\" male=\"{2}\" race=\"{3}\" variant=\"{4}\" />", new object[]
			{
				text,
				archetype.Name,
				archetype.IsMale.ToString().ToLower(),
				archetype.Race,
				archetype.Variant
			}));
		}
		streamWriter.WriteLine("</archetypes>");
		streamWriter.Flush();
		streamWriter.Close();
	}

	// Token: 0x060064E3 RID: 25827 RVA: 0x00276D2C File Offset: 0x00274F2C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseArchetype(XElement element)
	{
		bool flag = false;
		string text = "";
		bool flag2 = false;
		string text2 = "White";
		int num = 1;
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "blue01";
		if (element.Name == "archetype")
		{
			if (element.HasAttribute("name"))
			{
				text = element.GetAttribute("name");
				if (text == "BaseMale" || text == "BaseFemale")
				{
					flag = true;
					flag2 = text == "BaseMale";
					text2 = "White";
					num = 1;
				}
				else if (element.HasAttribute("male"))
				{
					flag2 = StringParsers.ParseBool(element.GetAttribute("male"), 0, -1, true);
				}
			}
			if (element.HasAttribute("race"))
			{
				text2 = element.GetAttribute("race");
			}
			if (element.HasAttribute("variant"))
			{
				num = StringParsers.ParseSInt32(element.GetAttribute("variant"), 0, -1, NumberStyles.Integer);
			}
			if (element.HasAttribute("hair"))
			{
				text3 = element.GetAttribute("hair");
			}
			if (element.HasAttribute("hair_color"))
			{
				text4 = element.GetAttribute("hair_color");
			}
			if (element.HasAttribute("mustache"))
			{
				text5 = element.GetAttribute("mustache");
			}
			if (element.HasAttribute("chops"))
			{
				text6 = element.GetAttribute("chops");
			}
			if (element.HasAttribute("beard"))
			{
				text7 = element.GetAttribute("beard");
			}
			if (element.HasAttribute("eye_color"))
			{
				text8 = element.GetAttribute("eye_color");
			}
			else
			{
				text8 = "Blue01";
			}
		}
		Archetype archetype = new Archetype(text, flag2, flag);
		archetype.Race = text2;
		archetype.Variant = num;
		archetype.Hair = text3;
		archetype.HairColor = text4;
		archetype.MustacheName = text5;
		archetype.ChopsName = text6;
		archetype.BeardName = text7;
		archetype.EyeColorName = text8;
		foreach (XElement xelement in element.Elements())
		{
			if (xelement.Name == "equipment")
			{
				SDCSUtils.SlotData slotData = SDCSArchetypesFromXml.parseEquipment(xelement);
				if (slotData != null)
				{
					archetype.AddEquipmentSlot(slotData);
				}
			}
		}
		Archetype.SetArchetype(archetype);
	}

	// Token: 0x060064E4 RID: 25828 RVA: 0x00277008 File Offset: 0x00275208
	[PublicizedFrom(EAccessModifier.Private)]
	public static SDCSUtils.SlotData parseEquipment(XElement element)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		if (element.HasAttribute("transform_name"))
		{
			text = element.GetAttribute("transform_name");
		}
		if (element.HasAttribute("prefab"))
		{
			text2 = element.GetAttribute("prefab");
		}
		if (element.HasAttribute("excludes"))
		{
			text3 = element.GetAttribute("excludes");
		}
		if (text != "" && text2 != "")
		{
			return new SDCSUtils.SlotData
			{
				PartName = text,
				PrefabName = text2,
				BaseToTurnOff = text3
			};
		}
		return null;
	}
}
