using System;
using UnityEngine.Scripting;

// Token: 0x020007BA RID: 1978
[Preserve]
public class NetPackageChunkRemove : NetPackage
{
	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x00020021 File Offset: 0x0001E221
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x0017CB22 File Offset: 0x0017AD22
	public NetPackageChunkRemove Setup(long _chunkKey)
	{
		this.chunkKey = _chunkKey;
		return this;
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x0017CB2C File Offset: 0x0017AD2C
	public override void read(PooledBinaryReader _reader)
	{
		this.chunkKey = _reader.ReadInt64();
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x0017CB3A File Offset: 0x0017AD3A
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.chunkKey);
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x0017CB4F File Offset: 0x0017AD4F
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_callbacks.RemoveChunk(this.chunkKey);
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x06003ABF RID: 15039 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003003 RID: 12291
	[PublicizedFrom(EAccessModifier.Private)]
	public long chunkKey;
}
