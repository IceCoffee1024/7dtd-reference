using System;
using System.Collections.Generic;

// Token: 0x02001475 RID: 5237
public class MemoryPooledObject<T> where T : IMemoryPoolableObject, new()
{
	// Token: 0x0600A3F2 RID: 41970 RVA: 0x003D590E File Offset: 0x003D3B0E
	public MemoryPooledObject(int _maxCapacity)
	{
		this.pool = new List<T>(_maxCapacity);
		this.maxCapacity = _maxCapacity;
	}

	// Token: 0x0600A3F3 RID: 41971 RVA: 0x003D5929 File Offset: 0x003D3B29
	public void SetCapacity(int _maxCapacity)
	{
		this.pool.Capacity = _maxCapacity;
		this.maxCapacity = _maxCapacity;
	}

	// Token: 0x0600A3F4 RID: 41972 RVA: 0x003D5940 File Offset: 0x003D3B40
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

	// Token: 0x0600A3F5 RID: 41973 RVA: 0x003D5984 File Offset: 0x003D3B84
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

	// Token: 0x0600A3F6 RID: 41974 RVA: 0x003D59EC File Offset: 0x003D3BEC
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

	// Token: 0x0600A3F7 RID: 41975 RVA: 0x003D5A60 File Offset: 0x003D3C60
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

	// Token: 0x0600A3F8 RID: 41976 RVA: 0x003D5AB4 File Offset: 0x003D3CB4
	public void FreeSync(T _t)
	{
		List<T> list = this.pool;
		lock (list)
		{
			this.Free(_t);
		}
	}

	// Token: 0x0600A3F9 RID: 41977 RVA: 0x003D5AF8 File Offset: 0x003D3CF8
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

	// Token: 0x0600A3FA RID: 41978 RVA: 0x003D5B3C File Offset: 0x003D3D3C
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

	// Token: 0x0600A3FB RID: 41979 RVA: 0x003D5B78 File Offset: 0x003D3D78
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

	// Token: 0x0600A3FC RID: 41980 RVA: 0x003D5BF8 File Offset: 0x003D3DF8
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

	// Token: 0x0600A3FD RID: 41981 RVA: 0x003D5C94 File Offset: 0x003D3E94
	public int GetPoolSize()
	{
		return this.poolSize;
	}

	// Token: 0x04007AE4 RID: 31460
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> pool;

	// Token: 0x04007AE5 RID: 31461
	[PublicizedFrom(EAccessModifier.Private)]
	public int poolSize;

	// Token: 0x04007AE6 RID: 31462
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCapacity;
}
