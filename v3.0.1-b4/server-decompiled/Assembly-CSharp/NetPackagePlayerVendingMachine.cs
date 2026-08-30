using System;
using UnityEngine.Scripting;

// Token: 0x02000827 RID: 2087
[Preserve]
public class NetPackagePlayerVendingMachine : NetPackage
{
	// Token: 0x06003D3D RID: 15677 RVA: 0x00184665 File Offset: 0x00182865
	public NetPackagePlayerVendingMachine Setup(PlatformUserIdentifierAbs _userId, Vector3i _position, bool _removing)
	{
		this.userId = _userId;
		this.position = _position;
		this.removing = _removing;
		return this;
	}

	// Token: 0x06003D3E RID: 15678 RVA: 0x0018467D File Offset: 0x0018287D
	public override void read(PooledBinaryReader _reader)
	{
		this.userId = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		this.position = new Vector3i(_reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32());
		this.removing = _reader.ReadBoolean();
	}

	// Token: 0x06003D3F RID: 15679 RVA: 0x001846B8 File Offset: 0x001828B8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.userId.ToStream(_writer, false);
		_writer.Write(this.position.x);
		_writer.Write(this.position.y);
		_writer.Write(this.position.z);
		_writer.Write(this.removing);
	}

	// Token: 0x06003D40 RID: 15680 RVA: 0x00184718 File Offset: 0x00182918
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

	// Token: 0x06003D41 RID: 15681 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003164 RID: 12644
	public PlatformUserIdentifierAbs userId;

	// Token: 0x04003165 RID: 12645
	public Vector3i position;

	// Token: 0x04003166 RID: 12646
	public bool removing;
}
