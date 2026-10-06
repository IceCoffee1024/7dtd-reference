using System;
using System.Xml;
using System.Xml.Linq;

// Token: 0x0200151A RID: 5402
public class XmlPatchException : Exception
{
	// Token: 0x0600A9A8 RID: 43432 RVA: 0x003F2058 File Offset: 0x003F0258
	public XmlPatchException(XElement _patchElement, string _patchMethodName, string _message)
		: this(XmlPatchException.buildMessage(_patchElement, _patchMethodName, _message))
	{
	}

	// Token: 0x0600A9A9 RID: 43433 RVA: 0x003F2068 File Offset: 0x003F0268
	public XmlPatchException(XElement _patchElement, string _patchMethodName, string _message, Exception _innerException)
		: this(XmlPatchException.buildMessage(_patchElement, _patchMethodName, _message), _innerException)
	{
	}

	// Token: 0x0600A9AA RID: 43434 RVA: 0x003F207C File Offset: 0x003F027C
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

	// Token: 0x0600A9AB RID: 43435 RVA: 0x003C5DC3 File Offset: 0x003C3FC3
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlPatchException(string _message)
		: base(_message)
	{
	}

	// Token: 0x0600A9AC RID: 43436 RVA: 0x003C5DCC File Offset: 0x003C3FCC
	public XmlPatchException(string _message, Exception _innerException)
		: base(_message, _innerException)
	{
	}
}
