using System;

// Token: 0x02001382 RID: 4994
public class ArrayListMP<T> where T : new()
{
	// Token: 0x06009D96 RID: 40342 RVA: 0x003B8E4A File Offset: 0x003B704A
	public ArrayListMP(MemoryPooledArray<T> _pool, int _minSize = 0)
	{
		this.pool = _pool;
		this.Count = 0;
		if (_minSize > 0)
		{
			this.Items = this.pool.Alloc(_minSize);
		}
	}

	// Token: 0x06009D97 RID: 40343 RVA: 0x003B8E78 File Offset: 0x003B7078
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~ArrayListMP()
	{
		if (this.Items != null)
		{
			this.pool.Free(this.Items);
			this.Items = null;
		}
	}

	// Token: 0x06009D98 RID: 40344 RVA: 0x003B8EC0 File Offset: 0x003B70C0
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

	// Token: 0x1700129D RID: 4765
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

	// Token: 0x06009D9B RID: 40347 RVA: 0x003B8F48 File Offset: 0x003B7148
	public void Clear()
	{
		this.Count = 0;
		if (this.Items != null)
		{
			this.pool.Free(this.Items);
			this.Items = null;
		}
	}

	// Token: 0x06009D9C RID: 40348 RVA: 0x003B8F74 File Offset: 0x003B7174
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

	// Token: 0x06009D9D RID: 40349 RVA: 0x003B8FAF File Offset: 0x003B71AF
	public void AddRange(T[] _range)
	{
		this.AddRange(_range, 0, _range.Length);
	}

	// Token: 0x06009D9E RID: 40350 RVA: 0x003B8FBC File Offset: 0x003B71BC
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

	// Token: 0x06009D9F RID: 40351 RVA: 0x003B9040 File Offset: 0x003B7240
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

	// Token: 0x06009DA0 RID: 40352 RVA: 0x003B90AE File Offset: 0x003B72AE
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

	// Token: 0x040077AA RID: 30634
	public readonly MemoryPooledArray<T> pool;

	// Token: 0x040077AB RID: 30635
	public T[] Items;

	// Token: 0x040077AC RID: 30636
	public int Count;
}
