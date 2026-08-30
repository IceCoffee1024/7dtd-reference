using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000170 RID: 368
	[NativeHeader("Runtime/Camera/OcclusionPortal.h")]
	public sealed class OcclusionPortal : Component
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000F5A RID: 3930
		// (set) Token: 0x06000F5B RID: 3931
		[NativeProperty("IsOpen")]
		public extern bool open
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
