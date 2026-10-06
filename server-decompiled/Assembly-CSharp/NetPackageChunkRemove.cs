using System;
using UnityEngine.Scripting;

// Token: 0x020007BD RID: 1981
[Preserve]
public class NetPackageChunkRemove : NetPackage
{
	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06003AE9 RID: 15081 RVA: 0x00020119 File Offset: 0x0001E319
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06003AEA RID: 15082 RVA: 0x0017D15E File Offset: 0x0017B35E
	public NetPackageChunkRemove Setup(long _chunkKey)
	{
		this.chunkKey = _chunkKey;
		return this;
	}

	// Token: 0x06003AEB RID: 15083 RVA: 0x0017D168 File Offset: 0x0017B368
	public override void read(PooledBinaryReader _reader)
	{
		this.chunkKey = _reader.ReadInt64();
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x0017D176 File Offset: 0x0017B376
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.chunkKey);
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x0017D18B File Offset: 0x0017B38B
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_callbacks.RemoveChunk(this.chunkKey);
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06003AEE RID: 15086 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04002FFD RID: 12285
	[PublicizedFrom(EAccessModifier.Private)]
	public long chunkKey;
}
