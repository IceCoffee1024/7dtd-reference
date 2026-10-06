using System;
using System.Runtime.InteropServices;

// Token: 0x02001DC8 RID: 7624
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public enum EAccessModifier
{
	// Token: 0x0400AE3A RID: 44602
	Unknown,
	// Token: 0x0400AE3B RID: 44603
	Public,
	// Token: 0x0400AE3C RID: 44604
	Private,
	// Token: 0x0400AE3D RID: 44605
	Protected,
	// Token: 0x0400AE3E RID: 44606
	Internal,
	// Token: 0x0400AE3F RID: 44607
	ProtectedInternal,
	// Token: 0x0400AE40 RID: 44608
	PrivateProtected
}
