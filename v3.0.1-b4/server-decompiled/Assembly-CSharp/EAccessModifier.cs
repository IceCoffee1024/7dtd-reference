using System;
using System.Runtime.InteropServices;

// Token: 0x02001D6F RID: 7535
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public enum EAccessModifier
{
	// Token: 0x0400AC2A RID: 44074
	Unknown,
	// Token: 0x0400AC2B RID: 44075
	Public,
	// Token: 0x0400AC2C RID: 44076
	Private,
	// Token: 0x0400AC2D RID: 44077
	Protected,
	// Token: 0x0400AC2E RID: 44078
	Internal,
	// Token: 0x0400AC2F RID: 44079
	ProtectedInternal,
	// Token: 0x0400AC30 RID: 44080
	PrivateProtected
}
