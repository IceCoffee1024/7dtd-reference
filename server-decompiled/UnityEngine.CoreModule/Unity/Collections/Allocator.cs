using System;
using UnityEngine.Scripting;

namespace Unity.Collections
{
	// Token: 0x02000093 RID: 147
	[UsedByNativeCode]
	public enum Allocator
	{
		// Token: 0x04000214 RID: 532
		Invalid,
		// Token: 0x04000215 RID: 533
		None,
		// Token: 0x04000216 RID: 534
		Temp,
		// Token: 0x04000217 RID: 535
		TempJob,
		// Token: 0x04000218 RID: 536
		Persistent,
		// Token: 0x04000219 RID: 537
		AudioKernel,
		// Token: 0x0400021A RID: 538
		FirstUserIndex = 64
	}
}
