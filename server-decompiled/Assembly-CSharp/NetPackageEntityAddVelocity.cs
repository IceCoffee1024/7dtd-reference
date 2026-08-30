using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007CD RID: 1997
[Preserve]
public class NetPackageEntityAddVelocity : NetPackage
{
	// Token: 0x06003B0C RID: 15116 RVA: 0x0017C947 File Offset: 0x0017AB47
	public NetPackageEntityAddVelocity Setup(int _entityId, Vector3 _addVelocity)
	{
		this.entityId = _entityId;
		this.addVelocity = _addVelocity;
		return this;
	}

	// Token: 0x06003B0D RID: 15117 RVA: 0x0017C958 File Offset: 0x0017AB58
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.addVelocity = StreamUtils.ReadVector3(_br);
	}

	// Token: 0x06003B0E RID: 15118 RVA: 0x0017C972 File Offset: 0x0017AB72
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		StreamUtils.Write(_bw, this.addVelocity);
	}

	// Token: 0x06003B0F RID: 15119 RVA: 0x0017C993 File Offset: 0x0017AB93
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().AddVelocityToEntityServer(this.entityId, this.addVelocity);
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06003B10 RID: 15120 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B11 RID: 15121 RVA: 0x0017BE6E File Offset: 0x0017A06E
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x04002FF5 RID: 12277
	public int entityId;

	// Token: 0x04002FF6 RID: 12278
	public Vector3 addVelocity;
}
