using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConcurrentCollections
{
	// Token: 0x02001AA7 RID: 6823
	[DebuggerDisplay("Count = {Count}")]
	public class ConcurrentHashSet<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable, ICollection<T>
	{
		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x0600CDD4 RID: 52692 RVA: 0x004ACF1B File Offset: 0x004AB11B
		public static int DefaultConcurrencyLevel
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return Math.Max(2, Environment.ProcessorCount);
			}
		}

		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x0600CDD5 RID: 52693 RVA: 0x004ACF28 File Offset: 0x004AB128
		public int Count
		{
			get
			{
				int num = 0;
				int num2 = 0;
				try
				{
					this.AcquireAllLocks(ref num2);
					for (int i = 0; i < this._tables.CountPerLock.Length; i++)
					{
						num += this._tables.CountPerLock[i];
					}
				}
				finally
				{
					this.ReleaseLocks(0, num2);
				}
				return num;
			}
		}

		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x0600CDD6 RID: 52694 RVA: 0x004ACF90 File Offset: 0x004AB190
		public bool IsEmpty
		{
			get
			{
				int num = 0;
				try
				{
					this.AcquireAllLocks(ref num);
					for (int i = 0; i < this._tables.CountPerLock.Length; i++)
					{
						if (this._tables.CountPerLock[i] != 0)
						{
							return false;
						}
					}
				}
				finally
				{
					this.ReleaseLocks(0, num);
				}
				return true;
			}
		}

		// Token: 0x0600CDD7 RID: 52695 RVA: 0x004ACFF8 File Offset: 0x004AB1F8
		public ConcurrentHashSet()
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, null)
		{
		}

		// Token: 0x0600CDD8 RID: 52696 RVA: 0x004AD009 File Offset: 0x004AB209
		public ConcurrentHashSet(Action<T> onRemovalFaiuire)
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, null)
		{
			this.OnRemovalFailure = onRemovalFaiuire;
		}

		// Token: 0x0600CDD9 RID: 52697 RVA: 0x004AD021 File Offset: 0x004AB221
		public ConcurrentHashSet(int concurrencyLevel, int capacity)
			: this(concurrencyLevel, capacity, false, null)
		{
		}

		// Token: 0x0600CDDA RID: 52698 RVA: 0x004AD02D File Offset: 0x004AB22D
		public ConcurrentHashSet(IEnumerable<T> collection)
			: this(collection, null)
		{
		}

		// Token: 0x0600CDDB RID: 52699 RVA: 0x004AD037 File Offset: 0x004AB237
		public ConcurrentHashSet(IEqualityComparer<T> comparer)
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, comparer)
		{
		}

		// Token: 0x0600CDDC RID: 52700 RVA: 0x004AD048 File Offset: 0x004AB248
		public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
			: this(comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x0600CDDD RID: 52701 RVA: 0x004AD066 File Offset: 0x004AB266
		public ConcurrentHashSet(int concurrencyLevel, IEnumerable<T> collection, IEqualityComparer<T> comparer)
			: this(concurrencyLevel, 31, false, comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x0600CDDE RID: 52702 RVA: 0x004AD088 File Offset: 0x004AB288
		public ConcurrentHashSet(int concurrencyLevel, int capacity, IEqualityComparer<T> comparer)
			: this(concurrencyLevel, capacity, false, comparer)
		{
		}

		// Token: 0x0600CDDF RID: 52703 RVA: 0x004AD094 File Offset: 0x004AB294
		[PublicizedFrom(EAccessModifier.Private)]
		public ConcurrentHashSet(int concurrencyLevel, int capacity, bool growLockArray, IEqualityComparer<T> comparer)
		{
			if (concurrencyLevel < 1)
			{
				throw new ArgumentOutOfRangeException("concurrencyLevel");
			}
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			if (capacity < concurrencyLevel)
			{
				capacity = concurrencyLevel;
			}
			object[] array = new object[concurrencyLevel];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new object();
			}
			int[] array2 = new int[array.Length];
			ConcurrentHashSet<T>.Node[] array3 = new ConcurrentHashSet<T>.Node[capacity];
			this._tables = new ConcurrentHashSet<T>.Tables(array3, array, array2);
			this._growLockArray = growLockArray;
			this._budget = array3.Length / array.Length;
			this._comparer = comparer ?? EqualityComparer<T>.Default;
		}

		// Token: 0x0600CDE0 RID: 52704 RVA: 0x004AD12E File Offset: 0x004AB32E
		public bool Add(T item)
		{
			return this.AddInternal(item, this._comparer.GetHashCode(item), true);
		}

		// Token: 0x0600CDE1 RID: 52705 RVA: 0x004AD144 File Offset: 0x004AB344
		public void Clear()
		{
			int num = 0;
			try
			{
				this.AcquireAllLocks(ref num);
				ConcurrentHashSet<T>.Tables tables = new ConcurrentHashSet<T>.Tables(new ConcurrentHashSet<T>.Node[31], this._tables.Locks, new int[this._tables.CountPerLock.Length]);
				this._tables = tables;
				this._budget = Math.Max(1, tables.Buckets.Length / tables.Locks.Length);
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		// Token: 0x0600CDE2 RID: 52706 RVA: 0x004AD1CC File Offset: 0x004AB3CC
		public bool Contains(T item)
		{
			int hashCode = this._comparer.GetHashCode(item);
			ConcurrentHashSet<T>.Tables tables = this._tables;
			int bucket = ConcurrentHashSet<T>.GetBucket(hashCode, tables.Buckets.Length);
			for (ConcurrentHashSet<T>.Node node = Volatile.Read<ConcurrentHashSet<T>.Node>(ref tables.Buckets[bucket]); node != null; node = node.Next)
			{
				if (hashCode == node.Hashcode && this._comparer.Equals(node.Item, item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CDE3 RID: 52707 RVA: 0x004AD240 File Offset: 0x004AB440
		public bool TryRemove(T item)
		{
			int hashCode = this._comparer.GetHashCode(item);
			for (;;)
			{
				ConcurrentHashSet<T>.Tables tables = this._tables;
				int num;
				int num2;
				ConcurrentHashSet<T>.GetBucketAndLockNo(hashCode, out num, out num2, tables.Buckets.Length, tables.Locks.Length);
				object obj = tables.Locks[num2];
				lock (obj)
				{
					if (tables != this._tables)
					{
						continue;
					}
					ConcurrentHashSet<T>.Node node = null;
					for (ConcurrentHashSet<T>.Node node2 = tables.Buckets[num]; node2 != null; node2 = node2.Next)
					{
						if (hashCode == node2.Hashcode && this._comparer.Equals(node2.Item, item))
						{
							if (node == null)
							{
								Volatile.Write<ConcurrentHashSet<T>.Node>(ref tables.Buckets[num], node2.Next);
							}
							else
							{
								node.Next = node2.Next;
							}
							tables.CountPerLock[num2]--;
							return true;
						}
						node = node2;
					}
				}
				break;
			}
			Action<T> onRemovalFailure = this.OnRemovalFailure;
			if (onRemovalFailure != null)
			{
				onRemovalFailure(item);
			}
			return false;
		}

		// Token: 0x0600CDE4 RID: 52708 RVA: 0x004AD360 File Offset: 0x004AB560
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600CDE5 RID: 52709 RVA: 0x004AD368 File Offset: 0x004AB568
		public IEnumerator<T> GetEnumerator()
		{
			ConcurrentHashSet<T>.Node[] buckets = this._tables.Buckets;
			int num;
			for (int i = 0; i < buckets.Length; i = num + 1)
			{
				ConcurrentHashSet<T>.Node current;
				for (current = Volatile.Read<ConcurrentHashSet<T>.Node>(ref buckets[i]); current != null; current = current.Next)
				{
					yield return current.Item;
				}
				current = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600CDE6 RID: 52710 RVA: 0x004AD377 File Offset: 0x004AB577
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<T>.Add(T item)
		{
			this.Add(item);
		}

		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x0600CDE7 RID: 52711 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool System.Collections.Generic.ICollection<T>.IsReadOnly
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600CDE8 RID: 52712 RVA: 0x004AD384 File Offset: 0x004AB584
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			int num = 0;
			try
			{
				this.AcquireAllLocks(ref num);
				int num2 = 0;
				int num3 = 0;
				while (num3 < this._tables.Locks.Length && num2 >= 0)
				{
					num2 += this._tables.CountPerLock[num3];
					num3++;
				}
				if (array.Length - num2 < arrayIndex || num2 < 0)
				{
					throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the set is greater than the available space from index to the end of the destination array.");
				}
				this.CopyToItems(array, arrayIndex);
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		// Token: 0x0600CDE9 RID: 52713 RVA: 0x004AD428 File Offset: 0x004AB628
		[PublicizedFrom(EAccessModifier.Private)]
		public bool System.Collections.Generic.ICollection<T>.Remove(T item)
		{
			return this.TryRemove(item);
		}

		// Token: 0x0600CDEA RID: 52714 RVA: 0x004AD434 File Offset: 0x004AB634
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitializeFromCollection(IEnumerable<T> collection)
		{
			foreach (T t in collection)
			{
				this.AddInternal(t, this._comparer.GetHashCode(t), false);
			}
			if (this._budget == 0)
			{
				this._budget = this._tables.Buckets.Length / this._tables.Locks.Length;
			}
		}

		// Token: 0x0600CDEB RID: 52715 RVA: 0x004AD4B8 File Offset: 0x004AB6B8
		public bool TryFirst(out T returnValue)
		{
			if (this._tables == null || this._tables.Buckets == null || this._tables.Buckets.Length == 0)
			{
				returnValue = default(T);
				return false;
			}
			ConcurrentHashSet<T>.Node node = this._tables.Buckets.FirstOrDefault<ConcurrentHashSet<T>.Node>((ConcurrentHashSet<T>.Node d) => d != null);
			if (node == null)
			{
				returnValue = default(T);
				return false;
			}
			returnValue = node.Item;
			return true;
		}

		// Token: 0x0600CDEC RID: 52716 RVA: 0x004AD544 File Offset: 0x004AB744
		public bool TryRemoveFirst(out T returnValue)
		{
			if (this._tables == null || this._tables.Buckets == null || this._tables.Buckets.Length == 0)
			{
				returnValue = default(T);
				return false;
			}
			ConcurrentHashSet<T>.Node node = this._tables.Buckets.FirstOrDefault<ConcurrentHashSet<T>.Node>((ConcurrentHashSet<T>.Node d) => d != null);
			if (node == null)
			{
				returnValue = default(T);
				return false;
			}
			returnValue = node.Item;
			this.TryRemove(returnValue);
			return true;
		}

		// Token: 0x0600CDED RID: 52717 RVA: 0x004AD5DC File Offset: 0x004AB7DC
		[PublicizedFrom(EAccessModifier.Private)]
		public bool AddInternal(T item, int hashcode, bool acquireLock)
		{
			checked
			{
				ConcurrentHashSet<T>.Tables tables;
				bool flag;
				for (;;)
				{
					tables = this._tables;
					int num;
					int num2;
					ConcurrentHashSet<T>.GetBucketAndLockNo(hashcode, out num, out num2, tables.Buckets.Length, tables.Locks.Length);
					flag = false;
					bool flag2 = false;
					try
					{
						if (acquireLock)
						{
							Monitor.Enter(tables.Locks[num2], ref flag2);
						}
						if (tables != this._tables)
						{
							continue;
						}
						for (ConcurrentHashSet<T>.Node node = tables.Buckets[num]; node != null; node = node.Next)
						{
							if (hashcode == node.Hashcode && this._comparer.Equals(node.Item, item))
							{
								return false;
							}
						}
						Volatile.Write<ConcurrentHashSet<T>.Node>(ref tables.Buckets[num], new ConcurrentHashSet<T>.Node(item, hashcode, tables.Buckets[num]));
						tables.CountPerLock[num2]++;
						if (tables.CountPerLock[num2] > this._budget)
						{
							flag = true;
						}
					}
					finally
					{
						if (flag2)
						{
							Monitor.Exit(tables.Locks[num2]);
						}
					}
					break;
				}
				if (flag)
				{
					this.GrowTable(tables);
				}
				return true;
			}
		}

		// Token: 0x0600CDEE RID: 52718 RVA: 0x004AD6E8 File Offset: 0x004AB8E8
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetBucket(int hashcode, int bucketCount)
		{
			return (hashcode & int.MaxValue) % bucketCount;
		}

		// Token: 0x0600CDEF RID: 52719 RVA: 0x004AD6F3 File Offset: 0x004AB8F3
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount)
		{
			bucketNo = (hashcode & int.MaxValue) % bucketCount;
			lockNo = bucketNo % lockCount;
		}

		// Token: 0x0600CDF0 RID: 52720 RVA: 0x004AD708 File Offset: 0x004AB908
		[PublicizedFrom(EAccessModifier.Private)]
		public void GrowTable(ConcurrentHashSet<T>.Tables tables)
		{
			int num = 0;
			try
			{
				this.AcquireLocks(0, 1, ref num);
				if (tables == this._tables)
				{
					long num2 = 0L;
					for (int i = 0; i < tables.CountPerLock.Length; i++)
					{
						num2 += (long)tables.CountPerLock[i];
					}
					if (num2 < (long)(tables.Buckets.Length / 4))
					{
						this._budget = 2 * this._budget;
						if (this._budget < 0)
						{
							this._budget = int.MaxValue;
						}
					}
					else
					{
						int num3 = 0;
						bool flag = false;
						object[] array;
						checked
						{
							try
							{
								num3 = tables.Buckets.Length * 2 + 1;
								while (num3 % 3 == 0 || num3 % 5 == 0 || num3 % 7 == 0)
								{
									num3 += 2;
								}
								if (num3 > 2146435071)
								{
									flag = true;
								}
							}
							catch (OverflowException)
							{
								flag = true;
							}
							if (flag)
							{
								num3 = 2146435071;
								this._budget = int.MaxValue;
							}
							this.AcquireLocks(1, tables.Locks.Length, ref num);
							array = tables.Locks;
						}
						if (this._growLockArray && tables.Locks.Length < 1024)
						{
							array = new object[tables.Locks.Length * 2];
							Array.Copy(tables.Locks, 0, array, 0, tables.Locks.Length);
							for (int j = tables.Locks.Length; j < array.Length; j++)
							{
								array[j] = new object();
							}
						}
						ConcurrentHashSet<T>.Node[] array2 = new ConcurrentHashSet<T>.Node[num3];
						int[] array3 = new int[array.Length];
						for (int k = 0; k < tables.Buckets.Length; k++)
						{
							checked
							{
								ConcurrentHashSet<T>.Node next;
								for (ConcurrentHashSet<T>.Node node = tables.Buckets[k]; node != null; node = next)
								{
									next = node.Next;
									int num4;
									int num5;
									ConcurrentHashSet<T>.GetBucketAndLockNo(node.Hashcode, out num4, out num5, array2.Length, array.Length);
									array2[num4] = new ConcurrentHashSet<T>.Node(node.Item, node.Hashcode, array2[num4]);
									array3[num5]++;
								}
							}
						}
						this._budget = Math.Max(1, array2.Length / array.Length);
						this._tables = new ConcurrentHashSet<T>.Tables(array2, array, array3);
					}
				}
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		// Token: 0x0600CDF1 RID: 52721 RVA: 0x004AD948 File Offset: 0x004ABB48
		[PublicizedFrom(EAccessModifier.Private)]
		public void AcquireAllLocks(ref int locksAcquired)
		{
			this.AcquireLocks(0, 1, ref locksAcquired);
			this.AcquireLocks(1, this._tables.Locks.Length, ref locksAcquired);
		}

		// Token: 0x0600CDF2 RID: 52722 RVA: 0x004AD96C File Offset: 0x004ABB6C
		[PublicizedFrom(EAccessModifier.Private)]
		public void AcquireLocks(int fromInclusive, int toExclusive, ref int locksAcquired)
		{
			object[] locks = this._tables.Locks;
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				bool flag = false;
				try
				{
					Monitor.Enter(locks[i], ref flag);
				}
				finally
				{
					if (flag)
					{
						locksAcquired++;
					}
				}
			}
		}

		// Token: 0x0600CDF3 RID: 52723 RVA: 0x004AD9BC File Offset: 0x004ABBBC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ReleaseLocks(int fromInclusive, int toExclusive)
		{
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				Monitor.Exit(this._tables.Locks[i]);
			}
		}

		// Token: 0x0600CDF4 RID: 52724 RVA: 0x004AD9EC File Offset: 0x004ABBEC
		[PublicizedFrom(EAccessModifier.Private)]
		public void CopyToItems(T[] array, int index)
		{
			foreach (ConcurrentHashSet<T>.Node node in this._tables.Buckets)
			{
				while (node != null)
				{
					array[index] = node.Item;
					index++;
					node = node.Next;
				}
			}
		}

		// Token: 0x04009BF5 RID: 39925
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultCapacity = 31;

		// Token: 0x04009BF6 RID: 39926
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxLockNumber = 1024;

		// Token: 0x04009BF7 RID: 39927
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEqualityComparer<T> _comparer;

		// Token: 0x04009BF8 RID: 39928
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool _growLockArray;

		// Token: 0x04009BF9 RID: 39929
		[PublicizedFrom(EAccessModifier.Private)]
		public int _budget;

		// Token: 0x04009BFA RID: 39930
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile ConcurrentHashSet<T>.Tables _tables;

		// Token: 0x04009BFB RID: 39931
		public Action<T> OnRemovalFailure;

		// Token: 0x02001AA8 RID: 6824
		[PublicizedFrom(EAccessModifier.Private)]
		public class Tables
		{
			// Token: 0x0600CDF5 RID: 52725 RVA: 0x004ADA39 File Offset: 0x004ABC39
			public Tables(ConcurrentHashSet<T>.Node[] buckets, object[] locks, int[] countPerLock)
			{
				this.Buckets = buckets;
				this.Locks = locks;
				this.CountPerLock = countPerLock;
			}

			// Token: 0x04009BFC RID: 39932
			public readonly ConcurrentHashSet<T>.Node[] Buckets;

			// Token: 0x04009BFD RID: 39933
			public readonly object[] Locks;

			// Token: 0x04009BFE RID: 39934
			public volatile int[] CountPerLock;
		}

		// Token: 0x02001AA9 RID: 6825
		[PublicizedFrom(EAccessModifier.Private)]
		public class Node
		{
			// Token: 0x0600CDF6 RID: 52726 RVA: 0x004ADA58 File Offset: 0x004ABC58
			public Node(T item, int hashcode, ConcurrentHashSet<T>.Node next)
			{
				this.Item = item;
				this.Hashcode = hashcode;
				this.Next = next;
			}

			// Token: 0x04009BFF RID: 39935
			public readonly T Item;

			// Token: 0x04009C00 RID: 39936
			public readonly int Hashcode;

			// Token: 0x04009C01 RID: 39937
			public volatile ConcurrentHashSet<T>.Node Next;
		}
	}
}
