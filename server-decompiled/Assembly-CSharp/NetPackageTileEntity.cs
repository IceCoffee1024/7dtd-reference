using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200085C RID: 2140
[Preserve]
public class NetPackageTileEntity : NetPackage
{
	// Token: 0x06003E20 RID: 15904 RVA: 0x0018A611 File Offset: 0x00188811
	public NetPackageTileEntity Setup(TileEntity _te, StreamModeWrite _eStreamMode)
	{
		return this.Setup(_te, _eStreamMode, byte.MaxValue);
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x0018A620 File Offset: 0x00188820
	public NetPackageTileEntity Setup(TileEntity _te, StreamModeWrite _eStreamMode, byte _handle)
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

	// Token: 0x06003E22 RID: 15906 RVA: 0x0018A6DC File Offset: 0x001888DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageTileEntity()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x0018A714 File Offset: 0x00188914
	public override void read(PooledBinaryReader _br)
	{
		this.handle = _br.ReadByte();
		this.teWorldPos = StreamUtils.ReadVector3i(_br);
		this.teBlockId = _br.ReadInt32();
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x0018A760 File Offset: 0x00188960
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.handle);
		StreamUtils.Write(_bw, this.teWorldPos);
		_bw.Write(this.teBlockId);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x0018A7BC File Offset: 0x001889BC
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
				tileEntity.read(pooledBinaryReader, _world.IsRemote() ? StreamModeRead.FromServer : StreamModeRead.FromClient);
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
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(tileEntity, StreamModeWrite.ToClient, this.handle), true, -1, -1, -1, vector, 192, false);
		}
	}

	// Token: 0x04003280 RID: 12928
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxPackageSize = 65535;

	// Token: 0x04003281 RID: 12929
	[PublicizedFrom(EAccessModifier.Private)]
	public byte handle;

	// Token: 0x04003282 RID: 12930
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i teWorldPos;

	// Token: 0x04003283 RID: 12931
	[PublicizedFrom(EAccessModifier.Private)]
	public int teBlockId;

	// Token: 0x04003284 RID: 12932
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
