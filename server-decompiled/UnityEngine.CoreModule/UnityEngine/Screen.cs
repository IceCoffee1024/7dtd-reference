using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000148 RID: 328
	[StaticAccessor("GetScreenManager()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/WindowLayout.h")]
	[NativeHeader("Runtime/Graphics/ScreenManager.h")]
	public sealed class Screen
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600091B RID: 2331
		public static extern int width
		{
			[NativeMethod(Name = "GetWidth", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600091C RID: 2332
		public static extern int height
		{
			[NativeMethod(Name = "GetHeight", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600091D RID: 2333
		public static extern float dpi
		{
			[NativeName("GetDPI")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600091E RID: 2334
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestOrientation(ScreenOrientation orient);

		// Token: 0x0600091F RID: 2335
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ScreenOrientation GetScreenOrientation();

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0000ED24 File Offset: 0x0000CF24
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x0000ED3C File Offset: 0x0000CF3C
		public static ScreenOrientation orientation
		{
			get
			{
				return Screen.GetScreenOrientation();
			}
			set
			{
				bool flag = value == ScreenOrientation.Unknown;
				if (flag)
				{
					Debug.Log("ScreenOrientation.Unknown is deprecated. Please use ScreenOrientation.AutoRotation");
					value = ScreenOrientation.AutoRotation;
				}
				Screen.RequestOrientation(value);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000922 RID: 2338
		// (set) Token: 0x06000923 RID: 2339
		[NativeProperty("ScreenTimeout")]
		public static extern int sleepTimeout
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000924 RID: 2340
		[NativeName("GetIsOrientationEnabled")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsOrientationEnabled(EnabledOrientation orient);

		// Token: 0x06000925 RID: 2341
		[NativeName("SetIsOrientationEnabled")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetOrientationEnabled(EnabledOrientation orient, bool enabled);

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0000ED6C File Offset: 0x0000CF6C
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x0000ED84 File Offset: 0x0000CF84
		public static bool autorotateToPortrait
		{
			get
			{
				return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToPortrait);
			}
			set
			{
				Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToPortrait, value);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0000ED90 File Offset: 0x0000CF90
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
		public static bool autorotateToPortraitUpsideDown
		{
			get
			{
				return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToPortraitUpsideDown);
			}
			set
			{
				Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToPortraitUpsideDown, value);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0000EDCC File Offset: 0x0000CFCC
		public static bool autorotateToLandscapeLeft
		{
			get
			{
				return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeLeft);
			}
			set
			{
				Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeLeft, value);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0000EDD8 File Offset: 0x0000CFD8
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
		public static bool autorotateToLandscapeRight
		{
			get
			{
				return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeRight);
			}
			set
			{
				Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeRight, value);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0000EDFC File Offset: 0x0000CFFC
		public static Resolution currentResolution
		{
			get
			{
				Resolution resolution;
				Screen.get_currentResolution_Injected(out resolution);
				return resolution;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600092F RID: 2351
		// (set) Token: 0x06000930 RID: 2352
		public static extern bool fullScreen
		{
			[NativeName("IsFullscreen")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeName("RequestSetFullscreenFromScript")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000931 RID: 2353
		// (set) Token: 0x06000932 RID: 2354
		public static extern FullScreenMode fullScreenMode
		{
			[NativeName("GetFullscreenMode")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeName("RequestSetFullscreenModeFromScript")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0000EE14 File Offset: 0x0000D014
		public static Rect safeArea
		{
			get
			{
				Rect rect;
				Screen.get_safeArea_Injected(out rect);
				return rect;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000934 RID: 2356
		public static extern Rect[] cutouts
		{
			[FreeFunction("ScreenScripting::GetCutouts")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0000EE29 File Offset: 0x0000D029
		[NativeName("RequestResolution")]
		public static void SetResolution(int width, int height, FullScreenMode fullscreenMode, RefreshRate preferredRefreshRate)
		{
			Screen.SetResolution_Injected(width, height, fullscreenMode, ref preferredRefreshRate);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0000EE38 File Offset: 0x0000D038
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("SetResolution(int, int, FullScreenMode, int) is obsolete. Use SetResolution(int, int, FullScreenMode, RefreshRate) instead.")]
		public static void SetResolution(int width, int height, FullScreenMode fullscreenMode, [DefaultValue("0")] int preferredRefreshRate)
		{
			bool flag = preferredRefreshRate < 0;
			if (flag)
			{
				preferredRefreshRate = 0;
			}
			Screen.SetResolution(width, height, fullscreenMode, new RefreshRate
			{
				numerator = (uint)preferredRefreshRate,
				denominator = 1U
			});
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0000EE74 File Offset: 0x0000D074
		public static void SetResolution(int width, int height, FullScreenMode fullscreenMode)
		{
			Screen.SetResolution(width, height, fullscreenMode, new RefreshRate
			{
				numerator = 0U,
				denominator = 1U
			});
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0000EEA4 File Offset: 0x0000D0A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("SetResolution(int, int, bool, int) is obsolete. Use SetResolution(int, int, FullScreenMode, RefreshRate) instead.")]
		public static void SetResolution(int width, int height, bool fullscreen, [DefaultValue("0")] int preferredRefreshRate)
		{
			bool flag = preferredRefreshRate < 0;
			if (flag)
			{
				preferredRefreshRate = 0;
			}
			Screen.SetResolution(width, height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, new RefreshRate
			{
				numerator = (uint)preferredRefreshRate,
				denominator = 1U
			});
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0000EEE5 File Offset: 0x0000D0E5
		public static void SetResolution(int width, int height, bool fullscreen)
		{
			Screen.SetResolution(width, height, fullscreen, 0);
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		public static Vector2Int mainWindowPosition
		{
			get
			{
				return Screen.GetMainWindowPosition();
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0000EF0C File Offset: 0x0000D10C
		public static DisplayInfo mainWindowDisplayInfo
		{
			get
			{
				return Screen.GetMainWindowDisplayInfo();
			}
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0000EF24 File Offset: 0x0000D124
		public static void GetDisplayLayout(List<DisplayInfo> displayLayout)
		{
			bool flag = displayLayout == null;
			if (flag)
			{
				throw new ArgumentNullException();
			}
			Screen.GetDisplayLayoutImpl(displayLayout);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0000EF48 File Offset: 0x0000D148
		public static AsyncOperation MoveMainWindowTo(in DisplayInfo display, Vector2Int position)
		{
			return Screen.MoveMainWindowImpl(in display, position);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0000EF64 File Offset: 0x0000D164
		[FreeFunction("GetMainWindowPosition")]
		private static Vector2Int GetMainWindowPosition()
		{
			Vector2Int vector2Int;
			Screen.GetMainWindowPosition_Injected(out vector2Int);
			return vector2Int;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0000EF7C File Offset: 0x0000D17C
		[FreeFunction("GetMainWindowDisplayInfo")]
		private static DisplayInfo GetMainWindowDisplayInfo()
		{
			DisplayInfo displayInfo;
			Screen.GetMainWindowDisplayInfo_Injected(out displayInfo);
			return displayInfo;
		}

		// Token: 0x06000940 RID: 2368
		[FreeFunction("GetDisplayLayout")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDisplayLayoutImpl(List<DisplayInfo> displayLayout);

		// Token: 0x06000941 RID: 2369 RVA: 0x0000EF91 File Offset: 0x0000D191
		[FreeFunction("MoveMainWindow")]
		private static AsyncOperation MoveMainWindowImpl(in DisplayInfo display, Vector2Int position)
		{
			return Screen.MoveMainWindowImpl_Injected(in display, ref position);
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000942 RID: 2370
		public static extern Resolution[] resolutions
		{
			[FreeFunction("ScreenScripting::GetResolutions")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000943 RID: 2371
		// (set) Token: 0x06000944 RID: 2372
		public static extern float brightness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0000EF9C File Offset: 0x0000D19C
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		[Obsolete("Use Cursor.lockState and Cursor.visible instead.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool lockCursor
		{
			get
			{
				return CursorLockMode.Locked == Cursor.lockState;
			}
			set
			{
				if (value)
				{
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
				else
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			}
		}

		// Token: 0x06000948 RID: 2376
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_currentResolution_Injected(out Resolution ret);

		// Token: 0x06000949 RID: 2377
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_safeArea_Injected(out Rect ret);

		// Token: 0x0600094A RID: 2378
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetResolution_Injected(int width, int height, FullScreenMode fullscreenMode, ref RefreshRate preferredRefreshRate);

		// Token: 0x0600094B RID: 2379
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMainWindowPosition_Injected(out Vector2Int ret);

		// Token: 0x0600094C RID: 2380
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMainWindowDisplayInfo_Injected(out DisplayInfo ret);

		// Token: 0x0600094D RID: 2381
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AsyncOperation MoveMainWindowImpl_Injected(in DisplayInfo display, ref Vector2Int position);
	}
}
