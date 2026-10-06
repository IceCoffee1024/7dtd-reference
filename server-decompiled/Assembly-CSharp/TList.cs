using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

// Token: 0x020014F2 RID: 5362
public class TList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x170013DD RID: 5085
	// (get) Token: 0x0600A7F1 RID: 42993 RVA: 0x003E8B5C File Offset: 0x003E6D5C
	// (set) Token: 0x0600A7F2 RID: 42994 RVA: 0x003E8B6C File Offset: 0x003E6D6C
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

	// Token: 0x0600A7F3 RID: 42995 RVA: 0x003E8B80 File Offset: 0x003E6D80
	public TList()
	{
		this.m_TList = new List<T>();
	}

	// Token: 0x0600A7F4 RID: 42996 RVA: 0x003E8B9E File Offset: 0x003E6D9E
	public TList(int capacity)
	{
		this.m_TList = new List<T>(capacity);
	}

	// Token: 0x0600A7F5 RID: 42997 RVA: 0x003E8BBD File Offset: 0x003E6DBD
	public TList(IEnumerable<T> collection)
	{
		this.m_TList = new List<T>(collection);
	}

	// Token: 0x0600A7F6 RID: 42998 RVA: 0x003E8BDC File Offset: 0x003E6DDC
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

	// Token: 0x0600A7F7 RID: 42999 RVA: 0x003E8BEB File Offset: 0x003E6DEB
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

	// Token: 0x0600A7F8 RID: 43000 RVA: 0x003E8BFA File Offset: 0x003E6DFA
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x0600A7F9 RID: 43001 RVA: 0x003E8C09 File Offset: 0x003E6E09
	[PublicizedFrom(EAccessModifier.Private)]
	public void Dispose(bool disposing)
	{
		if (this.Disposed)
		{
			return;
		}
		this.Disposed = true;
	}

	// Token: 0x0600A7FA RID: 43002 RVA: 0x003E8C20 File Offset: 0x003E6E20
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~TList()
	{
		this.Dispose(false);
	}

	// Token: 0x0600A7FB RID: 43003 RVA: 0x003E8C50 File Offset: 0x003E6E50
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

	// Token: 0x0600A7FC RID: 43004 RVA: 0x003E8C94 File Offset: 0x003E6E94
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

	// Token: 0x0600A7FD RID: 43005 RVA: 0x003E8CD8 File Offset: 0x003E6ED8
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

	// Token: 0x0600A7FE RID: 43006 RVA: 0x003E8D30 File Offset: 0x003E6F30
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

	// Token: 0x0600A7FF RID: 43007 RVA: 0x003E8D74 File Offset: 0x003E6F74
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

	// Token: 0x0600A800 RID: 43008 RVA: 0x003E8DB8 File Offset: 0x003E6FB8
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

	// Token: 0x0600A801 RID: 43009 RVA: 0x003E8E00 File Offset: 0x003E7000
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

	// Token: 0x170013DE RID: 5086
	// (get) Token: 0x0600A802 RID: 43010 RVA: 0x003E8E48 File Offset: 0x003E7048
	// (set) Token: 0x0600A803 RID: 43011 RVA: 0x003E8E8C File Offset: 0x003E708C
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

	// Token: 0x0600A804 RID: 43012 RVA: 0x003E8ED0 File Offset: 0x003E70D0
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

	// Token: 0x0600A805 RID: 43013 RVA: 0x003E8F14 File Offset: 0x003E7114
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

	// Token: 0x0600A806 RID: 43014 RVA: 0x003E8F58 File Offset: 0x003E7158
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

	// Token: 0x0600A807 RID: 43015 RVA: 0x003E8F9C File Offset: 0x003E719C
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

	// Token: 0x170013DF RID: 5087
	// (get) Token: 0x0600A808 RID: 43016 RVA: 0x003E8FE0 File Offset: 0x003E71E0
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

	// Token: 0x0600A809 RID: 43017 RVA: 0x003E9024 File Offset: 0x003E7224
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

	// Token: 0x0600A80A RID: 43018 RVA: 0x003E9068 File Offset: 0x003E7268
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

	// Token: 0x0600A80B RID: 43019 RVA: 0x003E90AC File Offset: 0x003E72AC
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

	// Token: 0x0600A80C RID: 43020 RVA: 0x003E90F0 File Offset: 0x003E72F0
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

	// Token: 0x0600A80D RID: 43021 RVA: 0x003E9134 File Offset: 0x003E7334
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

	// Token: 0x0600A80E RID: 43022 RVA: 0x003E917C File Offset: 0x003E737C
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

	// Token: 0x0600A80F RID: 43023 RVA: 0x003E91C4 File Offset: 0x003E73C4
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

	// Token: 0x0600A810 RID: 43024 RVA: 0x003E9208 File Offset: 0x003E7408
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

	// Token: 0x0600A811 RID: 43025 RVA: 0x003E924C File Offset: 0x003E744C
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

	// Token: 0x0600A812 RID: 43026 RVA: 0x003E9294 File Offset: 0x003E7494
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

	// Token: 0x0600A813 RID: 43027 RVA: 0x003E92DC File Offset: 0x003E74DC
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

	// Token: 0x0600A814 RID: 43028 RVA: 0x003E9320 File Offset: 0x003E7520
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

	// Token: 0x0600A815 RID: 43029 RVA: 0x003E9368 File Offset: 0x003E7568
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

	// Token: 0x0600A816 RID: 43030 RVA: 0x003E93AC File Offset: 0x003E75AC
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

	// Token: 0x0600A817 RID: 43031 RVA: 0x003E93F4 File Offset: 0x003E75F4
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

	// Token: 0x0600A818 RID: 43032 RVA: 0x003E943C File Offset: 0x003E763C
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

	// Token: 0x0600A819 RID: 43033 RVA: 0x003E9480 File Offset: 0x003E7680
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

	// Token: 0x170013E0 RID: 5088
	// (get) Token: 0x0600A81A RID: 43034 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A81B RID: 43035 RVA: 0x003E94C4 File Offset: 0x003E76C4
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

	// Token: 0x0600A81C RID: 43036 RVA: 0x003E9508 File Offset: 0x003E7708
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

	// Token: 0x0600A81D RID: 43037 RVA: 0x003E9550 File Offset: 0x003E7750
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

	// Token: 0x0600A81E RID: 43038 RVA: 0x003E9598 File Offset: 0x003E7798
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

	// Token: 0x0600A81F RID: 43039 RVA: 0x003E95DC File Offset: 0x003E77DC
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

	// Token: 0x0600A820 RID: 43040 RVA: 0x003E9620 File Offset: 0x003E7820
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

	// Token: 0x0600A821 RID: 43041 RVA: 0x003E9664 File Offset: 0x003E7864
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

	// Token: 0x0600A822 RID: 43042 RVA: 0x003E96A8 File Offset: 0x003E78A8
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

	// Token: 0x0600A823 RID: 43043 RVA: 0x003E96EC File Offset: 0x003E78EC
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

	// Token: 0x0600A824 RID: 43044 RVA: 0x003E9730 File Offset: 0x003E7930
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

	// Token: 0x0600A825 RID: 43045 RVA: 0x003E9774 File Offset: 0x003E7974
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

	// Token: 0x0600A826 RID: 43046 RVA: 0x003E97B8 File Offset: 0x003E79B8
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

	// Token: 0x0600A827 RID: 43047 RVA: 0x003E97FC File Offset: 0x003E79FC
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

	// Token: 0x0600A828 RID: 43048 RVA: 0x003E9840 File Offset: 0x003E7A40
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

	// Token: 0x0600A829 RID: 43049 RVA: 0x003E9884 File Offset: 0x003E7A84
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

	// Token: 0x0600A82A RID: 43050 RVA: 0x003E98C8 File Offset: 0x003E7AC8
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

	// Token: 0x04007CAA RID: 31914
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<T> m_TList;

	// Token: 0x04007CAB RID: 31915
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim LockList = new ReaderWriterLockSlim();

	// Token: 0x04007CAC RID: 31916
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_Disposed;
}
