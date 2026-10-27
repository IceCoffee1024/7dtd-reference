using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000810 RID: 2064
[Preserve]
public class NetPackageLobbyJoin : NetPackage
{
	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06003C8A RID: 15498 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x00183C32 File Offset: 0x00181E32
	public NetPackageLobbyJoin Setup(PlatformLobbyId lobbyId)
	{
		this.serverLobbyId = lobbyId;
		return this;
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x00183C3C File Offset: 0x00181E3C
	public override void read(PooledBinaryReader _br)
	{
		this.serverLobbyId = PlatformLobbyId.Read(_br);
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x00183C4A File Offset: 0x00181E4A
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.serverLobbyId.Write(_bw);
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x00183C60 File Offset: 0x00181E60
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

	// Token: 0x0400313D RID: 12605
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId serverLobbyId;
}
