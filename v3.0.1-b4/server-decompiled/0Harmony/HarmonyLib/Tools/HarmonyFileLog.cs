using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace HarmonyLib.Tools
{
	// Token: 0x02000057 RID: 87
	public static class HarmonyFileLog
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0000F7CD File Offset: 0x0000D9CD
		// (set) Token: 0x060003CC RID: 972 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
		public static bool Enabled
		{
			get
			{
				return HarmonyFileLog.enabled;
			}
			set
			{
				if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HARMONY_NO_LOG")))
				{
					return;
				}
				HarmonyFileLog.enabled = value;
				HarmonyFileLog.ToggleDebug();
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0000F7F3 File Offset: 0x0000D9F3
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0000F7FA File Offset: 0x0000D9FA
		public static TextWriter Writer
		{
			get
			{
				return HarmonyFileLog.textWriter;
			}
			set
			{
				TextWriter textWriter = HarmonyFileLog.textWriter;
				if (textWriter != null)
				{
					textWriter.Flush();
				}
				HarmonyFileLog.textWriter = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000F812 File Offset: 0x0000DA12
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0000F819 File Offset: 0x0000DA19
		public static string FileWriterPath { get; set; }

		// Token: 0x060003D1 RID: 977 RVA: 0x0000F824 File Offset: 0x0000DA24
		static HarmonyFileLog()
		{
			string text = Environment.GetEnvironmentVariable("HARMONY_LOG_FILE");
			if (string.IsNullOrEmpty(text))
			{
				text = "HarmonyFileLog.log";
			}
			HarmonyFileLog.FileWriterPath = text;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000F850 File Offset: 0x0000DA50
		private static void ToggleDebug()
		{
			if (HarmonyFileLog.Enabled)
			{
				if (HarmonyFileLog.Writer == null)
				{
					HarmonyFileLog.Writer = new StreamWriter(new FileStream(Path.GetFullPath(HarmonyFileLog.FileWriterPath), FileMode.Create, FileAccess.Write, FileShare.Read | FileShare.Write | FileShare.Delete));
				}
				EventHandler<Logger.LogEventArgs> eventHandler;
				if ((eventHandler = HarmonyFileLog.<>O.<0>__OnMessage) == null)
				{
					eventHandler = (HarmonyFileLog.<>O.<0>__OnMessage = new EventHandler<Logger.LogEventArgs>(HarmonyFileLog.OnMessage));
				}
				Logger.MessageReceived += eventHandler;
				return;
			}
			EventHandler<Logger.LogEventArgs> eventHandler2;
			if ((eventHandler2 = HarmonyFileLog.<>O.<0>__OnMessage) == null)
			{
				eventHandler2 = (HarmonyFileLog.<>O.<0>__OnMessage = new EventHandler<Logger.LogEventArgs>(HarmonyFileLog.OnMessage));
			}
			Logger.MessageReceived -= eventHandler2;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000F8C8 File Offset: 0x0000DAC8
		private static void OnMessage(object sender, Logger.LogEventArgs e)
		{
			TextWriter writer = HarmonyFileLog.Writer;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[");
			defaultInterpolatedStringHandler.AppendFormatted<Logger.LogChannel>(e.LogChannel);
			defaultInterpolatedStringHandler.AppendLiteral("] ");
			defaultInterpolatedStringHandler.AppendFormatted(e.Message);
			writer.WriteLine(defaultInterpolatedStringHandler.ToStringAndClear());
			HarmonyFileLog.Writer.Flush();
		}

		// Token: 0x040000D1 RID: 209
		private static bool enabled;

		// Token: 0x040000D2 RID: 210
		private static TextWriter textWriter;

		// Token: 0x020001CB RID: 459
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000259 RID: 601
			public static EventHandler<Logger.LogEventArgs> <0>__OnMessage;
		}
	}
}
