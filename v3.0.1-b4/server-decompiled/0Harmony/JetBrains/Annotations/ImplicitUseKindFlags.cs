using System;

namespace JetBrains.Annotations
{
	// Token: 0x02000009 RID: 9
	[Flags]
	internal enum ImplicitUseKindFlags
	{
		// Token: 0x0400000B RID: 11
		Default = 7,
		// Token: 0x0400000C RID: 12
		Access = 1,
		// Token: 0x0400000D RID: 13
		Assign = 2,
		// Token: 0x0400000E RID: 14
		InstantiatedWithFixedConstructorSignature = 4,
		// Token: 0x0400000F RID: 15
		InstantiatedNoFixedConstructorSignature = 8
	}
}
