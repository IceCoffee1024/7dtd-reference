using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000119 RID: 281
	[NativeHeader("Runtime/Export/Debug/Debug.bindings.h")]
	internal sealed class DebugLogHandler : ILogHandler
	{
		// Token: 0x060006D4 RID: 1748
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_Log(LogType level, LogOption options, string msg, Object obj);

		// Token: 0x060006D5 RID: 1749
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_LogException(Exception ex, Object obj);

		// Token: 0x060006D6 RID: 1750 RVA: 0x000098C4 File Offset: 0x00007AC4
		public void LogFormat(LogType logType, Object context, string format, params object[] args)
		{
			DebugLogHandler.Internal_Log(logType, LogOption.None, string.Format(format, args), context);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000098D8 File Offset: 0x00007AD8
		public void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
		{
			DebugLogHandler.Internal_Log(logType, logOptions, string.Format(format, args), context);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000098F0 File Offset: 0x00007AF0
		public void LogException(Exception exception, Object context)
		{
			bool flag = exception == null;
			if (flag)
			{
				throw new ArgumentNullException("exception");
			}
			DebugLogHandler.Internal_LogException(exception, context);
		}
	}
}
