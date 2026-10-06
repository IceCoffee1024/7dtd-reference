using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000806 RID: 2054
[Preserve]
public class NetPackageLobbyJoin : NetPackage
{
	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06003C67 RID: 15463 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x00182B2E File Offset: 0x00180D2E
	public NetPackageLobbyJoin Setup(PlatformLobbyId lobbyId)
	{
		this.serverLobbyId = lobbyId;
		return this;
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x00182B38 File Offset: 0x00180D38
	public override void read(PooledBinaryReader _br)
	{
		this.serverLobbyId = PlatformLobbyId.Read(_br);
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x00182B46 File Offset: 0x00180D46
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.serverLobbyId.Write(_bw);
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x00182B5C File Offset: 0x00180D5C
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

	// Token: 0x0400311D RID: 12573
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId serverLobbyId;
}
