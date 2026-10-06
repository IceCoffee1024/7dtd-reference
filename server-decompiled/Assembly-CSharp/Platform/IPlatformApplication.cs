using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Platform.Shared;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001BFA RID: 7162
	public interface IPlatformApplication
	{
		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x0600D531 RID: 54577
		Resolution[] SupportedResolutions { get; }

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x0600D532 RID: 54578
		[TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
		ValueTuple<int, int, FullScreenMode> ScreenOptions
		{
			[return: TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
			get;
		}

		// Token: 0x0600D533 RID: 54579
		void SetResolution(int width, int height, FullScreenMode fullscreen);

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x0600D534 RID: 54580
		string temporaryCachePath { get; }

		// Token: 0x0600D535 RID: 54581
		void RestartProcess(params string[] argv);

		// Token: 0x0600D536 RID: 54582 RVA: 0x004CC82C File Offset: 0x004CAA2C
		public static IPlatformApplication Create()
		{
			return new PlatformApplicationStandalone();
		}

		// Token: 0x0600D537 RID: 54583 RVA: 0x004CC833 File Offset: 0x004CAA33
		public static string JoinAndEscapeArgv(params string[] args)
		{
			if (args != null)
			{
				return string.Join<string>(' ', args.Select<string, string>(new Func<string, string>(IPlatformApplication.EscapeArg)));
			}
			return null;
		}

		// Token: 0x0600D538 RID: 54584 RVA: 0x004CC854 File Offset: 0x004CAA54
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

		// Token: 0x0600D539 RID: 54585 RVA: 0x004CC934 File Offset: 0x004CAB34
		RefreshRate GetCurrentRefreshRate()
		{
			return Screen.currentResolution.refreshRateRatio;
		}

		// Token: 0x0600D53A RID: 54586 RVA: 0x004CC94E File Offset: 0x004CAB4E
		Resolution GetCurrentResolution()
		{
			return Screen.currentResolution;
		}

		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x0600D53B RID: 54587 RVA: 0x004CC955 File Offset: 0x004CAB55
		EnumGamePrefs VSyncCountPref
		{
			get
			{
				return EnumGamePrefs.OptionsGfxVsync;
			}
		}
	}
}
