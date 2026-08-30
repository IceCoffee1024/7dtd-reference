using System;
using System.Collections.Generic;

// Token: 0x02001443 RID: 5187
public class MemoryPooledObject<T> where T : IMemoryPoolableObject, new()
{
	// Token: 0x0600A2EE RID: 41710 RVA: 0x003CE8FA File Offset: 0x003CCAFA
	public MemoryPooledObject(int _maxCapacity)
	{
		this.pool = new List<T>(_maxCapacity);
		this.maxCapacity = _maxCapacity;
	}

	// Token: 0x0600A2EF RID: 41711 RVA: 0x003CE915 File Offset: 0x003CCB15
	public void SetCapacity(int _maxCapacity)
	{
		this.pool.Capacity = _maxCapacity;
		this.maxCapacity = _maxCapacity;
	}

	// Token: 0x0600A2F0 RID: 41712 RVA: 0x003CE92C File Offset: 0x003CCB2C
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

	// Token: 0x0600A2F1 RID: 41713 RVA: 0x003CE970 File Offset: 0x003CCB70
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

	// Token: 0x0600A2F2 RID: 41714 RVA: 0x003CE9D8 File Offset: 0x003CCBD8
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

	// Token: 0x0600A2F3 RID: 41715 RVA: 0x003CEA4C File Offset: 0x003CCC4C
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

	// Token: 0x0600A2F4 RID: 41716 RVA: 0x003CEAA0 File Offset: 0x003CCCA0
	public void FreeSync(T _t)
	{
		List<T> list = this.pool;
		lock (list)
		{
			this.Free(_t);
		}
	}

	// Token: 0x0600A2F5 RID: 41717 RVA: 0x003CEAE4 File Offset: 0x003CCCE4
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

	// Token: 0x0600A2F6 RID: 41718 RVA: 0x003CEB28 File Offset: 0x003CCD28
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

	// Token: 0x0600A2F7 RID: 41719 RVA: 0x003CEB64 File Offset: 0x003CCD64
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

	// Token: 0x0600A2F8 RID: 41720 RVA: 0x003CEBE4 File Offset: 0x003CCDE4
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

	// Token: 0x0600A2F9 RID: 41721 RVA: 0x003CEC80 File Offset: 0x003CCE80
	public int GetPoolSize()
	{
		return this.poolSize;
	}

	// Token: 0x04007A28 RID: 31272
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T> pool;

	// Token: 0x04007A29 RID: 31273
	[PublicizedFrom(EAccessModifier.Private)]
	public int poolSize;

	// Token: 0x04007A2A RID: 31274
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCapacity;
}
