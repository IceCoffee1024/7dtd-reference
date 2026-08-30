using System;
using System.Xml;
using System.Xml.Linq;

// Token: 0x020014DD RID: 5341
public class XmlPatchException : Exception
{
	// Token: 0x0600A7F8 RID: 43000 RVA: 0x003E7EC4 File Offset: 0x003E60C4
	public XmlPatchException(XElement _patchElement, string _patchMethodName, string _message)
		: this(XmlPatchException.buildMessage(_patchElement, _patchMethodName, _message))
	{
	}

	// Token: 0x0600A7F9 RID: 43001 RVA: 0x003E7ED4 File Offset: 0x003E60D4
	public XmlPatchException(XElement _patchElement, string _patchMethodName, string _message, Exception _innerException)
		: this(XmlPatchException.buildMessage(_patchElement, _patchMethodName, _message), _innerException)
	{
	}

	// Token: 0x0600A7FA RID: 43002 RVA: 0x003E7EE8 File Offset: 0x003E60E8
	[PublicizedFrom(EAccessModifier.Private)]
	public static string buildMessage(XElement _patchElement, string _patchMethodName, string _message)
	{
		return string.Format("XML.{0} ({1}, line {2} at pos {3}): {4}", new object[]
		{
			_patchMethodName,
			_patchElement.GetXPath(),
			((IXmlLineInfo)_patchElement).LineNumber,
			((IXmlLineInfo)_patchElement).LinePosition,
			_message
		});
	}

	// Token: 0x0600A7FB RID: 43003 RVA: 0x003BBEE3 File Offset: 0x003BA0E3
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlPatchException(string _message)
		: base(_message)
	{
	}

	// Token: 0x0600A7FC RID: 43004 RVA: 0x003BBEEC File Offset: 0x003BA0EC
	public XmlPatchException(string _message, Exception _innerException)
		: base(_message, _innerException)
	{
	}
}
