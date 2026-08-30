using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace HarmonyLib
{
	// Token: 0x02000053 RID: 83
	[Obsolete("Use HarmonyFileLog instead")]
	public static class FileLog
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000E8DB File Offset: 0x0000CADB
		public static StreamWriter LogWriter { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000E8E4 File Offset: 0x0000CAE4
		public static string LogPath
		{
			get
			{
				object obj = FileLog.fileLock;
				string logPath;
				lock (obj)
				{
					if (!FileLog._logPathInited)
					{
						FileLog._logPathInited = true;
						string environmentVariable = Environment.GetEnvironmentVariable("HARMONY_NO_LOG");
						if (!string.IsNullOrEmpty(environmentVariable))
						{
							return null;
						}
						FileLog._logPath = Environment.GetEnvironmentVariable("HARMONY_LOG_FILE");
						if (string.IsNullOrEmpty(FileLog._logPath))
						{
							string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
							Directory.CreateDirectory(folderPath);
							FileLog._logPath = Path.Combine(folderPath, "harmony.log.txt");
						}
						else
						{
							FileLog._logPath += ".obsolete";
						}
					}
					logPath = FileLog._logPath;
				}
				return logPath;
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000E99C File Offset: 0x0000CB9C
		private static string IndentString()
		{
			return new string(FileLog.indentChar, FileLog.indentLevel);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000E9B0 File Offset: 0x0000CBB0
		public static void ChangeIndent(int delta)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.indentLevel = Math.Max(0, FileLog.indentLevel + delta);
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000E9FC File Offset: 0x0000CBFC
		public static void LogBuffered(string str)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer.Add(FileLog.IndentString() + str);
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000EA4C File Offset: 0x0000CC4C
		public static void LogBuffered(List<string> strings)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer.AddRange(strings);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000EA90 File Offset: 0x0000CC90
		public static List<string> GetBuffer(bool clear)
		{
			object obj = FileLog.fileLock;
			List<string> list2;
			lock (obj)
			{
				List<string> list = FileLog.buffer;
				if (clear)
				{
					FileLog.buffer = new List<string>();
				}
				list2 = list;
			}
			return list2;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000EAE0 File Offset: 0x0000CCE0
		public static void SetBuffer(List<string> buffer)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer = buffer;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000EB20 File Offset: 0x0000CD20
		public static void FlushBuffer()
		{
			if (FileLog.LogWriter != null)
			{
				foreach (string text in FileLog.buffer)
				{
					FileLog.LogWriter.WriteLine(text);
				}
				FileLog.buffer.Clear();
				return;
			}
			if (FileLog.LogPath == null)
			{
				return;
			}
			object obj = FileLog.fileLock;
			lock (obj)
			{
				if (FileLog.buffer.Count > 0)
				{
					using (StreamWriter streamWriter = File.AppendText(FileLog.LogPath))
					{
						foreach (string text2 in FileLog.buffer)
						{
							streamWriter.WriteLine(text2);
						}
						FileLog.buffer.Clear();
					}
				}
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000EC3C File Offset: 0x0000CE3C
		public static void Log(string str)
		{
			if (FileLog.LogWriter != null)
			{
				FileLog.LogWriter.WriteLine(FileLog.IndentString() + str);
				return;
			}
			if (FileLog.LogPath == null)
			{
				return;
			}
			object obj = FileLog.fileLock;
			lock (obj)
			{
				using (StreamWriter streamWriter = File.AppendText(FileLog.LogPath))
				{
					streamWriter.WriteLine(FileLog.IndentString() + str);
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000ECCC File Offset: 0x0000CECC
		public static void Debug(string str)
		{
			if (Harmony.DEBUG)
			{
				FileLog.Log(str);
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		public static void Reset()
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 2);
				defaultInterpolatedStringHandler.AppendFormatted(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
				defaultInterpolatedStringHandler.AppendFormatted<char>(Path.DirectorySeparatorChar);
				defaultInterpolatedStringHandler.AppendLiteral("harmony.log.txt");
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				File.Delete(text);
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000ED54 File Offset: 0x0000CF54
		public unsafe static void LogBytes(long ptr, int len)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				byte* ptr2 = ptr;
				string text = "";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				for (int i = 1; i <= len; i++)
				{
					if (text.Length == 0)
					{
						text = "#  ";
					}
					string text2 = text;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
					defaultInterpolatedStringHandler.AppendFormatted<byte>(*ptr2, "X2");
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					text = text2 + defaultInterpolatedStringHandler.ToStringAndClear();
					if (i > 1 || len == 1)
					{
						if (i % 8 == 0 || i == len)
						{
							FileLog.Log(text);
							text = "";
						}
						else if (i % 4 == 0)
						{
							text += " ";
						}
					}
					ptr2++;
				}
				byte[] array = new byte[len];
				Marshal.Copy((IntPtr)ptr, array, 0, len);
				MD5 md = MD5.Create();
				byte[] array2 = md.ComputeHash(array);
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < array2.Length; j++)
				{
					stringBuilder.Append(array2[j].ToString("X2"));
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
				defaultInterpolatedStringHandler.AppendLiteral("HASH: ");
				defaultInterpolatedStringHandler.AppendFormatted<StringBuilder>(stringBuilder);
				FileLog.Log(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x040000C2 RID: 194
		private static readonly object fileLock = new object();

		// Token: 0x040000C3 RID: 195
		private static bool _logPathInited;

		// Token: 0x040000C4 RID: 196
		private static string _logPath;

		// Token: 0x040000C6 RID: 198
		public static char indentChar = '\t';

		// Token: 0x040000C7 RID: 199
		public static int indentLevel = 0;

		// Token: 0x040000C8 RID: 200
		private static List<string> buffer = new List<string>();
	}
}
