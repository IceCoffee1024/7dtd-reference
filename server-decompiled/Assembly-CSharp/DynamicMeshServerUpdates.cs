using System;
using System.Collections.Generic;

// Token: 0x02000372 RID: 882
public class DynamicMeshServerUpdates
{
	// Token: 0x06001A15 RID: 6677 RVA: 0x0009A688 File Offset: 0x00098888
	public static void AddToPool(DynamicMeshServerUpdates data)
	{
		if (DynamicMeshServerUpdates.Pool.Count > 40)
		{
			data.Bytes = null;
			return;
		}
		DynamicMeshServerUpdates.Pool.Enqueue(data);
	}

	// Token: 0x06001A16 RID: 6678 RVA: 0x0009A6AB File Offset: 0x000988AB
	public static DynamicMeshServerUpdates GetFromPool()
	{
		if (DynamicMeshServerUpdates.Pool.Count > 0)
		{
			return DynamicMeshServerUpdates.Pool.Dequeue();
		}
		return new DynamicMeshServerUpdates();
	}

	// Token: 0x06001A17 RID: 6679 RVA: 0x0009A6CA File Offset: 0x000988CA
	public long GetKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundChunk(this.ChunkX)), World.toChunkXZ(DynamicMeshUnity.RoundChunk(this.ChunkZ)));
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06001A18 RID: 6680 RVA: 0x0009A6F1 File Offset: 0x000988F1
	// (set) Token: 0x06001A19 RID: 6681 RVA: 0x0009A6F9 File Offset: 0x000988F9
	public int ChunkX { get; set; }

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06001A1A RID: 6682 RVA: 0x0009A702 File Offset: 0x00098902
	// (set) Token: 0x06001A1B RID: 6683 RVA: 0x0009A70A File Offset: 0x0009890A
	public int ChunkZ { get; set; }

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06001A1C RID: 6684 RVA: 0x0009A713 File Offset: 0x00098913
	// (set) Token: 0x06001A1D RID: 6685 RVA: 0x0009A71B File Offset: 0x0009891B
	public int StartY { get; set; }

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06001A1E RID: 6686 RVA: 0x0009A724 File Offset: 0x00098924
	// (set) Token: 0x06001A1F RID: 6687 RVA: 0x0009A72C File Offset: 0x0009892C
	public int EndY { get; set; }

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0009A735 File Offset: 0x00098935
	// (set) Token: 0x06001A21 RID: 6689 RVA: 0x0009A73D File Offset: 0x0009893D
	public int UpdateTime { get; set; }

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06001A22 RID: 6690 RVA: 0x0009A746 File Offset: 0x00098946
	// (set) Token: 0x06001A23 RID: 6691 RVA: 0x0009A74E File Offset: 0x0009894E
	public List<byte> Bytes { get; set; }

	// Token: 0x06001A24 RID: 6692 RVA: 0x0009A757 File Offset: 0x00098957
	public DynamicMeshServerUpdates()
	{
		this.Bytes = new List<byte>();
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x0009A76A File Offset: 0x0009896A
	public void WriteAir(List<byte> tempArray)
	{
		tempArray.Add(0);
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x0009A773 File Offset: 0x00098973
	public void WriteLayer(List<byte> tempArray)
	{
		this.DataLayerCount++;
		this.Bytes.Add(byte.MaxValue);
		this.Bytes.AddRange(tempArray);
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x0009A79F File Offset: 0x0009899F
	public void WriteEmptyLayer()
	{
		this.EmptyLayerCount++;
		this.Bytes.Add(128);
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x0009A7C0 File Offset: 0x000989C0
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

	// Token: 0x040010A0 RID: 4256
	public const int DataLayer = 255;

	// Token: 0x040010A1 RID: 4257
	public const byte EmptyLayer = 128;

	// Token: 0x040010A2 RID: 4258
	public const int EmptyBlock = 0;

	// Token: 0x040010A3 RID: 4259
	[PublicizedFrom(EAccessModifier.Private)]
	public static Queue<DynamicMeshServerUpdates> Pool = new Queue<DynamicMeshServerUpdates>(20);

	// Token: 0x040010A4 RID: 4260
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<uint, byte[]> BlockBytes = new Dictionary<uint, byte[]>();

	// Token: 0x040010A5 RID: 4261
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<long, byte[]> TexBytes = new Dictionary<long, byte[]>();

	// Token: 0x040010AC RID: 4268
	public int EmptyLayerCount;

	// Token: 0x040010AD RID: 4269
	public int DataLayerCount;
}
