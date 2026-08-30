using System;
using UnityEngine.Scripting;

// Token: 0x02000335 RID: 821
[Preserve]
public class NetPackageDiscordLobbySecret : NetPackage
{
	// Token: 0x060017D4 RID: 6100 RVA: 0x00088D1E File Offset: 0x00086F1E
	public NetPackageDiscordLobbySecret Setup(DiscordManager.ELobbyType _lobbyType, string _lobbySecret)
	{
		this.lobbyType = _lobbyType;
		this.lobbySecret = _lobbySecret;
		return this;
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060017D5 RID: 6101 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x00088D2F File Offset: 0x00086F2F
	public override void read(PooledBinaryReader _reader)
	{
		this.lobbyType = (DiscordManager.ELobbyType)_reader.ReadByte();
		this.lobbySecret = StreamUtils.ReadString(_reader);
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x00088D49 File Offset: 0x00086F49
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.lobbyType);
		StreamUtils.Write(_writer, this.lobbySecret);
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x00088D6A File Offset: 0x00086F6A
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		DiscordManager.Instance.ReceivedLobbySecret(this.lobbyType, this.lobbySecret);
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x00088D82 File Offset: 0x00086F82
	public override int GetLength()
	{
		int num = 3;
		string text = this.lobbySecret;
		return num + ((text != null) ? text.Length : 0);
	}

	// Token: 0x04000F6B RID: 3947
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.ELobbyType lobbyType;

	// Token: 0x04000F6C RID: 3948
	[PublicizedFrom(EAccessModifier.Private)]
	public string lobbySecret;
}
