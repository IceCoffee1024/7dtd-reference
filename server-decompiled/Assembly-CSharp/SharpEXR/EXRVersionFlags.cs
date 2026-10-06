using System;

namespace SharpEXR
{
	// Token: 0x020016F9 RID: 5881
	[Flags]
	public enum EXRVersionFlags
	{
		// Token: 0x04008939 RID: 35129
		IsSinglePartTiled = 512,
		// Token: 0x0400893A RID: 35130
		LongNames = 1024,
		// Token: 0x0400893B RID: 35131
		NonImageParts = 2048,
		// Token: 0x0400893C RID: 35132
		MultiPart = 4096
	}
}
