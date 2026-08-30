using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000661 RID: 1633
public interface IRequirement
{
	// Token: 0x060034BC RID: 13500
	bool IsValid(MinEventParams _params);

	// Token: 0x060034BD RID: 13501
	bool ParseXAttribute(XAttribute _attribute);

	// Token: 0x060034BE RID: 13502
	void GetInfoStrings(ref List<string> list);

	// Token: 0x060034BF RID: 13503
	string GetInfoString();

	// Token: 0x060034C0 RID: 13504
	string GetDescription();

	// Token: 0x060034C1 RID: 13505
	void SetDescription(string desc);
}
