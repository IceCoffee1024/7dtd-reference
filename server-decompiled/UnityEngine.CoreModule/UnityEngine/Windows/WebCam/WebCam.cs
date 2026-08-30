using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Windows.WebCam
{
	// Token: 0x020002EF RID: 751
	[MovedFrom("UnityEngine.XR.WSA.WebCam")]
	[NativeHeader("PlatformDependent/Win/Webcam/WebCam.h")]
	[StaticAccessor("WebCam::GetInstance()", StaticAccessorType.Dot)]
	public class WebCam
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001F40 RID: 8000
		public static extern WebCamMode Mode
		{
			[NativeName("GetWebCamMode")]
			[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
