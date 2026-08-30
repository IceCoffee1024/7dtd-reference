using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

// Token: 0x020014B5 RID: 5301
public class TList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x1700135F RID: 4959
	// (get) Token: 0x0600A641 RID: 42561 RVA: 0x003DE9C8 File Offset: 0x003DCBC8
	// (set) Token: 0x0600A642 RID: 42562 RVA: 0x003DE9D8 File Offset: 0x003DCBD8
	public bool Disposed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Thread.VolatileRead(ref this.m_Disposed) == 1;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			Thread.VolatileWrite(ref this.m_Disposed, value ? 1 : 0);
		}
	}

	// Token: 0x0600A643 RID: 42563 RVA: 0x003DE9EC File Offset: 0x003DCBEC
	public TList()
	{
		this.m_TList = new List<T>();
	}

	// Token: 0x0600A644 RID: 42564 RVA: 0x003DEA0A File Offset: 0x003DCC0A
	public TList(int capacity)
	{
		this.m_TList = new List<T>(capacity);
	}

	// Token: 0x0600A645 RID: 42565 RVA: 0x003DEA29 File Offset: 0x003DCC29
	public TList(IEnumerable<T> collection)
	{
		this.m_TList = new List<T>(collection);
	}

	// Token: 0x0600A646 RID: 42566 RVA: 0x003DEA48 File Offset: 0x003DCC48
	public IEnumerator GetEnumerator()
	{
		this.LockList.EnterReadLock();
		List<T> list;
		try
		{
			list = new List<T>(this.m_TList);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		foreach (T t in list)
		{
			yield return t;
		}
		List<T>.Enumerator enumerator = default(List<T>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600A647 RID: 42567 RVA: 0x003DEA57 File Offset: 0x003DCC57
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		this.LockList.EnterReadLock();
		List<T> list;
		try
		{
			list = new List<T>(this.m_TList);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		foreach (T t in list)
		{
			yield return t;
		}
		List<T>.Enumerator enumerator = default(List<T>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600A648 RID: 42568 RVA: 0x003DEA66 File Offset: 0x003DCC66
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x0600A649 RID: 42569 RVA: 0x003DEA75 File Offset: 0x003DCC75
	[PublicizedFrom(EAccessModifier.Private)]
	public void Dispose(bool disposing)
	{
		if (this.Disposed)
		{
			return;
		}
		this.Disposed = true;
	}

	// Token: 0x0600A64A RID: 42570 RVA: 0x003DEA8C File Offset: 0x003DCC8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~TList()
	{
		this.Dispose(false);
	}

	// Token: 0x0600A64B RID: 42571 RVA: 0x003DEABC File Offset: 0x003DCCBC
	public void Add(T item)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.Add(item);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A64C RID: 42572 RVA: 0x003DEB00 File Offset: 0x003DCD00
	public void AddRange(IEnumerable<T> collection)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.AddRange(collection);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A64D RID: 42573 RVA: 0x003DEB44 File Offset: 0x003DCD44
	public bool AddIfNotExist(T item)
	{
		this.LockList.EnterWriteLock();
		bool flag;
		try
		{
			if (this.m_TList.Contains(item))
			{
				flag = false;
			}
			else
			{
				this.m_TList.Add(item);
				flag = true;
			}
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
		return flag;
	}

	// Token: 0x0600A64E RID: 42574 RVA: 0x003DEB9C File Offset: 0x003DCD9C
	public ReadOnlyCollection<T> AsReadOnly()
	{
		this.LockList.EnterReadLock();
		ReadOnlyCollection<T> readOnlyCollection;
		try
		{
			readOnlyCollection = this.m_TList.AsReadOnly();
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return readOnlyCollection;
	}

	// Token: 0x0600A64F RID: 42575 RVA: 0x003DEBE0 File Offset: 0x003DCDE0
	public int BinarySearch(T item)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.BinarySearch(item);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A650 RID: 42576 RVA: 0x003DEC24 File Offset: 0x003DCE24
	public int BinarySearch(T item, IComparer<T> comparer)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.BinarySearch(item, comparer);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A651 RID: 42577 RVA: 0x003DEC6C File Offset: 0x003DCE6C
	public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.BinarySearch(index, count, item, comparer);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x17001360 RID: 4960
	// (get) Token: 0x0600A652 RID: 42578 RVA: 0x003DECB4 File Offset: 0x003DCEB4
	// (set) Token: 0x0600A653 RID: 42579 RVA: 0x003DECF8 File Offset: 0x003DCEF8
	public int Capacity
	{
		get
		{
			this.LockList.EnterReadLock();
			int capacity;
			try
			{
				capacity = this.m_TList.Capacity;
			}
			finally
			{
				this.LockList.ExitReadLock();
			}
			return capacity;
		}
		set
		{
			this.LockList.EnterWriteLock();
			try
			{
				this.m_TList.Capacity = value;
			}
			finally
			{
				this.LockList.ExitWriteLock();
			}
		}
	}

	// Token: 0x0600A654 RID: 42580 RVA: 0x003DED3C File Offset: 0x003DCF3C
	public void Clear()
	{
		this.LockList.EnterReadLock();
		try
		{
			this.m_TList.Clear();
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
	}

	// Token: 0x0600A655 RID: 42581 RVA: 0x003DED80 File Offset: 0x003DCF80
	public bool Contains(T item)
	{
		this.LockList.EnterReadLock();
		bool flag;
		try
		{
			flag = this.m_TList.Contains(item);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return flag;
	}

	// Token: 0x0600A656 RID: 42582 RVA: 0x003DEDC4 File Offset: 0x003DCFC4
	public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
	{
		this.LockList.EnterReadLock();
		List<TOutput> list;
		try
		{
			list = this.m_TList.ConvertAll<TOutput>(converter);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return list;
	}

	// Token: 0x0600A657 RID: 42583 RVA: 0x003DEE08 File Offset: 0x003DD008
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.LockList.EnterReadLock();
		try
		{
			this.m_TList.CopyTo(array, arrayIndex);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
	}

	// Token: 0x17001361 RID: 4961
	// (get) Token: 0x0600A658 RID: 42584 RVA: 0x003DEE4C File Offset: 0x003DD04C
	public int Count
	{
		get
		{
			this.LockList.EnterReadLock();
			int count;
			try
			{
				count = this.m_TList.Count;
			}
			finally
			{
				this.LockList.ExitReadLock();
			}
			return count;
		}
	}

	// Token: 0x0600A659 RID: 42585 RVA: 0x003DEE90 File Offset: 0x003DD090
	public bool Exists(Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		bool flag;
		try
		{
			flag = this.m_TList.Exists(match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return flag;
	}

	// Token: 0x0600A65A RID: 42586 RVA: 0x003DEED4 File Offset: 0x003DD0D4
	public T Find(Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		T t;
		try
		{
			t = this.m_TList.Find(match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return t;
	}

	// Token: 0x0600A65B RID: 42587 RVA: 0x003DEF18 File Offset: 0x003DD118
	public List<T> FindAll(Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		List<T> list;
		try
		{
			list = this.m_TList.FindAll(match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return list;
	}

	// Token: 0x0600A65C RID: 42588 RVA: 0x003DEF5C File Offset: 0x003DD15C
	public int FindIndex(Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.FindIndex(match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A65D RID: 42589 RVA: 0x003DEFA0 File Offset: 0x003DD1A0
	public int FindIndex(int startIndex, Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.FindIndex(startIndex, match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A65E RID: 42590 RVA: 0x003DEFE8 File Offset: 0x003DD1E8
	public int FindIndex(int startIndex, int count, Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.FindIndex(startIndex, count, match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A65F RID: 42591 RVA: 0x003DF030 File Offset: 0x003DD230
	public T FindLast(Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		T t;
		try
		{
			t = this.m_TList.FindLast(match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return t;
	}

	// Token: 0x0600A660 RID: 42592 RVA: 0x003DF074 File Offset: 0x003DD274
	public int FindLastIndex(Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.FindLastIndex(match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A661 RID: 42593 RVA: 0x003DF0B8 File Offset: 0x003DD2B8
	public int FindLastIndex(int startIndex, Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.FindLastIndex(startIndex, match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A662 RID: 42594 RVA: 0x003DF100 File Offset: 0x003DD300
	public int FindLastIndex(int startIndex, int count, Predicate<T> match)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.FindLastIndex(startIndex, count, match);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A663 RID: 42595 RVA: 0x003DF148 File Offset: 0x003DD348
	public void ForEach(Action<T> action)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.ForEach(action);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A664 RID: 42596 RVA: 0x003DF18C File Offset: 0x003DD38C
	public List<T> GetRange(int index, int count)
	{
		this.LockList.EnterReadLock();
		List<T> range;
		try
		{
			range = this.m_TList.GetRange(index, count);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return range;
	}

	// Token: 0x0600A665 RID: 42597 RVA: 0x003DF1D4 File Offset: 0x003DD3D4
	public int IndexOf(T item)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.IndexOf(item);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A666 RID: 42598 RVA: 0x003DF218 File Offset: 0x003DD418
	public int IndexOf(T item, int index)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.IndexOf(item, index);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A667 RID: 42599 RVA: 0x003DF260 File Offset: 0x003DD460
	public int IndexOf(T item, int index, int count)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.IndexOf(item, index, count);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A668 RID: 42600 RVA: 0x003DF2A8 File Offset: 0x003DD4A8
	public void Insert(int index, T item)
	{
		this.LockList.ExitWriteLock();
		try
		{
			this.m_TList.Insert(index, item);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A669 RID: 42601 RVA: 0x003DF2EC File Offset: 0x003DD4EC
	public void InsertRange(int index, IEnumerable<T> range)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.InsertRange(index, range);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x17001362 RID: 4962
	// (get) Token: 0x0600A66A RID: 42602 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A66B RID: 42603 RVA: 0x003DF330 File Offset: 0x003DD530
	public int LastIndexOf(T item)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.LastIndexOf(item);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A66C RID: 42604 RVA: 0x003DF374 File Offset: 0x003DD574
	public int LastIndexOf(T item, int index)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.LastIndexOf(item, index);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A66D RID: 42605 RVA: 0x003DF3BC File Offset: 0x003DD5BC
	public int LastIndexOf(T item, int index, int count)
	{
		this.LockList.EnterReadLock();
		int num;
		try
		{
			num = this.m_TList.LastIndexOf(item, index, count);
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return num;
	}

	// Token: 0x0600A66E RID: 42606 RVA: 0x003DF404 File Offset: 0x003DD604
	public bool Remove(T item)
	{
		this.LockList.EnterWriteLock();
		bool flag;
		try
		{
			flag = this.m_TList.Remove(item);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
		return flag;
	}

	// Token: 0x0600A66F RID: 42607 RVA: 0x003DF448 File Offset: 0x003DD648
	public int RemoveAll(Predicate<T> match)
	{
		this.LockList.EnterWriteLock();
		int num;
		try
		{
			num = this.m_TList.RemoveAll(match);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
		return num;
	}

	// Token: 0x0600A670 RID: 42608 RVA: 0x003DF48C File Offset: 0x003DD68C
	public void RemoveAt(int index)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.RemoveAt(index);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A671 RID: 42609 RVA: 0x003DF4D0 File Offset: 0x003DD6D0
	public void RemoveRange(int index, int count)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.RemoveRange(index, count);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A672 RID: 42610 RVA: 0x003DF514 File Offset: 0x003DD714
	public void Reverse()
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.Reverse();
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A673 RID: 42611 RVA: 0x003DF558 File Offset: 0x003DD758
	public void Reverse(int index, int count)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.Reverse(index, count);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A674 RID: 42612 RVA: 0x003DF59C File Offset: 0x003DD79C
	public void Sort()
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.Sort();
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A675 RID: 42613 RVA: 0x003DF5E0 File Offset: 0x003DD7E0
	public void Sort(Comparison<T> comparison)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.Sort(comparison);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A676 RID: 42614 RVA: 0x003DF624 File Offset: 0x003DD824
	public void Sort(IComparer<T> comparer)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.Sort(comparer);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A677 RID: 42615 RVA: 0x003DF668 File Offset: 0x003DD868
	public void Sort(int index, int count, IComparer<T> comparer)
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.Sort(index, count, comparer);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A678 RID: 42616 RVA: 0x003DF6AC File Offset: 0x003DD8AC
	public T[] ToArray()
	{
		this.LockList.EnterReadLock();
		T[] array;
		try
		{
			array = this.m_TList.ToArray();
		}
		finally
		{
			this.LockList.ExitReadLock();
		}
		return array;
	}

	// Token: 0x0600A679 RID: 42617 RVA: 0x003DF6F0 File Offset: 0x003DD8F0
	public void TrimExcess()
	{
		this.LockList.EnterWriteLock();
		try
		{
			this.m_TList.TrimExcess();
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
	}

	// Token: 0x0600A67A RID: 42618 RVA: 0x003DF734 File Offset: 0x003DD934
	public bool TrueForAll(Predicate<T> match)
	{
		this.LockList.EnterWriteLock();
		bool flag;
		try
		{
			flag = this.m_TList.TrueForAll(match);
		}
		finally
		{
			this.LockList.ExitWriteLock();
		}
		return flag;
	}

	// Token: 0x04007B4E RID: 31566
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<T> m_TList;

	// Token: 0x04007B4F RID: 31567
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim LockList = new ReaderWriterLockSlim();

	// Token: 0x04007B50 RID: 31568
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_Disposed;
}
