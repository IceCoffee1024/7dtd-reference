using System;
using System.Collections.Generic;

// Token: 0x02000C4E RID: 3150
public class UtilList<T> where T : class
{
	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x06005F74 RID: 24436 RVA: 0x0025EA2E File Offset: 0x0025CC2E
	public int Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x06005F75 RID: 24437 RVA: 0x0025EA36 File Offset: 0x0025CC36
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x06005F76 RID: 24438 RVA: 0x0025EA40 File Offset: 0x0025CC40
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

	// Token: 0x06005F77 RID: 24439 RVA: 0x0025EA90 File Offset: 0x0025CC90
	public void Clear()
	{
		this.StartId = 0;
		this.EndId = 0;
		this.count = 0;
		this.IsFull = false;
		this.IsStandardState = true;
	}

	// Token: 0x06005F78 RID: 24440 RVA: 0x0025EAB8 File Offset: 0x0025CCB8
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

	// Token: 0x06005F79 RID: 24441 RVA: 0x0025EB34 File Offset: 0x0025CD34
	public T Peek()
	{
		return this.InternArray[this.StartId];
	}

	// Token: 0x06005F7A RID: 24442 RVA: 0x0025EB48 File Offset: 0x0025CD48
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

	// Token: 0x06005F7B RID: 24443 RVA: 0x0025EBC8 File Offset: 0x0025CDC8
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

	// Token: 0x06005F7C RID: 24444 RVA: 0x0025EC44 File Offset: 0x0025CE44
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

	// Token: 0x170009D8 RID: 2520
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

	// Token: 0x04004B09 RID: 19209
	[PublicizedFrom(EAccessModifier.Private)]
	public int capacity;

	// Token: 0x04004B0A RID: 19210
	[PublicizedFrom(EAccessModifier.Private)]
	public int StartId;

	// Token: 0x04004B0B RID: 19211
	[PublicizedFrom(EAccessModifier.Private)]
	public int EndId;

	// Token: 0x04004B0C RID: 19212
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04004B0D RID: 19213
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsStandardState;

	// Token: 0x04004B0E RID: 19214
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsFull;

	// Token: 0x04004B0F RID: 19215
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] InternArray;

	// Token: 0x04004B10 RID: 19216
	[PublicizedFrom(EAccessModifier.Private)]
	public T NullValue;
}
