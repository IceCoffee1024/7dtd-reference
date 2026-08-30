using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003E7 RID: 999
	public static class LoadStoreActionDebugModeSettings
	{
		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060021AB RID: 8619
		// (set) Token: 0x060021AC RID: 8620
		[StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
		public static extern bool LoadStoreDebugModeEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
