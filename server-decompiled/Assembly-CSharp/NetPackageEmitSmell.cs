using System;
using UnityEngine.Scripting;

// Token: 0x020007D4 RID: 2004
[Preserve]
public class NetPackageEmitSmell : NetPackage
{
	// Token: 0x06003B61 RID: 15201 RVA: 0x0017EE13 File Offset: 0x0017D013
	public NetPackageEmitSmell Setup(int entityId, string smellName)
	{
		this.EntityId = entityId;
		this.SmellName = smellName;
		return this;
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x0017EE24 File Offset: 0x0017D024
	public override void read(PooledBinaryReader _reader)
	{
		this.EntityId = _reader.ReadInt32();
		this.SmellName = _reader.ReadString();
		if (string.IsNullOrEmpty(this.SmellName))
		{
			this.SmellName = null;
		}
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x0017EE52 File Offset: 0x0017D052
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.EntityId);
		_writer.Write((this.SmellName != null) ? this.SmellName : "");
	}

	// Token: 0x06003B64 RID: 15204 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x0400305F RID: 12383
	public int EntityId;

	// Token: 0x04003060 RID: 12384
	public string SmellName;
}
