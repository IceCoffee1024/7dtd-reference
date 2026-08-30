using System;
using UnityEngine.Bindings;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x0200007F RID: 127
	[NativeHeader("Runtime/File/AsyncReadManagerMetrics.h")]
	public enum ProcessingState
	{
		// Token: 0x040001D1 RID: 465
		Unknown,
		// Token: 0x040001D2 RID: 466
		InQueue,
		// Token: 0x040001D3 RID: 467
		Reading,
		// Token: 0x040001D4 RID: 468
		Completed,
		// Token: 0x040001D5 RID: 469
		Failed,
		// Token: 0x040001D6 RID: 470
		Canceled
	}
}
