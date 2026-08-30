using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D2 RID: 2002
[Preserve]
public class NetPackageEntityLookAt : NetPackageEntityTargeted
{
	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06003B2F RID: 15151 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x0017CF19 File Offset: 0x0017B119
	public NetPackageEntityLookAt Setup(int _entityId, Vector3 _lookAtPosition)
	{
		base.Setup(_entityId);
		this.lookAtPosition = _lookAtPosition;
		return this;
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x0017CF2A File Offset: 0x0017B12A
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.lookAtPosition = new Vector3((float)_reader.ReadInt32(), (float)_reader.ReadInt32(), (float)_reader.ReadInt32());
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x0017CF53 File Offset: 0x0017B153
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.lookAtPosition.x);
		_writer.Write((int)this.lookAtPosition.y);
		_writer.Write((int)this.lookAtPosition.z);
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x0017CF94 File Offset: 0x0017B194
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = (EntityAlive)_world.GetEntity(this.entityId);
		if (entityAlive != null && entityAlive.emodel != null && entityAlive.emodel.avatarController != null)
		{
			entityAlive.emodel.avatarController.SetLookPosition(this.lookAtPosition);
		}
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003008 RID: 12296
	public Vector3 lookAtPosition;
}
