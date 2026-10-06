using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000663 RID: 1635
public interface IRequirement
{
	// Token: 0x060034FF RID: 13567
	bool IsValid(MinEventParams _params);

	// Token: 0x06003500 RID: 13568
	bool ParseXAttribute(XAttribute _attribute);

	// Token: 0x06003501 RID: 13569
	void GetInfoStrings(ref List<string> list);

	// Token: 0x06003502 RID: 13570
	string GetInfoString();

	// Token: 0x06003503 RID: 13571
	string GetDescription();

	// Token: 0x06003504 RID: 13572
	void SetDescription(string desc);
}
