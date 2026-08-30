using System;
using UnityEngine.Scripting;

// Token: 0x020007E2 RID: 2018
[Preserve]
public class NetPackageEntityPrimeDetonator : NetPackage
{
	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06003BAB RID: 15275 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x0017FB0C File Offset: 0x0017DD0C
	public NetPackageEntityPrimeDetonator Setup(EntityZombieCop entity)
	{
		this.id = entity.entityId;
		return this;
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x0017FB1B File Offset: 0x0017DD1B
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x0017FB29 File Offset: 0x0017DD29
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x0017FB40 File Offset: 0x0017DD40
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

	// Token: 0x06003BB0 RID: 15280 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x0400309D RID: 12445
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;
}
