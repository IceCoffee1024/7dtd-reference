using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Internal
{
	// Token: 0x020003DD RID: 989
	[ExcludeFromDocs]
	[NativeHeader("Runtime/GfxDevice/HDROutputSettings.h")]
	internal static class InternalHDROutputFaking
	{
		// Token: 0x0600214C RID: 8524
		[FreeFunction("HDROutputSettingsBindings::SetFakeHDROutputEnabled")]
		[ExcludeFromDocs]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetEnabled(bool enabled);
	}
}
