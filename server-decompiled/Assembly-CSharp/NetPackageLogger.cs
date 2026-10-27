using System;
using System.IO;
using UnityEngine;

// Token: 0x020007A9 RID: 1961
public static class NetPackageLogger
{
	// Token: 0x06003A64 RID: 14948 RVA: 0x0017C3A4 File Offset: 0x0017A5A4
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

	// Token: 0x06003A65 RID: 14949 RVA: 0x0017C404 File Offset: 0x0017A604
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

	// Token: 0x06003A66 RID: 14950 RVA: 0x0017C478 File Offset: 0x0017A678
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

	// Token: 0x06003A67 RID: 14951 RVA: 0x0017C558 File Offset: 0x0017A758
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

	// Token: 0x04002FBC RID: 12220
	[PublicizedFrom(EAccessModifier.Private)]
	public static int opened;

	// Token: 0x04002FBD RID: 12221
	[PublicizedFrom(EAccessModifier.Private)]
	public static StreamWriter logFileStream;

	// Token: 0x04002FBE RID: 12222
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool logEnabled;

	// Token: 0x04002FBF RID: 12223
	[PublicizedFrom(EAccessModifier.Private)]
	public static string logFilePathPrefix;
}
