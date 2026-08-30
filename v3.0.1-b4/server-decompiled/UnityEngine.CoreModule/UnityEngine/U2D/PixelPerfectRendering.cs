using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.U2D
{
	// Token: 0x020002B2 RID: 690
	[NativeHeader("Runtime/2D/Common/PixelSnapping.h")]
	[MovedFrom("UnityEngine.Experimental.U2D")]
	public static class PixelPerfectRendering
	{
		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001D67 RID: 7527
		// (set) Token: 0x06001D68 RID: 7528
		public static extern float pixelSnapSpacing
		{
			[FreeFunction("GetPixelSnapSpacing")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("SetPixelSnapSpacing")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
