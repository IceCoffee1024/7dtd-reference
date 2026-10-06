using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000363 RID: 867
public class DynamicMeshChunkData
{
	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x0600190B RID: 6411 RVA: 0x0008D68A File Offset: 0x0008B88A
	// (set) Token: 0x0600190C RID: 6412 RVA: 0x0008D692 File Offset: 0x0008B892
	public int X { get; set; }

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x0600190D RID: 6413 RVA: 0x0008D69B File Offset: 0x0008B89B
	// (set) Token: 0x0600190E RID: 6414 RVA: 0x0008D6A3 File Offset: 0x0008B8A3
	public int OffsetY { get; set; }

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x0600190F RID: 6415 RVA: 0x0008D6AC File Offset: 0x0008B8AC
	// (set) Token: 0x06001910 RID: 6416 RVA: 0x0008D6B4 File Offset: 0x0008B8B4
	public int Z { get; set; }

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001911 RID: 6417 RVA: 0x0008D6BD File Offset: 0x0008B8BD
	// (set) Token: 0x06001912 RID: 6418 RVA: 0x0008D6C5 File Offset: 0x0008B8C5
	public int UpdateTime { get; set; }

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001913 RID: 6419 RVA: 0x0008D6CE File Offset: 0x0008B8CE
	// (set) Token: 0x06001914 RID: 6420 RVA: 0x0008D6D6 File Offset: 0x0008B8D6
	public byte MainBiome { get; set; }

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001915 RID: 6421 RVA: 0x0008D6DF File Offset: 0x0008B8DF
	// (set) Token: 0x06001916 RID: 6422 RVA: 0x0008D6E7 File Offset: 0x0008B8E7
	public int EndY { get; set; }

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06001917 RID: 6423 RVA: 0x0008D6F0 File Offset: 0x0008B8F0
	// (set) Token: 0x06001918 RID: 6424 RVA: 0x0008D6F8 File Offset: 0x0008B8F8
	public List<byte> TerrainHeight { get; set; } = new List<byte>();

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06001919 RID: 6425 RVA: 0x0008D701 File Offset: 0x0008B901
	// (set) Token: 0x0600191A RID: 6426 RVA: 0x0008D709 File Offset: 0x0008B909
	public List<byte> Height { get; set; } = new List<byte>();

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x0600191B RID: 6427 RVA: 0x0008D712 File Offset: 0x0008B912
	// (set) Token: 0x0600191C RID: 6428 RVA: 0x0008D71A File Offset: 0x0008B91A
	public List<byte> TopSoil { get; set; } = new List<byte>();

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x0600191D RID: 6429 RVA: 0x0008D723 File Offset: 0x0008B923
	// (set) Token: 0x0600191E RID: 6430 RVA: 0x0008D72B File Offset: 0x0008B92B
	public List<uint> BlockRaw { get; set; } = new List<uint>();

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x0600191F RID: 6431 RVA: 0x0008D734 File Offset: 0x0008B934
	// (set) Token: 0x06001920 RID: 6432 RVA: 0x0008D73C File Offset: 0x0008B93C
	public List<sbyte> Densities { get; set; } = new List<sbyte>();

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06001921 RID: 6433 RVA: 0x0008D745 File Offset: 0x0008B945
	// (set) Token: 0x06001922 RID: 6434 RVA: 0x0008D74D File Offset: 0x0008B94D
	public List<long> Textures { get; set; } = new List<long>();

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06001923 RID: 6435 RVA: 0x0008D756 File Offset: 0x0008B956
	public int TotalBlocks
	{
		get
		{
			return Math.Min((this.EndY - this.OffsetY + 1) * 256, this.BlockRaw.Count);
		}
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x0008D77D File Offset: 0x0008B97D
	public DynamicMeshChunkData.ChunkNeighbourData GetNeighbourData(int x, int z)
	{
		return this._neighbours[x + 1, z + 1];
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x0008D790 File Offset: 0x0008B990
	public void SetNeighbourData(int x, int z, DynamicMeshChunkData.ChunkNeighbourData data)
	{
		this._neighbours[x + 1, z + 1] = data;
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x0008D7A4 File Offset: 0x0008B9A4
	public void Copy(DynamicMeshChunkData other)
	{
		if (other == null)
		{
			return;
		}
		this.Reset();
		this.X = other.X;
		this.OffsetY = other.OffsetY;
		this.MinTerrainHeight = other.MinTerrainHeight;
		this.Z = other.Z;
		this.UpdateTime = other.UpdateTime;
		this.EndY = other.EndY;
		this.MainBiome = other.MainBiome;
		this.TerrainHeight.AddRange(other.TerrainHeight);
		this.Height.AddRange(other.Height);
		this.TopSoil.AddRange(other.TopSoil);
		int totalBlocks = other.TotalBlocks;
		for (int i = 0; i < totalBlocks; i++)
		{
			this.BlockRaw.Add(other.BlockRaw[i]);
		}
		this.Densities.AddRange(other.Densities);
		this.Textures.AddRange(other.Textures);
		for (int j = -1; j < 2; j++)
		{
			for (int k = -1; k < 2; k++)
			{
				if (j != 0 || k != 0)
				{
					this.GetNeighbourData(j, k).Copy(other.GetNeighbourData(j, k));
				}
			}
		}
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x0008D8C4 File Offset: 0x0008BAC4
	public static DynamicMeshChunkData LoadFromStream(MemoryStream stream)
	{
		stream.Position = 0L;
		DynamicMeshChunkData fromCache = DynamicMeshChunkData.GetFromCache("_LoadStream_");
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			pooledBinaryReader.SetBaseStream(stream);
			fromCache.Read(pooledBinaryReader);
		}
		return fromCache;
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x0008D91C File Offset: 0x0008BB1C
	public void RecordCounts()
	{
		this.lastRaw = this.BlockRaw.Count;
		this.lastDen = this.Densities.Count;
		this.lastTex = this.Textures.Count;
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x0008D954 File Offset: 0x0008BB54
	public void ClearPreviousLayers()
	{
		if (this.lastRaw > 0)
		{
			this.BlockRaw.RemoveRange(0, this.lastRaw);
		}
		if (this.lastDen > 0)
		{
			this.Densities.RemoveRange(0, this.lastDen);
		}
		if (this.lastTex > 0)
		{
			this.Textures.RemoveRange(0, this.lastTex);
		}
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x0008D9B4 File Offset: 0x0008BBB4
	public void Reset()
	{
		this.X = 0;
		this.OffsetY = 0;
		this.Z = 0;
		this.UpdateTime = 0;
		this.MainBiome = 0;
		this.EndY = 0;
		this.TerrainHeight.Clear();
		this.Height.Clear();
		this.TopSoil.Clear();
		this.BlockRaw.Clear();
		this.Densities.Clear();
		this.Textures.Clear();
		this.MinTerrainHeight = 500;
		this.GetNeighbourData(-1, -1).Clear();
		this.GetNeighbourData(-1, 0).Clear();
		this.GetNeighbourData(-1, 1).Clear();
		this.GetNeighbourData(1, 1).Clear();
		this.GetNeighbourData(1, 0).Clear();
		this.GetNeighbourData(1, -1).Clear();
		this.GetNeighbourData(0, -1).Clear();
		this.GetNeighbourData(0, 1).Clear();
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x0008DAA0 File Offset: 0x0008BCA0
	public void SetTopSoil(byte[] soil)
	{
		this.TopSoil.AddRange(soil);
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x0008DAB0 File Offset: 0x0008BCB0
	public int GetStreamSize()
	{
		int num = 81 + this.TerrainHeight.Count + this.Height.Count + this.TopSoil.Count + 4 * this.BlockRaw.Count + this.Densities.Count + 8 * this.Textures.Count;
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				if (i != 0 || j != 0)
				{
					DynamicMeshChunkData.ChunkNeighbourData neighbourData = this.GetNeighbourData(i, j);
					num += 4 * neighbourData.BlockRaw.Count + neighbourData.Densities.Count + 8 * neighbourData.Textures.Count;
				}
			}
		}
		return num;
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x0008DB60 File Offset: 0x0008BD60
	public void Write(BinaryWriter writer)
	{
		writer.Write(this.X);
		writer.Write(this.OffsetY);
		writer.Write(this.Z);
		writer.Write(this.EndY);
		writer.Write(this.MinTerrainHeight);
		writer.Write(this.UpdateTime);
		writer.Write(this.MainBiome);
		writer.Write(this.TerrainHeight.Count);
		foreach (byte b in this.TerrainHeight)
		{
			writer.Write(b);
		}
		writer.Write(this.Height.Count);
		foreach (byte b2 in this.Height)
		{
			writer.Write(b2);
		}
		writer.Write(this.TopSoil.Count);
		foreach (byte b3 in this.TopSoil)
		{
			writer.Write(b3);
		}
		int totalBlocks = this.TotalBlocks;
		writer.Write(totalBlocks);
		for (int i = 0; i < totalBlocks; i++)
		{
			writer.Write(this.BlockRaw[i]);
		}
		writer.Write(this.Densities.Count);
		foreach (sbyte b4 in this.Densities)
		{
			writer.Write(b4);
		}
		writer.Write(this.Textures.Count);
		foreach (long num in this.Textures)
		{
			writer.Write(num);
		}
		for (int j = -1; j < 2; j++)
		{
			for (int k = -1; k < 2; k++)
			{
				if (j != 0 || k != 0)
				{
					this.GetNeighbourData(j, k).Write(writer);
				}
			}
		}
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x0008DDD8 File Offset: 0x0008BFD8
	public void Read(PooledBinaryReader reader)
	{
		this.X = reader.ReadInt32();
		this.OffsetY = reader.ReadInt32();
		this.Z = reader.ReadInt32();
		this.EndY = reader.ReadInt32();
		this.MinTerrainHeight = reader.ReadInt32();
		this.UpdateTime = reader.ReadInt32();
		this.MainBiome = reader.ReadByte();
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.TerrainHeight.Add(reader.ReadByte());
		}
		num = reader.ReadInt32();
		for (int j = 0; j < num; j++)
		{
			this.Height.Add(reader.ReadByte());
		}
		num = reader.ReadInt32();
		for (int k = 0; k < num; k++)
		{
			this.TopSoil.Add(reader.ReadByte());
		}
		num = reader.ReadInt32();
		for (int l = 0; l < num; l++)
		{
			this.BlockRaw.Add(reader.ReadUInt32());
		}
		num = reader.ReadInt32();
		for (int m = 0; m < num; m++)
		{
			this.Densities.Add(reader.ReadSByte());
		}
		num = reader.ReadInt32();
		for (int n = 0; n < num; n++)
		{
			this.Textures.Add(reader.ReadInt64());
		}
		for (int num2 = -1; num2 < 2; num2++)
		{
			for (int num3 = -1; num3 < 2; num3++)
			{
				if (num2 != 0 || num3 != 0)
				{
					this.GetNeighbourData(num2, num3).Read(reader);
				}
			}
		}
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x0008DF58 File Offset: 0x0008C158
	public void ApplyToChunk(Chunk chunk, ChunkCacheNeighborChunks cacheNeighbourChunks)
	{
		int num = 0;
		chunk.X = this.X;
		chunk.Z = this.Z;
		chunk.SetTopSoil(this.TopSoil);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				chunk.SetHeight(i, j, this.Height[num]);
				chunk.SetTerrainHeight(i, j, this.TerrainHeight[num++]);
			}
		}
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		BlockValue blockValue = default(BlockValue);
		for (int k = 0; k < 256; k++)
		{
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					chunk.SetLight(i, k, j, 15, Chunk.LIGHT_TYPE.SUN);
					if (k < this.OffsetY - 1 || k >= this.EndY)
					{
						chunk.SetBlockRaw(i, k, j, BlockValue.Air);
						chunk.SetDensity(i, k, j, MarchingCubes.DensityAir);
						chunk.SetTextureFull(i, k, j, 0L, 0);
					}
					else
					{
						blockValue.rawData = this.BlockRaw[num2];
						bool flag;
						if (flag = DynamicMeshSettings.UseImposterValues && DynamicMeshBlockSwap.BlockSwaps.TryGetValue(blockValue.type, out num4))
						{
							if (num4 == 0)
							{
								blockValue.rawData = 0U;
								num3++;
							}
							else
							{
								blockValue.type = num4;
							}
						}
						if (blockValue.rawData == 0U)
						{
							chunk.SetBlockRaw(i, k, j, BlockValue.Air);
							chunk.SetDensity(i, k, j, MarchingCubes.DensityAir);
							chunk.SetTextureFull(i, k, j, 0L, 0);
						}
						else
						{
							long num5 = (Block.list[blockValue.type].shape.IsTerrain() ? 0L : this.Textures[num3]);
							if (flag)
							{
								long num6;
								DynamicMeshBlockSwap.TextureSwaps.TryGetValue(num4, out num6);
								if (num5 == 0L && num6 != 0L)
								{
									num5 = num6 | (num6 << 8) | (num6 << 16) | (num6 << 24) | (num6 << 32) | (num6 << 40);
								}
							}
							chunk.SetBlockRaw(i, k, j, blockValue);
							chunk.SetDensity(i, k, j, this.Densities[num3]);
							chunk.SetTextureFull(i, k, j, num5, 0);
							num3++;
						}
						num2++;
					}
				}
			}
		}
		this.SetXNeighbour(15, (Chunk)cacheNeighbourChunks[-1, 0], this.GetNeighbourData(-1, 0));
		this.SetXNeighbour(0, (Chunk)cacheNeighbourChunks[1, 0], this.GetNeighbourData(1, 0));
		this.SetZNeighbour(15, (Chunk)cacheNeighbourChunks[0, -1], this.GetNeighbourData(0, -1));
		this.SetZNeighbour(0, (Chunk)cacheNeighbourChunks[0, 1], this.GetNeighbourData(0, 1));
		this.SetNeighbourCorner(15, 15, (Chunk)cacheNeighbourChunks[-1, -1], this.GetNeighbourData(-1, -1));
		this.SetNeighbourCorner(0, 15, (Chunk)cacheNeighbourChunks[1, -1], this.GetNeighbourData(1, -1));
		this.SetNeighbourCorner(15, 0, (Chunk)cacheNeighbourChunks[-1, 1], this.GetNeighbourData(-1, 1));
		this.SetNeighbourCorner(0, 0, (Chunk)cacheNeighbourChunks[1, 1], this.GetNeighbourData(1, 1));
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x0008E284 File Offset: 0x0008C484
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetNeighbourCorner(int x, int z, Chunk chunk, DynamicMeshChunkData.ChunkNeighbourData data)
	{
		BlockValue blockValue = new BlockValue(0U);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 256; i++)
		{
			blockValue.rawData = data.BlockRaw[num];
			num++;
			chunk.SetBlockRaw(x, i, z, blockValue);
			if (blockValue.rawData != 0U)
			{
				chunk.SetDensity(x, i, z, data.Densities[num2]);
				chunk.SetTextureFull(x, i, z, (long)data.Densities[num2], 0);
				num2++;
			}
			else
			{
				chunk.SetDensity(x, i, z, MarchingCubes.DensityAir);
				chunk.SetTextureFull(x, i, z, 0L, 0);
			}
		}
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x0008E32C File Offset: 0x0008C52C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetXNeighbour(int x, Chunk chunk, DynamicMeshChunkData.ChunkNeighbourData data)
	{
		BlockValue blockValue = new BlockValue(0U);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				blockValue.rawData = data.BlockRaw[num];
				num++;
				chunk.SetBlockRaw(x, i, j, blockValue);
				if (blockValue.rawData != 0U)
				{
					chunk.SetDensity(x, i, j, data.Densities[num2]);
					chunk.SetTextureFull(x, i, j, (long)data.Densities[num2], 0);
					num2++;
				}
				else
				{
					chunk.SetDensity(x, i, j, MarchingCubes.DensityAir);
					chunk.SetTextureFull(x, i, j, 0L, 0);
				}
			}
		}
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x0008E3F0 File Offset: 0x0008C5F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetZNeighbour(int z, Chunk chunk, DynamicMeshChunkData.ChunkNeighbourData data)
	{
		BlockValue blockValue = new BlockValue(0U);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				blockValue.rawData = data.BlockRaw[num];
				num++;
				chunk.SetBlockRaw(j, i, z, blockValue);
				if (blockValue.rawData != 0U)
				{
					chunk.SetDensity(j, i, z, data.Densities[num2]);
					chunk.SetTextureFull(j, i, z, (long)data.Densities[num2], 0);
					num2++;
				}
				else
				{
					chunk.SetDensity(j, i, z, MarchingCubes.DensityAir);
					chunk.SetTextureFull(j, i, z, 0L, 0);
				}
			}
		}
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x0008E4B4 File Offset: 0x0008C6B4
	public static DynamicMeshChunkData GetFromCache(string debug)
	{
		DynamicMeshChunkData dynamicMeshChunkData;
		if (!DynamicMeshChunkData.Cache.TryDequeue(out dynamicMeshChunkData))
		{
			dynamicMeshChunkData = DynamicMeshChunkData.Creates();
		}
		DynamicMeshChunkData.ActiveDataItems++;
		return dynamicMeshChunkData;
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x0008E4E2 File Offset: 0x0008C6E2
	public static void AddToCache(DynamicMeshChunkData data, string debug)
	{
		data.Reset();
		DynamicMeshChunkData.Cache.Enqueue(data);
		DynamicMeshChunkData.ActiveDataItems--;
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x0008E504 File Offset: 0x0008C704
	public static DynamicMeshChunkData Creates()
	{
		DynamicMeshChunkData dynamicMeshChunkData = new DynamicMeshChunkData
		{
			BlockRaw = new List<uint>(),
			Densities = new List<sbyte>(),
			Textures = new List<long>(),
			TopSoil = new List<byte>(32),
			Height = new List<byte>(256),
			TerrainHeight = new List<byte>(256)
		};
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				if (i != 0 || j != 0)
				{
					dynamicMeshChunkData.SetNeighbourData(i, j, DynamicMeshChunkData.ChunkNeighbourData.Create());
				}
			}
		}
		return dynamicMeshChunkData;
	}

	// Token: 0x04000FFC RID: 4092
	public int MinTerrainHeight;

	// Token: 0x04000FFD RID: 4093
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshChunkData.ChunkNeighbourData[,] _neighbours = new DynamicMeshChunkData.ChunkNeighbourData[3, 3];

	// Token: 0x04000FFE RID: 4094
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastRaw;

	// Token: 0x04000FFF RID: 4095
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDen;

	// Token: 0x04001000 RID: 4096
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastTex;

	// Token: 0x04001001 RID: 4097
	public static int ActiveDataItems = 0;

	// Token: 0x04001002 RID: 4098
	public static ConcurrentQueue<DynamicMeshChunkData> Cache = new ConcurrentQueue<DynamicMeshChunkData>();

	// Token: 0x02000364 RID: 868
	public class ChunkNeighbourData
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001938 RID: 6456 RVA: 0x0008E608 File Offset: 0x0008C808
		// (set) Token: 0x06001939 RID: 6457 RVA: 0x0008E610 File Offset: 0x0008C810
		public List<uint> BlockRaw { get; set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x0600193A RID: 6458 RVA: 0x0008E619 File Offset: 0x0008C819
		// (set) Token: 0x0600193B RID: 6459 RVA: 0x0008E621 File Offset: 0x0008C821
		public List<sbyte> Densities { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x0600193C RID: 6460 RVA: 0x0008E62A File Offset: 0x0008C82A
		// (set) Token: 0x0600193D RID: 6461 RVA: 0x0008E632 File Offset: 0x0008C832
		public List<long> Textures { get; set; }

		// Token: 0x0600193E RID: 6462 RVA: 0x0008E63B File Offset: 0x0008C83B
		public void Clear()
		{
			this.BlockRaw.Clear();
			this.Densities.Clear();
			this.Textures.Clear();
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0008E65E File Offset: 0x0008C85E
		public void SetData(uint blockraw, sbyte density, long texture)
		{
			this.BlockRaw.Add(blockraw);
			if (blockraw == 0U)
			{
				return;
			}
			this.Densities.Add(density);
			this.Textures.Add(texture);
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0008E688 File Offset: 0x0008C888
		public void Copy(DynamicMeshChunkData.ChunkNeighbourData other)
		{
			this.BlockRaw.Clear();
			this.Densities.Clear();
			this.Textures.Clear();
			this.BlockRaw.AddRange(other.BlockRaw);
			this.Densities.AddRange(other.Densities);
			this.Textures.AddRange(other.Textures);
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0008E6EC File Offset: 0x0008C8EC
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.BlockRaw.Count);
			foreach (uint num in this.BlockRaw)
			{
				writer.Write(num);
			}
			writer.Write(this.Densities.Count);
			foreach (sbyte b in this.Densities)
			{
				writer.Write(b);
			}
			writer.Write(this.Textures.Count);
			foreach (long num2 in this.Textures)
			{
				writer.Write(num2);
			}
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0008E7FC File Offset: 0x0008C9FC
		public void Read(PooledBinaryReader reader)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				this.BlockRaw.Add(reader.ReadUInt32());
			}
			num = reader.ReadInt32();
			for (int j = 0; j < num; j++)
			{
				this.Densities.Add(reader.ReadSByte());
			}
			num = reader.ReadInt32();
			for (int k = 0; k < num; k++)
			{
				this.Textures.Add(reader.ReadInt64());
			}
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0008E875 File Offset: 0x0008CA75
		public static DynamicMeshChunkData.ChunkNeighbourData Create()
		{
			return new DynamicMeshChunkData.ChunkNeighbourData
			{
				BlockRaw = new List<uint>(),
				Densities = new List<sbyte>(),
				Textures = new List<long>()
			};
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0008E89D File Offset: 0x0008CA9D
		public static DynamicMeshChunkData.ChunkNeighbourData CreateMax()
		{
			return new DynamicMeshChunkData.ChunkNeighbourData
			{
				BlockRaw = new List<uint>(65280),
				Densities = new List<sbyte>(65280),
				Textures = new List<long>(65280)
			};
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0008E8D4 File Offset: 0x0008CAD4
		public static DynamicMeshChunkData.ChunkNeighbourData CreateCorner()
		{
			return new DynamicMeshChunkData.ChunkNeighbourData
			{
				BlockRaw = new List<uint>(255),
				Densities = new List<sbyte>(255),
				Textures = new List<long>(255)
			};
		}
	}
}
