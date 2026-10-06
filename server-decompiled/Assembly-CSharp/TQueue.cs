using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

// Token: 0x020014F5 RID: 5365
public class TQueue<T> : ICollection, IEnumerable
{
	// Token: 0x0600A839 RID: 43065 RVA: 0x003E9BAB File Offset: 0x003E7DAB
	public TQueue()
	{
		this.m_Queue = new Queue<T>();
	}

	// Token: 0x0600A83A RID: 43066 RVA: 0x003E9BD4 File Offset: 0x003E7DD4
	public TQueue(int capacity)
	{
		this.m_Queue = new Queue<T>(capacity);
	}

	// Token: 0x0600A83B RID: 43067 RVA: 0x003E9BFE File Offset: 0x003E7DFE
	public TQueue(IEnumerable<T> collection)
	{
		this.m_Queue = new Queue<T>(collection);
	}

	// Token: 0x0600A83C RID: 43068 RVA: 0x003E9C28 File Offset: 0x003E7E28
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

	// Token: 0x0600A83D RID: 43069 RVA: 0x003E9C37 File Offset: 0x003E7E37
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

	// Token: 0x0600A83E RID: 43070 RVA: 0x003E9C48 File Offset: 0x003E7E48
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

	// Token: 0x0600A83F RID: 43071 RVA: 0x003E9C90 File Offset: 0x003E7E90
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

	// Token: 0x170013E5 RID: 5093
	// (get) Token: 0x0600A840 RID: 43072 RVA: 0x003E9CD4 File Offset: 0x003E7ED4
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

	// Token: 0x170013E6 RID: 5094
	// (get) Token: 0x0600A841 RID: 43073 RVA: 0x00020119 File Offset: 0x0001E319
	public bool IsSynchronized
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170013E7 RID: 5095
	// (get) Token: 0x0600A842 RID: 43074 RVA: 0x003E9D18 File Offset: 0x003E7F18
	public object SyncRoot
	{
		get
		{
			return this.objSyncRoot;
		}
	}

	// Token: 0x0600A843 RID: 43075 RVA: 0x003E9D20 File Offset: 0x003E7F20
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

	// Token: 0x0600A844 RID: 43076 RVA: 0x003E9D64 File Offset: 0x003E7F64
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

	// Token: 0x0600A845 RID: 43077 RVA: 0x003E9DA8 File Offset: 0x003E7FA8
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

	// Token: 0x0600A846 RID: 43078 RVA: 0x003E9E18 File Offset: 0x003E8018
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

	// Token: 0x0600A847 RID: 43079 RVA: 0x003E9E94 File Offset: 0x003E8094
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

	// Token: 0x0600A848 RID: 43080 RVA: 0x003E9EF4 File Offset: 0x003E80F4
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

	// Token: 0x0600A849 RID: 43081 RVA: 0x003E9F38 File Offset: 0x003E8138
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

	// Token: 0x0600A84A RID: 43082 RVA: 0x003E9F7C File Offset: 0x003E817C
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

	// Token: 0x0600A84B RID: 43083 RVA: 0x003E9FC0 File Offset: 0x003E81C0
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

	// Token: 0x0600A84C RID: 43084 RVA: 0x003EA004 File Offset: 0x003E8204
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

	// Token: 0x04007CB5 RID: 31925
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<T> m_Queue;

	// Token: 0x04007CB6 RID: 31926
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim LockQ = new ReaderWriterLockSlim();

	// Token: 0x04007CB7 RID: 31927
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object objSyncRoot = new object();
}
