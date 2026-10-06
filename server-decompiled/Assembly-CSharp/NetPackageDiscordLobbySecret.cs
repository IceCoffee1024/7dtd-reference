using System;
using UnityEngine.Scripting;

// Token: 0x0200033C RID: 828
[Preserve]
public class NetPackageDiscordLobbySecret : NetPackage
{
	// Token: 0x060017E5 RID: 6117 RVA: 0x00089651 File Offset: 0x00087851
	public NetPackageDiscordLobbySecret Setup(DiscordManager.ELobbyType _lobbyType, string _lobbySecret)
	{
		this.lobbyType = _lobbyType;
		this.lobbySecret = _lobbySecret;
		return this;
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x060017E6 RID: 6118 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x00089662 File Offset: 0x00087862
	public override void read(PooledBinaryReader _reader)
	{
		this.lobbyType = (DiscordManager.ELobbyType)_reader.ReadByte();
		this.lobbySecret = StreamUtils.ReadString(_reader);
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x0008967C File Offset: 0x0008787C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.lobbyType);
		StreamUtils.Write(_writer, this.lobbySecret);
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x0008969D File Offset: 0x0008789D
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		DiscordManager.Instance.ReceivedLobbySecret(this.lobbyType, this.lobbySecret);
	}

	// Token: 0x04000F75 RID: 3957
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.ELobbyType lobbyType;

	// Token: 0x04000F76 RID: 3958
	[PublicizedFrom(EAccessModifier.Private)]
	public string lobbySecret;
}
