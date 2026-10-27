using System;
using System.Collections.Generic;

// Token: 0x02000C43 RID: 3139
public class DChunkSquareMeshPool
{
	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x06005F0E RID: 24334 RVA: 0x00254070 File Offset: 0x00252270
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

	// Token: 0x06005F0F RID: 24335 RVA: 0x002540D4 File Offset: 0x002522D4
	public DChunkSquareMeshPool(int initialCapacity, int NbLODLevel)
	{
		this._poolLock = new object();
		this.pool = new List<DChunkSquareMesh>[NbLODLevel];
		for (int i = 0; i < NbLODLevel; i++)
		{
			this.pool[i] = new List<DChunkSquareMesh>(initialCapacity);
		}
	}

	// Token: 0x06005F10 RID: 24336 RVA: 0x00254118 File Offset: 0x00252318
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

	// Token: 0x06005F11 RID: 24337 RVA: 0x00254190 File Offset: 0x00252390
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

	// Token: 0x04004A24 RID: 18980
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DChunkSquareMesh>[] pool;

	// Token: 0x04004A25 RID: 18981
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
