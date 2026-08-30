using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001C84 RID: 7300
	public class PlatformApplicationStandalone : IPlatformApplication
	{
		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x0600D82B RID: 55339 RVA: 0x004DB384 File Offset: 0x004D9584
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

		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x0600D82C RID: 55340 RVA: 0x004DB44C File Offset: 0x004D964C
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

		// Token: 0x0600D82D RID: 55341 RVA: 0x004DB4D8 File Offset: 0x004D96D8
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

		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x0600D82E RID: 55342 RVA: 0x004DB537 File Offset: 0x004D9737
		public string temporaryCachePath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x0600D82F RID: 55343 RVA: 0x000876FD File Offset: 0x000858FD
		public void RestartProcess(params string[] argv)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400A3E3 RID: 41955
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefResolutionWidth = "Screenmanager Resolution Width";

		// Token: 0x0400A3E4 RID: 41956
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefResolutionHeight = "Screenmanager Resolution Height";

		// Token: 0x0400A3E5 RID: 41957
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefFullscreen = "Screenmanager Fullscreen mode";

		// Token: 0x0400A3E6 RID: 41958
		[PublicizedFrom(EAccessModifier.Private)]
		public const int minResWidth = 640;

		// Token: 0x0400A3E7 RID: 41959
		[PublicizedFrom(EAccessModifier.Private)]
		public const int minResHeight = 480;

		// Token: 0x0400A3E8 RID: 41960
		[PublicizedFrom(EAccessModifier.Private)]
		public Resolution[] lastResolutions;

		// Token: 0x0400A3E9 RID: 41961
		[PublicizedFrom(EAccessModifier.Private)]
		public Resolution[] supportedResolutions;
	}
}
