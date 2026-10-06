using System;
using System.Collections.Generic;

// Token: 0x02000C38 RID: 3128
public class DChunkSquareMeshPool
{
	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x06005EE9 RID: 24297 RVA: 0x00252B48 File Offset: 0x00250D48
	public int Count
	{
		get
		{
			object poolLock = this._poolLock;
			int num2;
			lock (poolLock)
			{
				int num = 0;
				for (int i = 0; i < this.pool.Length; i++)
				{
					num += this.pool[i].Count;
				}
				num2 = num;
			}
			return num2;
		}
	}

	// Token: 0x06005EEA RID: 24298 RVA: 0x00252BAC File Offset: 0x00250DAC
	public DChunkSquareMeshPool(int initialCapacity, int NbLODLevel)
	{
		this._poolLock = new object();
		this.pool = new List<DChunkSquareMesh>[NbLODLevel];
		for (int i = 0; i < NbLODLevel; i++)
		{
			this.pool[i] = new List<DChunkSquareMesh>(initialCapacity);
		}
	}

	// Token: 0x06005EEB RID: 24299 RVA: 0x00252BF0 File Offset: 0x00250DF0
	public DChunkSquareMesh GetObject(DistantChunkMap DCMap, int LODLevel)
	{
		object poolLock = this._poolLock;
		DChunkSquareMesh dchunkSquareMesh;
		lock (poolLock)
		{
			List<DChunkSquareMesh> list = this.pool[LODLevel];
			if (list.Count == 0)
			{
				dchunkSquareMesh = new DChunkSquareMesh(DCMap, LODLevel);
			}
			else
			{
				DChunkSquareMesh dchunkSquareMesh2 = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
				dchunkSquareMesh = dchunkSquareMesh2;
			}
		}
		return dchunkSquareMesh;
	}

	// Token: 0x06005EEC RID: 24300 RVA: 0x00252C68 File Offset: 0x00250E68
	public void ReturnObject(DChunkSquareMesh item, int LODLevel)
	{
		if (item == null)
		{
			throw new ArgumentNullException("DChunkSquareMesh is null");
		}
		object poolLock = this._poolLock;
		lock (poolLock)
		{
			if (this.pool[LODLevel].Contains(item))
			{
				throw new InvalidOperationException("ThreadProcessing already in pool");
			}
			this.pool[LODLevel].Add(item);
		}
	}

	// Token: 0x040049FF RID: 18943
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DChunkSquareMesh>[] pool;

	// Token: 0x04004A00 RID: 18944
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
