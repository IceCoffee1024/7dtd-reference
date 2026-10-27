using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007EA RID: 2026
[Preserve]
public class NetPackageEntityLookAt : NetPackageEntityTargeted
{
	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06003BC5 RID: 15301 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x00180939 File Offset: 0x0017EB39
	public NetPackageEntityLookAt Setup(int _entityId, Vector3 _lookAtPosition)
	{
		base.Setup(_entityId);
		this.lookAtPosition = _lookAtPosition;
		return this;
	}

	// Token: 0x06003BC7 RID: 15303 RVA: 0x0018094A File Offset: 0x0017EB4A
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.lookAtPosition = new Vector3((float)_reader.ReadInt32(), (float)_reader.ReadInt32(), (float)_reader.ReadInt32());
	}

	// Token: 0x06003BC8 RID: 15304 RVA: 0x00180973 File Offset: 0x0017EB73
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.lookAtPosition.x);
		_writer.Write((int)this.lookAtPosition.y);
		_writer.Write((int)this.lookAtPosition.z);
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x001809B4 File Offset: 0x0017EBB4
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

	// Token: 0x040030A0 RID: 12448
	public Vector3 lookAtPosition;
}
