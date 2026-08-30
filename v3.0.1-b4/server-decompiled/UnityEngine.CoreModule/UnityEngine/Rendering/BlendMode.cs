using System;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003F5 RID: 1013
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum BlendMode
	{
		// Token: 0x04000B6A RID: 2922
		Zero,
		// Token: 0x04000B6B RID: 2923
		One,
		// Token: 0x04000B6C RID: 2924
		DstColor,
		// Token: 0x04000B6D RID: 2925
		SrcColor,
		// Token: 0x04000B6E RID: 2926
		OneMinusDstColor,
		// Token: 0x04000B6F RID: 2927
		SrcAlpha,
		// Token: 0x04000B70 RID: 2928
		OneMinusSrcColor,
		// Token: 0x04000B71 RID: 2929
		DstAlpha,
		// Token: 0x04000B72 RID: 2930
		OneMinusDstAlpha,
		// Token: 0x04000B73 RID: 2931
		SrcAlphaSaturate,
		// Token: 0x04000B74 RID: 2932
		OneMinusSrcAlpha
	}
}
