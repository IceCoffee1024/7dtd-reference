using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E5 RID: 2021
[Preserve]
public class NetPackageEntityAddVelocity : NetPackage
{
	// Token: 0x06003BA7 RID: 15271 RVA: 0x001803A3 File Offset: 0x0017E5A3
	public NetPackageEntityAddVelocity Setup(int _entityId, Vector3 _addVelocity)
	{
		this.entityId = _entityId;
		this.addVelocity = _addVelocity;
		return this;
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x001803B4 File Offset: 0x0017E5B4
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.addVelocity = StreamUtils.ReadVector3(_br);
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x001803CE File Offset: 0x0017E5CE
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		StreamUtils.Write(_bw, this.addVelocity);
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x001803EF File Offset: 0x0017E5EF
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().AddVelocityToEntityServer(this.entityId, this.addVelocity);
	}

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06003BAB RID: 15275 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x0400308E RID: 12430
	public int entityId;

	// Token: 0x0400308F RID: 12431
	public Vector3 addVelocity;
}
