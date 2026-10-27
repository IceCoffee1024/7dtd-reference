using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007C4 RID: 1988
[Preserve]
public class NetPackageChunkClusterInfo : NetPackage
{
	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06003AFA RID: 15098 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AFB RID: 15099 RVA: 0x0017DFA4 File Offset: 0x0017C1A4
	public NetPackageChunkClusterInfo Setup(ChunkCluster _chunkCluster)
	{
		this.name = _chunkCluster.Name;
		this.cMinPos = _chunkCluster.ChunkMinPos;
		this.cMaxPos = _chunkCluster.ChunkMaxPos;
		this.bInfinite = !_chunkCluster.IsFixedSize;
		this.pos = _chunkCluster.Position;
		return this;
	}

	// Token: 0x06003AFC RID: 15100 RVA: 0x0017DFF4 File Offset: 0x0017C1F4
	public override void read(PooledBinaryReader _br)
	{
		this.name = _br.ReadString();
		this.cMinPos = new Vector2i(_br.ReadInt32(), _br.ReadInt32());
		this.cMaxPos = new Vector2i(_br.ReadInt32(), _br.ReadInt32());
		this.bInfinite = _br.ReadBoolean();
		this.pos = StreamUtils.ReadVector3(_br);
	}

	// Token: 0x06003AFD RID: 15101 RVA: 0x0017E054 File Offset: 0x0017C254
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

	// Token: 0x06003AFE RID: 15102 RVA: 0x0017E0D0 File Offset: 0x0017C2D0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.ChunkClusterInfo(this.name, this.bInfinite, this.cMinPos, this.cMaxPos, this.pos);
	}

	// Token: 0x04003014 RID: 12308
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003015 RID: 12309
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cMinPos;

	// Token: 0x04003016 RID: 12310
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cMaxPos;

	// Token: 0x04003017 RID: 12311
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bInfinite;

	// Token: 0x04003018 RID: 12312
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pos;
}
