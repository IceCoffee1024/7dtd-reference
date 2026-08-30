using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x02000432 RID: 1074
	[NativeHeader("Runtime/Graphics/DrawSplashScreenAndWatermarks.h")]
	public class SplashScreen
	{
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002425 RID: 9253
		public static extern bool isFinished
		{
			[FreeFunction("IsSplashScreenFinished")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06002426 RID: 9254
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CancelSplashScreen();

		// Token: 0x06002427 RID: 9255
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void BeginSplashScreenFade();

		// Token: 0x06002428 RID: 9256
		[FreeFunction("BeginSplashScreen_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Begin();

		// Token: 0x06002429 RID: 9257 RVA: 0x0003C020 File Offset: 0x0003A220
		public static void Stop(SplashScreen.StopBehavior stopBehavior)
		{
			bool flag = stopBehavior == SplashScreen.StopBehavior.FadeOut;
			if (flag)
			{
				SplashScreen.BeginSplashScreenFade();
			}
			else
			{
				SplashScreen.CancelSplashScreen();
			}
		}

		// Token: 0x0600242A RID: 9258
		[FreeFunction("DrawSplashScreen_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Draw();

		// Token: 0x0600242B RID: 9259
		[FreeFunction("SetSplashScreenTime")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetTime(float time);

		// Token: 0x02000433 RID: 1075
		public enum StopBehavior
		{
			// Token: 0x04000D23 RID: 3363
			StopImmediate,
			// Token: 0x04000D24 RID: 3364
			FadeOut
		}
	}
}
