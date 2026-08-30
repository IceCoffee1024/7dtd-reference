using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000859 RID: 2137
[Preserve]
public class NetPackageTileEntity : NetPackage
{
	// Token: 0x06003E7A RID: 15994 RVA: 0x0018A765 File Offset: 0x00188965
	public NetPackageTileEntity Setup(TileEntity _te, TileEntity.StreamModeWrite _eStreamMode)
	{
		return this.Setup(_te, _eStreamMode, byte.MaxValue);
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x0018A774 File Offset: 0x00188974
	public NetPackageTileEntity Setup(TileEntity _te, TileEntity.StreamModeWrite _eStreamMode, byte _handle)
	{
		this.handle = _handle;
		this.teWorldPos = _te.ToWorldPos();
		this.teBlockId = _te.blockValue.type;
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			_te.write(pooledBinaryWriter, _eStreamMode);
		}
		if (this.ms.Length > 65535L)
		{
			Log.Warning(string.Format("[NetPackageTileEntity] Oversized TE payload: {0}B for {1} at {2}.", this.ms.Length, _te.GetTileEntityType(), this.teWorldPos));
		}
		return this;
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x0018A830 File Offset: 0x00188A30
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageTileEntity()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x0018A868 File Offset: 0x00188A68
	public override void read(PooledBinaryReader _br)
	{
		this.handle = _br.ReadByte();
		this.teWorldPos = StreamUtils.ReadVector3i(_br);
		this.teBlockId = _br.ReadInt32();
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0018A8B4 File Offset: 0x00188AB4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.handle);
		StreamUtils.Write(_bw, this.teWorldPos);
		_bw.Write(this.teBlockId);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0018A910 File Offset: 0x00188B10
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		TileEntity tileEntity = _world.GetTileEntity(this.teWorldPos);
		if (tileEntity == null)
		{
			return;
		}
		if (_world.GetBlock(this.teWorldPos).type != this.teBlockId)
		{
			Log.Warning("[NetPackageTileEntity] Block type changed. Dropping package.");
			return;
		}
		tileEntity.SetHandle(this.handle);
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = this.ms;
			lock (pooledExpandableMemoryStream)
			{
				pooledBinaryReader.SetBaseStream(this.ms);
				this.ms.Position = 0L;
				tileEntity.read(pooledBinaryReader, _world.IsRemote() ? TileEntity.StreamModeRead.FromServer : TileEntity.StreamModeRead.FromClient);
			}
		}
		tileEntity.NotifyListeners();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			tileEntity.SetChunkModified();
			Vector3? vector = new Vector3?(tileEntity.ToWorldCenterPos());
			if (vector.Value == Vector3.zero)
			{
				vector = null;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(tileEntity, TileEntity.StreamModeWrite.ToClient, this.handle), true, -1, -1, -1, vector, 192, false);
		}
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x0018AA4C File Offset: 0x00188C4C
	public override int GetLength()
	{
		return (int)(22L + this.ms.Length);
	}

	// Token: 0x04003295 RID: 12949
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxPackageSize = 65535;

	// Token: 0x04003296 RID: 12950
	[PublicizedFrom(EAccessModifier.Private)]
	public byte handle;

	// Token: 0x04003297 RID: 12951
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i teWorldPos;

	// Token: 0x04003298 RID: 12952
	[PublicizedFrom(EAccessModifier.Private)]
	public int teBlockId;

	// Token: 0x04003299 RID: 12953
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
