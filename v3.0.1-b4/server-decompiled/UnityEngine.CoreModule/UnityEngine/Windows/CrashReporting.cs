using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Windows
{
	// Token: 0x020002C5 RID: 709
	public static class CrashReporting
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001E5F RID: 7775
		public static extern string crashReportFolder
		{
			[ThreadSafe]
			[NativeHeader("PlatformDependent/WinPlayer/Bindings/CrashReportingBindings.h")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
