using System;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x0200007A RID: 122
	public enum ReadStatus
	{
		// Token: 0x040001C4 RID: 452
		Complete,
		// Token: 0x040001C5 RID: 453
		InProgress,
		// Token: 0x040001C6 RID: 454
		Failed,
		// Token: 0x040001C7 RID: 455
		Truncated = 4,
		// Token: 0x040001C8 RID: 456
		Canceled
	}
}
