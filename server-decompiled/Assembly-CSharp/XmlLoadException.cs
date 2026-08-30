using System;
using System.Xml;
using System.Xml.Linq;

// Token: 0x020014DE RID: 5342
public class XmlLoadException : Exception
{
	// Token: 0x0600A7FD RID: 43005 RVA: 0x003E7F34 File Offset: 0x003E6134
	public XmlLoadException(string _xmlName, XElement _element, string _message)
		: this(XmlLoadException.buildMessage(_element, _xmlName, _message))
	{
	}

	// Token: 0x0600A7FE RID: 43006 RVA: 0x003E7F44 File Offset: 0x003E6144
	public XmlLoadException(string _xmlName, XElement _element, string _message, Exception _innerException)
		: this(XmlLoadException.buildMessage(_element, _xmlName, _message), _innerException)
	{
	}

	// Token: 0x0600A7FF RID: 43007 RVA: 0x003E7F58 File Offset: 0x003E6158
	[PublicizedFrom(EAccessModifier.Private)]
	public static string buildMessage(XElement _element, string _xmlName, string _message)
	{
		return string.Format("Error loading {0}: {1} (line {2} at pos {3})", new object[]
		{
			_xmlName,
			_message,
			((IXmlLineInfo)_element).LineNumber,
			((IXmlLineInfo)_element).LinePosition
		});
	}

	// Token: 0x0600A800 RID: 43008 RVA: 0x003BBEE3 File Offset: 0x003BA0E3
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlLoadException(string _message)
		: base(_message)
	{
	}

	// Token: 0x0600A801 RID: 43009 RVA: 0x003BBEEC File Offset: 0x003BA0EC
	public XmlLoadException(string _message, Exception _innerException)
		: base(_message, _innerException)
	{
	}
}
