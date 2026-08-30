using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Platform.Shared;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001BC8 RID: 7112
	public interface IPlatformApplication
	{
		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x0600D3B9 RID: 54201
		Resolution[] SupportedResolutions { get; }

		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x0600D3BA RID: 54202
		[TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
		ValueTuple<int, int, FullScreenMode> ScreenOptions
		{
			[return: TupleElementNames(new string[] { "width", "height", "fullScreenMode" })]
			get;
		}

		// Token: 0x0600D3BB RID: 54203
		void SetResolution(int width, int height, FullScreenMode fullscreen);

		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x0600D3BC RID: 54204
		string temporaryCachePath { get; }

		// Token: 0x0600D3BD RID: 54205
		void RestartProcess(params string[] argv);

		// Token: 0x0600D3BE RID: 54206 RVA: 0x004C7D44 File Offset: 0x004C5F44
		public static IPlatformApplication Create()
		{
			return new PlatformApplicationStandalone();
		}

		// Token: 0x0600D3BF RID: 54207 RVA: 0x004C7D4B File Offset: 0x004C5F4B
		public static string JoinAndEscapeArgv(params string[] args)
		{
			if (args != null)
			{
				return string.Join<string>(' ', args.Select<string, string>(new Func<string, string>(IPlatformApplication.EscapeArg)));
			}
			return null;
		}

		// Token: 0x0600D3C0 RID: 54208 RVA: 0x004C7D6C File Offset: 0x004C5F6C
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

		// Token: 0x0600D3C1 RID: 54209 RVA: 0x004C7E4C File Offset: 0x004C604C
		RefreshRate GetCurrentRefreshRate()
		{
			return Screen.currentResolution.refreshRateRatio;
		}

		// Token: 0x0600D3C2 RID: 54210 RVA: 0x004C7E66 File Offset: 0x004C6066
		Resolution GetCurrentResolution()
		{
			return Screen.currentResolution;
		}

		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x0600D3C3 RID: 54211 RVA: 0x004C7E6D File Offset: 0x004C606D
		EnumGamePrefs VSyncCountPref
		{
			get
			{
				return EnumGamePrefs.OptionsGfxVsync;
			}
		}
	}
}
