using System;
using System.Collections.Generic;

// Token: 0x02001429 RID: 5161
public class DynamicObjectPool<T> where T : new()
{
	// Token: 0x0600A1FD RID: 41469 RVA: 0x003C9944 File Offset: 0x003C7B44
	public DynamicObjectPool(int objectsPerBlock)
	{
		this.Init(objectsPerBlock, int.MaxValue);
	}

	// Token: 0x0600A1FE RID: 41470 RVA: 0x003C9958 File Offset: 0x003C7B58
	public DynamicObjectPool(int objectsPerBlock, int maxObjects)
	{
		this.Init(objectsPerBlock, maxObjects);
	}

	// Token: 0x0600A1FF RID: 41471 RVA: 0x003C9968 File Offset: 0x003C7B68
	public void AllocateBlock(int numObjects)
	{
		for (int i = 0; i < numObjects; i++)
		{
			this.push(new T());
		}
		this.m_numAllocatedObjects += numObjects;
	}

	// Token: 0x0600A200 RID: 41472 RVA: 0x003C999A File Offset: 0x003C7B9A
	public T Allocate()
	{
		if (this.m_numFreeObjects < 1)
		{
			this.AllocateBlock(this.m_numObjectsPerBlock);
		}
		this.m_numUsedObjects++;
		return this.pop();
	}

	// Token: 0x0600A201 RID: 41473 RVA: 0x003C99C8 File Offset: 0x003C7BC8
	public T[] Allocate(int numToAllocate)
	{
		if (numToAllocate < 1)
		{
			return null;
		}
		T[] array = new T[numToAllocate];
		while (this.m_numFreeObjects < numToAllocate)
		{
			this.AllocateBlock(this.m_numObjectsPerBlock);
		}
		for (int i = 0; i < numToAllocate; i++)
		{
			array[i] = this.Allocate();
		}
		return array;
	}

	// Token: 0x0600A202 RID: 41474 RVA: 0x003C9A13 File Offset: 0x003C7C13
	public void Free(T obj)
	{
		this.push(obj);
		this.m_numUsedObjects--;
	}

	// Token: 0x0600A203 RID: 41475 RVA: 0x003C9A2C File Offset: 0x003C7C2C
	public void Free(T[] array)
	{
		foreach (T t in array)
		{
			this.Free(t);
		}
	}

	// Token: 0x0600A204 RID: 41476 RVA: 0x003C9A58 File Offset: 0x003C7C58
	public void Compact()
	{
		this.m_numFreeObjects = 0;
		this.m_numAllocatedObjects -= this.m_freeObjects.Count;
		this.m_freeObjects = new List<T>(this.m_numObjectsPerBlock);
	}

	// Token: 0x170012EF RID: 4847
	// (get) Token: 0x0600A205 RID: 41477 RVA: 0x003C9A8A File Offset: 0x003C7C8A
	public int NumAllocatedObjects
	{
		get
		{
			return this.m_numAllocatedObjects;
		}
	}

	// Token: 0x170012F0 RID: 4848
	// (get) Token: 0x0600A206 RID: 41478 RVA: 0x003C9A92 File Offset: 0x003C7C92
	public int NumUsedObjects
	{
		get
		{
			return this.m_numUsedObjects;
		}
	}

	// Token: 0x170012F1 RID: 4849
	// (get) Token: 0x0600A207 RID: 41479 RVA: 0x003C9A9A File Offset: 0x003C7C9A
	public int NumFreeObjects
	{
		get
		{
			return this.m_numFreeObjects;
		}
	}

	// Token: 0x170012F2 RID: 4850
	// (get) Token: 0x0600A208 RID: 41480 RVA: 0x003C9AA2 File Offset: 0x003C7CA2
	public int MaxObjects
	{
		get
		{
			return this.m_maxObjects;
		}
	}

	// Token: 0x0600A209 RID: 41481 RVA: 0x003C9AAA File Offset: 0x003C7CAA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init(int objectsPerBlock, int maxObjects)
	{
		this.m_numObjectsPerBlock = objectsPerBlock;
		this.m_maxObjects = maxObjects;
		this.m_freeObjects = new List<T>(objectsPerBlock);
	}

	// Token: 0x0600A20A RID: 41482 RVA: 0x003C9AC8 File Offset: 0x003C7CC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void push(T t)
	{
		if (this.m_numFreeObjects >= this.m_freeObjects.Count)
		{
			this.m_freeObjects.Add(t);
		}
		else
		{
			this.m_freeObjects[this.m_numFreeObjects] = t;
		}
		this.m_numFreeObjects++;
	}

	// Token: 0x0600A20B RID: 41483 RVA: 0x003C9B18 File Offset: 0x003C7D18
	[PublicizedFrom(EAccessModifier.Private)]
	public T pop()
	{
		if (this.m_numFreeObjects < 1)
		{
			return default(T);
		}
		List<T> freeObjects = this.m_freeObjects;
		int num = this.m_numFreeObjects - 1;
		this.m_numFreeObjects = num;
		return freeObjects[num];
	}

	// Token: 0x04007952 RID: 31058
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numAllocatedObjects;

	// Token: 0x04007953 RID: 31059
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numUsedObjects;

	// Token: 0x04007954 RID: 31060
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numFreeObjects;

	// Token: 0x04007955 RID: 31061
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numObjectsPerBlock;

	// Token: 0x04007956 RID: 31062
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_maxObjects;

	// Token: 0x04007957 RID: 31063
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> m_freeObjects;
}
