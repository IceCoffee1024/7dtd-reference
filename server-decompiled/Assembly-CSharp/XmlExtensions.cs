using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x020014DF RID: 5343
public static class XmlExtensions
{
	// Token: 0x0600A802 RID: 43010 RVA: 0x003E7F9C File Offset: 0x003E619C
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

	// Token: 0x0600A803 RID: 43011 RVA: 0x003E8048 File Offset: 0x003E6248
	public static string GetXPath(this XElement _elem)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlExtensions.getXPath(stringBuilder, _elem);
		return stringBuilder.ToString();
	}

	// Token: 0x0600A804 RID: 43012 RVA: 0x003E805B File Offset: 0x003E625B
	public static string GetXPath(this XAttribute _attr)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlExtensions.getXPath(stringBuilder, _attr.Parent);
		stringBuilder.Append("[@");
		stringBuilder.Append(_attr.Name);
		stringBuilder.Append(']');
		return stringBuilder.ToString();
	}

	// Token: 0x0600A805 RID: 43013 RVA: 0x003E8095 File Offset: 0x003E6295
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

	// Token: 0x0600A806 RID: 43014 RVA: 0x003E80C1 File Offset: 0x003E62C1
	public static bool HasAttribute(this XElement _element, XName _name)
	{
		return _element.Attribute(_name) != null;
	}

	// Token: 0x0600A807 RID: 43015 RVA: 0x003E80D0 File Offset: 0x003E62D0
	public static string GetAttribute(this XElement _element, XName _name)
	{
		XAttribute xattribute = _element.Attribute(_name);
		if (xattribute == null)
		{
			return "";
		}
		return xattribute.Value;
	}

	// Token: 0x0600A808 RID: 43016 RVA: 0x003E80F4 File Offset: 0x003E62F4
	public static bool TryGetAttribute(this XElement _element, XName _name, out string _result)
	{
		XAttribute xattribute = _element.Attribute(_name);
		_result = ((xattribute != null) ? xattribute.Value : null);
		return xattribute != null;
	}

	// Token: 0x0600A809 RID: 43017 RVA: 0x003E811C File Offset: 0x003E631C
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

	// Token: 0x0600A80A RID: 43018 RVA: 0x003E8140 File Offset: 0x003E6340
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

	// Token: 0x0600A80B RID: 43019 RVA: 0x003E8164 File Offset: 0x003E6364
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

	// Token: 0x0600A80C RID: 43020 RVA: 0x003E8184 File Offset: 0x003E6384
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

	// Token: 0x0600A80D RID: 43021 RVA: 0x003E81B0 File Offset: 0x003E63B0
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

	// Token: 0x0600A80E RID: 43022 RVA: 0x003E81D8 File Offset: 0x003E63D8
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

	// Token: 0x0600A80F RID: 43023 RVA: 0x003E8204 File Offset: 0x003E6404
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

	// Token: 0x0600A810 RID: 43024 RVA: 0x003E8228 File Offset: 0x003E6428
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

	// Token: 0x0600A811 RID: 43025 RVA: 0x003E824C File Offset: 0x003E644C
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

	// Token: 0x0600A812 RID: 43026 RVA: 0x003E82B0 File Offset: 0x003E64B0
	public static void CreateXmlDeclaration(this XmlDocument _doc)
	{
		XmlDeclaration xmlDeclaration = _doc.CreateXmlDeclaration("1.0", "UTF-8", null);
		_doc.InsertBefore(xmlDeclaration, _doc.DocumentElement);
	}

	// Token: 0x0600A813 RID: 43027 RVA: 0x003E82E0 File Offset: 0x003E64E0
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

	// Token: 0x0600A814 RID: 43028 RVA: 0x003E8318 File Offset: 0x003E6518
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

	// Token: 0x0600A815 RID: 43029 RVA: 0x003E8350 File Offset: 0x003E6550
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

	// Token: 0x0600A816 RID: 43030 RVA: 0x003E8388 File Offset: 0x003E6588
	public static XmlElement SetAttrib(this XmlElement _element, string _name, string _value)
	{
		_element.SetAttribute(_name, _value);
		return _element;
	}

	// Token: 0x0600A817 RID: 43031 RVA: 0x003E8393 File Offset: 0x003E6593
	public static XmlElement AddXmlKeyValueProperty(this XmlNode _node, string _name, string _value)
	{
		return _node.AddXmlElement("property").SetAttrib("name", _name).SetAttrib("value", _value);
	}

	// Token: 0x0600A818 RID: 43032 RVA: 0x003E83B6 File Offset: 0x003E65B6
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
