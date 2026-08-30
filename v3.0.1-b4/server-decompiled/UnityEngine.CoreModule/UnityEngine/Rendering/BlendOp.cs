using System;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003F6 RID: 1014
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum BlendOp
	{
		// Token: 0x04000B76 RID: 2934
		Add,
		// Token: 0x04000B77 RID: 2935
		Subtract,
		// Token: 0x04000B78 RID: 2936
		ReverseSubtract,
		// Token: 0x04000B79 RID: 2937
		Min,
		// Token: 0x04000B7A RID: 2938
		Max,
		// Token: 0x04000B7B RID: 2939
		LogicalClear,
		// Token: 0x04000B7C RID: 2940
		LogicalSet,
		// Token: 0x04000B7D RID: 2941
		LogicalCopy,
		// Token: 0x04000B7E RID: 2942
		LogicalCopyInverted,
		// Token: 0x04000B7F RID: 2943
		LogicalNoop,
		// Token: 0x04000B80 RID: 2944
		LogicalInvert,
		// Token: 0x04000B81 RID: 2945
		LogicalAnd,
		// Token: 0x04000B82 RID: 2946
		LogicalNand,
		// Token: 0x04000B83 RID: 2947
		LogicalOr,
		// Token: 0x04000B84 RID: 2948
		LogicalNor,
		// Token: 0x04000B85 RID: 2949
		LogicalXor,
		// Token: 0x04000B86 RID: 2950
		LogicalEquivalence,
		// Token: 0x04000B87 RID: 2951
		LogicalAndReverse,
		// Token: 0x04000B88 RID: 2952
		LogicalAndInverted,
		// Token: 0x04000B89 RID: 2953
		LogicalOrReverse,
		// Token: 0x04000B8A RID: 2954
		LogicalOrInverted,
		// Token: 0x04000B8B RID: 2955
		Multiply,
		// Token: 0x04000B8C RID: 2956
		Screen,
		// Token: 0x04000B8D RID: 2957
		Overlay,
		// Token: 0x04000B8E RID: 2958
		Darken,
		// Token: 0x04000B8F RID: 2959
		Lighten,
		// Token: 0x04000B90 RID: 2960
		ColorDodge,
		// Token: 0x04000B91 RID: 2961
		ColorBurn,
		// Token: 0x04000B92 RID: 2962
		HardLight,
		// Token: 0x04000B93 RID: 2963
		SoftLight,
		// Token: 0x04000B94 RID: 2964
		Difference,
		// Token: 0x04000B95 RID: 2965
		Exclusion,
		// Token: 0x04000B96 RID: 2966
		HSLHue,
		// Token: 0x04000B97 RID: 2967
		HSLSaturation,
		// Token: 0x04000B98 RID: 2968
		HSLColor,
		// Token: 0x04000B99 RID: 2969
		HSLLuminosity
	}
}
