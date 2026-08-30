using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001CAB RID: 7339
	public class PlatformApplicationStandalone : IPlatformApplication
	{
		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x0600D9C1 RID: 55745 RVA: 0x004E1960 File Offset: 0x004DFB60
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

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x0600D9C2 RID: 55746 RVA: 0x004E1A28 File Offset: 0x004DFC28
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

		// Token: 0x0600D9C3 RID: 55747 RVA: 0x004E1AB4 File Offset: 0x004DFCB4
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

		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x0600D9C4 RID: 55748 RVA: 0x004E1B13 File Offset: 0x004DFD13
		public string temporaryCachePath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x0600D9C5 RID: 55749 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void RestartProcess(params string[] argv)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400A516 RID: 42262
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefResolutionWidth = "Screenmanager Resolution Width";

		// Token: 0x0400A517 RID: 42263
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefResolutionHeight = "Screenmanager Resolution Height";

		// Token: 0x0400A518 RID: 42264
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefFullscreen = "Screenmanager Fullscreen mode";

		// Token: 0x0400A519 RID: 42265
		[PublicizedFrom(EAccessModifier.Private)]
		public const int minResWidth = 640;

		// Token: 0x0400A51A RID: 42266
		[PublicizedFrom(EAccessModifier.Private)]
		public const int minResHeight = 480;

		// Token: 0x0400A51B RID: 42267
		[PublicizedFrom(EAccessModifier.Private)]
		public Resolution[] lastResolutions;

		// Token: 0x0400A51C RID: 42268
		[PublicizedFrom(EAccessModifier.Private)]
		public Resolution[] supportedResolutions;
	}
}
