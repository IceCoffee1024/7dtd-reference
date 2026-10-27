using System;
using UnityEngine.Scripting;

// Token: 0x020007F4 RID: 2036
[Preserve]
public class NetPackageEntitySetPartActive : NetPackage
{
	// Token: 0x06003BFC RID: 15356 RVA: 0x001818B8 File Offset: 0x0017FAB8
	public NetPackageEntitySetPartActive Setup(Entity entity, string partName, bool active)
	{
		this.id = entity.entityId;
		this.active = active;
		this.partName = partName;
		return this;
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x001818D5 File Offset: 0x0017FAD5
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
		this.active = _br.ReadBoolean();
		this.partName = _br.ReadString();
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x001818FB File Offset: 0x0017FAFB
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
		_bw.Write(this.active);
		_bw.Write(this.partName ?? "");
	}

	// Token: 0x06003BFF RID: 15359 RVA: 0x00181934 File Offset: 0x0017FB34
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

	// Token: 0x040030CA RID: 12490
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040030CB RID: 12491
	[PublicizedFrom(EAccessModifier.Private)]
	public bool active;

	// Token: 0x040030CC RID: 12492
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;
}
