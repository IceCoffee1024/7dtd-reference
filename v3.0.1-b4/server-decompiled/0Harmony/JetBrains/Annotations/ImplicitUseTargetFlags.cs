using System;

namespace JetBrains.Annotations
{
	// Token: 0x0200000A RID: 10
	[Flags]
	internal enum ImplicitUseTargetFlags
	{
		// Token: 0x04000011 RID: 17
		Default = 1,
		// Token: 0x04000012 RID: 18
		Itself = 1,
		// Token: 0x04000013 RID: 19
		Members = 2,
		// Token: 0x04000014 RID: 20
		WithInheritors = 4,
		// Token: 0x04000015 RID: 21
		WithMembers = 3
	}
}
