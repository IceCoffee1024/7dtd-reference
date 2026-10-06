using System;
using UnityEngine.Scripting;

// Token: 0x02000833 RID: 2099
[Preserve]
public class NetPackagePlayerVendingMachine : NetPackage
{
	// Token: 0x06003D56 RID: 15702 RVA: 0x00186575 File Offset: 0x00184775
	public NetPackagePlayerVendingMachine Setup(PlatformUserIdentifierAbs _userId, Vector3i _position, bool _removing)
	{
		this.userId = _userId;
		this.position = _position;
		this.removing = _removing;
		return this;
	}

	// Token: 0x06003D57 RID: 15703 RVA: 0x0018658D File Offset: 0x0018478D
	public override void read(PooledBinaryReader _reader)
	{
		this.userId = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		this.position = new Vector3i(_reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32());
		this.removing = _reader.ReadBoolean();
	}

	// Token: 0x06003D58 RID: 15704 RVA: 0x001865C8 File Offset: 0x001847C8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.userId.ToStream(_writer, false);
		_writer.Write(this.position.x);
		_writer.Write(this.position.y);
		_writer.Write(this.position.z);
		_writer.Write(this.removing);
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x00186628 File Offset: 0x00184828
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

	// Token: 0x040031CA RID: 12746
	public PlatformUserIdentifierAbs userId;

	// Token: 0x040031CB RID: 12747
	public Vector3i position;

	// Token: 0x040031CC RID: 12748
	public bool removing;
}
