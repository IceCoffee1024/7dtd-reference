using System;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000807 RID: 2055
[Preserve]
public class NetPackageLobbyJoin : NetPackage
{
	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06003C8E RID: 15502 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x00182AD6 File Offset: 0x00180CD6
	public NetPackageLobbyJoin Setup(PlatformLobbyId lobbyId)
	{
		this.serverLobbyId = lobbyId;
		return this;
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x00182AE0 File Offset: 0x00180CE0
	public override void read(PooledBinaryReader _br)
	{
		this.serverLobbyId = PlatformLobbyId.Read(_br);
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x00182AEE File Offset: 0x00180CEE
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.serverLobbyId.Write(_bw);
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x00182B04 File Offset: 0x00180D04
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

	// Token: 0x06003C93 RID: 15507 RVA: 0x00182BF0 File Offset: 0x00180DF0
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

	// Token: 0x0400312F RID: 12591
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId serverLobbyId;
}
