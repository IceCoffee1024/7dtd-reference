using System;
using UnityEngine.Scripting;

// Token: 0x020007C5 RID: 1989
[Preserve]
public class NetPackageChunkRemove : NetPackage
{
	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06003B00 RID: 15104 RVA: 0x00020119 File Offset: 0x0001E319
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06003B01 RID: 15105 RVA: 0x0017E0F6 File Offset: 0x0017C2F6
	public NetPackageChunkRemove Setup(long _chunkKey)
	{
		this.chunkKey = _chunkKey;
		return this;
	}

	// Token: 0x06003B02 RID: 15106 RVA: 0x0017E100 File Offset: 0x0017C300
	public override void read(PooledBinaryReader _reader)
	{
		this.chunkKey = _reader.ReadInt64();
	}

	// Token: 0x06003B03 RID: 15107 RVA: 0x0017E10E File Offset: 0x0017C30E
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.chunkKey);
	}

	// Token: 0x06003B04 RID: 15108 RVA: 0x0017E123 File Offset: 0x0017C323
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_callbacks.RemoveChunk(this.chunkKey);
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06003B05 RID: 15109 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003019 RID: 12313
	[PublicizedFrom(EAccessModifier.Private)]
	public long chunkKey;
}
