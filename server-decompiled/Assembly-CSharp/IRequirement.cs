using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x0200066A RID: 1642
public interface IRequirement
{
	// Token: 0x0600350D RID: 13581
	bool IsValid(MinEventParams _params);

	// Token: 0x0600350E RID: 13582
	bool ParseXAttribute(XAttribute _attribute);

	// Token: 0x0600350F RID: 13583
	void GetInfoStrings(ref List<string> list);

	// Token: 0x06003510 RID: 13584
	string GetInfoString();

	// Token: 0x06003511 RID: 13585
	string GetDescription();

	// Token: 0x06003512 RID: 13586
	void SetDescription(string desc);
}
