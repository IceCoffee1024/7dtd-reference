using System;
using UnityEngine.Scripting;

// Token: 0x0200083C RID: 2108
[Preserve]
public class NetPackageSetBlockTexture : NetPackage, IMemoryPoolableObject
{
	// Token: 0x06003DB2 RID: 15794 RVA: 0x001872EE File Offset: 0x001854EE
	public NetPackageSetBlockTexture Setup(Vector3i _blockPos, BlockFace _blockFace, int _idx, int _playerIdThatChanged, byte _channel)
	{
		this.blockPos = _blockPos;
		this.blockFace = _blockFace;
		this.idx = (byte)_idx;
		this.playerIdThatChanged = _playerIdThatChanged;
		this.channel = _channel;
		return this;
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x00187317 File Offset: 0x00185517
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockFace = (BlockFace)_br.ReadByte();
		this.idx = _br.ReadByte();
		this.playerIdThatChanged = _br.ReadInt32();
		this.channel = _br.ReadByte();
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x00187358 File Offset: 0x00185558
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write((byte)this.blockFace);
		_bw.Write(this.idx);
		_bw.Write(this.playerIdThatChanged);
		_bw.Write(this.channel);
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x001873A8 File Offset: 0x001855A8
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

	// Token: 0x06003DB6 RID: 15798 RVA: 0x000ECB9C File Offset: 0x000EAD9C
	public override int GetLength()
	{
		return 19;
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00176121 File Offset: 0x00174321
	public static int GetPoolSize()
	{
		return 500;
	}

	// Token: 0x040031E3 RID: 12771
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x040031E4 RID: 12772
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockFace blockFace;

	// Token: 0x040031E5 RID: 12773
	[PublicizedFrom(EAccessModifier.Private)]
	public byte idx;

	// Token: 0x040031E6 RID: 12774
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerIdThatChanged;

	// Token: 0x040031E7 RID: 12775
	[PublicizedFrom(EAccessModifier.Private)]
	public byte channel;
}
