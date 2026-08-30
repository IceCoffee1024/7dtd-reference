using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000199 RID: 409
	[UsedByNativeCode]
	[NativeHeader("Runtime/Graphics/ColorGamut.h")]
	public enum ColorPrimaries
	{
		// Token: 0x04000539 RID: 1337
		Unknown = -1,
		// Token: 0x0400053A RID: 1338
		Rec709,
		// Token: 0x0400053B RID: 1339
		Rec2020,
		// Token: 0x0400053C RID: 1340
		P3
	}
}
