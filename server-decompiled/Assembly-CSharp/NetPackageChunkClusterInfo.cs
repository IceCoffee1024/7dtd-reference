using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007AF RID: 1967
[Preserve]
public class NetPackageChunkClusterInfo : NetPackage
{
	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06003A5A RID: 14938 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x0017A74C File Offset: 0x0017894C
	public NetPackageChunkClusterInfo Setup(ChunkCluster _chunkCluster)
	{
		this.name = _chunkCluster.Name;
		this.cMinPos = _chunkCluster.ChunkMinPos;
		this.cMaxPos = _chunkCluster.ChunkMaxPos;
		this.bInfinite = !_chunkCluster.IsFixedSize;
		this.pos = _chunkCluster.Position;
		return this;
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x0017A79C File Offset: 0x0017899C
	public override void read(PooledBinaryReader _br)
	{
		this.name = _br.ReadString();
		this.cMinPos = new Vector2i(_br.ReadInt32(), _br.ReadInt32());
		this.cMaxPos = new Vector2i(_br.ReadInt32(), _br.ReadInt32());
		this.bInfinite = _br.ReadBoolean();
		this.pos = StreamUtils.ReadVector3(_br);
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x0017A7FC File Offset: 0x001789FC
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

	// Token: 0x06003A5E RID: 14942 RVA: 0x0017A878 File Offset: 0x00178A78
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.ChunkClusterInfo(this.name, this.bInfinite, this.cMinPos, this.cMaxPos, this.pos);
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x0017A89E File Offset: 0x00178A9E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x04002F85 RID: 12165
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04002F86 RID: 12166
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cMinPos;

	// Token: 0x04002F87 RID: 12167
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cMaxPos;

	// Token: 0x04002F88 RID: 12168
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bInfinite;

	// Token: 0x04002F89 RID: 12169
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pos;
}
