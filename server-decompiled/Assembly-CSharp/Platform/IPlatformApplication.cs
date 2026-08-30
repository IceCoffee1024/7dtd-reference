using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Platform.Shared;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001BA1 RID: 7073
	public interface IPlatformApplication
	{
		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x0600D22B RID: 53803
		Resolution[] SupportedResolutions { get; }

		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x0600D22C RID: 53804
		[TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
		ValueTuple<int, int, FullScreenMode> ScreenOptions
		{
			[return: TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
			get;
		}

		// Token: 0x0600D22D RID: 53805
		void SetResolution(int width, int height, FullScreenMode fullscreen);

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x0600D22E RID: 53806
		string temporaryCachePath { get; }

		// Token: 0x0600D22F RID: 53807
		void RestartProcess(params string[] argv);

		// Token: 0x0600D230 RID: 53808 RVA: 0x004C1950 File Offset: 0x004BFB50
		public static IPlatformApplication Create()
		{
			return new PlatformApplicationStandalone();
		}

		// Token: 0x0600D231 RID: 53809 RVA: 0x004C1957 File Offset: 0x004BFB57
		public static string JoinAndEscapeArgv(params string[] args)
		{
			if (args != null)
			{
				return string.Join<string>(' ', args.Select<string, string>(new Func<string, string>(IPlatformApplication.EscapeArg)));
			}
			return null;
		}

		// Token: 0x0600D232 RID: 53810 RVA: 0x004C1978 File Offset: 0x004BFB78
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

		// Token: 0x0600D233 RID: 53811 RVA: 0x004C1A58 File Offset: 0x004BFC58
		RefreshRate GetCurrentRefreshRate()
		{
			return Screen.currentResolution.refreshRateRatio;
		}

		// Token: 0x0600D234 RID: 53812 RVA: 0x004C1A72 File Offset: 0x004BFC72
		Resolution GetCurrentResolution()
		{
			return Screen.currentResolution;
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x0600D235 RID: 53813 RVA: 0x004C1A79 File Offset: 0x004BFC79
		EnumGamePrefs VSyncCountPref
		{
			get
			{
				return EnumGamePrefs.OptionsGfxVsync;
			}
		}
	}
}
