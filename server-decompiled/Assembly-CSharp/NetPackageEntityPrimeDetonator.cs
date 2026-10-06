using System;
using UnityEngine.Scripting;

// Token: 0x020007E5 RID: 2021
[Preserve]
public class NetPackageEntityPrimeDetonator : NetPackage
{
	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06003BBB RID: 15291 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x0018004C File Offset: 0x0017E24C
	public NetPackageEntityPrimeDetonator Setup(EntityZombieCop entity)
	{
		this.id = entity.entityId;
		return this;
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x0018005B File Offset: 0x0017E25B
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x00180069 File Offset: 0x0017E269
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x00180080 File Offset: 0x0017E280
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityZombieCop entityZombieCop = _world.GetEntity(this.id) as EntityZombieCop;
		if (entityZombieCop == null)
		{
			Log.Out("Discarding " + base.GetType().Name);
			return;
		}
		entityZombieCop.PrimeDetonator();
	}

	// Token: 0x04003096 RID: 12438
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;
}
