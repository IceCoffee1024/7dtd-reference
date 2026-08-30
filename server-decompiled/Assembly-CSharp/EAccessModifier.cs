using System;
using System.Runtime.InteropServices;

// Token: 0x02001D96 RID: 7574
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public enum EAccessModifier
{
	// Token: 0x0400AD61 RID: 44385
	Unknown,
	// Token: 0x0400AD62 RID: 44386
	Public,
	// Token: 0x0400AD63 RID: 44387
	Private,
	// Token: 0x0400AD64 RID: 44388
	Protected,
	// Token: 0x0400AD65 RID: 44389
	Internal,
	// Token: 0x0400AD66 RID: 44390
	ProtectedInternal,
	// Token: 0x0400AD67 RID: 44391
	PrivateProtected
}
