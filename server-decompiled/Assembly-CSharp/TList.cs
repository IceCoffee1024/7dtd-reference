using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

// Token: 0x020014D0 RID: 5328
public class TList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x17001398 RID: 5016
	// (get) Token: 0x0600A73E RID: 42814 RVA: 0x003E3D08 File Offset: 0x003E1F08
	// (set) Token: 0x0600A73F RID: 42815 RVA: 0x003E3D18 File Offset: 0x003E1F18
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

	// Token: 0x0600A740 RID: 42816 RVA: 0x003E3D2C File Offset: 0x003E1F2C
	public TList()
	{
		this.m_TList = new List<T>();
	}

	// Token: 0x0600A741 RID: 42817 RVA: 0x003E3D4A File Offset: 0x003E1F4A
	public TList(int capacity)
	{
		this.m_TList = new List<T>(capacity);
	}

	// Token: 0x0600A742 RID: 42818 RVA: 0x003E3D69 File Offset: 0x003E1F69
	public TList(IEnumerable<T> collection)
	{
		this.m_TList = new List<T>(collection);
	}

	// Token: 0x0600A743 RID: 42819 RVA: 0x003E3D88 File Offset: 0x003E1F88
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

	// Token: 0x0600A744 RID: 42820 RVA: 0x003E3D97 File Offset: 0x003E1F97
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

	// Token: 0x0600A745 RID: 42821 RVA: 0x003E3DA6 File Offset: 0x003E1FA6
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x0600A746 RID: 42822 RVA: 0x003E3DB5 File Offset: 0x003E1FB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Dispose(bool disposing)
	{
		if (this.Disposed)
		{
			return;
		}
		this.Disposed = true;
	}

	// Token: 0x0600A747 RID: 42823 RVA: 0x003E3DCC File Offset: 0x003E1FCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~TList()
	{
		this.Dispose(false);
	}

	// Token: 0x0600A748 RID: 42824 RVA: 0x003E3DFC File Offset: 0x003E1FFC
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

	// Token: 0x0600A749 RID: 42825 RVA: 0x003E3E40 File Offset: 0x003E2040
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

	// Token: 0x0600A74A RID: 42826 RVA: 0x003E3E84 File Offset: 0x003E2084
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

	// Token: 0x0600A74B RID: 42827 RVA: 0x003E3EDC File Offset: 0x003E20DC
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

	// Token: 0x0600A74C RID: 42828 RVA: 0x003E3F20 File Offset: 0x003E2120
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

	// Token: 0x0600A74D RID: 42829 RVA: 0x003E3F64 File Offset: 0x003E2164
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

	// Token: 0x0600A74E RID: 42830 RVA: 0x003E3FAC File Offset: 0x003E21AC
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

	// Token: 0x17001399 RID: 5017
	// (get) Token: 0x0600A74F RID: 42831 RVA: 0x003E3FF4 File Offset: 0x003E21F4
	// (set) Token: 0x0600A750 RID: 42832 RVA: 0x003E4038 File Offset: 0x003E2238
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

	// Token: 0x0600A751 RID: 42833 RVA: 0x003E407C File Offset: 0x003E227C
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

	// Token: 0x0600A752 RID: 42834 RVA: 0x003E40C0 File Offset: 0x003E22C0
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

	// Token: 0x0600A753 RID: 42835 RVA: 0x003E4104 File Offset: 0x003E2304
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

	// Token: 0x0600A754 RID: 42836 RVA: 0x003E4148 File Offset: 0x003E2348
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

	// Token: 0x1700139A RID: 5018
	// (get) Token: 0x0600A755 RID: 42837 RVA: 0x003E418C File Offset: 0x003E238C
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

	// Token: 0x0600A756 RID: 42838 RVA: 0x003E41D0 File Offset: 0x003E23D0
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

	// Token: 0x0600A757 RID: 42839 RVA: 0x003E4214 File Offset: 0x003E2414
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

	// Token: 0x0600A758 RID: 42840 RVA: 0x003E4258 File Offset: 0x003E2458
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

	// Token: 0x0600A759 RID: 42841 RVA: 0x003E429C File Offset: 0x003E249C
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

	// Token: 0x0600A75A RID: 42842 RVA: 0x003E42E0 File Offset: 0x003E24E0
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

	// Token: 0x0600A75B RID: 42843 RVA: 0x003E4328 File Offset: 0x003E2528
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

	// Token: 0x0600A75C RID: 42844 RVA: 0x003E4370 File Offset: 0x003E2570
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

	// Token: 0x0600A75D RID: 42845 RVA: 0x003E43B4 File Offset: 0x003E25B4
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

	// Token: 0x0600A75E RID: 42846 RVA: 0x003E43F8 File Offset: 0x003E25F8
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

	// Token: 0x0600A75F RID: 42847 RVA: 0x003E4440 File Offset: 0x003E2640
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

	// Token: 0x0600A760 RID: 42848 RVA: 0x003E4488 File Offset: 0x003E2688
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

	// Token: 0x0600A761 RID: 42849 RVA: 0x003E44CC File Offset: 0x003E26CC
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

	// Token: 0x0600A762 RID: 42850 RVA: 0x003E4514 File Offset: 0x003E2714
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

	// Token: 0x0600A763 RID: 42851 RVA: 0x003E4558 File Offset: 0x003E2758
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

	// Token: 0x0600A764 RID: 42852 RVA: 0x003E45A0 File Offset: 0x003E27A0
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

	// Token: 0x0600A765 RID: 42853 RVA: 0x003E45E8 File Offset: 0x003E27E8
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

	// Token: 0x0600A766 RID: 42854 RVA: 0x003E462C File Offset: 0x003E282C
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

	// Token: 0x1700139B RID: 5019
	// (get) Token: 0x0600A767 RID: 42855 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A768 RID: 42856 RVA: 0x003E4670 File Offset: 0x003E2870
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

	// Token: 0x0600A769 RID: 42857 RVA: 0x003E46B4 File Offset: 0x003E28B4
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

	// Token: 0x0600A76A RID: 42858 RVA: 0x003E46FC File Offset: 0x003E28FC
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

	// Token: 0x0600A76B RID: 42859 RVA: 0x003E4744 File Offset: 0x003E2944
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

	// Token: 0x0600A76C RID: 42860 RVA: 0x003E4788 File Offset: 0x003E2988
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

	// Token: 0x0600A76D RID: 42861 RVA: 0x003E47CC File Offset: 0x003E29CC
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

	// Token: 0x0600A76E RID: 42862 RVA: 0x003E4810 File Offset: 0x003E2A10
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

	// Token: 0x0600A76F RID: 42863 RVA: 0x003E4854 File Offset: 0x003E2A54
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

	// Token: 0x0600A770 RID: 42864 RVA: 0x003E4898 File Offset: 0x003E2A98
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

	// Token: 0x0600A771 RID: 42865 RVA: 0x003E48DC File Offset: 0x003E2ADC
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

	// Token: 0x0600A772 RID: 42866 RVA: 0x003E4920 File Offset: 0x003E2B20
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

	// Token: 0x0600A773 RID: 42867 RVA: 0x003E4964 File Offset: 0x003E2B64
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

	// Token: 0x0600A774 RID: 42868 RVA: 0x003E49A8 File Offset: 0x003E2BA8
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

	// Token: 0x0600A775 RID: 42869 RVA: 0x003E49EC File Offset: 0x003E2BEC
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

	// Token: 0x0600A776 RID: 42870 RVA: 0x003E4A30 File Offset: 0x003E2C30
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

	// Token: 0x0600A777 RID: 42871 RVA: 0x003E4A74 File Offset: 0x003E2C74
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

	// Token: 0x04007C27 RID: 31783
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<T> m_TList;

	// Token: 0x04007C28 RID: 31784
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim LockList = new ReaderWriterLockSlim();

	// Token: 0x04007C29 RID: 31785
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_Disposed;
}
