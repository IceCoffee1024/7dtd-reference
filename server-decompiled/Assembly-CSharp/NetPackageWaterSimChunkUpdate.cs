using System;
using UnityEngine.Scripting;

// Token: 0x0200085D RID: 2141
[Preserve]
public class NetPackageWaterSimChunkUpdate : NetPackage, IMemoryPoolableObject
{
	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06003E7B RID: 15995 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x00189C33 File Offset: 0x00187E33
	public static int GetPoolSize()
	{
		return 200;
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x00189C3C File Offset: 0x00187E3C
	public void SetupForSend(Chunk chunk)
	{
		this.ms = MemoryPools.poolMemoryStream.AllocSync(true);
		this.sendWriter = MemoryPools.poolBinaryWriter.AllocSync(true);
		this.ms.Position = 0L;
		this.sendWriter.SetBaseStream(this.ms);
		this.sendWriter.Write(chunk.X);
		this.sendWriter.Write(chunk.Z);
		this.lengthStreamPos = this.ms.Position;
		this.sendWriter.Write(0);
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x00189CC8 File Offset: 0x00187EC8
	public void AddChange(ushort _voxelIndex, WaterValue _newValue)
	{
		this.sendWriter.Write(_voxelIndex);
		_newValue.Write(this.sendWriter);
		this.numVoxelUpdates++;
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x00189CF4 File Offset: 0x00187EF4
	public void FinalizeSend()
	{
		this.ms.Position = this.lengthStreamPos;
		this.sendWriter.Write(this.numVoxelUpdates);
		this.sendLength = (int)this.ms.Length;
		this.sendBytes = MemoryPools.poolByte.Alloc(this.sendLength);
		this.ms.Position = 0L;
		this.ms.Read(this.sendBytes, 0, this.sendLength);
		MemoryPools.poolBinaryWriter.FreeSync(this.sendWriter);
		this.sendWriter = null;
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
		this.ms = null;
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x00189DA0 File Offset: 0x00187FA0
	public override void read(PooledBinaryReader _br)
	{
		this.ms = MemoryPools.poolMemoryStream.AllocSync(true);
		this.ms.Position = 0L;
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x00189DE6 File Offset: 0x00187FE6
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.sendLength);
		_bw.Write(this.sendBytes, 0, this.sendLength);
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x00189E10 File Offset: 0x00188010
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(true))
		{
			this.ms.Position = 0L;
			pooledBinaryReader.SetBaseStream(this.ms);
			int num = pooledBinaryReader.ReadInt32();
			int num2 = pooledBinaryReader.ReadInt32();
			long num3 = WorldChunkCache.MakeChunkKey(num, num2);
			WaterSimulationNative.Instance.changeApplier.GetChangeWriter(num3);
			using (WaterSimulationApplyChanges.ChangesForChunk.Writer changeWriter = WaterSimulationNative.Instance.changeApplier.GetChangeWriter(num3))
			{
				int num4 = pooledBinaryReader.ReadInt32();
				for (int i = 0; i < num4; i++)
				{
					int num5 = (int)pooledBinaryReader.ReadUInt16();
					WaterValue waterValue = WaterValue.FromStream(pooledBinaryReader);
					changeWriter.RecordChange(num5, waterValue);
				}
			}
		}
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x00189EE4 File Offset: 0x001880E4
	public override int GetLength()
	{
		return this.sendLength + 4;
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x00189EF0 File Offset: 0x001880F0
	public void Reset()
	{
		if (this.sendWriter != null)
		{
			MemoryPools.poolBinaryWriter.FreeSync(this.sendWriter);
			this.sendWriter = null;
		}
		if (this.ms != null)
		{
			MemoryPools.poolMemoryStream.FreeSync(this.ms);
			this.ms = null;
		}
		if (this.sendBytes != null)
		{
			MemoryPools.poolByte.Free(this.sendBytes);
			this.sendBytes = null;
		}
		this.lengthStreamPos = 0L;
		this.numVoxelUpdates = 0;
		this.sendLength = 0;
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x00189F70 File Offset: 0x00188170
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x04003243 RID: 12867
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms;

	// Token: 0x04003244 RID: 12868
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryWriter sendWriter;

	// Token: 0x04003245 RID: 12869
	[PublicizedFrom(EAccessModifier.Private)]
	public long lengthStreamPos;

	// Token: 0x04003246 RID: 12870
	[PublicizedFrom(EAccessModifier.Private)]
	public int numVoxelUpdates;

	// Token: 0x04003247 RID: 12871
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] sendBytes;

	// Token: 0x04003248 RID: 12872
	[PublicizedFrom(EAccessModifier.Private)]
	public int sendLength;
}
