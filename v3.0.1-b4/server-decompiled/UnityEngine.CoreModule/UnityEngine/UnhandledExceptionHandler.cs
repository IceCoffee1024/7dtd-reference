using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000264 RID: 612
	[NativeHeader("PlatformDependent/iPhonePlayer/IOSScriptBindings.h")]
	internal sealed class UnhandledExceptionHandler
	{
		// Token: 0x060019A3 RID: 6563 RVA: 0x0002B169 File Offset: 0x00029369
		[RequiredByNativeCode]
		private static void RegisterUECatcher()
		{
			AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e)
			{
				Debug.LogException(e.ExceptionObject as Exception);
			};
		}
	}
}
