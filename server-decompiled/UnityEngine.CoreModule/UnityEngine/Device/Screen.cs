using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Internal;

namespace UnityEngine.Device
{
	// Token: 0x020004B2 RID: 1202
	public static class Screen
	{
		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x0004716F File Offset: 0x0004536F
		// (set) Token: 0x06002A1E RID: 10782 RVA: 0x00047176 File Offset: 0x00045376
		public static float brightness
		{
			get
			{
				return Screen.brightness;
			}
			set
			{
				Screen.brightness = value;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x0004717F File Offset: 0x0004537F
		// (set) Token: 0x06002A20 RID: 10784 RVA: 0x00047186 File Offset: 0x00045386
		public static bool autorotateToLandscapeLeft
		{
			get
			{
				return Screen.autorotateToLandscapeLeft;
			}
			set
			{
				Screen.autorotateToLandscapeLeft = value;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x0004718F File Offset: 0x0004538F
		// (set) Token: 0x06002A22 RID: 10786 RVA: 0x00047196 File Offset: 0x00045396
		public static bool autorotateToLandscapeRight
		{
			get
			{
				return Screen.autorotateToLandscapeRight;
			}
			set
			{
				Screen.autorotateToLandscapeRight = value;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x0004719F File Offset: 0x0004539F
		// (set) Token: 0x06002A24 RID: 10788 RVA: 0x000471A6 File Offset: 0x000453A6
		public static bool autorotateToPortrait
		{
			get
			{
				return Screen.autorotateToPortrait;
			}
			set
			{
				Screen.autorotateToPortrait = value;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x000471AF File Offset: 0x000453AF
		// (set) Token: 0x06002A26 RID: 10790 RVA: 0x000471B6 File Offset: 0x000453B6
		public static bool autorotateToPortraitUpsideDown
		{
			get
			{
				return Screen.autorotateToPortraitUpsideDown;
			}
			set
			{
				Screen.autorotateToPortraitUpsideDown = value;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x000471BF File Offset: 0x000453BF
		public static Resolution currentResolution
		{
			get
			{
				return Screen.currentResolution;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002A28 RID: 10792 RVA: 0x000471C6 File Offset: 0x000453C6
		public static Rect[] cutouts
		{
			get
			{
				return Screen.cutouts;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002A29 RID: 10793 RVA: 0x000471CD File Offset: 0x000453CD
		public static float dpi
		{
			get
			{
				return Screen.dpi;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002A2A RID: 10794 RVA: 0x000471D4 File Offset: 0x000453D4
		// (set) Token: 0x06002A2B RID: 10795 RVA: 0x000471DB File Offset: 0x000453DB
		public static bool fullScreen
		{
			get
			{
				return Screen.fullScreen;
			}
			set
			{
				Screen.fullScreen = value;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002A2C RID: 10796 RVA: 0x000471E4 File Offset: 0x000453E4
		// (set) Token: 0x06002A2D RID: 10797 RVA: 0x000471EB File Offset: 0x000453EB
		public static FullScreenMode fullScreenMode
		{
			get
			{
				return Screen.fullScreenMode;
			}
			set
			{
				Screen.fullScreenMode = value;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002A2E RID: 10798 RVA: 0x000471F4 File Offset: 0x000453F4
		public static int height
		{
			get
			{
				return Screen.height;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x000471FB File Offset: 0x000453FB
		public static int width
		{
			get
			{
				return Screen.width;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06002A30 RID: 10800 RVA: 0x00047202 File Offset: 0x00045402
		// (set) Token: 0x06002A31 RID: 10801 RVA: 0x00047209 File Offset: 0x00045409
		public static ScreenOrientation orientation
		{
			get
			{
				return Screen.orientation;
			}
			set
			{
				Screen.orientation = value;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002A32 RID: 10802 RVA: 0x00047212 File Offset: 0x00045412
		public static Resolution[] resolutions
		{
			get
			{
				return Screen.resolutions;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002A33 RID: 10803 RVA: 0x00047219 File Offset: 0x00045419
		public static Rect safeArea
		{
			get
			{
				return Screen.safeArea;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x00047220 File Offset: 0x00045420
		// (set) Token: 0x06002A35 RID: 10805 RVA: 0x00047227 File Offset: 0x00045427
		public static int sleepTimeout
		{
			get
			{
				return Screen.sleepTimeout;
			}
			set
			{
				Screen.sleepTimeout = value;
			}
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x00047230 File Offset: 0x00045430
		public static void SetResolution(int width, int height, FullScreenMode fullscreenMode, RefreshRate preferredRefreshRate)
		{
			Screen.SetResolution(width, height, fullscreenMode, preferredRefreshRate);
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x00047240 File Offset: 0x00045440
		[Obsolete("SetResolution(int, int, FullScreenMode, int) is obsolete. Use SetResolution(int, int, FullScreenMode, RefreshRate) instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x06002A38 RID: 10808 RVA: 0x0004727C File Offset: 0x0004547C
		public static void SetResolution(int width, int height, FullScreenMode fullscreenMode)
		{
			Screen.SetResolution(width, height, fullscreenMode, new RefreshRate
			{
				numerator = 0U,
				denominator = 1U
			});
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000472AC File Offset: 0x000454AC
		[Obsolete("SetResolution(int, int, bool, int) is obsolete. Use SetResolution(int, int, FullScreenMode, RefreshRate) instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x06002A3A RID: 10810 RVA: 0x000472F0 File Offset: 0x000454F0
		public static void SetResolution(int width, int height, bool fullscreen)
		{
			Screen.SetResolution(width, height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, new RefreshRate
			{
				numerator = 0U,
				denominator = 1U
			});
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002A3B RID: 10811 RVA: 0x00047326 File Offset: 0x00045526
		public static Vector2Int mainWindowPosition
		{
			get
			{
				return Screen.mainWindowPosition;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002A3C RID: 10812 RVA: 0x0004732D File Offset: 0x0004552D
		public static DisplayInfo mainWindowDisplayInfo
		{
			get
			{
				return Screen.mainWindowDisplayInfo;
			}
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x00047334 File Offset: 0x00045534
		public static void GetDisplayLayout(List<DisplayInfo> displayLayout)
		{
			Screen.GetDisplayLayout(displayLayout);
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x0004733D File Offset: 0x0004553D
		public static AsyncOperation MoveMainWindowTo(in DisplayInfo display, Vector2Int position)
		{
			return Screen.MoveMainWindowTo(in display, position);
		}
	}
}
