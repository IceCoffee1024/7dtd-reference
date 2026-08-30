using System;
using System.IO;
using UnityEngine;

// Token: 0x0200079E RID: 1950
public static class NetPackageLogger
{
	// Token: 0x06003A0B RID: 14859 RVA: 0x0017AC54 File Offset: 0x00178E54
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

	// Token: 0x06003A0C RID: 14860 RVA: 0x0017ACB4 File Offset: 0x00178EB4
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

	// Token: 0x06003A0D RID: 14861 RVA: 0x0017AD28 File Offset: 0x00178F28
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

	// Token: 0x06003A0E RID: 14862 RVA: 0x0017AE08 File Offset: 0x00179008
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

	// Token: 0x04002FA6 RID: 12198
	[PublicizedFrom(EAccessModifier.Private)]
	public static int opened;

	// Token: 0x04002FA7 RID: 12199
	[PublicizedFrom(EAccessModifier.Private)]
	public static StreamWriter logFileStream;

	// Token: 0x04002FA8 RID: 12200
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool logEnabled;

	// Token: 0x04002FA9 RID: 12201
	[PublicizedFrom(EAccessModifier.Private)]
	public static string logFilePathPrefix;
}
