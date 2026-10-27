using System;
using System.Collections.Generic;

// Token: 0x02001476 RID: 5238
public class DynamicObjectPool<T> where T : new()
{
	// Token: 0x0600A3FE RID: 41982 RVA: 0x003D5C9C File Offset: 0x003D3E9C
	public DynamicObjectPool(int objectsPerBlock)
	{
		this.Init(objectsPerBlock, int.MaxValue);
	}

	// Token: 0x0600A3FF RID: 41983 RVA: 0x003D5CB0 File Offset: 0x003D3EB0
	public DynamicObjectPool(int objectsPerBlock, int maxObjects)
	{
		this.Init(objectsPerBlock, maxObjects);
	}

	// Token: 0x0600A400 RID: 41984 RVA: 0x003D5CC0 File Offset: 0x003D3EC0
	public void AllocateBlock(int numObjects)
	{
		for (int i = 0; i < numObjects; i++)
		{
			this.push(new T());
		}
		this.m_numAllocatedObjects += numObjects;
	}

	// Token: 0x0600A401 RID: 41985 RVA: 0x003D5CF2 File Offset: 0x003D3EF2
	public T Allocate()
	{
		if (this.m_numFreeObjects < 1)
		{
			this.AllocateBlock(this.m_numObjectsPerBlock);
		}
		this.m_numUsedObjects++;
		return this.pop();
	}

	// Token: 0x0600A402 RID: 41986 RVA: 0x003D5D20 File Offset: 0x003D3F20
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

	// Token: 0x0600A403 RID: 41987 RVA: 0x003D5D6B File Offset: 0x003D3F6B
	public void Free(T obj)
	{
		this.push(obj);
		this.m_numUsedObjects--;
	}

	// Token: 0x0600A404 RID: 41988 RVA: 0x003D5D84 File Offset: 0x003D3F84
	public void Free(T[] array)
	{
		foreach (T t in array)
		{
			this.Free(t);
		}
	}

	// Token: 0x0600A405 RID: 41989 RVA: 0x003D5DB0 File Offset: 0x003D3FB0
	public void Compact()
	{
		this.m_numFreeObjects = 0;
		this.m_numAllocatedObjects -= this.m_freeObjects.Count;
		this.m_freeObjects = new List<T>(this.m_numObjectsPerBlock);
	}

	// Token: 0x17001379 RID: 4985
	// (get) Token: 0x0600A406 RID: 41990 RVA: 0x003D5DE2 File Offset: 0x003D3FE2
	public int NumAllocatedObjects
	{
		get
		{
			return this.m_numAllocatedObjects;
		}
	}

	// Token: 0x1700137A RID: 4986
	// (get) Token: 0x0600A407 RID: 41991 RVA: 0x003D5DEA File Offset: 0x003D3FEA
	public int NumUsedObjects
	{
		get
		{
			return this.m_numUsedObjects;
		}
	}

	// Token: 0x1700137B RID: 4987
	// (get) Token: 0x0600A408 RID: 41992 RVA: 0x003D5DF2 File Offset: 0x003D3FF2
	public int NumFreeObjects
	{
		get
		{
			return this.m_numFreeObjects;
		}
	}

	// Token: 0x1700137C RID: 4988
	// (get) Token: 0x0600A409 RID: 41993 RVA: 0x003D5DFA File Offset: 0x003D3FFA
	public int MaxObjects
	{
		get
		{
			return this.m_maxObjects;
		}
	}

	// Token: 0x0600A40A RID: 41994 RVA: 0x003D5E02 File Offset: 0x003D4002
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init(int objectsPerBlock, int maxObjects)
	{
		this.m_numObjectsPerBlock = objectsPerBlock;
		this.m_maxObjects = maxObjects;
		this.m_freeObjects = new List<T>(objectsPerBlock);
	}

	// Token: 0x0600A40B RID: 41995 RVA: 0x003D5E20 File Offset: 0x003D4020
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

	// Token: 0x0600A40C RID: 41996 RVA: 0x003D5E70 File Offset: 0x003D4070
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

	// Token: 0x04007AE7 RID: 31463
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numAllocatedObjects;

	// Token: 0x04007AE8 RID: 31464
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numUsedObjects;

	// Token: 0x04007AE9 RID: 31465
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numFreeObjects;

	// Token: 0x04007AEA RID: 31466
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numObjectsPerBlock;

	// Token: 0x04007AEB RID: 31467
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_maxObjects;

	// Token: 0x04007AEC RID: 31468
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> m_freeObjects;
}
