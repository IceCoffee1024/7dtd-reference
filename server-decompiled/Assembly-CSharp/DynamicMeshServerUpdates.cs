using System;
using System.Collections.Generic;

// Token: 0x0200037B RID: 891
public class DynamicMeshServerUpdates
{
	// Token: 0x06001A31 RID: 6705 RVA: 0x0009B27C File Offset: 0x0009947C
	public static void AddToPool(DynamicMeshServerUpdates data)
	{
		if (DynamicMeshServerUpdates.Pool.Count > 40)
		{
			data.Bytes = null;
			return;
		}
		DynamicMeshServerUpdates.Pool.Enqueue(data);
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x0009B29F File Offset: 0x0009949F
	public static DynamicMeshServerUpdates GetFromPool()
	{
		if (DynamicMeshServerUpdates.Pool.Count > 0)
		{
			return DynamicMeshServerUpdates.Pool.Dequeue();
		}
		return new DynamicMeshServerUpdates();
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x0009B2BE File Offset: 0x000994BE
	public long GetKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundChunk(this.ChunkX)), World.toChunkXZ(DynamicMeshUnity.RoundChunk(this.ChunkZ)));
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06001A34 RID: 6708 RVA: 0x0009B2E5 File Offset: 0x000994E5
	// (set) Token: 0x06001A35 RID: 6709 RVA: 0x0009B2ED File Offset: 0x000994ED
	public int ChunkX { get; set; }

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06001A36 RID: 6710 RVA: 0x0009B2F6 File Offset: 0x000994F6
	// (set) Token: 0x06001A37 RID: 6711 RVA: 0x0009B2FE File Offset: 0x000994FE
	public int ChunkZ { get; set; }

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06001A38 RID: 6712 RVA: 0x0009B307 File Offset: 0x00099507
	// (set) Token: 0x06001A39 RID: 6713 RVA: 0x0009B30F File Offset: 0x0009950F
	public int StartY { get; set; }

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06001A3A RID: 6714 RVA: 0x0009B318 File Offset: 0x00099518
	// (set) Token: 0x06001A3B RID: 6715 RVA: 0x0009B320 File Offset: 0x00099520
	public int EndY { get; set; }

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06001A3C RID: 6716 RVA: 0x0009B329 File Offset: 0x00099529
	// (set) Token: 0x06001A3D RID: 6717 RVA: 0x0009B331 File Offset: 0x00099531
	public int UpdateTime { get; set; }

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06001A3E RID: 6718 RVA: 0x0009B33A File Offset: 0x0009953A
	// (set) Token: 0x06001A3F RID: 6719 RVA: 0x0009B342 File Offset: 0x00099542
	public List<byte> Bytes { get; set; }

	// Token: 0x06001A40 RID: 6720 RVA: 0x0009B34B File Offset: 0x0009954B
	public DynamicMeshServerUpdates()
	{
		this.Bytes = new List<byte>();
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x0009B35E File Offset: 0x0009955E
	public void WriteAir(List<byte> tempArray)
	{
		tempArray.Add(0);
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x0009B367 File Offset: 0x00099567
	public void WriteLayer(List<byte> tempArray)
	{
		this.DataLayerCount++;
		this.Bytes.Add(byte.MaxValue);
		this.Bytes.AddRange(tempArray);
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x0009B393 File Offset: 0x00099593
	public void WriteEmptyLayer()
	{
		this.EmptyLayerCount++;
		this.Bytes.Add(128);
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x0009B3B4 File Offset: 0x000995B4
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

	// Token: 0x040010BE RID: 4286
	public const int DataLayer = 255;

	// Token: 0x040010BF RID: 4287
	public const byte EmptyLayer = 128;

	// Token: 0x040010C0 RID: 4288
	public const int EmptyBlock = 0;

	// Token: 0x040010C1 RID: 4289
	[PublicizedFrom(EAccessModifier.Private)]
	public static Queue<DynamicMeshServerUpdates> Pool = new Queue<DynamicMeshServerUpdates>(20);

	// Token: 0x040010C2 RID: 4290
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<uint, byte[]> BlockBytes = new Dictionary<uint, byte[]>();

	// Token: 0x040010C3 RID: 4291
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<long, byte[]> TexBytes = new Dictionary<long, byte[]>();

	// Token: 0x040010CA RID: 4298
	public int EmptyLayerCount;

	// Token: 0x040010CB RID: 4299
	public int DataLayerCount;
}
