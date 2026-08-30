using System;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003F7 RID: 1015
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum CompareFunction
	{
		// Token: 0x04000B9B RID: 2971
		Disabled,
		// Token: 0x04000B9C RID: 2972
		Never,
		// Token: 0x04000B9D RID: 2973
		Less,
		// Token: 0x04000B9E RID: 2974
		Equal,
		// Token: 0x04000B9F RID: 2975
		LessEqual,
		// Token: 0x04000BA0 RID: 2976
		Greater,
		// Token: 0x04000BA1 RID: 2977
		NotEqual,
		// Token: 0x04000BA2 RID: 2978
		GreaterEqual,
		// Token: 0x04000BA3 RID: 2979
		Always
	}
}
