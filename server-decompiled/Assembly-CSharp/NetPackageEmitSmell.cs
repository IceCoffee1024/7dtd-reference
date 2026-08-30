using System;
using UnityEngine.Scripting;

// Token: 0x020007D1 RID: 2001
[Preserve]
public class NetPackageEmitSmell : NetPackage
{
	// Token: 0x06003B44 RID: 15172 RVA: 0x0017E893 File Offset: 0x0017CA93
	public NetPackageEmitSmell Setup(int entityId, string smellName)
	{
		this.EntityId = entityId;
		this.SmellName = smellName;
		return this;
	}

	// Token: 0x06003B45 RID: 15173 RVA: 0x0017E8A4 File Offset: 0x0017CAA4
	public override void read(PooledBinaryReader _reader)
	{
		this.EntityId = _reader.ReadInt32();
		this.SmellName = _reader.ReadString();
		if (string.IsNullOrEmpty(this.SmellName))
		{
			this.SmellName = null;
		}
	}

	// Token: 0x06003B46 RID: 15174 RVA: 0x0017E8D2 File Offset: 0x0017CAD2
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.EntityId);
		_writer.Write((this.SmellName != null) ? this.SmellName : "");
	}

	// Token: 0x06003B47 RID: 15175 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x06003B48 RID: 15176 RVA: 0x000811EE File Offset: 0x0007F3EE
	public override int GetLength()
	{
		return 10;
	}

	// Token: 0x04003065 RID: 12389
	public int EntityId;

	// Token: 0x04003066 RID: 12390
	public string SmellName;
}
