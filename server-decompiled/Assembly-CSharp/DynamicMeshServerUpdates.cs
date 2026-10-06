using System;
using System.Collections.Generic;

// Token: 0x02000379 RID: 889
public class DynamicMeshServerUpdates
{
	// Token: 0x06001A2D RID: 6701 RVA: 0x0009B048 File Offset: 0x00099248
	public static void AddToPool(DynamicMeshServerUpdates data)
	{
		if (DynamicMeshServerUpdates.Pool.Count > 40)
		{
			data.Bytes = null;
			return;
		}
		DynamicMeshServerUpdates.Pool.Enqueue(data);
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x0009B06B File Offset: 0x0009926B
	public static DynamicMeshServerUpdates GetFromPool()
	{
		if (DynamicMeshServerUpdates.Pool.Count > 0)
		{
			return DynamicMeshServerUpdates.Pool.Dequeue();
		}
		return new DynamicMeshServerUpdates();
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x0009B08A File Offset: 0x0009928A
	public long GetKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundChunk(this.ChunkX)), World.toChunkXZ(DynamicMeshUnity.RoundChunk(this.ChunkZ)));
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06001A30 RID: 6704 RVA: 0x0009B0B1 File Offset: 0x000992B1
	// (set) Token: 0x06001A31 RID: 6705 RVA: 0x0009B0B9 File Offset: 0x000992B9
	public int ChunkX { get; set; }

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06001A32 RID: 6706 RVA: 0x0009B0C2 File Offset: 0x000992C2
	// (set) Token: 0x06001A33 RID: 6707 RVA: 0x0009B0CA File Offset: 0x000992CA
	public int ChunkZ { get; set; }

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06001A34 RID: 6708 RVA: 0x0009B0D3 File Offset: 0x000992D3
	// (set) Token: 0x06001A35 RID: 6709 RVA: 0x0009B0DB File Offset: 0x000992DB
	public int StartY { get; set; }

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06001A36 RID: 6710 RVA: 0x0009B0E4 File Offset: 0x000992E4
	// (set) Token: 0x06001A37 RID: 6711 RVA: 0x0009B0EC File Offset: 0x000992EC
	public int EndY { get; set; }

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06001A38 RID: 6712 RVA: 0x0009B0F5 File Offset: 0x000992F5
	// (set) Token: 0x06001A39 RID: 6713 RVA: 0x0009B0FD File Offset: 0x000992FD
	public int UpdateTime { get; set; }

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06001A3A RID: 6714 RVA: 0x0009B106 File Offset: 0x00099306
	// (set) Token: 0x06001A3B RID: 6715 RVA: 0x0009B10E File Offset: 0x0009930E
	public List<byte> Bytes { get; set; }

	// Token: 0x06001A3C RID: 6716 RVA: 0x0009B117 File Offset: 0x00099317
	public DynamicMeshServerUpdates()
	{
		this.Bytes = new List<byte>();
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x0009B12A File Offset: 0x0009932A
	public void WriteAir(List<byte> tempArray)
	{
		tempArray.Add(0);
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x0009B133 File Offset: 0x00099333
	public void WriteLayer(List<byte> tempArray)
	{
		this.DataLayerCount++;
		this.Bytes.Add(byte.MaxValue);
		this.Bytes.AddRange(tempArray);
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x0009B15F File Offset: 0x0009935F
	public void WriteEmptyLayer()
	{
		this.EmptyLayerCount++;
		this.Bytes.Add(128);
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x0009B180 File Offset: 0x00099380
	public void WriteBinaryBlock(List<byte> tempArray, BlockValue b, sbyte dens, long tex)
	{
		if (b.isair)
		{
			tempArray.Add(0);
			return;
		}
		byte[] array;
		if (!DynamicMeshServerUpdates.BlockBytes.TryGetValue(b.rawData, out array))
		{
			array = BitConverter.GetBytes(b.type);
			DynamicMeshServerUpdates.BlockBytes.Add(b.rawData, array);
		}
		tempArray.AddRange(array);
		if (!DynamicMeshServerUpdates.TexBytes.TryGetValue(tex, out array))
		{
			array = BitConverter.GetBytes(tex);
			DynamicMeshServerUpdates.TexBytes.Add(tex, array);
		}
		tempArray.AddRange(array);
		tempArray.Add((byte)dens);
	}

	// Token: 0x040010AE RID: 4270
	public const int DataLayer = 255;

	// Token: 0x040010AF RID: 4271
	public const byte EmptyLayer = 128;

	// Token: 0x040010B0 RID: 4272
	public const int EmptyBlock = 0;

	// Token: 0x040010B1 RID: 4273
	[PublicizedFrom(EAccessModifier.Private)]
	public static Queue<DynamicMeshServerUpdates> Pool = new Queue<DynamicMeshServerUpdates>(20);

	// Token: 0x040010B2 RID: 4274
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<uint, byte[]> BlockBytes = new Dictionary<uint, byte[]>();

	// Token: 0x040010B3 RID: 4275
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<long, byte[]> TexBytes = new Dictionary<long, byte[]>();

	// Token: 0x040010BA RID: 4282
	public int EmptyLayerCount;

	// Token: 0x040010BB RID: 4283
	public int DataLayerCount;
}
