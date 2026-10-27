using System;
using UnityEngine.Scripting;

// Token: 0x0200033E RID: 830
[Preserve]
public class NetPackageDiscordLobbySecret : NetPackage
{
	// Token: 0x060017E9 RID: 6121 RVA: 0x00089885 File Offset: 0x00087A85
	public NetPackageDiscordLobbySecret Setup(DiscordManager.ELobbyType _lobbyType, string _lobbySecret)
	{
		this.lobbyType = _lobbyType;
		this.lobbySecret = _lobbySecret;
		return this;
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x060017EA RID: 6122 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x00089896 File Offset: 0x00087A96
	public override void read(PooledBinaryReader _reader)
	{
		this.lobbyType = (DiscordManager.ELobbyType)_reader.ReadByte();
		this.lobbySecret = StreamUtils.ReadString(_reader);
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000898B0 File Offset: 0x00087AB0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.lobbyType);
		StreamUtils.Write(_writer, this.lobbySecret);
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x000898D1 File Offset: 0x00087AD1
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		DiscordManager.Instance.ReceivedLobbySecret(this.lobbyType, this.lobbySecret);
	}

	// Token: 0x04000F85 RID: 3973
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.ELobbyType lobbyType;

	// Token: 0x04000F86 RID: 3974
	[PublicizedFrom(EAccessModifier.Private)]
	public string lobbySecret;
}
