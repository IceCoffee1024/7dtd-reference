using System;
using System.Collections.Generic;

// Token: 0x02000C26 RID: 3110
public class DChunkSquareMeshPool
{
	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x06005ED0 RID: 24272 RVA: 0x0024F84C File Offset: 0x0024DA4C
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

	// Token: 0x06005ED1 RID: 24273 RVA: 0x0024F8B0 File Offset: 0x0024DAB0
	public DChunkSquareMeshPool(int initialCapacity, int NbLODLevel)
	{
		this._poolLock = new object();
		this.pool = new List<DChunkSquareMesh>[NbLODLevel];
		for (int i = 0; i < NbLODLevel; i++)
		{
			this.pool[i] = new List<DChunkSquareMesh>(initialCapacity);
		}
	}

	// Token: 0x06005ED2 RID: 24274 RVA: 0x0024F8F4 File Offset: 0x0024DAF4
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

	// Token: 0x06005ED3 RID: 24275 RVA: 0x0024F96C File Offset: 0x0024DB6C
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

	// Token: 0x040049C0 RID: 18880
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DChunkSquareMesh>[] pool;

	// Token: 0x040049C1 RID: 18881
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
