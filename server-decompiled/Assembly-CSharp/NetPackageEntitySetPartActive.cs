using System;
using UnityEngine.Scripting;

// Token: 0x020007DC RID: 2012
[Preserve]
public class NetPackageEntitySetPartActive : NetPackage
{
	// Token: 0x06003B6F RID: 15215 RVA: 0x0017DEA4 File Offset: 0x0017C0A4
	public NetPackageEntitySetPartActive Setup(Entity entity, string partName, bool active)
	{
		this.id = entity.entityId;
		this.active = active;
		this.partName = partName;
		return this;
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x0017DEC1 File Offset: 0x0017C0C1
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
		this.active = _br.ReadBoolean();
		this.partName = _br.ReadString();
	}

	// Token: 0x06003B71 RID: 15217 RVA: 0x0017DEE7 File Offset: 0x0017C0E7
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
		_bw.Write(this.active);
		_bw.Write(this.partName ?? "");
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x0017DF20 File Offset: 0x0017C120
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

	// Token: 0x06003B73 RID: 15219 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04003032 RID: 12338
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x04003033 RID: 12339
	[PublicizedFrom(EAccessModifier.Private)]
	public bool active;

	// Token: 0x04003034 RID: 12340
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;
}
