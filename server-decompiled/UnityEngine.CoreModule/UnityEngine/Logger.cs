using System;
using System.Globalization;

namespace UnityEngine
{
	// Token: 0x020001E6 RID: 486
	public class Logger : ILogger, ILogHandler
	{
		// Token: 0x060014C9 RID: 5321 RVA: 0x00009E2F File Offset: 0x0000802F
		private Logger()
		{
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0001E40A File Offset: 0x0001C60A
		public Logger(ILogHandler logHandler)
		{
			this.logHandler = logHandler;
			this.logEnabled = true;
			this.filterLogType = LogType.Log;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0001E42C File Offset: 0x0001C62C
		// (set) Token: 0x060014CC RID: 5324 RVA: 0x0001E434 File Offset: 0x0001C634
		public ILogHandler logHandler { get; set; }

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x0001E43D File Offset: 0x0001C63D
		// (set) Token: 0x060014CE RID: 5326 RVA: 0x0001E445 File Offset: 0x0001C645
		public bool logEnabled { get; set; }

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060014CF RID: 5327 RVA: 0x0001E44E File Offset: 0x0001C64E
		// (set) Token: 0x060014D0 RID: 5328 RVA: 0x0001E456 File Offset: 0x0001C656
		public LogType filterLogType { get; set; }

		// Token: 0x060014D1 RID: 5329 RVA: 0x0001E460 File Offset: 0x0001C660
		public bool IsLogTypeAllowed(LogType logType)
		{
			bool logEnabled = this.logEnabled;
			if (logEnabled)
			{
				bool flag = logType == LogType.Exception;
				if (flag)
				{
					return true;
				}
				bool flag2 = this.filterLogType != LogType.Exception;
				if (flag2)
				{
					return logType <= this.filterLogType;
				}
			}
			return false;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0001E4AC File Offset: 0x0001C6AC
		private static string GetString(object message)
		{
			bool flag = message == null;
			string text;
			if (flag)
			{
				text = "Null";
			}
			else
			{
				IFormattable formattable = message as IFormattable;
				bool flag2 = formattable != null;
				if (flag2)
				{
					text = formattable.ToString(null, CultureInfo.InvariantCulture);
				}
				else
				{
					text = message.ToString();
				}
			}
			return text;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0001E4F8 File Offset: 0x0001C6F8
		public void Log(LogType logType, object message)
		{
			bool flag = this.IsLogTypeAllowed(logType);
			if (flag)
			{
				this.logHandler.LogFormat(logType, null, "{0}", new object[] { Logger.GetString(message) });
			}
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0001E534 File Offset: 0x0001C734
		public void Log(LogType logType, object message, Object context)
		{
			bool flag = this.IsLogTypeAllowed(logType);
			if (flag)
			{
				this.logHandler.LogFormat(logType, context, "{0}", new object[] { Logger.GetString(message) });
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0001E570 File Offset: 0x0001C770
		public void Log(LogType logType, string tag, object message)
		{
			bool flag = this.IsLogTypeAllowed(logType);
			if (flag)
			{
				this.logHandler.LogFormat(logType, null, "{0}: {1}", new object[]
				{
					tag,
					Logger.GetString(message)
				});
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0001E5B0 File Offset: 0x0001C7B0
		public void Log(LogType logType, string tag, object message, Object context)
		{
			bool flag = this.IsLogTypeAllowed(logType);
			if (flag)
			{
				this.logHandler.LogFormat(logType, context, "{0}: {1}", new object[]
				{
					tag,
					Logger.GetString(message)
				});
			}
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0001E5F0 File Offset: 0x0001C7F0
		public void Log(object message)
		{
			bool flag = this.IsLogTypeAllowed(LogType.Log);
			if (flag)
			{
				this.logHandler.LogFormat(LogType.Log, null, "{0}", new object[] { Logger.GetString(message) });
			}
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0001E62C File Offset: 0x0001C82C
		public void Log(string tag, object message)
		{
			bool flag = this.IsLogTypeAllowed(LogType.Log);
			if (flag)
			{
				this.logHandler.LogFormat(LogType.Log, null, "{0}: {1}", new object[]
				{
					tag,
					Logger.GetString(message)
				});
			}
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0001E66C File Offset: 0x0001C86C
		public void Log(string tag, object message, Object context)
		{
			bool flag = this.IsLogTypeAllowed(LogType.Log);
			if (flag)
			{
				this.logHandler.LogFormat(LogType.Log, context, "{0}: {1}", new object[]
				{
					tag,
					Logger.GetString(message)
				});
			}
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0001E6AC File Offset: 0x0001C8AC
		public void LogWarning(string tag, object message)
		{
			bool flag = this.IsLogTypeAllowed(LogType.Warning);
			if (flag)
			{
				this.logHandler.LogFormat(LogType.Warning, null, "{0}: {1}", new object[]
				{
					tag,
					Logger.GetString(message)
				});
			}
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0001E6EC File Offset: 0x0001C8EC
		public void LogWarning(string tag, object message, Object context)
		{
			bool flag = this.IsLogTypeAllowed(LogType.Warning);
			if (flag)
			{
				this.logHandler.LogFormat(LogType.Warning, context, "{0}: {1}", new object[]
				{
					tag,
					Logger.GetString(message)
				});
			}
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0001E72C File Offset: 0x0001C92C
		public void LogError(string tag, object message)
		{
			bool flag = this.IsLogTypeAllowed(LogType.Error);
			if (flag)
			{
				this.logHandler.LogFormat(LogType.Error, null, "{0}: {1}", new object[]
				{
					tag,
					Logger.GetString(message)
				});
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0001E76C File Offset: 0x0001C96C
		public void LogError(string tag, object message, Object context)
		{
			bool flag = this.IsLogTypeAllowed(LogType.Error);
			if (flag)
			{
				this.logHandler.LogFormat(LogType.Error, context, "{0}: {1}", new object[]
				{
					tag,
					Logger.GetString(message)
				});
			}
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0001E7AC File Offset: 0x0001C9AC
		public void LogException(Exception exception)
		{
			bool logEnabled = this.logEnabled;
			if (logEnabled)
			{
				this.logHandler.LogException(exception, null);
			}
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0001E7D4 File Offset: 0x0001C9D4
		public void LogException(Exception exception, Object context)
		{
			bool logEnabled = this.logEnabled;
			if (logEnabled)
			{
				this.logHandler.LogException(exception, context);
			}
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0001E7FC File Offset: 0x0001C9FC
		public void LogFormat(LogType logType, string format, params object[] args)
		{
			bool flag = this.IsLogTypeAllowed(logType);
			if (flag)
			{
				this.logHandler.LogFormat(logType, null, format, args);
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0001E828 File Offset: 0x0001CA28
		public void LogFormat(LogType logType, Object context, string format, params object[] args)
		{
			bool flag = this.IsLogTypeAllowed(logType);
			if (flag)
			{
				this.logHandler.LogFormat(logType, context, format, args);
			}
		}

		// Token: 0x040007D6 RID: 2006
		private const string kNoTagFormat = "{0}";

		// Token: 0x040007D7 RID: 2007
		private const string kTagFormat = "{0}: {1}";
	}
}
