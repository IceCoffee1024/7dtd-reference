using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace XMLEditing
{
	// Token: 0x02001652 RID: 5714
	public static class XMLUtils
	{
		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x0600B343 RID: 45891 RVA: 0x004306CB File Offset: 0x0042E8CB
		public static string DefaultBlocksFilePath
		{
			get
			{
				return GameIO.GetGameDir("Data/Config") + "/blocks.xml";
			}
		}

		// Token: 0x0600B344 RID: 45892 RVA: 0x004306E4 File Offset: 0x0042E8E4
		public static XDocument LoadXDocument(string filePath)
		{
			XDocument xdocument;
			using (Stream stream = SdFile.OpenRead(filePath))
			{
				xdocument = XDocument.Load(stream, LoadOptions.PreserveWhitespace);
			}
			return xdocument;
		}

		// Token: 0x0600B345 RID: 45893 RVA: 0x00430720 File Offset: 0x0042E920
		public static XElement SetArray(XElement _element, string _arrayName)
		{
			XElement xelement = (from e in _element.Elements(XNames.array)
				where e.GetAttribute(XNames.name) == _arrayName
				select e).FirstOrDefault<XElement>();
			if (xelement == null)
			{
				xelement = new XElement(XNames.array, new XAttribute(XNames.name, _arrayName));
				_element.Add("\t");
				_element.Add(xelement);
				_element.Add("\r\n");
				_element.Add("\t");
			}
			return xelement;
		}

		// Token: 0x0600B346 RID: 45894 RVA: 0x004307A4 File Offset: 0x0042E9A4
		public static XElement SetProperty(XElement _element, string _propertyName, XName _attribName, string _value)
		{
			XElement xelement = (from e in _element.Elements(XNames.property)
				where e.GetAttribute(XNames.name) == _propertyName
				select e).FirstOrDefault<XElement>();
			if (xelement == null)
			{
				xelement = new XElement(XNames.property, new XAttribute(XNames.name, _propertyName));
				_element.Add("\t");
				_element.Add(xelement);
				_element.Add("\r\n");
				_element.Add("\t");
			}
			xelement.SetAttributeValue(_attribName, _value);
			return xelement;
		}

		// Token: 0x0600B347 RID: 45895 RVA: 0x00430830 File Offset: 0x0042EA30
		public static bool IsChildOf(this XElement element, XElement parent)
		{
			for (XElement xelement = element; xelement != null; xelement = xelement.Parent)
			{
				if (xelement == parent)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B348 RID: 45896 RVA: 0x00430854 File Offset: 0x0042EA54
		public static void SaveXDocument(XDocument doc, string filePath, bool omitXmlDeclaration = false)
		{
			using (Stream stream = SdFile.Create(filePath))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
				{
					Encoding = Encoding.UTF8,
					Indent = true,
					IndentChars = "\t",
					NewLineChars = "\r\n",
					NewLineHandling = NewLineHandling.Replace,
					OmitXmlDeclaration = omitXmlDeclaration
				}))
				{
					doc.WriteTo(xmlWriter);
				}
			}
		}

		// Token: 0x0600B349 RID: 45897 RVA: 0x004308E8 File Offset: 0x0042EAE8
		public static void CleanAndRepairBlocksXML()
		{
			string text = SdFile.ReadAllText(XMLUtils.DefaultBlocksFilePath);
			string text2 = "<block [^>]*>[\\s\\S]*?</block>";
			text = Regex.Replace(text, text2, (Match m) => Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(m.Value, "\r\n\\s*\r\n", "\r\n"), "/>\r\n\t(\t)?(<property name=\"(ModelOffset|OversizedBounds)\")", "/>\r\n\t$2"), "\r\n\t</block>", "\r\n</block>"), "\r\n\t <!--[\\s\\S]*?-->\r\n", "\r\n"), "(name=\"MeshDamage\" value=\")([^\"]+)\"", delegate(Match match)
			{
				string text3 = Regex.Replace(match.Groups[2].Value, " {2,3}", "\r\n\t\t");
				return match.Groups[1].Value + text3 + "\"";
			}), " />", "/>"), RegexOptions.Singleline);
			SdFile.WriteAllText(XMLUtils.DefaultBlocksFilePath, text);
		}

		// Token: 0x0600B34A RID: 45898 RVA: 0x0043093C File Offset: 0x0042EB3C
		public static HashSet<string> ParseStringList(string targetListString, char separator)
		{
			string[] array = targetListString.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
			HashSet<string> hashSet = new HashSet<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i].Trim();
				hashSet.Add(text);
			}
			return hashSet;
		}

		// Token: 0x0600B34B RID: 45899 RVA: 0x00430984 File Offset: 0x0042EB84
		public static HashSet<string> GetReplacementBlockNames(HashSet<string> targetNames)
		{
			XElement root = XMLUtils.LoadXDocument(GameIO.GetGameDir("Data/Config") + "/blockplaceholders.xml").Root;
			if (root == null || !root.HasElements)
			{
				throw new Exception("No element <blockplaceholders> found!");
			}
			Dictionary<string, XElement> dictionary = new CaseInsensitiveStringDictionary<XElement>();
			foreach (XElement xelement in root.Elements("placeholder"))
			{
				string attribute = xelement.GetAttribute(XNames.name);
				dictionary[attribute] = xelement;
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string text in targetNames)
			{
				string text2 = text.Trim();
				XElement xelement2;
				if (dictionary.TryGetValue(text2, out xelement2))
				{
					foreach (XElement xelement3 in xelement2.Elements(XNames.block))
					{
						hashSet.Add(xelement3.GetAttribute(XNames.name).Trim());
					}
				}
			}
			return hashSet;
		}

		// Token: 0x0600B34C RID: 45900 RVA: 0x00430ACC File Offset: 0x0042ECCC
		public static bool AllAttributesAreEqual(XElement elementA, XElement elementB, StringComparison comparisonType)
		{
			if (elementB.Attributes().Count<XAttribute>() != elementA.Attributes().Count<XAttribute>())
			{
				return false;
			}
			foreach (XAttribute xattribute in elementA.Attributes())
			{
				XAttribute xattribute2 = elementB.Attribute(xattribute.Name);
				if (xattribute2 == null)
				{
					return false;
				}
				string text = xattribute.Value.Trim();
				string text2 = xattribute2.Value.Trim();
				if (!string.Equals(text, text2, comparisonType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B34D RID: 45901 RVA: 0x00430B6C File Offset: 0x0042ED6C
		public static void PopulateReplacementMap(Dictionary<string, HashSet<string>> replacementMap)
		{
			replacementMap.Clear();
			XElement root = XMLUtils.LoadXDocument(GameIO.GetGameDir("Data/Config") + "/blockplaceholders.xml").Root;
			if (root == null || !root.HasElements)
			{
				throw new Exception("No element <blockplaceholders> found!");
			}
			foreach (XElement xelement in root.Elements("placeholder"))
			{
				string text = xelement.GetAttribute(XNames.name).Trim();
				HashSet<string> hashSet = new HashSet<string>();
				replacementMap[text] = hashSet;
				foreach (XElement xelement2 in xelement.Elements(XNames.block))
				{
					hashSet.Add(xelement2.GetAttribute(XNames.name).Trim());
				}
			}
		}

		// Token: 0x04008641 RID: 34369
		public const string IndentChars = "\t";

		// Token: 0x04008642 RID: 34370
		public const string NewLineChars = "\r\n";
	}
}
