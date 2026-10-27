using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConcurrentCollections
{
	// Token: 0x02001B04 RID: 6916
	[DebuggerDisplay("Count = {Count}")]
	public class ConcurrentHashSet<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable, ICollection<T>
	{
		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x0600D040 RID: 53312 RVA: 0x004B4EE3 File Offset: 0x004B30E3
		public static int DefaultConcurrencyLevel
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return Math.Max(2, Environment.ProcessorCount);
			}
		}

		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x0600D041 RID: 53313 RVA: 0x004B4EF0 File Offset: 0x004B30F0
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

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x0600D042 RID: 53314 RVA: 0x004B4F58 File Offset: 0x004B3158
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

		// Token: 0x0600D043 RID: 53315 RVA: 0x004B4FC0 File Offset: 0x004B31C0
		public ConcurrentHashSet()
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, null)
		{
		}

		// Token: 0x0600D044 RID: 53316 RVA: 0x004B4FD1 File Offset: 0x004B31D1
		public ConcurrentHashSet(Action<T> onRemovalFaiuire)
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, null)
		{
			this.OnRemovalFailure = onRemovalFaiuire;
		}

		// Token: 0x0600D045 RID: 53317 RVA: 0x004B4FE9 File Offset: 0x004B31E9
		public ConcurrentHashSet(int concurrencyLevel, int capacity)
			: this(concurrencyLevel, capacity, false, null)
		{
		}

		// Token: 0x0600D046 RID: 53318 RVA: 0x004B4FF5 File Offset: 0x004B31F5
		public ConcurrentHashSet(IEnumerable<T> collection)
			: this(collection, null)
		{
		}

		// Token: 0x0600D047 RID: 53319 RVA: 0x004B4FFF File Offset: 0x004B31FF
		public ConcurrentHashSet(IEqualityComparer<T> comparer)
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, comparer)
		{
		}

		// Token: 0x0600D048 RID: 53320 RVA: 0x004B5010 File Offset: 0x004B3210
		public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
			: this(comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x0600D049 RID: 53321 RVA: 0x004B502E File Offset: 0x004B322E
		public ConcurrentHashSet(int concurrencyLevel, IEnumerable<T> collection, IEqualityComparer<T> comparer)
			: this(concurrencyLevel, 31, false, comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x0600D04A RID: 53322 RVA: 0x004B5050 File Offset: 0x004B3250
		public ConcurrentHashSet(int concurrencyLevel, int capacity, IEqualityComparer<T> comparer)
			: this(concurrencyLevel, capacity, false, comparer)
		{
		}

		// Token: 0x0600D04B RID: 53323 RVA: 0x004B505C File Offset: 0x004B325C
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

		// Token: 0x0600D04C RID: 53324 RVA: 0x004B50F6 File Offset: 0x004B32F6
		public bool Add(T item)
		{
			return this.AddInternal(item, this._comparer.GetHashCode(item), true);
		}

		// Token: 0x0600D04D RID: 53325 RVA: 0x004B510C File Offset: 0x004B330C
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

		// Token: 0x0600D04E RID: 53326 RVA: 0x004B5194 File Offset: 0x004B3394
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

		// Token: 0x0600D04F RID: 53327 RVA: 0x004B5208 File Offset: 0x004B3408
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

		// Token: 0x0600D050 RID: 53328 RVA: 0x004B5328 File Offset: 0x004B3528
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600D051 RID: 53329 RVA: 0x004B5330 File Offset: 0x004B3530
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

		// Token: 0x0600D052 RID: 53330 RVA: 0x004B533F File Offset: 0x004B353F
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<T>.Add(T item)
		{
			this.Add(item);
		}

		// Token: 0x17001A4C RID: 6732
		// (get) Token: 0x0600D053 RID: 53331 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool System.Collections.Generic.ICollection<T>.IsReadOnly
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600D054 RID: 53332 RVA: 0x004B534C File Offset: 0x004B354C
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

		// Token: 0x0600D055 RID: 53333 RVA: 0x004B53F0 File Offset: 0x004B35F0
		[PublicizedFrom(EAccessModifier.Private)]
		public bool System.Collections.Generic.ICollection<T>.Remove(T item)
		{
			return this.TryRemove(item);
		}

		// Token: 0x0600D056 RID: 53334 RVA: 0x004B53FC File Offset: 0x004B35FC
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

		// Token: 0x0600D057 RID: 53335 RVA: 0x004B5480 File Offset: 0x004B3680
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

		// Token: 0x0600D058 RID: 53336 RVA: 0x004B550C File Offset: 0x004B370C
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

		// Token: 0x0600D059 RID: 53337 RVA: 0x004B55A4 File Offset: 0x004B37A4
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

		// Token: 0x0600D05A RID: 53338 RVA: 0x004B56B0 File Offset: 0x004B38B0
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetBucket(int hashcode, int bucketCount)
		{
			return (hashcode & int.MaxValue) % bucketCount;
		}

		// Token: 0x0600D05B RID: 53339 RVA: 0x004B56BB File Offset: 0x004B38BB
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount)
		{
			bucketNo = (hashcode & int.MaxValue) % bucketCount;
			lockNo = bucketNo % lockCount;
		}

		// Token: 0x0600D05C RID: 53340 RVA: 0x004B56D0 File Offset: 0x004B38D0
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

		// Token: 0x0600D05D RID: 53341 RVA: 0x004B5910 File Offset: 0x004B3B10
		[PublicizedFrom(EAccessModifier.Private)]
		public void AcquireAllLocks(ref int locksAcquired)
		{
			this.AcquireLocks(0, 1, ref locksAcquired);
			this.AcquireLocks(1, this._tables.Locks.Length, ref locksAcquired);
		}

		// Token: 0x0600D05E RID: 53342 RVA: 0x004B5934 File Offset: 0x004B3B34
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

		// Token: 0x0600D05F RID: 53343 RVA: 0x004B5984 File Offset: 0x004B3B84
		[PublicizedFrom(EAccessModifier.Private)]
		public void ReleaseLocks(int fromInclusive, int toExclusive)
		{
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				Monitor.Exit(this._tables.Locks[i]);
			}
		}

		// Token: 0x0600D060 RID: 53344 RVA: 0x004B59B4 File Offset: 0x004B3BB4
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

		// Token: 0x04009DA4 RID: 40356
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultCapacity = 31;

		// Token: 0x04009DA5 RID: 40357
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxLockNumber = 1024;

		// Token: 0x04009DA6 RID: 40358
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEqualityComparer<T> _comparer;

		// Token: 0x04009DA7 RID: 40359
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool _growLockArray;

		// Token: 0x04009DA8 RID: 40360
		[PublicizedFrom(EAccessModifier.Private)]
		public int _budget;

		// Token: 0x04009DA9 RID: 40361
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile ConcurrentHashSet<T>.Tables _tables;

		// Token: 0x04009DAA RID: 40362
		public Action<T> OnRemovalFailure;

		// Token: 0x02001B05 RID: 6917
		[PublicizedFrom(EAccessModifier.Private)]
		public class Tables
		{
			// Token: 0x0600D061 RID: 53345 RVA: 0x004B5A01 File Offset: 0x004B3C01
			public Tables(ConcurrentHashSet<T>.Node[] buckets, object[] locks, int[] countPerLock)
			{
				this.Buckets = buckets;
				this.Locks = locks;
				this.CountPerLock = countPerLock;
			}

			// Token: 0x04009DAB RID: 40363
			public readonly ConcurrentHashSet<T>.Node[] Buckets;

			// Token: 0x04009DAC RID: 40364
			public readonly object[] Locks;

			// Token: 0x04009DAD RID: 40365
			public volatile int[] CountPerLock;
		}

		// Token: 0x02001B06 RID: 6918
		[PublicizedFrom(EAccessModifier.Private)]
		public class Node
		{
			// Token: 0x0600D062 RID: 53346 RVA: 0x004B5A20 File Offset: 0x004B3C20
			public Node(T item, int hashcode, ConcurrentHashSet<T>.Node next)
			{
				this.Item = item;
				this.Hashcode = hashcode;
				this.Next = next;
			}

			// Token: 0x04009DAE RID: 40366
			public readonly T Item;

			// Token: 0x04009DAF RID: 40367
			public readonly int Hashcode;

			// Token: 0x04009DB0 RID: 40368
			public volatile ConcurrentHashSet<T>.Node Next;
		}
	}
}
