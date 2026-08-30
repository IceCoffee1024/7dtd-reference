using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D8 RID: 2008
[Preserve]
public class NetPackageEntityAddVelocity : NetPackage
{
	// Token: 0x06003B6B RID: 15211 RVA: 0x0017ED1F File Offset: 0x0017CF1F
	public NetPackageEntityAddVelocity Setup(int _entityId, Vector3 _addVelocity)
	{
		this.entityId = _entityId;
		this.addVelocity = _addVelocity;
		return this;
	}

	// Token: 0x06003B6C RID: 15212 RVA: 0x0017ED30 File Offset: 0x0017CF30
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.addVelocity = StreamUtils.ReadVector3(_br);
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x0017ED4A File Offset: 0x0017CF4A
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		StreamUtils.Write(_bw, this.addVelocity);
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x0017ED6B File Offset: 0x0017CF6B
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().AddVelocityToEntityServer(this.entityId, this.addVelocity);
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06003B6F RID: 15215 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x0017E246 File Offset: 0x0017C446
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x04003074 RID: 12404
	public int entityId;

	// Token: 0x04003075 RID: 12405
	public Vector3 addVelocity;
}
