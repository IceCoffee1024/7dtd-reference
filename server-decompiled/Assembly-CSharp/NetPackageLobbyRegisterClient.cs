using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000808 RID: 2056
[Preserve]
public class NetPackageLobbyRegisterClient : NetPackage
{
	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06003C70 RID: 15472 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x00182C66 File Offset: 0x00180E66
	public NetPackageLobbyRegisterClient Setup(PlatformLobbyId lobbyId, bool overwriteExistingLobby)
	{
		this.lobbyId = lobbyId;
		this.overwriteExistingLobby = overwriteExistingLobby;
		return this;
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x00182C77 File Offset: 0x00180E77
	public override void read(PooledBinaryReader _br)
	{
		this.lobbyId = PlatformLobbyId.Read(_br);
		this.overwriteExistingLobby = _br.ReadBoolean();
	}

	// Token: 0x06003C73 RID: 15475 RVA: 0x00182C91 File Offset: 0x00180E91
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.lobbyId.Write(_bw);
		_bw.Write(this.overwriteExistingLobby);
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x00182CB4 File Offset: 0x00180EB4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.lobbyId.PlatformIdentifier == PlatformManager.NativePlatform.PlatformIdentifier)
		{
			return;
		}
		if (this.lobbyId.PlatformIdentifier != base.Sender.PlatformId.PlatformIdentifier)
		{
			Log.Warning(string.Format("Received {0} for lobby with platform {1} but client is from {2}. This is not permitted, lobby will not be registered", "NetPackageLobbyRegisterClient", this.lobbyId.PlatformIdentifier, base.Sender.PlatformId.PlatformIdentifier));
			return;
		}
		PlatformManager.ClientLobbyManager.RegisterLobbyClient(this.lobbyId, base.Sender, this.overwriteExistingLobby);
	}

	// Token: 0x04003120 RID: 12576
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId lobbyId;

	// Token: 0x04003121 RID: 12577
	[PublicizedFrom(EAccessModifier.Private)]
	public bool overwriteExistingLobby;
}
