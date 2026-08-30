using System;

namespace UnityEngine
{
	// Token: 0x020001E4 RID: 484
	public interface ILogger : ILogHandler
	{
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060014B3 RID: 5299
		// (set) Token: 0x060014B4 RID: 5300
		ILogHandler logHandler { get; set; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060014B5 RID: 5301
		// (set) Token: 0x060014B6 RID: 5302
		bool logEnabled { get; set; }

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060014B7 RID: 5303
		// (set) Token: 0x060014B8 RID: 5304
		LogType filterLogType { get; set; }

		// Token: 0x060014B9 RID: 5305
		bool IsLogTypeAllowed(LogType logType);

		// Token: 0x060014BA RID: 5306
		void Log(LogType logType, object message);

		// Token: 0x060014BB RID: 5307
		void Log(LogType logType, object message, Object context);

		// Token: 0x060014BC RID: 5308
		void Log(LogType logType, string tag, object message);

		// Token: 0x060014BD RID: 5309
		void Log(LogType logType, string tag, object message, Object context);

		// Token: 0x060014BE RID: 5310
		void Log(object message);

		// Token: 0x060014BF RID: 5311
		void Log(string tag, object message);

		// Token: 0x060014C0 RID: 5312
		void Log(string tag, object message, Object context);

		// Token: 0x060014C1 RID: 5313
		void LogWarning(string tag, object message);

		// Token: 0x060014C2 RID: 5314
		void LogWarning(string tag, object message, Object context);

		// Token: 0x060014C3 RID: 5315
		void LogError(string tag, object message);

		// Token: 0x060014C4 RID: 5316
		void LogError(string tag, object message, Object context);

		// Token: 0x060014C5 RID: 5317
		void LogFormat(LogType logType, string format, params object[] args);

		// Token: 0x060014C6 RID: 5318
		void LogException(Exception exception);
	}
}
