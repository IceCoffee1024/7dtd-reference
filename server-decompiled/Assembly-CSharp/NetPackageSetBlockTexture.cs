using System;
using UnityEngine.Scripting;

// Token: 0x02000848 RID: 2120
[Preserve]
public class NetPackageSetBlockTexture : NetPackage, IMemoryPoolableObject
{
	// Token: 0x06003E17 RID: 15895 RVA: 0x0018930E File Offset: 0x0018750E
	public NetPackageSetBlockTexture Setup(Vector3i _blockPos, BlockFace _blockFace, int _idx, int _playerIdThatChanged, byte _channel)
	{
		this.blockPos = _blockPos;
		this.blockFace = _blockFace;
		this.idx = (byte)_idx;
		this.playerIdThatChanged = _playerIdThatChanged;
		this.channel = _channel;
		return this;
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x00189337 File Offset: 0x00187537
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockFace = (BlockFace)_br.ReadByte();
		this.idx = _br.ReadByte();
		this.playerIdThatChanged = _br.ReadInt32();
		this.channel = _br.ReadByte();
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x00189378 File Offset: 0x00187578
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write((byte)this.blockFace);
		_bw.Write(this.idx);
		_bw.Write(this.playerIdThatChanged);
		_bw.Write(this.channel);
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x001893C8 File Offset: 0x001875C8
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

	// Token: 0x06003E1B RID: 15899 RVA: 0x000ED7F0 File Offset: 0x000EB9F0
	public override int GetLength()
	{
		return 19;
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x06003E1D RID: 15901 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06003E1E RID: 15902 RVA: 0x001783A1 File Offset: 0x001765A1
	public static int GetPoolSize()
	{
		return 500;
	}

	// Token: 0x04003262 RID: 12898
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04003263 RID: 12899
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockFace blockFace;

	// Token: 0x04003264 RID: 12900
	[PublicizedFrom(EAccessModifier.Private)]
	public byte idx;

	// Token: 0x04003265 RID: 12901
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerIdThatChanged;

	// Token: 0x04003266 RID: 12902
	[PublicizedFrom(EAccessModifier.Private)]
	public byte channel;
}
