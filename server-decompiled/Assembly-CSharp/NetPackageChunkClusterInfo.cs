using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007BC RID: 1980
[Preserve]
public class NetPackageChunkClusterInfo : NetPackage
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AE4 RID: 15076 RVA: 0x0017D00C File Offset: 0x0017B20C
	public NetPackageChunkClusterInfo Setup(ChunkCluster _chunkCluster)
	{
		this.name = _chunkCluster.Name;
		this.cMinPos = _chunkCluster.ChunkMinPos;
		this.cMaxPos = _chunkCluster.ChunkMaxPos;
		this.bInfinite = !_chunkCluster.IsFixedSize;
		this.pos = _chunkCluster.Position;
		return this;
	}

	// Token: 0x06003AE5 RID: 15077 RVA: 0x0017D05C File Offset: 0x0017B25C
	public override void read(PooledBinaryReader _br)
	{
		this.name = _br.ReadString();
		this.cMinPos = new Vector2i(_br.ReadInt32(), _br.ReadInt32());
		this.cMaxPos = new Vector2i(_br.ReadInt32(), _br.ReadInt32());
		this.bInfinite = _br.ReadBoolean();
		this.pos = StreamUtils.ReadVector3(_br);
	}

	// Token: 0x06003AE6 RID: 15078 RVA: 0x0017D0BC File Offset: 0x0017B2BC
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

	// Token: 0x06003AE7 RID: 15079 RVA: 0x0017D138 File Offset: 0x0017B338
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.ChunkClusterInfo(this.name, this.bInfinite, this.cMinPos, this.cMaxPos, this.pos);
	}

	// Token: 0x04002FF8 RID: 12280
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04002FF9 RID: 12281
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cMinPos;

	// Token: 0x04002FFA RID: 12282
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cMaxPos;

	// Token: 0x04002FFB RID: 12283
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bInfinite;

	// Token: 0x04002FFC RID: 12284
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pos;
}
