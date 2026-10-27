using System;
using System.Xml;
using System.Xml.Linq;

// Token: 0x0200152C RID: 5420
public class XmlPatchException : Exception
{
	// Token: 0x0600AA00 RID: 43520 RVA: 0x003F445C File Offset: 0x003F265C
	public XmlPatchException(XElement _patchElement, string _patchMethodName, string _message)
		: this(XmlPatchException.buildMessage(_patchElement, _patchMethodName, _message))
	{
	}

	// Token: 0x0600AA01 RID: 43521 RVA: 0x003F446C File Offset: 0x003F266C
	public XmlPatchException(XElement _patchElement, string _patchMethodName, string _message, Exception _innerException)
		: this(XmlPatchException.buildMessage(_patchElement, _patchMethodName, _message), _innerException)
	{
	}

	// Token: 0x0600AA02 RID: 43522 RVA: 0x003F4480 File Offset: 0x003F2680
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

	// Token: 0x0600AA03 RID: 43523 RVA: 0x003C81C7 File Offset: 0x003C63C7
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlPatchException(string _message)
		: base(_message)
	{
	}

	// Token: 0x0600AA04 RID: 43524 RVA: 0x003C81D0 File Offset: 0x003C63D0
	public XmlPatchException(string _message, Exception _innerException)
		: base(_message, _innerException)
	{
	}
}
