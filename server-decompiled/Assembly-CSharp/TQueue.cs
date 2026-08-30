using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

// Token: 0x020014D3 RID: 5331
public class TQueue<T> : ICollection, IEnumerable
{
	// Token: 0x0600A786 RID: 42886 RVA: 0x003E4D57 File Offset: 0x003E2F57
	public TQueue()
	{
		this.m_Queue = new Queue<T>();
	}

	// Token: 0x0600A787 RID: 42887 RVA: 0x003E4D80 File Offset: 0x003E2F80
	public TQueue(int capacity)
	{
		this.m_Queue = new Queue<T>(capacity);
	}

	// Token: 0x0600A788 RID: 42888 RVA: 0x003E4DAA File Offset: 0x003E2FAA
	public TQueue(IEnumerable<T> collection)
	{
		this.m_Queue = new Queue<T>(collection);
	}

	// Token: 0x0600A789 RID: 42889 RVA: 0x003E4DD4 File Offset: 0x003E2FD4
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

	// Token: 0x0600A78A RID: 42890 RVA: 0x003E4DE3 File Offset: 0x003E2FE3
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

	// Token: 0x0600A78B RID: 42891 RVA: 0x003E4DF4 File Offset: 0x003E2FF4
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

	// Token: 0x0600A78C RID: 42892 RVA: 0x003E4E3C File Offset: 0x003E303C
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

	// Token: 0x170013A0 RID: 5024
	// (get) Token: 0x0600A78D RID: 42893 RVA: 0x003E4E80 File Offset: 0x003E3080
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

	// Token: 0x170013A1 RID: 5025
	// (get) Token: 0x0600A78E RID: 42894 RVA: 0x00020021 File Offset: 0x0001E221
	public bool IsSynchronized
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170013A2 RID: 5026
	// (get) Token: 0x0600A78F RID: 42895 RVA: 0x003E4EC4 File Offset: 0x003E30C4
	public object SyncRoot
	{
		get
		{
			return this.objSyncRoot;
		}
	}

	// Token: 0x0600A790 RID: 42896 RVA: 0x003E4ECC File Offset: 0x003E30CC
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

	// Token: 0x0600A791 RID: 42897 RVA: 0x003E4F10 File Offset: 0x003E3110
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

	// Token: 0x0600A792 RID: 42898 RVA: 0x003E4F54 File Offset: 0x003E3154
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

	// Token: 0x0600A793 RID: 42899 RVA: 0x003E4FC4 File Offset: 0x003E31C4
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

	// Token: 0x0600A794 RID: 42900 RVA: 0x003E5040 File Offset: 0x003E3240
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

	// Token: 0x0600A795 RID: 42901 RVA: 0x003E50A0 File Offset: 0x003E32A0
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

	// Token: 0x0600A796 RID: 42902 RVA: 0x003E50E4 File Offset: 0x003E32E4
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

	// Token: 0x0600A797 RID: 42903 RVA: 0x003E5128 File Offset: 0x003E3328
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

	// Token: 0x0600A798 RID: 42904 RVA: 0x003E516C File Offset: 0x003E336C
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

	// Token: 0x0600A799 RID: 42905 RVA: 0x003E51B0 File Offset: 0x003E33B0
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

	// Token: 0x04007C32 RID: 31794
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<T> m_Queue;

	// Token: 0x04007C33 RID: 31795
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim LockQ = new ReaderWriterLockSlim();

	// Token: 0x04007C34 RID: 31796
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object objSyncRoot = new object();
}
