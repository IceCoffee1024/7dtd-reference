using System;

// Token: 0x02001368 RID: 4968
public class ArrayListMP<T> where T : new()
{
	// Token: 0x06009C9B RID: 40091 RVA: 0x003B3B96 File Offset: 0x003B1D96
	public ArrayListMP(MemoryPooledArray<T> _pool, int _minSize = 0)
	{
		this.pool = _pool;
		this.Count = 0;
		if (_minSize > 0)
		{
			this.Items = this.pool.Alloc(_minSize);
		}
	}

	// Token: 0x06009C9C RID: 40092 RVA: 0x003B3BC4 File Offset: 0x003B1DC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~ArrayListMP()
	{
		if (this.Items != null)
		{
			this.pool.Free(this.Items);
			this.Items = null;
		}
	}

	// Token: 0x06009C9D RID: 40093 RVA: 0x003B3C0C File Offset: 0x003B1E0C
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

	// Token: 0x17001264 RID: 4708
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

	// Token: 0x06009CA0 RID: 40096 RVA: 0x003B3C94 File Offset: 0x003B1E94
	public void Clear()
	{
		this.Count = 0;
		if (this.Items != null)
		{
			this.pool.Free(this.Items);
			this.Items = null;
		}
	}

	// Token: 0x06009CA1 RID: 40097 RVA: 0x003B3CC0 File Offset: 0x003B1EC0
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

	// Token: 0x06009CA2 RID: 40098 RVA: 0x003B3CFB File Offset: 0x003B1EFB
	public void AddRange(T[] _range)
	{
		this.AddRange(_range, 0, _range.Length);
	}

	// Token: 0x06009CA3 RID: 40099 RVA: 0x003B3D08 File Offset: 0x003B1F08
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

	// Token: 0x06009CA4 RID: 40100 RVA: 0x003B3D8C File Offset: 0x003B1F8C
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

	// Token: 0x06009CA5 RID: 40101 RVA: 0x003B3DFA File Offset: 0x003B1FFA
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

	// Token: 0x040076D2 RID: 30418
	public readonly MemoryPooledArray<T> pool;

	// Token: 0x040076D3 RID: 30419
	public T[] Items;

	// Token: 0x040076D4 RID: 30420
	public int Count;
}
