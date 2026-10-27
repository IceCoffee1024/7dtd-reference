using System;
using UnityEngine.Scripting;

// Token: 0x0200083D RID: 2109
[Preserve]
public class NetPackagePlayerVendingMachine : NetPackage
{
	// Token: 0x06003D79 RID: 15737 RVA: 0x00187679 File Offset: 0x00185879
	public NetPackagePlayerVendingMachine Setup(PlatformUserIdentifierAbs _userId, Vector3i _position, bool _removing)
	{
		this.userId = _userId;
		this.position = _position;
		this.removing = _removing;
		return this;
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x00187691 File Offset: 0x00185891
	public override void read(PooledBinaryReader _reader)
	{
		this.userId = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		this.position = new Vector3i(_reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32());
		this.removing = _reader.ReadBoolean();
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x001876CC File Offset: 0x001858CC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.userId.ToStream(_writer, false);
		_writer.Write(this.position.x);
		_writer.Write(this.position.y);
		_writer.Write(this.position.z);
		_writer.Write(this.removing);
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x0018772C File Offset: 0x0018592C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		PersistentPlayerData persistentPlayerData2;
		if (this.removing)
		{
			PersistentPlayerData persistentPlayerData;
			if (_callbacks.persistentPlayers.Players.TryGetValue(this.userId, out persistentPlayerData))
			{
				persistentPlayerData.TryRemoveVendingMachinePosition(this.position);
				return;
			}
		}
		else if (_callbacks.persistentPlayers.Players.TryGetValue(this.userId, out persistentPlayerData2))
		{
			persistentPlayerData2.AddVendingMachinePosition(this.position);
		}
	}

	// Token: 0x040031EA RID: 12778
	public PlatformUserIdentifierAbs userId;

	// Token: 0x040031EB RID: 12779
	public Vector3i position;

	// Token: 0x040031EC RID: 12780
	public bool removing;
}
