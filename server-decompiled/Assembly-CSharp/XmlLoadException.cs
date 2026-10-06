using System;
using System.Xml;
using System.Xml.Linq;

// Token: 0x0200151B RID: 5403
public class XmlLoadException : Exception
{
	// Token: 0x0600A9AD RID: 43437 RVA: 0x003F20C8 File Offset: 0x003F02C8
	public XmlLoadException(string _xmlName, XElement _element, string _message)
		: this(XmlLoadException.buildMessage(_element, _xmlName, _message))
	{
	}

	// Token: 0x0600A9AE RID: 43438 RVA: 0x003F20D8 File Offset: 0x003F02D8
	public XmlLoadException(string _xmlName, XElement _element, string _message, Exception _innerException)
		: this(XmlLoadException.buildMessage(_element, _xmlName, _message), _innerException)
	{
	}

	// Token: 0x0600A9AF RID: 43439 RVA: 0x003F20EC File Offset: 0x003F02EC
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

	// Token: 0x0600A9B0 RID: 43440 RVA: 0x003C5DC3 File Offset: 0x003C3FC3
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlLoadException(string _message)
		: base(_message)
	{
	}

	// Token: 0x0600A9B1 RID: 43441 RVA: 0x003C5DCC File Offset: 0x003C3FCC
	public XmlLoadException(string _message, Exception _innerException)
		: base(_message, _innerException)
	{
	}
}
