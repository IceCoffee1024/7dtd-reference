using System;

// Token: 0x020013A2 RID: 5026
public class ArrayListMP<T> where T : new()
{
	// Token: 0x06009E41 RID: 40513 RVA: 0x003BD9C6 File Offset: 0x003BBBC6
	public ArrayListMP(MemoryPooledArray<T> _pool, int _minSize = 0)
	{
		this.pool = _pool;
		this.Count = 0;
		if (_minSize > 0)
		{
			this.Items = this.pool.Alloc(_minSize);
		}
	}

	// Token: 0x06009E42 RID: 40514 RVA: 0x003BD9F4 File Offset: 0x003BBBF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~ArrayListMP()
	{
		if (this.Items != null)
		{
			this.pool.Free(this.Items);
			this.Items = null;
		}
	}

	// Token: 0x06009E43 RID: 40515 RVA: 0x003BDA3C File Offset: 0x003BBC3C
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

	// Token: 0x170012E2 RID: 4834
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

	// Token: 0x06009E46 RID: 40518 RVA: 0x003BDAC4 File Offset: 0x003BBCC4
	public void Clear()
	{
		this.Count = 0;
		if (this.Items != null)
		{
			this.pool.Free(this.Items);
			this.Items = null;
		}
	}

	// Token: 0x06009E47 RID: 40519 RVA: 0x003BDAF0 File Offset: 0x003BBCF0
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

	// Token: 0x06009E48 RID: 40520 RVA: 0x003BDB2B File Offset: 0x003BBD2B
	public void AddRange(T[] _range)
	{
		this.AddRange(_range, 0, _range.Length);
	}

	// Token: 0x06009E49 RID: 40521 RVA: 0x003BDB38 File Offset: 0x003BBD38
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

	// Token: 0x06009E4A RID: 40522 RVA: 0x003BDBBC File Offset: 0x003BBDBC
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

	// Token: 0x06009E4B RID: 40523 RVA: 0x003BDC2A File Offset: 0x003BBE2A
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

	// Token: 0x04007825 RID: 30757
	public readonly MemoryPooledArray<T> pool;

	// Token: 0x04007826 RID: 30758
	public T[] Items;

	// Token: 0x04007827 RID: 30759
	public int Count;
}
