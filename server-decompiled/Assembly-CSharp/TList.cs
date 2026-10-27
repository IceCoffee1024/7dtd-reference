using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

// Token: 0x02001504 RID: 5380
public class TList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x170013E9 RID: 5097
	// (get) Token: 0x0600A849 RID: 43081 RVA: 0x003EAF60 File Offset: 0x003E9160
	// (set) Token: 0x0600A84A RID: 43082 RVA: 0x003EAF70 File Offset: 0x003E9170
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

	// Token: 0x0600A84B RID: 43083 RVA: 0x003EAF84 File Offset: 0x003E9184
	public TList()
	{
		this.m_TList = new List<T>();
	}

	// Token: 0x0600A84C RID: 43084 RVA: 0x003EAFA2 File Offset: 0x003E91A2
	public TList(int capacity)
	{
		this.m_TList = new List<T>(capacity);
	}

	// Token: 0x0600A84D RID: 43085 RVA: 0x003EAFC1 File Offset: 0x003E91C1
	public TList(IEnumerable<T> collection)
	{
		this.m_TList = new List<T>(collection);
	}

	// Token: 0x0600A84E RID: 43086 RVA: 0x003EAFE0 File Offset: 0x003E91E0
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

	// Token: 0x0600A84F RID: 43087 RVA: 0x003EAFEF File Offset: 0x003E91EF
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

	// Token: 0x0600A850 RID: 43088 RVA: 0x003EAFFE File Offset: 0x003E91FE
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x0600A851 RID: 43089 RVA: 0x003EB00D File Offset: 0x003E920D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Dispose(bool disposing)
	{
		if (this.Disposed)
		{
			return;
		}
		this.Disposed = true;
	}

	// Token: 0x0600A852 RID: 43090 RVA: 0x003EB024 File Offset: 0x003E9224
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~TList()
	{
		this.Dispose(false);
	}

	// Token: 0x0600A853 RID: 43091 RVA: 0x003EB054 File Offset: 0x003E9254
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

	// Token: 0x0600A854 RID: 43092 RVA: 0x003EB098 File Offset: 0x003E9298
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

	// Token: 0x0600A855 RID: 43093 RVA: 0x003EB0DC File Offset: 0x003E92DC
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

	// Token: 0x0600A856 RID: 43094 RVA: 0x003EB134 File Offset: 0x003E9334
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

	// Token: 0x0600A857 RID: 43095 RVA: 0x003EB178 File Offset: 0x003E9378
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

	// Token: 0x0600A858 RID: 43096 RVA: 0x003EB1BC File Offset: 0x003E93BC
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

	// Token: 0x0600A859 RID: 43097 RVA: 0x003EB204 File Offset: 0x003E9404
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

	// Token: 0x170013EA RID: 5098
	// (get) Token: 0x0600A85A RID: 43098 RVA: 0x003EB24C File Offset: 0x003E944C
	// (set) Token: 0x0600A85B RID: 43099 RVA: 0x003EB290 File Offset: 0x003E9490
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

	// Token: 0x0600A85C RID: 43100 RVA: 0x003EB2D4 File Offset: 0x003E94D4
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

	// Token: 0x0600A85D RID: 43101 RVA: 0x003EB318 File Offset: 0x003E9518
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

	// Token: 0x0600A85E RID: 43102 RVA: 0x003EB35C File Offset: 0x003E955C
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

	// Token: 0x0600A85F RID: 43103 RVA: 0x003EB3A0 File Offset: 0x003E95A0
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

	// Token: 0x170013EB RID: 5099
	// (get) Token: 0x0600A860 RID: 43104 RVA: 0x003EB3E4 File Offset: 0x003E95E4
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

	// Token: 0x0600A861 RID: 43105 RVA: 0x003EB428 File Offset: 0x003E9628
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

	// Token: 0x0600A862 RID: 43106 RVA: 0x003EB46C File Offset: 0x003E966C
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

	// Token: 0x0600A863 RID: 43107 RVA: 0x003EB4B0 File Offset: 0x003E96B0
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

	// Token: 0x0600A864 RID: 43108 RVA: 0x003EB4F4 File Offset: 0x003E96F4
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

	// Token: 0x0600A865 RID: 43109 RVA: 0x003EB538 File Offset: 0x003E9738
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

	// Token: 0x0600A866 RID: 43110 RVA: 0x003EB580 File Offset: 0x003E9780
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

	// Token: 0x0600A867 RID: 43111 RVA: 0x003EB5C8 File Offset: 0x003E97C8
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

	// Token: 0x0600A868 RID: 43112 RVA: 0x003EB60C File Offset: 0x003E980C
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

	// Token: 0x0600A869 RID: 43113 RVA: 0x003EB650 File Offset: 0x003E9850
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

	// Token: 0x0600A86A RID: 43114 RVA: 0x003EB698 File Offset: 0x003E9898
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

	// Token: 0x0600A86B RID: 43115 RVA: 0x003EB6E0 File Offset: 0x003E98E0
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

	// Token: 0x0600A86C RID: 43116 RVA: 0x003EB724 File Offset: 0x003E9924
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

	// Token: 0x0600A86D RID: 43117 RVA: 0x003EB76C File Offset: 0x003E996C
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

	// Token: 0x0600A86E RID: 43118 RVA: 0x003EB7B0 File Offset: 0x003E99B0
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

	// Token: 0x0600A86F RID: 43119 RVA: 0x003EB7F8 File Offset: 0x003E99F8
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

	// Token: 0x0600A870 RID: 43120 RVA: 0x003EB840 File Offset: 0x003E9A40
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

	// Token: 0x0600A871 RID: 43121 RVA: 0x003EB884 File Offset: 0x003E9A84
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

	// Token: 0x170013EC RID: 5100
	// (get) Token: 0x0600A872 RID: 43122 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A873 RID: 43123 RVA: 0x003EB8C8 File Offset: 0x003E9AC8
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

	// Token: 0x0600A874 RID: 43124 RVA: 0x003EB90C File Offset: 0x003E9B0C
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

	// Token: 0x0600A875 RID: 43125 RVA: 0x003EB954 File Offset: 0x003E9B54
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

	// Token: 0x0600A876 RID: 43126 RVA: 0x003EB99C File Offset: 0x003E9B9C
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

	// Token: 0x0600A877 RID: 43127 RVA: 0x003EB9E0 File Offset: 0x003E9BE0
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

	// Token: 0x0600A878 RID: 43128 RVA: 0x003EBA24 File Offset: 0x003E9C24
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

	// Token: 0x0600A879 RID: 43129 RVA: 0x003EBA68 File Offset: 0x003E9C68
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

	// Token: 0x0600A87A RID: 43130 RVA: 0x003EBAAC File Offset: 0x003E9CAC
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

	// Token: 0x0600A87B RID: 43131 RVA: 0x003EBAF0 File Offset: 0x003E9CF0
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

	// Token: 0x0600A87C RID: 43132 RVA: 0x003EBB34 File Offset: 0x003E9D34
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

	// Token: 0x0600A87D RID: 43133 RVA: 0x003EBB78 File Offset: 0x003E9D78
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

	// Token: 0x0600A87E RID: 43134 RVA: 0x003EBBBC File Offset: 0x003E9DBC
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

	// Token: 0x0600A87F RID: 43135 RVA: 0x003EBC00 File Offset: 0x003E9E00
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

	// Token: 0x0600A880 RID: 43136 RVA: 0x003EBC44 File Offset: 0x003E9E44
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

	// Token: 0x0600A881 RID: 43137 RVA: 0x003EBC88 File Offset: 0x003E9E88
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

	// Token: 0x0600A882 RID: 43138 RVA: 0x003EBCCC File Offset: 0x003E9ECC
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

	// Token: 0x04007CEB RID: 31979
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<T> m_TList;

	// Token: 0x04007CEC RID: 31980
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim LockList = new ReaderWriterLockSlim();

	// Token: 0x04007CED RID: 31981
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_Disposed;
}
