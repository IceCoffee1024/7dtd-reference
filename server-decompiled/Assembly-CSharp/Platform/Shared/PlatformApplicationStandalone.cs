using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001D0E RID: 7438
	public class PlatformApplicationStandalone : IPlatformApplication
	{
		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x0600DC4F RID: 56399 RVA: 0x004EA048 File Offset: 0x004E8248
		public Resolution[] SupportedResolutions
		{
			get
			{
				bool flag = false;
				if (this.lastResolutions != null)
				{
					Resolution[] resolutions = Screen.resolutions;
					if (this.lastResolutions.Length == resolutions.Length)
					{
						for (int i = 0; i < resolutions.Length; i++)
						{
							Resolution resolution = this.lastResolutions[i];
							Resolution resolution2 = resolutions[i];
							if (!resolution.Equals(resolution2))
							{
								flag = true;
								this.lastResolutions = resolutions;
								break;
							}
						}
					}
					else
					{
						this.lastResolutions = resolutions;
						flag = true;
					}
				}
				else
				{
					this.lastResolutions = Screen.resolutions;
					flag = true;
				}
				if (flag)
				{
					this.supportedResolutions = this.lastResolutions.Where<Resolution>((Resolution res) => res.width >= 640 && res.height >= 480).ToArray<Resolution>();
				}
				return this.supportedResolutions;
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x0600DC50 RID: 56400 RVA: 0x004EA110 File Offset: 0x004E8310
		[TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
		public ValueTuple<int, int, FullScreenMode> ScreenOptions
		{
			[return: TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
			get
			{
				FullScreenMode @int = (FullScreenMode)PlayerPrefs.GetInt("Screenmanager Fullscreen mode", 3);
				if (PlayerPrefs.HasKey("Screenmanager Resolution Width") && PlayerPrefs.HasKey("Screenmanager Resolution Height"))
				{
					int int2 = PlayerPrefs.GetInt("Screenmanager Resolution Width");
					int int3 = PlayerPrefs.GetInt("Screenmanager Resolution Height");
					return new ValueTuple<int, int, FullScreenMode>(int2, int3, @int);
				}
				Resolution[] array = this.SupportedResolutions;
				if (array.Length > 1)
				{
					Resolution resolution = array[array.Length - 2];
					return new ValueTuple<int, int, FullScreenMode>(resolution.width, resolution.height, @int);
				}
				return new ValueTuple<int, int, FullScreenMode>(Screen.width, Screen.height, FullScreenMode.Windowed);
			}
		}

		// Token: 0x0600DC51 RID: 56401 RVA: 0x004EA19C File Offset: 0x004E839C
		public void SetResolution(int width, int height, FullScreenMode fullscreen)
		{
			if (width < 640 || height < 480 || width <= height)
			{
				fullscreen = FullScreenMode.Windowed;
				PlayerPrefs.SetInt("UnitySelectMonitor", 0);
			}
			if (height > width)
			{
				height = width;
			}
			PlayerPrefs.SetInt("Screenmanager Resolution Width", width);
			PlayerPrefs.SetInt("Screenmanager Resolution Height", height);
			PlayerPrefs.SetInt("Screenmanager Fullscreen mode", (int)fullscreen);
			Screen.SetResolution(width, height, fullscreen);
		}

		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x0600DC52 RID: 56402 RVA: 0x004EA1FB File Offset: 0x004E83FB
		public string temporaryCachePath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x0600DC53 RID: 56403 RVA: 0x000882A0 File Offset: 0x000864A0
		public void RestartProcess(params string[] argv)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400A6D4 RID: 42708
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefResolutionWidth = "Screenmanager Resolution Width";

		// Token: 0x0400A6D5 RID: 42709
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefResolutionHeight = "Screenmanager Resolution Height";

		// Token: 0x0400A6D6 RID: 42710
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefFullscreen = "Screenmanager Fullscreen mode";

		// Token: 0x0400A6D7 RID: 42711
		[PublicizedFrom(EAccessModifier.Private)]
		public const int minResWidth = 640;

		// Token: 0x0400A6D8 RID: 42712
		[PublicizedFrom(EAccessModifier.Private)]
		public const int minResHeight = 480;

		// Token: 0x0400A6D9 RID: 42713
		[PublicizedFrom(EAccessModifier.Private)]
		public Resolution[] lastResolutions;

		// Token: 0x0400A6DA RID: 42714
		[PublicizedFrom(EAccessModifier.Private)]
		public Resolution[] supportedResolutions;
	}
}
