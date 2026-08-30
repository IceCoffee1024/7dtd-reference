using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class Log
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	// (remove) Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
	public static event Log.LogCallbackDelegate LogCallbacks;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000003 RID: 3 RVA: 0x000020B8 File Offset: 0x000002B8
	// (remove) Token: 0x06000004 RID: 4 RVA: 0x000020EC File Offset: 0x000002EC
	public static event Log.LogCallbackExtendedDelegate LogCallbacksExtended;

	// Token: 0x06000005 RID: 5 RVA: 0x00002120 File Offset: 0x00000320
	static Log()
	{
		Application.logMessageReceivedThreaded += Log.UnityLogCallback;
		Log.InitLogStringBuilder();
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002175 File Offset: 0x00000375
	public static void AddOutputPath(string _path)
	{
		Log.additionalOutputPaths.Add(_path);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002184 File Offset: 0x00000384
	public static void WriteLine(string _logString)
	{
		object obj = Log.lockObject;
		lock (obj)
		{
			Console.WriteLine(_logString);
			foreach (string text in Log.additionalOutputPaths)
			{
				try
				{
					using (StreamWriter streamWriter = new StreamWriter(text, true))
					{
						streamWriter.WriteLine(_logString);
					}
				}
				catch (Exception)
				{
				}
			}
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002238 File Offset: 0x00000438
	private static void UnityLogCallback(string _logString, string _stackTrace, LogType _type)
	{
		object obj = Log.lockObject;
		lock (obj)
		{
			DateTime dateTime;
			long num;
			Log.SendToLogListeners(Log.formatMessage(_logString, _type, out dateTime, out num), _stackTrace, _type, dateTime, num, _logString);
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002288 File Offset: 0x00000488
	private static void SendToLogListeners(string _formattedMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime, string _message)
	{
		Log.LogCallbackDelegate logCallbacks = Log.LogCallbacks;
		if (logCallbacks != null)
		{
			logCallbacks(_formattedMessage, _trace, _type);
		}
		Log.LogCallbackExtendedDelegate logCallbacksExtended = Log.LogCallbacksExtended;
		if (logCallbacksExtended == null)
		{
			return;
		}
		logCallbacksExtended(_formattedMessage, _message, _trace, _type, _timestamp, _uptime);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000022B4 File Offset: 0x000004B4
	private static string formatUnityMessage(string _txt)
	{
		return _txt;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000022B8 File Offset: 0x000004B8
	private static void InitLogStringBuilder()
	{
		Log.logStringBuilder.Length = 20;
		Log.logStringBuilder.Capacity = 250;
		Log.logStringBuilder[4] = '-';
		Log.logStringBuilder[7] = '-';
		Log.logStringBuilder[10] = 'T';
		Log.logStringBuilder[13] = ':';
		Log.logStringBuilder[16] = ':';
		Log.logStringBuilder[19] = ' ';
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002334 File Offset: 0x00000534
	private static void SetDateTime(DateTime _dt)
	{
		Log.logStringBuilder.Length = 20;
		Log.logStringBuilder[0] = (char)(_dt.Year / 1000 + 48);
		Log.logStringBuilder[1] = (char)(_dt.Year / 100 % 10 + 48);
		Log.logStringBuilder[2] = (char)(_dt.Year / 10 % 10 + 48);
		Log.logStringBuilder[3] = (char)(_dt.Year % 10 + 48);
		Log.logStringBuilder[5] = (char)(_dt.Month / 10 + 48);
		Log.logStringBuilder[6] = (char)(_dt.Month % 10 + 48);
		Log.logStringBuilder[8] = (char)(_dt.Day / 10 + 48);
		Log.logStringBuilder[9] = (char)(_dt.Day % 10 + 48);
		Log.logStringBuilder[11] = (char)(_dt.Hour / 10 + 48);
		Log.logStringBuilder[12] = (char)(_dt.Hour % 10 + 48);
		Log.logStringBuilder[14] = (char)(_dt.Minute / 10 + 48);
		Log.logStringBuilder[15] = (char)(_dt.Minute % 10 + 48);
		Log.logStringBuilder[17] = (char)(_dt.Second / 10 + 48);
		Log.logStringBuilder[18] = (char)(_dt.Second % 10 + 48);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000024BC File Offset: 0x000006BC
	private static string formatMessage(string _txt, LogType _logType, out DateTime _timestamp, out long _uptime)
	{
		_timestamp = DateTime.Now;
		Log.SetDateTime(_timestamp);
		_uptime = Log.globalWatch.ElapsedMilliseconds;
		int num = 0;
		long num2 = _uptime;
		while (num2 > 0L || num < 4)
		{
			Log.logStringBuilder.Insert(Log.logStringBuilder.Length - num, (char)(num2 % 10L + 48L));
			num++;
			num2 /= 10L;
		}
		Log.logStringBuilder.Insert(Log.logStringBuilder.Length - 3, '.');
		switch (_logType)
		{
		case LogType.Error:
			Log.logStringBuilder.Append(" ERR ");
			goto IL_00EA;
		case LogType.Assert:
			Log.logStringBuilder.Append(" ASS ");
			goto IL_00EA;
		case LogType.Warning:
			Log.logStringBuilder.Append(" WRN ");
			goto IL_00EA;
		case LogType.Exception:
			Log.logStringBuilder.Append(" EXC ");
			goto IL_00EA;
		}
		Log.logStringBuilder.Append(" INF ");
		IL_00EA:
		Log.logStringBuilder.Append(_txt);
		return Log.logStringBuilder.ToString();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000025CC File Offset: 0x000007CC
	private static void masterLogStandalone(string _txt, LogType _logType, string _trace = null)
	{
		object obj = Log.lockObject;
		lock (obj)
		{
			DateTime dateTime;
			long num;
			string text = Log.formatMessage(_txt, _logType, out dateTime, out num);
			Log.WriteLine(text);
			if (_trace != null)
			{
				Log.WriteLine(_trace);
			}
			Log.SendToLogListeners(text, _trace, _logType, dateTime, num, _txt);
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000262C File Offset: 0x0000082C
	public static void Out(string _txt)
	{
		if (Log.isEditor)
		{
			string text = Log.formatUnityMessage(_txt);
			object obj = Log.lockObject;
			lock (obj)
			{
				Debug.Log(text);
				return;
			}
		}
		Log.masterLogStandalone(_txt, LogType.Log, null);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002684 File Offset: 0x00000884
	public static void Warning(string _txt)
	{
		if (Log.isEditor)
		{
			string text = Log.formatUnityMessage(_txt);
			object obj = Log.lockObject;
			lock (obj)
			{
				Debug.LogWarning(text);
				return;
			}
		}
		Log.masterLogStandalone(_txt, LogType.Warning, null);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000026DC File Offset: 0x000008DC
	public static void Error(string _txt)
	{
		if (Log.isEditor)
		{
			string text = Log.formatUnityMessage(_txt);
			object obj = Log.lockObject;
			lock (obj)
			{
				Debug.LogError(text);
				return;
			}
		}
		Log.masterLogStandalone(_txt, LogType.Error, null);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002734 File Offset: 0x00000934
	public static void Exception(Exception _e)
	{
		if (Log.isEditor)
		{
			object obj = Log.lockObject;
			lock (obj)
			{
				Debug.LogException(_e);
				return;
			}
		}
		string text = _e.Message;
		if (_e.InnerException != null)
		{
			text = text + " ---> " + _e.InnerException.Message;
		}
		string text2 = StackTraceUtility.ExtractStringFromException(_e);
		text2 = text2.Substring(text2.IndexOf('\n') + 1);
		Log.masterLogStandalone(text, LogType.Exception, text2);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000027C0 File Offset: 0x000009C0
	public static void Out(string _format, params object[] _values)
	{
		Log.Out(string.Format(_format, _values));
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000027CE File Offset: 0x000009CE
	public static void Warning(string _format, params object[] _values)
	{
		Log.Warning(string.Format(_format, _values));
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000027DC File Offset: 0x000009DC
	public static void Error(string _format, params object[] _values)
	{
		Log.Error(string.Format(_format, _values));
	}

	// Token: 0x04000003 RID: 3
	private static readonly bool isEditor = Application.isEditor;

	// Token: 0x04000004 RID: 4
	private static readonly object lockObject = new object();

	// Token: 0x04000005 RID: 5
	private static readonly List<string> additionalOutputPaths = new List<string>();

	// Token: 0x04000006 RID: 6
	private static readonly StringBuilder logStringBuilder = new StringBuilder();

	// Token: 0x04000007 RID: 7
	private static readonly MicroStopwatch globalWatch = new MicroStopwatch();

	// Token: 0x02000006 RID: 6
	// (Invoke) Token: 0x0600001E RID: 30
	public delegate void LogCallbackDelegate(string _msg, string _trace, LogType _type);

	// Token: 0x02000007 RID: 7
	// (Invoke) Token: 0x06000022 RID: 34
	public delegate void LogCallbackExtendedDelegate(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);
}
