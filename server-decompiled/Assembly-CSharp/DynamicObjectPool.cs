using System;
using System.Collections.Generic;

// Token: 0x02001464 RID: 5220
public class DynamicObjectPool<T> where T : new()
{
	// Token: 0x0600A3A6 RID: 41894 RVA: 0x003D3898 File Offset: 0x003D1A98
	public DynamicObjectPool(int objectsPerBlock)
	{
		this.Init(objectsPerBlock, int.MaxValue);
	}

	// Token: 0x0600A3A7 RID: 41895 RVA: 0x003D38AC File Offset: 0x003D1AAC
	public DynamicObjectPool(int objectsPerBlock, int maxObjects)
	{
		this.Init(objectsPerBlock, maxObjects);
	}

	// Token: 0x0600A3A8 RID: 41896 RVA: 0x003D38BC File Offset: 0x003D1ABC
	public void AllocateBlock(int numObjects)
	{
		for (int i = 0; i < numObjects; i++)
		{
			this.push(new T());
		}
		this.m_numAllocatedObjects += numObjects;
	}

	// Token: 0x0600A3A9 RID: 41897 RVA: 0x003D38EE File Offset: 0x003D1AEE
	public T Allocate()
	{
		if (this.m_numFreeObjects < 1)
		{
			this.AllocateBlock(this.m_numObjectsPerBlock);
		}
		this.m_numUsedObjects++;
		return this.pop();
	}

	// Token: 0x0600A3AA RID: 41898 RVA: 0x003D391C File Offset: 0x003D1B1C
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

	// Token: 0x0600A3AB RID: 41899 RVA: 0x003D3967 File Offset: 0x003D1B67
	public void Free(T obj)
	{
		this.push(obj);
		this.m_numUsedObjects--;
	}

	// Token: 0x0600A3AC RID: 41900 RVA: 0x003D3980 File Offset: 0x003D1B80
	public void Free(T[] array)
	{
		foreach (T t in array)
		{
			this.Free(t);
		}
	}

	// Token: 0x0600A3AD RID: 41901 RVA: 0x003D39AC File Offset: 0x003D1BAC
	public void Compact()
	{
		this.m_numFreeObjects = 0;
		this.m_numAllocatedObjects -= this.m_freeObjects.Count;
		this.m_freeObjects = new List<T>(this.m_numObjectsPerBlock);
	}

	// Token: 0x1700136D RID: 4973
	// (get) Token: 0x0600A3AE RID: 41902 RVA: 0x003D39DE File Offset: 0x003D1BDE
	public int NumAllocatedObjects
	{
		get
		{
			return this.m_numAllocatedObjects;
		}
	}

	// Token: 0x1700136E RID: 4974
	// (get) Token: 0x0600A3AF RID: 41903 RVA: 0x003D39E6 File Offset: 0x003D1BE6
	public int NumUsedObjects
	{
		get
		{
			return this.m_numUsedObjects;
		}
	}

	// Token: 0x1700136F RID: 4975
	// (get) Token: 0x0600A3B0 RID: 41904 RVA: 0x003D39EE File Offset: 0x003D1BEE
	public int NumFreeObjects
	{
		get
		{
			return this.m_numFreeObjects;
		}
	}

	// Token: 0x17001370 RID: 4976
	// (get) Token: 0x0600A3B1 RID: 41905 RVA: 0x003D39F6 File Offset: 0x003D1BF6
	public int MaxObjects
	{
		get
		{
			return this.m_maxObjects;
		}
	}

	// Token: 0x0600A3B2 RID: 41906 RVA: 0x003D39FE File Offset: 0x003D1BFE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init(int objectsPerBlock, int maxObjects)
	{
		this.m_numObjectsPerBlock = objectsPerBlock;
		this.m_maxObjects = maxObjects;
		this.m_freeObjects = new List<T>(objectsPerBlock);
	}

	// Token: 0x0600A3B3 RID: 41907 RVA: 0x003D3A1C File Offset: 0x003D1C1C
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

	// Token: 0x0600A3B4 RID: 41908 RVA: 0x003D3A6C File Offset: 0x003D1C6C
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

	// Token: 0x04007AA6 RID: 31398
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numAllocatedObjects;

	// Token: 0x04007AA7 RID: 31399
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numUsedObjects;

	// Token: 0x04007AA8 RID: 31400
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numFreeObjects;

	// Token: 0x04007AA9 RID: 31401
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numObjectsPerBlock;

	// Token: 0x04007AAA RID: 31402
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_maxObjects;

	// Token: 0x04007AAB RID: 31403
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> m_freeObjects;
}
