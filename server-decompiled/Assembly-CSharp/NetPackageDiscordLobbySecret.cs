using System;
using UnityEngine.Scripting;

// Token: 0x02000339 RID: 825
[Preserve]
public class NetPackageDiscordLobbySecret : NetPackage
{
	// Token: 0x060017E9 RID: 6121 RVA: 0x000893BE File Offset: 0x000875BE
	public NetPackageDiscordLobbySecret Setup(DiscordManager.ELobbyType _lobbyType, string _lobbySecret)
	{
		this.lobbyType = _lobbyType;
		this.lobbySecret = _lobbySecret;
		return this;
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x060017EA RID: 6122 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000893CF File Offset: 0x000875CF
	public override void read(PooledBinaryReader _reader)
	{
		this.lobbyType = (DiscordManager.ELobbyType)_reader.ReadByte();
		this.lobbySecret = StreamUtils.ReadString(_reader);
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000893E9 File Offset: 0x000875E9
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.lobbyType);
		StreamUtils.Write(_writer, this.lobbySecret);
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x0008940A File Offset: 0x0008760A
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		DiscordManager.Instance.ReceivedLobbySecret(this.lobbyType, this.lobbySecret);
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x00089422 File Offset: 0x00087622
	public override int GetLength()
	{
		int num = 3;
		string text = this.lobbySecret;
		return num + ((text != null) ? text.Length : 0);
	}

	// Token: 0x04000F7C RID: 3964
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.ELobbyType lobbyType;

	// Token: 0x04000F7D RID: 3965
	[PublicizedFrom(EAccessModifier.Private)]
	public string lobbySecret;
}
