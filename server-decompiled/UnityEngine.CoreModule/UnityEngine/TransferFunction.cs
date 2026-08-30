using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200019B RID: 411
	[UsedByNativeCode]
	[NativeHeader("Runtime/Graphics/ColorGamut.h")]
	public enum TransferFunction
	{
		// Token: 0x04000541 RID: 1345
		Unknown = -1,
		// Token: 0x04000542 RID: 1346
		sRGB,
		// Token: 0x04000543 RID: 1347
		BT1886,
		// Token: 0x04000544 RID: 1348
		PQ,
		// Token: 0x04000545 RID: 1349
		Linear,
		// Token: 0x04000546 RID: 1350
		Gamma22
	}
}
