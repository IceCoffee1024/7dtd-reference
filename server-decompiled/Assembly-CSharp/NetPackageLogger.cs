using System;
using System.IO;
using UnityEngine;

// Token: 0x020007A1 RID: 1953
public static class NetPackageLogger
{
	// Token: 0x06003A4D RID: 14925 RVA: 0x0017B40C File Offset: 0x0017960C
	public static void Init()
	{
		if (NetPackageLogger.logFilePathPrefix != null)
		{
			return;
		}
		if (string.IsNullOrEmpty(Application.consoleLogPath))
		{
			NetPackageLogger.logFilePathPrefix = "";
			NetPackageLogger.logEnabled = false;
			return;
		}
		NetPackageLogger.logFilePathPrefix = Path.GetDirectoryName(Application.consoleLogPath) + "/netpackages_";
		NetPackageLogger.logEnabled = GameUtils.GetLaunchArgument("debugpackages") != null;
	}

	// Token: 0x06003A4E RID: 14926 RVA: 0x0017B46C File Offset: 0x0017966C
	public static void BeginLog(bool _asServer)
	{
		if (!NetPackageLogger.logEnabled)
		{
			return;
		}
		NetPackageLogger.opened++;
		if (NetPackageLogger.logFileStream != null)
		{
			return;
		}
		NetPackageLogger.logFileStream = SdFile.CreateText(NetPackageLogger.logFilePathPrefix + (_asServer ? "S_" : "C_") + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv");
		NetPackageLogger.logFileStream.WriteLine("Time,Dir,Src/Tgt,PackageType,Chn,Len,Encrypted,Compressed,Pkg# in Msg,Pkgs in Msg");
	}

	// Token: 0x06003A4F RID: 14927 RVA: 0x0017B4E0 File Offset: 0x001796E0
	public static void LogPackage(bool _dirIsOut, ClientInfo _clientInfo, NetPackage _packageType, int _channel, int _length, bool _encrypted, bool _compressed, int _pkgNumInMsg, int _pkgsInMsg)
	{
		if (NetPackageLogger.logFileStream == null)
		{
			return;
		}
		string text = ((_clientInfo == null) ? "Server" : _clientInfo.InternalId.CombinedString);
		StreamWriter streamWriter = NetPackageLogger.logFileStream;
		lock (streamWriter)
		{
			NetPackageLogger.logFileStream.WriteLine(string.Format("{0:O},{1},{2},{3},{4},{5},{6},{7},{8},{9}", new object[]
			{
				DateTime.Now,
				_dirIsOut ? "Out" : "In",
				text,
				_packageType.GetType().Name,
				_channel,
				_length,
				_encrypted,
				_compressed,
				_pkgNumInMsg,
				_pkgsInMsg
			}));
		}
	}

	// Token: 0x06003A50 RID: 14928 RVA: 0x0017B5C0 File Offset: 0x001797C0
	public static void EndLog()
	{
		if (NetPackageLogger.logFileStream == null)
		{
			return;
		}
		if (--NetPackageLogger.opened > 0)
		{
			return;
		}
		NetPackageLogger.logFileStream.Close();
		NetPackageLogger.logFileStream = null;
	}

	// Token: 0x04002FA0 RID: 12192
	[PublicizedFrom(EAccessModifier.Private)]
	public static int opened;

	// Token: 0x04002FA1 RID: 12193
	[PublicizedFrom(EAccessModifier.Private)]
	public static StreamWriter logFileStream;

	// Token: 0x04002FA2 RID: 12194
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool logEnabled;

	// Token: 0x04002FA3 RID: 12195
	[PublicizedFrom(EAccessModifier.Private)]
	public static string logFilePathPrefix;
}
