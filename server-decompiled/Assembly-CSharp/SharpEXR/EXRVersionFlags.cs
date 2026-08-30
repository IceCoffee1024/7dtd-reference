using System;

namespace SharpEXR
{
	// Token: 0x020016CD RID: 5837
	[Flags]
	public enum EXRVersionFlags
	{
		// Token: 0x04008874 RID: 34932
		IsSinglePartTiled = 512,
		// Token: 0x04008875 RID: 34933
		LongNames = 1024,
		// Token: 0x04008876 RID: 34934
		NonImageParts = 2048,
		// Token: 0x04008877 RID: 34935
		MultiPart = 4096
	}
}
