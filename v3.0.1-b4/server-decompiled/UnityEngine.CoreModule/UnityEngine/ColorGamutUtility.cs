using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000155 RID: 341
	public class ColorGamutUtility
	{
		// Token: 0x06000ACF RID: 2767
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ColorPrimaries GetColorPrimaries(ColorGamut gamut);

		// Token: 0x06000AD0 RID: 2768
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern WhitePoint GetWhitePoint(ColorGamut gamut);

		// Token: 0x06000AD1 RID: 2769
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern TransferFunction GetTransferFunction(ColorGamut gamut);
	}
}
