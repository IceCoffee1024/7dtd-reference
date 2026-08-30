using System;
using UnityEngine.Scripting;

// Token: 0x020007B0 RID: 1968
[Preserve]
public class NetPackageChunkRemove : NetPackage
{
	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x06003A61 RID: 14945 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x0017A8A2 File Offset: 0x00178AA2
	public NetPackageChunkRemove Setup(long _chunkKey)
	{
		this.chunkKey = _chunkKey;
		return this;
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x0017A8AC File Offset: 0x00178AAC
	public override void read(PooledBinaryReader _reader)
	{
		this.chunkKey = _reader.ReadInt64();
	}

	// Token: 0x06003A64 RID: 14948 RVA: 0x0017A8BA File Offset: 0x00178ABA
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.chunkKey);
	}

	// Token: 0x06003A65 RID: 14949 RVA: 0x0017A8CF File Offset: 0x00178ACF
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_callbacks.RemoveChunk(this.chunkKey);
	}

	// Token: 0x06003A66 RID: 14950 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x06003A67 RID: 14951 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04002F8A RID: 12170
	[PublicizedFrom(EAccessModifier.Private)]
	public long chunkKey;
}
