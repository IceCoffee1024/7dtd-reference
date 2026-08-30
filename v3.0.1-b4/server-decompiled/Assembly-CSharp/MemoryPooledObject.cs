using System;
using System.Collections.Generic;

// Token: 0x02001428 RID: 5160
public class MemoryPooledObject<T> where T : IMemoryPoolableObject, new()
{
	// Token: 0x0600A1F1 RID: 41457 RVA: 0x003C95B6 File Offset: 0x003C77B6
	public MemoryPooledObject(int _maxCapacity)
	{
		this.pool = new List<T>(_maxCapacity);
		this.maxCapacity = _maxCapacity;
	}

	// Token: 0x0600A1F2 RID: 41458 RVA: 0x003C95D1 File Offset: 0x003C77D1
	public void SetCapacity(int _maxCapacity)
	{
		this.pool.Capacity = _maxCapacity;
		this.maxCapacity = _maxCapacity;
	}

	// Token: 0x0600A1F3 RID: 41459 RVA: 0x003C95E8 File Offset: 0x003C77E8
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

	// Token: 0x0600A1F4 RID: 41460 RVA: 0x003C962C File Offset: 0x003C782C
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

	// Token: 0x0600A1F5 RID: 41461 RVA: 0x003C9694 File Offset: 0x003C7894
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

	// Token: 0x0600A1F6 RID: 41462 RVA: 0x003C9708 File Offset: 0x003C7908
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

	// Token: 0x0600A1F7 RID: 41463 RVA: 0x003C975C File Offset: 0x003C795C
	public void FreeSync(T _t)
	{
		List<T> list = this.pool;
		lock (list)
		{
			this.Free(_t);
		}
	}

	// Token: 0x0600A1F8 RID: 41464 RVA: 0x003C97A0 File Offset: 0x003C79A0
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

	// Token: 0x0600A1F9 RID: 41465 RVA: 0x003C97E4 File Offset: 0x003C79E4
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

	// Token: 0x0600A1FA RID: 41466 RVA: 0x003C9820 File Offset: 0x003C7A20
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

	// Token: 0x0600A1FB RID: 41467 RVA: 0x003C98A0 File Offset: 0x003C7AA0
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

	// Token: 0x0600A1FC RID: 41468 RVA: 0x003C993C File Offset: 0x003C7B3C
	public int GetPoolSize()
	{
		return this.poolSize;
	}

	// Token: 0x0400794F RID: 31055
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> pool;

	// Token: 0x04007950 RID: 31056
	[PublicizedFrom(EAccessModifier.Private)]
	public int poolSize;

	// Token: 0x04007951 RID: 31057
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCapacity;
}
