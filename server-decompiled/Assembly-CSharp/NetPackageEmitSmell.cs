using System;
using UnityEngine.Scripting;

// Token: 0x020007DE RID: 2014
[Preserve]
public class NetPackageEmitSmell : NetPackage
{
	// Token: 0x06003B84 RID: 15236 RVA: 0x0017FF17 File Offset: 0x0017E117
	public NetPackageEmitSmell Setup(int entityId, string smellName)
	{
		this.EntityId = entityId;
		this.SmellName = smellName;
		return this;
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x0017FF28 File Offset: 0x0017E128
	public override void read(PooledBinaryReader _reader)
	{
		this.EntityId = _reader.ReadInt32();
		this.SmellName = _reader.ReadString();
		if (string.IsNullOrEmpty(this.SmellName))
		{
			this.SmellName = null;
		}
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0017FF56 File Offset: 0x0017E156
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.EntityId);
		_writer.Write((this.SmellName != null) ? this.SmellName : "");
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x0400307F RID: 12415
	public int EntityId;

	// Token: 0x04003080 RID: 12416
	public string SmellName;
}
