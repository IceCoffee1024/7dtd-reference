using System;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003FA RID: 1018
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum StencilOp
	{
		// Token: 0x04000BAF RID: 2991
		Keep,
		// Token: 0x04000BB0 RID: 2992
		Zero,
		// Token: 0x04000BB1 RID: 2993
		Replace,
		// Token: 0x04000BB2 RID: 2994
		IncrementSaturate,
		// Token: 0x04000BB3 RID: 2995
		DecrementSaturate,
		// Token: 0x04000BB4 RID: 2996
		Invert,
		// Token: 0x04000BB5 RID: 2997
		IncrementWrap,
		// Token: 0x04000BB6 RID: 2998
		DecrementWrap
	}
}
