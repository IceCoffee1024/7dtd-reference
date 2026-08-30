using System;
using System.Collections.Generic;

// Token: 0x02000376 RID: 886
public class DynamicMeshServerUpdates
{
	// Token: 0x06001A32 RID: 6706 RVA: 0x0009ADC8 File Offset: 0x00098FC8
	public static void AddToPool(DynamicMeshServerUpdates data)
	{
		if (DynamicMeshServerUpdates.Pool.Count > 40)
		{
			data.Bytes = null;
			return;
		}
		DynamicMeshServerUpdates.Pool.Enqueue(data);
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x0009ADEB File Offset: 0x00098FEB
	public static DynamicMeshServerUpdates GetFromPool()
	{
		if (DynamicMeshServerUpdates.Pool.Count > 0)
		{
			return DynamicMeshServerUpdates.Pool.Dequeue();
		}
		return new DynamicMeshServerUpdates();
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x0009AE0A File Offset: 0x0009900A
	public long GetKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundChunk(this.ChunkX)), World.toChunkXZ(DynamicMeshUnity.RoundChunk(this.ChunkZ)));
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06001A35 RID: 6709 RVA: 0x0009AE31 File Offset: 0x00099031
	// (set) Token: 0x06001A36 RID: 6710 RVA: 0x0009AE39 File Offset: 0x00099039
	public int ChunkX { get; set; }

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06001A37 RID: 6711 RVA: 0x0009AE42 File Offset: 0x00099042
	// (set) Token: 0x06001A38 RID: 6712 RVA: 0x0009AE4A File Offset: 0x0009904A
	public int ChunkZ { get; set; }

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06001A39 RID: 6713 RVA: 0x0009AE53 File Offset: 0x00099053
	// (set) Token: 0x06001A3A RID: 6714 RVA: 0x0009AE5B File Offset: 0x0009905B
	public int StartY { get; set; }

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06001A3B RID: 6715 RVA: 0x0009AE64 File Offset: 0x00099064
	// (set) Token: 0x06001A3C RID: 6716 RVA: 0x0009AE6C File Offset: 0x0009906C
	public int EndY { get; set; }

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06001A3D RID: 6717 RVA: 0x0009AE75 File Offset: 0x00099075
	// (set) Token: 0x06001A3E RID: 6718 RVA: 0x0009AE7D File Offset: 0x0009907D
	public int UpdateTime { get; set; }

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06001A3F RID: 6719 RVA: 0x0009AE86 File Offset: 0x00099086
	// (set) Token: 0x06001A40 RID: 6720 RVA: 0x0009AE8E File Offset: 0x0009908E
	public List<byte> Bytes { get; set; }

	// Token: 0x06001A41 RID: 6721 RVA: 0x0009AE97 File Offset: 0x00099097
	public DynamicMeshServerUpdates()
	{
		this.Bytes = new List<byte>();
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x0009AEAA File Offset: 0x000990AA
	public void WriteAir(List<byte> tempArray)
	{
		tempArray.Add(0);
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x0009AEB3 File Offset: 0x000990B3
	public void WriteLayer(List<byte> tempArray)
	{
		this.DataLayerCount++;
		this.Bytes.Add(byte.MaxValue);
		this.Bytes.AddRange(tempArray);
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x0009AEDF File Offset: 0x000990DF
	public void WriteEmptyLayer()
	{
		this.EmptyLayerCount++;
		this.Bytes.Add(128);
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x0009AF00 File Offset: 0x00099100
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

	// Token: 0x040010B5 RID: 4277
	public const int DataLayer = 255;

	// Token: 0x040010B6 RID: 4278
	public const byte EmptyLayer = 128;

	// Token: 0x040010B7 RID: 4279
	public const int EmptyBlock = 0;

	// Token: 0x040010B8 RID: 4280
	[PublicizedFrom(EAccessModifier.Private)]
	public static Queue<DynamicMeshServerUpdates> Pool = new Queue<DynamicMeshServerUpdates>(20);

	// Token: 0x040010B9 RID: 4281
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<uint, byte[]> BlockBytes = new Dictionary<uint, byte[]>();

	// Token: 0x040010BA RID: 4282
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<long, byte[]> TexBytes = new Dictionary<long, byte[]>();

	// Token: 0x040010C1 RID: 4289
	public int EmptyLayerCount;

	// Token: 0x040010C2 RID: 4290
	public int DataLayerCount;
}
