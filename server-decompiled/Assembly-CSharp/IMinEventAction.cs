using System;
using System.Xml.Linq;

// Token: 0x02000713 RID: 1811
public interface IMinEventAction
{
	// Token: 0x0600373D RID: 14141
	bool CanExecute(MinEventTypes _eventType, MinEventParams _params);

	// Token: 0x0600373E RID: 14142
	void Execute(MinEventParams _params);

	// Token: 0x0600373F RID: 14143
	bool ParseXmlAttribute(XAttribute _attribute);

	// Token: 0x06003740 RID: 14144
	void ParseXMLPostProcess();
}
