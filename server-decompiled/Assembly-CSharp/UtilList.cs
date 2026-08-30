using System;
using System.Collections.Generic;

// Token: 0x02000C3C RID: 3132
public class UtilList<T> where T : class
{
	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x06005F5B RID: 24411 RVA: 0x0025B732 File Offset: 0x00259932
	public int Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06005F5C RID: 24412 RVA: 0x0025B73A File Offset: 0x0025993A
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x06005F5D RID: 24413 RVA: 0x0025B744 File Offset: 0x00259944
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

	// Token: 0x06005F5E RID: 24414 RVA: 0x0025B794 File Offset: 0x00259994
	public void Clear()
	{
		this.StartId = 0;
		this.EndId = 0;
		this.count = 0;
		this.IsFull = false;
		this.IsStandardState = true;
	}

	// Token: 0x06005F5F RID: 24415 RVA: 0x0025B7BC File Offset: 0x002599BC
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

	// Token: 0x06005F60 RID: 24416 RVA: 0x0025B838 File Offset: 0x00259A38
	public T Peek()
	{
		return this.InternArray[this.StartId];
	}

	// Token: 0x06005F61 RID: 24417 RVA: 0x0025B84C File Offset: 0x00259A4C
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

	// Token: 0x06005F62 RID: 24418 RVA: 0x0025B8CC File Offset: 0x00259ACC
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

	// Token: 0x06005F63 RID: 24419 RVA: 0x0025B948 File Offset: 0x00259B48
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

	// Token: 0x170009B6 RID: 2486
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

	// Token: 0x04004ACA RID: 19146
	[PublicizedFrom(EAccessModifier.Private)]
	public int capacity;

	// Token: 0x04004ACB RID: 19147
	[PublicizedFrom(EAccessModifier.Private)]
	public int StartId;

	// Token: 0x04004ACC RID: 19148
	[PublicizedFrom(EAccessModifier.Private)]
	public int EndId;

	// Token: 0x04004ACD RID: 19149
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04004ACE RID: 19150
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsStandardState;

	// Token: 0x04004ACF RID: 19151
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsFull;

	// Token: 0x04004AD0 RID: 19152
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] InternArray;

	// Token: 0x04004AD1 RID: 19153
	[PublicizedFrom(EAccessModifier.Private)]
	public T NullValue;
}
