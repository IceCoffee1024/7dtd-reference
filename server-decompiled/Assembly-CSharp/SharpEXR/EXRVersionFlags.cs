using System;

namespace SharpEXR
{
	// Token: 0x02001718 RID: 5912
	[Flags]
	public enum EXRVersionFlags
	{
		// Token: 0x040089EC RID: 35308
		IsSinglePartTiled = 512,
		// Token: 0x040089ED RID: 35309
		LongNames = 1024,
		// Token: 0x040089EE RID: 35310
		NonImageParts = 2048,
		// Token: 0x040089EF RID: 35311
		MultiPart = 4096
	}
}
