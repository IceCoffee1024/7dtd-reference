using System;
using UnityEngine.Scripting;

// Token: 0x020007EF RID: 2031
[Preserve]
public class NetPackageEntityPrimeDetonator : NetPackage
{
	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x06003BDE RID: 15326 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x00181150 File Offset: 0x0017F350
	public NetPackageEntityPrimeDetonator Setup(EntityZombieCop entity)
	{
		this.id = entity.entityId;
		return this;
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x0018115F File Offset: 0x0017F35F
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x0018116D File Offset: 0x0017F36D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x00181184 File Offset: 0x0017F384
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

	// Token: 0x040030B6 RID: 12470
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;
}
