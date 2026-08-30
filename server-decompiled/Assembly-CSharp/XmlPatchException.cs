using System;
using System.Xml;
using System.Xml.Linq;

// Token: 0x020014F8 RID: 5368
public class XmlPatchException : Exception
{
	// Token: 0x0600A8F5 RID: 43253 RVA: 0x003ED204 File Offset: 0x003EB404
	public XmlPatchException(XElement _patchElement, string _patchMethodName, string _message)
		: this(XmlPatchException.buildMessage(_patchElement, _patchMethodName, _message))
	{
	}

	// Token: 0x0600A8F6 RID: 43254 RVA: 0x003ED214 File Offset: 0x003EB414
	public XmlPatchException(XElement _patchElement, string _patchMethodName, string _message, Exception _innerException)
		: this(XmlPatchException.buildMessage(_patchElement, _patchMethodName, _message), _innerException)
	{
	}

	// Token: 0x0600A8F7 RID: 43255 RVA: 0x003ED228 File Offset: 0x003EB428
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

	// Token: 0x0600A8F8 RID: 43256 RVA: 0x003C1227 File Offset: 0x003BF427
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlPatchException(string _message)
		: base(_message)
	{
	}

	// Token: 0x0600A8F9 RID: 43257 RVA: 0x003C1230 File Offset: 0x003BF430
	public XmlPatchException(string _message, Exception _innerException)
		: base(_message, _innerException)
	{
	}
}
