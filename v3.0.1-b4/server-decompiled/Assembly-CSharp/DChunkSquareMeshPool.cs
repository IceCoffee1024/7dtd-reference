using System;
using System.Collections.Generic;

// Token: 0x02000C14 RID: 3092
public class DChunkSquareMeshPool
{
	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x06005E46 RID: 24134 RVA: 0x0024C99C File Offset: 0x0024AB9C
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

	// Token: 0x06005E47 RID: 24135 RVA: 0x0024CA00 File Offset: 0x0024AC00
	public DChunkSquareMeshPool(int initialCapacity, int NbLODLevel)
	{
		this._poolLock = new object();
		this.pool = new List<DChunkSquareMesh>[NbLODLevel];
		for (int i = 0; i < NbLODLevel; i++)
		{
			this.pool[i] = new List<DChunkSquareMesh>(initialCapacity);
		}
	}

	// Token: 0x06005E48 RID: 24136 RVA: 0x0024CA44 File Offset: 0x0024AC44
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

	// Token: 0x06005E49 RID: 24137 RVA: 0x0024CABC File Offset: 0x0024ACBC
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

	// Token: 0x04004923 RID: 18723
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DChunkSquareMesh>[] pool;

	// Token: 0x04004924 RID: 18724
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
