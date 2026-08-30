using System;
using System.Xml.Linq;

// Token: 0x020006FF RID: 1791
public interface IMinEventAction
{
	// Token: 0x0600368D RID: 13965
	bool CanExecute(MinEventTypes _eventType, MinEventParams _params);

	// Token: 0x0600368E RID: 13966
	void Execute(MinEventParams _params);

	// Token: 0x0600368F RID: 13967
	bool ParseXmlAttribute(XAttribute _attribute);

	// Token: 0x06003690 RID: 13968
	void ParseXMLPostProcess();
}
