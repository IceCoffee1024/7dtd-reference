using System;
using System.Xml.Linq;

// Token: 0x0200070B RID: 1803
public interface IMinEventAction
{
	// Token: 0x06003726 RID: 14118
	bool CanExecute(MinEventTypes _eventType, MinEventParams _params);

	// Token: 0x06003727 RID: 14119
	void Execute(MinEventParams _params);

	// Token: 0x06003728 RID: 14120
	bool ParseXmlAttribute(XAttribute _attribute);

	// Token: 0x06003729 RID: 14121
	void ParseXMLPostProcess();
}
