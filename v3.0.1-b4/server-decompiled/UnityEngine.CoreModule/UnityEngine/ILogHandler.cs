using System;

namespace UnityEngine
{
	// Token: 0x020001E5 RID: 485
	public interface ILogHandler
	{
		// Token: 0x060014C7 RID: 5319
		void LogFormat(LogType logType, Object context, string format, params object[] args);

		// Token: 0x060014C8 RID: 5320
		void LogException(Exception exception, Object context);
	}
}
