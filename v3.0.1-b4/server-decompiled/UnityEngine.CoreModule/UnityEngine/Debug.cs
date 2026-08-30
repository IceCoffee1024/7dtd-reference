using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200011A RID: 282
	[NativeHeader("Runtime/Export/Debug/Debug.bindings.h")]
	public class Debug
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00009919 File Offset: 0x00007B19
		public static ILogger unityLogger
		{
			get
			{
				return Debug.s_Logger;
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00009920 File Offset: 0x00007B20
		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
		{
			bool flag = true;
			Debug.DrawLine(start, end, color, duration, flag);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0000993C File Offset: 0x00007B3C
		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end, Color color)
		{
			bool flag = true;
			float num = 0f;
			Debug.DrawLine(start, end, color, num, flag);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00009960 File Offset: 0x00007B60
		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end)
		{
			bool flag = true;
			float num = 0f;
			Color white = Color.white;
			Debug.DrawLine(start, end, white, num, flag);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00009987 File Offset: 0x00007B87
		[FreeFunction("DebugDrawLine", IsThreadSafe = true)]
		public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
		{
			Debug.DrawLine_Injected(ref start, ref end, ref color, duration, depthTest);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00009998 File Offset: 0x00007B98
		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
		{
			bool flag = true;
			Debug.DrawRay(start, dir, color, duration, flag);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x000099B4 File Offset: 0x00007BB4
		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color)
		{
			bool flag = true;
			float num = 0f;
			Debug.DrawRay(start, dir, color, num, flag);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000099D8 File Offset: 0x00007BD8
		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir)
		{
			bool flag = true;
			float num = 0f;
			Color white = Color.white;
			Debug.DrawRay(start, dir, white, num, flag);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x000099FF File Offset: 0x00007BFF
		public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
		{
			Debug.DrawLine(start, start + dir, color, duration, depthTest);
		}

		// Token: 0x060006E3 RID: 1763
		[FreeFunction("PauseEditor")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Break();

		// Token: 0x060006E4 RID: 1764
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DebugBreak();

		// Token: 0x060006E5 RID: 1765
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern int ExtractStackTraceNoAlloc(byte* buffer, int bufferMax, string projectFolder);

		// Token: 0x060006E6 RID: 1766 RVA: 0x00009A14 File Offset: 0x00007C14
		public static void Log(object message)
		{
			Debug.unityLogger.Log(LogType.Log, message);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00009A24 File Offset: 0x00007C24
		public static void Log(object message, Object context)
		{
			Debug.unityLogger.Log(LogType.Log, message, context);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00009A35 File Offset: 0x00007C35
		public static void LogFormat(string format, params object[] args)
		{
			Debug.unityLogger.LogFormat(LogType.Log, format, args);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00009A46 File Offset: 0x00007C46
		public static void LogFormat(Object context, string format, params object[] args)
		{
			Debug.unityLogger.LogFormat(LogType.Log, context, format, args);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00009A58 File Offset: 0x00007C58
		public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
		{
			DebugLogHandler debugLogHandler = Debug.unityLogger.logHandler as DebugLogHandler;
			bool flag = debugLogHandler == null;
			if (flag)
			{
				Debug.unityLogger.LogFormat(logType, context, format, args);
			}
			else
			{
				bool flag2 = Debug.unityLogger.IsLogTypeAllowed(logType);
				if (flag2)
				{
					debugLogHandler.LogFormat(logType, logOptions, context, format, args);
				}
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00009AAC File Offset: 0x00007CAC
		public static void LogError(object message)
		{
			Debug.unityLogger.Log(LogType.Error, message);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00009ABC File Offset: 0x00007CBC
		public static void LogError(object message, Object context)
		{
			Debug.unityLogger.Log(LogType.Error, message, context);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00009ACD File Offset: 0x00007CCD
		public static void LogErrorFormat(string format, params object[] args)
		{
			Debug.unityLogger.LogFormat(LogType.Error, format, args);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00009ADE File Offset: 0x00007CDE
		public static void LogErrorFormat(Object context, string format, params object[] args)
		{
			Debug.unityLogger.LogFormat(LogType.Error, context, format, args);
		}

		// Token: 0x060006EF RID: 1775
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearDeveloperConsole();

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060006F0 RID: 1776
		// (set) Token: 0x060006F1 RID: 1777
		public static extern bool developerConsoleEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060006F2 RID: 1778
		// (set) Token: 0x060006F3 RID: 1779
		public static extern bool developerConsoleVisible
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00009AF0 File Offset: 0x00007CF0
		public static void LogException(Exception exception)
		{
			Debug.unityLogger.LogException(exception, null);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00009B00 File Offset: 0x00007D00
		public static void LogException(Exception exception, Object context)
		{
			Debug.unityLogger.LogException(exception, context);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00009B10 File Offset: 0x00007D10
		public static void LogWarning(object message)
		{
			Debug.unityLogger.Log(LogType.Warning, message);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00009B20 File Offset: 0x00007D20
		public static void LogWarning(object message, Object context)
		{
			Debug.unityLogger.Log(LogType.Warning, message, context);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00009B31 File Offset: 0x00007D31
		public static void LogWarningFormat(string format, params object[] args)
		{
			Debug.unityLogger.LogFormat(LogType.Warning, format, args);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00009B42 File Offset: 0x00007D42
		public static void LogWarningFormat(Object context, string format, params object[] args)
		{
			Debug.unityLogger.LogFormat(LogType.Warning, context, format, args);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00009B54 File Offset: 0x00007D54
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.Log(LogType.Assert, "Assertion failed");
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00009B7C File Offset: 0x00007D7C
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, Object context)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.Log(LogType.Assert, "Assertion failed", context);
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00009BA4 File Offset: 0x00007DA4
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.Log(LogType.Assert, message);
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00009BC8 File Offset: 0x00007DC8
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string message)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.Log(LogType.Assert, message);
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00009BEC File Offset: 0x00007DEC
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message, Object context)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.Log(LogType.Assert, message, context);
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00009C10 File Offset: 0x00007E10
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string message, Object context)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.Log(LogType.Assert, message, context);
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00009C34 File Offset: 0x00007E34
		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertFormat(bool condition, string format, params object[] args)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.LogFormat(LogType.Assert, format, args);
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00009C58 File Offset: 0x00007E58
		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertFormat(bool condition, Object context, string format, params object[] args)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.LogFormat(LogType.Assert, context, format, args);
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00009C7D File Offset: 0x00007E7D
		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertion(object message)
		{
			Debug.unityLogger.Log(LogType.Assert, message);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00009C8D File Offset: 0x00007E8D
		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertion(object message, Object context)
		{
			Debug.unityLogger.Log(LogType.Assert, message, context);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00009C9E File Offset: 0x00007E9E
		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertionFormat(string format, params object[] args)
		{
			Debug.unityLogger.LogFormat(LogType.Assert, format, args);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00009CAF File Offset: 0x00007EAF
		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertionFormat(Object context, string format, params object[] args)
		{
			Debug.unityLogger.LogFormat(LogType.Assert, context, format, args);
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000706 RID: 1798
		public static extern bool isDebugBuild
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000707 RID: 1799
		[FreeFunction("DeveloperConsole_OpenConsoleFile")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OpenConsoleFile();

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000708 RID: 1800
		[NativeThrows]
		internal static extern DiagnosticSwitch[] diagnosticSwitches
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00009CC4 File Offset: 0x00007EC4
		internal static DiagnosticSwitch GetDiagnosticSwitch(string name)
		{
			foreach (DiagnosticSwitch diagnosticSwitch in Debug.diagnosticSwitches)
			{
				bool flag = diagnosticSwitch.name == name;
				if (flag)
				{
					return diagnosticSwitch;
				}
			}
			throw new ArgumentException("Could not find DiagnosticSwitch named " + name);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00009D18 File Offset: 0x00007F18
		[RequiredByNativeCode]
		internal static bool CallOverridenDebugHandler(Exception exception, Object obj)
		{
			bool flag = Debug.unityLogger.logHandler is DebugLogHandler;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				try
				{
					Debug.unityLogger.LogException(exception, obj);
				}
				catch (Exception ex)
				{
					Debug.s_DefaultLogger.LogError(string.Format("Invalid exception thrown from custom {0}.LogException(). Message: {1}", Debug.unityLogger.logHandler.GetType(), ex), obj);
					return false;
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00009D94 File Offset: 0x00007F94
		[RequiredByNativeCode]
		internal static bool IsLoggingEnabled()
		{
			bool flag = Debug.unityLogger.logHandler is DebugLogHandler;
			bool flag2;
			if (flag)
			{
				flag2 = Debug.unityLogger.logEnabled;
			}
			else
			{
				flag2 = Debug.s_DefaultLogger.logEnabled;
			}
			return flag2;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00009DD4 File Offset: 0x00007FD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Assert(bool, string, params object[]) is obsolete. Use AssertFormat(bool, string, params object[]) (UnityUpgradable) -> AssertFormat(*)", true)]
		public static void Assert(bool condition, string format, params object[] args)
		{
			bool flag = !condition;
			if (flag)
			{
				Debug.unityLogger.LogFormat(LogType.Assert, format, args);
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00009DF8 File Offset: 0x00007FF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Debug.logger is obsolete. Please use Debug.unityLogger instead (UnityUpgradable) -> unityLogger")]
		public static ILogger logger
		{
			get
			{
				return Debug.s_Logger;
			}
		}

		// Token: 0x06000710 RID: 1808
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawLine_Injected(ref Vector3 start, ref Vector3 end, [DefaultValue("Color.white")] ref Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest);

		// Token: 0x040003A1 RID: 929
		internal static readonly ILogger s_DefaultLogger = new Logger(new DebugLogHandler());

		// Token: 0x040003A2 RID: 930
		internal static ILogger s_Logger = new Logger(new DebugLogHandler());
	}
}
