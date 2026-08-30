using System;
using System.Collections.Generic;

// Token: 0x02000C2A RID: 3114
public class UtilList<T> where T : class
{
	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06005ED1 RID: 24273 RVA: 0x00258882 File Offset: 0x00256A82
	public int Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x06005ED2 RID: 24274 RVA: 0x0025888A File Offset: 0x00256A8A
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x06005ED3 RID: 24275 RVA: 0x00258894 File Offset: 0x00256A94
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

	// Token: 0x06005ED4 RID: 24276 RVA: 0x002588E4 File Offset: 0x00256AE4
	public void Clear()
	{
		this.StartId = 0;
		this.EndId = 0;
		this.count = 0;
		this.IsFull = false;
		this.IsStandardState = true;
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x0025890C File Offset: 0x00256B0C
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

	// Token: 0x06005ED6 RID: 24278 RVA: 0x00258988 File Offset: 0x00256B88
	public T Peek()
	{
		return this.InternArray[this.StartId];
	}

	// Token: 0x06005ED7 RID: 24279 RVA: 0x0025899C File Offset: 0x00256B9C
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

	// Token: 0x06005ED8 RID: 24280 RVA: 0x00258A1C File Offset: 0x00256C1C
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

	// Token: 0x06005ED9 RID: 24281 RVA: 0x00258A98 File Offset: 0x00256C98
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

	// Token: 0x170009A3 RID: 2467
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

	// Token: 0x04004A2D RID: 18989
	[PublicizedFrom(EAccessModifier.Private)]
	public int capacity;

	// Token: 0x04004A2E RID: 18990
	[PublicizedFrom(EAccessModifier.Private)]
	public int StartId;

	// Token: 0x04004A2F RID: 18991
	[PublicizedFrom(EAccessModifier.Private)]
	public int EndId;

	// Token: 0x04004A30 RID: 18992
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04004A31 RID: 18993
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsStandardState;

	// Token: 0x04004A32 RID: 18994
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsFull;

	// Token: 0x04004A33 RID: 18995
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] InternArray;

	// Token: 0x04004A34 RID: 18996
	[PublicizedFrom(EAccessModifier.Private)]
	public T NullValue;
}
