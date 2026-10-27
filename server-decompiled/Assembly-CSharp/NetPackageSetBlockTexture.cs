using System;
using UnityEngine.Scripting;

// Token: 0x02000853 RID: 2131
[Preserve]
public class NetPackageSetBlockTexture : NetPackage, IMemoryPoolableObject
{
	// Token: 0x06003DE4 RID: 15844 RVA: 0x00189EDE File Offset: 0x001880DE
	public NetPackageSetBlockTexture Setup(Vector3i _blockPos, BlockFace _blockFace, int _idx, int _playerIdThatChanged, byte _channel)
	{
		this.blockPos = _blockPos;
		this.blockFace = _blockFace;
		this.idx = (byte)_idx;
		this.playerIdThatChanged = _playerIdThatChanged;
		this.channel = _channel;
		return this;
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x00189F07 File Offset: 0x00188107
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockFace = (BlockFace)_br.ReadByte();
		this.idx = _br.ReadByte();
		this.playerIdThatChanged = _br.ReadInt32();
		this.channel = _br.ReadByte();
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x00189F48 File Offset: 0x00188148
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write((byte)this.blockFace);
		_bw.Write(this.idx);
		_bw.Write(this.playerIdThatChanged);
		_bw.Write(this.channel);
	}

	// Token: 0x06003DE7 RID: 15847 RVA: 0x00189F98 File Offset: 0x00188198
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

	// Token: 0x06003DE8 RID: 15848 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x06003DE9 RID: 15849 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06003DEA RID: 15850 RVA: 0x00179AF1 File Offset: 0x00177CF1
	public static int GetPoolSize()
	{
		return 500;
	}

	// Token: 0x04003269 RID: 12905
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400326A RID: 12906
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockFace blockFace;

	// Token: 0x0400326B RID: 12907
	[PublicizedFrom(EAccessModifier.Private)]
	public byte idx;

	// Token: 0x0400326C RID: 12908
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerIdThatChanged;

	// Token: 0x0400326D RID: 12909
	[PublicizedFrom(EAccessModifier.Private)]
	public byte channel;
}
