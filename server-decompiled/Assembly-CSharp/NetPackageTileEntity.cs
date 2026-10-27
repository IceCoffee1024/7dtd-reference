using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000866 RID: 2150
[Preserve]
public class NetPackageTileEntity : NetPackage
{
	// Token: 0x06003E43 RID: 15939 RVA: 0x0018B6B9 File Offset: 0x001898B9
	public NetPackageTileEntity Setup(TileEntity _te, StreamModeWrite _eStreamMode)
	{
		return this.Setup(_te, _eStreamMode, byte.MaxValue);
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x0018B6C8 File Offset: 0x001898C8
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

	// Token: 0x06003E45 RID: 15941 RVA: 0x0018B784 File Offset: 0x00189984
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageTileEntity()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003E46 RID: 15942 RVA: 0x0018B7BC File Offset: 0x001899BC
	public override void read(PooledBinaryReader _br)
	{
		this.handle = _br.ReadByte();
		this.teWorldPos = StreamUtils.ReadVector3i(_br);
		this.teBlockId = _br.ReadInt32();
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003E47 RID: 15943 RVA: 0x0018B808 File Offset: 0x00189A08
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.handle);
		StreamUtils.Write(_bw, this.teWorldPos);
		_bw.Write(this.teBlockId);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003E48 RID: 15944 RVA: 0x0018B864 File Offset: 0x00189A64
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

	// Token: 0x040032A0 RID: 12960
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxPackageSize = 65535;

	// Token: 0x040032A1 RID: 12961
	[PublicizedFrom(EAccessModifier.Private)]
	public byte handle;

	// Token: 0x040032A2 RID: 12962
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i teWorldPos;

	// Token: 0x040032A3 RID: 12963
	[PublicizedFrom(EAccessModifier.Private)]
	public int teBlockId;

	// Token: 0x040032A4 RID: 12964
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
