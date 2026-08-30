using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200035C RID: 860
public class DynamicMeshChunkData
{
	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x060018F3 RID: 6387 RVA: 0x0008CCCA File Offset: 0x0008AECA
	// (set) Token: 0x060018F4 RID: 6388 RVA: 0x0008CCD2 File Offset: 0x0008AED2
	public int X { get; set; }

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x060018F5 RID: 6389 RVA: 0x0008CCDB File Offset: 0x0008AEDB
	// (set) Token: 0x060018F6 RID: 6390 RVA: 0x0008CCE3 File Offset: 0x0008AEE3
	public int OffsetY { get; set; }

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x060018F7 RID: 6391 RVA: 0x0008CCEC File Offset: 0x0008AEEC
	// (set) Token: 0x060018F8 RID: 6392 RVA: 0x0008CCF4 File Offset: 0x0008AEF4
	public int Z { get; set; }

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x060018F9 RID: 6393 RVA: 0x0008CCFD File Offset: 0x0008AEFD
	// (set) Token: 0x060018FA RID: 6394 RVA: 0x0008CD05 File Offset: 0x0008AF05
	public int UpdateTime { get; set; }

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x060018FB RID: 6395 RVA: 0x0008CD0E File Offset: 0x0008AF0E
	// (set) Token: 0x060018FC RID: 6396 RVA: 0x0008CD16 File Offset: 0x0008AF16
	public byte MainBiome { get; set; }

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x060018FD RID: 6397 RVA: 0x0008CD1F File Offset: 0x0008AF1F
	// (set) Token: 0x060018FE RID: 6398 RVA: 0x0008CD27 File Offset: 0x0008AF27
	public int EndY { get; set; }

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x060018FF RID: 6399 RVA: 0x0008CD30 File Offset: 0x0008AF30
	// (set) Token: 0x06001900 RID: 6400 RVA: 0x0008CD38 File Offset: 0x0008AF38
	public List<byte> TerrainHeight { get; set; } = new List<byte>();

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06001901 RID: 6401 RVA: 0x0008CD41 File Offset: 0x0008AF41
	// (set) Token: 0x06001902 RID: 6402 RVA: 0x0008CD49 File Offset: 0x0008AF49
	public List<byte> Height { get; set; } = new List<byte>();

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06001903 RID: 6403 RVA: 0x0008CD52 File Offset: 0x0008AF52
	// (set) Token: 0x06001904 RID: 6404 RVA: 0x0008CD5A File Offset: 0x0008AF5A
	public List<byte> TopSoil { get; set; } = new List<byte>();

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06001905 RID: 6405 RVA: 0x0008CD63 File Offset: 0x0008AF63
	// (set) Token: 0x06001906 RID: 6406 RVA: 0x0008CD6B File Offset: 0x0008AF6B
	public List<uint> BlockRaw { get; set; } = new List<uint>();

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06001907 RID: 6407 RVA: 0x0008CD74 File Offset: 0x0008AF74
	// (set) Token: 0x06001908 RID: 6408 RVA: 0x0008CD7C File Offset: 0x0008AF7C
	public List<sbyte> Densities { get; set; } = new List<sbyte>();

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06001909 RID: 6409 RVA: 0x0008CD85 File Offset: 0x0008AF85
	// (set) Token: 0x0600190A RID: 6410 RVA: 0x0008CD8D File Offset: 0x0008AF8D
	public List<long> Textures { get; set; } = new List<long>();

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x0600190B RID: 6411 RVA: 0x0008CD96 File Offset: 0x0008AF96
	public int TotalBlocks
	{
		get
		{
			return Math.Min((this.EndY - this.OffsetY + 1) * 256, this.BlockRaw.Count);
		}
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x0008CDBD File Offset: 0x0008AFBD
	public DynamicMeshChunkData.ChunkNeighbourData GetNeighbourData(int x, int z)
	{
		return this._neighbours[x + 1, z + 1];
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x0008CDD0 File Offset: 0x0008AFD0
	public void SetNeighbourData(int x, int z, DynamicMeshChunkData.ChunkNeighbourData data)
	{
		this._neighbours[x + 1, z + 1] = data;
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x0008CDE4 File Offset: 0x0008AFE4
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

	// Token: 0x0600190F RID: 6415 RVA: 0x0008CF04 File Offset: 0x0008B104
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

	// Token: 0x06001910 RID: 6416 RVA: 0x0008CF5C File Offset: 0x0008B15C
	public void RecordCounts()
	{
		this.lastRaw = this.BlockRaw.Count;
		this.lastDen = this.Densities.Count;
		this.lastTex = this.Textures.Count;
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x0008CF94 File Offset: 0x0008B194
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

	// Token: 0x06001912 RID: 6418 RVA: 0x0008CFF4 File Offset: 0x0008B1F4
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

	// Token: 0x06001913 RID: 6419 RVA: 0x0008D0E0 File Offset: 0x0008B2E0
	public void SetTopSoil(byte[] soil)
	{
		this.TopSoil.AddRange(soil);
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x0008D0F0 File Offset: 0x0008B2F0
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

	// Token: 0x06001915 RID: 6421 RVA: 0x0008D1A0 File Offset: 0x0008B3A0
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

	// Token: 0x06001916 RID: 6422 RVA: 0x0008D418 File Offset: 0x0008B618
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

	// Token: 0x06001917 RID: 6423 RVA: 0x0008D598 File Offset: 0x0008B798
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

	// Token: 0x06001918 RID: 6424 RVA: 0x0008D8C4 File Offset: 0x0008BAC4
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

	// Token: 0x06001919 RID: 6425 RVA: 0x0008D96C File Offset: 0x0008BB6C
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

	// Token: 0x0600191A RID: 6426 RVA: 0x0008DA30 File Offset: 0x0008BC30
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

	// Token: 0x0600191B RID: 6427 RVA: 0x0008DAF4 File Offset: 0x0008BCF4
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

	// Token: 0x0600191C RID: 6428 RVA: 0x0008DB22 File Offset: 0x0008BD22
	public static void AddToCache(DynamicMeshChunkData data, string debug)
	{
		data.Reset();
		DynamicMeshChunkData.Cache.Enqueue(data);
		DynamicMeshChunkData.ActiveDataItems--;
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x0008DB44 File Offset: 0x0008BD44
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

	// Token: 0x04000FEE RID: 4078
	public int MinTerrainHeight;

	// Token: 0x04000FEF RID: 4079
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshChunkData.ChunkNeighbourData[,] _neighbours = new DynamicMeshChunkData.ChunkNeighbourData[3, 3];

	// Token: 0x04000FF0 RID: 4080
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastRaw;

	// Token: 0x04000FF1 RID: 4081
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDen;

	// Token: 0x04000FF2 RID: 4082
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastTex;

	// Token: 0x04000FF3 RID: 4083
	public static int ActiveDataItems = 0;

	// Token: 0x04000FF4 RID: 4084
	public static ConcurrentQueue<DynamicMeshChunkData> Cache = new ConcurrentQueue<DynamicMeshChunkData>();

	// Token: 0x0200035D RID: 861
	public class ChunkNeighbourData
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001920 RID: 6432 RVA: 0x0008DC48 File Offset: 0x0008BE48
		// (set) Token: 0x06001921 RID: 6433 RVA: 0x0008DC50 File Offset: 0x0008BE50
		public List<uint> BlockRaw { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06001922 RID: 6434 RVA: 0x0008DC59 File Offset: 0x0008BE59
		// (set) Token: 0x06001923 RID: 6435 RVA: 0x0008DC61 File Offset: 0x0008BE61
		public List<sbyte> Densities { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001924 RID: 6436 RVA: 0x0008DC6A File Offset: 0x0008BE6A
		// (set) Token: 0x06001925 RID: 6437 RVA: 0x0008DC72 File Offset: 0x0008BE72
		public List<long> Textures { get; set; }

		// Token: 0x06001926 RID: 6438 RVA: 0x0008DC7B File Offset: 0x0008BE7B
		public void Clear()
		{
			this.BlockRaw.Clear();
			this.Densities.Clear();
			this.Textures.Clear();
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0008DC9E File Offset: 0x0008BE9E
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

		// Token: 0x06001928 RID: 6440 RVA: 0x0008DCC8 File Offset: 0x0008BEC8
		public void Copy(DynamicMeshChunkData.ChunkNeighbourData other)
		{
			this.BlockRaw.Clear();
			this.Densities.Clear();
			this.Textures.Clear();
			this.BlockRaw.AddRange(other.BlockRaw);
			this.Densities.AddRange(other.Densities);
			this.Textures.AddRange(other.Textures);
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0008DD2C File Offset: 0x0008BF2C
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

		// Token: 0x0600192A RID: 6442 RVA: 0x0008DE3C File Offset: 0x0008C03C
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

		// Token: 0x0600192B RID: 6443 RVA: 0x0008DEB5 File Offset: 0x0008C0B5
		public static DynamicMeshChunkData.ChunkNeighbourData Create()
		{
			return new DynamicMeshChunkData.ChunkNeighbourData
			{
				BlockRaw = new List<uint>(),
				Densities = new List<sbyte>(),
				Textures = new List<long>()
			};
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0008DEDD File Offset: 0x0008C0DD
		public static DynamicMeshChunkData.ChunkNeighbourData CreateMax()
		{
			return new DynamicMeshChunkData.ChunkNeighbourData
			{
				BlockRaw = new List<uint>(65280),
				Densities = new List<sbyte>(65280),
				Textures = new List<long>(65280)
			};
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0008DF14 File Offset: 0x0008C114
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
