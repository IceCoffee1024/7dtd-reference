using System;
using UnityEngine.Scripting;

// Token: 0x02000876 RID: 2166
[Preserve]
public class NetPackageWaterSimChunkUpdate : NetPackage, IMemoryPoolableObject
{
	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x06003E9B RID: 16027 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0018CC22 File Offset: 0x0018AE22
	public static int GetPoolSize()
	{
		return 200;
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x0018CC2C File Offset: 0x0018AE2C
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

	// Token: 0x06003E9E RID: 16030 RVA: 0x0018CCB8 File Offset: 0x0018AEB8
	public void AddChange(ushort _voxelIndex, WaterValue _newValue)
	{
		this.sendWriter.Write(_voxelIndex);
		_newValue.Write(this.sendWriter);
		this.numVoxelUpdates++;
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x0018CCE4 File Offset: 0x0018AEE4
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

	// Token: 0x06003EA0 RID: 16032 RVA: 0x0018CD90 File Offset: 0x0018AF90
	public override void read(PooledBinaryReader _br)
	{
		this.ms = MemoryPools.poolMemoryStream.AllocSync(true);
		this.ms.Position = 0L;
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003EA1 RID: 16033 RVA: 0x0018CDD6 File Offset: 0x0018AFD6
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.sendLength);
		_bw.Write(this.sendBytes, 0, this.sendLength);
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x0018CE00 File Offset: 0x0018B000
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

	// Token: 0x06003EA3 RID: 16035 RVA: 0x0018CED4 File Offset: 0x0018B0D4
	public int GetPackageLength()
	{
		return this.sendLength + 4;
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x0018CEE0 File Offset: 0x0018B0E0
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

	// Token: 0x06003EA5 RID: 16037 RVA: 0x0018CF60 File Offset: 0x0018B160
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x040032CF RID: 13007
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms;

	// Token: 0x040032D0 RID: 13008
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryWriter sendWriter;

	// Token: 0x040032D1 RID: 13009
	[PublicizedFrom(EAccessModifier.Private)]
	public long lengthStreamPos;

	// Token: 0x040032D2 RID: 13010
	[PublicizedFrom(EAccessModifier.Private)]
	public int numVoxelUpdates;

	// Token: 0x040032D3 RID: 13011
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] sendBytes;

	// Token: 0x040032D4 RID: 13012
	[PublicizedFrom(EAccessModifier.Private)]
	public int sendLength;
}
