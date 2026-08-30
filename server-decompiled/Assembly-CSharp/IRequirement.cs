using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000658 RID: 1624
public interface IRequirement
{
	// Token: 0x06003469 RID: 13417
	bool IsValid(MinEventParams _params);

	// Token: 0x0600346A RID: 13418
	bool ParseXAttribute(XAttribute _attribute);

	// Token: 0x0600346B RID: 13419
	void GetInfoStrings(ref List<string> list);

	// Token: 0x0600346C RID: 13420
	string GetInfoString();

	// Token: 0x0600346D RID: 13421
	string GetDescription();

	// Token: 0x0600346E RID: 13422
	void SetDescription(string desc);
}
