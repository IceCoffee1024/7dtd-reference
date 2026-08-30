using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000CD RID: 205
	[Flags]
	public enum ImplicitUseKindFlags
	{
		// Token: 0x04000259 RID: 601
		Default = 7,
		// Token: 0x0400025A RID: 602
		Access = 1,
		// Token: 0x0400025B RID: 603
		Assign = 2,
		// Token: 0x0400025C RID: 604
		InstantiatedWithFixedConstructorSignature = 4,
		// Token: 0x0400025D RID: 605
		InstantiatedNoFixedConstructorSignature = 8
	}
}
