using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UAI;
using UnityEngine.Scripting;

// Token: 0x02000CB4 RID: 3252
[Preserve]
public class UAIFromXml
{
	// Token: 0x0600631C RID: 25372 RVA: 0x00272BF9 File Offset: 0x00270DF9
	public static IEnumerator Load(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <ai_packages> found!");
		}
		using (IEnumerator<XElement> enumerator = root.Elements("ai_packages").GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XElement xelement = enumerator.Current;
				UAIFromXml.parseAIPackagesNode(xelement);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x0600631D RID: 25373 RVA: 0x00272C08 File Offset: 0x00270E08
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseAIPackagesNode(XElement _element)
	{
		foreach (XElement xelement in _element.Elements("ai_package"))
		{
			UAIFromXml.parseAIPackageNode(xelement);
		}
	}

	// Token: 0x0600631E RID: 25374 RVA: 0x00272C5C File Offset: 0x00270E5C
	public static void Cleanup()
	{
		UAIBase.Cleanup();
	}

	// Token: 0x0600631F RID: 25375 RVA: 0x00272C64 File Offset: 0x00270E64
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseAIPackageNode(XElement _element)
	{
		string text = "";
		float num = 1f;
		if (_element.HasAttribute("name"))
		{
			text = _element.GetAttribute("name");
		}
		if (_element.HasAttribute("weight"))
		{
			num = StringParsers.ParseFloat(_element.GetAttribute("weight"), 0, -1, NumberStyles.Any);
		}
		UAIPackage uaipackage = new UAIPackage(text, num);
		foreach (XElement xelement in _element.Elements("action"))
		{
			UAIFromXml.parseActionNode(uaipackage, xelement);
		}
		if (!UAIBase.AIPackages.ContainsKey(text))
		{
			UAIBase.AIPackages.Add(text, uaipackage);
		}
	}

	// Token: 0x06006320 RID: 25376 RVA: 0x00272D3C File Offset: 0x00270F3C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseActionNode(UAIPackage _package, XElement _element)
	{
		string text = "";
		float num = 1f;
		if (_element.HasAttribute("name"))
		{
			text = _element.GetAttribute("name");
		}
		if (_element.HasAttribute("weight"))
		{
			num = StringParsers.ParseFloat(_element.GetAttribute("weight"), 0, -1, NumberStyles.Any);
		}
		UAIAction uaiaction = new UAIAction(text, num);
		foreach (XElement xelement in _element.Elements())
		{
			if (xelement.Name == "task")
			{
				UAIFromXml.parseTaskNode(uaiaction, xelement);
			}
			if (xelement.Name == "consideration")
			{
				UAIFromXml.parseConsiderationNode(uaiaction, xelement);
			}
		}
		_package.AddAction(uaiaction);
	}

	// Token: 0x06006321 RID: 25377 RVA: 0x00272E30 File Offset: 0x00271030
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseTaskNode(UAIAction _action, XElement _element)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (XAttribute xattribute in _element.Attributes())
		{
			dictionary.Add(xattribute.Name.LocalName, xattribute.Value);
		}
		if (_element.HasAttribute("class"))
		{
			string attribute = _element.GetAttribute("class");
			Type typeWithPrefix = ReflectionHelpers.GetTypeWithPrefix("UAI.UAITask", attribute);
			if (typeWithPrefix != null)
			{
				UAITaskBase uaitaskBase = (UAITaskBase)Activator.CreateInstance(typeWithPrefix);
				uaitaskBase.Name = attribute;
				uaitaskBase.Parameters = dictionary;
				_action.AddTask(uaitaskBase);
			}
		}
	}

	// Token: 0x06006322 RID: 25378 RVA: 0x00272EFC File Offset: 0x002710FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseConsiderationNode(UAIAction _action, XElement _element)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (XAttribute xattribute in _element.Attributes())
		{
			dictionary.Add(xattribute.Name.LocalName, xattribute.Value);
		}
		if (_element.HasAttribute("class"))
		{
			string attribute = _element.GetAttribute("class");
			Type typeWithPrefix = ReflectionHelpers.GetTypeWithPrefix("UAI.UAIConsideration", attribute);
			if (typeWithPrefix != null)
			{
				UAIConsiderationBase uaiconsiderationBase = (UAIConsiderationBase)Activator.CreateInstance(typeWithPrefix);
				uaiconsiderationBase.Name = attribute;
				uaiconsiderationBase.Init(dictionary);
				_action.AddConsideration(uaiconsiderationBase);
			}
		}
	}
}
