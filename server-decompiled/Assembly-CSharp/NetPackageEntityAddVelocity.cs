using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007DB RID: 2011
[Preserve]
public class NetPackageEntityAddVelocity : NetPackage
{
	// Token: 0x06003B84 RID: 15236 RVA: 0x0017F29F File Offset: 0x0017D49F
	public NetPackageEntityAddVelocity Setup(int _entityId, Vector3 _addVelocity)
	{
		this.entityId = _entityId;
		this.addVelocity = _addVelocity;
		return this;
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x0017F2B0 File Offset: 0x0017D4B0
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.addVelocity = StreamUtils.ReadVector3(_br);
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0017F2CA File Offset: 0x0017D4CA
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		StreamUtils.Write(_bw, this.addVelocity);
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x0017F2EB File Offset: 0x0017D4EB
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().AddVelocityToEntityServer(this.entityId, this.addVelocity);
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06003B88 RID: 15240 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x0400306E RID: 12398
	public int entityId;

	// Token: 0x0400306F RID: 12399
	public Vector3 addVelocity;
}
