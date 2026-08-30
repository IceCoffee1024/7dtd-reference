using System;
using System.Collections.Generic;

// Token: 0x0200146A RID: 5226
[Serializable]
public class OptimizedList<T>
{
	// Token: 0x0600A3F0 RID: 41968 RVA: 0x003D23B0 File Offset: 0x003D05B0
	public OptimizedList()
		: this(10)
	{
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A3F1 RID: 41969 RVA: 0x003D23CF File Offset: 0x003D05CF
	public OptimizedList(int Capacity)
	{
		this.Count = 0;
		this.array = new T[Capacity];
		this.length = Capacity;
		this.DoubleSize = 0;
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A3F2 RID: 41970 RVA: 0x003D2410 File Offset: 0x003D0610
	public OptimizedList(OptimizedList<T> L)
	{
		if (L.Count == 0)
		{
			this.Count = 0;
			this.array = new T[2];
			this.length = 2;
			return;
		}
		this.Count = 0;
		this.array = new T[L.Count];
		this.length = L.Count;
		this.DoubleSize = 0;
		this.IsValueType = typeof(T).IsValueType;
		this.AddRange(L);
	}

	// Token: 0x0600A3F3 RID: 41971 RVA: 0x003D2490 File Offset: 0x003D0690
	public OptimizedList(T[] L)
	{
		if (L.Length == 0)
		{
			this.Count = 0;
			this.array = new T[2];
			this.length = 2;
			return;
		}
		this.Count = 0;
		this.array = new T[L.Length];
		this.length = L.Length;
		this.DoubleSize = 0;
		this.IsValueType = typeof(T).IsValueType;
		this.AddRange(L);
	}

	// Token: 0x0600A3F4 RID: 41972 RVA: 0x003D2503 File Offset: 0x003D0703
	public OptimizedList(int Capacity, int DoubleSize)
	{
		this.Count = 0;
		this.array = new T[Capacity];
		this.length = Capacity;
		this.DoubleSize = DoubleSize;
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A3F5 RID: 41973 RVA: 0x003D2544 File Offset: 0x003D0744
	public T Last()
	{
		if (this.Count > 0)
		{
			return this.array[this.Count - 1];
		}
		return default(T);
	}

	// Token: 0x0600A3F6 RID: 41974 RVA: 0x003D2578 File Offset: 0x003D0778
	public void Add(T value)
	{
		if (this.length == this.Count)
		{
			this.length = ((this.DoubleSize == 0) ? (this.Count * 2) : (this.Count + this.DoubleSize));
			T[] array = new T[this.length];
			Array.Copy(this.array, 0, array, 0, this.Count);
			this.array = array;
		}
		T[] array2 = this.array;
		int count = this.Count;
		this.Count = count + 1;
		array2[count] = value;
	}

	// Token: 0x0600A3F7 RID: 41975 RVA: 0x003D25FC File Offset: 0x003D07FC
	public void Add(ref T value)
	{
		if (this.length == this.Count)
		{
			this.length = ((this.DoubleSize == 0) ? (this.Count * 2) : (this.Count + this.DoubleSize));
			T[] array = new T[this.length];
			Array.Copy(this.array, 0, array, 0, this.Count);
			this.array = array;
		}
		T[] array2 = this.array;
		int count = this.Count;
		this.Count = count + 1;
		array2[count] = value;
	}

	// Token: 0x0600A3F8 RID: 41976 RVA: 0x003D2688 File Offset: 0x003D0888
	public void AddSafe(T value)
	{
		T[] array = this.array;
		int count = this.Count;
		this.Count = count + 1;
		array[count] = value;
	}

	// Token: 0x0600A3F9 RID: 41977 RVA: 0x003D26B4 File Offset: 0x003D08B4
	public void AddSafe(T valueA, T valueB, T valueC, T valueD)
	{
		if (this.length - (this.Count + 4) <= 0)
		{
			this.length = ((this.DoubleSize == 0) ? (this.Count * 2 + 4) : (this.Count + this.DoubleSize + 4));
			T[] array = new T[this.length];
			Array.Copy(this.array, 0, array, 0, this.Count);
			this.array = array;
		}
		T[] array2 = this.array;
		int num = this.Count;
		this.Count = num + 1;
		array2[num] = valueA;
		T[] array3 = this.array;
		num = this.Count;
		this.Count = num + 1;
		array3[num] = valueB;
		T[] array4 = this.array;
		num = this.Count;
		this.Count = num + 1;
		array4[num] = valueC;
		T[] array5 = this.array;
		num = this.Count;
		this.Count = num + 1;
		array5[num] = valueD;
	}

	// Token: 0x0600A3FA RID: 41978 RVA: 0x003D2798 File Offset: 0x003D0998
	public void AddRange(T[] values)
	{
		int num = values.Length;
		if (this.length - (this.Count + num) <= 0)
		{
			T[] array = new T[this.Count * 2 + num];
			Array.Copy(this.array, 0, array, 0, this.Count);
			this.array = array;
			this.length = this.Count * 2 + num;
		}
		Array.Copy(values, 0, this.array, this.Count, num);
		this.Count += num;
	}

	// Token: 0x0600A3FB RID: 41979 RVA: 0x003D2818 File Offset: 0x003D0A18
	public void AddRange(OptimizedList<T> values)
	{
		if (values == null || values.Count == 0)
		{
			return;
		}
		int num = values.Length;
		if (this.length - (this.Count + num) <= 0)
		{
			this.length = this.Count * 2 + num;
			T[] array = new T[this.length];
			Array.Copy(this.array, 0, array, 0, this.Count);
			this.array = array;
		}
		Array.Copy(values.array, 0, this.array, this.Count, num);
		this.Count += num;
	}

	// Token: 0x0600A3FC RID: 41980 RVA: 0x003D28A8 File Offset: 0x003D0AA8
	public bool Remove(T obj)
	{
		int num = Array.IndexOf<T>(this.array, obj, 0, this.Count);
		if (num >= 0)
		{
			this.Count--;
			if (num < this.Count)
			{
				Array.Copy(this.array, num + 1, this.array, num, this.Count - num);
			}
			this.array[this.Count] = default(T);
			return true;
		}
		return false;
	}

	// Token: 0x0600A3FD RID: 41981 RVA: 0x003D2920 File Offset: 0x003D0B20
	public void RemoveAt(int index)
	{
		if (index >= this.Count)
		{
			return;
		}
		this.Count--;
		if (index < this.Count)
		{
			Array.Copy(this.array, index + 1, this.array, index, this.Count - index);
		}
		this.array[this.Count] = default(T);
	}

	// Token: 0x0600A3FE RID: 41982 RVA: 0x003D2985 File Offset: 0x003D0B85
	public bool Contains(T obj)
	{
		return Array.IndexOf<T>(this.array, obj, 0, this.Count) >= 0;
	}

	// Token: 0x0600A3FF RID: 41983 RVA: 0x003D29A0 File Offset: 0x003D0BA0
	public void Set(T[] values)
	{
		this.array = values;
		this.length = values.Length;
		this.Count = this.length;
	}

	// Token: 0x0600A400 RID: 41984 RVA: 0x003D29BE File Offset: 0x003D0BBE
	public void Clear()
	{
		if (this.Count > 0)
		{
			Array.Clear(this.array, 0, this.array.Length);
		}
		this.Count = 0;
	}

	// Token: 0x0600A401 RID: 41985 RVA: 0x003D29E4 File Offset: 0x003D0BE4
	public T[] ToArray()
	{
		if (this.Count == 0)
		{
			return null;
		}
		T[] array = new T[this.Count];
		Array.Copy(this.array, array, this.Count);
		return array;
	}

	// Token: 0x0600A402 RID: 41986 RVA: 0x003D2A1C File Offset: 0x003D0C1C
	public void CheckArray(int Size)
	{
		if (this.length - (this.Count + Size) <= 0)
		{
			this.length = ((this.DoubleSize == 0) ? (this.Count * 2 + Size) : (this.Count + this.DoubleSize + Size));
			T[] array = new T[this.length];
			Array.Copy(this.array, 0, array, 0, this.Count);
			this.array = array;
		}
	}

	// Token: 0x0600A403 RID: 41987 RVA: 0x003D2A8B File Offset: 0x003D0C8B
	public void Sort(IComparer<T> comparer)
	{
		this.Sort(0, this.Count, comparer);
	}

	// Token: 0x0600A404 RID: 41988 RVA: 0x003D2A9B File Offset: 0x003D0C9B
	public void Sort(int index, int count, IComparer<T> comparer)
	{
		if (this.length - index < count)
		{
			return;
		}
		Array.Sort<T>(this.array, index, count, comparer);
	}

	// Token: 0x1700134E RID: 4942
	// (get) Token: 0x0600A405 RID: 41989 RVA: 0x003D2AB7 File Offset: 0x003D0CB7
	public int Length
	{
		get
		{
			return this.Count;
		}
	}

	// Token: 0x04007AAD RID: 31405
	public T[] array;

	// Token: 0x04007AAE RID: 31406
	public int Count;

	// Token: 0x04007AAF RID: 31407
	public int length;

	// Token: 0x04007AB0 RID: 31408
	public int DoubleSize;

	// Token: 0x04007AB1 RID: 31409
	public bool IsValueType;
}
