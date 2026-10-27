using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

// Token: 0x02001507 RID: 5383
public class TQueue<T> : ICollection, IEnumerable
{
	// Token: 0x0600A891 RID: 43153 RVA: 0x003EBFAF File Offset: 0x003EA1AF
	public TQueue()
	{
		this.m_Queue = new Queue<T>();
	}

	// Token: 0x0600A892 RID: 43154 RVA: 0x003EBFD8 File Offset: 0x003EA1D8
	public TQueue(int capacity)
	{
		this.m_Queue = new Queue<T>(capacity);
	}

	// Token: 0x0600A893 RID: 43155 RVA: 0x003EC002 File Offset: 0x003EA202
	public TQueue(IEnumerable<T> collection)
	{
		this.m_Queue = new Queue<T>(collection);
	}

	// Token: 0x0600A894 RID: 43156 RVA: 0x003EC02C File Offset: 0x003EA22C
	public IEnumerator<T> GetEnumerator()
	{
		this.LockQ.EnterReadLock();
		Queue<T> queue;
		try
		{
			queue = new Queue<T>(this.m_Queue);
		}
		finally
		{
			this.LockQ.ExitReadLock();
		}
		foreach (T t in queue)
		{
			yield return t;
		}
		Queue<T>.Enumerator enumerator = default(Queue<T>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600A895 RID: 43157 RVA: 0x003EC03B File Offset: 0x003EA23B
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		this.LockQ.EnterReadLock();
		Queue<T> queue;
		try
		{
			queue = new Queue<T>(this.m_Queue);
		}
		finally
		{
			this.LockQ.ExitReadLock();
		}
		foreach (T t in queue)
		{
			yield return t;
		}
		Queue<T>.Enumerator enumerator = default(Queue<T>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600A896 RID: 43158 RVA: 0x003EC04C File Offset: 0x003EA24C
	public void CopyTo(Array array, int index)
	{
		this.LockQ.EnterReadLock();
		try
		{
			this.m_Queue.ToArray().CopyTo(array, index);
		}
		finally
		{
			this.LockQ.ExitReadLock();
		}
	}

	// Token: 0x0600A897 RID: 43159 RVA: 0x003EC094 File Offset: 0x003EA294
	public void CopyTo(T[] array, int index)
	{
		this.LockQ.EnterReadLock();
		try
		{
			this.m_Queue.CopyTo(array, index);
		}
		finally
		{
			this.LockQ.ExitReadLock();
		}
	}

	// Token: 0x170013F1 RID: 5105
	// (get) Token: 0x0600A898 RID: 43160 RVA: 0x003EC0D8 File Offset: 0x003EA2D8
	public int Count
	{
		get
		{
			this.LockQ.EnterReadLock();
			int count;
			try
			{
				count = this.m_Queue.Count;
			}
			finally
			{
				this.LockQ.ExitReadLock();
			}
			return count;
		}
	}

	// Token: 0x170013F2 RID: 5106
	// (get) Token: 0x0600A899 RID: 43161 RVA: 0x00020119 File Offset: 0x0001E319
	public bool IsSynchronized
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170013F3 RID: 5107
	// (get) Token: 0x0600A89A RID: 43162 RVA: 0x003EC11C File Offset: 0x003EA31C
	public object SyncRoot
	{
		get
		{
			return this.objSyncRoot;
		}
	}

	// Token: 0x0600A89B RID: 43163 RVA: 0x003EC124 File Offset: 0x003EA324
	public void Enqueue(T item)
	{
		this.LockQ.EnterWriteLock();
		try
		{
			this.m_Queue.Enqueue(item);
		}
		finally
		{
			this.LockQ.ExitWriteLock();
		}
	}

	// Token: 0x0600A89C RID: 43164 RVA: 0x003EC168 File Offset: 0x003EA368
	public T Dequeue()
	{
		this.LockQ.EnterWriteLock();
		T t;
		try
		{
			t = this.m_Queue.Dequeue();
		}
		finally
		{
			this.LockQ.ExitWriteLock();
		}
		return t;
	}

	// Token: 0x0600A89D RID: 43165 RVA: 0x003EC1AC File Offset: 0x003EA3AC
	public void EnqueueAll(IEnumerable<T> ItemsToQueue)
	{
		this.LockQ.EnterWriteLock();
		try
		{
			foreach (T t in ItemsToQueue)
			{
				this.m_Queue.Enqueue(t);
			}
		}
		finally
		{
			this.LockQ.ExitWriteLock();
		}
	}

	// Token: 0x0600A89E RID: 43166 RVA: 0x003EC21C File Offset: 0x003EA41C
	public void EnqueueAll(TList<T> ItemsToQueue)
	{
		this.LockQ.EnterWriteLock();
		try
		{
			foreach (object obj in ItemsToQueue)
			{
				T t = (T)((object)obj);
				this.m_Queue.Enqueue(t);
			}
		}
		finally
		{
			this.LockQ.ExitWriteLock();
		}
	}

	// Token: 0x0600A89F RID: 43167 RVA: 0x003EC298 File Offset: 0x003EA498
	public TList<T> DequeueAll()
	{
		this.LockQ.EnterWriteLock();
		TList<T> tlist2;
		try
		{
			TList<T> tlist = new TList<T>();
			while (this.m_Queue.Count > 0)
			{
				tlist.Add(this.m_Queue.Dequeue());
			}
			tlist2 = tlist;
		}
		finally
		{
			this.LockQ.ExitWriteLock();
		}
		return tlist2;
	}

	// Token: 0x0600A8A0 RID: 43168 RVA: 0x003EC2F8 File Offset: 0x003EA4F8
	public void Clear()
	{
		this.LockQ.EnterWriteLock();
		try
		{
			this.m_Queue.Clear();
		}
		finally
		{
			this.LockQ.ExitWriteLock();
		}
	}

	// Token: 0x0600A8A1 RID: 43169 RVA: 0x003EC33C File Offset: 0x003EA53C
	public bool Contains(T item)
	{
		this.LockQ.EnterReadLock();
		bool flag;
		try
		{
			flag = this.m_Queue.Contains(item);
		}
		finally
		{
			this.LockQ.ExitReadLock();
		}
		return flag;
	}

	// Token: 0x0600A8A2 RID: 43170 RVA: 0x003EC380 File Offset: 0x003EA580
	public T Peek()
	{
		this.LockQ.EnterReadLock();
		T t;
		try
		{
			t = this.m_Queue.Peek();
		}
		finally
		{
			this.LockQ.ExitReadLock();
		}
		return t;
	}

	// Token: 0x0600A8A3 RID: 43171 RVA: 0x003EC3C4 File Offset: 0x003EA5C4
	public T[] ToArray()
	{
		this.LockQ.EnterReadLock();
		T[] array;
		try
		{
			array = this.m_Queue.ToArray();
		}
		finally
		{
			this.LockQ.ExitReadLock();
		}
		return array;
	}

	// Token: 0x0600A8A4 RID: 43172 RVA: 0x003EC408 File Offset: 0x003EA608
	public void TrimExcess()
	{
		this.LockQ.EnterWriteLock();
		try
		{
			this.m_Queue.TrimExcess();
		}
		finally
		{
			this.LockQ.ExitWriteLock();
		}
	}

	// Token: 0x04007CF6 RID: 31990
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<T> m_Queue;

	// Token: 0x04007CF7 RID: 31991
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim LockQ = new ReaderWriterLockSlim();

	// Token: 0x04007CF8 RID: 31992
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object objSyncRoot = new object();
}
