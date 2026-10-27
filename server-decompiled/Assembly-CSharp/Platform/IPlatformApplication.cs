using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Platform.Shared;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001C2B RID: 7211
	public interface IPlatformApplication
	{
		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x0600D645 RID: 54853
		Resolution[] SupportedResolutions { get; }

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x0600D646 RID: 54854
		[TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
		ValueTuple<int, int, FullScreenMode> ScreenOptions
		{
			[return: TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
			get;
		}

		// Token: 0x0600D647 RID: 54855
		void SetResolution(int width, int height, FullScreenMode fullscreen);

		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x0600D648 RID: 54856
		string temporaryCachePath { get; }

		// Token: 0x0600D649 RID: 54857
		void RestartProcess(params string[] argv);

		// Token: 0x0600D64A RID: 54858 RVA: 0x004D0354 File Offset: 0x004CE554
		public static IPlatformApplication Create()
		{
			return new PlatformApplicationStandalone();
		}

		// Token: 0x0600D64B RID: 54859 RVA: 0x004D035B File Offset: 0x004CE55B
		public static string JoinAndEscapeArgv(params string[] args)
		{
			if (args != null)
			{
				return string.Join<string>(' ', args.Select<string, string>(new Func<string, string>(IPlatformApplication.EscapeArg)));
			}
			return null;
		}

		// Token: 0x0600D64C RID: 54860 RVA: 0x004D037C File Offset: 0x004CE57C
		public static string EscapeArg(string arg)
		{
			if (arg.Length > 0 && arg.AsSpan().IndexOfAny<char>(" \t\n\v\"") < 0)
			{
				return arg;
			}
			if (arg.Length <= 0)
			{
				return "\"\"";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('"');
			int num = 0;
			int num2;
			for (;;)
			{
				num2 = 0;
				while (num < arg.Length && arg[num] == '\\')
				{
					num++;
					num2++;
				}
				if (num >= arg.Length)
				{
					break;
				}
				if (arg[num] == '"')
				{
					stringBuilder.Append('\\', num2 * 2 + 1);
					stringBuilder.Append(arg[num]);
				}
				else
				{
					stringBuilder.Append('\\', num2);
					stringBuilder.Append(arg[num]);
				}
				num++;
			}
			stringBuilder.Append('\\', num2 * 2);
			stringBuilder.Append('"');
			return stringBuilder.ToString();
		}

		// Token: 0x0600D64D RID: 54861 RVA: 0x004D045C File Offset: 0x004CE65C
		RefreshRate GetCurrentRefreshRate()
		{
			return Screen.currentResolution.refreshRateRatio;
		}

		// Token: 0x0600D64E RID: 54862 RVA: 0x004D0476 File Offset: 0x004CE676
		Resolution GetCurrentResolution()
		{
			return Screen.currentResolution;
		}

		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x0600D64F RID: 54863 RVA: 0x004D047D File Offset: 0x004CE67D
		EnumGamePrefs VSyncCountPref
		{
			get
			{
				return EnumGamePrefs.OptionsGfxVsync;
			}
		}
	}
}
