using System;
using System.Xml;
using System.Xml.Linq;

// Token: 0x020014F9 RID: 5369
public class XmlLoadException : Exception
{
	// Token: 0x0600A8FA RID: 43258 RVA: 0x003ED274 File Offset: 0x003EB474
	public XmlLoadException(string _xmlName, XElement _element, string _message)
		: this(XmlLoadException.buildMessage(_element, _xmlName, _message))
	{
	}

	// Token: 0x0600A8FB RID: 43259 RVA: 0x003ED284 File Offset: 0x003EB484
	public XmlLoadException(string _xmlName, XElement _element, string _message, Exception _innerException)
		: this(XmlLoadException.buildMessage(_element, _xmlName, _message), _innerException)
	{
	}

	// Token: 0x0600A8FC RID: 43260 RVA: 0x003ED298 File Offset: 0x003EB498
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

	// Token: 0x0600A8FD RID: 43261 RVA: 0x003C1227 File Offset: 0x003BF427
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlLoadException(string _message)
		: base(_message)
	{
	}

	// Token: 0x0600A8FE RID: 43262 RVA: 0x003C1230 File Offset: 0x003BF430
	public XmlLoadException(string _message, Exception _innerException)
		: base(_message, _innerException)
	{
	}
}
