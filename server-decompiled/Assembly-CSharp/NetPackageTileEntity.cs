using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200084D RID: 2125
[Preserve]
public class NetPackageTileEntity : NetPackage
{
	// Token: 0x06003E15 RID: 15893 RVA: 0x00188731 File Offset: 0x00186931
	public NetPackageTileEntity Setup(TileEntity _te, TileEntity.StreamModeWrite _eStreamMode)
	{
		return this.Setup(_te, _eStreamMode, byte.MaxValue);
	}

	// Token: 0x06003E16 RID: 15894 RVA: 0x00188740 File Offset: 0x00186940
	public NetPackageTileEntity Setup(TileEntity _te, TileEntity.StreamModeWrite _eStreamMode, byte _handle)
	{
		this.handle = _handle;
		this.teWorldPos = _te.ToWorldPos();
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			_te.write(pooledBinaryWriter, _eStreamMode);
		}
		return this;
	}

	// Token: 0x06003E17 RID: 15895 RVA: 0x001887A0 File Offset: 0x001869A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageTileEntity()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x001887D8 File Offset: 0x001869D8
	public override void read(PooledBinaryReader _br)
	{
		this.handle = _br.ReadByte();
		this.teWorldPos = StreamUtils.ReadVector3i(_br);
		int num = (int)_br.ReadUInt16();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x00188818 File Offset: 0x00186A18
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.handle);
		StreamUtils.Write(_bw, this.teWorldPos);
		_bw.Write((ushort)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x00188868 File Offset: 0x00186A68
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

	// Token: 0x06003E1B RID: 15899 RVA: 0x0018897C File Offset: 0x00186B7C
	public override int GetLength()
	{
		return (int)(22L + this.ms.Length);
	}

	// Token: 0x04003216 RID: 12822
	[PublicizedFrom(EAccessModifier.Private)]
	public byte handle;

	// Token: 0x04003217 RID: 12823
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i teWorldPos;

	// Token: 0x04003218 RID: 12824
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
