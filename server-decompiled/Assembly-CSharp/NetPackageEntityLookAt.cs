using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007DD RID: 2013
[Preserve]
public class NetPackageEntityLookAt : NetPackageEntityTargeted
{
	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06003B8E RID: 15246 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x0017F2F1 File Offset: 0x0017D4F1
	public NetPackageEntityLookAt Setup(int _entityId, Vector3 _lookAtPosition)
	{
		base.Setup(_entityId);
		this.lookAtPosition = _lookAtPosition;
		return this;
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0017F302 File Offset: 0x0017D502
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.lookAtPosition = new Vector3((float)_reader.ReadInt32(), (float)_reader.ReadInt32(), (float)_reader.ReadInt32());
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0017F32B File Offset: 0x0017D52B
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.lookAtPosition.x);
		_writer.Write((int)this.lookAtPosition.y);
		_writer.Write((int)this.lookAtPosition.z);
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0017F36C File Offset: 0x0017D56C
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

	// Token: 0x06003B93 RID: 15251 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003087 RID: 12423
	public Vector3 lookAtPosition;
}
