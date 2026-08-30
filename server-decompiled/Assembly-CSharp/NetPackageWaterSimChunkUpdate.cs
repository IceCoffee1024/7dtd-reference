using System;
using UnityEngine.Scripting;

// Token: 0x02000869 RID: 2153
[Preserve]
public class NetPackageWaterSimChunkUpdate : NetPackage, IMemoryPoolableObject
{
	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06003EE0 RID: 16096 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x0018BD03 File Offset: 0x00189F03
	public static int GetPoolSize()
	{
		return 200;
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x0018BD0C File Offset: 0x00189F0C
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

	// Token: 0x06003EE3 RID: 16099 RVA: 0x0018BD98 File Offset: 0x00189F98
	public void AddChange(ushort _voxelIndex, WaterValue _newValue)
	{
		this.sendWriter.Write(_voxelIndex);
		_newValue.Write(this.sendWriter);
		this.numVoxelUpdates++;
	}

	// Token: 0x06003EE4 RID: 16100 RVA: 0x0018BDC4 File Offset: 0x00189FC4
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

	// Token: 0x06003EE5 RID: 16101 RVA: 0x0018BE70 File Offset: 0x0018A070
	public override void read(PooledBinaryReader _br)
	{
		this.ms = MemoryPools.poolMemoryStream.AllocSync(true);
		this.ms.Position = 0L;
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x0018BEB6 File Offset: 0x0018A0B6
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.sendLength);
		_bw.Write(this.sendBytes, 0, this.sendLength);
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x0018BEE0 File Offset: 0x0018A0E0
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

	// Token: 0x06003EE8 RID: 16104 RVA: 0x0018BFB4 File Offset: 0x0018A1B4
	public override int GetLength()
	{
		return this.sendLength + 4;
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x0018BFC0 File Offset: 0x0018A1C0
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

	// Token: 0x06003EEA RID: 16106 RVA: 0x0018C040 File Offset: 0x0018A240
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x040032C4 RID: 12996
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms;

	// Token: 0x040032C5 RID: 12997
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryWriter sendWriter;

	// Token: 0x040032C6 RID: 12998
	[PublicizedFrom(EAccessModifier.Private)]
	public long lengthStreamPos;

	// Token: 0x040032C7 RID: 12999
	[PublicizedFrom(EAccessModifier.Private)]
	public int numVoxelUpdates;

	// Token: 0x040032C8 RID: 13000
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] sendBytes;

	// Token: 0x040032C9 RID: 13001
	[PublicizedFrom(EAccessModifier.Private)]
	public int sendLength;
}
