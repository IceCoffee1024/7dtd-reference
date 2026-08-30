using System;
using UnityEngine.Scripting;

namespace Unity.Collections
{
	// Token: 0x02000095 RID: 149
	[UsedByNativeCode]
	internal enum LeakCategory
	{
		// Token: 0x04000220 RID: 544
		Invalid,
		// Token: 0x04000221 RID: 545
		Malloc,
		// Token: 0x04000222 RID: 546
		TempJob,
		// Token: 0x04000223 RID: 547
		Persistent,
		// Token: 0x04000224 RID: 548
		LightProbesQuery,
		// Token: 0x04000225 RID: 549
		NativeTest,
		// Token: 0x04000226 RID: 550
		MeshDataArray,
		// Token: 0x04000227 RID: 551
		TransformAccessArray,
		// Token: 0x04000228 RID: 552
		NavMeshQuery
	}
}
