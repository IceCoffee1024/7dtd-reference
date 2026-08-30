using System;
using UnityEngine.Scripting;

// Token: 0x02000832 RID: 2098
[Preserve]
public class NetPackagePlayerVendingMachine : NetPackage
{
	// Token: 0x06003D9C RID: 15772 RVA: 0x00186A3D File Offset: 0x00184C3D
	public NetPackagePlayerVendingMachine Setup(PlatformUserIdentifierAbs _userId, Vector3i _position, bool _removing)
	{
		this.userId = _userId;
		this.position = _position;
		this.removing = _removing;
		return this;
	}

	// Token: 0x06003D9D RID: 15773 RVA: 0x00186A55 File Offset: 0x00184C55
	public override void read(PooledBinaryReader _reader)
	{
		this.userId = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		this.position = new Vector3i(_reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32());
		this.removing = _reader.ReadBoolean();
	}

	// Token: 0x06003D9E RID: 15774 RVA: 0x00186A90 File Offset: 0x00184C90
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.userId.ToStream(_writer, false);
		_writer.Write(this.position.x);
		_writer.Write(this.position.y);
		_writer.Write(this.position.z);
		_writer.Write(this.removing);
	}

	// Token: 0x06003D9F RID: 15775 RVA: 0x00186AF0 File Offset: 0x00184CF0
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

	// Token: 0x06003DA0 RID: 15776 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040031E3 RID: 12771
	public PlatformUserIdentifierAbs userId;

	// Token: 0x040031E4 RID: 12772
	public Vector3i position;

	// Token: 0x040031E5 RID: 12773
	public bool removing;
}
