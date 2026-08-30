using System;

namespace SharpEXR
{
	// Token: 0x020016A7 RID: 5799
	[Flags]
	public enum EXRVersionFlags
	{
		// Token: 0x04008758 RID: 34648
		IsSinglePartTiled = 512,
		// Token: 0x04008759 RID: 34649
		LongNames = 1024,
		// Token: 0x0400875A RID: 34650
		NonImageParts = 2048,
		// Token: 0x0400875B RID: 34651
		MultiPart = 4096
	}
}
