using System;
using UnityEngine.Scripting;

// Token: 0x020007EA RID: 2026
[Preserve]
public class NetPackageEntitySetPartActive : NetPackage
{
	// Token: 0x06003BD9 RID: 15321 RVA: 0x001807B4 File Offset: 0x0017E9B4
	public NetPackageEntitySetPartActive Setup(Entity entity, string partName, bool active)
	{
		this.id = entity.entityId;
		this.active = active;
		this.partName = partName;
		return this;
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x001807D1 File Offset: 0x0017E9D1
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
		this.active = _br.ReadBoolean();
		this.partName = _br.ReadString();
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x001807F7 File Offset: 0x0017E9F7
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
		_bw.Write(this.active);
		_bw.Write(this.partName ?? "");
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x00180830 File Offset: 0x0017EA30
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Entity entity = _world.GetEntity(this.id);
		if (entity == null)
		{
			Log.Out("Discarding " + base.GetType().Name);
			return;
		}
		if (string.IsNullOrEmpty(this.partName))
		{
			Log.Out("Discarding " + base.GetType().Name + " unexpected no part name");
			return;
		}
		entity.SetTransformActive(this.partName, this.active);
	}

	// Token: 0x040030AA RID: 12458
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040030AB RID: 12459
	[PublicizedFrom(EAccessModifier.Private)]
	public bool active;

	// Token: 0x040030AC RID: 12460
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;
}
