using System;
using System.Collections.Generic;

// Token: 0x02001444 RID: 5188
public class DynamicObjectPool<T> where T : new()
{
	// Token: 0x0600A2FA RID: 41722 RVA: 0x003CEC88 File Offset: 0x003CCE88
	public DynamicObjectPool(int objectsPerBlock)
	{
		this.Init(objectsPerBlock, int.MaxValue);
	}

	// Token: 0x0600A2FB RID: 41723 RVA: 0x003CEC9C File Offset: 0x003CCE9C
	public DynamicObjectPool(int objectsPerBlock, int maxObjects)
	{
		this.Init(objectsPerBlock, maxObjects);
	}

	// Token: 0x0600A2FC RID: 41724 RVA: 0x003CECAC File Offset: 0x003CCEAC
	public void AllocateBlock(int numObjects)
	{
		for (int i = 0; i < numObjects; i++)
		{
			this.push(new T());
		}
		this.m_numAllocatedObjects += numObjects;
	}

	// Token: 0x0600A2FD RID: 41725 RVA: 0x003CECDE File Offset: 0x003CCEDE
	public T Allocate()
	{
		if (this.m_numFreeObjects < 1)
		{
			this.AllocateBlock(this.m_numObjectsPerBlock);
		}
		this.m_numUsedObjects++;
		return this.pop();
	}

	// Token: 0x0600A2FE RID: 41726 RVA: 0x003CED0C File Offset: 0x003CCF0C
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

	// Token: 0x0600A2FF RID: 41727 RVA: 0x003CED57 File Offset: 0x003CCF57
	public void Free(T obj)
	{
		this.push(obj);
		this.m_numUsedObjects--;
	}

	// Token: 0x0600A300 RID: 41728 RVA: 0x003CED70 File Offset: 0x003CCF70
	public void Free(T[] array)
	{
		foreach (T t in array)
		{
			this.Free(t);
		}
	}

	// Token: 0x0600A301 RID: 41729 RVA: 0x003CED9C File Offset: 0x003CCF9C
	public void Compact()
	{
		this.m_numFreeObjects = 0;
		this.m_numAllocatedObjects -= this.m_freeObjects.Count;
		this.m_freeObjects = new List<T>(this.m_numObjectsPerBlock);
	}

	// Token: 0x17001328 RID: 4904
	// (get) Token: 0x0600A302 RID: 41730 RVA: 0x003CEDCE File Offset: 0x003CCFCE
	public int NumAllocatedObjects
	{
		get
		{
			return this.m_numAllocatedObjects;
		}
	}

	// Token: 0x17001329 RID: 4905
	// (get) Token: 0x0600A303 RID: 41731 RVA: 0x003CEDD6 File Offset: 0x003CCFD6
	public int NumUsedObjects
	{
		get
		{
			return this.m_numUsedObjects;
		}
	}

	// Token: 0x1700132A RID: 4906
	// (get) Token: 0x0600A304 RID: 41732 RVA: 0x003CEDDE File Offset: 0x003CCFDE
	public int NumFreeObjects
	{
		get
		{
			return this.m_numFreeObjects;
		}
	}

	// Token: 0x1700132B RID: 4907
	// (get) Token: 0x0600A305 RID: 41733 RVA: 0x003CEDE6 File Offset: 0x003CCFE6
	public int MaxObjects
	{
		get
		{
			return this.m_maxObjects;
		}
	}

	// Token: 0x0600A306 RID: 41734 RVA: 0x003CEDEE File Offset: 0x003CCFEE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init(int objectsPerBlock, int maxObjects)
	{
		this.m_numObjectsPerBlock = objectsPerBlock;
		this.m_maxObjects = maxObjects;
		this.m_freeObjects = new List<T>(objectsPerBlock);
	}

	// Token: 0x0600A307 RID: 41735 RVA: 0x003CEE0C File Offset: 0x003CD00C
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

	// Token: 0x0600A308 RID: 41736 RVA: 0x003CEE5C File Offset: 0x003CD05C
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

	// Token: 0x04007A2B RID: 31275
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numAllocatedObjects;

	// Token: 0x04007A2C RID: 31276
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numUsedObjects;

	// Token: 0x04007A2D RID: 31277
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numFreeObjects;

	// Token: 0x04007A2E RID: 31278
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numObjectsPerBlock;

	// Token: 0x04007A2F RID: 31279
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_maxObjects;

	// Token: 0x04007A30 RID: 31280
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> m_freeObjects;
}
