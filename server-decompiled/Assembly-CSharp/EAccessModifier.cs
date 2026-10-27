using System;
using System.Runtime.InteropServices;

// Token: 0x02001DF8 RID: 7672
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public enum EAccessModifier
{
	// Token: 0x0400AF23 RID: 44835
	Unknown,
	// Token: 0x0400AF24 RID: 44836
	Public,
	// Token: 0x0400AF25 RID: 44837
	Private,
	// Token: 0x0400AF26 RID: 44838
	Protected,
	// Token: 0x0400AF27 RID: 44839
	Internal,
	// Token: 0x0400AF28 RID: 44840
	ProtectedInternal,
	// Token: 0x0400AF29 RID: 44841
	PrivateProtected
}
