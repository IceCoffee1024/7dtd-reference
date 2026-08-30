using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007B9 RID: 1977
[Preserve]
public class NetPackageChunkClusterInfo : NetPackage
{
	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x0017C9CC File Offset: 0x0017ABCC
	public NetPackageChunkClusterInfo Setup(ChunkCluster _chunkCluster)
	{
		this.name = _chunkCluster.Name;
		this.cMinPos = _chunkCluster.ChunkMinPos;
		this.cMaxPos = _chunkCluster.ChunkMaxPos;
		this.bInfinite = !_chunkCluster.IsFixedSize;
		this.pos = _chunkCluster.Position;
		return this;
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x0017CA1C File Offset: 0x0017AC1C
	public override void read(PooledBinaryReader _br)
	{
		this.name = _br.ReadString();
		this.cMinPos = new Vector2i(_br.ReadInt32(), _br.ReadInt32());
		this.cMaxPos = new Vector2i(_br.ReadInt32(), _br.ReadInt32());
		this.bInfinite = _br.ReadBoolean();
		this.pos = StreamUtils.ReadVector3(_br);
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x0017CA7C File Offset: 0x0017AC7C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.name);
		_bw.Write(this.cMinPos.x);
		_bw.Write(this.cMinPos.y);
		_bw.Write(this.cMaxPos.x);
		_bw.Write(this.cMaxPos.y);
		_bw.Write(this.bInfinite);
		StreamUtils.Write(_bw, this.pos);
	}

	// Token: 0x06003AB6 RID: 15030 RVA: 0x0017CAF8 File Offset: 0x0017ACF8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.ChunkClusterInfo(this.name, this.bInfinite, this.cMinPos, this.cMaxPos, this.pos);
	}

	// Token: 0x06003AB7 RID: 15031 RVA: 0x0017CB1E File Offset: 0x0017AD1E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x04002FFE RID: 12286
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04002FFF RID: 12287
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cMinPos;

	// Token: 0x04003000 RID: 12288
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cMaxPos;

	// Token: 0x04003001 RID: 12289
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bInfinite;

	// Token: 0x04003002 RID: 12290
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pos;
}
