using System;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000809 RID: 2057
[Preserve]
public class NetPackageLobbyRegisterClient : NetPackage
{
	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x06003C98 RID: 15512 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C99 RID: 15513 RVA: 0x00182C35 File Offset: 0x00180E35
	public NetPackageLobbyRegisterClient Setup(PlatformLobbyId lobbyId, bool overwriteExistingLobby)
	{
		this.lobbyId = lobbyId;
		this.overwriteExistingLobby = overwriteExistingLobby;
		return this;
	}

	// Token: 0x06003C9A RID: 15514 RVA: 0x00182C46 File Offset: 0x00180E46
	public override void read(PooledBinaryReader _br)
	{
		this.lobbyId = PlatformLobbyId.Read(_br);
		this.overwriteExistingLobby = _br.ReadBoolean();
	}

	// Token: 0x06003C9B RID: 15515 RVA: 0x00182C60 File Offset: 0x00180E60
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.lobbyId.Write(_bw);
		_bw.Write(this.overwriteExistingLobby);
	}

	// Token: 0x06003C9C RID: 15516 RVA: 0x00182C84 File Offset: 0x00180E84
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

	// Token: 0x06003C9D RID: 15517 RVA: 0x00182D1C File Offset: 0x00180F1C
	public override int GetLength()
	{
		Encoding utf = Encoding.UTF8;
		PlatformLobbyId platformLobbyId = this.lobbyId;
		if (platformLobbyId == null)
		{
			return 0;
		}
		return platformLobbyId.GetWriteLength(utf);
	}

	// Token: 0x04003132 RID: 12594
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId lobbyId;

	// Token: 0x04003133 RID: 12595
	[PublicizedFrom(EAccessModifier.Private)]
	public bool overwriteExistingLobby;
}
