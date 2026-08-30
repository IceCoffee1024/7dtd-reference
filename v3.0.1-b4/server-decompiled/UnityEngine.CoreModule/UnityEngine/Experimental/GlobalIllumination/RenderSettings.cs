using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004CC RID: 1228
	[NativeHeader("Runtime/Camera/RenderSettings.h")]
	[StaticAccessor("GetRenderSettings()", StaticAccessorType.Dot)]
	public class RenderSettings
	{
		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002AEA RID: 10986
		// (set) Token: 0x06002AEB RID: 10987
		public static extern bool useRadianceAmbientProbe
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
