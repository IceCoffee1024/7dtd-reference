using System;
using System.Collections.Generic;

// Token: 0x0200144F RID: 5199
[Serializable]
public class OptimizedList<T>
{
	// Token: 0x0600A2F3 RID: 41715 RVA: 0x003CD06C File Offset: 0x003CB26C
	public OptimizedList()
		: this(10)
	{
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A2F4 RID: 41716 RVA: 0x003CD08B File Offset: 0x003CB28B
	public OptimizedList(int Capacity)
	{
		this.Count = 0;
		this.array = new T[Capacity];
		this.length = Capacity;
		this.DoubleSize = 0;
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A2F5 RID: 41717 RVA: 0x003CD0CC File Offset: 0x003CB2CC
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

	// Token: 0x0600A2F6 RID: 41718 RVA: 0x003CD14C File Offset: 0x003CB34C
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

	// Token: 0x0600A2F7 RID: 41719 RVA: 0x003CD1BF File Offset: 0x003CB3BF
	public OptimizedList(int Capacity, int DoubleSize)
	{
		this.Count = 0;
		this.array = new T[Capacity];
		this.length = Capacity;
		this.DoubleSize = DoubleSize;
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A2F8 RID: 41720 RVA: 0x003CD200 File Offset: 0x003CB400
	public T Last()
	{
		if (this.Count > 0)
		{
			return this.array[this.Count - 1];
		}
		return default(T);
	}

	// Token: 0x0600A2F9 RID: 41721 RVA: 0x003CD234 File Offset: 0x003CB434
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

	// Token: 0x0600A2FA RID: 41722 RVA: 0x003CD2B8 File Offset: 0x003CB4B8
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

	// Token: 0x0600A2FB RID: 41723 RVA: 0x003CD344 File Offset: 0x003CB544
	public void AddSafe(T value)
	{
		T[] array = this.array;
		int count = this.Count;
		this.Count = count + 1;
		array[count] = value;
	}

	// Token: 0x0600A2FC RID: 41724 RVA: 0x003CD370 File Offset: 0x003CB570
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

	// Token: 0x0600A2FD RID: 41725 RVA: 0x003CD454 File Offset: 0x003CB654
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

	// Token: 0x0600A2FE RID: 41726 RVA: 0x003CD4D4 File Offset: 0x003CB6D4
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

	// Token: 0x0600A2FF RID: 41727 RVA: 0x003CD564 File Offset: 0x003CB764
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

	// Token: 0x0600A300 RID: 41728 RVA: 0x003CD5DC File Offset: 0x003CB7DC
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

	// Token: 0x0600A301 RID: 41729 RVA: 0x003CD641 File Offset: 0x003CB841
	public bool Contains(T obj)
	{
		return Array.IndexOf<T>(this.array, obj, 0, this.Count) >= 0;
	}

	// Token: 0x0600A302 RID: 41730 RVA: 0x003CD65C File Offset: 0x003CB85C
	public void Set(T[] values)
	{
		this.array = values;
		this.length = values.Length;
		this.Count = this.length;
	}

	// Token: 0x0600A303 RID: 41731 RVA: 0x003CD67A File Offset: 0x003CB87A
	public void Clear()
	{
		if (this.Count > 0)
		{
			Array.Clear(this.array, 0, this.array.Length);
		}
		this.Count = 0;
	}

	// Token: 0x0600A304 RID: 41732 RVA: 0x003CD6A0 File Offset: 0x003CB8A0
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

	// Token: 0x0600A305 RID: 41733 RVA: 0x003CD6D8 File Offset: 0x003CB8D8
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

	// Token: 0x0600A306 RID: 41734 RVA: 0x003CD747 File Offset: 0x003CB947
	public void Sort(IComparer<T> comparer)
	{
		this.Sort(0, this.Count, comparer);
	}

	// Token: 0x0600A307 RID: 41735 RVA: 0x003CD757 File Offset: 0x003CB957
	public void Sort(int index, int count, IComparer<T> comparer)
	{
		if (this.length - index < count)
		{
			return;
		}
		Array.Sort<T>(this.array, index, count, comparer);
	}

	// Token: 0x17001315 RID: 4885
	// (get) Token: 0x0600A308 RID: 41736 RVA: 0x003CD773 File Offset: 0x003CB973
	public int Length
	{
		get
		{
			return this.Count;
		}
	}

	// Token: 0x040079D4 RID: 31188
	public T[] array;

	// Token: 0x040079D5 RID: 31189
	public int Count;

	// Token: 0x040079D6 RID: 31190
	public int length;

	// Token: 0x040079D7 RID: 31191
	public int DoubleSize;

	// Token: 0x040079D8 RID: 31192
	public bool IsValueType;
}
