using System;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007FE RID: 2046
[Preserve]
public class NetPackageLobbyRegisterClient : NetPackage
{
	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x06003C39 RID: 15417 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C3A RID: 15418 RVA: 0x0018085D File Offset: 0x0017EA5D
	public NetPackageLobbyRegisterClient Setup(PlatformLobbyId lobbyId, bool overwriteExistingLobby)
	{
		this.lobbyId = lobbyId;
		this.overwriteExistingLobby = overwriteExistingLobby;
		return this;
	}

	// Token: 0x06003C3B RID: 15419 RVA: 0x0018086E File Offset: 0x0017EA6E
	public override void read(PooledBinaryReader _br)
	{
		this.lobbyId = PlatformLobbyId.Read(_br);
		this.overwriteExistingLobby = _br.ReadBoolean();
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x00180888 File Offset: 0x0017EA88
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.lobbyId.Write(_bw);
		_bw.Write(this.overwriteExistingLobby);
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x001808AC File Offset: 0x0017EAAC
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

	// Token: 0x06003C3E RID: 15422 RVA: 0x00180944 File Offset: 0x0017EB44
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

	// Token: 0x040030B3 RID: 12467
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId lobbyId;

	// Token: 0x040030B4 RID: 12468
	[PublicizedFrom(EAccessModifier.Private)]
	public bool overwriteExistingLobby;
}
