using System;
using UnityEngine.Scripting;

// Token: 0x020007C6 RID: 1990
[Preserve]
public class NetPackageEmitSmell : NetPackage
{
	// Token: 0x06003AE5 RID: 15077 RVA: 0x0017C4BB File Offset: 0x0017A6BB
	public NetPackageEmitSmell Setup(int entityId, string smellName)
	{
		this.EntityId = entityId;
		this.SmellName = smellName;
		return this;
	}

	// Token: 0x06003AE6 RID: 15078 RVA: 0x0017C4CC File Offset: 0x0017A6CC
	public override void read(PooledBinaryReader _reader)
	{
		this.EntityId = _reader.ReadInt32();
		this.SmellName = _reader.ReadString();
		if (string.IsNullOrEmpty(this.SmellName))
		{
			this.SmellName = null;
		}
	}

	// Token: 0x06003AE7 RID: 15079 RVA: 0x0017C4FA File Offset: 0x0017A6FA
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.EntityId);
		_writer.Write((this.SmellName != null) ? this.SmellName : "");
	}

	// Token: 0x06003AE8 RID: 15080 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x06003AE9 RID: 15081 RVA: 0x00080B3A File Offset: 0x0007ED3A
	public override int GetLength()
	{
		return 10;
	}

	// Token: 0x04002FE6 RID: 12262
	public int EntityId;

	// Token: 0x04002FE7 RID: 12263
	public string SmellName;
}
