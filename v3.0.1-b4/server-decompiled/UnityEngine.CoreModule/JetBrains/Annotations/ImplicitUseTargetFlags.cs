using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000CE RID: 206
	[Flags]
	public enum ImplicitUseTargetFlags
	{
		// Token: 0x0400025F RID: 607
		Default = 1,
		// Token: 0x04000260 RID: 608
		Itself = 1,
		// Token: 0x04000261 RID: 609
		Members = 2,
		// Token: 0x04000262 RID: 610
		WithMembers = 3
	}
}
