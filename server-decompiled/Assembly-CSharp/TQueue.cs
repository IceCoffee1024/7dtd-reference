using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

// Token: 0x020014B8 RID: 5304
public class TQueue<T> : ICollection, IEnumerable
{
	// Token: 0x0600A689 RID: 42633 RVA: 0x003DFA17 File Offset: 0x003DDC17
	public TQueue()
	{
		this.m_Queue = new Queue<T>();
	}

	// Token: 0x0600A68A RID: 42634 RVA: 0x003DFA40 File Offset: 0x003DDC40
	public TQueue(int capacity)
	{
		this.m_Queue = new Queue<T>(capacity);
	}

	// Token: 0x0600A68B RID: 42635 RVA: 0x003DFA6A File Offset: 0x003DDC6A
	public TQueue(IEnumerable<T> collection)
	{
		this.m_Queue = new Queue<T>(collection);
	}

	// Token: 0x0600A68C RID: 42636 RVA: 0x003DFA94 File Offset: 0x003DDC94
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

	// Token: 0x0600A68D RID: 42637 RVA: 0x003DFAA3 File Offset: 0x003DDCA3
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

	// Token: 0x0600A68E RID: 42638 RVA: 0x003DFAB4 File Offset: 0x003DDCB4
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

	// Token: 0x0600A68F RID: 42639 RVA: 0x003DFAFC File Offset: 0x003DDCFC
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

	// Token: 0x17001367 RID: 4967
	// (get) Token: 0x0600A690 RID: 42640 RVA: 0x003DFB40 File Offset: 0x003DDD40
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

	// Token: 0x17001368 RID: 4968
	// (get) Token: 0x0600A691 RID: 42641 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool IsSynchronized
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001369 RID: 4969
	// (get) Token: 0x0600A692 RID: 42642 RVA: 0x003DFB84 File Offset: 0x003DDD84
	public object SyncRoot
	{
		get
		{
			return this.objSyncRoot;
		}
	}

	// Token: 0x0600A693 RID: 42643 RVA: 0x003DFB8C File Offset: 0x003DDD8C
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

	// Token: 0x0600A694 RID: 42644 RVA: 0x003DFBD0 File Offset: 0x003DDDD0
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

	// Token: 0x0600A695 RID: 42645 RVA: 0x003DFC14 File Offset: 0x003DDE14
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

	// Token: 0x0600A696 RID: 42646 RVA: 0x003DFC84 File Offset: 0x003DDE84
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

	// Token: 0x0600A697 RID: 42647 RVA: 0x003DFD00 File Offset: 0x003DDF00
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

	// Token: 0x0600A698 RID: 42648 RVA: 0x003DFD60 File Offset: 0x003DDF60
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

	// Token: 0x0600A699 RID: 42649 RVA: 0x003DFDA4 File Offset: 0x003DDFA4
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

	// Token: 0x0600A69A RID: 42650 RVA: 0x003DFDE8 File Offset: 0x003DDFE8
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

	// Token: 0x0600A69B RID: 42651 RVA: 0x003DFE2C File Offset: 0x003DE02C
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

	// Token: 0x0600A69C RID: 42652 RVA: 0x003DFE70 File Offset: 0x003DE070
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

	// Token: 0x04007B59 RID: 31577
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<T> m_Queue;

	// Token: 0x04007B5A RID: 31578
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim LockQ = new ReaderWriterLockSlim();

	// Token: 0x04007B5B RID: 31579
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object objSyncRoot = new object();
}
