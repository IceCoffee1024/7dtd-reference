using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x020014FA RID: 5370
public static class XmlExtensions
{
	// Token: 0x0600A8FF RID: 43263 RVA: 0x003ED2DC File Offset: 0x003EB4DC
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

	// Token: 0x0600A900 RID: 43264 RVA: 0x003ED388 File Offset: 0x003EB588
	public static string GetXPath(this XElement _elem)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlExtensions.getXPath(stringBuilder, _elem);
		return stringBuilder.ToString();
	}

	// Token: 0x0600A901 RID: 43265 RVA: 0x003ED39B File Offset: 0x003EB59B
	public static string GetXPath(this XAttribute _attr)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlExtensions.getXPath(stringBuilder, _attr.Parent);
		stringBuilder.Append("[@");
		stringBuilder.Append(_attr.Name);
		stringBuilder.Append(']');
		return stringBuilder.ToString();
	}

	// Token: 0x0600A902 RID: 43266 RVA: 0x003ED3D5 File Offset: 0x003EB5D5
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

	// Token: 0x0600A903 RID: 43267 RVA: 0x003ED401 File Offset: 0x003EB601
	public static bool HasAttribute(this XElement _element, XName _name)
	{
		return _element.Attribute(_name) != null;
	}

	// Token: 0x0600A904 RID: 43268 RVA: 0x003ED410 File Offset: 0x003EB610
	public static string GetAttribute(this XElement _element, XName _name)
	{
		XAttribute xattribute = _element.Attribute(_name);
		if (xattribute == null)
		{
			return "";
		}
		return xattribute.Value;
	}

	// Token: 0x0600A905 RID: 43269 RVA: 0x003ED434 File Offset: 0x003EB634
	public static bool TryGetAttribute(this XElement _element, XName _name, out string _result)
	{
		XAttribute xattribute = _element.Attribute(_name);
		_result = ((xattribute != null) ? xattribute.Value : null);
		return xattribute != null;
	}

	// Token: 0x0600A906 RID: 43270 RVA: 0x003ED45C File Offset: 0x003EB65C
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

	// Token: 0x0600A907 RID: 43271 RVA: 0x003ED480 File Offset: 0x003EB680
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

	// Token: 0x0600A908 RID: 43272 RVA: 0x003ED4A4 File Offset: 0x003EB6A4
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

	// Token: 0x0600A909 RID: 43273 RVA: 0x003ED4C4 File Offset: 0x003EB6C4
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

	// Token: 0x0600A90A RID: 43274 RVA: 0x003ED4F0 File Offset: 0x003EB6F0
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

	// Token: 0x0600A90B RID: 43275 RVA: 0x003ED518 File Offset: 0x003EB718
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

	// Token: 0x0600A90C RID: 43276 RVA: 0x003ED544 File Offset: 0x003EB744
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

	// Token: 0x0600A90D RID: 43277 RVA: 0x003ED568 File Offset: 0x003EB768
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

	// Token: 0x0600A90E RID: 43278 RVA: 0x003ED58C File Offset: 0x003EB78C
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

	// Token: 0x0600A90F RID: 43279 RVA: 0x003ED5F0 File Offset: 0x003EB7F0
	public static void CreateXmlDeclaration(this XmlDocument _doc)
	{
		XmlDeclaration xmlDeclaration = _doc.CreateXmlDeclaration("1.0", "UTF-8", null);
		_doc.InsertBefore(xmlDeclaration, _doc.DocumentElement);
	}

	// Token: 0x0600A910 RID: 43280 RVA: 0x003ED620 File Offset: 0x003EB820
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

	// Token: 0x0600A911 RID: 43281 RVA: 0x003ED658 File Offset: 0x003EB858
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

	// Token: 0x0600A912 RID: 43282 RVA: 0x003ED690 File Offset: 0x003EB890
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

	// Token: 0x0600A913 RID: 43283 RVA: 0x003ED6C8 File Offset: 0x003EB8C8
	public static XmlElement SetAttrib(this XmlElement _element, string _name, string _value)
	{
		_element.SetAttribute(_name, _value);
		return _element;
	}

	// Token: 0x0600A914 RID: 43284 RVA: 0x003ED6D3 File Offset: 0x003EB8D3
	public static XmlElement AddXmlKeyValueProperty(this XmlNode _node, string _name, string _value)
	{
		return _node.AddXmlElement("property").SetAttrib("name", _name).SetAttrib("value", _value);
	}

	// Token: 0x0600A915 RID: 43285 RVA: 0x003ED6F6 File Offset: 0x003EB8F6
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
