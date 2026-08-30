using System;
using UnityEngine.Scripting;

// Token: 0x020007E7 RID: 2023
[Preserve]
public class NetPackageEntitySetPartActive : NetPackage
{
	// Token: 0x06003BCE RID: 15310 RVA: 0x0018027C File Offset: 0x0017E47C
	public NetPackageEntitySetPartActive Setup(Entity entity, string partName, bool active)
	{
		this.id = entity.entityId;
		this.active = active;
		this.partName = partName;
		return this;
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x00180299 File Offset: 0x0017E499
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
		this.active = _br.ReadBoolean();
		this.partName = _br.ReadString();
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x001802BF File Offset: 0x0017E4BF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
		_bw.Write(this.active);
		_bw.Write(this.partName ?? "");
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x001802F8 File Offset: 0x0017E4F8
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

	// Token: 0x06003BD2 RID: 15314 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040030B1 RID: 12465
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040030B2 RID: 12466
	[PublicizedFrom(EAccessModifier.Private)]
	public bool active;

	// Token: 0x040030B3 RID: 12467
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;
}
