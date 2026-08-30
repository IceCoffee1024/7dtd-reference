using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UAI;
using UnityEngine.Scripting;

// Token: 0x02000C9B RID: 3227
[Preserve]
public class UAIFromXml
{
	// Token: 0x06006304 RID: 25348 RVA: 0x0026E28D File Offset: 0x0026C48D
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

	// Token: 0x06006305 RID: 25349 RVA: 0x0026E29C File Offset: 0x0026C49C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void parseAIPackagesNode(XElement _element)
	{
		foreach (XElement xelement in _element.Elements("ai_package"))
		{
			UAIFromXml.parseAIPackageNode(xelement);
		}
	}

	// Token: 0x06006306 RID: 25350 RVA: 0x0026E2F0 File Offset: 0x0026C4F0
	public static void Cleanup()
	{
		UAIBase.Cleanup();
	}

	// Token: 0x06006307 RID: 25351 RVA: 0x0026E2F8 File Offset: 0x0026C4F8
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

	// Token: 0x06006308 RID: 25352 RVA: 0x0026E3D0 File Offset: 0x0026C5D0
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

	// Token: 0x06006309 RID: 25353 RVA: 0x0026E4C4 File Offset: 0x0026C6C4
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

	// Token: 0x0600630A RID: 25354 RVA: 0x0026E590 File Offset: 0x0026C790
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
