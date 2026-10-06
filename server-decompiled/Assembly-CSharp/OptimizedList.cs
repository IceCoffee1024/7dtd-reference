using System;
using System.Collections.Generic;

// Token: 0x0200148A RID: 5258
[Serializable]
public class OptimizedList<T>
{
	// Token: 0x0600A49C RID: 42140 RVA: 0x003D6FC0 File Offset: 0x003D51C0
	public OptimizedList()
		: this(10)
	{
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A49D RID: 42141 RVA: 0x003D6FDF File Offset: 0x003D51DF
	public OptimizedList(int Capacity)
	{
		this.Count = 0;
		this.array = new T[Capacity];
		this.length = Capacity;
		this.DoubleSize = 0;
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A49E RID: 42142 RVA: 0x003D7020 File Offset: 0x003D5220
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

	// Token: 0x0600A49F RID: 42143 RVA: 0x003D70A0 File Offset: 0x003D52A0
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

	// Token: 0x0600A4A0 RID: 42144 RVA: 0x003D7113 File Offset: 0x003D5313
	public OptimizedList(int Capacity, int DoubleSize)
	{
		this.Count = 0;
		this.array = new T[Capacity];
		this.length = Capacity;
		this.DoubleSize = DoubleSize;
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A4A1 RID: 42145 RVA: 0x003D7154 File Offset: 0x003D5354
	public T Last()
	{
		if (this.Count > 0)
		{
			return this.array[this.Count - 1];
		}
		return default(T);
	}

	// Token: 0x0600A4A2 RID: 42146 RVA: 0x003D7188 File Offset: 0x003D5388
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

	// Token: 0x0600A4A3 RID: 42147 RVA: 0x003D720C File Offset: 0x003D540C
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

	// Token: 0x0600A4A4 RID: 42148 RVA: 0x003D7298 File Offset: 0x003D5498
	public void AddSafe(T value)
	{
		T[] array = this.array;
		int count = this.Count;
		this.Count = count + 1;
		array[count] = value;
	}

	// Token: 0x0600A4A5 RID: 42149 RVA: 0x003D72C4 File Offset: 0x003D54C4
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

	// Token: 0x0600A4A6 RID: 42150 RVA: 0x003D73A8 File Offset: 0x003D55A8
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

	// Token: 0x0600A4A7 RID: 42151 RVA: 0x003D7428 File Offset: 0x003D5628
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

	// Token: 0x0600A4A8 RID: 42152 RVA: 0x003D74B8 File Offset: 0x003D56B8
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

	// Token: 0x0600A4A9 RID: 42153 RVA: 0x003D7530 File Offset: 0x003D5730
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

	// Token: 0x0600A4AA RID: 42154 RVA: 0x003D7595 File Offset: 0x003D5795
	public bool Contains(T obj)
	{
		return Array.IndexOf<T>(this.array, obj, 0, this.Count) >= 0;
	}

	// Token: 0x0600A4AB RID: 42155 RVA: 0x003D75B0 File Offset: 0x003D57B0
	public void Set(T[] values)
	{
		this.array = values;
		this.length = values.Length;
		this.Count = this.length;
	}

	// Token: 0x0600A4AC RID: 42156 RVA: 0x003D75CE File Offset: 0x003D57CE
	public void Clear()
	{
		if (this.Count > 0)
		{
			Array.Clear(this.array, 0, this.array.Length);
		}
		this.Count = 0;
	}

	// Token: 0x0600A4AD RID: 42157 RVA: 0x003D75F4 File Offset: 0x003D57F4
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

	// Token: 0x0600A4AE RID: 42158 RVA: 0x003D762C File Offset: 0x003D582C
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

	// Token: 0x0600A4AF RID: 42159 RVA: 0x003D769B File Offset: 0x003D589B
	public void Sort(IComparer<T> comparer)
	{
		this.Sort(0, this.Count, comparer);
	}

	// Token: 0x0600A4B0 RID: 42160 RVA: 0x003D76AB File Offset: 0x003D58AB
	public void Sort(int index, int count, IComparer<T> comparer)
	{
		if (this.length - index < count)
		{
			return;
		}
		Array.Sort<T>(this.array, index, count, comparer);
	}

	// Token: 0x17001393 RID: 5011
	// (get) Token: 0x0600A4B1 RID: 42161 RVA: 0x003D76C7 File Offset: 0x003D58C7
	public int Length
	{
		get
		{
			return this.Count;
		}
	}

	// Token: 0x04007B28 RID: 31528
	public T[] array;

	// Token: 0x04007B29 RID: 31529
	public int Count;

	// Token: 0x04007B2A RID: 31530
	public int length;

	// Token: 0x04007B2B RID: 31531
	public int DoubleSize;

	// Token: 0x04007B2C RID: 31532
	public bool IsValueType;
}
