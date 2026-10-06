using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x0200151C RID: 5404
public static class XmlExtensions
{
	// Token: 0x0600A9B2 RID: 43442 RVA: 0x003F2130 File Offset: 0x003F0330
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

	// Token: 0x0600A9B3 RID: 43443 RVA: 0x003F21DC File Offset: 0x003F03DC
	public static string GetXPath(this XElement _elem)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlExtensions.getXPath(stringBuilder, _elem);
		return stringBuilder.ToString();
	}

	// Token: 0x0600A9B4 RID: 43444 RVA: 0x003F21EF File Offset: 0x003F03EF
	public static string GetXPath(this XAttribute _attr)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlExtensions.getXPath(stringBuilder, _attr.Parent);
		stringBuilder.Append("[@");
		stringBuilder.Append(_attr.Name);
		stringBuilder.Append(']');
		return stringBuilder.ToString();
	}

	// Token: 0x0600A9B5 RID: 43445 RVA: 0x003F2229 File Offset: 0x003F0429
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

	// Token: 0x0600A9B6 RID: 43446 RVA: 0x003F2255 File Offset: 0x003F0455
	public static bool HasAttribute(this XElement _element, XName _name)
	{
		return _element.Attribute(_name) != null;
	}

	// Token: 0x0600A9B7 RID: 43447 RVA: 0x003F2264 File Offset: 0x003F0464
	public static string GetAttribute(this XElement _element, XName _name)
	{
		XAttribute xattribute = _element.Attribute(_name);
		if (xattribute == null)
		{
			return "";
		}
		return xattribute.Value;
	}

	// Token: 0x0600A9B8 RID: 43448 RVA: 0x003F2288 File Offset: 0x003F0488
	public static bool TryGetAttribute(this XElement _element, XName _name, out string _result)
	{
		XAttribute xattribute = _element.Attribute(_name);
		_result = ((xattribute != null) ? xattribute.Value : null);
		return xattribute != null;
	}

	// Token: 0x0600A9B9 RID: 43449 RVA: 0x003F22B0 File Offset: 0x003F04B0
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

	// Token: 0x0600A9BA RID: 43450 RVA: 0x003F22D4 File Offset: 0x003F04D4
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

	// Token: 0x0600A9BB RID: 43451 RVA: 0x003F22F8 File Offset: 0x003F04F8
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

	// Token: 0x0600A9BC RID: 43452 RVA: 0x003F2318 File Offset: 0x003F0518
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

	// Token: 0x0600A9BD RID: 43453 RVA: 0x003F2344 File Offset: 0x003F0544
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

	// Token: 0x0600A9BE RID: 43454 RVA: 0x003F236C File Offset: 0x003F056C
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

	// Token: 0x0600A9BF RID: 43455 RVA: 0x003F2398 File Offset: 0x003F0598
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

	// Token: 0x0600A9C0 RID: 43456 RVA: 0x003F23BC File Offset: 0x003F05BC
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

	// Token: 0x0600A9C1 RID: 43457 RVA: 0x003F23E0 File Offset: 0x003F05E0
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

	// Token: 0x0600A9C2 RID: 43458 RVA: 0x003F2444 File Offset: 0x003F0644
	public static void CreateXmlDeclaration(this XmlDocument _doc)
	{
		XmlDeclaration xmlDeclaration = _doc.CreateXmlDeclaration("1.0", "UTF-8", null);
		_doc.InsertBefore(xmlDeclaration, _doc.DocumentElement);
	}

	// Token: 0x0600A9C3 RID: 43459 RVA: 0x003F2474 File Offset: 0x003F0674
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

	// Token: 0x0600A9C4 RID: 43460 RVA: 0x003F24AC File Offset: 0x003F06AC
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

	// Token: 0x0600A9C5 RID: 43461 RVA: 0x003F24E4 File Offset: 0x003F06E4
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

	// Token: 0x0600A9C6 RID: 43462 RVA: 0x003F251C File Offset: 0x003F071C
	public static XmlElement SetAttrib(this XmlElement _element, string _name, string _value)
	{
		_element.SetAttribute(_name, _value);
		return _element;
	}

	// Token: 0x0600A9C7 RID: 43463 RVA: 0x003F2527 File Offset: 0x003F0727
	public static XmlElement AddXmlKeyValueProperty(this XmlNode _node, string _name, string _value)
	{
		return _node.AddXmlElement("property").SetAttrib("name", _name).SetAttrib("value", _value);
	}

	// Token: 0x0600A9C8 RID: 43464 RVA: 0x003F254A File Offset: 0x003F074A
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
