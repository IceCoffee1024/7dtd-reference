using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

// Token: 0x02000D16 RID: 3350
public class EntityGroupsFromXml
{
	// Token: 0x060065DB RID: 26075 RVA: 0x00281688 File Offset: 0x0027F888
	public static IEnumerator LoadEntityGroups(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (root == null || !root.HasElements)
		{
			throw new Exception("No root element found!");
		}
		int num = 0;
		using (IEnumerator<XElement> enumerator = root.Elements("entitygroup").GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XElement xelement = enumerator.Current;
				EntityGroupsFromXml.parseGroup(xelement, ref num);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x060065DC RID: 26076 RVA: 0x00281698 File Offset: 0x0027F898
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseGroup(XElement _elementGroup, ref int _count)
	{
		string attribute = _elementGroup.GetAttribute("name");
		if (attribute.Length == 0)
		{
			throw new Exception("Attribute 'name' missing on entitygroup element");
		}
		List<SEntityClassAndProb> list = new List<SEntityClassAndProb>();
		EntityGroups.list[attribute] = list;
		float num = 0f;
		foreach (XNode xnode in _elementGroup.Nodes())
		{
			if (xnode.NodeType == XmlNodeType.Text)
			{
				EntityGroupsFromXml.parseTextBasedList(((XText)xnode).Value, list, ref num, ref _count);
			}
			if (xnode.NodeType == XmlNodeType.Element)
			{
				XElement xelement = xnode as XElement;
				if (xelement != null && (xelement.Name == "entity" || xelement.Name == "e"))
				{
					EntityGroupsFromXml.parseElementBased(xelement, attribute, list, ref num, ref _count);
				}
			}
		}
		if (num > 0f)
		{
			EntityGroups.Normalize(attribute, num);
		}
		if (list.Count == 0)
		{
			throw new Exception("Empty entity groups not allowed! Group name: " + attribute);
		}
	}

	// Token: 0x060065DD RID: 26077 RVA: 0x002817B8 File Offset: 0x0027F9B8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseTextBasedList(string _nodeText, List<SEntityClassAndProb> _targetList, ref float _totalProb, ref int _count)
	{
		int num = 0;
		int i = _nodeText.IndexOf('\n', num);
		if (i < 0)
		{
			i = _nodeText.Length;
		}
		while (i >= 0)
		{
			string text = _nodeText.Substring(num, i - num);
			num = i + 1;
			if (num >= _nodeText.Length)
			{
				i = -1;
			}
			else
			{
				i = _nodeText.IndexOf('\n', num);
				if (i < 0)
				{
					i = _nodeText.Length;
				}
			}
			string text2 = text;
			float num2 = 1f;
			int num3 = text.IndexOf(',');
			if (num3 >= 0)
			{
				text2 = text.Substring(0, num3);
				num2 = StringParsers.ParseFloat(text, num3 + 1, -1, NumberStyles.Any);
			}
			text2 = text2.Trim();
			if (text2.Length > 0)
			{
				EntityGroupsFromXml.addEntity(text2, num2, _targetList, ref _totalProb, ref _count);
			}
		}
	}

	// Token: 0x060065DE RID: 26078 RVA: 0x0028186C File Offset: 0x0027FA6C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseElementBased(XElement _entityElement, string _groupName, List<SEntityClassAndProb> _targetList, ref float _totalProb, ref int _count)
	{
		string text;
		if (!_entityElement.TryGetAttribute("name", out text) && !_entityElement.TryGetAttribute("n", out text))
		{
			throw new Exception("Attribute 'name' missing on entity in group '" + _groupName + "'");
		}
		text = text.Trim();
		if (text.Length == 0)
		{
			throw new Exception("Attribute 'name' empty on entity in group '" + _groupName + "'");
		}
		float num = 0f;
		if (!_entityElement.ParseAttribute("prob", ref num) && !_entityElement.ParseAttribute("p", ref num))
		{
			num = 1f;
		}
		EntityGroupsFromXml.addEntity(text, num, _targetList, ref _totalProb, ref _count);
	}

	// Token: 0x060065DF RID: 26079 RVA: 0x0028191C File Offset: 0x0027FB1C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void addEntity(string _entityName, float _prob, List<SEntityClassAndProb> _targetList, ref float _totalProb, ref int _count)
	{
		int num = 0;
		if (_entityName != "none")
		{
			num = EntityClass.FromString(_entityName);
			if (!EntityClass.list.ContainsKey(num))
			{
				throw new Exception("Entity with name '" + _entityName + "' not found");
			}
		}
		SEntityClassAndProb sentityClassAndProb = new SEntityClassAndProb
		{
			entityClassId = num,
			prob = _prob
		};
		_targetList.Add(sentityClassAndProb);
		_count++;
		_totalProb += _prob;
	}
}
