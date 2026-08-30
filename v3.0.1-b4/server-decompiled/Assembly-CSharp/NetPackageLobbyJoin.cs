using System;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007FC RID: 2044
[Preserve]
public class NetPackageLobbyJoin : NetPackage
{
	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06003C2F RID: 15407 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x001806FE File Offset: 0x0017E8FE
	public NetPackageLobbyJoin Setup(PlatformLobbyId lobbyId)
	{
		this.serverLobbyId = lobbyId;
		return this;
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x00180708 File Offset: 0x0017E908
	public override void read(PooledBinaryReader _br)
	{
		this.serverLobbyId = PlatformLobbyId.Read(_br);
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x00180716 File Offset: 0x0017E916
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.serverLobbyId.Write(_bw);
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x0018072C File Offset: 0x0017E92C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ILobbyHost lobbyHost = PlatformManager.NativePlatform.LobbyHost;
		if (lobbyHost == null)
		{
			Log.Warning(string.Format("Unexpected {0}, no lobby host for {1}", "NetPackageLobbyJoin", PlatformManager.NativePlatform.PlatformIdentifier));
			return;
		}
		if (PlatformManager.NativePlatform.PlatformIdentifier != this.serverLobbyId.PlatformIdentifier)
		{
			Log.Warning(string.Format("Received {0} for different platform: {1}", "NetPackageLobbyJoin", this.serverLobbyId.PlatformIdentifier));
			return;
		}
		string lobbyId = lobbyHost.LobbyId;
		if (lobbyId != null && lobbyId.Equals(this.serverLobbyId.LobbyId))
		{
			Log.Out("Received NetPackageLobbyJoin with " + this.serverLobbyId.LobbyId + " but we're already in the lobby");
			return;
		}
		lobbyHost.JoinLobby(this.serverLobbyId.LobbyId, delegate(LobbyHostJoinResult joinResult)
		{
			if (!joinResult.success)
			{
				Log.Warning("Failed to join server requested lobby, this client may be out of sync with the native lobby");
			}
		});
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x00180818 File Offset: 0x0017EA18
	public override int GetLength()
	{
		Encoding utf = Encoding.UTF8;
		PlatformLobbyId platformLobbyId = this.serverLobbyId;
		if (platformLobbyId == null)
		{
			return 0;
		}
		return platformLobbyId.GetWriteLength(utf);
	}

	// Token: 0x040030B0 RID: 12464
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId serverLobbyId;
}
