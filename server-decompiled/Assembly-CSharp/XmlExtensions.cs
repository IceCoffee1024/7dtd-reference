using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x0200152E RID: 5422
public static class XmlExtensions
{
	// Token: 0x0600AA0A RID: 43530 RVA: 0x003F4534 File Offset: 0x003F2734
	public static string GetElementString(this XElement _elem)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('<');
		stringBuilder.Append(_elem.Name);
		foreach (XAttribute xattribute in _elem.Attributes())
		{
			stringBuilder.Append(' ');
			stringBuilder.Append(xattribute.Name);
			stringBuilder.Append("=\"");
			stringBuilder.Append(xattribute.Value);
			stringBuilder.Append('"');
		}
		stringBuilder.Append(' ');
		return stringBuilder.ToString();
	}

	// Token: 0x0600AA0B RID: 43531 RVA: 0x003F45E0 File Offset: 0x003F27E0
	public static string GetXPath(this XElement _elem)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlExtensions.getXPath(stringBuilder, _elem);
		return stringBuilder.ToString();
	}

	// Token: 0x0600AA0C RID: 43532 RVA: 0x003F45F3 File Offset: 0x003F27F3
	public static string GetXPath(this XAttribute _attr)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlExtensions.getXPath(stringBuilder, _attr.Parent);
		stringBuilder.Append("[@");
		stringBuilder.Append(_attr.Name);
		stringBuilder.Append(']');
		return stringBuilder.ToString();
	}

	// Token: 0x0600AA0D RID: 43533 RVA: 0x003F462D File Offset: 0x003F282D
	[PublicizedFrom(EAccessModifier.Private)]
	public static void getXPath(StringBuilder _sb, XElement _current)
	{
		if (_current.Parent != null)
		{
			XmlExtensions.getXPath(_sb, _current.Parent);
		}
		_sb.Append('/');
		_sb.Append(_current.Name);
	}

	// Token: 0x0600AA0E RID: 43534 RVA: 0x003F4659 File Offset: 0x003F2859
	public static bool HasAttribute(this XElement _element, XName _name)
	{
		return _element.Attribute(_name) != null;
	}

	// Token: 0x0600AA0F RID: 43535 RVA: 0x003F4668 File Offset: 0x003F2868
	public static string GetAttribute(this XElement _element, XName _name)
	{
		XAttribute xattribute = _element.Attribute(_name);
		if (xattribute == null)
		{
			return "";
		}
		return xattribute.Value;
	}

	// Token: 0x0600AA10 RID: 43536 RVA: 0x003F468C File Offset: 0x003F288C
	public static bool TryGetAttribute(this XElement _element, XName _name, out string _result)
	{
		XAttribute xattribute = _element.Attribute(_name);
		_result = ((xattribute != null) ? xattribute.Value : null);
		return xattribute != null;
	}

	// Token: 0x0600AA11 RID: 43537 RVA: 0x003F46B4 File Offset: 0x003F28B4
	public static bool ParseAttribute(this XElement _element, XName _name, ref int _result)
	{
		string text;
		if (_element.TryGetAttribute(_name, out text))
		{
			_result = int.Parse(text);
			return true;
		}
		return false;
	}

	// Token: 0x0600AA12 RID: 43538 RVA: 0x003F46D8 File Offset: 0x003F28D8
	public static bool ParseAttribute(this XElement _element, XName _name, ref short _result)
	{
		string text;
		if (_element.TryGetAttribute(_name, out text))
		{
			_result = short.Parse(text);
			return true;
		}
		return false;
	}

	// Token: 0x0600AA13 RID: 43539 RVA: 0x003F46FC File Offset: 0x003F28FC
	public static bool ParseAttribute(this XElement _element, XName _name, ref string _result)
	{
		string text;
		if (_element.TryGetAttribute(_name, out text))
		{
			_result = text;
			return true;
		}
		return false;
	}

	// Token: 0x0600AA14 RID: 43540 RVA: 0x003F471C File Offset: 0x003F291C
	public static bool ParseAttribute(this XElement _element, XName _name, ref float _result)
	{
		string text;
		if (_element.TryGetAttribute(_name, out text))
		{
			_result = StringParsers.ParseFloat(text, 0, -1, NumberStyles.Any);
			return true;
		}
		return false;
	}

	// Token: 0x0600AA15 RID: 43541 RVA: 0x003F4748 File Offset: 0x003F2948
	public static bool ParseAttribute(this XElement _element, XName _name, ref Vector2 _result)
	{
		string text;
		if (_element.TryGetAttribute(_name, out text))
		{
			_result = StringParsers.ParseVector2(text);
			return true;
		}
		return false;
	}

	// Token: 0x0600AA16 RID: 43542 RVA: 0x003F4770 File Offset: 0x003F2970
	public static bool ParseAttribute(this XElement _element, XName _name, ref Vector3 _result)
	{
		string text;
		if (_element.TryGetAttribute(_name, out text))
		{
			_result = StringParsers.ParseVector3(text, 0, -1);
			return true;
		}
		return false;
	}

	// Token: 0x0600AA17 RID: 43543 RVA: 0x003F479C File Offset: 0x003F299C
	public static bool ParseAttribute(this XElement _element, XName _name, ref ulong _result)
	{
		string text;
		if (_element.TryGetAttribute(_name, out text))
		{
			_result = ulong.Parse(text);
			return true;
		}
		return false;
	}

	// Token: 0x0600AA18 RID: 43544 RVA: 0x003F47C0 File Offset: 0x003F29C0
	public static bool ParseAttribute(this XElement _element, XName _name, ref bool _result)
	{
		string text;
		if (_element.TryGetAttribute(_name, out text))
		{
			_result = bool.Parse(text);
			return true;
		}
		return false;
	}

	// Token: 0x0600AA19 RID: 43545 RVA: 0x003F47E4 File Offset: 0x003F29E4
	public static List<XmlNode> ToList(this XmlNodeList _xmlNodeList)
	{
		List<XmlNode> list = new List<XmlNode>(_xmlNodeList.Count);
		foreach (object obj in _xmlNodeList)
		{
			XmlNode xmlNode = (XmlNode)obj;
			list.Add(xmlNode);
		}
		return list;
	}

	// Token: 0x0600AA1A RID: 43546 RVA: 0x003F4848 File Offset: 0x003F2A48
	public static void CreateXmlDeclaration(this XmlDocument _doc)
	{
		XmlDeclaration xmlDeclaration = _doc.CreateXmlDeclaration("1.0", "UTF-8", null);
		_doc.InsertBefore(xmlDeclaration, _doc.DocumentElement);
	}

	// Token: 0x0600AA1B RID: 43547 RVA: 0x003F4878 File Offset: 0x003F2A78
	public static XmlElement AddXmlElement(this XmlNode _node, string _name)
	{
		XmlDocument xmlDocument;
		if (_node.NodeType == XmlNodeType.Document)
		{
			xmlDocument = (XmlDocument)_node;
		}
		else
		{
			xmlDocument = _node.OwnerDocument;
		}
		XmlElement xmlElement = xmlDocument.CreateElement(_name);
		_node.AppendChild(xmlElement);
		return xmlElement;
	}

	// Token: 0x0600AA1C RID: 43548 RVA: 0x003F48B0 File Offset: 0x003F2AB0
	public static XmlComment AddXmlComment(this XmlNode _node, string _content)
	{
		XmlDocument xmlDocument;
		if (_node.NodeType == XmlNodeType.Document)
		{
			xmlDocument = (XmlDocument)_node;
		}
		else
		{
			xmlDocument = _node.OwnerDocument;
		}
		XmlComment xmlComment = xmlDocument.CreateComment(_content);
		_node.AppendChild(xmlComment);
		return xmlComment;
	}

	// Token: 0x0600AA1D RID: 43549 RVA: 0x003F48E8 File Offset: 0x003F2AE8
	public static XmlText AddXmlText(this XmlNode _node, string _text)
	{
		XmlDocument xmlDocument;
		if (_node.NodeType == XmlNodeType.Document)
		{
			xmlDocument = (XmlDocument)_node;
		}
		else
		{
			xmlDocument = _node.OwnerDocument;
		}
		XmlText xmlText = xmlDocument.CreateTextNode(_text);
		_node.AppendChild(xmlText);
		return xmlText;
	}

	// Token: 0x0600AA1E RID: 43550 RVA: 0x003F4920 File Offset: 0x003F2B20
	public static XmlElement SetAttrib(this XmlElement _element, string _name, string _value)
	{
		_element.SetAttribute(_name, _value);
		return _element;
	}

	// Token: 0x0600AA1F RID: 43551 RVA: 0x003F492B File Offset: 0x003F2B2B
	public static XmlElement AddXmlKeyValueProperty(this XmlNode _node, string _name, string _value)
	{
		return _node.AddXmlElement("property").SetAttrib("name", _name).SetAttrib("value", _value);
	}

	// Token: 0x0600AA20 RID: 43552 RVA: 0x003F494E File Offset: 0x003F2B4E
	public static bool TryGetAttribute(this XmlElement _element, string _name, out string _result)
	{
		if (!_element.HasAttribute(_name))
		{
			_result = null;
			return false;
		}
		_result = _element.GetAttribute(_name);
		return true;
	}
}
