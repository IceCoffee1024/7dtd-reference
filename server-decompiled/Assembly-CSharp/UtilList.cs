using System;
using System.Collections.Generic;

// Token: 0x02000C59 RID: 3161
public class UtilList<T> where T : class
{
	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x06005F99 RID: 24473 RVA: 0x0025FF56 File Offset: 0x0025E156
	public int Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x06005F9A RID: 24474 RVA: 0x0025FF5E File Offset: 0x0025E15E
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x0025FF68 File Offset: 0x0025E168
	public UtilList(int _capacity, T _NullValue)
	{
		this.capacity = _capacity;
		this.StartId = 0;
		this.EndId = 0;
		this.count = 0;
		this.IsStandardState = true;
		this.IsFull = false;
		this.InternArray = new T[_capacity];
		this.NullValue = _NullValue;
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x0025FFB8 File Offset: 0x0025E1B8
	public void Clear()
	{
		this.StartId = 0;
		this.EndId = 0;
		this.count = 0;
		this.IsFull = false;
		this.IsStandardState = true;
	}

	// Token: 0x06005F9D RID: 24477 RVA: 0x0025FFE0 File Offset: 0x0025E1E0
	public void Add(T NewItem)
	{
		if (this.IsFull)
		{
			throw new ArgumentException("Overflow : UtilList is full");
		}
		this.InternArray[this.EndId] = NewItem;
		int num = this.EndId + 1;
		this.EndId = num;
		if (num == this.capacity)
		{
			this.EndId = 0;
			this.IsStandardState = false;
		}
		if (this.EndId == this.StartId)
		{
			this.IsFull = true;
		}
		this.count++;
	}

	// Token: 0x06005F9E RID: 24478 RVA: 0x0026005C File Offset: 0x0025E25C
	public T Peek()
	{
		return this.InternArray[this.StartId];
	}

	// Token: 0x06005F9F RID: 24479 RVA: 0x00260070 File Offset: 0x0025E270
	public T Dequeue()
	{
		if (this.StartId == this.EndId && !this.IsFull)
		{
			throw new ArgumentException("UtilList is EMPTY");
		}
		int startId = this.StartId;
		this.StartId++;
		if (this.StartId == this.capacity)
		{
			this.StartId = 0;
			this.IsStandardState = true;
		}
		this.IsFull = false;
		this.count--;
		return this.InternArray[startId];
	}

	// Token: 0x06005FA0 RID: 24480 RVA: 0x002600F0 File Offset: 0x0025E2F0
	public void RemoveNFirst(int N)
	{
		if (this.count < N)
		{
			throw new ArgumentException("UtilList Out of range");
		}
		this.StartId += N;
		if (this.StartId >= this.capacity)
		{
			this.StartId -= this.capacity;
			this.IsStandardState = true;
		}
		this.count -= N;
		this.IsFull = this.count == this.capacity;
	}

	// Token: 0x06005FA1 RID: 24481 RVA: 0x0026016C File Offset: 0x0025E36C
	public void RemoveAt(List<int> IdList)
	{
		int num = IdList.Count;
		for (int i = 0; i < num; i++)
		{
			this[IdList[i]] = this.NullValue;
		}
		num = 0;
		for (int j = 0; j < this.Count; j++)
		{
			if (this[j] != null)
			{
				this[num++] = this[j];
			}
		}
		this.count = num;
		this.EndId = this.StartId + this.count;
		if (this.EndId >= this.capacity)
		{
			this.EndId -= this.capacity;
			this.IsStandardState = false;
		}
		else
		{
			this.IsStandardState = true;
		}
		this.IsFull = this.count == this.capacity;
	}

	// Token: 0x170009DE RID: 2526
	public T this[int Id]
	{
		get
		{
			if (this.Count == 0)
			{
				throw new ArgumentException("UtilList is EMPTY");
			}
			int num = this.StartId + Id;
			if (this.IsStandardState)
			{
				if (num >= this.EndId)
				{
					throw new ArgumentException("UtilList index is out of range");
				}
			}
			else if (num >= this.capacity)
			{
				num -= this.capacity;
				if (num >= this.EndId)
				{
					throw new ArgumentException("UtilList Index is out of range");
				}
			}
			return this.InternArray[num];
		}
		set
		{
			if (this.Count == 0)
			{
				throw new ArgumentException("UtilList is EMPTY");
			}
			int num = this.StartId + Id;
			if (this.IsStandardState)
			{
				if (num >= this.EndId)
				{
					throw new ArgumentException("UtilList : Index is out of range");
				}
			}
			else if (num >= this.capacity)
			{
				num -= this.capacity;
				if (num >= this.EndId)
				{
					throw new ArgumentException("UtilList : Index is out of range");
				}
			}
			this.InternArray[num] = value;
		}
	}

	// Token: 0x04004B2E RID: 19246
	[PublicizedFrom(EAccessModifier.Private)]
	public int capacity;

	// Token: 0x04004B2F RID: 19247
	[PublicizedFrom(EAccessModifier.Private)]
	public int StartId;

	// Token: 0x04004B30 RID: 19248
	[PublicizedFrom(EAccessModifier.Private)]
	public int EndId;

	// Token: 0x04004B31 RID: 19249
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04004B32 RID: 19250
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsStandardState;

	// Token: 0x04004B33 RID: 19251
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsFull;

	// Token: 0x04004B34 RID: 19252
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] InternArray;

	// Token: 0x04004B35 RID: 19253
	[PublicizedFrom(EAccessModifier.Private)]
	public T NullValue;
}
