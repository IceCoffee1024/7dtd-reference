using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001CDD RID: 7389
	public class PlatformApplicationStandalone : IPlatformApplication
	{
		// Token: 0x17001BB1 RID: 7089
		// (get) Token: 0x0600DB3B RID: 56123 RVA: 0x004E64A8 File Offset: 0x004E46A8
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

		// Token: 0x17001BB2 RID: 7090
		// (get) Token: 0x0600DB3C RID: 56124 RVA: 0x004E6570 File Offset: 0x004E4770
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

		// Token: 0x0600DB3D RID: 56125 RVA: 0x004E65FC File Offset: 0x004E47FC
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

		// Token: 0x17001BB3 RID: 7091
		// (get) Token: 0x0600DB3E RID: 56126 RVA: 0x004E665B File Offset: 0x004E485B
		public string temporaryCachePath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x0600DB3F RID: 56127 RVA: 0x0008806C File Offset: 0x0008626C
		public void RestartProcess(params string[] argv)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400A5EB RID: 42475
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefResolutionWidth = "Screenmanager Resolution Width";

		// Token: 0x0400A5EC RID: 42476
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefResolutionHeight = "Screenmanager Resolution Height";

		// Token: 0x0400A5ED RID: 42477
		[PublicizedFrom(EAccessModifier.Private)]
		public const string prefFullscreen = "Screenmanager Fullscreen mode";

		// Token: 0x0400A5EE RID: 42478
		[PublicizedFrom(EAccessModifier.Private)]
		public const int minResWidth = 640;

		// Token: 0x0400A5EF RID: 42479
		[PublicizedFrom(EAccessModifier.Private)]
		public const int minResHeight = 480;

		// Token: 0x0400A5F0 RID: 42480
		[PublicizedFrom(EAccessModifier.Private)]
		public Resolution[] lastResolutions;

		// Token: 0x0400A5F1 RID: 42481
		[PublicizedFrom(EAccessModifier.Private)]
		public Resolution[] supportedResolutions;
	}
}
