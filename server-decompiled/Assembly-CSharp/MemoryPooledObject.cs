using System;
using System.Collections.Generic;

// Token: 0x02001463 RID: 5219
public class MemoryPooledObject<T> where T : IMemoryPoolableObject, new()
{
	// Token: 0x0600A39A RID: 41882 RVA: 0x003D350A File Offset: 0x003D170A
	public MemoryPooledObject(int _maxCapacity)
	{
		this.pool = new List<T>(_maxCapacity);
		this.maxCapacity = _maxCapacity;
	}

	// Token: 0x0600A39B RID: 41883 RVA: 0x003D3525 File Offset: 0x003D1725
	public void SetCapacity(int _maxCapacity)
	{
		this.pool.Capacity = _maxCapacity;
		this.maxCapacity = _maxCapacity;
	}

	// Token: 0x0600A39C RID: 41884 RVA: 0x003D353C File Offset: 0x003D173C
	public T AllocSync(bool _bReset)
	{
		List<T> list = this.pool;
		T t;
		lock (list)
		{
			t = this.Alloc(_bReset);
		}
		return t;
	}

	// Token: 0x0600A39D RID: 41885 RVA: 0x003D3580 File Offset: 0x003D1780
	public T Alloc(bool _bReset)
	{
		T t;
		if (this.poolSize == 0)
		{
			t = new T();
		}
		else
		{
			this.poolSize--;
			t = this.pool[this.poolSize];
			this.pool[this.poolSize] = default(T);
		}
		if (_bReset)
		{
			t.Reset();
		}
		return t;
	}

	// Token: 0x0600A39E RID: 41886 RVA: 0x003D35E8 File Offset: 0x003D17E8
	public void FreeSync(IList<T> _array)
	{
		List<T> list = this.pool;
		lock (list)
		{
			for (int i = 0; i < _array.Count; i++)
			{
				T t = _array[i];
				if (t != null)
				{
					this.Free(t);
					_array[i] = default(T);
				}
			}
		}
	}

	// Token: 0x0600A39F RID: 41887 RVA: 0x003D365C File Offset: 0x003D185C
	public void FreeSync(Queue<T> _queue)
	{
		List<T> list = this.pool;
		lock (list)
		{
			while (_queue.Count > 0)
			{
				this.Free(_queue.Dequeue());
			}
		}
	}

	// Token: 0x0600A3A0 RID: 41888 RVA: 0x003D36B0 File Offset: 0x003D18B0
	public void FreeSync(T _t)
	{
		List<T> list = this.pool;
		lock (list)
		{
			this.Free(_t);
		}
	}

	// Token: 0x0600A3A1 RID: 41889 RVA: 0x003D36F4 File Offset: 0x003D18F4
	public void Free(T[] _array)
	{
		for (int i = 0; i < _array.Length; i++)
		{
			T t = _array[i];
			if (t != null)
			{
				this.Free(t);
				_array[i] = default(T);
			}
		}
	}

	// Token: 0x0600A3A2 RID: 41890 RVA: 0x003D3738 File Offset: 0x003D1938
	public void Free(List<T> _list)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			T t = _list[i];
			if (t != null)
			{
				this.Free(t);
			}
		}
		_list.Clear();
	}

	// Token: 0x0600A3A3 RID: 41891 RVA: 0x003D3774 File Offset: 0x003D1974
	public void Cleanup()
	{
		List<T> list = this.pool;
		lock (list)
		{
			for (int i = 0; i < this.poolSize; i++)
			{
				T t = this.pool[i];
				if (t != null)
				{
					t.Cleanup();
				}
			}
			this.pool.Clear();
			this.poolSize = 0;
		}
	}

	// Token: 0x0600A3A4 RID: 41892 RVA: 0x003D37F4 File Offset: 0x003D19F4
	public void Free(T _t)
	{
		if (this.poolSize >= this.pool.Count && this.poolSize < this.maxCapacity)
		{
			_t.Reset();
			this.pool.Add(_t);
			this.poolSize++;
			return;
		}
		if (this.poolSize < this.maxCapacity)
		{
			_t.Reset();
			List<T> list = this.pool;
			int num = this.poolSize;
			this.poolSize = num + 1;
			list[num] = _t;
			return;
		}
		_t.Cleanup();
	}

	// Token: 0x0600A3A5 RID: 41893 RVA: 0x003D3890 File Offset: 0x003D1A90
	public int GetPoolSize()
	{
		return this.poolSize;
	}

	// Token: 0x04007AA3 RID: 31395
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> pool;

	// Token: 0x04007AA4 RID: 31396
	[PublicizedFrom(EAccessModifier.Private)]
	public int poolSize;

	// Token: 0x04007AA5 RID: 31397
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCapacity;
}
