using System;
using System.IO;
using UnityEngine;

// Token: 0x02000794 RID: 1940
public static class NetPackageLogger
{
	// Token: 0x060039B3 RID: 14771 RVA: 0x001789D4 File Offset: 0x00176BD4
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

	// Token: 0x060039B4 RID: 14772 RVA: 0x00178A34 File Offset: 0x00176C34
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

	// Token: 0x060039B5 RID: 14773 RVA: 0x00178AA8 File Offset: 0x00176CA8
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

	// Token: 0x060039B6 RID: 14774 RVA: 0x00178B88 File Offset: 0x00176D88
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

	// Token: 0x04002F2D RID: 12077
	[PublicizedFrom(EAccessModifier.Private)]
	public static int opened;

	// Token: 0x04002F2E RID: 12078
	[PublicizedFrom(EAccessModifier.Private)]
	public static StreamWriter logFileStream;

	// Token: 0x04002F2F RID: 12079
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool logEnabled;

	// Token: 0x04002F30 RID: 12080
	[PublicizedFrom(EAccessModifier.Private)]
	public static string logFilePathPrefix;
}
