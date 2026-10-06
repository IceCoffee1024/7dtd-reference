using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E0 RID: 2016
[Preserve]
public class NetPackageEntityLookAt : NetPackageEntityTargeted
{
	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06003BA2 RID: 15266 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0017F835 File Offset: 0x0017DA35
	public NetPackageEntityLookAt Setup(int _entityId, Vector3 _lookAtPosition)
	{
		base.Setup(_entityId);
		this.lookAtPosition = _lookAtPosition;
		return this;
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x0017F846 File Offset: 0x0017DA46
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.lookAtPosition = new Vector3((float)_reader.ReadInt32(), (float)_reader.ReadInt32(), (float)_reader.ReadInt32());
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x0017F86F File Offset: 0x0017DA6F
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.lookAtPosition.x);
		_writer.Write((int)this.lookAtPosition.y);
		_writer.Write((int)this.lookAtPosition.z);
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x0017F8B0 File Offset: 0x0017DAB0
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

	// Token: 0x04003080 RID: 12416
	public Vector3 lookAtPosition;
}
