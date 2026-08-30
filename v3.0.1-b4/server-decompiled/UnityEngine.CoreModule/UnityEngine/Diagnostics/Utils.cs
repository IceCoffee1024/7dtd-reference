using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Diagnostics
{
	// Token: 0x020004AF RID: 1199
	[NativeHeader("Runtime/Misc/GarbageCollectSharedAssets.h")]
	[NativeHeader("Runtime/Export/Diagnostics/DiagnosticsUtils.bindings.h")]
	public static class Utils
	{
		// Token: 0x060029D1 RID: 10705
		[FreeFunction("DiagnosticsUtils_Bindings::ForceCrash", IsThreadSafe = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ForceCrash(ForcedCrashCategory crashCategory);

		// Token: 0x060029D2 RID: 10706
		[FreeFunction("DiagnosticsUtils_Bindings::NativeAssert", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void NativeAssert(string message);

		// Token: 0x060029D3 RID: 10707
		[FreeFunction("DiagnosticsUtils_Bindings::NativeError", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void NativeError(string message);

		// Token: 0x060029D4 RID: 10708
		[FreeFunction("DiagnosticsUtils_Bindings::NativeWarning", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void NativeWarning(string message);

		// Token: 0x060029D5 RID: 10709
		[FreeFunction("ValidateHeap")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ValidateHeap();
	}
}
