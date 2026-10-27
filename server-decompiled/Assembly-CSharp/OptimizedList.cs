using System;
using System.Collections.Generic;

// Token: 0x0200149C RID: 5276
[Serializable]
public class OptimizedList<T>
{
	// Token: 0x0600A4F4 RID: 42228 RVA: 0x003D93C4 File Offset: 0x003D75C4
	public OptimizedList()
		: this(10)
	{
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A4F5 RID: 42229 RVA: 0x003D93E3 File Offset: 0x003D75E3
	public OptimizedList(int Capacity)
	{
		this.Count = 0;
		this.array = new T[Capacity];
		this.length = Capacity;
		this.DoubleSize = 0;
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A4F6 RID: 42230 RVA: 0x003D9424 File Offset: 0x003D7624
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

	// Token: 0x0600A4F7 RID: 42231 RVA: 0x003D94A4 File Offset: 0x003D76A4
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

	// Token: 0x0600A4F8 RID: 42232 RVA: 0x003D9517 File Offset: 0x003D7717
	public OptimizedList(int Capacity, int DoubleSize)
	{
		this.Count = 0;
		this.array = new T[Capacity];
		this.length = Capacity;
		this.DoubleSize = DoubleSize;
		this.IsValueType = typeof(T).IsValueType;
	}

	// Token: 0x0600A4F9 RID: 42233 RVA: 0x003D9558 File Offset: 0x003D7758
	public T Last()
	{
		if (this.Count > 0)
		{
			return this.array[this.Count - 1];
		}
		return default(T);
	}

	// Token: 0x0600A4FA RID: 42234 RVA: 0x003D958C File Offset: 0x003D778C
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

	// Token: 0x0600A4FB RID: 42235 RVA: 0x003D9610 File Offset: 0x003D7810
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

	// Token: 0x0600A4FC RID: 42236 RVA: 0x003D969C File Offset: 0x003D789C
	public void AddSafe(T value)
	{
		T[] array = this.array;
		int count = this.Count;
		this.Count = count + 1;
		array[count] = value;
	}

	// Token: 0x0600A4FD RID: 42237 RVA: 0x003D96C8 File Offset: 0x003D78C8
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

	// Token: 0x0600A4FE RID: 42238 RVA: 0x003D97AC File Offset: 0x003D79AC
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

	// Token: 0x0600A4FF RID: 42239 RVA: 0x003D982C File Offset: 0x003D7A2C
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

	// Token: 0x0600A500 RID: 42240 RVA: 0x003D98BC File Offset: 0x003D7ABC
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

	// Token: 0x0600A501 RID: 42241 RVA: 0x003D9934 File Offset: 0x003D7B34
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

	// Token: 0x0600A502 RID: 42242 RVA: 0x003D9999 File Offset: 0x003D7B99
	public bool Contains(T obj)
	{
		return Array.IndexOf<T>(this.array, obj, 0, this.Count) >= 0;
	}

	// Token: 0x0600A503 RID: 42243 RVA: 0x003D99B4 File Offset: 0x003D7BB4
	public void Set(T[] values)
	{
		this.array = values;
		this.length = values.Length;
		this.Count = this.length;
	}

	// Token: 0x0600A504 RID: 42244 RVA: 0x003D99D2 File Offset: 0x003D7BD2
	public void Clear()
	{
		if (this.Count > 0)
		{
			Array.Clear(this.array, 0, this.array.Length);
		}
		this.Count = 0;
	}

	// Token: 0x0600A505 RID: 42245 RVA: 0x003D99F8 File Offset: 0x003D7BF8
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

	// Token: 0x0600A506 RID: 42246 RVA: 0x003D9A30 File Offset: 0x003D7C30
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

	// Token: 0x0600A507 RID: 42247 RVA: 0x003D9A9F File Offset: 0x003D7C9F
	public void Sort(IComparer<T> comparer)
	{
		this.Sort(0, this.Count, comparer);
	}

	// Token: 0x0600A508 RID: 42248 RVA: 0x003D9AAF File Offset: 0x003D7CAF
	public void Sort(int index, int count, IComparer<T> comparer)
	{
		if (this.length - index < count)
		{
			return;
		}
		Array.Sort<T>(this.array, index, count, comparer);
	}

	// Token: 0x1700139F RID: 5023
	// (get) Token: 0x0600A509 RID: 42249 RVA: 0x003D9ACB File Offset: 0x003D7CCB
	public int Length
	{
		get
		{
			return this.Count;
		}
	}

	// Token: 0x04007B69 RID: 31593
	public T[] array;

	// Token: 0x04007B6A RID: 31594
	public int Count;

	// Token: 0x04007B6B RID: 31595
	public int length;

	// Token: 0x04007B6C RID: 31596
	public int DoubleSize;

	// Token: 0x04007B6D RID: 31597
	public bool IsValueType;
}
