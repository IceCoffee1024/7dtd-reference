using System;

namespace HarmonyLib.Tools
{
	// Token: 0x02000058 RID: 88
	public static class Logger
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000F92B File Offset: 0x0000DB2B
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x0000F932 File Offset: 0x0000DB32
		public static Logger.LogChannel ChannelFilter { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003D6 RID: 982 RVA: 0x0000F93C File Offset: 0x0000DB3C
		// (remove) Token: 0x060003D7 RID: 983 RVA: 0x0000F970 File Offset: 0x0000DB70
		public static event EventHandler<Logger.LogEventArgs> MessageReceived;

		// Token: 0x060003D8 RID: 984 RVA: 0x0000F9A3 File Offset: 0x0000DBA3
		internal static bool IsEnabledFor(Logger.LogChannel channel)
		{
			return (channel & Logger.ChannelFilter) > Logger.LogChannel.None;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000F9AF File Offset: 0x0000DBAF
		internal static void Log(Logger.LogChannel channel, Func<string> message, bool forcePropagation = false)
		{
			if (forcePropagation || Logger.IsEnabledFor(channel))
			{
				EventHandler<Logger.LogEventArgs> messageReceived = Logger.MessageReceived;
				if (messageReceived == null)
				{
					return;
				}
				messageReceived(null, new Logger.LogEventArgs
				{
					LogChannel = channel,
					Message = message()
				});
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000F9E4 File Offset: 0x0000DBE4
		internal static void LogText(Logger.LogChannel channel, string message, bool forcePropagation = false)
		{
			if (forcePropagation || Logger.IsEnabledFor(channel))
			{
				EventHandler<Logger.LogEventArgs> messageReceived = Logger.MessageReceived;
				if (messageReceived == null)
				{
					return;
				}
				messageReceived(null, new Logger.LogEventArgs
				{
					LogChannel = channel,
					Message = message
				});
			}
		}

		// Token: 0x020001CC RID: 460
		public class LogEventArgs : EventArgs
		{
			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x060007BD RID: 1981 RVA: 0x0001C21B File Offset: 0x0001A41B
			// (set) Token: 0x060007BE RID: 1982 RVA: 0x0001C223 File Offset: 0x0001A423
			public Logger.LogChannel LogChannel { get; internal set; }

			// Token: 0x170001F8 RID: 504
			// (get) Token: 0x060007BF RID: 1983 RVA: 0x0001C22C File Offset: 0x0001A42C
			// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0001C234 File Offset: 0x0001A434
			public string Message { get; internal set; }
		}

		// Token: 0x020001CD RID: 461
		[Flags]
		public enum LogChannel
		{
			// Token: 0x0400025D RID: 605
			None = 0,
			// Token: 0x0400025E RID: 606
			Info = 2,
			// Token: 0x0400025F RID: 607
			IL = 4,
			// Token: 0x04000260 RID: 608
			Warn = 8,
			// Token: 0x04000261 RID: 609
			Error = 16,
			// Token: 0x04000262 RID: 610
			Debug = 32,
			// Token: 0x04000263 RID: 611
			All = 62
		}
	}
}
