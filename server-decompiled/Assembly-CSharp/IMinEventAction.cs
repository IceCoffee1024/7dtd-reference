using System;
using System.Xml.Linq;

// Token: 0x02000709 RID: 1801
public interface IMinEventAction
{
	// Token: 0x060036E3 RID: 14051
	bool CanExecute(MinEventTypes _eventType, MinEventParams _params);

	// Token: 0x060036E4 RID: 14052
	void Execute(MinEventParams _params);

	// Token: 0x060036E5 RID: 14053
	bool ParseXmlAttribute(XAttribute _attribute);

	// Token: 0x060036E6 RID: 14054
	void ParseXMLPostProcess();
}
