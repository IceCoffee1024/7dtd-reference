using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000812 RID: 2066
[Preserve]
public class NetPackageLobbyRegisterClient : NetPackage
{
	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06003C93 RID: 15507 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C94 RID: 15508 RVA: 0x00183D6A File Offset: 0x00181F6A
	public NetPackageLobbyRegisterClient Setup(PlatformLobbyId lobbyId, bool overwriteExistingLobby)
	{
		this.lobbyId = lobbyId;
		this.overwriteExistingLobby = overwriteExistingLobby;
		return this;
	}

	// Token: 0x06003C95 RID: 15509 RVA: 0x00183D7B File Offset: 0x00181F7B
	public override void read(PooledBinaryReader _br)
	{
		this.lobbyId = PlatformLobbyId.Read(_br);
		this.overwriteExistingLobby = _br.ReadBoolean();
	}

	// Token: 0x06003C96 RID: 15510 RVA: 0x00183D95 File Offset: 0x00181F95
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.lobbyId.Write(_bw);
		_bw.Write(this.overwriteExistingLobby);
	}

	// Token: 0x06003C97 RID: 15511 RVA: 0x00183DB8 File Offset: 0x00181FB8
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

	// Token: 0x04003140 RID: 12608
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId lobbyId;

	// Token: 0x04003141 RID: 12609
	[PublicizedFrom(EAccessModifier.Private)]
	public bool overwriteExistingLobby;
}
