using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000198 RID: 408
	[UsedByNativeCode]
	[NativeHeader("Runtime/Graphics/ColorGamut.h")]
	public enum ColorGamut
	{
		// Token: 0x04000531 RID: 1329
		sRGB,
		// Token: 0x04000532 RID: 1330
		Rec709,
		// Token: 0x04000533 RID: 1331
		Rec2020,
		// Token: 0x04000534 RID: 1332
		DisplayP3,
		// Token: 0x04000535 RID: 1333
		HDR10,
		// Token: 0x04000536 RID: 1334
		DolbyHDR,
		// Token: 0x04000537 RID: 1335
		P3D65G22
	}
}
