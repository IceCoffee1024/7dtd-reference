using System;
using System.Xml;
using System.Xml.Linq;

// Token: 0x0200152D RID: 5421
public class XmlLoadException : Exception
{
	// Token: 0x0600AA05 RID: 43525 RVA: 0x003F44CC File Offset: 0x003F26CC
	public XmlLoadException(string _xmlName, XElement _element, string _message)
		: this(XmlLoadException.buildMessage(_element, _xmlName, _message))
	{
	}

	// Token: 0x0600AA06 RID: 43526 RVA: 0x003F44DC File Offset: 0x003F26DC
	public XmlLoadException(string _xmlName, XElement _element, string _message, Exception _innerException)
		: this(XmlLoadException.buildMessage(_element, _xmlName, _message), _innerException)
	{
	}

	// Token: 0x0600AA07 RID: 43527 RVA: 0x003F44F0 File Offset: 0x003F26F0
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

	// Token: 0x0600AA08 RID: 43528 RVA: 0x003C81C7 File Offset: 0x003C63C7
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlLoadException(string _message)
		: base(_message)
	{
	}

	// Token: 0x0600AA09 RID: 43529 RVA: 0x003C81D0 File Offset: 0x003C63D0
	public XmlLoadException(string _message, Exception _innerException)
		: base(_message, _innerException)
	{
	}
}
