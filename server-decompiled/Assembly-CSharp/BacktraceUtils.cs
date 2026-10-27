using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using Backtrace.Unity;
using Backtrace.Unity.Model;
using Backtrace.Unity.Types;
using Platform;
using UniLinq;
using UnityEngine;

// Token: 0x020013CB RID: 5067
public static class BacktraceUtils
{
	// Token: 0x1700130C RID: 4876
	// (get) Token: 0x06009F47 RID: 40775 RVA: 0x003C1AB9 File Offset: 0x003BFCB9
	public static bool Initialized
	{
		get
		{
			return BacktraceUtils.s_Configuration != null;
		}
	}

	// Token: 0x1700130D RID: 4877
	// (get) Token: 0x06009F48 RID: 40776 RVA: 0x003C1AC6 File Offset: 0x003BFCC6
	public static bool Enabled
	{
		get
		{
			return BacktraceUtils.s_BacktraceEnabled && BacktraceUtils.s_VersionEnabled;
		}
	}

	// Token: 0x1700130E RID: 4878
	// (get) Token: 0x06009F49 RID: 40777 RVA: 0x003C1AD6 File Offset: 0x003BFCD6
	public static bool BugReportFeature
	{
		get
		{
			return BacktraceUtils.Enabled && BacktraceUtils.s_BugReportFeature;
		}
	}

	// Token: 0x1700130F RID: 4879
	// (get) Token: 0x06009F4A RID: 40778 RVA: 0x003C1AE6 File Offset: 0x003BFCE6
	public static bool BugReportAttachSaveFeature
	{
		get
		{
			return BacktraceUtils.Enabled && BacktraceUtils.s_BugReportFeature && BacktraceUtils.s_BugReportAttachSaveFeature;
		}
	}

	// Token: 0x17001310 RID: 4880
	// (get) Token: 0x06009F4B RID: 40779 RVA: 0x003C1AFD File Offset: 0x003BFCFD
	public static bool BugReportAttachWholeWorldFeature
	{
		get
		{
			return BacktraceUtils.s_BugReportFeature && BacktraceUtils.s_BugReportAttachSaveFeature && BacktraceUtils.s_BugReportAttachWholeWorldFeature;
		}
	}

	// Token: 0x06009F4C RID: 40780 RVA: 0x003C1B14 File Offset: 0x003BFD14
	public static void InitializeBacktrace()
	{
		Log.Out("[BACKTRACE] Initialize");
		BacktraceUtils.InitializeConfiguration();
		BacktraceUtils.InitializeBacktraceClient();
		Log.Out("[BACKTRACE] Initialized");
	}

	// Token: 0x06009F4D RID: 40781 RVA: 0x003C1B34 File Offset: 0x003BFD34
	public static void BacktraceUserLoggedIn(IPlatform platform)
	{
		Log.Out(string.Format("[BACKTRACE] Attempting to get User ID from platform: {0}", (platform != null) ? new EPlatformIdentifier?(platform.PlatformIdentifier) : null));
		bool flag;
		if (platform == null)
		{
			flag = null != null;
		}
		else
		{
			IUserClient user = platform.User;
			flag = ((user != null) ? user.PlatformUserId : null) != null;
		}
		if (!flag)
		{
			Log.Out(string.Format("[BACKTRACE] {0} PlatformUserId missing at this time", (platform != null) ? new EPlatformIdentifier?(platform.PlatformIdentifier) : null));
			return;
		}
		BacktraceUtils.userIdString = platform.User.PlatformUserId.PlatformIdentifierString + "-" + platform.User.PlatformUserId.ReadablePlatformUserIdentifier;
		BacktraceUtils.RefreshUserIdAttribute();
	}

	// Token: 0x06009F4E RID: 40782 RVA: 0x003C1BEC File Offset: 0x003BFDEC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RefreshUserIdAttribute()
	{
		if (BacktraceUtils.s_BacktraceClient == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(BacktraceUtils.userIdString))
		{
			return;
		}
		BacktraceUtils.s_BacktraceClient.SetAttributes(new Dictionary<string, string> { 
		{
			"gamestats.platformuserid",
			BacktraceUtils.userIdString
		} });
		Log.Out("[BACKTRACE] Platform ID set to: \"" + BacktraceUtils.userIdString + "\"");
	}

	// Token: 0x06009F4F RID: 40783 RVA: 0x003C1C4C File Offset: 0x003BFE4C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Reset()
	{
		BacktraceUtils.s_BacktraceEnabled = false;
		BacktraceUtils.s_VersionEnabled = false;
		BacktraceUtils.s_Configuration.HandleUnhandledExceptions = false;
		BacktraceUtils.s_Configuration.Sampling = 0.01;
		BacktraceUtils.s_Configuration.CaptureNativeCrashes = true;
		BacktraceUtils.s_Configuration.MinidumpType = MiniDumpType.Normal;
		BacktraceUtils.s_Configuration.DeduplicationStrategy = DeduplicationStrategy.Default;
	}

	// Token: 0x06009F50 RID: 40784 RVA: 0x003C1CA4 File Offset: 0x003BFEA4
	public static void SendBugReport(string message, string worldName, string saveName, string worldDir = null, string saveDir = null, bool sendSave = false, string screenshotPath = null, Action<BacktraceResult> callback = null)
	{
		if (!BacktraceUtils.BugReportFeature)
		{
			Log.Out("[BACKTRACE] Backtrace bug reporting disabled by platform");
			return;
		}
		if (BacktraceUtils.s_BacktraceClient == null)
		{
			BacktraceUtils.DebugEnableBacktrace();
		}
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(screenshotPath))
		{
			list.Add(screenshotPath);
		}
		if (saveDir != null)
		{
			if (BacktraceUtils.BugReportAttachSaveFeature && sendSave)
			{
				bool flag = false;
				string text = Path.Join(PlatformApplicationManager.Application.temporaryCachePath, string.Concat(new string[] { "Save_", worldName, "_", saveName, ".zip" }));
				BacktraceUtils.SaveArchiveResult saveArchiveResult = BacktraceUtils.TryCreateSaveArchive(saveDir, text);
				if (saveArchiveResult == BacktraceUtils.SaveArchiveResult.Success)
				{
					Log.Out("[BACKTRACE] Save file path: " + text);
					list.Add(text);
				}
				else if (saveArchiveResult == BacktraceUtils.SaveArchiveResult.MissingRegions)
				{
					list.Add(text);
					List<string> list2;
					if (BacktraceUtils.TryCreateRegionArchives(worldName + "_" + saveName, saveDir, out list2))
					{
						list.AddRange(list2);
						Log.Out("[BACKTRACE] region file paths: " + string.Join(", ", list2));
						saveArchiveResult = BacktraceUtils.SaveArchiveResult.Success;
						flag = true;
					}
				}
				if (saveArchiveResult != BacktraceUtils.SaveArchiveResult.FailureToArchive && worldDir != null)
				{
					string text2 = Path.Join(PlatformApplicationManager.Application.temporaryCachePath, "/world_" + worldName + ".zip");
					if (BacktraceUtils.TryCreateWorldArchive(worldDir, text2))
					{
						list.Add(text2);
						Log.Out("[BACKTRACE] World file path: " + text2);
						saveArchiveResult = BacktraceUtils.SaveArchiveResult.Success;
						flag = true;
					}
				}
				if (saveArchiveResult == BacktraceUtils.SaveArchiveResult.Success && flag)
				{
					list = BacktraceUtils.CondenseZipFiles(Path.Join(PlatformApplicationManager.Application.temporaryCachePath, string.Concat(new string[] { "/combined_", worldName, "_", saveName, ".zip" })), list);
				}
				Log.Out("[BACKTRACE] File Sizes:");
				foreach (string text3 in list)
				{
					Log.Out(string.Format("[BACKTRACE] {0}: {1:N3}MB", text3, (double)new SdFileInfo(text3).Length / 1024.0 / 1024.0));
				}
			}
			if (worldDir != null)
			{
				string text4 = Path.Combine(worldDir, "checksums.txt");
				if (SdFile.Exists(text4))
				{
					string text5 = Path.Combine(PlatformApplicationManager.Application.temporaryCachePath, "backtraceTemp", "checksums.txt");
					Log.Out("[BACKTRACE] World file path: " + worldDir);
					if (!SdDirectory.Exists(text5))
					{
						SdDirectory.CreateDirectory(Path.GetDirectoryName(text5));
					}
					SdFile.Copy(text4, text5, true);
					list.Add(text5);
				}
				string text6 = Path.Combine(worldDir, "map_info.xml");
				if (SdFile.Exists(text6))
				{
					string text7 = Path.Combine(PlatformApplicationManager.Application.temporaryCachePath, "backtraceTemp", "map_info.xml");
					if (!SdDirectory.Exists(text7))
					{
						SdDirectory.CreateDirectory(Path.GetDirectoryName(text7));
					}
					SdFile.Copy(text6, text7, true);
					list.Add(text7);
				}
			}
		}
		if (callback != null)
		{
			callback = (Action<BacktraceResult>)Delegate.Combine(callback, new Action<BacktraceResult>(BacktraceUtils.ClearMessageTypeAttribute));
		}
		else
		{
			callback = new Action<BacktraceResult>(BacktraceUtils.ClearMessageTypeAttribute);
		}
		BacktraceUtils.SetAttribute("messagetype", "bugreport");
		BacktraceClient backtraceClient = BacktraceUtils.s_BacktraceClient;
		if (backtraceClient == null)
		{
			return;
		}
		backtraceClient.Send(message, callback, list, null);
	}

	// Token: 0x06009F51 RID: 40785 RVA: 0x003C1FFC File Offset: 0x003C01FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ClearMessageTypeAttribute(BacktraceResult _result)
	{
		BacktraceUtils.SetAttribute("messagetype", null);
	}

	// Token: 0x06009F52 RID: 40786 RVA: 0x003C200C File Offset: 0x003C020C
	public static void SendErrorReport(string messageType, string message, List<string> files = null, Dictionary<string, string> attributes = null)
	{
		BacktraceUtils.<>c__DisplayClass40_0 CS$<>8__locals1 = new BacktraceUtils.<>c__DisplayClass40_0();
		CS$<>8__locals1.attributes = attributes;
		if (BacktraceUtils.ErrorReportingDisabledMessageTypes.Contains(messageType))
		{
			Log.Out("[BACKTRACE] Opted out of reporting error of type " + messageType);
			return;
		}
		if (!BacktraceUtils.Enabled)
		{
			Log.Out("[BACKTRACE] Not reporting error of type " + messageType + ", reporting is not enabled");
			return;
		}
		if (BacktraceUtils.s_BacktraceClient == null)
		{
			BacktraceUtils.DebugEnableBacktrace();
		}
		BacktraceUtils.SetAttribute("messagetype", messageType);
		if (CS$<>8__locals1.attributes != null)
		{
			BacktraceUtils.SetAttributes(CS$<>8__locals1.attributes);
		}
		if (files != null)
		{
			BacktraceClient backtraceClient = BacktraceUtils.s_BacktraceClient;
			if (backtraceClient == null)
			{
				return;
			}
			backtraceClient.Send(message, new Action<BacktraceResult>(CS$<>8__locals1.<SendErrorReport>g__ClearAll|0), files, null);
			return;
		}
		else
		{
			BacktraceClient backtraceClient2 = BacktraceUtils.s_BacktraceClient;
			if (backtraceClient2 == null)
			{
				return;
			}
			backtraceClient2.Send(message, new Action<BacktraceResult>(CS$<>8__locals1.<SendErrorReport>g__ClearAll|0), null, null);
			return;
		}
	}

	// Token: 0x06009F53 RID: 40787 RVA: 0x003C20D4 File Offset: 0x003C02D4
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<string> CondenseZipFiles(string combinedArchivePath, List<string> attachmentPaths)
	{
		string text = Path.Join(PlatformApplicationManager.Application.temporaryCachePath, Path.GetFileNameWithoutExtension(combinedArchivePath));
		if (SdDirectory.Exists(text))
		{
			SdDirectory.Delete(text, true);
		}
		SdDirectory.CreateDirectory(text);
		List<string> list = new List<string>();
		List<string> list2 = new List<string>(attachmentPaths);
		foreach (string text2 in attachmentPaths)
		{
			if (Path.GetFileName(text2).ContainsCaseInsensitive("zip"))
			{
				string text3 = Path.Join(text, Path.GetFileNameWithoutExtension(text2)) + Path.AltDirectorySeparatorChar.ToString();
				SdDirectory.CreateDirectory(text3);
				ZipFile.ExtractToDirectory(text2, text3);
				list.Add(text2);
				list2.Remove(text2);
			}
		}
		using (Stream stream = SdFile.Open(combinedArchivePath, FileMode.Create, FileAccess.Write))
		{
			using (ZipArchive zipArchive = new ZipArchive(stream, 1))
			{
				zipArchive.CreateFromDirectory(text);
			}
		}
		SdDirectory.Delete(text, true);
		if ((double)new SdFileInfo(combinedArchivePath).Length / 1024.0 / 1024.0 <= 30.0)
		{
			Log.Out("[BACKTRACE] Created combined archive: " + combinedArchivePath);
			Log.Out("[BACKTRACE] Remove the following: " + string.Join(", ", list));
			attachmentPaths = list2;
			attachmentPaths.Add(combinedArchivePath);
			using (List<string>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text4 = enumerator.Current;
					SdFile.Delete(text4);
				}
				return attachmentPaths;
			}
		}
		Log.Out("[BACKTRACE] Combined archive is too large to upload: " + combinedArchivePath);
		SdFile.Delete(combinedArchivePath);
		return attachmentPaths;
	}

	// Token: 0x06009F54 RID: 40788 RVA: 0x003C22C8 File Offset: 0x003C04C8
	public static BacktraceUtils.SaveArchiveResult TryCreateSaveArchive(string saveDir, string archivePath)
	{
		BacktraceUtils.SaveArchiveResult saveArchiveResult;
		try
		{
			if (SdFile.Exists(archivePath))
			{
				Log.Out("[BACKTRACE] Old save archive path: {0} exists, deleting...", new object[] { archivePath });
				SdFile.Delete(archivePath);
			}
			using (Stream stream = SdFile.OpenWrite(archivePath))
			{
				using (ZipArchive zipArchive = new ZipArchive(stream, 1, false, null))
				{
					zipArchive.CreateFromDirectory(saveDir);
				}
			}
			double num = (double)new SdFileInfo(archivePath).Length / 1024.0 / 1024.0;
			if (SdFile.Exists(archivePath) && num <= 30.0)
			{
				if (new SdFileInfo(archivePath).Length != 0L)
				{
					Log.Out("[BACKTRACE] Save archive path: {0} exists, size is {1}MB, success!", new object[]
					{
						archivePath,
						num.ToString("N3")
					});
					return BacktraceUtils.SaveArchiveResult.Success;
				}
				Log.Warning("[BACKTRACE] Save archive exists: {0}, but is empty, retry", new object[] { archivePath });
				SdFile.Delete(archivePath);
			}
			else if (SdFile.Exists(archivePath))
			{
				Log.Out("[BACKTRACE] Save archive path: {0} exists, size is too big, {1}MB, deleting...", new object[]
				{
					archivePath,
					num.ToString("N3")
				});
				SdFile.Delete(archivePath);
			}
			using (ZipArchive zipArchive2 = ZipFile.Open(archivePath, 1))
			{
				SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(saveDir);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.txt", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.sdf", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.ttw", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.xml", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.7dt", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.nim", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.dat", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.ttp", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.ttp.meta", SearchOption.AllDirectories);
				zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.7rm", SearchOption.AllDirectories);
			}
			if (SdFile.Exists(archivePath))
			{
				saveArchiveResult = BacktraceUtils.SaveArchiveResult.MissingRegions;
			}
			else
			{
				saveArchiveResult = BacktraceUtils.SaveArchiveResult.FailureToArchive;
			}
		}
		catch (Exception ex)
		{
			Log.Error("[BACKTRACE]  Exception: Could not create save archive: {0}", new object[] { ex.Message });
			archivePath = null;
			saveArchiveResult = BacktraceUtils.SaveArchiveResult.FailureToArchive;
		}
		return saveArchiveResult;
	}

	// Token: 0x06009F55 RID: 40789 RVA: 0x003C2530 File Offset: 0x003C0730
	public static bool TryCreateRegionArchives(string tag, string saveDir, out List<string> archivePaths)
	{
		bool flag;
		try
		{
			archivePaths = new List<string>();
			foreach (SdFileSystemInfo sdFileSystemInfo in from info in new SdDirectoryInfo(PlatformApplicationManager.Application.temporaryCachePath).EnumerateFileSystemInfos()
				where info.Name.EndsWith("_Region.zip", StringComparison.InvariantCulture)
				select info)
			{
				try
				{
					SdFile.Delete(sdFileSystemInfo.FullName);
				}
				catch (Exception ex)
				{
					Log.Warning(string.Format("[BACKTRACE] Failed To Delete {0}, Reason {1}", sdFileSystemInfo.FullName, ex));
				}
			}
			int num = 0;
			string text = Path.Join(PlatformApplicationManager.Application.temporaryCachePath, string.Format("Save_{0}_{1}_Region.zip", tag, num));
			Log.Out("[BACKTRACE] World archive path: {0}", new object[] { text });
			SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(saveDir);
			IEnumerable<SdFileSystemInfo> enumerable = (from fileSystemInfo in sdDirectoryInfo.EnumerateFileSystemInfos("*.7rg", SearchOption.AllDirectories)
				orderby fileSystemInfo.LastWriteTime.ToFileTimeUtc()
				select fileSystemInfo).Reverse<SdFileSystemInfo>();
			if (!(from fileSystemInfo in sdDirectoryInfo.EnumerateFileSystemInfos("*.7rg", SearchOption.AllDirectories)
				orderby fileSystemInfo.LastWriteTime.ToFileTimeUtc()
				select fileSystemInfo).Reverse<SdFileSystemInfo>().Any<SdFileSystemInfo>())
			{
				enumerable = sdDirectoryInfo.EnumerateFileSystemInfos("*.7rg", SearchOption.AllDirectories);
			}
			Queue<SdFileSystemInfo> queue = new Queue<SdFileSystemInfo>(enumerable);
			while (queue.Any<SdFileSystemInfo>())
			{
				enumerable = queue;
				foreach (SdFileSystemInfo sdFileSystemInfo2 in enumerable)
				{
					SdFileInfo sdFileInfo = new SdFileInfo(sdFileSystemInfo2.FullName);
					Log.Out(string.Format("{0} File size: {1} MiB", sdFileInfo.FullName, (float)sdFileInfo.Length / 1024f * 1024f));
					if (sdFileInfo.Length > 15728640L)
					{
						Log.Warning("File too big! " + text);
					}
					else
					{
						using (Stream stream = SdFile.Open(text, FileMode.OpenOrCreate, FileAccess.ReadWrite))
						{
							using (ZipArchive zipArchive = new ZipArchive(stream, 2, false, null))
							{
								zipArchive.CreateEntryFromFile(sdFileSystemInfo2, sdFileSystemInfo2.Name, global::System.IO.Compression.CompressionLevel.Optimal);
							}
						}
						if (new SdFileInfo(text).Length > 31457280L)
						{
							Log.Warning("Archive too big! " + text);
							archivePaths.Add(text);
							break;
						}
						queue.Dequeue();
					}
				}
				archivePaths.Add(text);
				num++;
				text = Path.Join(PlatformApplicationManager.Application.temporaryCachePath, string.Format("Save_{0}_{1}_Region.zip", tag, num));
			}
			archivePaths = new List<string>(archivePaths.Distinct<string>());
			flag = true;
		}
		catch (Exception ex2)
		{
			Log.Error("[BACKTRACE] Exception: Could not create save region archive: {0}", new object[] { ex2.Message });
			archivePaths = new List<string>();
			flag = false;
		}
		return flag;
	}

	// Token: 0x06009F56 RID: 40790 RVA: 0x003C28C4 File Offset: 0x003C0AC4
	public static bool TryCreateWorldArchive(string worldDir, string archivePath)
	{
		bool flag;
		try
		{
			Log.Out("[BACKTRACE] Creating archive for GeneratedWorld {0}", new object[] { worldDir });
			if (SdFile.Exists(archivePath))
			{
				Log.Out("[BACKTRACE] Old World archive path: {0} exists, deleting...", new object[] { archivePath });
			}
			Log.Out("[BACKTRACE] World archive path: {0}. Creating archive...", new object[] { archivePath });
			if (BacktraceUtils.BugReportAttachWholeWorldFeature)
			{
				using (Stream stream = SdFile.Open(archivePath, FileMode.Create, FileAccess.ReadWrite))
				{
					using (ZipArchive zipArchive = new ZipArchive(stream, 1, false, null))
					{
						zipArchive.CreateFromDirectory(worldDir);
					}
				}
				double num = (double)new SdFileInfo(archivePath).Length / 1024.0 / 1024.0;
				if (SdFile.Exists(archivePath) && (BacktraceUtils.BugReportAttachWholeWorldFeature || num <= 30.0))
				{
					if (new SdFileInfo(archivePath).Length != 0L)
					{
						Log.Out("[BACKTRACE] World archive path: {0} exists, size is {1}MB, success!", new object[]
						{
							archivePath,
							num.ToString("N3")
						});
						return true;
					}
					Log.Warning("[BACKTRACE] World archive exists: {0}, but is empty, retry", new object[] { archivePath });
					SdFile.Delete(archivePath);
				}
				else if (SdFile.Exists(archivePath))
				{
					Log.Out("[BACKTRACE] World archive path: {0} exists, size is too big, {1}MB, deleting...", new object[]
					{
						archivePath,
						num.ToString("N3")
					});
					SdFile.Delete(archivePath);
				}
			}
			Log.Out("[BACKTRACE] Attempting to archive only required elements...", new object[] { archivePath });
			using (Stream stream2 = SdFile.Open(archivePath, FileMode.Create, FileAccess.ReadWrite))
			{
				using (ZipArchive zipArchive2 = new ZipArchive(stream2, 2, false, null))
				{
					SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(worldDir);
					zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.ttw", SearchOption.TopDirectoryOnly);
					zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.xml", SearchOption.TopDirectoryOnly);
					zipArchive2.AddSearchPattern(sdDirectoryInfo, "*.txt", SearchOption.TopDirectoryOnly);
				}
			}
			if (SdFile.Exists(archivePath))
			{
				double num2 = (double)new SdFileInfo(archivePath).Length / 1024.0 / 1024.0;
				Log.Out("[BACKTRACE] World archive path: {0}", new object[] { archivePath });
				Log.Out("[BACKTRACE] World archive File size: {0}MB", new object[] { num2.ToString("N3") });
				if (!BacktraceUtils.BugReportAttachWholeWorldFeature && num2 > 30.0)
				{
					SdFile.Delete(archivePath);
					archivePath = null;
					Log.Error("[BACKTRACE] Exception: World archive too big, deleted");
					return false;
				}
			}
			flag = true;
		}
		catch (Exception ex)
		{
			Log.Error("[BACKTRACE] Exception: Could not create world archive: {0}", new object[] { ex.Message });
			archivePath = null;
			flag = false;
		}
		return flag;
	}

	// Token: 0x06009F57 RID: 40791 RVA: 0x003C2BBC File Offset: 0x003C0DBC
	public static void DebugEnableBacktrace()
	{
		BacktraceUtils.s_BacktraceEnabled = true;
		BacktraceUtils.s_VersionEnabled = true;
		BacktraceUtils.InitializeBacktraceClient();
	}

	// Token: 0x06009F58 RID: 40792 RVA: 0x003C2BD0 File Offset: 0x003C0DD0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void InitializeBacktraceClient()
	{
		if (BacktraceUtils.Enabled)
		{
			if (BacktraceUtils.s_Configuration == null)
			{
				BacktraceUtils.InitializeConfiguration();
			}
			BacktraceUtils.s_BacktraceClient = BacktraceClient.Initialize(BacktraceUtils.s_Configuration, null, "BacktraceClient");
			BacktraceUtils.SetAttributes(new Dictionary<string, string>
			{
				{
					"svn.commit",
					BacktraceUtils.s_svncommit
				},
				{
					"game.version",
					Constants.cVersionInformation.SerializableString
				}
			});
			BacktraceClient backtraceClient = BacktraceUtils.s_BacktraceClient;
			backtraceClient.OnServerError = (Action<Exception>)Delegate.Combine(backtraceClient.OnServerError, new Action<Exception>(delegate(Exception e)
			{
				Log.Error("[BACKTRACE] Error response: " + e.Message);
			}));
			BacktraceClient backtraceClient2 = BacktraceUtils.s_BacktraceClient;
			backtraceClient2.OnClientReportLimitReached = (Action<BacktraceReport>)Delegate.Combine(backtraceClient2.OnClientReportLimitReached, new Action<BacktraceReport>(delegate(BacktraceReport e)
			{
				Log.Error("[BACKTRACE] Report Limit Reached Error: " + e.Message);
			}));
			BacktraceUtils.RefreshUserIdAttribute();
			return;
		}
		BacktraceUtils.s_BacktraceClient = null;
	}

	// Token: 0x06009F59 RID: 40793 RVA: 0x003C2CBC File Offset: 0x003C0EBC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void InitializeConfiguration()
	{
		BacktraceUtils.s_Configuration = ScriptableObject.CreateInstance<BacktraceConfiguration>();
		BacktraceUtils.s_Configuration.ServerUrl = "https://thefunpimps.sp.backtrace.io:6098/post?format=json&token=4deafd275ace1a865cc35c882f48a2d1f848c59fabea40227c1bfd84d9c794d9";
		BacktraceUtils.s_Configuration.Enabled = true;
		BacktraceUtils.s_platformString = DeviceFlag.StandaloneWindows.ToString().ToUpper();
		BacktraceUtils.Reset();
		object @lock = BacktraceUtils._lock;
		lock (@lock)
		{
			string logRoot = BacktraceUtils.GetLogRoot();
			Log.Out("[BACKTRACE] Root log path: " + logRoot);
			string text = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss.fff");
			if (Application.isEditor)
			{
				BacktraceUtils.s_PlayerLogAttachmentPath = Path.Join(logRoot, "Player-InEditor-" + text + ".log");
			}
			else
			{
				BacktraceUtils.s_PlayerLogAttachmentPath = Path.Join(logRoot, "Player-" + text + ".log");
			}
			if (SdDirectory.Exists(logRoot))
			{
				List<string> listOfLogPaths = BacktraceUtils.GetListOfLogPaths();
				listOfLogPaths.Add(BacktraceUtils.s_PlayerLogAttachmentPath);
				BacktraceUtils.s_Configuration.AttachmentPaths = listOfLogPaths.ToArray();
			}
			else
			{
				SdDirectory.CreateDirectory(logRoot);
				BacktraceUtils.s_Configuration.AttachmentPaths = new string[] { BacktraceUtils.s_PlayerLogAttachmentPath };
			}
			Log.AddOutputPath(BacktraceUtils.s_PlayerLogAttachmentPath);
			Log.Out("[BACKTRACE] Player log path: " + BacktraceUtils.s_PlayerLogAttachmentPath);
			if (!SdFile.Exists(BacktraceUtils.s_PlayerLogAttachmentPath))
			{
				SdFile.CreateText(BacktraceUtils.s_PlayerLogAttachmentPath);
			}
		}
	}

	// Token: 0x06009F5A RID: 40794 RVA: 0x003C2E48 File Offset: 0x003C1048
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<string> GetListOfLogPaths()
	{
		List<string> list = null;
		string logRoot = BacktraceUtils.GetLogRoot();
		if (SdDirectory.Exists(logRoot))
		{
			int num = Directory.EnumerateFiles(logRoot).Count<string>((string enumerateFile) => enumerateFile.Contains("Player") && enumerateFile.EndsWith(".log"));
			list = new List<string>(num + 1);
			int num2 = num;
			foreach (string text in Directory.EnumerateFiles(logRoot, "Player*.log", SearchOption.TopDirectoryOnly).OrderBy<string, string>((string sort) => sort, StringComparer.InvariantCulture))
			{
				if (text.Contains("Player") && text.EndsWith(".log"))
				{
					if (num2 <= 3)
					{
						list.Add(text);
						Log.Out("[BACKTRACE] Using log: " + text);
					}
					else
					{
						num2--;
						SdFile.Delete(text);
						Log.Out("[BACKTRACE] Deleted old log: " + text);
					}
				}
			}
		}
		GC.Collect();
		return list;
	}

	// Token: 0x06009F5B RID: 40795 RVA: 0x003C2F64 File Offset: 0x003C1164
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetLogRoot()
	{
		return Path.GetFullPath(Path.Join(GameIO.GetDefaultPersistentDataPath(), "BacktraceLogs"));
	}

	// Token: 0x06009F5C RID: 40796 RVA: 0x003C2F84 File Offset: 0x003C1184
	public static void StartStatisticsUpdate()
	{
		DebugGameStats.StartStatisticsUpdate(new DebugGameStats.StatisticsUpdatedCallback(BacktraceUtils.SetAttributes));
	}

	// Token: 0x06009F5D RID: 40797 RVA: 0x003C2F97 File Offset: 0x003C1197
	public static void SetAttributes(Dictionary<string, string> _attributesDictionary)
	{
		BacktraceClient backtraceClient = BacktraceUtils.s_BacktraceClient;
		if (backtraceClient == null)
		{
			return;
		}
		backtraceClient.SetAttributes(_attributesDictionary);
	}

	// Token: 0x06009F5E RID: 40798 RVA: 0x003C2FA9 File Offset: 0x003C11A9
	public static void SetAttribute(string _attributeName, string _attributeValue)
	{
		BacktraceClient backtraceClient = BacktraceUtils.s_BacktraceClient;
		if (backtraceClient == null)
		{
			return;
		}
		backtraceClient.SetAttributes(new Dictionary<string, string> { { _attributeName, _attributeValue } });
	}

	// Token: 0x06009F5F RID: 40799 RVA: 0x003C2FC8 File Offset: 0x003C11C8
	public static void UpdateConfig(XmlFile _xmlFile)
	{
		if (BacktraceUtils.s_Configuration == null)
		{
			return;
		}
		BacktraceUtils.Reset();
		XElement root = _xmlFile.XmlDoc.Root;
		if (root == null)
		{
			Log.Out("Could not load Backtrace Config from file " + _xmlFile.Filename + ".");
			return;
		}
		BacktraceUtils.s_VersionEnabled = true;
		foreach (XElement xelement in root.Elements("platform"))
		{
			BacktraceUtils.ParsePlatform(xelement);
		}
		BacktraceUtils.InitializeBacktraceClient();
		Log.Out(string.Format("[BACKTRACE] Configuration refreshed from XML: Enabled {0}", BacktraceUtils.Enabled));
		Log.Out("[BACKTRACE] Bug reporting: " + (BacktraceUtils.s_BugReportFeature ? "Enabled" : "Disabled"));
		Log.Out("[BACKTRACE] Bug reporting attach save feature: " + (BacktraceUtils.s_BugReportAttachSaveFeature ? "Enabled" : "Disabled"));
		Log.Out("[BACKTRACE] Bug reporting attach entire world feature: " + (BacktraceUtils.s_BugReportAttachWholeWorldFeature ? "Enabled" : "Disabled"));
	}

	// Token: 0x06009F60 RID: 40800 RVA: 0x003C30E4 File Offset: 0x003C12E4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParsePlatform(XElement _element)
	{
		string text;
		if (!_element.TryGetAttribute("name", out text))
		{
			throw new XmlLoadException("BacktraceConfig", _element, "Platform node attribute 'name' missing");
		}
		if (text.ToUpper() == "DEFAULT" || text.ToUpper() == BacktraceUtils.s_platformString)
		{
			foreach (XElement xelement in _element.Elements())
			{
				BacktraceUtils.ParsePlatformElement(xelement);
			}
		}
	}

	// Token: 0x06009F61 RID: 40801 RVA: 0x003C3178 File Offset: 0x003C1378
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParsePlatformElement(XElement _element)
	{
		if (_element.Name == BacktraceUtils.enabled)
		{
			string text;
			BacktraceUtils.s_BacktraceEnabled = _element.TryGetAttribute("value", out text) && text.Equals("true");
			return;
		}
		if (_element.Name == BacktraceUtils.excludedversions)
		{
			string text2;
			_element.TryGetAttribute("value", out text2);
			if (BacktraceUtils.s_VersionEnabled)
			{
				foreach (string text3 in text2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					BacktraceUtils.s_VersionEnabled = !Constants.cVersionInformation.SerializableString.ContainsCaseInsensitive(text3);
					if (!BacktraceUtils.s_VersionEnabled)
					{
						Log.Out("[BACKTRACE] version " + text3 + " is Excluded, and matches current version, backtrace disabled");
						return;
					}
				}
				return;
			}
		}
		else if (_element.Name == BacktraceUtils.sampling)
		{
			string text4;
			_element.TryGetAttribute("value", out text4);
			double num;
			if (double.TryParse(text4, out num))
			{
				BacktraceUtils.s_Configuration.Sampling = num;
				return;
			}
		}
		else if (_element.Name == BacktraceUtils.deduplicationStrategy)
		{
			string text5;
			_element.TryGetAttribute("value", out text5);
			DeduplicationStrategy deduplicationStrategy;
			if (Enum.TryParse<DeduplicationStrategy>(text5, out deduplicationStrategy))
			{
				BacktraceUtils.s_Configuration.DeduplicationStrategy = deduplicationStrategy;
				return;
			}
		}
		else if (_element.Name == BacktraceUtils.minidumptype)
		{
			string text6;
			_element.TryGetAttribute("value", out text6);
			MiniDumpType miniDumpType;
			if (Enum.TryParse<MiniDumpType>(text6, out miniDumpType))
			{
				BacktraceUtils.s_Configuration.MinidumpType = miniDumpType;
				return;
			}
		}
		else
		{
			if (_element.Name == BacktraceUtils.enablemetricssupport)
			{
				string text7;
				BacktraceUtils.s_Configuration.EnableMetricsSupport = _element.TryGetAttribute("value", out text7) && text7.Equals("true");
				return;
			}
			if (_element.Name == BacktraceUtils.bugreporting)
			{
				string text8;
				BacktraceUtils.s_BugReportFeature = _element.TryGetAttribute("value", out text8) && text8.Equals("true");
				Log.Out("[BACKTRACE] Bug reporting " + (BacktraceUtils.s_BugReportFeature ? "Enabled" : "Disabled") + " with save uploading: " + (BacktraceUtils.s_BugReportAttachSaveFeature ? "Enabled" : "Disabled"));
				return;
			}
			if (_element.Name == BacktraceUtils.attachsaves)
			{
				string text9;
				BacktraceUtils.s_BugReportAttachSaveFeature = _element.TryGetAttribute("value", out text9) && text9.Equals("true");
				Log.Out("[BACKTRACE] Save Attaching Feature: " + (BacktraceUtils.s_BugReportAttachSaveFeature ? "Enabled" : "Disabled"));
				return;
			}
			if (_element.Name == BacktraceUtils.attachentireworld)
			{
				string text10;
				BacktraceUtils.s_BugReportAttachWholeWorldFeature = _element.TryGetAttribute("value", out text10) && text10.Equals("true");
				Log.Out("[BACKTRACE] Entire World Attaching Feature: " + (BacktraceUtils.s_BugReportAttachWholeWorldFeature ? "Enabled" : "Disabled"));
				return;
			}
			if (_element.Name == BacktraceUtils.errorreportdisabledmessagetypes)
			{
				string text11;
				_element.TryGetAttribute("value", out text11);
				string[] array2 = text11.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				Log.Out("[BACKTRACE] Disabling Error Report Message Types: " + string.Join(", ", array2));
				BacktraceUtils.ErrorReportingDisabledMessageTypes.AddRange(array2);
			}
		}
	}

	// Token: 0x040078AF RID: 30895
	[PublicizedFrom(EAccessModifier.Private)]
	public const long BacktraceFileSizeLimitMebibyte = 30L;

	// Token: 0x040078B0 RID: 30896
	[PublicizedFrom(EAccessModifier.Private)]
	public static BacktraceClient s_BacktraceClient;

	// Token: 0x040078B1 RID: 30897
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object _lock = new object();

	// Token: 0x040078B2 RID: 30898
	[PublicizedFrom(EAccessModifier.Private)]
	public static string s_PlayerLogAttachmentPath;

	// Token: 0x040078B3 RID: 30899
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string s_svncommit = "93874";

	// Token: 0x040078B4 RID: 30900
	[PublicizedFrom(EAccessModifier.Private)]
	public static string s_platformString;

	// Token: 0x040078B5 RID: 30901
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName enabled = "enabled";

	// Token: 0x040078B6 RID: 30902
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName excludedversions = "excludedversions";

	// Token: 0x040078B7 RID: 30903
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName sampling = "sampling";

	// Token: 0x040078B8 RID: 30904
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName deduplicationStrategy = "deduplicationstrategy";

	// Token: 0x040078B9 RID: 30905
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName minidumptype = "minidumptype";

	// Token: 0x040078BA RID: 30906
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName enablemetricssupport = "enablemetricssupport";

	// Token: 0x040078BB RID: 30907
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName bugreporting = "bugreportfeature";

	// Token: 0x040078BC RID: 30908
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName attachsaves = "bugreportattachsaves";

	// Token: 0x040078BD RID: 30909
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName attachentireworld = "bugreportattachentireworld";

	// Token: 0x040078BE RID: 30910
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XName errorreportdisabledmessagetypes = "errorreportdisabledmessagetypes";

	// Token: 0x040078BF RID: 30911
	[PublicizedFrom(EAccessModifier.Private)]
	public static BacktraceConfiguration s_Configuration;

	// Token: 0x040078C0 RID: 30912
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_BacktraceEnabled = false;

	// Token: 0x040078C1 RID: 30913
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_VersionEnabled = false;

	// Token: 0x040078C2 RID: 30914
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_BugReportFeature = false;

	// Token: 0x040078C3 RID: 30915
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_BugReportAttachSaveFeature = false;

	// Token: 0x040078C4 RID: 30916
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_BugReportAttachWholeWorldFeature = false;

	// Token: 0x040078C5 RID: 30917
	public static List<string> ErrorReportingDisabledMessageTypes = new List<string>();

	// Token: 0x040078C6 RID: 30918
	[PublicizedFrom(EAccessModifier.Private)]
	public static string userIdString = string.Empty;

	// Token: 0x020013CC RID: 5068
	public enum SaveArchiveResult
	{
		// Token: 0x040078C8 RID: 30920
		FailureToArchive,
		// Token: 0x040078C9 RID: 30921
		Success,
		// Token: 0x040078CA RID: 30922
		MissingRegions
	}
}
