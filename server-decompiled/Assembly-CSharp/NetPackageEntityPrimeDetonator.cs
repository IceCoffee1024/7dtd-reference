using System;
using UnityEngine.Scripting;

// Token: 0x020007D7 RID: 2007
[Preserve]
public class NetPackageEntityPrimeDetonator : NetPackage
{
	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06003B4C RID: 15180 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x0017D734 File Offset: 0x0017B934
	public NetPackageEntityPrimeDetonator Setup(EntityZombieCop entity)
	{
		this.id = entity.entityId;
		return this;
	}

	// Token: 0x06003B4E RID: 15182 RVA: 0x0017D743 File Offset: 0x0017B943
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
	}

	// Token: 0x06003B4F RID: 15183 RVA: 0x0017D751 File Offset: 0x0017B951
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x0017D768 File Offset: 0x0017B968
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

	// Token: 0x06003B51 RID: 15185 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x0400301E RID: 12318
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;
}
