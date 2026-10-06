using System;
using UnityEngine.Scripting;

// Token: 0x0200086C RID: 2156
[Preserve]
public class NetPackageWaterSimChunkUpdate : NetPackage, IMemoryPoolableObject
{
	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06003E78 RID: 15992 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0018BB7A File Offset: 0x00189D7A
	public static int GetPoolSize()
	{
		return 200;
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0018BB84 File Offset: 0x00189D84
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

	// Token: 0x06003E7B RID: 15995 RVA: 0x0018BC10 File Offset: 0x00189E10
	public void AddChange(ushort _voxelIndex, WaterValue _newValue)
	{
		this.sendWriter.Write(_voxelIndex);
		_newValue.Write(this.sendWriter);
		this.numVoxelUpdates++;
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x0018BC3C File Offset: 0x00189E3C
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

	// Token: 0x06003E7D RID: 15997 RVA: 0x0018BCE8 File Offset: 0x00189EE8
	public override void read(PooledBinaryReader _br)
	{
		this.ms = MemoryPools.poolMemoryStream.AllocSync(true);
		this.ms.Position = 0L;
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0018BD2E File Offset: 0x00189F2E
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.sendLength);
		_bw.Write(this.sendBytes, 0, this.sendLength);
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0018BD58 File Offset: 0x00189F58
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

	// Token: 0x06003E80 RID: 16000 RVA: 0x0018BE2C File Offset: 0x0018A02C
	public int GetPackageLength()
	{
		return this.sendLength + 4;
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x0018BE38 File Offset: 0x0018A038
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

	// Token: 0x06003E82 RID: 16002 RVA: 0x0018BEB8 File Offset: 0x0018A0B8
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x040032AF RID: 12975
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms;

	// Token: 0x040032B0 RID: 12976
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryWriter sendWriter;

	// Token: 0x040032B1 RID: 12977
	[PublicizedFrom(EAccessModifier.Private)]
	public long lengthStreamPos;

	// Token: 0x040032B2 RID: 12978
	[PublicizedFrom(EAccessModifier.Private)]
	public int numVoxelUpdates;

	// Token: 0x040032B3 RID: 12979
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] sendBytes;

	// Token: 0x040032B4 RID: 12980
	[PublicizedFrom(EAccessModifier.Private)]
	public int sendLength;
}
