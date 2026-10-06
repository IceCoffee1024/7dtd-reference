using System;
using UnityEngine.Scripting;

// Token: 0x02000849 RID: 2121
[Preserve]
public class NetPackageSetBlockTexture : NetPackage, IMemoryPoolableObject
{
	// Token: 0x06003DC1 RID: 15809 RVA: 0x00188E36 File Offset: 0x00187036
	public NetPackageSetBlockTexture Setup(Vector3i _blockPos, BlockFace _blockFace, int _idx, int _playerIdThatChanged, byte _channel)
	{
		this.blockPos = _blockPos;
		this.blockFace = _blockFace;
		this.idx = (byte)_idx;
		this.playerIdThatChanged = _playerIdThatChanged;
		this.channel = _channel;
		return this;
	}

	// Token: 0x06003DC2 RID: 15810 RVA: 0x00188E5F File Offset: 0x0018705F
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockFace = (BlockFace)_br.ReadByte();
		this.idx = _br.ReadByte();
		this.playerIdThatChanged = _br.ReadInt32();
		this.channel = _br.ReadByte();
	}

	// Token: 0x06003DC3 RID: 15811 RVA: 0x00188EA0 File Offset: 0x001870A0
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write((byte)this.blockFace);
		_bw.Write(this.idx);
		_bw.Write(this.playerIdThatChanged);
		_bw.Write(this.channel);
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x00188EF0 File Offset: 0x001870F0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world != null && _world.ChunkCache != null)
		{
			GameManager.Instance.SetBlockTextureClient(this.blockPos, this.blockFace, (int)this.idx, this.channel);
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			NetPackageSetBlockTexture netPackageSetBlockTexture = NetPackageManager.GetPackage<NetPackageSetBlockTexture>().Setup(this.blockPos, this.blockFace, (int)this.idx, this.playerIdThatChanged, this.channel);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageSetBlockTexture, false, -1, this.playerIdThatChanged, -1, null, 192, false);
		}
	}

	// Token: 0x06003DC5 RID: 15813 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06003DC7 RID: 15815 RVA: 0x00178B59 File Offset: 0x00176D59
	public static int GetPoolSize()
	{
		return 500;
	}

	// Token: 0x04003249 RID: 12873
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400324A RID: 12874
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockFace blockFace;

	// Token: 0x0400324B RID: 12875
	[PublicizedFrom(EAccessModifier.Private)]
	public byte idx;

	// Token: 0x0400324C RID: 12876
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerIdThatChanged;

	// Token: 0x0400324D RID: 12877
	[PublicizedFrom(EAccessModifier.Private)]
	public byte channel;
}
