using System;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000818 RID: 2072
[Preserve]
public class NetPackagePackageIds : NetPackage
{
	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06003CCA RID: 15562 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06003CCB RID: 15563 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06003CCC RID: 15564 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x00184364 File Offset: 0x00182564
	public NetPackagePackageIds Setup()
	{
		this.toSendCount = NetPackageManager.KnownPackageCount;
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		bool? flag;
		if (crossplatformPlatform == null)
		{
			flag = null;
		}
		else
		{
			IAntiCheatServer antiCheatServer = crossplatformPlatform.AntiCheatServer;
			flag = ((antiCheatServer != null) ? new bool?(antiCheatServer.ServerEacEnabled()) : null);
		}
		bool? flag2 = flag;
		this.serverUseEAC = flag2.GetValueOrDefault();
		if (this.serverUseEAC)
		{
			IPlatform crossplatformPlatform2 = PlatformManager.CrossplatformPlatform;
			bool? flag3;
			if (crossplatformPlatform2 == null)
			{
				flag3 = null;
			}
			else
			{
				IAntiCheatServer antiCheatServer2 = crossplatformPlatform2.AntiCheatServer;
				flag3 = ((antiCheatServer2 != null) ? new bool?(antiCheatServer2.GetHostUserIdAndToken(out this.hostUserAndToken)) : null);
			}
			flag2 = flag3;
			this.hasHostUserAndToken = flag2.GetValueOrDefault();
		}
		return this;
	}

	// Token: 0x06003CCE RID: 15566 RVA: 0x0018440C File Offset: 0x0018260C
	public override void read(PooledBinaryReader _reader)
	{
		this.compatVersion = VersionInformation.Read(_reader);
		int num = _reader.ReadInt32();
		this.mappings = new string[num];
		for (int i = 0; i < num; i++)
		{
			this.mappings[i] = _reader.ReadString();
		}
		this.serverUseEAC = _reader.ReadBoolean();
		this.hasHostUserAndToken = _reader.ReadBoolean();
		if (this.hasHostUserAndToken)
		{
			this.hostUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_reader, false, true), _reader.ReadString());
		}
	}

	// Token: 0x06003CCF RID: 15567 RVA: 0x0018448C File Offset: 0x0018268C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		Constants.cVersionInformation.Write(_writer);
		Type[] packageMappings = NetPackageManager.PackageMappings;
		_writer.Write(packageMappings.Length);
		foreach (Type type in packageMappings)
		{
			_writer.Write(type.Name);
		}
		_writer.Write(this.serverUseEAC);
		_writer.Write(this.hasHostUserAndToken);
		if (this.hasHostUserAndToken)
		{
			this.hostUserAndToken.Item1.ToStream(_writer, true);
			_writer.Write(this.hostUserAndToken.Item2 ?? "");
		}
	}

	// Token: 0x06003CD0 RID: 15568 RVA: 0x00184528 File Offset: 0x00182728
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.compatVersion.EqualsMinor(Constants.cVersionInformation))
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.VersionMismatch;
			int num = 0;
			string longStringNoBuild = this.compatVersion.LongStringNoBuild;
			GameUtils.KickPlayerData kickPlayerData = new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), longStringNoBuild);
			GameManager.Instance.ShowMessageServerAuthFailed(kickPlayerData.LocalizedMessage());
			return;
		}
		NetPackageManager.IdMappingsReceived(this.mappings);
		if (this.serverUseEAC)
		{
			IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
			if (antiCheatClient == null || !antiCheatClient.ClientAntiCheatEnabled())
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
				GameUtils.KickPlayerData kickPlayerData2 = new GameUtils.KickPlayerData(GameUtils.EKickReason.EosEacViolation, 4, default(DateTime), "");
				GameManager.Instance.ShowMessageServerAuthFailed(kickPlayerData2.LocalizedMessage());
				return;
			}
			INetConnection[] connectionToServer = SingletonMonoBehaviour<ConnectionManager>.Instance.GetConnectionToServer();
			for (int i = 0; i < connectionToServer.Length; i++)
			{
				connectionToServer[i].SetEncryptionModule(PlatformManager.MultiPlatform.AntiCheatClient);
			}
			PlatformManager.MultiPlatform.AntiCheatClient.ConnectToServer(this.hostUserAndToken, delegate
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendLogin();
			}, delegate(string errorMessage)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
				GameUtils.KickPlayerData kickPlayerData3 = new GameUtils.KickPlayerData(GameUtils.EKickReason.CrossPlatformAuthenticationFailed, 50, default(DateTime), errorMessage);
				GameManager.Instance.ShowMessageServerAuthFailed(kickPlayerData3.LocalizedMessage());
			});
			return;
		}
		else
		{
			if (!this.hasHostUserAndToken && (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() && Submission.Enabled)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
				GameManager.Instance.ShowMessageServerAuthFailed(Localization.Get("auth_serverEACRequired", false, null));
				return;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendLogin();
			return;
		}
	}

	// Token: 0x0400316A RID: 12650
	[PublicizedFrom(EAccessModifier.Private)]
	public int toSendCount;

	// Token: 0x0400316B RID: 12651
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] mappings;

	// Token: 0x0400316C RID: 12652
	[PublicizedFrom(EAccessModifier.Private)]
	public VersionInformation compatVersion;

	// Token: 0x0400316D RID: 12653
	[PublicizedFrom(EAccessModifier.Private)]
	public bool serverUseEAC;

	// Token: 0x0400316E RID: 12654
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasHostUserAndToken;

	// Token: 0x0400316F RID: 12655
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> hostUserAndToken;
}
