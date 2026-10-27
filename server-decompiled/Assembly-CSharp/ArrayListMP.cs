using System;

// Token: 0x020013B4 RID: 5044
public class ArrayListMP<T> where T : new()
{
	// Token: 0x06009E99 RID: 40601 RVA: 0x003BFDCA File Offset: 0x003BDFCA
	public ArrayListMP(MemoryPooledArray<T> _pool, int _minSize = 0)
	{
		this.pool = _pool;
		this.Count = 0;
		if (_minSize > 0)
		{
			this.Items = this.pool.Alloc(_minSize);
		}
	}

	// Token: 0x06009E9A RID: 40602 RVA: 0x003BFDF8 File Offset: 0x003BDFF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~ArrayListMP()
	{
		if (this.Items != null)
		{
			this.pool.Free(this.Items);
			this.Items = null;
		}
	}

	// Token: 0x06009E9B RID: 40603 RVA: 0x003BFE40 File Offset: 0x003BE040
	public void Add(T _item)
	{
		if (this.Items == null)
		{
			this.Items = this.pool.Alloc(0);
		}
		if (this.Count >= this.Items.Length)
		{
			this.Items = this.pool.Grow(this.Items);
		}
		T[] items = this.Items;
		int count = this.Count;
		this.Count = count + 1;
		items[count] = _item;
	}

	// Token: 0x170012EE RID: 4846
	public T this[int idx]
	{
		get
		{
			return this.Items[idx];
		}
		set
		{
			this.Items[idx] = value;
		}
	}

	// Token: 0x06009E9E RID: 40606 RVA: 0x003BFEC8 File Offset: 0x003BE0C8
	public void Clear()
	{
		this.Count = 0;
		if (this.Items != null)
		{
			this.pool.Free(this.Items);
			this.Items = null;
		}
	}

	// Token: 0x06009E9F RID: 40607 RVA: 0x003BFEF4 File Offset: 0x003BE0F4
	public T[] ToArray()
	{
		if (this.Items == null)
		{
			return new T[0];
		}
		T[] array = new T[this.Count];
		Array.Copy(this.Items, array, this.Count);
		return array;
	}

	// Token: 0x06009EA0 RID: 40608 RVA: 0x003BFF2F File Offset: 0x003BE12F
	public void AddRange(T[] _range)
	{
		this.AddRange(_range, 0, _range.Length);
	}

	// Token: 0x06009EA1 RID: 40609 RVA: 0x003BFF3C File Offset: 0x003BE13C
	public void AddRange(T[] _range, int _offs, int _count)
	{
		if (_range == null || _range.Length == 0)
		{
			return;
		}
		if (this.Items == null)
		{
			this.Items = this.pool.Alloc(_count);
		}
		if (this.Count + _count >= this.Items.Length)
		{
			this.Items = this.pool.Grow(this.Items, this.Count + _count);
		}
		Array.Copy(_range, _offs, this.Items, this.Count, _count);
		this.Count += _count;
	}

	// Token: 0x06009EA2 RID: 40610 RVA: 0x003BFFC0 File Offset: 0x003BE1C0
	public int Alloc(int _count)
	{
		if (this.Items == null)
		{
			this.Items = this.pool.Alloc(_count);
		}
		else if (this.Count + _count > this.Items.Length)
		{
			this.Items = this.pool.Grow(this.Items, this.Count + _count);
		}
		int count = this.Count;
		this.Count += _count;
		return count;
	}

	// Token: 0x06009EA3 RID: 40611 RVA: 0x003C002E File Offset: 0x003BE22E
	public void Grow(int newSize)
	{
		if (this.Items == null)
		{
			this.Items = this.pool.Alloc(newSize);
			return;
		}
		if (newSize > this.Items.Length)
		{
			this.Items = this.pool.Grow(this.Items, newSize);
		}
	}

	// Token: 0x04007866 RID: 30822
	public readonly MemoryPooledArray<T> pool;

	// Token: 0x04007867 RID: 30823
	public T[] Items;

	// Token: 0x04007868 RID: 30824
	public int Count;
}
